#if INTERACTIVE
namespace Database
#endif

module DatabaseBackup 


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
   
    [<Literal>]
    let backupSql =   @"  
                    DECLARE UserDatabases_CTE_Cursor Cursor
                    FOR
                    select name as DatabaseName
                    from sys.sysdatabases
                    where ([dbid] > 4) and ([name] like '%.local%')
 
                    OPEN UserDatabases_CTE_Cursor
                    DECLARE @dbName varchar(100)
                    DECLARE @backupPath varchar(100)
                    DECLARE @backupQuery varchar(500)
 
                    set @backupPath = 'c:\backup\'
                    print 'Backing up to '  + @backupPath

                    Fetch NEXT FROM UserDatabases_CTE_Cursor INTO @dbName
                    While (@@FETCH_STATUS <> -1)
 
                    BEGIN


                    set @backupQuery =  'backup database [' + @dbName + '] to disk = ''' + @backupPath + @dbName + '.bak'''
 
                    print @backupQuery
 
                    EXEC (@backupQuery)

                    Fetch NEXT FROM UserDatabases_CTE_Cursor INTO @dbName
                    END
 
                    CLOSE UserDatabases_CTE_Cursor
                    DEALLOCATE UserDatabases_CTE_Cursor
	

            "
    type BackupQuery = SqlCommandProvider<backupSql, connectionstring>

    [<Fact(Skip="")>]
    //[<Fact>]
    let ``Database backup`` () =  
                                try
                                    let cmd = new BackupQuery()
                                    cmd.Execute() 
                                    |> printfn "%A"
                                with
                                    | exn -> printfn "Exception:\n%s" exn.Message


    let restoreSql dbname =  String.Format(
                                                    """  
                                                            RESTORE FILELISTONLY
                                                            FROM DISK = '{0}\{1}.bak'
                                                   
                                                            ALTER DATABASE {1}
                                                            SET SINGLE_USER WITH
                                                            ROLLBACK IMMEDIATE

                                                            RESTORE DATABASE {1}
                                                            FROM DISK =  '{0}\{1}.bak'
                                                            WITH MOVE '{1}' TO '{0}\{1}.mdf',
                                                            MOVE '{1}' TO '{0}\{1}.ldf'

                                                            ALTER DATABASE {1} SET MULTI_USER
                                                            GO
                                                    """,backupfolder,dbname)
   
   