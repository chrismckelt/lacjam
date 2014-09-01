namespace Lacjam.Real.Tests
open System
open System.Configuration
open System.Linq
open System.Data.Sql
open System.Reflection
open Microsoft.FSharp
open Xunit
open FsUnit.Xunit

#if DEBUG

open Lacjam.Core
open Lacjam.Core.Domain.MetadataDefinitionGroups
open Lacjam.Core.Domain.MetadataDefinitionGroups.Events
open Lacjam.Framework.Events
open Lacjam.Framework.Storage
open Lacjam.Core.Infrastructure
open Lacjam.Core.Infrastructure.Ioc
open Lacjam.Core.Infrastructure.Database
open Newtonsoft
open Newtonsoft.Json
open TypeLite
open TypeLite.Net4
open Lacjam.WebApi.Controllers.MetadataDefinitionGroup

module ScratchPad =
    
   // let getTypes (defs:TypeScriptFluent) = defs.(fun (TypeLite.TsModels.IMemberIdentifier identifier) -> Char.ToLower(identifier.Name[0]) + identifier.Name.Substring(1)).WithVisibility(vis))

    let gf = (fun (word:TypeLite.TsModels.IMemberIdentifier) -> word.Name.Substring(0, 1).ToLowerInvariant() +  word.Name.Substring(1))

    [<FactAttribute(Skip="")>]
    let ``Generate typescript files from model`` () =
    
        let vis = new TsTypeVisibilityFormatter(fun st -> st.Contains("export"))
        let ass = Assembly.GetAssembly(typedefof<Lacjam.WebApi.Controllers.MetadataDefinition.MetadataDefinitionController>)
        let defs = TypeScript.Definitions().For(ass).WithFormatter(gf)
        let ts = defs.WithVisibility(vis)

        let enums = ts.Generate(TsGeneratorOutput.Enums);
        Console.WriteLine(enums)
        let classes = ts.Generate(TsGeneratorOutput.Classes);
        Console.WriteLine(classes);         

    ()

    [<FactAttribute(Skip="")>]
    let ``Generate JSON `` () =
    
      let obj = new MetadataDefinitionGroupResource()
      obj.Description <- "DDD"
      obj.Name <- "nnnn"
      obj.Identity <- Guid.NewGuid()
      let json = JsonConvert.SerializeObject(obj)
      Console.WriteLine(json)

    ()

    

#endif