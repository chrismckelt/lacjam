/// <reference path="app/_references.ts" />

/* tslint:disable */
//https://gist.github.com/scottmcarthur/9051681

"use strict";

 export module LacjamModule {


    export class Item {
        constructor(public name: string, public object: any) {
        }
    }

    export class Dictionary<T> {
        private items = [];

        add(key: string, value: T) {
            if (value) {
                this.items.push(value);
                this.items[key] = value;
            } else {
                console.error("Failed to add item to lacjam cache (null) - " + key);
            }
        }

        getByIndex(index: number) {
            return this.items[index];
        }

        getByKey(key: string) {
            return this.items[key];
        }
    }

    export var common: any;
    export var rootScope: any;

    export class Fun {

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
            return new Date();
        }

        public static textContains(text: string, searchText: string) {
            return text && -1 !== text.toLowerCase().indexOf(searchText.toLowerCase());
        }

        public static isNumber(val) {
            // negative or positive
            return /^[-]?\d+$/.test(val);
        }

        public addDays(date, days) {
            var result = new Date(date);
            result.setDate(date.getDate() + days);
            return result;
        }

        public makeModel<T>(controllerId: string) {
            //lacjam.fun.safeApply(
            //    () => {
            //        // if (typeof this.$scope.vm === "undefined") {
            //        lacjam.log.debug("making model for " + controllerId);
            //        var name = lacjam.fun.capitaliseFirstLetter(controllerId) + "Model";
            //        try {
            //            var made = InstanceLoader.getInstance<T>(lacjam.controllers, name, []);
            //            return <T>made;

            //        } catch (ex) {
            //            lacjam.log.error("makeModel error - " + controllerId, ex);
            //            return null;
            //        }
            //        //  }
            //    });

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

        public static spinStart(key = "spinner") {
           // lacjam.common.broadcast("cc-spinner:spin", key);
        }

        public static spinStop(key = "spinner") {
            //lacjam.common.broadcast("cc-spinner:stop", key);
        }

        public static replaceLocationUrlGuidWithId(id) {
            // If the current Url is a Guid, then we replace 
            // it with the passed in id. Otherwise, we exit.
            //var currentPath = lacjam.resolveByName("$location").path();
            //var slashPos = currentPath.lastIndexOf("/", currentPath.length - 2);
            //var currentParameter = currentPath.substring(slashPos - 1);

            //if (lacjam.fun.isNumber(currentParameter)) {
            //    return;
            //}

            //var newPath = currentPath.substring(0, slashPos + 1) + id;
            //lacjam.redirectToUrl(newPath);
        }

        public static safeApply(fn: () => any) {
            //var phase = lacjam.rootScope.$root.$$phase;
            //if (phase == '$apply' || phase == '$digest') {
            //    if (fn && (typeof (fn) === 'function')) {
            //        fn();
            //    }
            //} else {
            //    //lacjam.rootScope.$apply().$apply(fn);
            //}
        }
    }

    export class Global {

        public static angularModuleReference: any;
        public static typesCache: Dictionary<any> = new Dictionary<any>();
        public static $injector;

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

        public static appName: string = "lacjam";
        public static spinner: any;//Spinner;


        public static appControllers: string = "lacjam.controllers";
        public static appDirectives: string = "lacjam.directives";
        public static appServices: string = "lacjam.services";
        public static appModels: string = "lacjam.models";
        public static appProviders: string = "lacjam.providers";
        public static appFilters: string = "lacjam.filters";
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

    export module Base {

        export interface IScopeBase {
            vm: any;
            activator: ControllerBase
        }

        export class ModelBase {
            hasLoaded: boolean = false;
        }

        export class ControllerBase {
            public title: string;
            public hasLoaded = false;

            public controllerId: string = typeof (this.constructor.prototype.name);

            public activate = () => {
                //lacjam.log.debug("Base controller loaded for " + this.controllerId);
                this.activateController([], this.controllerId);

            }

            public activateController(promises: any[], controllerId: string): any {

                //return lacjam.common.$q.all(promises)
                //    .then(eventArgs => {
                //    lacjam.log.debug("Controller loaded : " + this.controllerId);
                //    //lacjam.log.debug(eventArgs);
                //    var data = { controllerId: this.controllerId, eventArgs: eventArgs };
                //    lacjam.common.broadcast(lacjam.global.events.controllerActivateSuccess, data);

                //});
            }
        }

        export class ServiceBase<T> implements IService {
            public entity: T;

            constructor(public serviceUri: string, public entityName: string) {

            }

            public activate() { }

            public getAll = () => {
               // return $http({ method: 'GET', url: lacjam.fun.appUrl(this.serviceUri + '/list/all') });
            }

            public create = (model: T) => {
                //return lacjam.common.$http({
                //    method: 'POST',
                //    url: lacjam.fun.appUrl(this.serviceUri),
                //    data: model
                //});
            }

            public update = (model: T, identity: any) => {
                //return lacjam.common.$http({
                //    method: 'PUT',
                //    url: lacjam.fun.appUrl(this.serviceUri + '/' + identity),
                //    data: model
                //});
            }

            public get = (identity: any) => {
                //return lacjam.common.$http({
                //    method: 'GET',
                //    url: lacjam.fun.appUrl(this.serviceUri + '/' + identity)
                //});
            }

            public doDelete = (identity: any) => {
                //return lacjam.common.$http({
                //    method: 'DELETE',
                //    url: lacjam.fun.appUrl(this.serviceUri + '/' + identity)
                //});
            }
        }
    }

    export class Model {
        null;
    }

    export class Filters {
        null;
    }

    export class Directives {
        null;
    }

    export class Controllers {
        null;
    }

    export class Services {
        null;
    }

    export interface IController {

    }

    export interface IDirective  {
        restrict: string;
        link($scope: any, element: JQuery, attrs: any): any;
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
       // log.debug("controllers regististration for " + className);
       // angular.module("lacjam.controllers").controller(className, ctor);

    }

    /**
     * Register new filter.
     *
     * @param className
     * @param services
     */
    export function registerFilter(className: string, services = []) {
        var filter = "lacjam.filters." + className;
        //log.debug("Registering filter: " + filter);
      //  services.push(new lacjam.filters[className]().filter);
        //angular.module("lacjam.filters").filter(className, services);
    }

    /**
 * Register new value.

 */
    export function registerValue(name: string, obj: any) {
        Global.typesCache.add(name, obj);
       // angular.module("lacjam").value(name, obj);
    }


    /**
     * Register new directive.
     *
     * @param className
     * @param services
     */
    export function registerDirective(className: string, services = []) {
        //var directive = className[0].toLowerCase() + className.slice(1);
       // lacjam.log.debug("Registering directive: " + className);
        //services.push(() => new lacjam.directives[className]());
        //angular.module("lacjam.directives").directive(className,() => new lacjam.directives[className](services));
    }


    /**
     * Register new service.
     *
     * @param className
     * @param services
     */
    // export function registerService(className: string, services = []) {
    export function registerService(ctor: any, services = []) {
        //var name = lacjam.Describer.getName(ctor);
        //var obj = lacjam.InstanceLoader.getInstance(lacjam.services, name);
        //lacjam.global.typesCache.add(name, obj);
        //var arr = ["lacjam.services"];
       // angular.module("lacjam.services").service(name, arr);
    }


    /**
 * Register new factory.
 *
 * @param className
 * @param factory
 */
    export function registerFactory(ctor: any, services: any = []) {


        //try {
        //    var neat = lacjam.Describer.getName(ctor);
        //    var obj = InstanceLoader.getInstance(lacjam.services, neat, services);
        //    services.push(obj); // dynamic class creation in typescript
        //    lacjam.log.debug("Created :" + neat);
        //   // angular.module("lacjam.services").factory(neat, services);
        //    lacjam.global.typesCache.add(neat, obj);


        //} catch (e) {
        //    lacjam.log.error("factory registration failed - " + neat);
        //    lacjam.log.warn(e);
        //}
    }


    /**
   * Register new Provider.
   *
   * @param className
   * @param services
   */
    ///ssssssssssssssssssssssssssssssssssssssss
    export function registerProvider(className: string, obj: any) {
        //var nice: ng.IServiceProviderClass;
        //nice = obj;
        //lacjam.global.typesCache.add(className, nice);
        //angular.module(className + "Provider").provider(className, nice);

    }

    export interface IService {

    }


    ///-----------------------------------
    /// global functions
    ///-----------------------------------

    export function showRegistrations(mod, r) {
        //var inj = angular.element(document).injector().get;
        //if (!r) r = {};
        //angular.forEach(angular.module(mod).requires, m => { showRegistrations(m, r); });
        //var queue: any = angular.module(mod);
        //angular.forEach(queue._invokeQueue, a => {
        //    try {
        //        r[a[2][0]] = inj(a[2][0]);
        //        lacjam.log.debug(inj(a[2][0]));

        //    } catch (e) {
        //        lacjam.log.debug("Error", e);
        //    }
        //});
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

    // for application health logging  -- for business lacjam logging use lacjam.common.$log.error
    export class Log {

        public static debug(message?: any, ...optionalParams: any[]) {
            console.debug(message, optionalParams);
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

    export function redirectToRoute(route: any) {
      //  lacjam.resolveByName<anyService>("$state").go(route);
    }

    export function redirectToUrl(url: string) {
      //  lacjam.resolveByName<ng.ILocationService>("$location").path(url);
    }

    export function resolveService<T>(svc: IService): T {
        //var neat = lacjam.Describer.getName(svc);
        //var cache = Global.typesCache.getByKey(neat);
        //if (cache) {
        //    log.debug("Found in type cache " + neat);
        //    return cache;
        //}
        //var obj = InstanceLoader.getInstance<T>(lacjam.services, neat, services);
        //return obj;
        return null;
    }

    export function resolveByName<T>(name: string, services: any[] = []) {
        name = name[0].toUpperCase() + name.slice(1);

        //lacjam.log.debug("resolveByName:" + name);
        //if (lacjam.global.typesCache.getByKey(name)) {
        //    lacjam.log.debug("Found in type cache " + name);
        //    return lacjam.global.typesCache.getByKey(name);
        //}

        //lacjam.log.debug("$injector resolving:" + name);
        //if (lacjam.global.$injector) {

        //    try {
        //        var found = lacjam.global.$injector.get(name);
        //        if (found) {
        //            lacjam.global.typesCache.add(name, found);
        //            lacjam.log.debug("Success");
        //            return found;
        //        }

        //    } catch (e) {
        //        console.warn(name + " not resolved");
        //        var svc = lacjam.services[name];
        //        if (svc) {
        //            lacjam.global.typesCache.add(name, svc);
        //            lacjam.log.debug(name + " deep down resolved");
        //            return svc;
        //        }
        //        throw new EvalError(name + " Could not be resolved");
        //    }
        //} else {
        //    lacjam.log.debug(name + " not resolved");

        //    var obj = InstanceLoader.getInstance<T>(window, name, services);
        //    if (obj) {
        //        lacjam.log.debug(name + " resolved (finally)");
        //        return obj;
        //    }
        //    var svc2 = lacjam.services[name](services);
        //    if (svc2) {
        //        lacjam.global.typesCache.add(name, found);
        //        lacjam.log.debug(name + " deep down finally resolved");
        //        return svc2;
        //    }
        //    throw new EvalError(name + " Could (2) not be resolved");
        //}
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

 export class LacjamApp {

     public fun: LacjamModule.Fun = new LacjamModule.Fun();
     public global: LacjamModule.Global = new LacjamModule.Global();
     public log: LacjamModule.Log = new LacjamModule.Log();
     public controllers: LacjamModule.Controllers = new LacjamModule.Controllers();
     public services: LacjamModule.Services = new LacjamModule.Services();

 }

//define('LacjamModule', ["exports"], function (exports) {

//    var lac = LacjamModule;
//     declare var lacjam = lac;
//});


import lacjam = LacjamModule
