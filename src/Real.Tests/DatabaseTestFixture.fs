namespace Lacjam.Real.Tests
open System
open System.Linq
open Microsoft.FSharp
open Xunit
open FsUnit.Xunit

#if DEBUG


open Lacjam.Core
open Lacjam.Core.Infrastructure
open Lacjam.Core.Infrastructure.Database
open Lacjam.Core.Infrastructure.Ioc
open Lacjam.Core.Domain.MetadataDefinitionGroups
open Lacjam.Core.Domain.MetadataDefinitionGroups.Events
open Lacjam.Framework.Events
open Lacjam.Framework.Storage
open Lacjam.Core.Infrastructure
open Lacjam.Core.Infrastructure.Ioc
open Lacjam.Core.Infrastructure.Database
open HibernatingRhinos.Profiler.Appender.NHibernate
open Lacjam.Database

module Database =
    
    [<Literal>]
    let connectionstring_local = "Data Source=(local)\SQL2012;Database=Lacjam; Integrated Security=SSPI;Connection Timeout=300"

    [<Literal>]
    let connectionstring_local_master = "Data Source=(local)\SQL2012;Database=Master; Integrated Security=SSPI;Connection Timeout=300"

    [<Literal>]
    let connectionstring_test = "Data Source=localhost;Database=Lacjam; Integrated Security=SSPI;"
    
    [<FactAttribute(Skip="")>]
    let ``Create local database from NHibernate mappings - seed 003_InitialSchema.sql until we hit production`` () = 
        let db = new ApplicationDatabase()
        let cq = db.WithConnectionString(connectionstring_local)
        cq.CreateDatabaseScript();
        ()

    [<FactAttribute(Skip="")>]
    let ``Create DB from scratch `` () = 
        Lacjam.Database.DatabaseRunner.CreateDatabase(connectionstring_local_master)
        ()

    [<FactAttribute(Skip="")>]
    let ``Run sql fluent migrations`` () = 
        Lacjam.Database.DatabaseRunner.MigrateToLatest(connectionstring_local)
        ()

#endif