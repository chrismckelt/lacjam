module RuntimeTestFixture

open Autofac
open FsUnit.Xunit
open Lacjam
open Lacjam.Core
open Lacjam.Core.Runtime
open Microsoft.FSharp.Control
open System
module RuntimeTestFixture  =

    open System.Diagnostics
    open Xunit

    [<Literal>]
    let url = "http://www.bedlam.net.au"

    type Message = int * AsyncReplyChannel<int>

    let formatString = "Message number {0} was received. Message contents: {1}"
    let printThreadId note =
        // Append the thread ID.
        printfn "%d : %s" System.Threading.Thread.CurrentThread.ManagedThreadId note

    [<Fact>]
    let ``IOC contains ILogWriter``() =
        let found = Lacjam.Core.Runtime.Ioc.Resolve<ILogWriter>()
        found |> should not' (Null)