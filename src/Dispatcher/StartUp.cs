using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Registration;
using Lacjam.Core.Infrastructure.Ioc;
using Lacjam.Core.Infrastructure.Ioc.Convo;
using Lacjam.Framework.Dispatchers;

namespace Lacjam.Dispatcher
{
    public static class StartUp
    {
        public static IDispatcher GetDispatcher()
        {
            DispatcherSetup.Configure();
          
            return WindsorAccessor.Instance.Container.Resolve<IDispatcher>();
        } 
    }
}