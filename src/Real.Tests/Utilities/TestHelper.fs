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
open RestSharp
open System
open System.Net
open System.IO
open System.Text
open RestSharp
open Newtonsoft.Json
open ApiResponse
open RestSharpWrapper

module TestHelper =

    type User = {
        Username: string option
        Password: string option
    }

    let private toCredentials user = 
        match (user.Username, user.Password) with
        | Some(u),Some(p) -> Some(Username(u),Password(p))
        | Some(u),None ->
            printfn "No password supplied"
            None
        | None,Some(p) ->
            printfn "No username supplied"
            None
        | None,None -> None

    let public StartState = {
        Credentials = None
    }

    let AnonymousUser = {
        Username = None
        Password = None
    }

    let AuthenticatedUser = {
        Username = Some("test username")
        Password = Some("test password")
    }

    let DefaultState user =
        { StartState with
            Credentials = user |> toCredentials }


     // -------------------- //
    // Internal functions   //
    // -------------------- //
    let public DeserializeResponseContent (response:ApiResponse.Response<'T>) = 
        match response.StatusCode with            
        | HttpStatusCode.OK | HttpStatusCode.Created ->
            { response with Content = Content(JsonConvert.DeserializeObject<'T>(response.ContentRaw)) }
        | HttpStatusCode.Unauthorized ->
            { response with Content = Error(JsonConvert.DeserializeObject<Message>(response.ContentRaw)) }
        | _ -> { response with ResponseStatus = ErrorResponse(response.StatusDescription) }

    let internal SerializeToJson x = 
        JsonConvert.SerializeObject(x)


    let internal GetApiResponse<'T> state = 
        state 
        |> RestfulResponse 
        >> ConvertResponse<'T>

    let internal GetDeserializedApiResponse<'T> state = 
        state
        |> RestfulResponse
        >> ConvertResponse<'T>
        >> DeserializeResponseContent