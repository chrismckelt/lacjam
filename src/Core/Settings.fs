namespace Lacjam.Core
    open System
    open SettingsProviderNet
    open Autofac
    open FSharp.Configuration

    [<AutoOpen>]
    module Settings = 
        

        type Config() = 
            member val Pop3Host  = "" with get, set
            member val Pop3Ssl  = "" with get, set
            member val Pop3User  = "" with get, set
            member val Pop3Password  = "" with get, set
            member val ConnectionString  = "" with get, set

        [<CLIMutable>]
        type TwitterSettings = {ConsumerKey:string;ConsumerSecret:string;AccessToken:string;AccessTokenSecret:string;UserId:string;OAuthToken:string;OAuthTokenSecret:string;ScreenName:string;}
        
        let lacjamTwitterSettings = { 
                                        TwitterSettings.ConsumerKey = "wahYDK1SPIpePCoYYjem3Q"; 
                                        TwitterSettings.ConsumerSecret = "boSzSvntgZyVeECnsWqiI7ZO42igWmmiTa8rb8AI0";
                                        TwitterSettings.UserId = "lacjam"; 
                                        TwitterSettings.ScreenName = "lacjam"; 
                                        TwitterSettings.OAuthToken = "2195787894-oRUiJuIVw1aPgpKqVSQYptKcTXlDdFcMsbPPrfY"; 
                                        TwitterSettings.OAuthTokenSecret = "kyqBaoGqT2H3ZPmpgl8mwSvN2hzYs6pgNnLr3gZsdRUsk" ;
                                        TwitterSettings.AccessToken = "2195787894-oRUiJuIVw1aPgpKqVSQYptKcTXlDdFcMsbPPrfY" ;
                                        TwitterSettings.AccessTokenSecret = "kyqBaoGqT2H3ZPmpgl8mwSvN2hzYs6pgNnLr3gZsdRUsk"; 
                        }
        


        let saveTwitterSettings lacjamTwitterSettings =  
                                                    try
                                                        let sp = new SettingsProvider(new RoamingAppDataStorage("lacjam"))
                                                        sp.SaveSettings(lacjamTwitterSettings)
                                                    with | ex -> Console.Write(ex.ToString())

        let getTwitterSettings =    saveTwitterSettings lacjamTwitterSettings
                                    let sp = new SettingsProvider(new RoamingAppDataStorage("lacjam"))
                                    //sp.GetSettings<TwitterSettings>()
                                    lacjamTwitterSettings


