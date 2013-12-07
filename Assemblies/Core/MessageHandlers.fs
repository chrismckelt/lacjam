namespace Lacjam.Core
open System
open NServiceBus
open NServiceBus.MessageInterfaces
open Lacjam.Core.Jobs

module MessageHandlers =

    type BedlamPollHandler() =
         interface IHandleMessages<Lacjam.Core.Messages.BedlamPoll> with
              member this.Handle(bp :Lacjam.Core.Messages.BedlamPoll) = 
                let result = bp.Scraper.Execute()
                Console.WriteLine(result)
                
    

