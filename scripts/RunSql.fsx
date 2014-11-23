#r "System.dll"
#r "System.Data.dll"
#r "FSharp.Data.TypeProviders.dll"
#r "System.Data.Linq.dll"
//          http://msdn.microsoft.com/en-us/library/hh361033.aspx
open System
open System.Data
open System.Data.Linq
open Microsoft.FSharp.Data.TypeProviders
open Microsoft.FSharp.Linq
// test         = data source=Lacjam.csae43ljslde.ap-southeast-2.rds.amazonaws.com;Initial Catalog=Lacjam.Passport2;User Id=Lacjam.Passport2.Api;Password=DvcSL1dbzjvVqvjremA3;
// production   = 
type dbSchema = SqlDataConnection<"data source=Lacjam.csae43ljslde.ap-southeast-2.rds.amazonaws.com;Initial Catalog=Lacjam.Passport2;User Id=Lacjam.Passport2.Api;Password=DvcSL1dbzjvVqvjremA3;">
let db = dbSchema.GetDataContext()
// Enable the logging of database activity to the console.
//db.DataContext.Log <- System.Console.Out
let query1 = 
    query {
        for row in db.Event do
        //order by row.ClientEventId
        select row
        

    }
query1 |> Seq.iter (fun row -> printfn "%s" (row.ClientEventId.ToString()))

