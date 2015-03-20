/// <reference path="../_references.ts" />
"use strict";
define(["require", "exports"], function(require, exports) {
    var Routes = (function () {
        function Routes() {
        }
        Routes.getRoutes = function () {
            var list = new Array();

            //http://www.ng-newsletter.com/posts/angular-ui-router
            list.push(Routes.home, Routes.dashboard, Routes.clients, Routes.accounts, Routes.documents);

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
            route: "",
            moduleId: "./app/Index/Index",
            nav: true,
            title: "Home"
        };

        Routes.dashboard = {
            route: "dashboard",
            moduleId: "./app/Dashboards/Dashboard",
            nav: true,
            title: "Dashboard"
        };

        Routes.clients = {
            route: "clients",
            moduleId: "./app/clients/client",
            nav: true,
            title: "Clients"
        };

        Routes.accounts = {
            route: "accounts",
            moduleId: "./app/Accounts/account",
            nav: true,
            title: "Accounts"
        };

        Routes.documents = {
            route: "documents",
            moduleId: "./app/documents/document",
            nav: true,
            title: "Documents"
        };
        return Routes;
    })();
    exports.Routes = Routes;
});
