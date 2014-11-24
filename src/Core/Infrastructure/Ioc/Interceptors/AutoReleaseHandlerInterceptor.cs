using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.Core;
using Castle.DynamicProxy;
using Castle.MicroKernel;

namespace Lacjam.Core.Infrastructure.Ioc.Interceptors
{
    [Transient]
    public class AutoReleaseHandlerInterceptor : IInterceptor
    {
      
        private readonly IKernel _kernel;
        public AutoReleaseHandlerInterceptor(IKernel kernel)
        {
            _kernel = kernel;
        }

        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method != null)
            {
                invocation.Proceed();
                return;
            }
            try
            {
                invocation.Proceed();
            }
            finally
            {
                _kernel.ReleaseComponent(invocation.Proxy);
            }
        }
    }
}
