using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using Lacjam.Core.Infrastructure.Ioc.Convo;
using Lacjam.Framework.Logging;
using uNhAddIns.SessionEasier.Contexts;
using IDependencyResolver = System.Web.Http.Dependencies.IDependencyResolver;

namespace Lacjam.WebApi.Infrastructure.Ioc
{
    [DebuggerStepThrough]
    public class WindsorDependencyResolver : IDependencyResolver
    {
        /// <summary>
        ///     Indicates whether or not the class has previously been disposed.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="CastleDependencyResolver"/> class.
        /// </summary>
        /// <param name="container">The Castle container used to resolve components.</param>
        /// <exception cref="System.ArgumentNullException">container;@The instance of the container cannot be null.</exception>
        public WindsorDependencyResolver(IWindsorContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container", @"The instance of the container cannot be null.");
            }

            this.Container = container;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="CastleDependencyResolver"/> class.
        /// </summary>
        ~WindsorDependencyResolver()
        {
            Dispose(false);
        }

        /// <summary>
        ///    Gets or sets the Castle container.
        /// </summary>
        public IWindsorContainer Container { get; protected set; }

        /// <summary>
        ///     Starts a resolution scope.
        /// </summary>
        /// <returns>
        ///     A new instance of the <c>CastleDependencyScope</c>.
        /// </returns>
        public IDependencyScope BeginScope()
        {
            return new CastleDependencyScope(Container);
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Resolves a component from the Castle container.
        /// </summary>
        /// <param name="serviceType">The type of the component to be resolved.</param>
        /// <returns>
        ///     The resolved component; otherwise <c>null</c> if no component could be resolved.
        /// </returns>
        public object GetService(Type serviceType)
        {
            try
            {
                if (this.Container.Kernel.HasComponent(serviceType))
                {
                   
                    return this.Container.Resolve(serviceType);
                }
                this.Container.Resolve<ILogWriter>().Debug("GetService - " + serviceType.Name);
                return null;
            }
            catch (ComponentNotFoundException ex)
            {
                this.Container.Resolve<ILogWriter>().Warn(EventIds.Warn ,"GetService  - " + serviceType.Name, ex);
                return null;
            }
        }

        /// <summary>
        ///     Resolves a collection components from the Castle container.
        /// </summary>
        /// <param name="serviceType">The type of the component to be resolved.</param>
        /// <returns>
        ///     A list of the resolved components; otherwise <c>new List()</c> if no components could be resolved.
        /// </returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            // Not expected to trow an exception, rather return an empty IEnumerable.
            return Container.ResolveAll(serviceType).Cast<object>();
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Cleanup managed resources here, if necessary.
                    if (Container != null)
                    {
                        Container.Dispose();
                        Container = null;
                    }
                }

                // Cleanup any unmanaged resources here, if necessary.
                _disposed = true;
            }

            // Call the base class's Dispose(Boolean) method, if available.
            // base.Dispose( disposing );
        }
    }

    [DebuggerStepThrough]
    public class CastleDependencyScope : IDependencyScope
    {
        /// <summary>
        ///     Indicates whether or not the class has previously been disposed.
        /// </summary>
        private bool _disposed;

        /// <summary>
        ///     The scope of the components being resolved.
        /// </summary>
        /// <remarks>
        ///     This scope variable contains the castle scope object created when 
        ///     resolving the component with castle.
        /// </remarks>
        private IDisposable _scope;

        /// <summary>
        /// Initializes a new instance of the <see cref="CastleDependencyScope"/> class.
        /// </summary>
        /// <param name="container">The castle container used to resolve components.</param>
        /// <exception cref="System.ArgumentNullException">container</exception>
        public CastleDependencyScope(IWindsorContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.Container = container;
            _scope = Container.BeginScope();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="CastleDependencyScope"/> class.
        /// </summary>
        ~CastleDependencyScope()
        {
            Dispose(false);
        }


        /// <summary>
        ///     The Castle container used to resolve components.
        /// </summary>
        public IWindsorContainer Container { get; protected set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                
                if (disposing)
                {
                    _scope.Dispose();
                }

                // Cleanup any unmanaged resources here, if necessary.

                _disposed = true;
            }

            // Call the base class's Dispose(Boolean) method, if available.
            // base.Dispose( disposing );
        }

        

        /// <summary>
        ///     Resolves a component from the Castle container.
        /// </summary>
        /// <param name="serviceType">The type of the component to be resolved.</param>
        /// <returns>
        ///     The resolved component; otherwise <c>null</c> if no component could be resolved.
        /// </returns>
        public object GetService(Type serviceType)
        {
            try
            {
                if (this.Container.Kernel.HasComponent(serviceType))
                {
                     return this.Container.Resolve(serviceType);
                }
                this.Container.Resolve<ILogWriter>().Warn(EventIds.Warn, "GetService - " + serviceType.Name);
                return null;
            }
            catch (ComponentNotFoundException ex)
            {
                this.Container.Resolve<ILogWriter>().Warn(EventIds.Warn, "GetService  - " + serviceType.Name, ex);
                return null;
            }

        }

        /// <summary>
        ///     Resolves a collection components from the Castle container.
        /// </summary>
        /// <param name="serviceType">The type of the component to be resolved.</param>
        /// <returns>
        ///     A list of the resolved components; otherwise <c>new List()</c> if no components could be resolved.
        /// </returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            // Not expected to trow an exception, rather return an empty IEnumerable.
            return Container.ResolveAll(serviceType).Cast<object>();
        }

        
    }
}


 //this.Container.Resolve<ILogWriter>().Debug("GetService- " + serviceType.Name);
 //                   this.Container.Resolve<UnitOfWork>().Start();
 //                   CurrentSessionContext.Bind(this.Container.Resolve<UnitOfWork>().Session);
 //                   return this.Container.Resolve<UnitOfWork>().Run<object>(() => this.Container.Resolve(serviceType));