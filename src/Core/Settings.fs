namespace Lacjam.Core
    open System
    open SettingsProviderNet

    module Settings = 

        [<CLIMutable>]
        type TwitterSettings = {ConsumerKey:string;ConsumerSecret:string;AccessToken:string;AccessTokenSecret:string;UserId:string;OAuthToken:string;OAuthTokenSecret:string;ScreenName:string;}
        
        let lacjamTwitterSettings = { 
                                        TwitterSettings.ConsumerKey = "wahYDK1SPIpePCoYYjem3Q"; 
                                        TwitterSettings.ConsumerSecret = "boSzSvntgZyVeECnsWqiI7ZO42igWmmiTa8rb8AI0";
                                        TwitterSettings.UserId = "lacjam"; 
                                        TwitterSettings.ScreenName = "lacjam"; 
                                        TwitterSettings.OAuthToken = "1195787894-760NvbldEQrNPFPSMFrBaD3IxbRICYe6yV9UGNG"; 
                                        TwitterSettings.OAuthTokenSecret = "kyqBaoGqT2H3ZPmpgl8mwSvN2hzYs6pgNnLr3gZsdRUsk" ;
                                        TwitterSettings.AccessToken = "1195787894-oRUiJuIVw1aPgpKqVSQYptKcTXlDdFcMsbPPrfY" ;
                                        TwitterSettings.AccessTokenSecret = "kyqBaoGqT2H3ZPmpgl8mwSvN2hzYs6pgNnLr3gZsdRUsk"; 
                        }
        


        let saveTwitterSettings lacjamTwitterSettings =  
                                                    try
                                                        let sp = new SettingsProvider(new RoamingAppDataStorage("lacjam"))
                                                        sp.SaveSettings(lacjamTwitterSettings)
                                                    with | ex -> Console.Write(ex.ToString())

        let getTwitterSettings =    saveTwitterSettings lacjamTwitterSettings
                                    let sp = new SettingsProvider(new RoamingAppDataStorage("lacjam"))
                                    sp.GetSettings<TwitterSettings>()


