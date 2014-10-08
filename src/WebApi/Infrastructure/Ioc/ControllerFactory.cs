using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Windsor;
using Structerre.MetaStore.Framework.Utilities;

namespace Structerre.MetaStore.WebApi.Infrastructure.Ioc
{
    public class ControllerFactory : DefaultControllerFactory
    {
        private readonly IWindsorContainer _container;
     
        public ControllerFactory(IWindsorContainer container)
        {
            _container = container;
          ;
        }

        public override void ReleaseController(IController controller)
        {
            _container.Kernel.ReleaseComponent(controller);
        }


        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            // ignore files such as jpegs
            string ext = Path.GetExtension(requestContext.HttpContext.Request.AppRelativeCurrentExecutionFilePath);

            if (!string.IsNullOrWhiteSpace(ext) || GetFileExtensions().Any(a=>a==ext.ToLowerInvariant()))
            {
                // A file with a known extension (jpg, html, etc) has not been found on the server, and passed to MVC for routing.
                throw new HttpException(404, "File not found");
            }

#if DEBUG
            //try
            //{
            //    _logWriter.Debug(EventIds.DEBUG_INFORMATION, "ControllerFactory GetControllerInstance " + controllerType.ToString());
            //    return (IController)_container.Resolve(controllerType);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("ControllerFactory.GetControllerInstance: " + ex.ToString());
            //    //throw new ApplicationException("Controller does not exist - CREATE IT!", ex);
            //}
#endif

            if (controllerType != null)
            {
                var ctlr = _container.Resolve(controllerType);
                if (ctlr!=null)
                {
                    return (IController)ctlr;
                }
            }

            var filename = controllerType == null ? "File" : controllerType.ToString();
            throw new HttpException(404, filename + " not found");
        }

        private static IEnumerable<string> GetFileExtensions()
        {
            return Util.GetConstants(typeof (Constant.FileExtensions), true);
        }
    }
}
