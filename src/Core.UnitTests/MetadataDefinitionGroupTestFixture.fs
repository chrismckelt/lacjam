namespace Lacjam.Core.UnitTests

open System
open Xunit
open Lacjam.Core.Domain.MetadataDefinitionGroups
open Lacjam.Core.Domain.MetadataDefinitionGroups.Events

module MetadataDefinitionGroupTestFixture = 


    let createaggregrate () =
        let aggregateIdentity = Guid.NewGuid ()

        let aggregate = new MetadataDefinitionGroup(aggregateIdentity, new MetadataDefinitionGroupName ("Name"), new MetadataDefinitionGroupDescription ("Description"))

        aggregate

    [<Fact>]
    let ``When creating a new metadata definition group it should contain changes`` () = 

        let aggregate = createaggregrate ()

        Assert.NotEmpty (aggregate.GetUncommitedChanges () )

    [<Fact>]
    let ``When creating a new metadata definition group it should contain the correct event indicating the name changes have succeeded`` () = 

        let aggregate = createaggregrate ()
        let changes = aggregate.GetUncommitedChanges () 
        let evt = changes |> Seq.head 
        let event = evt :?> MetadataDefinitionGroupCreatedEvent
        
        Assert.Equal<string> ( "Name", (event.Name.Name) ) 

    [<Fact>]
    let ``When creating a new metadata definition group it should contain the correct event indicating the description changes have succeeded`` () = 

        let aggregate = createaggregrate ()

        let changes = aggregate.GetUncommitedChanges () 
        let evt = changes |> Seq.head 
        let event = evt :?> MetadataDefinitionGroupCreatedEvent
        
        Assert.Equal<string> ( "Description", (event.Description.Description) ) 

    [<Fact>]
    let ``When changing the name on a metadata definition group it should contain the correct event indicating the name changes have succeeded`` () = 

        let aggregate = createaggregrate ()
        aggregate.ChangeName ( new MetadataDefinitionGroupName("Change"))

        let changes = aggregate.GetUncommitedChanges () 
        let evt = changes |> Seq.skip 1 |>  Seq.head 
        let event = evt :?> MetadataDefinitionGroupNameChangedEvent
        
        Assert.Equal<string> ( "Change", (event.Name.Name) ) 

    [<Fact>]
    let ``When changing the description on a metadata definition group it should contain the correct event indicating the description changes have succeeded`` () = 

        let aggregate = createaggregrate ()
        aggregate.ChangeDescription ( new MetadataDefinitionGroupDescription("Change"))

        let changes = aggregate.GetUncommitedChanges () 
        let evt = changes |> Seq.skip 1 |>  Seq.head 
        let event = evt :?> MetadataDefinitionGroupDescriptionChangedEvent
        
        Assert.Equal<string> ( "Change", (event.Description.Description) ) 

    [<Fact>]
    let ``When deleting a metadata definition group it should contain the correct event indicating the changes have succeeded`` () = 

        let aggregate = createaggregrate ()
        aggregate.Delete ()

        let changes = aggregate.GetUncommitedChanges () 
        let evt = changes |> Seq.skip 1 |>  Seq.head 
        let event = evt :?> MetadataDefinitionGroupDeletedEvent
        
        Assert.NotNull (event)