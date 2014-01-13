open System
open System.Net
open System.Net.Http
open System.Linq
open Lacjam
open Lacjam.Core
open Lacjam.Integration
open HtmlAgilityPack
  

[<EntryPoint>]
let main argv = 
    printfn "%A" argv    
    do System.Net.ServicePointManager.ServerCertificateValidationCallback <- (fun _ _ _ _ -> true) //four underscores (and seven years ago?)

     //views-label views-label-field-surf-report-date
   
    //http://www.swellnet.com/reports/australia/new-south-wales/cronulla

    Jira.outputRoadmap() |> ignore

    Console.ReadLine()  |> ignore
    0 // return an integer exit code