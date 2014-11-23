using Lacjam.Framework.Dispatchers;
using Topshelf;

namespace Lacjam.Dispatcher
{
    class Program
    {
        static void Main()
        {
            var mode = "";
            NHibernateProfilerBootstrapper.PreStart();

            HostFactory.Run(x =>
            {
                x.AddCommandLineDefinition("mode", m=> {mode = m;});
                x.Service<IDispatcher>(s =>
                {
                    s.ConstructUsing(name => StartUp.GetDispatcher());
                    s.WhenStarted(dispatcher => dispatcher.Start(mode));
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

                x.SetDescription("Lacjam Event Dispatcher");
                x.SetDisplayName("Lacjam Event Dispatcher");
                x.SetServiceName("Lacjam.Dispatcher");
            });
        }

    }
}


