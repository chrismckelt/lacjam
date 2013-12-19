namespace Lacjam.Core


module JobHandlers =
    open System
    open Autofac
    open NServiceBus
    open NServiceBus.MessageInterfaces
    open Lacjam
    open Lacjam.Core
    open Lacjam.Core.Runtime
    open Lacjam.Core.Payloads
    open Lacjam.Core.Payloads.Jobs

    type SiteScraperHandler(logger:ILogWriter) =
        interface IHandleMessages<Lacjam.Core.Payloads.Jobs.SiteScraper> with      
            member this.Handle(sc) = 
                let job = (sc:>JobBase)
                logger.Write(LogMessage.Debug(job.CreateDate.ToString() + "   " + job.Name))
                let html = job.Execute
                let bus = Lacjam.Core.Runtime.Ioc.Resolve<IBus>()
                try
                    let rv = Jobs.SiteScraper("Bedlam", "http://www.bedlam.net.au")
                    let jr = JobResult(rv.Id,true,rv.Execute)
                    bus.Reply(jr)
                with | ex -> logger.Write(LogMessage.Error(job.Name, ex, true))
                
                //Console.WriteLine(html)
                
                
    

