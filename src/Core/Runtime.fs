namespace Lacjam.Core

[<AutoOpen>]
module Runtime =
    open System
    open System.IO
    open System.Net.Mail
    open System.Configuration       
    open Autofac
    open log4net
    open log4net.Core
    open NServiceBus
    open NServiceBus.Features
    open NServiceBus.Mailer
    open Lacjam.Core
    open Quartz
    open Quartz.Impl
    open Raven
    open Raven.Client
    open Raven.Client.Connection 
    open Raven.Client.Document

    type LogMessage =
        | Debug of string
        | Info of string
        | Warn of string * Exception
        | Error of string * Exception * bool // bool true = alerts

    type ILogWriter =
        abstract member Write : LogMessage -> unit

    [<Literal>]
    let mailDir = @"c:\temp\mails"

    type ToDirectorySmtpBuilder() =
                       interface ISmtpBuilder with
                            member x.BuildClient() = new SmtpClient(DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,PickupDirectoryLocation = mailDir)
                       

    let t = Lacjam.Core.Domain.Investor()
    let private _logger = log4net.LogManager.GetLogger(t.GetType()).Logger

    type LogWriter() =
        let lFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"log4net.config")
        let excludeFromLoggingList = ["Polling for timeouts";"Polling next retrieval is"]
        let exclude str list  = List.exists (fun elem -> elem = str) list
        do
            if  File.Exists lFile then
                log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(lFile)) |> ignore
            else
                //failwith "log4net.config not found"
                Console.WriteLine("log4net.config not found")

        interface ILogWriter  with
            member x.Write lm  =
                match lm with
                | Debug(s) ->
                    let le = new LoggingEvent(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,_logger.Repository, lm.GetType().Name, log4net.Core.Level.Debug,s, null)
                    if not <| (exclude s excludeFromLoggingList) then
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

    let ContainerBuilder = 
                                let cb = new ContainerBuilder()
                                cb.Register(fun x -> new LogWriter()).As<ILogWriter>() |> ignore
                                cb.Register(fun x -> new ToDirectorySmtpBuilder()).As<ISmtpBuilder>() |> ignore
                                cb
    
    let Ioc =
        ContainerBuilder.Register(fun a->
                                          let store = new DocumentStore(Url = System.Configuration.ConfigurationManager.AppSettings.Item("RavenDBUrl"))
                                          store.DefaultDatabase <- "Lacjam"
                                          store.Initialize() |> ignore
                                          store  
                                 ).As<IDocumentStore>().SingleInstance() |> ignore

        ContainerBuilder.Register(fun a -> a.Resolve<IDocumentStore>().OpenSession())
           .As<IDocumentSession>()
           .InstancePerLifetimeScope()
           .OnRelease(fun b ->
                       // When the scope is released, save changes
                       //  before disposing the session.
                       b.SaveChanges() |> ignore
                       b.Dispose()  |> ignore
            ) |> ignore
    

        let con = ContainerBuilder.Build()
        con