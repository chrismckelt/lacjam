namespace Lacjam.Core.Messages
open System
open System.Runtime.Serialization
open NServiceBus
open Lacjam.Core.Jobs

type Schedule = 
    | EveryFiveMinutes of TimeSpan
    | OncePerDay of DateTime

[<Serializable>]
type SiteRetrieverResult() = 
    
    member val Html = "BedlamPoll" with get, set
    //member this.Schedule = EveryFiveMinutes
    interface IMessage

[<Serializable>]
type BedlamPoll() = 
    
    member val JobName = "BedlamPoll" with get, set
    //member this.Schedule = EveryFiveMinutes
    interface IMessage
        