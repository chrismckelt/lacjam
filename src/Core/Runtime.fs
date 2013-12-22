namespace Lacjam.Core 

module Runtime =
    open System
    open System.IO
    open Autofac
    open log4net
    open log4net.Core
    open NServiceBus
    open NServiceBus.Features
    open Lacjam.Core

    type LogMessage = 
        | Debug of string 
        | Info of string
        | Warn of string * Exception
        | Error of string * Exception * bool // bool true = alerts

    type ILogWriter =  
        abstract member Write : LogMessage -> unit
        
    let t = new Lacjam.Core.Domain.Result()
    let private _logger = log4net.LogManager.GetLogger(t.GetType()).Logger

    type LogWriter() =
        let lFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"log4net.config")
        do 
            if  File.Exists lFile then 
                log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(lFile)) |> ignore
            else 
                failwith "log4net.config not found"
             
        interface ILogWriter  with
            member x.Write lm  = 
                match lm with 
                | Debug(s) -> 
                    let le = new LoggingEvent(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,_logger.Repository, lm.GetType().Name, log4net.Core.Level.Debug,s, null)
                    printfn "%A" lm
                    le.Properties.Item("EventID") <- 100
                    _logger.Log(le)
                | Info(s) -> 
                    let le = new LoggingEvent(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,_logger.Repository, lm.GetType().Name, log4net.Core.Level.Debug,s, null)
                    printfn "%A" lm
                    le.Properties.Item("EventID") <- 200
                    _logger.Log(le)
                | Warn(s,e)-> 
                    let le = new LoggingEvent(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,_logger.Repository, lm.GetType().Name, log4net.Core.Level.Debug,s, e)
                    printfn "%A" lm
                    le.Properties.Item("EventID") <- 300
                    _logger.Log(le)
                | Error(s,e,b) -> 
                    let le = new LoggingEvent(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,_logger.Repository, lm.GetType().Name, log4net.Core.Level.Debug,s, e)
                    printfn "%A" lm
                    le.Properties.Item("EventID") <- 400
                    _logger.Log(le)
                    //TODO alert B
                
    let Ioc = 
        let cb = new ContainerBuilder()
        cb.Register(fun x -> new LogWriter()).As<ILogWriter>() |> ignore
        let con = cb.Build()
        con 
     
