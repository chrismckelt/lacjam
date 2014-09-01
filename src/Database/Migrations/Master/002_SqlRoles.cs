using System.Configuration;
using System.IO;
using System.Reflection;

namespace Lacjam.Database.Migrations.Master
{
    //[Migration(2, TransactionBehavior.None)]
    public static class SqlRoles
    {
        public static string Sql
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                const string resourceName = "Lacjam.Database.Migrations.Scripts.002_SqlRoles.sql";

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (var reader = new StreamReader(stream))
                {
                    string user = ConfigurationManager.AppSettings.Get("DatabaseUsername") ?? "MetastoreUser";
                    string psw = ConfigurationManager.AppSettings.Get("DatabasePassword") ?? "MetastorePassword999";

                    string sql = reader.ReadToEnd();
                    sql = sql.Replace("{DatabaseUsername}", user);
                    sql = sql.Replace("{DatabasePassword}", psw);
                    return sql;
                    //  Execute.Script(sql);
                }
            }
        }
    }
}