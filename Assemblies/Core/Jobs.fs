namespace Lacjam.Core

module Jobs = 
    open System
    open HttpClient
    
    do printfn "%s" (createRequest Get "http://www.google.com" |> getResponseBody)
