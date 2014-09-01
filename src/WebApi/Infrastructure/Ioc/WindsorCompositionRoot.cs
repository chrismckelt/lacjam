using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Castle.Windsor;

namespace Lacjam.WebApi.Infrastructure.Ioc
{
    public class WindsorCompositionRoot : IHttpControllerActivator
    {
        private readonly IWindsorContainer _container;

        public WindsorCompositionRoot(IWindsorContainer container)
        {
            this._container = container;
        }

        public IHttpController Create(
            HttpRequestMessage request,
            HttpControllerDescriptor controllerDescriptor,
            Type controllerType)
        {
            var controller = (IHttpController)this._container.Resolve(
                controllerType,
                new
                {
                    request = request
                });

            request.RegisterForDispose(
                new Release(
                    () => this._container.Release(controller)));
            return controller;
        }

        private class Release : IDisposable
        {
            private readonly Action _release;

            public Release(Action release)
            {
                this._release = release;
            }

            public void Dispose()
            {
                this._release();
            }
        }
    }
}