namespace Lacjam.Core
    open System
    open System.Net
    open System.Net.Http
    open System.Linq
    open Lacjam
    open Lacjam.Core
    open HtmlAgilityPack


    [<AutoOpen>]
    module Utility =
         
            ///http://books.google.com.au/books?id=MH3-T2jGFsEC&pg=PA22&lpg=PA22&dq=daniel+mohl+nullcheck&source=bl&ots=a8vv40Iklg&sig=j21km-HFd3Jan9uqoIPIyBLzmvU&hl=en&sa=X&ei=Nma-UoGcO4XMkQXi8YDwDQ&ved=0CEMQ6AEwAw#v=onepage&q=daniel%20mohl%20nullcheck&f=false
        let NullCheck = function
            | v when v <> null -> Some v
            | _ -> None

        module MongoDB =
            let deleteAll<'a> (session) = ()//session.Advanced.DocumentStore.DatabaseCommands.DeleteByIndex("Raven/DocumentsByEntityName", (new IndexQuery(Query = ("Tag:" + typedefof<'a>.Name))),true)
            

        module Html  =

            let rec findNodesByClassName (node:HtmlNode, className:string) =
                match node with 
                    | null -> None
                    | _ ->  match node.HasChildNodes with 
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

      
      
      module HtmlAgilityPackFSharp =
            ///http://blog.codebeside.org/blog/2013/10/14/fsharp-for-screen-scraping/?utm_source=feedburner&utm_medium=feed&utm_campaign=Feed%3A+CodeBeside+%28Code+Beside%29
            open HtmlAgilityPack

            type HtmlNode with

                member x.FollowingSibling name =
                    let sibling = x.NextSibling
                    if sibling = null then
                        null
                    elif sibling.Name = name then
                        sibling
                    else
                        sibling.FollowingSibling name

                member x.FollowingSiblings name = seq {
                    let sibling = x.NextSibling
                    if sibling <> null then
                        if sibling.Name = name then
                            yield sibling
                        yield! sibling.FollowingSiblings name
                }

                member x.PrecedingSibling name =
                    let sibling = x.PreviousSibling
                    if sibling = null then
                        null
                    elif sibling.Name = name then
                        sibling
                    else
                        sibling.PrecedingSibling name

                member x.PrecedingSiblings name = seq {
                    let sibling = x.PreviousSibling
                    if sibling <> null then
                        if sibling.Name = name then
                            yield sibling
                        yield! sibling.PrecedingSiblings name
                }

            let parent (node : HtmlNode) =
                node.ParentNode

            let element name (node : HtmlNode) =
                node.Element name

            let elements name (node : HtmlNode) =
                node.Elements name

            let descendants name (node : HtmlNode) =
                node.Descendants name

            let descendantsAndSelf name (node : HtmlNode) =
                node.DescendantsAndSelf name

            let ancestors name (node : HtmlNode) =
                node.Ancestors name

            let ancestorsAndSelf name (node : HtmlNode) =
                node.AncestorsAndSelf name

            let followingSibling name (node : HtmlNode) =
                node.FollowingSibling name

            let followingSiblings name (node : HtmlNode) =
                node.FollowingSiblings name

            let precedingSibling name (node : HtmlNode) =
                node.PrecedingSibling name

            let precedingSiblings name (node : HtmlNode) =
                node.PrecedingSiblings name

            let inline innerText (node : HtmlNode) =
                node.InnerText

            let inline attr name (node : HtmlNode) =
                node.GetAttributeValue(name, "")

            let inline (?) (node : HtmlNode) name =
                attr name node

            let inline hasAttr name value node =
                attr name node = value

            let inline hasId value node =
                hasAttr "id" value node

            let inline hasClass value node =
                hasAttr "class" value node

            let inline hasText value (node : HtmlNode) =
                node.InnerText = value

            let createDoc html =
                let doc = new HtmlDocument()
                doc.LoadHtml html
                doc.DocumentNode

