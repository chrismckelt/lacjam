using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Lacjam.Core.Infrastructure.Ioc;

namespace Lacjam.Dispatcher
{
    public static class DispatcherSetup
    {
        public static void Configure()
        {
            WindsorAccessor.Instance.Container.Install(new DispatcherInstaller());
        }
    }

    public class DispatcherInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {

            NHibernateBootStrap.Configure();

            container.Register(
                Classes.FromThisAssembly()
                    .Pick()
                    .WithServiceFirstInterface()
                    .LifestyleSingleton());

        }
    }
}