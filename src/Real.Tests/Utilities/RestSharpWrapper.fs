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


open RestSharp

module RestSharpWrapper =

    [<Literal>]
    let serviceBaseUri = "http://www.Lacjam.local/api/" // @"http://ec2-54-206-31-126.ap-southeast-2.compute.amazonaws.com:9090/api/" 
    //let serviceBaseUri = @"http://ec2-54-206-31-126.ap-southeast-2.compute.amazonaws.com:9090/api/" 

    // -------------------- //
    // Public data types    //
    // -------------------- //
    type Username = Username of string
    type Password = Password of string
    type Credentials = Username * Password

    type QueryState = {
        Credentials: Credentials option
    }

    // --------------------- //
    // public data types   //
    // --------------------- //
    type public RestMethod = 
        | GET
        | POST
        | PUT
        | DELETE

    type public Request = {
        RestResource: string
        Method: RestMethod
        Content: string
        Data : Object
    }

    // -------------------- //
    // public functions   //
    // -------------------- //
    let public request = {
        RestResource = ""
        Method = GET
        Content = ""
        Data = null
    }

    let public ApiClient state = 
        let mutable client = new RestClient(serviceBaseUri)
        match state.Credentials with
        | Some(Username(u),Password(p)) ->
            printfn "Attempting authenticated connection"
            client.Authenticator <- new HttpBasicAuthenticator(u, p)            
        | _ -> printfn "Anonymous connection"
        client

    let public Get request state = 
        let client = state |> ApiClient
        let get = new RestRequest(resource=request.RestResource)
        client.Execute(request=get)

    let public Put request state json =
        let client = state |> ApiClient
        let mutable post = new RestRequest(("/" + request.RestResource), Method.PUT)
        post.RequestFormat <- DataFormat.Json
        post.UseDefaultCredentials <- true
       // post.AddHeader("Accept", "application/json")  |> ignore   
        post.AddHeader("Content-Type", "application/json; charset=utf-8")  |> ignore   
        //post.AddParameter(@"text\json", json, ParameterType.RequestBody) |> ignore              
       // post.AddHeader("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(String.Format("{0}:{1}", "anonymous",""))));
        post.AddBody(request.Data)   |> ignore 
        client.Execute(request=post) 

    let public Post request state json =
        let client = state |> ApiClient
        let mutable post = new RestRequest(("/" + request.RestResource), Method.POST)
        post.RequestFormat <- DataFormat.Json
        post.UseDefaultCredentials <- true
       // post.AddHeader("Accept", "application/json")  |> ignore   
        post.AddHeader("Content-Type", "application/json; charset=utf-8")  |> ignore   
        //post.AddParameter(@"text\json", json, ParameterType.RequestBody) |> ignore              
       // post.AddHeader("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(String.Format("{0}:{1}", "anonymous",""))));
        post.AddBody(request.Data)   |> ignore   
       
        client.Execute(request=post)

    let public Delete request state = 
        let client = state |> ApiClient
        let delete = new RestRequest(request.RestResource, Method.DELETE)
        client.Execute(request=delete)

    let public RestfulResponse (x:Request->Request) s = 
        let r = x(request)
        match r.Method with
        | PUT -> Put r s r.Content
        | POST -> Post r s r.Content
        | DELETE -> Delete r s
        | _ -> Get r s