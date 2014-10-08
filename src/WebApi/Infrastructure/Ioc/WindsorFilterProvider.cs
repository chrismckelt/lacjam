using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Castle.MicroKernel;

namespace Structerre.MetaStore.WebApi.Infrastructure.Ioc
{
    public class WindsorFilterProvider : FilterAttributeFilterProvider
    {
        public WindsorFilterProvider(IKernel kernel)
        {
            _kernel = kernel;
        }

        private readonly IKernel _kernel;

        public override IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var filters = base.GetFilters(controllerContext, actionDescriptor);
            foreach (var filter in filters)
            {
                _kernel.InjectProperties(filter.Instance);
            }

            var resolvedFilters = filters.Select(
                    f => new Filter(_kernel.Resolve<IActionFilter>(f.Instance.GetType().Name), f.Scope, f.Order));

            return filters;
        }
    }
}