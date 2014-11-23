using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Snowball;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Search.Highlight;
using Lucene.Net.Search.Spans;
using Lucene.Net.Store;
using Lacjam.Core.Domain.Entities;
using Lacjam.Core.Domain.MetadataDefinitionGroups;
using Lacjam.Core.Domain.MetadataDefinitions;
using Lacjam.Core.Infrastructure;
using Lacjam.Framework.Extensions;
using Version = Lucene.Net.Util.Version;

namespace Lacjam.Core.Services
{
    public class EntityIndexer: IEntityIndexer
    {
        private static readonly FSDirectory IndexDir = FSDirectory.Open(Constant.PATH_INDEX_BASE_DIR + "/Entities");
        
        private const string FIELD_ID = "identity";
        private const string FIELD_NAME = "name";
        private const string FIELD_NAME_RAW = "name_raw";
        private const string FIELD_GROUP_ID = "groupId";
        private const string FIELD_GROUP_NAME = "groupName";
        private const string FIELD_METADATA = "metadata";
       
        private readonly QueryParser _keywordQueryParser;
        private readonly MultiFieldQueryParser _allMetadataQueryParser;
        private readonly Analyzer _analyzer;

        public EntityIndexer()
        {
            _analyzer = new SnowballAnalyzer(Version.LUCENE_30, "English", new HashSet<string>());
            var keywordFields = new[] { FIELD_NAME, FIELD_GROUP_NAME }.Union(MetadataDefinition.KeywordDefinitions).ToArray();
            _keywordQueryParser = new MultiFieldQueryParser(Version.LUCENE_30, keywordFields, _analyzer)
            {
                DefaultOperator = QueryParser.Operator.AND,
                AllowLeadingWildcard = true,
            };

            _allMetadataQueryParser = new MultiFieldQueryParser(Version.LUCENE_30, new[] { FIELD_NAME, FIELD_GROUP_NAME, FIELD_METADATA }, _analyzer)
            {
                DefaultOperator = QueryParser.Operator.AND,
                AllowLeadingWildcard = true
            };

            InitIndex();
        }

        private IndexWriter OpenWriter()
        {
            var create = !IndexDir.Directory.Exists;
            return new IndexWriter(IndexDir, _analyzer, create, IndexWriter.MaxFieldLength.UNLIMITED);
        }

        private void InitIndex()
        {
            if (!IndexDir.Directory.Exists)
            {
                using (new IndexWriter(IndexDir, _analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED)) ;
            }
        }

        public void SaveIndex(EntityProjection entity, MetadataDefinitionGroupProjection group, IEnumerable<EntityValueProjection> values)
        {
            var doc = ToDocument(entity, group, values);
            var term = new Term(FIELD_ID, entity.Identity.ToString());

            using (var writer = OpenWriter())
            {
                writer.UpdateDocument(term, doc);
                writer.Commit();
                writer.Optimize();
            }
        }

        public EntitySearchResults SearchKeywords(string q, int pageSize, int page)
        {
            return Search(q, pageSize, page, _keywordQueryParser);
        }

        public EntitySearchResults SearchAllMetadata(string q, int pageSize, int page)
        {
            return Search(q, pageSize, page, _allMetadataQueryParser);
        }

        public void RenameGroup(MetadataDefinitionGroupProjection @group)
        {
            var term = new Term(FIELD_GROUP_ID, @group.Identity.ToString());
            using (var writer = OpenWriter())
            using (var searcher = new IndexSearcher(IndexDir, true))
            {
                while(true)
                {
                    const int BATCH = 100;
                    var result = searcher.Search(new TermQuery(term), BATCH);
                    foreach (var scoreDoc in result.ScoreDocs)
                    {
                        var doc = searcher.Doc(scoreDoc.Doc);
                        doc.GetField(FIELD_GROUP_NAME).SetValue(group.Name);
                        writer.UpdateDocument(new Term(FIELD_ID, doc.Get(FIELD_ID)), doc);
                    }

                    if (result.TotalHits <= BATCH)
                        break;
                }

                writer.Commit();
                writer.Optimize();
            }
        }

        public void DeleteIndex(Guid id)
        {
            var term = new Term(FIELD_ID, id.ToString());

            using (var writer = OpenWriter())
            {
                writer.DeleteDocuments(term);
                writer.Commit();
                writer.Optimize();
            }
        }

