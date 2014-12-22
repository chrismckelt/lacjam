/// <reference path="_references.ts" />
/* tslint:disable */
//https://gist.github.com/scottmcarthur/9051681

"use strict";

// Create and register modules
var modules = ["app.directives", "app.filters", "app.services", "app.controllers"];
modules.forEach((m) => angular.module(m, []));

modules.push(
    "ui",
    "ngCookies",
    "ngGrid",
    "ngCookies",
    "ngAnimate", // animations
    "ngSanitize", // sanitizes html bindings (ex: Sidebar.js)
    "ngResource",
    "ui.router",
    "ui.bootstrap",
    "dialogs.main",
    "dialogs.default-translations",
    "pascalprecht.translate"
    );

angular.module("app", modules)
    .config([
        "$stateProvider", "$urlRouterProvider", "$injector", "$locationProvider", "$httpProvider", "$controllerProvider",
        (
            $stateProvider: ng.ui.IStateProvider,
            $urlRouterProvider: ng.ui.IUrlRouterProvider,
            $injector: ng.auto.IInjectorService,
            $locationProvider: ng.ILocationProvider,
            $httpProvider: ng.IHttpProvider,
            $controllerProvider : ng.IControllerProvider
        ) => {

            app.log.debug("app.config started...");
            $controllerProvider.allowGlobals(); //http://www.lcube.se/angular-js-controller-error-argument-is-not-a-function/

            app.global.typesCache.add("$stateProvider", $stateProvider);
            app.global.typesCache.add("$urlRouterProvider", $urlRouterProvider);
            app.global.typesCache.add("$injector", $injector);
            app.global.typesCache.add("$locationProvider", $locationProvider);
            app.global.typesCache.add("$httpProvider", $httpProvider);

            //  $httpProvider.defaults.headers.common["X-Requested-With"] = "XMLHttpRequest";

            //// allow case insensitive urls
            $urlRouterProvider.rule(($injector: ng.auto.IInjectorService, $location: ng.ILocationService) => {
                //what this function returns will be set as the $location.url
                var path = $location.path(), normalized = path.toLowerCase();
                if (path != normalized) {
                    //instead of returning a new url string, I"ll just change the $location.path directly so I don"t have to worry about constructing a new url string and so a new state change is not triggered
                    $location.replace().path(normalized);
                }
                // because we"ve returned nothing, no state change occurs
            });


            // state provider
            app.log.debug("Registering routes with state provider");
            // routing
            angular.forEach(app.Routes.getRoutes(), (x: ng.ui.IState) => $stateProvider.state(x.name, x));

            $stateProvider.state("otherwise", {
                url: "*path",
                templateUrl: "app/404.cshtml",
                name: "404"
            });

            app.global.angularModuleReference = this;

            app.log.debug("app.config finished...");
        }
    ])
    .run([
        "$rootScope", "$log", "$http", "$state", "$stateParams", "$location", "$injector", "$q", "$timeout", "$window", "$templateCache",
        ($rootScope: ng.IRootScopeService,
            $log: ng.ILogService,
            $http: ng.IHttpService,
            $state: ng.ui.IStateService,
            $stateParams: ng.ui.IStateParamsService,
            $location: ng.ILocationService,
            $injector: ng.auto.IInjectorService,
            $q: ng.IQService,
            $timeout: ng.ITimeoutService,
            $window: ng.IWindowService,
            $templateCache : ng.ITemplateCacheService
            ) => {

            //$injector.invoke(($rootScope, $compile, $document) => {
            //    $compile($document)($rootScope);
            //    $rootScope.$digest();
            //});

            app.log.debug("app.run started...");

            app.global.typesCache.add("$rootScope", $rootScope);
            app.global.typesCache.add("$log", $log);
            app.global.typesCache.add("$http", $http);
            app.global.typesCache.add("$state", $state);
            app.global.typesCache.add("$location", $location);
            app.global.typesCache.add("$injector", $injector);
            app.global.typesCache.add("$timeout", $timeout);
            app.global.typesCache.add("$q", $q);
            app.global.typesCache.add("$window", $window);


            $rootScope.$on("$stateNotFound",
                (event, unfoundState, fromState, fromParams) => {
                    app.log.debug("State not found"); // "lazy.state"
                    app.log.debug("$stateNotFound"); // "lazy.state"
                });
            ///https://github.com/angular-ui/ui-router/wiki#state-change-events
            $rootScope.$on("$stateChangeStart",
                (event, toState, toParams, fromState, fromParams) => {
                    app.log.debug("$stateChangeStart:" + toState.name);
                    app.fn.spinStart();
                });

            $rootScope.$on('$stateChangeSuccess',
                (event, toState, toParams, fromState, fromParams) => {
                    app.log.debug("$stateChangeStart:" + toState.name);
                    app.fn.spinStop();
                });

            // app services init
            app.common = new app.services.Common($rootScope, $log, $timeout, $http, $q);
            app.rootScope = $rootScope;

            app.log.debug("activating services");
            angular.forEach(app.global.serviceNames, (service) => {
                var act = app.global.typesCache.getByKey(service);
                act.activate();});


            app.log.debug("app.run finished...");
            // $timeout(() => app.log.debug("timeout callback - state name : " + $state.current.name), 5000);

            app.redirectToRoute(app.Routes.documents);

            $timeout(() => {
                app.log.info("-- ALL SERVICES --");
                app.showRegistrations("app",null);
            }, 5000
                );
        }
    ]);


