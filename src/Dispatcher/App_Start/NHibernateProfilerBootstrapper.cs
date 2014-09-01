namespace Lacjam.Dispatcher
{
	public static class NHibernateProfilerBootstrapper
	{
		public static void PreStart()
		{

#if DEBUG
			// Initialize the profiler
			//NHibernateProfiler.Initialize();
#endif
			
			// You can also use the profiler in an offline manner.
			// This will generate a file with a snapshot of all the NHibernate activity in the application,
			// which you can use for later analysis by loading the file into the profiler.
			// var filename = @"c:\profiler-log";
			// NHibernateProfiler.InitializeOfflineProfiling(filename);
		}
	}
}

