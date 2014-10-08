using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using Newtonsoft.Json.Serialization;
using Lacjam.Core.Infrastructure.Ioc;
using Lacjam.Core.Infrastructure.Ioc.Convo;
using Lacjam.Framework.Converters;
using Lacjam.Framework.Logging;
using Lacjam.WebApi.Infrastructure.Attributes;
using Lacjam.WebApi.Infrastructure.Ioc;
using System;

namespace Lacjam.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication, IContainerAccessor
    {
        protected void Application_Start()
        {
            StartUp.Intitialize();
            SetupWebApi();
        
        }

        private void SetupWebApi()
        {
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            System.Web.Http.GlobalConfiguration.Configuration.Filters.Clear();
            GlobalConfiguration.Configuration.Filters.Add(new TransactionalAttribute()); //webapi
            GlobalConfiguration.Configuration.Filters.Add(new ValidateModelStateAttribute()); //webapi


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

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            WindsorAccessor.Instance.Container.Resolve<ILogWriter>().Info("Application_Error");
            // Get the exception object.
            Exception exc = Server.GetLastError();

            WindsorAccessor.Instance.Container.Resolve<ILogWriter>().Warn(EventIds.GlobalAsax,"Application_Error", exc);
            WindsorAccessor.Instance.Container.Resolve<IUnitOfWork>().Abort();

            if (IsMaxRequestExceededException(exc))
            {
                this.Server.ClearError();
                Response.Write(@"Request data is too large to process");
                Response.End();
            }

        }

        const int TimedOutExceptionCode = -2147467259;
        public static bool IsMaxRequestExceededException(Exception e)
        {
            // http://stackoverflow.com/questions/665453/catching-maximum-request-length-exceeded
            Exception main;
            var unhandled = e as HttpUnhandledException;

            if (unhandled != null && unhandled.ErrorCode == TimedOutExceptionCode)
            {
                main = unhandled.InnerException;
            }
            else
            {
                main = e;
            }

            var http = main as HttpException;

            if (http != null && http.ErrorCode == TimedOutExceptionCode)
            {
                // hack: no real method of identifying if the error is max request exceeded as 
                // it is treated as a timeout exception
                if (http.StackTrace.Contains("GetEntireRawContent"))
                {
                    // MAX REQUEST HAS BEEN EXCEEDED
                    return true;
                }
            }
            return false;
        }

    }
}
