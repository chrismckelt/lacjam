module ServiceNow
    open System
    open System.ServiceModel
    open Microsoft.FSharp.Linq
    open Microsoft.FSharp.Data.TypeProviders
    open Lacjam
    open Lacjam.Core
    open Lacjam.Core.Runtime
    open Lacjam.Core.User

    //type service =  WsdlService<ServiceUri="https://challenger.service-now.com/change_request.do?WSDL", LocalSchemaFile = @"C:\dev\FSharpTypeProvidersSpike\WebServiceTypeProviderTest\change_request.wsdlschema", ForceUpdate = false>
    //https://challenger.service-now.com/incident.do?WSDL
    type IncidentsServiceWsdl = WsdlService< @"http://localhost/incident.xml" > //, ForceUpdate = false, LocalSchemaFile = @"C:\dev\FSharpTypeProvidersSpike\ConsoleF\ServiceNowWsdl\incident.wsdlschema" >
    type UserServiceWsdl = WsdlService< @"http://localhost/sys_user.xml" >
    type ChangeRequestWsdl = WsdlService< @"http://localhost/change_request.xml" >
//    type IncidentsServiceWsdl = WsdlService< @"https://challenger.service-now.com/incident.do?WSDL" > //, ForceUpdate = false, LocalSchemaFile = @"C:\dev\FSharpTypeProvidersSpike\ConsoleF\ServiceNowWsdl\incident.wsdlschema" >
//    type UserServiceWsdl = WsdlService< @"https://challenger.service-now.com/sys_user.do?WSDL" >
//    type ChangeRequestWsdl = WsdlService< @"https://challenger.service-now.com/change_request.do?WSDL" >

    let binding =  new System.ServiceModel.BasicHttpBinding()
    binding.MaxReceivedMessageSize <- 10000000L
    binding.AllowCookies <- true
    binding.Security.Transport.ClientCredentialType <- HttpClientCredentialType.Basic
    binding.Security.Mode <- BasicHttpSecurityMode.Transport
    binding.MaxBufferPoolSize <- 20000000L
    binding.MaxBufferSize <- 20000000
    binding.MaxReceivedMessageSize <- 20000000L

    let IncidentsService = 
        let ep = new System.ServiceModel.EndpointAddress(new Uri("https://challenger.service-now.com/incident.do?SOAP"))
        let client = IncidentsServiceWsdl.GetServiceNowSoap(ep)
        client.DataContext.Endpoint.Binding <- binding //new System.ServiceModel.BasicHttpBinding("ServiceNowSoap")
        let behaviours  = 
            client.DataContext.Endpoint.Behaviors |> Seq.choose (function | :? System.ServiceModel.Description.ClientCredentials as x -> Some x | _ -> None ) |> Seq.tryPick Some 
        match behaviours  with 
        | behaviour ->
            behaviour.Value.UserName.UserName <- System.Environment.UserName
            behaviour.Value.UserName.Password <- Lacjam.Core.User.WindowsAccount.getPassword()
            behaviour.Value.Windows.AllowedImpersonationLevel <- System.Security.Principal.TokenImpersonationLevel.Impersonation;
        | _ -> ()
        client

    let UserService = 
        let ep = new System.ServiceModel.EndpointAddress(new Uri("https://challenger.service-now.com/sys_user.do?SOAP"))
        let client = UserServiceWsdl.GetServiceNowSoap(ep)
        client.DataContext.Endpoint.Binding <- binding // new System.ServiceModel.BasicHttpBinding("ServiceNowSoap")
        let behaviours  = client.DataContext.Endpoint.Behaviors |> Seq.choose (function | :? System.ServiceModel.Description.ClientCredentials as x -> Some x | _ -> None ) |> Seq.tryPick Some 
        match behaviours  with 
        | behaviour ->
            behaviour.Value.UserName.UserName <- System.Environment.UserName
            behaviour.Value.UserName.Password <- Lacjam.Core.User.WindowsAccount.getPassword()
            behaviour.Value.Windows.AllowedImpersonationLevel <- System.Security.Principal.TokenImpersonationLevel.Impersonation;
        | _ -> ()
        client

    let ChangeRequestService = 
        let ep = new System.ServiceModel.EndpointAddress(new Uri("https://challenger.service-now.com/change_request.do?SOAP"))
        let client = ChangeRequestWsdl.GetServiceNowSoap(ep)
        client.DataContext.Endpoint.Binding  <- binding //new System.ServiceModel.BasicHttpBinding("ServiceNowSoap")
        let behaviours  = client.DataContext.Endpoint.Behaviors |> Seq.choose (function | :? System.ServiceModel.Description.ClientCredentials as x -> Some x | _ -> None ) |> Seq.tryPick Some 
        match behaviours  with 
        | behaviour ->
            behaviour.Value.UserName.UserName <- System.Environment.UserName
            behaviour.Value.UserName.Password <- Lacjam.Core.User.WindowsAccount.getPassword()
            behaviour.Value.Windows.AllowedImpersonationLevel <- System.Security.Principal.TokenImpersonationLevel.Impersonation;
        | _ -> ()
        client

    let getMyCallerId() =   
        let query = UserServiceWsdl.ServiceTypes.getRecords()
       // query.user_name <- System.Environment.UserName
        query.email <- "cmckelt@challenger.com.au"
        UserService.getRecords(query) 
        |> Seq.head

    let getDpmItCmrs() =   
        let query = ChangeRequestWsdl.ServiceTypes.getRecords()
        query.__limit <- "250"
        query.sys_created_on <- " >= " + System.DateTime.Now.AddMonths(-6).ToShortDateString()
        query.opened_at <- " >= " + System.DateTime.Now.AddMonths(-6).ToShortDateString()
        query.u_it_owner_approver <- "6d18a53d0a0a781101b45f5f3465b436"
        ChangeRequestService.getRecords(query)   

    let getMyIncidents() =    
        let me = getMyCallerId()
        let query = IncidentsServiceWsdl.ServiceTypes.getRecords()
        query.caller_id <- me.sys_id
        query.sys_created_on <- "> " + DateTime.Now.AddMonths(-3).ToShortDateString()
        let records = IncidentsService.getRecords(query)
        records 
