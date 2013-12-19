namespace Lacjam.Core


module JobHandlers =
    open System
    open Autofac
    open NServiceBus
    open NServiceBus.MessageInterfaces
    open Lacjam
    open Lacjam.Core
    open Lacjam.Core.Runtime
    open Lacjam.Core.Jobs

    type BedlamPollHandler(logger:ILogWriter) =
        interface IHandleMessages<Lacjam.Core.Jobs.BedlamPoll> with      
            member this.Handle(bp) = 
                let result = bp.JobName
                logger.Write(LogMessage.Debug("Received: " + bp.JobName))
                let job = new SiteScraper("Bedlam", "http://www.bedlam.net.au") 
                let html = (job :> IAmAJob).Execute
                let bus = Lacjam.Core.Runtime.Ioc.Resolve<IBus>()
                try
                    let rv = new SiteRetrieverResult()
                    rv.Html <- html
                    bus.Reply(rv)
                with | ex -> logger.Write(LogMessage.Error(bp.JobName, ex, true))
                
                //Console.WriteLine(html)
                
                
    

