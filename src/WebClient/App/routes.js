/// <reference path="_references.ts" />
"use strict";
var app;
(function (app) {
    ///https://github.com/angular-ui/ui-router/wiki
    var Routes = (function () {
        function Routes() {
        }
        Routes.getRoutes = function () {
            var list = new Array();

            //http://www.ng-newsletter.com/posts/angular-ui-router.cshtml
            list.push(Routes.home, Routes.search);

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
        };
        Routes.home = {
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

        Routes.search = {
            name: "search",
            url: "/search",
            templateUrl: 'app/Search/Search.cshtml',
            data: {
                isVisible: true
            }
        };
        return Routes;
    })();
    app.Routes = Routes;
})(app || (app = {}));
//# sourceMappingURL=routes.js.map
