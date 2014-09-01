using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Lacjam.Core.Infrastructure.Ioc;
using Lacjam.Core.Infrastructure.Ioc.Convo;
using Lacjam.Framework.Logging;

namespace Lacjam.WebApi.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TransactionalAttribute : ActionFilterAttribute
    {

        //public override void OnActionExecuting(HttpActionContext actionContext)
        //{
        //    Start(actionContext);
        //}

        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            Start(actionContext);
            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

        //public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        //{
        //    End(actionExecutedContext);
        //}

      
        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext,
            CancellationToken cancellationToken)
        {

            End(actionExecutedContext);
            var tsk =  base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);

            tsk.Wait(cancellationToken);

            if (tsk.IsFaulted)
            {
                WindsorAccessor.Instance.Container.Resolve<IUnitOfWork>().Abort();
            }

            WindsorAccessor.Instance.Container.Resolve<IUnitOfWork>().End();

            return tsk;

        }


        private static void Start(HttpActionContext actionContext)
        {
            var logger = WindsorAccessor.Instance.Container.Resolve<ILogWriter>();
            logger.Debug(@" OnActionExecuting ------------------------------------------------------------");
            foreach (var actionParameter in actionContext.ActionArguments)
            {
                logger.Debug(actionParameter.Key + "  -   " + actionParameter.Value);
            }

            WindsorAccessor.Instance.Container.Resolve<IUnitOfWork>().Start();
        }


        private void End(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception != null)
            {
                WindsorAccessor.Instance.Container.Resolve<IUnitOfWork>().Abort();
            }


            WindsorAccessor.Instance.Container.Resolve<IUnitOfWork>().End();
        }


    }
}