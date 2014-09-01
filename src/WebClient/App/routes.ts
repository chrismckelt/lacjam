/// <reference path="_references.ts" />
"use strict";

module app {


    ///https://github.com/angular-ui/ui-router/wiki
    export class Routes {

        // start page
        public static home: ng.ui.IState = {
            name: "home",
            url: "/",
            templateUrl: "app/Index/view.cshtml",
            controller: app.controllers.Index,
            data: {
                isVisible: true,
                nav: 2,
                content: "<i class='home'></i> Home"
            }
        };



        //public static admin: ng.ui.IState = {
        //    name: "admin",
        //    controller: app.controllers.Admin,
        //    templateUrl: "app/admin/admin.cshtml",
        //    data:
        //    {
        //        isVisible: true,
        //        secure: true,
        //        nav: 99,
        //        content: '<i class=" fa fa-cog"></i> Settings'
        //    }

        //};




        public static search: ng.ui.IState = {
            name: "search",
            url: "/search",
            templateUrl: 'app/Search/Search.cshtml',
            data:
            {
                isVisible: true,
            }
        };


        public static getRoutes(): Array<ng.ui.IState> {

            var list = new Array<ng.ui.IState>();

            //http://www.ng-newsletter.com/posts/angular-ui-router.cshtml
            list.push(
                Routes.home,
                Routes.search
            );


            //_.each(this.getRoutes(), (route: any) => {
            //    app.log("routes -> " + route.config.title);
            //    if (route.config && route.config.title) {
            //        var view: nav = {
            //            name: route.config.title,
            //            url: "/" + route.url,
            //            templateUrl: route.config.templateUrl,
            //            controller: app.controllers[route.config.title],
            //            params: route.config.settings,
            //        };

            //        list.push(view);
            //    } else {
            //        app.log("Dud route = " + route);
            //    }


            //  });
            app.log.debug("total routes -> " + list.length.toLocaleString());
            return list;

        }

    }
}
