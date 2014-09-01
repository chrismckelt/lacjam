using System;
using System.Diagnostics;
using Castle.DynamicProxy;
using Lacjam.Framework.Logging;

namespace Lacjam.Core.Infrastructure.Ioc.Interceptors
{
    public class ExceptionHandlerInterceptor : IInterceptor
    {

        [DebuggerStepThrough]
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                WindsorAccessor.Instance.Container.Resolve<ILogWriter>().Warn(ex.ToString());

                throw;
            }
        }
    }
}