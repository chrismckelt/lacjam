namespace Lacjam.Core 
    open System
    open Autofac
    open NServiceBus
    open NServiceBus.Features
    open Lacjam.Core
    open Lacjam.Core.Jobs
    module Ioc  = 
        let ContainerBuilder = new ContainerBuilder()
        let Container = ContainerBuilder.Build()

