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

//[<Fact>] 
//let ``Investor.ToString should write name`` () =
//    let inv = new Investor()
//    inv.GivenName <- "chris"
//    inv.Surname <- "mckelt"
//    inv.ToString.Length |> should equal 12

//[<Fact>]
//let ``Bedlam site scrape`` () =
//    let wtf = JobType.SiteScraperType
//    Debug.WriteLine(wtf.ToString())
//    match wtf with 
//    | _-> () 
//    let siteRetriever = Lacjam.Core.Jobs.SiteScraper("Bedlam", url) :> JobBase
//    let result = siteRetriever.Execute
//    Debug.WriteLine(siteRetriever.Name)
//    Debug.WriteLine(result)

[<Fact>]
let ``Mailbox test`` () =
    let agent = MailboxProcessor<Message>.Start(fun aaa ->        
        async {
            try
                let! (message, replyChannel) = aaa.Receive()
                printThreadId "MailboxProcessor" 
                if (message > 5) then
                    replyChannel.Reply(message+1)
                else
                    replyChannel.Reply(message+10)
             with | ex -> printf "%s" ex.Message
        })
        

    let mutable count = 0
    let reply = agent.PostAndReply(fun replyChannel -> 15, replyChannel)
    Console.WriteLine("Reply: %s" + reply.ToString())


