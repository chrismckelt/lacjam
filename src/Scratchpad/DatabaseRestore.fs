#if INTERACTIVE
namespace Database
#endif

module DatabaseRestore 


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

    [<Literal>]
    let backupfolder = @"C:\backup"


    let restoreSql dbname =  String.Format(
                                                    """  
                                                            ALTER DATABASE [{1}] 
                                                            SET SINGLE_USER
                                                           
                                                            WITH ROLLBACK IMMEDIATE
                                                           
                                                            RESTORE DATABASE [{1}] FROM DISK =  '{0}\{1}.bak'  WITH REPLACE


                                                            ALTER DATABASE [{1}] 
                                                            SET MULTI_USER
                                                            WITH ROLLBACK IMMEDIATE
                                                            
                                                    """,backupfolder,dbname)
   
    type RestoreQuery = SqlProgrammabilityProvider<connectionstring>
       
    [<Fact>]
    //[<Fact(Skip="")>]
    let ``Database restore`` () =  
                                
                                try
                                     Directory.GetFiles(backupfolder, "*", SearchOption.AllDirectories)   
                                     |> Seq.iter(fun x ->                                   
                                                        use conn = new SqlConnection(connectionstring)
                                                        conn.Open()
                                                        let formattedText = restoreSql(Path.GetFileNameWithoutExtension(x))
                                                        use cmd = new SqlCommand(formattedText, conn)
                                                        cmd.CommandType <- CommandType.Text
                                                        cmd.ExecuteNonQuery() |> ignore
                                                        conn.Close()
                                                 )                      
                                    |> printfn "%A"
                                with
                                    | exn -> printfn "Exception:\n%s" exn.Message
