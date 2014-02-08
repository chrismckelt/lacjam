namespace Lacjam.Core
    open System
    open Microsoft.FSharp
    open Microsoft.FSharp.Core
    open Microsoft.FSharp.Collections

    [<AutoOpen>]
    module Domain =

        [<CLIMutable>]
        type Audit = {mutable Id:Guid;AuditType:string;Message:string;CreatedDate:DateTime}

        [<CLIMutable>]            
        type Setting =  {mutable Id:Guid;Name:string;Value:string;CreatedDate:DateTime}

        [<CLIMutable>]
        type Site     =  {mutable Id:Guid;Name:string;Url:string;CreatedDate:DateTime}


        type Investor() =
            member val Id = int with get, set
            member val CreatedDate = DateTime.UtcNow with get
            member val GivenName = "" with get, set
            member val Surname = "" with get, set
            member val Title = "" with get, set
            member val IsActive = Boolean() with get, set
            with override x.ToString() = (String.Format("{0} {1} ", x.GivenName, x.Surname))
            //new() = new Investor()