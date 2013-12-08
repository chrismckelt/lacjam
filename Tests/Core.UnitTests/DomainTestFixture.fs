module DomainTestFixture
open System
open System.Diagnostics
open Microsoft.FSharp.Control
open Xunit
open FsUnit.Xunit
open Lacjam
open Lacjam.Core

[<Literal>]
let url = "http://www.bedlam.net.au"

type Message = int * AsyncReplyChannel<int>
let formatString = "Message number {0} was received. Message contents: {1}" 

let printThreadId note =
    // Append the thread ID.
    printfn "%d : %s" System.Threading.Thread.CurrentThread.ManagedThreadId note

[<Fact>] 
let ``FSharp Test`` () =
    Domain.z().ToString().Length |> should equal 3

[<Fact>]
let ``Bedlam site scrape`` () =
    let wtf = JobNames.SiteScraperJob
    Debug.WriteLine(wtf.ToString())
    match wtf with 
    | _-> () 
    let siteRetriever = Lacjam.Core.Jobs.SiteScraper("Bedlam", url) :> Lacjam.Core.Jobs.IAmAJob
    let result = siteRetriever.Execute
    Debug.WriteLine(siteRetriever.Name)
    Debug.WriteLine(result)

[<Fact>]
let ``mailbox test`` () =
    let agent = MailboxProcessor<Message>.Start(fun aaa ->        
        async {
            try
                let! (message, replyChannel) = aaa.Receive();
                printThreadId "MailboxProcessor" 
                if (message > 5) then
                    replyChannel.Reply(message+1)
                else
                    replyChannel.Reply(message+10)
             with
            | :? Exception as x -> printf "%s" x.Message
        })
        

    let mutable count = 0
    let reply = agent.PostAndReply(fun replyChannel -> 15, replyChannel)
    Console.WriteLine("Reply: %s" + reply.ToString())

//    while count < 0 do
//        let input = count
//        printThreadId("Console loop")
//        let reply = agent.PostAndReply(fun replyChannel -> input, replyChannel)
//        if (reply > 3) then
//            Console.WriteLine("Reply: %s" + reply.ToString())
//        else
//            ()
//        count = count+1 
//        |> ignore


