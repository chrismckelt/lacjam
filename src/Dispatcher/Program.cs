using Lacjam.Framework.Dispatchers;
using Topshelf;

namespace Lacjam.Dispatcher
{
    class Program
    {

        static void Main()
        {
            NHibernateProfilerBootstrapper.PreStart();

            HostFactory.Run(x =>
            {
                x.Service<IDispatcher>(s =>
                {
                    s.ConstructUsing(name => StartUp.GetDispatcher());
                    s.WhenStarted(dispatcher => dispatcher.Start());
                    s.WhenStopped(dispatcher => dispatcher.Stop());
                });
                x.RunAsLocalSystem();

                x.EnableServiceRecovery(r =>
                {
                    r.RestartService(1);
                    r.RestartService(1);
                    r.RestartService(1);

                    r.SetResetPeriod(1);
                });

                x.SetDescription("Lacjam MetaStore Event Dispatcher");
                x.SetDisplayName("Lacjam MetaStore Event Dispatcher");
                x.SetServiceName("Lacjam.Dispatcher");
            });
        }

    }
}


