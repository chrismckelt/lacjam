module StartupBatchJobs
    open System
    open Autofac
    open NServiceBus
    open NServiceBus.Features
    open Lacjam.Core
    open Lacjam.Core.Runtime
    open Lacjam.Core.Scheduler
    open Lacjam.Core.Scheduler.Jobs

    let j1 = SiteScraper(Payload="http://www.bedlam.net.au") :> Job
    let j2 = SiteScraper(Payload="http://www.mckelt.com")  :> Job
    let j3 = SiteScraper(Payload="http://www.mckelt.com/blog") :> Job
    let batchJobs = seq [j1; j2; j3;]
       
    let Batches = {
        Batch.Id = Guid.NewGuid(); 
        Batch.Name = "site-wakeup" ; 
        Batch.Jobs = batchJobs 
        Batch.RunOnSchedule =TimeSpan.FromMinutes(Convert.ToDouble(1))
        }


    