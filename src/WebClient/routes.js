/// <reference path="../_references.ts" />
"use strict";
define(["require", "exports"], function(require, exports) {
    var Routes = (function () {
        function Routes() {
        }
        Routes.getRoutes = function () {
            var list = new Array();

            //http://www.ng-newsletter.com/posts/angular-ui-router
            list.push(Routes.home, Routes.dashboard, Routes.clients);

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
        };
        Routes.home = {
            route: "index",
            moduleId: "app/Index/Index",
            nav: true,
            title: "Home"
        };

        Routes.dashboard = {
            route: "dashboard",
            moduleId: "app/Dashboard/DashboardView",
            nav: true,
            title: "dashboard"
        };

        Routes.clients = {
            route: "clients",
            moduleId: "app/clients/clientview",
            nav: true,
            title: "clients"
        };

        Routes.accounts = {
            route: "accounts",
            moduleId: "app/Accounts/AccountView",
            nav: true,
            title: "accounts"
        };

        Routes.documents = {
            route: "documents",
            moduleId: "app/documents/documentsview",
            nav: true,
            title: "documents"
        };
        return Routes;
    })();
    exports.Routes = Routes;
});
