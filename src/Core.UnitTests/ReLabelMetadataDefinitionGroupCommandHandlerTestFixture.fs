namespace Lacjam.Core.UnitTests

open System
open Xunit
open Lacjam.Core.Domain.MetadataDefinitionGroups
open Lacjam.Framework.Model
open Lacjam.Core.Domain.MetadataDefinitionGroups.Commands
open  Lacjam.Framework.FP

module ReLabelMetadataDefinitionGroupCommandHandlerTestFixture =


    [<Fact(Skip="build server")>]
    let ``When requesting to relabel a non existing metadata definition group it should pass a none instance to the repository`` () =

        let aggregateIdentity = Guid.NewGuid ()
        let tb = new Lacjam.Core.Infrastructure.TrackingBase() 
        let command = new ReLabelMetadataDefinitionGroupCommand ( aggregateIdentity, new MetadataDefinitionGroupName ("Name"), new MetadataDefinitionGroupDescription ("Description"), (tb) )
        let called = ref false
        let repository = { new IRepository<MetadataDefinitionGroup> with 
                                    member x.Get(identity : Guid) = new None<MetadataDefinitionGroup>() :> IMaybe<MetadataDefinitionGroup>
                                    member x.Save(aggregrate, false) = 
                                        aggregrate.OnEmpty (( fun () -> called := true ))
                                        ()
                           }

        let commandhandler = new ReLabelMetadataDefinitionGroupCommandHandler  (repository )
        commandhandler.Handle ( command) 

        Assert.True (!called)


    [<Fact(Skip="build server")>]
    let ``When requesting to relabel an existing metadata definition group it should pass a just instance to the repository`` () =

        let aggregateIdentity = Guid.NewGuid ()
        let tb = new Lacjam.Core.Infrastructure.TrackingBase() 
        let command = new ReLabelMetadataDefinitionGroupCommand ( aggregateIdentity, new MetadataDefinitionGroupName ("Name"), new MetadataDefinitionGroupDescription ("Description"),tb )
        let called = ref false
        let repository = { new IRepository<MetadataDefinitionGroup> with 
                                    member x.Get(identity : Guid) = 
                                        let aggregate = new MetadataDefinitionGroup(aggregateIdentity, new MetadataDefinitionGroupName("OriginalName"), new MetadataDefinitionGroupDescription("OriginalDesc" ))
                                        new Just<MetadataDefinitionGroup>( aggregate ) :> IMaybe<MetadataDefinitionGroup>
                                    member x.Save(aggregrate,false) = 
                                        aggregrate.OnNonEmpty (( fun x -> called := true ))
                                        ()
                           }

        let commandhandler = new ReLabelMetadataDefinitionGroupCommandHandler  (repository )
        commandhandler.Handle ( command) 

        Assert.True (!called)

        ()