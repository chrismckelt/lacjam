using System.Collections.Generic;
using System.Web;

namespace Lacjam.WebApi.Infrastructure.Ioc
{
    /// <summary>
    /// Storage for PerHttpApplication lifestyle components
    /// </summary>
    public class PerHttpApplicationLifestyleModule : IHttpModule
    {
        public void Init(HttpApplication context) { }
        public void Dispose() { }

        private readonly IDictionary<string, object> Components = new Dictionary<string, object>();

        public bool HasComponent(string id)
        {
            return Components.ContainsKey(id);
        }

        public object this[string id]
        {
            get { return Components[id]; }
            set { Components[id] = value; }
        }
    }
}