using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Structerre.MetaStore.WebApi.Infrastructure.Ioc
{
    public class ReadServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                    Classes.FromThisAssembly()
                           .Where(x => x.Name.Contains("ReadService"))
                           .WithServiceAllInterfaces()
                           .LifestyleScoped()
                           
                          
                          
            );
        }        
    }
}