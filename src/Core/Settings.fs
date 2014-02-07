namespace Lacjam.Core
    open System
    open SettingsProviderNet

    [<AutoOpen>]
    module Settings = 

        [<CLIMutable>]
        type TwitterSettings = {ConsumerKey:string;ConsumerSecret:string;AccessToken:string;AccessTokenSecret:string;UserId:string;OAuthToken:string;OAuthTokenSecret:string;ScreenName:string;}
        
        let lacjamTwitterSettings = { 
                                TwitterSettings.ConsumerKey="sample" ; 
                                TwitterSettings.ConsumerSecret="sample"; 
                                TwitterSettings.AccessToken="sample-oRUiJuIVw1aPgpKqVSQYptKcTXlDdFcMsbPPrfY"; 
                                TwitterSettings.AccessTokenSecret="sample"; 
                                TwitterSettings.UserId="lacjam"; 
                                TwitterSettings.OAuthToken="sample-oRUiJuIVw1aPgpKqVSQYptKcTXlDdFcMsbPPrfY"; 
                                TwitterSettings.OAuthTokenSecret="sample"; 
                                TwitterSettings.ScreenName="lacjam"; 
 
                        }
        


        let saveTwitterSettings lacjamTwitterSettings =  
                                                    try
                                                        let sp = new SettingsProvider(new RoamingAppDataStorage("lacjam"))
                                                        sp.SaveSettings(lacjamTwitterSettings)
                                                    with | ex -> Console.Write(ex.ToString())

        let getTwitterSettings =    //saveTwitterSettings lacjamTwitterSettings
                                    let sp = new SettingsProvider(new RoamingAppDataStorage("lacjam"))
                                    sp.GetSettings<TwitterSettings>()


