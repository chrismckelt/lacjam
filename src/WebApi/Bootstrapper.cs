//using Nancy.Bootstrapper
//using Nancy.Conventions;

//namespace Lacjam.Web.WebApi
//{
//    public class Bootstrapper : DefaultBootstrapper
//    {
//        protected override void ConfigureConventions(NancyConventions nancyConventions)
//        {
//            base.ConfigureConventions(nancyConventions);
//            nancyConventions.StaticContentsConventions
//               .Add(StaticContentConventionBuilder.AddDirectory("Content", "Content"));
//            nancyConventions.StaticContentsConventions
//                .Add(StaticContentConventionBuilder.AddDirectory("Scripts", "Scripts"));
//            nancyConventions.StaticContentsConventions
//                .Add(StaticContentConventionBuilder.AddDirectory("Fonts", "Fonts"));
//        }
//    }
//}