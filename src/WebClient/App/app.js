/// <reference path="../scripts/typings/angularjs/angular.d.ts" />
/// <reference path="../scripts/typings/angularjs/angular-resource.d.ts" />
/// <reference path="../scripts/typings/angularjs/angular-route.d.ts" />
/// <reference path="../scripts/typings/angularjs/angular-sanitize.d.ts" />
/// <reference path="../scripts/typings/angularjs/angular-scenario.d.ts" />
/// <reference path="../scripts/typings/angularjs/angular-cookies.d.ts" />
/// <reference path="../scripts/typings/angularjs/angular-mocks.d.ts" />
/// <reference path="../scripts/typings/angularjs/angular-animate.d.ts" />
/// <reference path="../scripts/typings/bootstrap/bootstrap.d.ts" />
/// <reference path="../scripts/typings/bootstrap.datepicker/bootstrap.datepicker.d.ts" />
/// <reference path="../scripts/typings/jquery/jquery.d.ts" />
'use strict';
define(['services/routeResolver'], function () {
    var app = angular.module('lacjam', ['ngRoute', 'ngAnimate', 'routeResolverServices', 'wc.Directives', 'wc.Animations', 'ui.bootstrap']);

    app.config([
        '$routeProvider', 'routeResolverProvider', '$controllerProvider', '$compileProvider', '$filterProvider', '$provide', '$httpProvider',
        function ($routeProvider, routeResolverProvider, $controllerProvider, $compileProvider, $filterProvider, $provide, $httpProvider) {
            //Change default views and controllers directory using the following:
            //routeResolverProvider.routeConfig.setBaseDirectories('/app/views', '/app/controllers');
            app.register = {
                controller: $controllerProvider.register,
                directive: $compileProvider.directive,
                filter: $filterProvider.register,
                factory: $provide.factory,
                service: $provide.service
            };

            //Define routes - controllers will be loaded dynamically
            var route = routeResolverProvider.route;

            $routeProvider.when('/customers', route.resolve('Customers', 'customers/')).when('/customerorders/:customerID', route.resolve('CustomerOrders', 'customers/')).when('/customeredit/:customerID', route.resolve('CustomerEdit', 'customers/')).when('/orders', route.resolve('Orders', 'orders/')).when('/about', route.resolve('About')).otherwise({ redirectTo: '/customers' });
        }]);

    //Only needed for Breeze. Maps Q (used by default in Breeze) to Angular's $q to avoid having to call scope.$apply()
    app.run([
        '$q', '$rootScope',
        function ($q, $rootScope) {
            breeze.core.extendQ($rootScope, $q);
        }]);

    return app;
});
//# sourceMappingURL=app.js.map