module app {
    export class Item {
        constructor(public name: string, public object: any) {
        }
    }

    export class Dictionary<T> {
        private items = [];

        add(key: string, value: T) {
            if (value && typeof value != "undefined") {
                this.items.push(value);
                this.items[key] = value;
            } else {
                app.log.error("Failed to add item to app cache (null) - " + key);
            }
        }

        getByIndex(index: number) {
            return this.items[index];
        }

        getByKey(key: string) {
            return this.items[key];
        }
    }


    export var common: app.services.Common;    
    export var rootScope: ng.IRootScopeService;

    export class fn {

        public static appUrl(url: string) {
            return "/api/" + url;
        }

        public static createGuid() {
            var d = new Date().getTime();
            var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, c => {
                var r = (d + Math.random() * 16) % 16 | 0;
                d = Math.floor(d / 16);
                return (c == 'x' ? r : (r & 0x7 | 0x8)).toString(16);
            });
            return uuid;
        }

        public static getCurrentDate() {
            return moment();
        }

        public static textContains(text: string, searchText: string) {
            return text && -1 !== text.toLowerCase().indexOf(searchText.toLowerCase());
        }

        public static isNumber(val) {
            // negative or positive
            return /^[-]?\d+$/.test(val);
        }

        public static applyConstructor(constr, args) {
            var obj = Object.create(constr.prototype);
            var services: any[] = [];
            angular.forEach(args, x => {
                try {
                    services.push(app.resolveByName(x));
                } catch (e) {
                    services.push(x);
                }
            });
            constr.apply(obj, services);
            return obj;
        }

        public addDays(date, days) {
            var result = new Date(date);
            result.setDate(date.getDate() + days);
            return result;
        }

        public makeModel<T>(controllerId: string) {
            app.fn.safeApply(
                () => {
                    // if (typeof this.$scope.vm === "undefined") {
                    app.log.debug("making model for " + controllerId);
                    var name = app.fn.capitaliseFirstLetter(controllerId) + "Model";
                    try {
                        var made = InstanceLoader.getInstance<T>(app.controllers, name, []);
                        return <T>made;

                    } catch (ex) {
                        app.log.error("makeModel error - " + controllerId, ex);
                        return null;
                    }
                    //  }
                });

            return null;
        }

        public static copyProperties(source: any, target: any): void {
            for (var prop in source) {
                if (target[prop] !== undefined) {
                    target[prop] = source[prop];
                } else {
                    console.error("Cannot set undefined property: " + prop);
                }
            }
        }


