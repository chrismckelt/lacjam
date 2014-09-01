namespace Lacjam.Framework.UnitTests
open System
open System.Linq
open Microsoft.FSharp
open FsUnit.Xunit
open Xunit
open Lacjam.Framework.Hash

module HashTestFixture =

    [<Literal>]
    let password = "this-is-my-password"

    [<Literal>]
    let hashed = "$2a$10$NrBk8OrsT8qduwDj6rBdFuauDyTTdggn5m01jaUGKxgij9tBScVd."  // from above

    //

    [<Fact>]
    let ``Hasher should transform clear text password to obsfucated string``() =
        let pw = PasswordHasher()
        pw.GetHash password
        |> should not' (equal password) 
    ()

    [<Fact>]
    let ``Hasher should verify unobsfucate encoded string``() =
        let pw = PasswordHasher();
        pw.Verify(password,hashed)
        |> should be True 
    ()