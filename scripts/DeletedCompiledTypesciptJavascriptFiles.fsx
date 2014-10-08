open System
open System.Collections.Concurrent
open System.Collections.Generic
open System.IO
open System.Net
open System.Text.RegularExpressions
open System.Linq

module Capitalise =
    let rootFolder = @"C:\dev\STRUCterre.MetaStore\src\WebClient\App\"
    let rec fix folder =
        Directory.EnumerateFiles(folder)
        |> Seq.iter (fun file-> if (Path.GetExtension(file)=".js") then
                                    //Console.WriteLine(Path.GetFileName(b))
                                    let dir = Path.GetDirectoryName(file)
                                    let ts = Path.Combine(dir,Path.GetFileNameWithoutExtension(file)+".ts")
                                    if (File.Exists(ts)) then
                                        File.Delete(file)                                  
                                        File.Delete(Path.Combine(dir,Path.GetFileNameWithoutExtension(file)+".js.map"))
                               
                                 
                    )

        Directory.EnumerateDirectories(folder)
        |> Seq.iter (fun b-> 
                            fix b 
                            Console.WriteLine("Dir:"+b)
                            )


    fix rootFolder