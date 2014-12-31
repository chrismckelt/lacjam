module Mercury 

    open System
    open System.Data
    open System.IO
    open System.Data
    open System.Data.SqlClient
    open System.Linq
    open FSharp
    open FSharp.Data
    open FSharp.Data.SqlClient
    open FSharp.IO
    open Microsoft.FSharp.Data.TypeProviders
    open Xunit

    [<Literal>]
    let connectionstring = "Data Source=localhost;Initial Catalog=CommandHub.Local;Integrated Security=SSPI;"

    [<Literal>]
    let rootFolder = @"C:\Users\chris.mckelt\AppData\Local\LightBlue\dev\blob\messages\"

    [<Literal>]
    let query = "
                select blobname
                from [MessageHub].[Message]
                where [AggregateTypeId] = 7
                and SequenceId = (select max(SequenceId) from [MessageHub].[Message] where [AggregateTypeId] = 7 )
                " 

    type NotificationQuery = SqlCommandProvider<query, connectionstring>
   
    [<Fact>]
    let ``Sms Code`` () =  let cmd = new NotificationQuery()
                           let filename = cmd.Execute().First()
                           let text = File.ReadAllText(Path.Combine(rootFolder,filename))
                           printfn "%A" text
