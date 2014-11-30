//using Castle.MicroKernel;
//using Castle.MicroKernel.Facilities;

//namespace Lacjam.Core.Infrastructure.Ioc.Interceptors
//{
//    /// <summary>
//    /// Hooks into kernel component registration events in order to attach interceptors
//    /// </summary>
//    public class InterceptionFacility : AbstractFacility
//    {
//        protected override void Init()
//        {
//            Kernel.ComponentRegistered += new ComponentDataDelegate(Kernel_ComponentRegistered);
//        }

//        void Kernel_ComponentRegistered(string key, IHandler handler)
//        {
//            // handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(LoggingInterceptor)));
//            // handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(ExceptionHandlerInterceptor)));

//            // Add ExceptionHandlingInterceptor as last so that it can wrap exceptions thrown out of components
//            // including other interceptors.
//            //handler.ComponentModel.Interceptors.AddLast(new InterceptorReference(typeof(ExceptionHandlingInterceptor)));
//        }
//    }
//}
