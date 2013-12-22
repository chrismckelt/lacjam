namespace Lacjam.Core.Messages

module Utility =
    open System
    open System.Xml
    open System.Runtime.Serialization.Json
    open System.IO
    open System.Text
    open NServiceBus
    open Lacjam.Core.Jobs

    /// Object to Json 
    let internal json<'t> (myObj:'t) =   
            use ms = new MemoryStream() 
            (new DataContractJsonSerializer(typeof<'t>)).WriteObject(ms, myObj) 
            Encoding.Default.GetString(ms.ToArray()) 


    /// Object from Json 
    let internal unjson<'t> (jsonString:string)  : 't =  
            use ms = new MemoryStream(ASCIIEncoding.Default.GetBytes(jsonString)) 
            let obj = (new DataContractJsonSerializer(typeof<'t>)).ReadObject(ms) 
            obj :?> 't


