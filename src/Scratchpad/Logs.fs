#if INTERACTIVE

#else
module Logs
#endif
    #if INTERACTIVE
    
    #r "System.Data.dll"
    #r "System.Data.Linq.dll"
    #r @"FSharp.Core.dll"
    #r @"C:\dev\Mercury.Scripts\packages\FSharp.Data.2.1.0\lib\net40\FSharp.Data.dll"
    #r "FSharp.Data.TypeProviders.dll"
    #r @"C:\dev\Mercury.Scripts\packages\FSharpx.Core.1.8.41\lib\40\FSharpx.Core.dll"    

    #endif

    open System
    open System.Data
    open System.Linq
    open System.IO
    open Microsoft.FSharp.Data.TypeProviders
    open Microsoft.FSharp.Linq
    open FSharpx.JSON
    open FSharpx.DataStructures
    open FSharp.Data


    [<Literal>]
    let connectionString = "Data Source=localhost; Initial Catalog=LogRepository.local; Integrated Security=True;"

    type Sql = SqlDataConnection<connectionString>
    let db = Sql.GetDataContext()

    // Enable the logging of database activity to the console.
    //db.DataContext.Log <- System.Console.Out

    let query1 =
            query {
                for row in db.LogRepository_Logs do
                where (row.ApplicationId.Contains("Local Mercury ContractSigningProcess ProcessManager"))
                select row
            }
    let row = query1 |> Seq.head


    [<Literal>]
    let lightbluepath = @"C:\Users\chris.mckelt\AppData\Local\LightBlue\"
    let files = Directory.GetFiles(lightbluepath,row.ApplicationId,SearchOption.AllDirectories)
    if (files <> null)  then
        let content = System.IO.File.ReadAllText (files.First())
        printfn "%s" content


    //[<Literal>]
    //let initschema = """ {"Name":"Cairns","StoreNumber":0,"FranchiseId":"69dd128d-8e55-4820-a3b2-d47c877a204b","Enabled":true,"StoreTimeZone":"E. Australia Standard Time","TestStore":false,"Jurisdiction":5,"SequenceId":21,"MessageId":"dfffbc67-b036-427c-b185-68e11acd0ed8","AggregateId":"4356ad82-9ef1-4bf5-b7a8-b0e6bd3ac5ba","Timestamp":"2014-12-05T15:37:23.5478016+08:00","CorrelationId":"dfffbc67-b036-427c-b185-68e11acd0ed8","CausationId":"00000000-0000-0000-0000-000000000000","InstigatorId":"47d3c6db-b2c2-4146-b3a4-0d8dc4be001b","InstigatorType":"InitialState","InstigatorName":"InitialState"}  """
    //
    //type Values = JsonProvider<initschema>
    //Values.GetSample().
