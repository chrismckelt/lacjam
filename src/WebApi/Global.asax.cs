using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using Iesi.Collections;
using Newtonsoft.Json.Serialization;
using Lacjam.Core.Infrastructure.Ioc;
using Lacjam.Core.Infrastructure.Ioc.Convo;
using Lacjam.Framework.Converters;
using Lacjam.Framework.Logging;
using Lacjam.WebApi.Infrastructure;
using Lacjam.WebApi.Infrastructure.Attributes;
using Lacjam.WebApi.Infrastructure.Ioc;
using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using AuthorizeAttribute = System.Web.Http.AuthorizeAttribute;
using IFilterProvider = System.Web.Mvc.IFilterProvider;

namespace Lacjam.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication, IContainerAccessor
    {
        protected void Application_Start()
        {
            StartUp.Intitialize();
            SetupWebApi();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //SetupFilterProvider();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
     
        }

        private void SetupWebApi()
        {
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            System.Web.Http.GlobalConfiguration.Configuration.Filters.Clear();
            GlobalConfiguration.Configuration.Filters.Add(new TransactionalAttribute()); //webapi

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new InlineJsonConverter());

            var appXmlType =
                GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(
                    t => t.MediaType == "application/xml");
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
            GlobalConfiguration.Configuration.DependencyResolver = new WindsorDependencyResolver(WindsorAccessor.Instance.Container);    // wire up all webapi through castle

           
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            WindsorAccessor.Instance.Container.Resolve<ILogWriter>().Info("Application_BeginRequest");
            WindsorAccessor.Instance.Container.BeginScope();
 

        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            WindsorAccessor.Instance.Container.Resolve<ILogWriter>().Info("Application_EndRequest");
           
        }


        public IWindsorContainer Container
        {
            get { return WindsorAccessor.Instance.WithSessionManagement().Container; }

        }

        private void SetupFilterProvider()
        {
            var providers = GlobalConfiguration.Configuration.Services.GetFilterProviders().ToList();

            var defaultprovider = providers.First(i => i is ActionDescriptorFilterProvider);
            GlobalConfiguration.Configuration.Services.Remove(typeof(IFilterProvider), defaultprovider);

            GlobalConfiguration.Configuration.Services.Add(typeof(IFilterProvider),
                                                           new WindsorFilterProvider(WindsorAccessor.Instance.Container.Kernel));
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            
        }
    }
}
