using System.Diagnostics;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Registration;
using Lacjam.Core.Infrastructure.Ioc;
using Lacjam.Core.Infrastructure.Ioc.Convo;
using Lacjam.Framework.Installer;
using Lacjam.Framework.Logging;

namespace Lacjam.WebApi.Infrastructure.Ioc
{
    public static class StartUp
    {
        public static void Intitialize()
        {
            WindsorAccessor.Instance.WithSessionManagement().Container.Resolve<ILogWriter>().Info("Application_Start  IOC SETUP started");
            
            WindsorAccessor.Instance.Container.Install(
               
                new WindsorHttpModuleInstaller(),
                new FrameworkInstaller(),
                new WebApiInstaller()
                
            );

            WindsorAccessor.Instance.Container.Register(Component.For<IUnitOfWork>().ImplementedBy<UnitOfWork>().LifestyleScoped());

            //WindsorAccessor.Instance.Container.Register(Component.For<NHibernateSessionModule>().LifestyleScoped());
          
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator),
               new WindsorCompositionRoot(WindsorAccessor.Instance.Container));
            WindsorAccessor.Instance.Container.Resolve<ILogWriter>().Info("Application_Start  IOC SETUP complete");
        }
    }
}