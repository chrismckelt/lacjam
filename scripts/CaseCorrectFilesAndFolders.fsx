open System
open System.Collections.Concurrent
open System.Collections.Generic
open System.IO
open System.Net
open System.Text.RegularExpressions
open System.Linq

module Capitalise =
    let rootFolder = @"C:\dev\Lacjam\src\WebClient\"
    let rec fix folder =
        Directory.EnumerateFiles(folder)
        |> Seq.iter (fun b-> if (Path.GetFileName(b).ToCharArray().First().ToString()) <> (Path.GetFileName(b).ToCharArray().First().ToString().ToLowerInvariant()) then
                                    //Console.WriteLine(Path.GetFileName(b))
                                    let renamed = String.Concat((Path.GetFileName(b).ToCharArray().First().ToString().ToLowerInvariant()),(Path.GetFileName(b).Substring(1)))
                                    // Console.WriteLine (Path.Combine(Path.GetDirectoryName(b),renamed))
                                    let temp = Path.Combine(Path.GetDirectoryName(b),"___"+renamed)
                                    let good = Path.Combine(Path.GetDirectoryName(b),renamed)
                                    File.Move(b, temp)
                                    File.Move(temp, good)
                                    Console.WriteLine good
                             
                                 
                    )

        Directory.EnumerateDirectories(folder)
        |> Seq.iter (fun b-> 
                            fix b 
                            Console.WriteLine("Dir:"+b)
                            )


    fix rootFolder