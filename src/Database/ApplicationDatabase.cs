using System.Data.SqlClient;
using System.IO;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Lacjam.Core.Infrastructure.Database;
using Lacjam.Core.Infrastructure.Ioc;

namespace Lacjam.Database
{
    public class ApplicationDatabase
    {
        public string ConnectionString { get; set; }
        public FluentConfiguration Fluent { get; private set; }
        private Configuration _configuration;
        const string _outputpath = @"c:\temp\Lacjam.schema";

        public ApplicationDatabase()
        {
          
        }

        public ApplicationDatabase WithConnectionString(string connectionstring)
        {
            ConnectionString = connectionstring;
   
            return this;
        }

        public string CreateDatabaseScript()
        {
            Setup();
            using (var file = new StreamWriter(_outputpath))
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    var export = new SchemaExport(_configuration);
                    export.Execute(true, true, false, conn, file);
                }
                
            }
            return File.ReadAllText(_outputpath);
        }

        public void DropDatabase()
        {
            Setup();
            var session = _configuration.BuildSessionFactory().OpenSession();
            var export = new SchemaExport(_configuration);
            export.Execute(true, false, true, session.Connection, null);
            export.Drop(false, true);
        }

        private void Setup()
        {
            if (string.IsNullOrEmpty(this.ConnectionString))
                this.ConnectionString = WindsorAccessor.Instance.WithSessionManagement().Container.Resolve<ISession>().Connection.ConnectionString;
           
        }

    }
}