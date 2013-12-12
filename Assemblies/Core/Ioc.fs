namespace Lacjam.Core 
    open System
    open Autofac
    open NServiceBus
    open NServiceBus.Features
    open Lacjam.Core
    open Lacjam.Core.Jobs
    module Ioc = 
    let Container = 
                    let cb = new ContainerBuilder()
                    let con = cb.Build()
                    con :> IContainer


