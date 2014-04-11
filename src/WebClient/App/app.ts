
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


module app {
    'use strict';

    var lac: ng.IModule = angular.module('app', ['ngRoute', 'ui.bootstrap']);

    lac.controller('controller', (function() {}));

    lac.service('service', (function () { }));

    lac.directive('directive', (function () { }));

    lac.config([
        '$routeProvider', function($routeProvider: ng.route.IRouteProvider) {
            $routeProvider.when('/', { templateUrl: 'index.cshtml' }).
                otherwise({ redirectTo: '/' });
        }
    ]);
}

//'use strict';

//define(['services/routeResolver'], function () {

//    var app = angular.module('lacjam', ['ngRoute', 'ngAnimate', 'routeResolverServices', 'wc.Directives', 'wc.Animations', 'ui.bootstrap']);

//    app.config(['$routeProvider', 'routeResolverProvider', '$controllerProvider', '$compileProvider', '$filterProvider', '$provide', '$httpProvider',
//        function ($routeProvider, routeResolverProvider, $controllerProvider, $compileProvider, $filterProvider, $provide, $httpProvider) {

//            //Change default views and controllers directory using the following:
//            //routeResolverProvider.routeConfig.setBaseDirectories('/app/views', '/app/controllers');

//            //app.register =
//            //{
//            //    controller: $controllerProvider.register,
//            //    directive: $compileProvider.directive,
//            //    filter: $filterProvider.register,
//            //    factory: $provide.factory,
//            //    service: $provide.service
//            //};

//            //Define routes - controllers will be loaded dynamically
//            var route = routeResolverProvider.route;

//            $routeProvider
//            //route.resolve() now accepts the convention to use (name of controller & view) as well as the 
//            //path where the controller or view lives in the controllers or views folder if it's in a sub folder. 
//            //For example, the controllers for customers live in controllers/customers and the views are in views/customers.
//            //The controllers for orders live in controllers/orders and the views are in views/orders
//            //The second parameter allows for putting related controllers/views into subfolders to better organize large projects
//            //Thanks to Ton Yeung for the idea and contribution
//                .when('/customers', route.resolve('Customers', 'customers/'))
//                .when('/customerorders/:customerID', route.resolve('CustomerOrders', 'customers/'))
//                .when('/customeredit/:customerID', route.resolve('CustomerEdit', 'customers/'))
//                .when('/orders', route.resolve('Orders', 'orders/'))
//                .when('/about', route.resolve('About'))
//                .otherwise({ redirectTo: '/customers' });

//        }]);
//});