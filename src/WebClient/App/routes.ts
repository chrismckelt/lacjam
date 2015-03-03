/// <reference path="../_references.ts" />
"use strict";

 export class Routes {

        // start page
        public static home: any = {
            route: "",
            moduleId: "./app/Index/Index",
            nav: true,
            title: "Home"
        };

        public static dashboard: any = {
            route: "dashboard",
            moduleId: "app/Dashboard/DashboardView", 
            nav: true,
            title: "dashboard"
        };

        public static clients: any = {
            route: "clients",
            moduleId: "app/clients/clientview",
            nav: true,
            title: "clients"
        };

        public static accounts: any = {
            route: "accounts",
            moduleId: "./app/Accounts",
            nav: true,
            title: "accounts"
        };

        public static documents: any = {
            route: "documents",
            moduleId: "./app/documents",
            nav: true,
            title: "documents"
        };


        public static getRoutes(): Array<any> {

            var list = new Array<any>();

            //http://www.ng-newsletter.com/posts/angular-ui-router
            list.push(
                Routes.home
                //Routes.dashboard,
               // Routes.clients
              //  Routes.accounts,
              //  Routes.documents
   
            );


            //_.each(this.getRoutes(), (route: any) => {
            //    app.log("routes -> " + route.config.title);
            //    if (route.config && route.config.title) {
            //        var view: nav = {
            //            route: route.config.title,
            //            moduleId: "/" + route.url,
            //            moduleId:: route.config.templateUrl,
            //            controller: lacjam.controllers[route.config.title],
            //            params: route.config.settings,
            //        };

            //        list.push(view);
            //    } else {
            //        app.log("Dud route = " + route);
            //    }


            //  });
            //lacjam.log.debug("total routes -> " + list.length.toLocaleString());
            return list;

        }

    }

