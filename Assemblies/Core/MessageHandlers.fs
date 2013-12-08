namespace Lacjam.Core
open System
open NServiceBus
open NServiceBus.MessageInterfaces
open Lacjam.Core.Jobs

module MessageHandlers =

    type BedlamPollHandler() =
         interface IHandleMessages<Lacjam.Core.Messages.BedlamPoll> with
              member this.Handle(bp) = 
                let result = bp.JobName
                Console.WriteLine(result)
                
    

