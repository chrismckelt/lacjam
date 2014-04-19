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
    
 

    type LogMessage =
        | Debug of string
        | Info of string
        | Warn of string * Exception
        | Error of string * Exception * bool // bool true = alerts

    type ILogWriter =
        abstract member Write : LogMessage -> unit
        abstract member Debug : string -> unit
        abstract member Info : string -> unit
        abstract member Warn : string -> unit
        abstract member Error : string*Exception -> unit

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
//        do
//            if  File.Exists lFile then
//                log4net.Config.XmlConfigurator.Configure(new FileInfo(lFile)) |> ignore
//            else
//                //failwith "log4net.config not found"
//                Console.WriteLine("log4net.config not found")

        let writeLog (lvl:Level) str exn  =     let eveId = 100// match lvl.Name with | (Level.Debug.Name) -> 100 | Level.Info.Name -> 200  | Level.Warn.Name -> 300 | Level.Error.Name -> 400  
                                                let le = new LoggingEvent(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,_logger.Repository, "All", lvl,str, exn)
                                                le.Properties.Item("EventID") <- eveId
                                                _logger.Log(le)

        interface ILogWriter  with
            member x.Write lm  =
                match lm with
                | Debug(str) ->       writeLog Level.Debug str null                   
                | Info(str) ->        writeLog Level.Info str null
                | Warn(str,e)->       writeLog Level.Warn str e
                | Error(str,e,b) ->   writeLog Level.Error str e  //TODO b

            member x.Debug str = writeLog Level.Debug str null
            member x.Info str = writeLog Level.Info str null
            member x.Warn str = writeLog Level.Warn str null
            member x.Error(str,exn)= writeLog Level.Error str exn


    let ContainerBuilder = 
                                let cb = new ContainerBuilder()
                                cb.Register(fun x -> new LogWriter()).As<ILogWriter>() |> ignore
                                cb.Register(fun x -> new ToDirectorySmtpBuilder()).As<ISmtpBuilder>() |> ignore
                                cb
    
    let Ioc =
//        ContainerBuilder.Register(fun a->
//                                          let store = new DocumentStore(Url = System.Configuration.ConfigurationManager.AppSettings.Item("RavenDBUrl"))
//                                          store.DefaultDatabase <- "Lacjam"
//                                          store.Initialize() |> ignore
//                                          store  
//                                 ).As<IDocumentStore>().SingleInstance() |> ignore
//
//        ContainerBuilder.Register(fun a -> a.Resolve<IDocumentStore>().OpenSession())
//           .As<IDocumentSession>()
//           .InstancePerLifetimeScope()
//           .OnRelease(fun b ->
//                       // When the scope is released, save changes
//                       //  before disposing the session.
//                       b.SaveChanges() |> ignore
//                       b.Dispose()  |> ignore
//            ) |> ignore
    

        let con = ContainerBuilder.Build()
        con