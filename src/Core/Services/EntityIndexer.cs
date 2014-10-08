using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
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
        private const string FIELD_GROUP_ID = "groupId";
        private const string FIELD_GROUP_NAME = "groupName";
        private const string FIELD_METADATA = "metadata";
       
        private readonly QueryParser _keywordQueryParser;
        private readonly MultiFieldQueryParser _allMetadataQueryParser;
        private StandardAnalyzer _analyzer;

        public EntityIndexer()
        {
            _analyzer = new StandardAnalyzer(Version.LUCENE_30);
            var keywordFields = new[]{FIELD_NAME}.Union(MetadataDefinition.KeywordDefinitions).ToArray();
            _keywordQueryParser = new MultiFieldQueryParser(Version.LUCENE_30, keywordFields, _analyzer)
            {
                DefaultOperator = QueryParser.Operator.AND,
                AllowLeadingWildcard = true
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
                if (q == null)
                {
                    query = new MatchAllDocsQuery();
                }
                else
                {
                    if (q.Any() && !StringExtension.EndsWithWhitespace(q))
                        q += "*";
                    query = queryParser.Parse(q);
                }

                var take = page * pageSize;
                var docs = searcher.Search(query, take);

                var results = new EntitySearchResults
                {
                    TotalHits = docs.TotalHits,
                    Hits = docs.ScoreDocs.Skip(take - pageSize).Select(x => ScoreDocToSearchHit(searcher, x)).ToArray()
                };

                return results;
            }
        }

        private EntitySearchResults.Hit ScoreDocToSearchHit(IndexSearcher searcher, ScoreDoc scoreDoc)
        {
            var hit = new EntitySearchResults.Hit();
            var doc = searcher.Doc(scoreDoc.Doc);
            hit.Id = Guid.Parse(doc.Get(FIELD_ID));
            hit.Text = doc.Get(FIELD_NAME);
            hit.Group = doc.Get(FIELD_GROUP_NAME);
            hit.Metadata = doc.GetFields().Where(x => MetadataDefinition.KeywordDefinitions.Contains(x.Name))
                .GroupBy(x => x.Name, x => x.StringValue)
                .ToDictionary(x => x.Key, x => x.ToArray());

            return hit;
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

                yield return new Field(FIELD_METADATA, value.Name, Field.Store.YES, Field.Index.ANALYZED);
            }
        }
    }
}