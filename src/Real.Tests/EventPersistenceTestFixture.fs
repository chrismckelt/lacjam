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

open Lacjam.Core.Infrastructure.Storage
open Lacjam.Core.Infrastructure.Database
open Lacjam.Core.Infrastructure.Ioc

open Castle.MicroKernel.Lifestyle
open Castle.MicroKernel


module EventPersistenceTestFixture =
    



    [<Fact>]
    let ``When attempting to persist an event it should be persisted to the storage mechanism`` () =

        ()

