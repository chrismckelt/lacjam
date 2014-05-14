namespace Lacjam.Modules

module SchedulerStats =
    open HtmlAgilityPack
    open Lacjam
    open Lacjam.Core
    open Lacjam.Core.Domain
    open Microsoft.FSharp
    open Newtonsoft.Json
    open System
    open System.Diagnostics
    open System.Linq
    open System.Net.Mail
    open System.Text
    open log4net

    let Statistics = ()