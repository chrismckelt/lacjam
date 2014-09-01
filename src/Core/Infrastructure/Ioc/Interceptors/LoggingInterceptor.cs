using System;
using System.Diagnostics;
using Castle.DynamicProxy;
using Lacjam.Framework.Logging;
using IInterceptor = Castle.DynamicProxy.IInterceptor;

namespace Lacjam.Core.Infrastructure.Ioc.Interceptors
{
    public class LoggingInterceptor :  IInterceptor
    {
        private readonly ILogWriter _logger;

        public LoggingInterceptor(ILogWriter logger)
        {
            _logger = logger;
        }

        [DebuggerStepThrough]
        public void Intercept(IInvocation invocation)
        {
            string methodName = invocation.TargetType.FullName + "." + invocation.GetConcreteMethod().Name;

            _logger.Debug( GetDateInLogFormat() + " " + methodName + " Entered");
            invocation.Proceed();
            _logger.Debug( GetDateInLogFormat() + " " + methodName + " Exited");
        }

        [DebuggerStepThrough]
        private string GetDateInLogFormat()
        {
            DateTime theDate = DateTime.Now;
            return String.Concat(theDate.ToShortDateString(), " ", theDate.TimeOfDay.ToString());
        }
    }
}