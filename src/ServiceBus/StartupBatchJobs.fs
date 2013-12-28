module StartupBatchJobs
    open System
    open Autofac
    open NServiceBus
    open NServiceBus.Features
    open Lacjam.Core
    open Lacjam.Core.Runtime
    open Lacjam.Core.Scheduler
    open Lacjam.Core.Scheduler.Jobs

    let ssJob = SiteScraper()
    ssJob.Payload <- "http://www.bedlam.net.au"
        
    let BedlamBatch = {
        Batch.Id = Guid.NewGuid(); 
        Batch.Name = "BB" ; 
        Batch.Jobs = seq [| ssJob |] 
        Batch.RunOnSchedule =TimeSpan.FromMinutes(Convert.ToDouble(1))
        }