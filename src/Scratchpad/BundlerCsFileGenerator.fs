#if INTERACTIVE
namespace BundlerCs
#endif

module BundlerCsFileGenerator 


#if INTERACTIVE
=
#r "System.dll"
#r "System.Data.dll"
#l "C:\Program Files (x86)\Reference Assemblies\Microsoft\FSharp\.NETFramework\v4.0\4.3.1.0\FSharp.Core.dll"
#r "FSharp.Data.dll"
#r "FSharp.Data.TypeProviders.dll"
#r "System.Data.Linq.dll"
#endif

    open System
    open System.Data
    open System.IO
    open System.Data
    open System.Data.SqlClient
    open FSharp
    open FSharp.Data
    open FSharp.Data.SqlClient
    open FSharp.IO
    open Microsoft.FSharp.Data.TypeProviders
    open Xunit


    [<Literal>]
    let connectionstring = "Data Source=localhost;Initial Catalog=Master;Integrated Security=SSPI;"

    type LacjamPage = HtmlProvider<"http://www.lacjam.local/">
   
    type RestoreQuery = SqlProgrammabilityProvider<connectionstring>
       
    [<Fact>]
    let ``Create bundler file`` () =  
                                
                                try
                                    LacjamPage.Load("http://www.lacjam.local/").Html.Elements["script"]
                                    |> printfn "%A"
                                with
                                    | exn -> printfn "Exception:\n%s" exn.Message