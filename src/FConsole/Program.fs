open System
open System.Net
open System.Net.Http
open Lacjam
open Lacjam.Core
open Lacjam.Integration
  

[<EntryPoint>]
let main argv = 
    printfn "%A" argv    
    do System.Net.ServicePointManager.ServerCertificateValidationCallback <- (fun _ _ _ _ -> true) //four underscores (and seven years ago?)
    do Jira.outputRoadmap()
    Console.ReadLine()  |> ignore
    0 // return an integer exit code