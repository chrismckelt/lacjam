using Nancy.Bootstrappers.Autofac;
using Nancy.Conventions;

namespace Lacjam.Web.WebApi
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {
        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);
            nancyConventions.StaticContentsConventions
               .Add(StaticContentConventionBuilder.AddDirectory("Content", "Content"));
            nancyConventions.StaticContentsConventions
                .Add(StaticContentConventionBuilder.AddDirectory("Scripts", "Scripts"));
            nancyConventions.StaticContentsConventions
                .Add(StaticContentConventionBuilder.AddDirectory("Fonts", "Fonts"));
        }
    }
}