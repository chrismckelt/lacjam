namespace Lacjam.Core.UnitTests

open System
open Lacjam.Core.Domain.MetadataDefinitionGroups
open Lacjam.Core.Domain.MetadataDefinitionGroups.Events
open Lacjam.Framework.Events
open Lacjam.Framework.Model
open Microsoft.FSharp
open Xunit
open FsUnit.Xunit

module AggregateReplayTestFixture = 

    [<Fact>]
    let ``When loading an aggregate from stream it should not fail`` () =

    let identity = Guid.NewGuid()
    let event = new MetadataDefinitionGroupCreatedEvent( identity, new MetadataDefinitionGroupName("Metadata Definition Group Name"), new MetadataDefinitionGroupDescription("Description") )

    let events = Seq.ofList <| (event :> IEvent) :: []

    let aggregate = AggregateBuilder.LoadFromEvents<MetadataDefinitionGroup>(identity, events)

    ()