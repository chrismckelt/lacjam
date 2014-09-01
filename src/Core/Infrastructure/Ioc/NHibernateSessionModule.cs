using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Castle.MicroKernel.Lifestyle;
using Lacjam.Core.Infrastructure.Ioc.Convo;
using Lacjam.Framework.Logging;
using uNhAddIns.SessionEasier;

namespace Lacjam.Core.Infrastructure.Ioc
{
    public class NHibernateSessionModule : IHttpModule
    {
        private HttpApplication _app;
        private ISessionFactoryProvider _sfp;
        private UnitOfWork _uow;


        public void Init(HttpApplication context)
        {
            WindsorAccessor.Instance.Container.Resolve<ILogWriter>()
                .Debug(@"Init ------------------------------------------------------------");


            context.BeginRequest += ContextBeginRequest;
            context.EndRequest += ContextEndRequest;
            context.RequestCompleted += RequestCompleted;
            context.Error += ContextError;
        }

        private void ContextBeginRequest(object sender, EventArgs e)
        {
            //WindsorAccessor.Instance.WithSessionManagement()
            //    .Container.Resolve<ILogWriter>()
            //    .Debug(@" ContextBeginRequest ------------------------------------------------------------");
            //WindsorAccessor.Instance.WithSessionManagement().Container.BeginScope();
        }


        private void ContextEndRequest(object sender, EventArgs e)
        {
        }

        private void RequestCompleted(object sender, EventArgs e)
        {
        }


        private void ContextError(object sender, EventArgs e)
        {
            WindsorAccessor.Instance.Container.Resolve<ILogWriter>()
                .Error(@"Error ------------------------------------------------------------");

            var httpContext = HttpContext.Current;

            var imageRequestTypes =
                httpContext.Request.AcceptTypes.Where(a => a.StartsWith("image/")).Select(a => a.Count());

            if (imageRequestTypes.Count() > 0)
            {
                httpContext.ClearError();
                return;
            }

            var lastException = HttpContext.Current.Server.GetLastError().GetBaseException();
            var httpException = lastException as HttpException;
            var statusCode = (int)HttpStatusCode.InternalServerError;

            if (httpException != null)
            {
                statusCode = httpException.GetHttpCode();
                if ((statusCode != (int)HttpStatusCode.NotFound) &&
                    (statusCode != (int)HttpStatusCode.ServiceUnavailable))
                {
                    WindsorAccessor.Instance.Container.Resolve<ILogWriter>()
                        .Error(EventIds.Error, "exception=", httpException);
                }
            }

            var redirectUrl = string.Empty;

            if (httpContext.IsCustomErrorEnabled)
            {
                var errorsSection = WebConfigurationManager.GetSection("system.web/customErrors") as CustomErrorsSection;
                if (errorsSection != null)
                {
                    redirectUrl = errorsSection.DefaultRedirect;

                    if (httpException != null && errorsSection.Errors.Count > 0)
                    {
                        var item = errorsSection.Errors[statusCode.ToString()];

                        if (item != null)
                        {
                            redirectUrl = item.Redirect;
                        }
                    }
                }
            }

            httpContext.Response.Clear();
            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.TrySkipIisCustomErrors = true;
            httpContext.ClearError();

            if (!string.IsNullOrEmpty(redirectUrl))
            {
                var mvcHandler = httpContext.CurrentHandler as MvcHandler;
                if (mvcHandler == null)
                {
                    httpContext.Server.Transfer(redirectUrl);
                }
                else
                {
                    var uriBuilder = new UriBuilder(
                        httpContext.Request.Url.Scheme,
                        httpContext.Request.Url.Host,
                        httpContext.Request.Url.Port,
                        httpContext.Request.ApplicationPath);

                    uriBuilder.Path += redirectUrl;

                    string path = httpContext.Server.UrlDecode(uriBuilder.Uri.PathAndQuery);
                    HttpContext.Current.RewritePath(path, false);
                    IHttpHandler httpHandler = new MvcHttpHandler();

                    httpHandler.ProcessRequest(HttpContext.Current);
                }
            }
        }

        public void Dispose()
        {
            _app.BeginRequest -= ContextBeginRequest;
            _app.EndRequest -= ContextEndRequest;
            _app.RequestCompleted -= RequestCompleted;
            _app.Error -= ContextError;
        }
    }
}