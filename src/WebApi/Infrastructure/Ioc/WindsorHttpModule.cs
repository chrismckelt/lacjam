using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using Castle.Windsor;

namespace Lacjam.WebApi.Infrastructure.Ioc
{
    /// <summary>
    /// Initializes and disposes Windsor-managed IHttpModules
    /// </summary>
    public class WindsorHttpModule : IHttpModule
    {
        private readonly IList<IHttpModule> modulesToDispose = new List<IHttpModule>();

        public void Init(HttpApplication context)
        {
            foreach (var module in GetModules(context))
            {
                var m = module;
                m.Init(context);
                if (!(m is IDisposable))
                { // if module is IDisposable it will be handled by the release policy
                    modulesToDispose.Add(m);
                }
            }
        }

        private IHttpModule[] GetModules(HttpApplication context)
        {
            var containerAccessor = context as IContainerAccessor;
            if (containerAccessor == null)
                throw new ConfigurationErrorsException(string.Format("The Global HttpApplication instance needs to implement {0}", typeof(IContainerAccessor).FullName));
            var container = containerAccessor.Container;
            if (container == null)
                throw new ConfigurationErrorsException("HttpApplication has no container initialized");
            return container.ResolveAll<IHttpModule>();
        }

        public void Dispose()
        {
            foreach (var module in modulesToDispose)
                module.Dispose();
        }
    }
}