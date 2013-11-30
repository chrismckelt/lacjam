module DomainTestFixture
open Xunit
open FsUnit.Xunit
open Lacjam
open Lacjam.Core

[<Fact>] 
let ``FSharp Test`` () =
    Domain.z().ToString().Length |> should equal 3