using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using NHibernate.Cfg;
using NHibernate.Util;
using Structerre.MetaStore.Framework.Logging;

namespace Structerre.MetaStore.Core.Infrastructure.Database
{
    public class FluentConfigurationProvider : AbstractConfigurationProvider
    {
        
        private ILogWriter _logWriter;
        private readonly string _connectionString;
        private readonly Assembly _assembly;
        private static Configuration _configuration;

        public FluentConfigurationProvider()
            : this()
        {
        }


        public FluentConfigurationProvider(string connectionString)
        {
            _connectionString = connectionString;
            _assembly = null;
        }

        public FluentConfigurationProvider(string connectionString, Assembly assembly)
        {
     
            _connectionString = connectionString;
            _assembly = assembly;
        }

        private void InitializeNHibernate()
        {
            if (_logWriter == null) _logWriter = new LogWriter();

            try
            {
                if (string.IsNullOrWhiteSpace(_connectionString))
                {
                    var dir = AppDomain.CurrentDomain.BaseDirectory.ToLowerInvariant();
                    var configPath = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        dir.EndsWith("wcfbackend\\") || dir.EndsWith("quotefacade\\") ? "bin\\" : "",
                        "NHibernate.config");

                    if (!File.Exists(configPath))
                    {
                        _logWriter.Error(EventIds.BACKEND_SERVER_DOWN, "Phoenix:: Cannot find NHibernate.config file ");
                        throw new ApplicationException(configPath + " does not exist");
                    }

                    _logWriter.Debug(EventIds.DEBUG_INFORMATION, "Phoenix:: found NH config file " + configPath);

                    _configuration = CreateConfiguration().Configure(configPath);
                }
                else
                {
                    var props = new Dictionary<string, string>
                                    {
                                        {"dialect", "NHibernate.Dialect.MsSql2005Dialect"},
                                        {"connection.driver_class", "NHibernate.Driver.SqlClientDriver"},
                                        {"connection.provider", "NHibernate.Connection.DriverConnectionProvider"},
                                        {"connection.release_mode", "auto"},
                                        {"adonet.batch_size", "500"},
                                        {"current_session_context_class", "thread_static"},
                                        {"show_sql", "true"},
                                        {"prepare_sql", "true"},
                                        {"connection.connection_string", _connectionString.Trim()}
                                    };

                    _configuration = CreateConfiguration().AddProperties(props);
                }

                var generator = _assembly == null
                                    ? new AutoPersistenceModelGenerator()
                                    : new AutoPersistenceModelGenerator(_assembly);

                CreateFluentConfiguration(generator.Generate());
            }
            catch (ReflectionTypeLoadException ex)
            {
                var sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    if (exSub is FileNotFoundException)
                    {
                        var exFileNotFound = exSub as FileNotFoundException;
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                string errorMessage = sb.ToString();
                //Display or log the error based on your application.
                _logWriter.Error(EventIds.BACKEND_SERVER_DOWN, errorMessage);
            }

            catch (Exception ex)
            {
                //Display or log the error based on your application.
                _logWriter.Error(EventIds.BACKEND_SERVER_DOWN, "InitializeNHibernate" + ex);
                throw;
            }
           
        }

        private void CreateFluentConfiguration(AutoPersistenceModel autoPersistenceModel)
        {
            try
            {
                Fluently.Configure(_configuration)
                    .Mappings(m => m.AutoMappings.Add(autoPersistenceModel))
#if DEBUG
                  //   .ExportTo(@"C:\temp\"))
#endif
                     .BuildConfiguration()
                    .CurrentSessionContext<ThreadLocalConversationalSessionContext>()
                    ;
            }
            catch (ReflectionTypeLoadException ex)
            {
                var sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    if (exSub is FileNotFoundException)
                    {
                        var exFileNotFound = exSub as FileNotFoundException;
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                    sb.AppendLine(ex.ToString());
                }
                var errorMessage = sb.ToString();
                _logWriter.Error(EventIds.BACKEND_SERVER_DOWN, "CreateFluentConfiguration: " + errorMessage);
            }
        }

        public override IEnumerable<Configuration> Configure()
        {
            //NHibernateInitializer.Instance().InitializeNHibernateOnce(InitializeNHibernate);  // magic
            InitializeNHibernate();
            AddNamedQueries(_configuration);
            DoAfterConfigure(_configuration);
            return new SingletonEnumerable<Configuration>(_configuration);
        }

        private void AddNamedQueries(Configuration configuration)
        {
            /*
             * http://stackoverflow.com/questions/3409812/how-can-the-namedsqlquerydefinition-class-be-used-dynamically-as-a-sql-query-equ
             */

            // sql named Queries
            //configuration.AddSqlNamedQuery"query name goes here","sql goes here");

            // hql named Queries
            //configuration.AddSqlNamedQuery"query name goes here","sql goes here");
        }
    }
}