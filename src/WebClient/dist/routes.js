/// <reference path="_references.ts" />
"use strict";
define(["require", "exports"], function (require, exports) {
    ///https://github.com/angular-ui/ui-router/wiki
    var Routes = (function () {
        function Routes() {
        }
        Routes.getRoutes = function () {
            var list = new Array();
            //http://www.ng-newsletter.com/posts/angular-ui-router.cshtml
            list.push(Routes.home, Routes.dashboard, Routes.clients, Routes.accounts, Routes.documents);
            //_.each(this.getRoutes(), (route: any) => {
            //    app.log("routes -> " + route.config.title);
            //    if (route.config && route.config.title) {
            //        var view: nav = {
            //            name: route.config.title,
            //            url: "/" + route.url,
            //            templateUrl: route.config.templateUrl,
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
        // start page
        Routes.home = {
            name: "index",
            url: "/",
            templateUrl: "app/Index/IndexView.cshtml",
            // controller: lacjam.controllers.IndexController,
            data: {
                isVisible: true,
            }
        };
        Routes.dashboard = {
            name: "dashboard",
            url: "/dashboard",
            templateUrl: "app/Dashboard/DashboardView.cshtml",
            //controller: lacjam.controllers.DashboardController,
            data: {
                isVisible: true,
            }
        };
        Routes.clients = {
            name: "clients",
            url: "/clients",
            templateUrl: "app/clients/clientView.cshtml",
            //  controller: lacjam.controllers.ClientController,
            data: {
                isVisible: true,
            }
        };
        Routes.accounts = {
            name: "accounts",
            url: "/accounts",
            templateUrl: "app/accounts/accountView.cshtml",
            // controller: lacjam.controllers.AccountController,
            data: {
                isVisible: true,
            }
        };
        Routes.documents = {
            name: "documents",
            url: "/documents",
            templateUrl: "app/documents/documentview.cshtml",
            //  controller: lacjam.controllers.DocumentController,
            data: {
                isVisible: true,
            }
        };
        return Routes;
    })();
    exports.Routes = Routes;
});
//# sourceMappingURL=../src/routes.js.map