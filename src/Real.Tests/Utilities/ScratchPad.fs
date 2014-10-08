namespace Lacjam.Real.Tests
open System
open System.Configuration
open System.Linq
open System.Data.Sql
open System.Reflection
open Microsoft.FSharp
open Xunit
open FsUnit.Xunit

open System.Text

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
open TypeLite.TsModels
open Lacjam.WebApi.Controllers.MetadataDefinitionGroup
open Lacjam.Framework.Extensions

module ScratchPad =
    
   // let getTypes (defs:TypeScriptFluent) = defs.(fun (TypeLite.TsModels.IMemberIdentifier identifier) -> Char.ToLower(identifier.Name[0]) + identifier.Name.Substring(1)).WithVisibility(vis))
    let appmodel = "app.model";
    let olddomain = " Lacjam.WebApi";
    let sb = new StringBuilder();
    let any = "any";

    let gf = (fun (word:TypeLite.TsModels.IMemberIdentifier) -> word.Name.Substring(0, 1).ToLowerInvariant() +  word.Name.Substring(1))

    let customModuleNameFormatter modulename = appmodel
    let customTypeConverter (tsType:TsType) (formatter:ITsTypeFormatter) = tsType.ClrType.Name
    let customMemberTypeConverter (memberTypeName:string) (isMemberCollection:bool)  =  match isMemberCollection with | true -> System.String.Format("Array<{0}>", memberTypeName) | false -> any
    let customTypeVisiblityFormatter typename = true
    let customMemberIdentifierConverter (identifier:IMemberIdentifier) = (identifier.Name.ToCamelCase())

    let getTsModel =
        let builder = TypeScript.Definitions().For<Lacjam.WebApi.Controllers.MetadataDefinitionGroup.MetadataDefinitionGroupResource>().ModelBuilder
        let model = builder.Build()
        model

    let getTsGenerator =
        let target = new TsGenerator()
        target.AddReference("_references.ts")
        target.RegisterIdentifierFormatter(new TsMemberIdentifierFormatter(customMemberIdentifierConverter)) |> ignore
        target.RegisterMemberTypeFormatter(new TsMemberTypeFormatter(customMemberTypeConverter))
        target.RegisterTypeFormatter(new TsTypeFormatter(customTypeConverter))
        target.RegisterModuleNameFormatter(new TsModuleNameFormatter(customModuleNameFormatter))
        target.RegisterTypeVisibilityFormatter(new TsTypeVisibilityFormatter(customTypeVisiblityFormatter))
        target

    let createTypeScriptEnums =
        let model = getTsModel
        let target = getTsGenerator
        let enums = target.Generate(model, TsGeneratorOutput.Enums)
        sb.AppendLine(enums) |> ignore
        ignore

    let createTypeScriptClasses =
        let model = getTsModel
        let target = getTsGenerator
        let classes = target.Generate(model, TsGeneratorOutput.Classes)
        sb.AppendLine(classes) |> ignore 
        ignore 

    let registerClasses =
        let model = getTsModel
        model.Classes
        |> Seq.iter(fun a-> 
                (
                            let ns = String.Format("{0}.{1}", appmodel, a.Name)
                            let js = String.Format("app.registerValue('{0}',{1});", a.ClrType.Name.ToCamelCase(),ns)
                            sb.AppendLine(js) |> ignore
                            ()
                )
         )

    let createExtraFixups =
        let output = sb.ToString().Replace("interface", "class")
        output.Replace("implements", "extends")   |> ignore 
        output.Replace("`1", "")  |> ignore 
        output
    
    [<Fact>]
    let GenerateTypeScriptModel() =
        createTypeScriptEnums() |> ignore
        createTypeScriptClasses() |> ignore
        registerClasses |> ignore
        let output = createExtraFixups;
        Console.WriteLine(@"/// Model.ts");
        Console.WriteLine(output);
    

   
