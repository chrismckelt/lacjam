using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Lacjam.Framework.Dispatchers;
using Lacjam.Framework.Handlers;
using Lacjam.Framework.Hash;
using Lacjam.Framework.Logging;
using Lacjam.Framework.Model;
using Lacjam.Framework.Projection;
using Lacjam.Framework.Storage;

namespace Lacjam.Framework.Installer
{
    public class FrameworkInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //container.Register(Component.For(typeof (IRepository<>)).ImplementedBy(typeof (Repository<>)).LifestyleSingleton());

            //container.Register(Component.For<IHandlerSequenceRespository>().ImplementedBy<Lacjam.Framework.Handlers.HandlerSequenceRespository>());
            //container.Register(Component.For<IHandlerExecutor>().ImplementedBy<Lacjam.Framework.Handlers.HandlerExecutor>());


            container.Register(
               Classes.FromThisAssembly()
                   .Pick()
                   .WithServiceFirstInterface()
                   .LifestyleTransient());

        }
    }
}