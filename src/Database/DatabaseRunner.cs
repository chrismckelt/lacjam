using System;
using System.Data.SqlClient;
using System.Reflection;
using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using Lacjam.Database.Migrations.Master;

namespace Lacjam.Database
{
    public static class DatabaseRunner
    {
        public class MigrationOptions : IMigrationProcessorOptions
        {
            public bool PreviewOnly { get; set; }
            public string ProviderSwitches { get; set; }
            public int Timeout { get; set; }
        }

        public static void MigrateToLatest(string connectionString)
        {
            if (!IsConnectionStringValid(connectionString)) return;
            var announcer = new TextWriterAnnouncer(Console.WriteLine);
            var assembly = Assembly.GetExecutingAssembly();

            var migrationContext = new RunnerContext(announcer)
            {
                Connection = connectionString,
                TransactionPerSession = true
            };

            var options = new MigrationOptions { PreviewOnly = false, Timeout = 60 };
            var factory = new FluentMigrator.Runner.Processors.SqlServer.SqlServer2012ProcessorFactory();
            var processor = factory.Create(connectionString, announcer, options);
            var runner = new MigrationRunner(assembly, migrationContext, processor);
            runner.MigrateUp(false);
        }

        public static void CreateDatabase(string connectionString)
        {
            if (!connectionString.Contains("Master"))
                connectionString = connectionString.Replace("Lacjam", "Master");

            ExecuteSql(connectionString,InitialSetup.Sql);
            ExecuteSql(connectionString,SqlRoles.Sql);
           
        }


        public static void DeleteAllTables(string connectionString)
        {
            if (!IsConnectionStringValid(connectionString)) return;

            const string sql = @"DECLARE @Sql NVARCHAR(500) DECLARE @Cursor CURSOR SET @Cursor = CURSOR FAST_FORWARD FOR SELECT DISTINCT sql = 'ALTER TABLE [' + tc2.TABLE_NAME + '] DROP [' + rc1.CONSTRAINT_NAME + ']' FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc1 LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc2 ON tc2.CONSTRAINT_NAME =rc1.CONSTRAINT_NAME OPEN @Cursor FETCH NEXT FROM @Cursor INTO @Sql WHILE (@@FETCH_STATUS = 0) BEGIN Exec SP_EXECUTESQL @Sql FETCH NEXT FROM @Cursor INTO @Sql END CLOSE @Cursor DEALLOCATE @Cursor EXEC sp_MSForEachTable 'DROP TABLE ?' ";
            ExecuteSql(connectionString,sql);
        }

        public static bool IsConnectionStringValid(string connectionString)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select getdate()";
                        cmd.ExecuteNonQuery();
                    }

                    conn.Close();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                
            }
        }

        private static void ExecuteSql(string connectionstring, string sql)
        {
            using (var conn = new SqlConnection(connectionstring))
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
