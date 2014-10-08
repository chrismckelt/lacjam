using System;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using NUnit.Framework;
using Structerre.MetaStore.Core.Domain.Entities;
using Structerre.MetaStore.Core.Domain.MetadataDefinitionGroups;
using Structerre.MetaStore.Core.Services;
using Version = Lucene.Net.Util.Version;

namespace Core.UnitTests.Csharp
{
    public class IndexerSpike
    {
        [Test]
        public void Test()
        {
            var indexer = new EntityIndexer();
            var group = new MetadataDefinitionGroupProjection{Identity = Guid.NewGuid(), Name="xxx"};

            indexer.SaveIndex(new EntityProjection(Guid.NewGuid(), Guid.Empty, "sheep"),
                group, 
                new EntityValueProjection[0]);
            indexer.SaveIndex(new EntityProjection(Guid.NewGuid(), Guid.Empty, "shop"),
                group,
                new EntityValueProjection[0]);

            var result = indexer.SearchKeywords("shop", 10, 1);

            Assert.Greater(result.TotalHits, 0);
        }

        [Test]
        public void Blah()
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            var dir = FSDirectory.Open("c:/data/LuceneIndex/Entities");

            using (var writer = new IndexWriter(dir, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                var doc = new Document();
                doc.Add(new Field("title", "sheep", Field.Store.YES, Field.Index.ANALYZED));

                writer.AddDocument(doc);
                writer.Commit();
            }

            using (var searcher = new IndexSearcher(dir, true))
            {
                var queryParser = new QueryParser(Version.LUCENE_30, "title", analyzer);
                var query = queryParser.Parse("sheep");

                var result = searcher.Search(query, 10);
                Assert.Greater(result.TotalHits, 0);

            }
        }
    }
}