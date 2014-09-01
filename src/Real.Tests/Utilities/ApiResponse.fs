
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


module ApiResponse = 

    open System.Net
    open Newtonsoft.Json
    open RestSharp
        

    // -------------------- //
    // Public data types    //
    // -------------------- //
    type Error = {
        [<field: JsonProperty(PropertyName="resource")>] 
        Resource : string
        [<field: JsonProperty(PropertyName="field", Required=Required.Default)>]
        Field : string
        [<field: JsonProperty(PropertyName="code", Required=Required.Default)>] 
        Code : string
    }

    type Message = {
        [<field: JsonProperty(PropertyName="message")>] 
        Message : string
        [<field: JsonProperty(PropertyName="errors", Required=Required.Default)>]
        Errors : Error array
    }

    type ResponseStatus = 
        | Completed
        | ErrorResponse of string
        | Timeout
        | Aborted
        | NoResponse

    type ResponseContent<'T> = 
        | Content of 'T
        | Error of Message
        | NoContent

    type Response<'T> = {
        StatusCode          : HttpStatusCode
        StatusDescription   : string
        ResponseStatus      : ResponseStatus       
        ContentRaw          : string
        Content             : ResponseContent<'T> }

    let ConvertResponse<'T> (r:IRestResponse) = 
        let map = 
            match r.ResponseStatus with 
            | RestSharp.ResponseStatus.Completed -> Completed
            | RestSharp.ResponseStatus.TimedOut -> Timeout
            | RestSharp.ResponseStatus.Aborted -> Aborted
            | RestSharp.ResponseStatus.Error -> ErrorResponse(r.ErrorMessage)
            | RestSharp.ResponseStatus.None -> NoResponse
        { StatusCode          = r.StatusCode
          StatusDescription   = r.StatusDescription
          ResponseStatus      = map
          ContentRaw          = r.Content
          Content             = ResponseContent<'T>.NoContent }