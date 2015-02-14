/// <reference path="_references.ts" />
/* tslint:disable */
//https://gist.github.com/scottmcarthur/9051681
"use strict";
define(["require", "exports"], function (require, exports) {
    var LacjamModule;
    (function (LacjamModule) {
        var Item = (function () {
            function Item(name, object) {
                this.name = name;
                this.object = object;
            }
            return Item;
        })();
        LacjamModule.Item = Item;
        var Dictionary = (function () {
            function Dictionary() {
                this.items = [];
            }
            Dictionary.prototype.add = function (key, value) {
                if (value) {
                    this.items.push(value);
                    this.items[key] = value;
                }
                else {
                    console.error("Failed to add item to lacjam cache (null) - " + key);
                }
            };
            Dictionary.prototype.getByIndex = function (index) {
                return this.items[index];
            };
            Dictionary.prototype.getByKey = function (key) {
                return this.items[key];
            };
            return Dictionary;
        })();
        LacjamModule.Dictionary = Dictionary;
        LacjamModule.common;
        LacjamModule.rootScope;
        var Fun = (function () {
            function Fun() {
            }
            Fun.appUrl = function (url) {
                return "/api/" + url;
            };
            Fun.createGuid = function () {
                var d = new Date().getTime();
                var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                    var r = (d + Math.random() * 16) % 16 | 0;
                    d = Math.floor(d / 16);
                    return (c == 'x' ? r : (r & 0x7 | 0x8)).toString(16);
                });
                return uuid;
            };
            Fun.getCurrentDate = function () {
                return new Date();
            };
            Fun.textContains = function (text, searchText) {
                return text && -1 !== text.toLowerCase().indexOf(searchText.toLowerCase());
            };
            Fun.isNumber = function (val) {
                // negative or positive
                return /^[-]?\d+$/.test(val);
            };
            Fun.prototype.addDays = function (date, days) {
                var result = new Date(date);
                result.setDate(date.getDate() + days);
                return result;
            };
            Fun.prototype.makeModel = function (controllerId) {
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
            };
            Fun.copyProperties = function (source, target) {
                for (var prop in source) {
                    if (target[prop] !== undefined) {
                        target[prop] = source[prop];
                    }
                    else {
                        console.error("Cannot set undefined property: " + prop);
                    }
                }
            };
            Fun.capitaliseFirstLetter = function (str) {
                return str.charAt(0).toUpperCase() + str.slice(1);
            };
            Fun.spinStart = function (key) {
                if (key === void 0) { key = "spinner"; }
                // lacjam.common.broadcast("cc-spinner:spin", key);
            };
            Fun.spinStop = function (key) {
                if (key === void 0) { key = "spinner"; }
                //lacjam.common.broadcast("cc-spinner:stop", key);
            };
            Fun.replaceLocationUrlGuidWithId = function (id) {
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
            };
            Fun.safeApply = function (fn) {
                //var phase = lacjam.rootScope.$root.$$phase;
                //if (phase == '$apply' || phase == '$digest') {
                //    if (fn && (typeof (fn) === 'function')) {
                //        fn();
                //    }
                //} else {
                //    //lacjam.rootScope.$apply().$apply(fn);
                //}
            };
            return Fun;
        })();
        LacjamModule.Fun = Fun;
        var Global = (function () {
            function Global() {
            }
            Global.typesCache = new Dictionary();
            Global.$scope = "$scope";
            Global.$location = "$location";
            Global.$log = "$log";
            Global.$cookieStore = "$cookieStore";
            Global.$ngCookies = "$ngCookies";
            Global.$http = "$http";
            Global.$resource = "$resource";
            Global.$state = "$state";
            Global.$timeout = "$q";
            Global.$q = "$q";
            Global.appName = "lacjam";
            Global.appControllers = "lacjam.controllers";
            Global.appDirectives = "lacjam.directives";
            Global.appServices = "lacjam.services";
            Global.appModels = "lacjam.models";
            Global.appProviders = "lacjam.providers";
            Global.appFilters = "lacjam.filters";
            Global.serviceNames = [];
            Global.nulloDate = new Date(1900, 0, 1);
            Global.standardDateFormat = "DD/MM/YYYY";
            Global.keyCodes = {
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
            Global.events = {
                controllerActivateSuccess: "controller.activateSuccess",
            };
            return Global;
        })();
        LacjamModule.Global = Global;
        var Base;
        (function (Base) {
            var ModelBase = (function () {
                function ModelBase() {
                    this.hasLoaded = false;
                }
                return ModelBase;
            })();
            Base.ModelBase = ModelBase;
            var ControllerBase = (function () {
                function ControllerBase() {
                    var _this = this;
                    this.hasLoaded = false;
                    this.controllerId = typeof (this.constructor.prototype.name);
                    this.activate = function () {
                        //lacjam.log.debug("Base controller loaded for " + this.controllerId);
                        _this.activateController([], _this.controllerId);
                    };
                }
                ControllerBase.prototype.activateController = function (promises, controllerId) {
                    //return lacjam.common.$q.all(promises)
                    //    .then(eventArgs => {
                    //    lacjam.log.debug("Controller loaded : " + this.controllerId);
                    //    //lacjam.log.debug(eventArgs);
                    //    var data = { controllerId: this.controllerId, eventArgs: eventArgs };
                    //    lacjam.common.broadcast(lacjam.global.events.controllerActivateSuccess, data);
                    //});
                };
                return ControllerBase;
            })();
            Base.ControllerBase = ControllerBase;
            var ServiceBase = (function () {
                function ServiceBase(serviceUri, entityName) {
                    this.serviceUri = serviceUri;
                    this.entityName = entityName;
                    this.getAll = function () {
                        // return $http({ method: 'GET', url: lacjam.fun.appUrl(this.serviceUri + '/list/all') });
                    };
                    this.create = function (model) {
                        //return lacjam.common.$http({
                        //    method: 'POST',
                        //    url: lacjam.fun.appUrl(this.serviceUri),
                        //    data: model
                        //});
                    };
                    this.update = function (model, identity) {
                        //return lacjam.common.$http({
                        //    method: 'PUT',
                        //    url: lacjam.fun.appUrl(this.serviceUri + '/' + identity),
                        //    data: model
                        //});
                    };
                    this.get = function (identity) {
                        //return lacjam.common.$http({
                        //    method: 'GET',
                        //    url: lacjam.fun.appUrl(this.serviceUri + '/' + identity)
                        //});
                    };
                    this.doDelete = function (identity) {
                        //return lacjam.common.$http({
                        //    method: 'DELETE',
                        //    url: lacjam.fun.appUrl(this.serviceUri + '/' + identity)
                        //});
                    };
                }
                ServiceBase.prototype.activate = function () {
                };
                return ServiceBase;
            })();
            Base.ServiceBase = ServiceBase;
        })(Base = LacjamModule.Base || (LacjamModule.Base = {}));
        var Model = (function () {
            function Model() {
            }
            return Model;
        })();
        LacjamModule.Model = Model;
        var Filters = (function () {
            function Filters() {
            }
            return Filters;
        })();
        LacjamModule.Filters = Filters;
        var Directives = (function () {
            function Directives() {
            }
            return Directives;
        })();
        LacjamModule.Directives = Directives;
        var Controllers = (function () {
            function Controllers() {
            }
            return Controllers;
        })();
        LacjamModule.Controllers = Controllers;
        var Services = (function () {
            function Services() {
            }
            return Services;
        })();
        LacjamModule.Services = Services;
        /**
        * Register new controller.
        *
        * @param className
        * @param services
        */
        function registerController(className, ctor) {
            // log.debug("controllers regististration for " + className);
            // angular.module("lacjam.controllers").controller(className, ctor);
            if (ctor === void 0) { ctor = null; }
        }
        LacjamModule.registerController = registerController;
        /**
         * Register new filter.
         *
         * @param className
         * @param services
         */
        function registerFilter(className, services) {
            if (services === void 0) { services = []; }
            var filter = "lacjam.filters." + className;
            //log.debug("Registering filter: " + filter);
            //  services.push(new lacjam.filters[className]().filter);
            //angular.module("lacjam.filters").filter(className, services);
        }
        LacjamModule.registerFilter = registerFilter;
        /**
     * Register new value.
    
     */
        function registerValue(name, obj) {
            Global.typesCache.add(name, obj);
            // angular.module("lacjam").value(name, obj);
        }
        LacjamModule.registerValue = registerValue;
        /**
         * Register new directive.
         *
         * @param className
         * @param services
         */
        function registerDirective(className, services) {
            if (services === void 0) { services = []; }
            //var directive = className[0].toLowerCase() + className.slice(1);
            // lacjam.log.debug("Registering directive: " + className);
            //services.push(() => new lacjam.directives[className]());
            //angular.module("lacjam.directives").directive(className,() => new lacjam.directives[className](services));
        }
        LacjamModule.registerDirective = registerDirective;
        /**
         * Register new service.
         *
         * @param className
         * @param services
         */
        // export function registerService(className: string, services = []) {
        function registerService(ctor, services) {
            if (services === void 0) { services = []; }
            //var name = lacjam.Describer.getName(ctor);
            //var obj = lacjam.InstanceLoader.getInstance(lacjam.services, name);
            //lacjam.global.typesCache.add(name, obj);
            //var arr = ["lacjam.services"];
            // angular.module("lacjam.services").service(name, arr);
        }
        LacjamModule.registerService = registerService;
        /**
     * Register new factory.
     *
     * @param className
     * @param factory
     */
        function registerFactory(ctor, services) {
            //try {
            //    var neat = lacjam.Describer.getName(ctor);
            //    var obj = InstanceLoader.getInstance(lacjam.services, neat, services);
            //    services.push(obj); // dynamic class creation in typescript
            //    lacjam.log.debug("Created :" + neat);
            //   // angular.module("lacjam.services").factory(neat, services);
            //    lacjam.global.typesCache.add(neat, obj);
            if (services === void 0) { services = []; }
            //} catch (e) {
            //    lacjam.log.error("factory registration failed - " + neat);
            //    lacjam.log.warn(e);
            //}
        }
        LacjamModule.registerFactory = registerFactory;
        /**
       * Register new Provider.
       *
       * @param className
       * @param services
       */
        ///ssssssssssssssssssssssssssssssssssssssss
        function registerProvider(className, obj) {
            //var nice: ng.IServiceProviderClass;
            //nice = obj;
            //lacjam.global.typesCache.add(className, nice);
            //angular.module(className + "Provider").provider(className, nice);
        }
        LacjamModule.registerProvider = registerProvider;
        ///-----------------------------------
        /// global functions
        ///-----------------------------------
        function showRegistrations(mod, r) {
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
        }
        LacjamModule.showRegistrations = showRegistrations;
        ;
        var InstanceLoader = (function () {
            function InstanceLoader() {
            }
            InstanceLoader.getInstance = function (context, name) {
                var args = [];
                for (var _i = 2; _i < arguments.length; _i++) {
                    args[_i - 2] = arguments[_i];
                }
                var instance = Object.create(context[name].prototype);
                instance.constructor.apply(instance, args);
                return instance;
            };
            InstanceLoader.create = function () {
                return {};
            };
            return InstanceLoader;
        })();
        LacjamModule.InstanceLoader = InstanceLoader;
        // for application health logging  -- for business lacjam logging use lacjam.common.$log.error
        var Log = (function () {
            function Log() {
            }
            Log.debug = function (message) {
                var optionalParams = [];
                for (var _i = 1; _i < arguments.length; _i++) {
                    optionalParams[_i - 1] = arguments[_i];
                }
                console.debug(message, optionalParams);
            };
            Log.info = function (message) {
                var optionalParams = [];
                for (var _i = 1; _i < arguments.length; _i++) {
                    optionalParams[_i - 1] = arguments[_i];
                }
                console.info(message, optionalParams);
            };
            Log.warn = function (message) {
                var optionalParams = [];
                for (var _i = 1; _i < arguments.length; _i++) {
                    optionalParams[_i - 1] = arguments[_i];
                }
                console.warn(message, optionalParams);
            };
            Log.error = function (message) {
                var optionalParams = [];
                for (var _i = 1; _i < arguments.length; _i++) {
                    optionalParams[_i - 1] = arguments[_i];
                }
                console.error(message, optionalParams);
            };
            Log.clear = function () {
                console.log(new Array(24 + 1).join("\n"));
            };
            return Log;
        })();
        LacjamModule.Log = Log;
        function redirectToRoute(route) {
            //  lacjam.resolveByName<anyService>("$state").go(route);
        }
        LacjamModule.redirectToRoute = redirectToRoute;
        function redirectToUrl(url) {
            //  lacjam.resolveByName<ng.ILocationService>("$location").path(url);
        }
        LacjamModule.redirectToUrl = redirectToUrl;
        function resolveService(svc) {
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
        LacjamModule.resolveService = resolveService;
        function resolveByName(name, services) {
            if (services === void 0) { services = []; }
            name = name[0].toUpperCase() + name.slice(1);
            throw new EvalError(name + " Could not be resolved - end if nigh ");
        }
        LacjamModule.resolveByName = resolveByName;
        ///http://www.stevefenton.co.uk/Content/Blog/Date/201304/Blog/Obtaining-A-Class-Name-At-Runtime-In-TypeScript/
        var Describer = (function () {
            function Describer() {
            }
            Describer.getName = function (ent) {
                if (typeof ent == "string")
                    return ent;
                if (ent.constructor && ent.constructor.name != "Function") {
                    return ent.constructor.name || (ent.toString().match(/function (.+?)\(/) || [, ''])[1];
                }
                else {
                    return ent.name;
                }
                //var funcNameRegex = /function (.{1,})\(/;
                //var results = (funcNameRegex).exec((<any> inputClass).constructor.toString());
                //return (results && results.length > 1) ? results[1] : "";
            };
            return Describer;
        })();
        LacjamModule.Describer = Describer;
    })(LacjamModule = exports.LacjamModule || (exports.LacjamModule = {}));
    var LacjamApp = (function () {
        function LacjamApp() {
            this.fun = new LacjamModule.Fun();
            this.global = new LacjamModule.Global();
            this.log = new LacjamModule.Log();
            this.controllers = new LacjamModule.Controllers();
            this.services = new LacjamModule.Services();
        }
        return LacjamApp;
    })();
    exports.LacjamApp = LacjamApp;
});
//# sourceMappingURL=lacjam.js.map