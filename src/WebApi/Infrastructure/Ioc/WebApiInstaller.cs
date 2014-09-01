using System.Web.Http;
using System.Web.Http.Controllers;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Lacjam.Core.Infrastructure.Ioc.Interceptors;
using Lacjam.Framework.Dispatchers;
using Lacjam.Framework.Handlers;
using Lacjam.WebApi.Infrastructure.Dispatch;

namespace Lacjam.WebApi.Infrastructure.Ioc
{
    public class WebApiInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes.FromThisAssembly()
                    .BasedOn<IHttpController>()
                    .Configure(a => a.Interceptors<LoggingInterceptor>())
                    .Configure(a => a.Interceptors<ExceptionHandlerInterceptor>())
                    .ConfigureFor<ApiController>(c => c.PropertiesIgnore(x => x.Name == "Request"))
                    .LifestyleScoped()
                );

         
            container.Register(
                Classes.FromThisAssembly()
                    .Where(x => x.Name.Contains("ReadService"))
                    .WithServiceAllInterfaces()
                    .LifestyleScoped());
                           
            container.Register(
                Classes.From().InNamespace("Lacjam")
                    .Where(x => x.Name.ToLowerInvariant().Contains("Lacjam"))
                    .WithServiceAllInterfaces()
                    .LifestyleScoped());

            container.Register(Component.For<ICommandDispatcher>()
                                        .ImplementedBy<CommandDispatcher>()
                                        .LifestyleScoped() 
            );
        }
    }
}