module Jira
    open System
    open System.ServiceModel
    open Microsoft.FSharp.Linq
    open Microsoft.FSharp.Data.TypeProviders
    open Lacjam
    open Lacjam.Core
    open Lacjam.Core.Runtime
    open Lacjam.Core.User


    type Project = {key:string}
    type Fields = {fields : System.Collections.Generic.Dictionary<string, System.Object>}
    type TimeTracking = { originalEstimate : System.Decimal}
    type IssueTypeSection = {name: System.String}

    let createIssue() = 
    try
      
       let rc = new RestSharp.RestClient("https://atlassian.au.challenger.net/jira/rest/api/2/issue/")
       let request = new RestSharp.RestRequest(Method = RestSharp.Method.POST,RequestFormat = RestSharp.DataFormat.Json)
       request.AddHeader("Content-Type", "application/json") |> ignore
       request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(String.Format("{0}:{1}", Environment.UserName, User.WindowsAccount.getPassword())))) |> ignore
       let issueData = new System.Collections.Generic.Dictionary<string, System.Object>()
       
       let p = {key="DPMITPROJ"}
       issueData.Add("project",p)
       issueData.Add("summary", "Phoenix penetration test fixes")
       issueData.Add("duedate", System.DateTime.Now.AddMonths(1))        
       issueData.Add("customfield_10360", System.DateTime.Now)
       let it = {name="Epic"}
       issueData.Add("issuetype", it)
      // let json = IO.File.ReadAllText(@"c:\temp\rest.txt")
       request.RequestFormat <- RestSharp.DataFormat.Json
       request.UseDefaultCredentials <- true
       let mt = {fields = issueData}      
       request.AddBody(mt) |> ignore
       
       let result = rc.Execute(request)
       //let issue = restClient.CreateIssue("DPMITPROJ","5", "ddd")
       Console.WriteLine(result)
    with
    | exn -> printfn "An exception occurred: %s" exn.Message