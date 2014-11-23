using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Snowball;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Search.Highlight;
using Lucene.Net.Store;
using Lacjam.Core.Domain.MetadataDefinitions;
using Lacjam.Core.Infrastructure;
using Lacjam.Framework.Extensions;
using Version = Lucene.Net.Util.Version;

namespace Lacjam.Core.Services
{
    public class MetadataDefinitionIndexer : IMetadataDefinitionIndexer
    {
        private static readonly FSDirectory IndexDir = FSDirectory.Open(Constant.PATH_INDEX_BASE_DIR + "/Definitions");

        private const string FIELD_ID = "identity";
        private const string FIELD_NAME = "name";
        private const string FIELD_DESCRIPTION = "description";
        private const string FIELD_DATA_TYPE = "data_type";

        private readonly QueryParser _queryParser;
        private readonly Analyzer _analyzer;

        public MetadataDefinitionIndexer()
        {
            _analyzer = new SnowballAnalyzer(Version.LUCENE_30, "English", new HashSet<string>());
            _queryParser = new MultiFieldQueryParser(Version.LUCENE_30, new[] { FIELD_NAME, FIELD_DESCRIPTION, FIELD_DATA_TYPE }, _analyzer)
            {
                DefaultOperator = QueryParser.Operator.AND,
                AllowLeadingWildcard = true
            };
            InitIndex();
        }

        private IndexWriter OpenWriter()
        {
            return new IndexWriter(IndexDir, _analyzer, false, IndexWriter.MaxFieldLength.UNLIMITED);
        }

        private void InitIndex()
        {
            if (!IndexDir.Directory.Exists)
            {
                using (new IndexWriter(IndexDir, _analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED));
            }
        }


        public void SaveIndex(MetadataDefinitionProjection definition)
        {
            var doc = ToDocument(definition);
            var term = new Term(FIELD_ID, definition.Identity.ToString());

            using (var writer = OpenWriter())
            {
                writer.UpdateDocument(term, doc);
                writer.Optimize();
                writer.Commit();
            }
        }

        public MetadataDefinitionSearchResults Search(string q, int pageSize, int page)
        {
            using (var searcher = new IndexSearcher(IndexDir, true))
            {
                Query query;
                Highlighter highlighter = null;
                if (q == null)
                {
                    query = new MatchAllDocsQuery();
                }
                else
                {
                    query = _queryParser.Parse(q);
                    
                    if (q.Any() && !StringExtension.EndsWithWhitespace(q) &&
                        !new[] {'"', '*', '~', '?'}.Contains(q.Last()))
                    {
                        query = new BooleanQuery
                        {
                            {_queryParser.Parse(q + "*"), Occur.SHOULD},
                            {query, Occur.SHOULD}
                        };
                    }
                        
                    highlighter = new Highlighter(new SimpleHTMLFormatter("<span class=\"is-highlighted\">", "</span>"), new QueryScorer(query));
                }

                var take = page * pageSize;
                var sort = new Sort(new SortField(FIELD_NAME, CultureInfo.CurrentCulture));
                var docs = searcher.Search(query, null, take, sort);

                var results = new MetadataDefinitionSearchResults
                {
                    TotalHits = docs.TotalHits,
                    Hits = docs.ScoreDocs.Skip(take - pageSize).Select(x => ScoreDocToSearchHit(searcher, highlighter, x)).ToArray()
                };

                return results;
            }
        }

        public void DeleteIndex(Guid id)
        {
            var term = new Term(FIELD_ID, id.ToString());

            using (var writer = OpenWriter())
            {
                writer.DeleteDocuments(term);
                writer.Optimize();
                writer.Commit();
            }
        }

        private MetadataDefinitionSearchResults.Hit ScoreDocToSearchHit(IndexSearcher searcher, Highlighter highlighter, ScoreDoc scoreDoc)
        {
            var doc = searcher.Doc(scoreDoc.Doc);
            return new MetadataDefinitionSearchResults.Hit
            {
                Id = Guid.Parse(doc.Get(FIELD_ID)),
                Name = Highlight(highlighter, doc.Get(FIELD_NAME)),
                Description = Highlight(highlighter, doc.Get(FIELD_DESCRIPTION)),
                DataType = Highlight(highlighter, doc.Get(FIELD_DATA_TYPE))
            };
        }

        private string Highlight(Highlighter highlighter, string description)
        {
            description = SimpleHTMLEncoder.HtmlEncode(description);
            if (highlighter != null)
            {
                var stream = _analyzer.TokenStream("", new StringReader(description));
                var sample = highlighter.GetBestFragments(stream, description, 2, "...");
                if (!string.IsNullOrEmpty(sample))
                {
                    return sample;
                }
            }
            return description;
        }

        private Document ToDocument(MetadataDefinitionProjection definition)
        {
            var doc = new Document();
            foreach (var field in ToFields(definition))
                doc.Add(field);
            return doc;
        }

        private static IEnumerable<IFieldable> ToFields(MetadataDefinitionProjection definition)
        {
            yield return new Field(FIELD_ID, definition.Identity.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED);
            yield return new Field(FIELD_NAME, definition.Name, Field.Store.YES, Field.Index.ANALYZED);
            yield return new Field(FIELD_DESCRIPTION, definition.Description, Field.Store.YES, Field.Index.ANALYZED);
            yield return new Field(FIELD_DATA_TYPE, definition.DataType, Field.Store.YES, Field.Index.NOT_ANALYZED);
        }
    }
}