using System;
using System.Configuration;
using System.IO;
using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Lacjam.Core.Infrastructure.Ioc.Convo;

namespace Lacjam.Core.Infrastructure.Ioc
{
    public class WindsorAccessor : IContainerAccessor
    {
        private static IWindsorContainer _container;
        public static WindsorAccessor Instance = new WindsorAccessor();
        private bool _withSessionManagement;
        private bool _withConversationPerThread;

        private WindsorAccessor()
        {
        }

        public WindsorAccessor WithContainer(IWindsorContainer container)  // for testing
        {
            _container = container;
            return this;
        }

        /// <summary>
        /// needs to be the included in the first call for the AppDomains access to this singleton
        /// </summary>
        /// <returns></returns>
        public WindsorAccessor WithSessionManagement()  
        {
            _withSessionManagement = true;
            return this;
        }

        public WindsorAccessor WithConversationPerThread()
        {
            _withConversationPerThread = true;
            
            return this;
        }

        private static void SetConversationPerThread()
        {
            WindsorAccessor.Instance.Container.Register(
                Component.For<IUnitOfWork>().ImplementedBy<UnitOfWork>().LifestylePerThread());
            WindsorAccessor.Instance.Container.BeginScope();
        }

        public IWindsorContainer Container
        {
            get
            {
                if (_container == null)
                {
                    InitContainer();

                    if (_withSessionManagement)
                        _container.Install(new SessionManagementInstaller());

                    if (_withConversationPerThread)
                        SetConversationPerThread();
                }

                return _container;
            }
        }

        private void InitContainer()
        {
            _container = new IocContainer();
        }

        public static void Dispose()
        {
            if (Instance.Container != null) Instance.Container.Dispose();
        }

        public static string FindNHibernateConfigFile()
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory.ToLowerInvariant();

            foreach (string file in Directory.EnumerateFiles(dir, "NHibernate.config", SearchOption.AllDirectories))
                return file;

            throw new ConfigurationErrorsException("NHibernate file not found - NHibernate.config");

        }
    }
}