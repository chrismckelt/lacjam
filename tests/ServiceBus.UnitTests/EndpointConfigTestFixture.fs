module EndpointConfigTestFixture
open System
open System.Diagnostics
open Microsoft.FSharp.Control
open Xunit
open FsUnit.Xunit
open Lacjam
open Lacjam.Core
open Lacjam.Core.Domain
open Lacjam.ServiceBus

[<Fact>] 
let ``FSharp Test`` () =
    let investor = new Investor()
    investor.GivenName <- "chris"
    investor.Surname <- "mckelt"
    investor.ToString().Length |> should equal 13