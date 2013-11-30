namespace Lacjam.Core
open System


module Domain =

    type Xxx = {name:string}
    type Yyy = {name:string}
    let z() = 
        let (a:Xxx) = {name="aaa"}
        a.name
    z()

