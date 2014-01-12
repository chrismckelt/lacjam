namespace Lacjam.Core
    open System
    open System.Net
    open System.Net.Http
    open System.Linq
    open Lacjam
    open Lacjam.Core
    open HtmlAgilityPack

    module Utility =
         
         ///http://books.google.com.au/books?id=MH3-T2jGFsEC&pg=PA22&lpg=PA22&dq=daniel+mohl+nullcheck&source=bl&ots=a8vv40Iklg&sig=j21km-HFd3Jan9uqoIPIyBLzmvU&hl=en&sa=X&ei=Nma-UoGcO4XMkQXi8YDwDQ&ved=0CEMQ6AEwAw#v=onepage&q=daniel%20mohl%20nullcheck&f=false
        let NullCheck = function
            | v when v <> null -> Some v
            | _ -> None


        module Html  =

            let rec findNodesByClassName (node:HtmlNode, className:string) =
                match node.HasChildNodes with 
                            | false -> None
                            | true  ->  let o =  (node.ChildNodes) 
                                                    |> Seq.choose (fun x -> 
                                                                        match (x.Attributes.Item("class")) with
                                                                        | null -> None
                                                                        | z -> match z.Value.Contains(className) with | false -> None | true -> Some(z)
                                                                        )  
                                                    |> Seq.distinct
                                        if not <| (Seq.isEmpty o) then
                                            let head = Seq.head o
                                            Some(head)
                                        else
                                            None

