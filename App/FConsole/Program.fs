open System
open System.Net
open System.Net.Http
open Lacjam
open Lacjam.Core
open Lacjam.Core.Jobs
  


[<EntryPoint>]
let main argv = 
    printfn "%A" argv    
    let wtf = JobNames.SiteScraperJob
    Console.WriteLine(wtf.ToString())
    match wtf with 
    | _-> () 
    let siteRetriever = Lacjam.Core.Jobs.SiteScraper("Bedlam", "http://www.bedlam.net.au/") :> Lacjam.Core.Jobs.IAmAJob
    let result = siteRetriever.Execute
    Console.WriteLine(siteRetriever.Name)
    Console.WriteLine(result)
    Console.ReadLine()
    0 // return an integer exit code