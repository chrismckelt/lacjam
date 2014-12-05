open System
open System.Collections.Concurrent
open System.Collections.Generic
open System.IO
open System.Net
open System.Text.RegularExpressions
open System.Linq

module EnsureJavascriptMinFilesOkForBundler =
    let rootFolder = @"C:\dev\Workspace\CherryByte Software General\Jaxon\src\WebClient\Scripts"
    let rec fix folder =
        Directory.EnumerateFiles(folder)
        |> Seq.iter (fun b-> if (Path.GetFileName(b).Contains(".min.")) then
                                    //Console.WriteLine(Path.GetFileName(b))
                                    let nf = b.Replace(".min", "") 
                                    if not <| (File.Exists(nf)) then
                                        File.Copy(b,nf)
                                        Console.WriteLine((Path.GetFileName(b)))
                                    
                                    ) 
                             
                                 
                    

        Directory.EnumerateDirectories(folder)
        |> Seq.iter (fun b-> 
                            fix b 
                            Console.WriteLine("Dir:"+b)
                            )


    fix rootFolder