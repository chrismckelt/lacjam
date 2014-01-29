open System
open System.Net
open System.Net.Http
open System.Linq
open Lacjam
open Lacjam.Core
open Lacjam.Integration
open HtmlAgilityPack
open System
open System.IO
open Autofac
open NServiceBus
open NServiceBus.Features
open Lacjam.Core
open Lacjam.Core.Utility
open Lacjam.Core.Runtime
open Lacjam.Core.Scheduling
open Lacjam.Core.Jobs
open Lacjam.Core.Settings
open Lacjam.Integration
open Quartz
open Quartz.Spi
open Quartz.Impl
open Autofac
open NServiceBus.ObjectBuilder
open NServiceBus.ObjectBuilder.Common
    

[<EntryPoint>]
let main argv = 
    printfn "%A" argv    
    do System.Net.ServicePointManager.ServerCertificateValidationCallback <- (fun _ _ _ _ -> true) //four underscores (and seven years ago?)

    let re = Settings.getTwitterSettings
    Console.WriteLine(re.ConsumerKey)
    Console.ReadLine()  |> ignore
    0 // return an integer exit code