        private EntitySearchResults Search(string q, int pageSize, int page, QueryParser queryParser)
        {
            using (var searcher = new IndexSearcher(IndexDir, true))
            {
                Query query;
                Highlighter highlighter=null;
                if (q == null)
                {
                    query = new MatchAllDocsQuery();
                }
                else
                {
                    query = queryParser.Parse(q);
                    
                    if (q.Any() && !StringExtension.EndsWithWhitespace(q) &&
                        !new[] {'"', '*', '~', '?'}.Contains(q.Last()))
                    {
                        query = new BooleanQuery
                        {
                            {queryParser.Parse(q + "*"), Occur.SHOULD},
                            {query, Occur.SHOULD}
                        };
                    }
                    highlighter = new Highlighter(new SimpleHTMLFormatter("<span class=\"is-highlighted\">", "</span>"), new QueryScorer(query));
                }

                var take = page * pageSize;
                var sort = new Sort(new SortField(FIELD_NAME, SortField.STRING));
                var docs = searcher.Search(query, null, take, sort);
                
                var results = new EntitySearchResults
                {
                    TotalHits = docs.TotalHits,
                    Hits = docs.ScoreDocs.Skip(take - pageSize).AsParallel().AsOrdered().Select(x => ScoreDocToSearchHit(searcher, highlighter, x)).ToArray()
                };

                return results;
            }
        }

        public IEnumerable<EntitySearchResults.Hit> GetByNames(string[] names)
        {
            using (var searcher = new IndexSearcher(IndexDir, true))
            {
                var query = new BooleanQuery();
                foreach (var name in names)
                {
                    query.Add(new SpanTermQuery(new Term(FIELD_NAME_RAW, name)), Occur.SHOULD);
                }

                var docs = searcher.Search(query, names.Length*2 /* double it just in case */);
                return docs.ScoreDocs.Select(x => ScoreDocToSearchHit(searcher, null, x)).ToArray();
            }
        }

        private EntitySearchResults.Hit ScoreDocToSearchHit(IndexSearcher searcher, Highlighter highlighter, ScoreDoc scoreDoc)
        {
            var hit = new EntitySearchResults.Hit();
            var doc = searcher.Doc(scoreDoc.Doc);
            hit.Id = Guid.Parse(doc.Get(FIELD_ID));
            hit.Text = Highlight(highlighter, doc.Get(FIELD_NAME));
            hit.Group = Highlight(highlighter, doc.Get(FIELD_GROUP_NAME));

            hit.Description = Highlight(highlighter, string.Join("\n", doc.GetFields(MetadataDefinition.DescriptionDefinition).Select(x => x.StringValue)));
            
            hit.Tags = doc.GetFields()
                    .Where(x => MetadataDefinition.KeywordDefinitions.Except(new[]{MetadataDefinition.DescriptionDefinition}).Contains(x.Name))
                    .Select(x => Highlight(highlighter, x.StringValue))
                    .ToArray();

            return hit;
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

        private Document ToDocument(EntityProjection entity, MetadataDefinitionGroupProjection group, IEnumerable<EntityValueProjection> values)
        {
            var doc = new Document();
            foreach (var field in ToFields(entity, group, values))
                doc.Add(field);
            return doc;
        }

        public IEnumerable<Field> ToFields(EntityProjection entity, MetadataDefinitionGroupProjection group, IEnumerable<EntityValueProjection> values)
        {
            yield return new Field(FIELD_ID, entity.Identity.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED);
            yield return new Field(FIELD_NAME, entity.Name, Field.Store.YES, Field.Index.ANALYZED);
            yield return new Field(FIELD_NAME_RAW, entity.Name, Field.Store.NO, Field.Index.NOT_ANALYZED);
            if (group!= null)
            {
                yield return new Field(FIELD_GROUP_ID, group.Identity.ToString(), Field.Store.YES, Field.Index.ANALYZED);
                yield return new Field(FIELD_GROUP_NAME, group.Name, Field.Store.YES, Field.Index.ANALYZED);        
            }
            
            foreach (var value in values)
            {
                foreach (var keyword in MetadataDefinition.KeywordDefinitions.Where(x => x.Equals(value.Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    yield return new Field(keyword, value.Value, Field.Store.YES, Field.Index.ANALYZED);    
                }

                yield return new Field(FIELD_METADATA, value.Value, Field.Store.YES, Field.Index.ANALYZED);
            }
        }
    }
}