        public static capitaliseFirstLetter(str: string) {
            return str.charAt(0).toUpperCase() + str.slice(1);
        }

        public static spinStart(key= "spinner") {
            app.common.broadcast("cc-spinner:spin", key);
        }

        public static spinStop(key= "spinner") {
            app.common.broadcast("cc-spinner:stop", key);
        }

        public static replaceLocationUrlGuidWithId(id) {
            // If the current Url is a Guid, then we replace 
            // it with the passed in id. Otherwise, we exit.
            var currentPath = app.resolveByName("$location").path();
            var slashPos = currentPath.lastIndexOf("/", currentPath.length - 2);
            var currentParameter = currentPath.substring(slashPos - 1);

            if (app.fn.isNumber(currentParameter)) {
                return;
            }

            var newPath = currentPath.substring(0, slashPos + 1) + id;
            app.redirectToUrl(newPath);
        }

        public static safeApply(fn: () => any) {
            var phase = app.rootScope.$root.$$phase;
            if (phase == '$apply' || phase == '$digest') {
                if (fn && (typeof (fn) === 'function')) {
                    fn();
                }
            } else {
                app.rootScope.$apply().$apply(fn);
            }
        }
    }

    export class global {

        public static angularModuleReference: ng.IModule;
        public static typesCache: Dictionary<any> = new Dictionary<any>();
        public static $injector = angular.injector(["ng"]);

        public static $scope: string = "$scope";
        public static $location: string = "$location";
        public static $log: string = "$log";
        public static $cookieStore: string = "$cookieStore";
        public static $ngCookies: string = "$ngCookies";
        public static $http: string = "$http";
        public static $resource: string = "$resource";
        public static $state: string = "$state";
        public static $timeout: string = "$q";
        public static $q: string = "$q";

        public static appName: string = "app";
        public static spinner: Spinner;


        public static appControllers: string = "app.controllers";
        public static appDirectives: string = "app.directives";
        public static appServices: string = "app.services";
        public static appModels: string = "app.models";
        public static appProviders: string = "app.providers";
        public static appFilters: string = "app.filters";
        public static serviceNames: Array<string> = [];
        public static nulloDate = new Date(1900, 0, 1);
        public static standardDateFormat = "DD/MM/YYYY";


        public static keyCodes = {
            backspace: 8,
            tab: 9,
            enter: 13,
            esc: 27,
            space: 32,
            pageup: 33,
            pagedown: 34,
            end: 35,
            home: 36,
            left: 37,
            up: 38,
            right: 39,
            down: 40,
            insert: 45,
            del: 46
        };

        public static events = {
            controllerActivateSuccess: "controller.activateSuccess",
        };

    }

    export module base {

        export interface IScopeBase extends ng.IScope {
            vm: any;
            activator: ControllerBase
        }

        export class ModelBase {
            hasLoaded: boolean = false;
        }

        export class ControllerBase {
            public title : string;
            public hasLoaded = false;

            public controllerId: string = typeof (this.constructor.prototype.name);

            public activate = () => {
                app.log.debug("Base controller loaded for " + this.controllerId);
                this.activateController([], this.controllerId);

            }

            public activateController(promises: any[], controllerId: string): ng.IPromise<any> {

                return app.common.$q.all(promises)
                    .then(eventArgs => {
                        app.log.debug("Controller loaded : " + this.controllerId);
                        //app.log.debug(eventArgs);
                        var data = { controllerId: this.controllerId, eventArgs: eventArgs };
                        app.common.broadcast(app.global.events.controllerActivateSuccess, data);

                    });
            }
        }

        export class ServiceBase<T> implements IService
        {
            public entity: T;

            constructor(public serviceUri: string,public entityName: string) {

            }

            public activate() { }

            public getAll = () => {
                return app.common.$http({ method: 'GET', url: app.fn.appUrl(this.serviceUri + '/list/all') });
            }

            public create = (model : T) => {
                return app.common.$http({
                    method: 'POST',
                    url: app.fn.appUrl(this.serviceUri), 
                    data : model
                });
            }

            public update = (model: T, identity: app.model.Guid) => {
                return app.common.$http({
                    method: 'PUT',
                    url: app.fn.appUrl(this.serviceUri + '/' + identity),
                    data: model
                });
            }

             public get = (identity: app.model.Guid) => {
                return app.common.$http({
                    method: 'GET',
                    url: app.fn.appUrl(this.serviceUri + '/' + identity)
                });
             }

            public doDelete = (identity: app.model.Guid) => {
                return app.common.$http({
                    method: 'DELETE',
                    url: app.fn.appUrl(this.serviceUri + '/' + identity)
                });
            }
        }
    }

    export module model {
        null;
    }

    export module filters {
        null;
    }

    export module directives {
        null;
    }

    export module controllers {
        null;
    }

    export module services {
        null;
    }

    export interface IController {

    }

    export interface IDirective extends ng.IDirective {
        restrict: string;
        link($scope: ng.IScope, element: JQuery, attrs: ng.IAttributes): any;
    }

    export interface IFilter {
        filter(input: any, ...args: any[]): any;
    }

    export interface IService {
        serviceUri: string;
        activate: () => void;
    }

    /**
    * Register new controller.
    *
    * @param className
    * @param services
    */
    export function registerController(className: string, ctor: any = null) {
        app.log.info("controllers regististration for " + className);
        angular.module(app.global.appControllers).controller(className, ctor);

    }

    /**
     * Register new filter.
     *
     * @param className
     * @param services
     */
    export function registerFilter(className: string, services = []) {
        var filter = "app.filters." + className;
        app.log.debug("Registering filter: " + filter);
        services.push(new app.filters[className]().filter);
        angular.module("app.filters").filter(className, services);
    }

    /**
 * Register new value.

 */
    export function registerValue(name: string, obj: any) {
        app.global.typesCache.add(name, obj);
        angular.module("app").value(name, obj);
    }


    /**
        * Register new directive.
        *
        * @param className
        * @param services
        */
    export function registerDirective(className: string, services = []) {
        //var directive = className[0].toLowerCase() + className.slice(1);
        app.log.debug("Registering directive: " + className);
        services.push(() => new app.directives[className]());
        angular.module("app.directives").directive(className, () => new app.directives[className](services));
    }




    /**
     * Register new service.
     *
     * @param className
     * @param services
     */
    // export function registerService(className: string, services = []) {
    export function registerService(ctor: any, services = [], name: string= null) {

        try {
            if (!name || name == "")
                name = app.Describer.getName(ctor);
            app.log.info("Registering Service -- " + name);
            var obj = app.fn.applyConstructor(ctor, services);
            services.push(() => obj);
            angular.module(app.global.appName).service(name, services);
        } catch (e) {
            app.log.error(e);
        }

    }


    /**
 * Register new factory.
 *
 * @param className
 * @param factory
 */
    export function registerFactory(ctor: any, services: any = []) {
        var name = app.Describer.getName(ctor);
        var obj = app.InstanceLoader.getInstance<IService>(app.services, name);
        obj.activate();
        app.global.typesCache.add(name, obj);
        app.log.info("Registering Factory -- " + name);
        services.push(ctor);
        angular.module(app.global.appServices).factory(name, services);
    }


    /**
   * Register new Provider.
   *
   * @param className
   * @param services
   */
    ///ssssssssssssssssssssssssssssssssssssssss
    export function registerProvider(className: string, obj: any) {
        var nice: ng.IServiceProviderClass;
        nice = obj;
        app.global.typesCache.add(className, nice);
        angular.module(className + "Provider").provider(className, nice);

    }

    export interface IService {

    }


    ///-----------------------------------
    /// global functions
    ///-----------------------------------

    export function showRegistrations(mod, r) {
        var inj = angular.element(document).injector().get;
        if (!r) r = {};
        angular.forEach(angular.module(mod).requires, m => { showRegistrations(m, r); });
        var queue: any = angular.module(mod);
        angular.forEach(queue._invokeQueue, a => {
            try {
                 r[a[2][0]] = inj(a[2][0]);
                app.log.debug(inj(a[2][0]));

            } catch (e) {
                app.log.debug("Error",e);
            }
        });
        return r;
    };


    export class InstanceLoader {
        static getInstance<T>(context: Object, name: string, ...args: any[]): T {
            var instance = Object.create(context[name].prototype);
            instance.constructor.apply(instance, args);
            return <T> instance;
        }

        static create<T>() {
            return <any>{};
        }
    }

    // for application health logging  -- for business app logging use app.common.$log.error
    export class log {

        public static debug(message?: any, ...optionalParams: any[]) {
           // console.debug(message, optionalParams);
        }

        public static info(message?: any, ...optionalParams: any[]) {
            console.info(message, optionalParams);
        }

        public static warn(message?: any, ...optionalParams: any[]) {
            console.warn(message, optionalParams);
        }

        public static error(message?: any, ...optionalParams: any[]) {
            console.error(message, optionalParams);
        }

        public static clear() {
            console.log(new Array(24 + 1).join("\n"));
        }
    }

    export function redirectToRoute(route: ng.ui.IState) {
        app.redirectToUrl(route.url);
    }

    export function redirectToUrl(url: string) {
        app.resolveByName<ng.ILocationService>("$location").path(url);
    }

    export function resolveService<T>(svc: IService): T {
        var neat = app.Describer.getName(svc);
        var cache = app.global.typesCache.getByKey(neat);
        if (cache) {
            app.log.debug("Found in type cache " + neat);
            return cache;
        }
        var obj = InstanceLoader.getInstance<T>(app.services, neat, services);
        return obj;
    }

    export function resolveByName<T>(name: string, services: any[]= []) {
        name = name[0].toUpperCase() + name.slice(1);

        app.log.debug("resolveByName:" + name);
        if (app.global.typesCache.getByKey(name)) {
            app.log.debug("Found in type cache " + name);
            return app.global.typesCache.getByKey(name);
        }

        app.log.debug("$injector resolving:" + name);
        if (app.global.$injector) {

            try {
                var found = app.global.$injector.get(name);
                if (found) {
                    app.global.typesCache.add(name, found);
                    app.log.debug("Success");
                    return found;
                }

            } catch (e) {
                console.warn(name + " not resolved");
                var svc = app.services[name];
                if (svc) {
                    app.global.typesCache.add(name, svc);
                    app.log.debug(name + " deep down resolved");
                    return svc;
                }
                throw new EvalError(name + " Could not be resolved");
            }
        } else {
            app.log.debug(name + " not resolved");

            var obj = InstanceLoader.getInstance<T>(window, name, services);
            if (obj) {
                app.log.debug(name + " resolved (finally)");
                return obj;
            }
            var svc2 = app.services[name](services);
            if (svc2) {
                app.global.typesCache.add(name, found);
                app.log.debug(name + " deep down finally resolved");
                return svc2;
            }
            throw new EvalError(name + " Could (2) not be resolved");
        }
        throw new EvalError(name + " Could not be resolved - end if nigh ");
    }

    ///http://www.stevefenton.co.uk/Content/Blog/Date/201304/Blog/Obtaining-A-Class-Name-At-Runtime-In-TypeScript/
    export class Describer {
        static getName(ent) {
            if (typeof ent == "string") return ent;

            if (ent.constructor && ent.constructor.name != "Function") {
                return ent.constructor.name || (ent.toString().match(/function (.+?)\(/) || [, ''])[1];
            } else {
                return ent.name;
            }
            //var funcNameRegex = /function (.{1,})\(/;
            //var results = (funcNameRegex).exec((<any> inputClass).constructor.toString());
            //return (results && results.length > 1) ? results[1] : "";
        }
    }

}
