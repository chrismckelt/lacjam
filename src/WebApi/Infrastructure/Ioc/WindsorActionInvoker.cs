using System.Collections.Generic;
using System.Web.Mvc;
using Castle.MicroKernel;

namespace Lacjam.WebApi.Infrastructure.Ioc
{
    public class WindsorActionInvoker : ControllerActionInvoker
    {
        readonly IKernel _kernel;
        public WindsorActionInvoker(IKernel kernel)
        {
            this._kernel = kernel;
        }
        protected override ActionExecutedContext InvokeActionMethodWithFilters(
            ControllerContext controllerContext
            , IList<IActionFilter> filters
            , ActionDescriptor actionDescriptor
            , IDictionary<string, object> parameters)
        {
            foreach (IActionFilter actionFilter in filters)
            {
                _kernel.InjectProperties(actionFilter);
            }
            return base.InvokeActionMethodWithFilters(controllerContext, filters, actionDescriptor, parameters);
        }
    }

}