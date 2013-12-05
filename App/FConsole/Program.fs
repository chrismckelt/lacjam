open System
open HttpClient  
[<EntryPoint>]
let main argv = 
    printfn "%A" argv    
    printfn "%s" (createRequest Get "http://www.bedlam.net.au" |> getResponseBody)
    Console.ReadLine()
    0 // return an integer exit code
