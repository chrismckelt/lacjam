using System.Web;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;

namespace Lacjam.WebApi.Infrastructure.Ioc
{

    public class WindsorHttpModuleInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<HttpRequestBase>()
                .LifeStyle.PerWebRequest
                .UsingFactoryMethod(() => new HttpRequestWrapper(HttpContext.Current.Request)));

            container.Register(Component.For<HttpContextBase>()
                .LifeStyle.PerWebRequest
                .UsingFactoryMethod(() => new HttpContextWrapper(HttpContext.Current)));

        }
    }
}