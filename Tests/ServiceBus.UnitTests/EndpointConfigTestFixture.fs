module EndpointConfigTestFixture
open System
open System.Diagnostics
open Microsoft.FSharp.Control
open Xunit
open FsUnit.Xunit
open Lacjam
open Lacjam.Core
open Lacjam.ServiceBus

[<Fact>] 
let ``FSharp Test`` () =
    Domain.z().ToString().Length |> should equal 3