module Lacjam.Framework.UnitTests.MaybeTestFixture

open System
open Lacjam.Framework.FP
open Xunit

[<Fact>]
let ``When mapping over a maybe it should only update the contained value when it is a Just`` () =
    
    let value = new Just<int>(5)
    let updated = value.Fmap( fun x -> x + 5)
    let expected = 10
    let result = value.Fold( (fun x -> x + 5), (fun () -> 0))

    Assert.Equal (expected, result)
    
[<Fact>]
let ``When mapping over a maybe it should not update the contained value when it is a None`` () =
    
    let value = new None<int>()
    let updated = value.Fmap( fun x -> x + 5)
    let expected = 0
    let result = value.Fold( (fun x -> x + 5), (fun () -> 0))

    Assert.Equal (expected, result)

[<Fact>]
let ``When binding over a maybe it should not update the contained value when it is a None`` () =
    
    let value = new None<int>()
    let updated = value.Bind( fun x -> new Just<int>(x + 5) :> IMaybe<int>)
    let expected = 0
    let result = value.Fold( (fun x -> x + 5), (fun () -> 0))

    Assert.Equal (expected, result)

[<Fact>]
let ``When binding over a maybe it should update the contained value when it is a Just`` () =
    
    let value = new Just<int>(5)
    let updated = value.Bind( fun x -> new Just<int>(x + 5) :> IMaybe<int>)
    let expected = 10
    let result = value.Fold( (fun x -> x + 5), (fun () -> 0))

    Assert.Equal (expected, result)