using HibernatingRhinos.Profiler.Appender.NHibernate;
using Lacjam.WebApi;

[assembly: WebActivator.PreApplicationStartMethod(typeof(NHibernateProfilerBootstrapper), "PreStart")]
namespace Lacjam.WebApi
{
    public static class NHibernateProfilerBootstrapper
    {
        public static void PreStart()
        {
#if DEBUG
            NHibernateProfiler.Initialize();
#endif

        }
    }
}

