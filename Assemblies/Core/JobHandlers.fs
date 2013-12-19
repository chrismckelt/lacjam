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

    type SiteScraperHandler(logger:ILogWriter, bus:IBus) =
        interface IHandleMessages<Lacjam.Core.Payloads.Jobs.SiteScraper> with      
            member this.Handle(sc) = 
                let job = (sc:>JobBase)
                logger.Write(LogMessage.Debug(job.CreateDate.ToString() + "   " + job.Name))
                let html = job.Execute
                let bus = Lacjam.Core.Runtime.Ioc.Resolve<IBus>()
                try
                    let rv = Jobs.SiteScraperResult()
                    rv.Html <- html
                    bus.Reply(rv)
                with | ex -> logger.Write(LogMessage.Error(job.Name, ex, true))
                
                //Console.WriteLine(html)
                
                
    

