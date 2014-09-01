using System.Configuration;
using System.IO;
using System.Reflection;
using FluentMigrator;

namespace Lacjam.Database.Migrations.Master
{
    /// <summary>
    /// migrate --dbType=SqlServer2012 --conn="Data Source=localhost\SQL2012;Database=Master; Integrated Security=SSPI;" --assembly=C:\dev\Lacjam\src\Database\bin\Debug\Lacjam.Database.dll 
    /// </summary>
    //[Migration(1, TransactionBehavior.None)]
    public static class InitialSetup //: Migration
    {
        public static string Sql
        {
            get
            {
                var assembly = Assembly.GetExecutingAssembly();
                const string resourceName = "Lacjam.Database.Migrations.Scripts.001_InitialSetup.sql";

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (var reader = new StreamReader(stream))
                {
                    string user = ConfigurationManager.AppSettings.Get("DatabaseUsername") ?? "MetastoreUser";
                    string psw = ConfigurationManager.AppSettings.Get("DatabasePassword") ?? "MetastorePassword999";

                    string sql = reader.ReadToEnd();
                    sql = sql.Replace("{DatabaseUsername}", user);
                    sql = sql.Replace("{DatabasePassword}", psw);

                    return sql;
                }
            }
        }

    }
}