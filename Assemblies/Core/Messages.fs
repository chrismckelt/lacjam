namespace Lacjam.Core.Messages
open System
open System.Runtime.Serialization
open NServiceBus
open Lacjam.Core.Jobs

type Schedule = 
    | EveryFiveMinutes of TimeSpan
    | OncePerDay of DateTime

[<Serializable>]
type BedlamPoll() = 
    member val JobName = "" with get, set
    //member this.Schedule = EveryFiveMinutes
    interface IMessage
        