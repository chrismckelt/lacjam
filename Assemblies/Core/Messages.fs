namespace Lacjam.Core
open System
open NServiceBus
open Lacjam.Core.Jobs

module Messages =
    
    type Schedule = 
        | EveryFiveMinutes of TimeSpan
        | OncePerDay of DateTime

    type BedlamPoll() =
        interface IMessage
        member this.Scraper:IAmAJob = new Lacjam.Core.Jobs.SiteScraper("Bedlam", "http://www.bedlam.net.au/") :> IAmAJob
        member this.Schedule = EveryFiveMinutes