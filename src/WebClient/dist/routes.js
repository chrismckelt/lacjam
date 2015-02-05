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
            list.push(Routes.home, Routes.dashboard, Routes.clients, Routes.accounts, Routes.documents, Routes.metadataDefinitions, Routes.metadataDefinitionEdit, Routes.metadataDefinitionUpdate, Routes.entities, Routes.entityCreate, Routes.entityEdit, Routes.entityDuplicate);

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
            name: "index",
            url: "/",
            templateUrl: "app/Index/IndexView.cshtml",
            controller: app.controllers.IndexController,
            data: {
                isVisible: true
            }
        };

        Routes.dashboard = {
            name: "dashboard",
            url: "/dashboard",
            templateUrl: "app/Dashboard/DashboardView.cshtml",
            controller: app.controllers.DashboardController,
            data: {
                isVisible: true
            }
        };

        Routes.clients = {
            name: "clients",
            url: "/clients",
            templateUrl: "app/clients/clientView.cshtml",
            controller: app.controllers.ClientController,
            data: {
                isVisible: true
            }
        };

        Routes.accounts = {
            name: "accounts",
            url: "/accounts",
            templateUrl: "app/accounts/accountView.cshtml",
            controller: app.controllers.AccountController,
            data: {
                isVisible: true
            }
        };

        Routes.documents = {
            name: "documents",
            url: "/documents",
            templateUrl: "app/documents/documentview.cshtml",
            controller: app.controllers.DocumentController,
            data: {
                isVisible: true
            }
        };

        Routes.metadataDefinitions = {
            name: "metadatadefinitions",
            url: "/metadatadefinitions",
            templateUrl: "app/metadatadefinitions/MetadataDefinitionView.cshtml",
            controller: app.controllers.MetadataDefinitionController,
            data: {
                isVisible: true
            }
        };

        Routes.metadataDefinitionEdit = {
            name: "metadatadefinitionsedit",
            url: "/metadatadefinitionsedit",
            templateUrl: "app/metadatadefinitions/MetadataDefinitionEdit.cshtml",
            controller: app.controllers.MetadataDefinitionEditController,
            data: {
                isVisible: true
            }
        };

        Routes.metadataDefinitionUpdate = {
            name: "metadatadefinitionsupdate",
            url: "/metadatadefinitionsedit/{identity}",
            templateUrl: "app/metadatadefinitions/MetadataDefinitionEdit.cshtml",
            controller: app.controllers.MetadataDefinitionEditController,
            data: {
                isVisible: true
            }
        };

        Routes.entities = {
            name: "entities",
            url: "/entities",
            templateUrl: "app/entities/EntityView.cshtml",
            controller: app.controllers.EntityController,
            data: {
                isVisible: true
            }
        };

        Routes.entityCreate = {
            name: "entitycreate",
            url: "/entitycreate",
            templateUrl: "app/entities/EntityEdit.cshtml",
            controller: app.controllers.EntityEditController,
            data: {
                isVisible: true
            }
        };

        Routes.entityEdit = {
            name: "entityedit",
            url: "/entityedit/{identity}",
            templateUrl: "app/entities/EntityEdit.cshtml",
            controller: app.controllers.EntityEditController,
            data: {
                isVisible: true
            }
        };

        Routes.entityDuplicate = {
            name: "entityduplicate",
            url: "/entityedit/{identity}/duplicate",
            templateUrl: "app/entities/EntityEdit.cshtml",
            controller: app.controllers.EntityEditController,
            data: {
                isVisible: true,
                duplicate: true
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
//# sourceMappingURL=../src/routes.js.map
