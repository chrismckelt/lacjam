namespace Lacjam.Real.Tests
open System
open System.Linq
open System.Data.Sql
open Microsoft.FSharp
open Xunit
open FsUnit.Xunit

open Lacjam.Core
open Lacjam.Core.Domain.MetadataDefinitionGroups
open Lacjam.Core.Domain.MetadataDefinitionGroups.Events
open Lacjam.Framework.Events
open Lacjam.Framework.Storage
open Lacjam.Core.Infrastructure
open Lacjam.Core.Infrastructure.Ioc
open Lacjam.Core.Infrastructure.Database
open HibernatingRhinos.Profiler.Appender.NHibernate
open NHibernate.Context;
open NHibernate

open Lacjam.Core.Infrastructure.Storage
open Lacjam.Core.Infrastructure.Database
open Lacjam.Core.Infrastructure.Ioc
open Lacjam.Core.Infrastructure.Ioc.Convo

open Castle.MicroKernel.Lifestyle
open Castle.MicroKernel
open Lacjam.Core.Infrastructure.Ioc

module EventPersistenceTestFixture =
    

    let bindSession (sf : ISessionFactory) =
        let session = sf.OpenSession()
        CurrentSessionContext.Bind(session)
        sf.GetCurrentSession();

    let unbind (sf: ISessionFactory) =
        if CurrentSessionContext.HasBind(sf) then
            let session = CurrentSessionContext.Unbind(sf)
            session.Flush()
            session.Dispose()
            ()
        else
            ()


    [<Fact>]
    let ``When attempting to persist an event it should be persisted to the storage mechanism`` () =

        NHibernateProfiler.Initialize();

        let identity = Guid.NewGuid()
        let event = new MetadataDefinitionGroupCreatedEvent( identity, new MetadataDefinitionGroupName("Metadata Definition Group Name"), new MetadataDefinitionGroupDescription("Description") )
        let container = WindsorAccessor.Instance.WithSessionManagement().WithConversationPerThread().Container
        container.BeginScope() |> ignore
        let sessionfactory = container.Resolve<ISessionFactory>()
        let uow = container.Resolve<Lacjam.Core.Infrastructure.Ioc.Convo.IUnitOfWork>()
        uow.Start()      
        let storage = new EventStore(sessionfactory)
        storage.SaveStream (identity, Seq.ofList <| (event :> IEvent) :: [], 0)
        uow.End()
        ()

