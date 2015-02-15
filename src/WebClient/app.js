/// <reference path="_references.ts" />
define(["require", "exports", "aurelia-router", "app/routes"], function (require, exports, aur, routes) {
    var App = (function () {
        function App(router) {
            this.router = router;
            this.router.configure(function (config) {
                config.title = "Aurelia demo";
                config.map([
                    { route: ["", "home"], moduleId: "app/index", nav: true, title: "home" },
                    { route: "login", moduleId: "app/login", nav: false, title: "login" },
                    { route: "accounts", moduleId: routes.Routes.accounts.moduleId, nav: true, title: routes.Routes.accounts.title },
                    routes.Routes.dashboard
                ]);
            });
        }
        App.inject = [aur.Router];
        return App;
    })();
    exports.App = App;
});
//# sourceMappingURL=app.js.map