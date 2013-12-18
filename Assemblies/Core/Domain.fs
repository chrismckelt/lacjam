﻿namespace Lacjam.Core
    open System
    open Microsoft.FSharp
    open Microsoft.FSharp.Core
    open Microsoft.FSharp.Collections

    module Domain =
    
        type Result() = class end

        type Xxx = {name:string}
        type Yyy = {name:string}
        let z() = 
            let (a:Xxx) = {name="aaa"}
            a.name
        z()

        type IGetDataService = interface 
            abstract GetData : value:string -> string
        end