namespace Lacjam.Core
open System
open NServiceBus
open NServiceBus.MessageInterfaces
open Lacjam
open Lacjam.Core
open Lacjam.Core.Messages
open Lacjam.Core.Jobs

module MessageHandlers =

    type BedlamPollHandler() =
         interface IHandleMessages<Lacjam.Core.Messages.BedlamPoll> with
              member this.Handle(bp) = 
                let result = bp.JobName
                let job:IAmAJob = new SiteScraper("Bedlam", "http://www.bedlam.net.au") :> IAmAJob
                let html = job.Execute
                Console.WriteLine(html)
                
    

