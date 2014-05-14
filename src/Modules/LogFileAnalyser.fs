namespace Lacjam.Modules

module LogFileAnalyser =
    open System
    open System.IO
    open System.Collections.Generic

    let outputPath = @"c:\\temp\logs\"

    let rec directoryCopy srcPath dstPath copySubDirs =

        if not <| System.IO.Directory.Exists(srcPath) then
            let msg = System.String.Format("Source directory does not exist or could not be found: {0}", srcPath)
            raise (System.IO.DirectoryNotFoundException(msg))

        if not <| System.IO.Directory.Exists(dstPath) then
            System.IO.Directory.CreateDirectory(dstPath) |> ignore

        let srcDir = new System.IO.DirectoryInfo(srcPath)

        for file in srcDir.GetFiles() do
            let temppath = System.IO.Path.Combine(dstPath, file.Name)
            file.CopyTo(temppath, true) |> ignore

        if copySubDirs then
            for subdir in srcDir.GetDirectories() do
                let dstSubDir = System.IO.Path.Combine(dstPath, subdir.Name)
                directoryCopy subdir.FullName dstSubDir copySubDirs

    let getLogs (kvp:KeyValuePair<string,string>) = 
        async  {
            let np = Path.Combine(outputPath, kvp.Key)
            Directory.CreateDirectory kvp.Value |> ignore
            directoryCopy kvp.Value np true
        }

    let rec deleteFiles srcPath =
    
        if not <| System.IO.Directory.Exists(srcPath) then
            let msg = System.String.Format("Source directory does not exist or could not be found: {0}", srcPath)
            raise (System.IO.DirectoryNotFoundException(msg))

        for file in System.IO.Directory.EnumerateFiles(srcPath) do
            let tempPath = System.IO.Path.Combine(srcPath, file)
            System.IO.File.Delete(tempPath)

        let srcDir = new System.IO.DirectoryInfo(srcPath)
        for subdir in srcDir.GetDirectories() do
            deleteFiles subdir.FullName 


    let logFolders = new System.Collections.Generic.Dictionary<string, string>()
    logFolders.Add("225-Admin", @"\\vsydpapp225\d$\AdministrationSite\logs") |> ignore
    logFolders.Add("225-Authentication", @"\\vsydpapp225\d$\AuthenticationSite\logs") |> ignore
    logFolders.Add("225-PhoenixMessageHandler", @"\\vsydpapp225\d$\Phoenix.MessageHandler\bin\Release\logs") |> ignore
    logFolders.Add("225-PhoenixOneVue", @"\\vsydpapp225\d$\Phoenix.NServiceBus.OneVue\bin\Release\logs") |> ignore
    logFolders.Add("225-PhoenixWCFBackend",@"\\vsydpapp225\d$\PhoenixWcfBackEnd\logs") |> ignore
    logFolders.Add("225-QuoteFacade",@"\\vsydpapp225\d$\QuoteFacade\logs") |> ignore
    logFolders.Add("225-STSMessageHandler",@"\\vsydpapp225\d$\StsMessageHandler\logs") |> ignore
    logFolders.Add("226-Admin", @"\\vsydpapp226\d$\AdministrationSite\logs") |> ignore
    logFolders.Add("226-Authentication", @"\\vsydpapp226\d$\AuthenticationSite\logs") |> ignore
    logFolders.Add("226-PhoenixMessageHandler", @"\\vsydpapp226\d$\Phoenix.MessageHandler\bin\Release\logs") |> ignore
    logFolders.Add("226-PhoenixOneVue", @"\\vsydpapp226\d$\Phoenix.NServiceBus.OneVue\bin\Release\logs") |> ignore
    logFolders.Add("226-PhoenixWCFBackend",@"\\vsydpapp226\d$\PhoenixWcfBackEnd\logs") |> ignore
    logFolders.Add("226-QuoteFacade",@"\\vsydpapp226\d$\QuoteFacade\logs") |> ignore
    logFolders.Add("226-STSMessageHandler",@"\\vsydpapp226\d$\StsMessageHandler\logs") |> ignore


    let collect() = (
        try
            if System.IO.Directory.Exists(outputPath) then
                deleteFiles outputPath 
            else
                Directory.CreateDirectory(outputPath) |> ignore
        with
        | exn -> printfn "An exception occurred: %s" exn.Message

        logFolders 
        |> Seq.map getLogs 
        |> Async.Parallel 
        |> Async.RunSynchronously
        |> ignore
    )

    collect()