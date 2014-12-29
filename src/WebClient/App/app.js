/// <reference path="_references.ts" />
/* tslint:disable */
//https://gist.github.com/scottmcarthur/9051681
"use strict";
var _this = this;
// Create and register modules
var modules = ["app.directives", "app.filters", "app.services", "app.controllers"];
modules.forEach(function (m) {
    return angular.module(m, []);
});

modules.push("ui", "ngCookies", "ngGrid", "ngCookies", "ngAnimate", "ngSanitize", "ngResource", "ui.router", "ui.bootstrap", "dialogs.main", "dialogs.default-translations", "pascalprecht.translate");

angular.module("app", modules).config([
    "$stateProvider", "$urlRouterProvider", "$injector", "$locationProvider", "$httpProvider", "$controllerProvider",
    function ($stateProvider, $urlRouterProvider, $injector, $locationProvider, $httpProvider, $controllerProvider) {
        app.log.debug("app.config started...");
        $controllerProvider.allowGlobals(); //http://www.lcube.se/angular-js-controller-error-argument-is-not-a-function/

        app.global.typesCache.add("$stateProvider", $stateProvider);
        app.global.typesCache.add("$urlRouterProvider", $urlRouterProvider);
        app.global.typesCache.add("$injector", $injector);
        app.global.typesCache.add("$locationProvider", $locationProvider);
        app.global.typesCache.add("$httpProvider", $httpProvider);

        //  $httpProvider.defaults.headers.common["X-Requested-With"] = "XMLHttpRequest";
        //// allow case insensitive urls
        $urlRouterProvider.rule(function ($injector, $location) {
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
        angular.forEach(app.Routes.getRoutes(), function (x) {
            return $stateProvider.state(x.name, x);
        });

        $stateProvider.state("otherwise", {
            url: "*path",
            templateUrl: "app/404.cshtml",
            name: "404"
        });

        app.global.angularModuleReference = _this;

        app.log.debug("app.config finished...");
    }
]).run([
    "$rootScope", "$log", "$http", "$state", "$stateParams", "$location", "$injector", "$q", "$timeout", "$window", "$templateCache",
    function ($rootScope, $log, $http, $state, $stateParams, $location, $injector, $q, $timeout, $window, $templateCache) {
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

        $rootScope.$on("$stateNotFound", function (event, unfoundState, fromState, fromParams) {
            app.log.debug("State not found"); // "lazy.state"
            app.log.debug("$stateNotFound"); // "lazy.state"
        });

        ///https://github.com/angular-ui/ui-router/wiki#state-change-events
        $rootScope.$on("$stateChangeStart", function (event, toState, toParams, fromState, fromParams) {
            app.log.debug("$stateChangeStart:" + toState.name);
            app.fn.spinStart();
        });

        $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
            app.log.debug("$stateChangeStart:" + toState.name);
            app.fn.spinStop();
            app.global.stateCurrent = toState;
            app.global.statePrevious = fromState;
        });

        // app services init
        app.common = new app.services.Common($rootScope, $log, $timeout, $http, $q);
        app.rootScope = $rootScope;

        app.log.debug("activating services");
        angular.forEach(app.global.serviceNames, function (service) {
            var act = app.global.typesCache.getByKey(service);
            act.activate();
        });

        app.log.debug("app.run finished...");
        $timeout(function () {
            return app.log.debug("timeout callback - state name : " + $state.current.name);
        }, 5000);

        app.redirectToRoute(app.Routes.home);
        //$timeout(() => {
        //    app.log.info("-- ALL SERVICES --");
        //    app.showRegistrations("app",null);
        //}, 5000
        //    );
    }
]);

var app;
(function (app) {
    var Item = (function () {
        function Item(name, object) {
            this.name = name;
            this.object = object;
        }
        return Item;
    })();
    app.Item = Item;

    var Dictionary = (function () {
        function Dictionary() {
            this.items = [];
        }
        Dictionary.prototype.add = function (key, value) {
            if (value && typeof value != "undefined") {
                this.items.push(value);
                this.items[key] = value;
            } else {
                app.log.error("Failed to add item to app cache (null) - " + key);
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
    app.Dictionary = Dictionary;

    app.common;
    app.rootScope;

    var fn = (function () {
        function fn() {
        }
        fn.appUrl = function (url) {
            return "/api/" + url;
        };

        fn.createGuid = function () {
            var d = new Date().getTime();
            var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = (d + Math.random() * 16) % 16 | 0;
                d = Math.floor(d / 16);
                return (c == 'x' ? r : (r & 0x7 | 0x8)).toString(16);
            });
            return uuid;
        };

        fn.getCurrentDate = function () {
            return moment();
        };

        fn.textContains = function (text, searchText) {
            return text && -1 !== text.toLowerCase().indexOf(searchText.toLowerCase());
        };

        fn.isNumber = function (val) {
            // negative or positive
            return /^[-]?\d+$/.test(val);
        };

        fn.applyConstructor = function (constr, args) {
            var obj = Object.create(constr.prototype);
            var services = [];
            angular.forEach(args, function (x) {
                try  {
                    services.push(app.resolveByName(x));
                } catch (e) {
                    services.push(x);
                }
            });
            constr.apply(obj, services);
            return obj;
        };

        fn.prototype.addDays = function (date, days) {
            var result = new Date(date);
            result.setDate(date.getDate() + days);
            return result;
        };

        fn.prototype.makeModel = function (controllerId) {
            app.fn.safeApply(function () {
                // if (typeof this.$scope.vm === "undefined") {
                app.log.debug("making model for " + controllerId);
                var name = app.fn.capitaliseFirstLetter(controllerId) + "Model";
                try  {
                    var made = InstanceLoader.getInstance(app.controllers, name, []);
                    return made;
                } catch (ex) {
                    app.log.error("makeModel error - " + controllerId, ex);
                    return null;
                }
                //  }
            });

            return null;
        };

        fn.copyProperties = function (source, target) {
            for (var prop in source) {
                if (target[prop] !== undefined) {
                    target[prop] = source[prop];
                } else {
                    console.error("Cannot set undefined property: " + prop);
                }
            }
        };

        fn.capitaliseFirstLetter = function (str) {
            return str.charAt(0).toUpperCase() + str.slice(1);
        };

        fn.spinStart = function (key) {
            if (typeof key === "undefined") { key = "spinner"; }
            app.common.broadcast("cc-spinner:spin", key);
        };

        fn.spinStop = function (key) {
            if (typeof key === "undefined") { key = "spinner"; }
            app.common.broadcast("cc-spinner:stop", key);
        };

        fn.replaceLocationUrlGuidWithId = function (id) {
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
        };

        fn.safeApply = function (fn) {
            var phase = app.rootScope.$root.$$phase;
            if (phase == '$apply' || phase == '$digest') {
                if (fn && (typeof (fn) === 'function')) {
                    fn();
                }
            } else {
                app.rootScope.$apply().$apply(fn);
            }
        };
        return fn;
    })();
    app.fn = fn;

    var global = (function () {
        function global() {
        }
        global.typesCache = new Dictionary();
        global.$injector = angular.injector(["ng"]);

        global.$scope = "$scope";
        global.$location = "$location";
        global.$log = "$log";
        global.$cookieStore = "$cookieStore";
        global.$ngCookies = "$ngCookies";
        global.$http = "$http";
        global.$resource = "$resource";
        global.$state = "$state";
        global.$timeout = "$q";
        global.$q = "$q";

        global.appName = "app";

        global.appControllers = "app.controllers";
        global.appDirectives = "app.directives";
        global.appServices = "app.services";
        global.appModels = "app.models";
        global.appProviders = "app.providers";
        global.appFilters = "app.filters";
        global.serviceNames = [];
        global.nulloDate = new Date(1900, 0, 1);
        global.standardDateFormat = "DD/MM/YYYY";

        global.keyCodes = {
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

        global.events = {
            controllerActivateSuccess: "controller.activateSuccess"
        };
        return global;
    })();
    app.global = global;

    (function (base) {
        var ModelBase = (function () {
            function ModelBase() {
                this.hasLoaded = false;
            }
            return ModelBase;
        })();
        base.ModelBase = ModelBase;

        var ControllerBase = (function () {
            function ControllerBase() {
                var _this = this;
                this.hasLoaded = false;
                this.controllerId = typeof (this.constructor.prototype.name);
                this.activate = function () {
                    app.log.debug("Base controller loaded for " + _this.controllerId);
                    _this.activateController([], _this.controllerId);
                };
            }
            ControllerBase.prototype.activateController = function (promises, controllerId) {
                var _this = this;
                return app.common.$q.all(promises).then(function (eventArgs) {
                    app.log.debug("Controller loaded : " + _this.controllerId);

                    //app.log.debug(eventArgs);
                    var data = { controllerId: _this.controllerId, eventArgs: eventArgs };
                    app.common.broadcast(app.global.events.controllerActivateSuccess, data);
                });
            };
            return ControllerBase;
        })();
        base.ControllerBase = ControllerBase;

        var ServiceBase = (function () {
            function ServiceBase(serviceUri, entityName) {
                var _this = this;
                this.serviceUri = serviceUri;
                this.entityName = entityName;
                this.getAll = function () {
                    return app.common.$http({ method: 'GET', url: app.fn.appUrl(_this.serviceUri + '/list/all') });
                };
                this.create = function (model) {
                    return app.common.$http({
                        method: 'POST',
                        url: app.fn.appUrl(_this.serviceUri),
                        data: model
                    });
                };
                this.update = function (model, identity) {
                    return app.common.$http({
                        method: 'PUT',
                        url: app.fn.appUrl(_this.serviceUri + '/' + identity),
                        data: model
                    });
                };
                this.get = function (identity) {
                    return app.common.$http({
                        method: 'GET',
                        url: app.fn.appUrl(_this.serviceUri + '/' + identity)
                    });
                };
                this.doDelete = function (identity) {
                    return app.common.$http({
                        method: 'DELETE',
                        url: app.fn.appUrl(_this.serviceUri + '/' + identity)
                    });
                };
            }
            ServiceBase.prototype.activate = function () {
            };
            return ServiceBase;
        })();
        base.ServiceBase = ServiceBase;
    })(app.base || (app.base = {}));
    var base = app.base;

    (function (model) {
        null;
    })(app.model || (app.model = {}));
    var model = app.model;

    (function (filters) {
        null;
    })(app.filters || (app.filters = {}));
    var filters = app.filters;

    (function (directives) {
        null;
    })(app.directives || (app.directives = {}));
    var directives = app.directives;

    (function (controllers) {
        null;
    })(app.controllers || (app.controllers = {}));
    var controllers = app.controllers;

    (function (services) {
        null;
    })(app.services || (app.services = {}));
    var services = app.services;

    /**
    * Register new controller.
    *
    * @param className
    * @param services
    */
    function registerController(className, ctor) {
        if (typeof ctor === "undefined") { ctor = null; }
        app.log.info("controllers regististration for " + className);
        angular.module(app.global.appControllers).controller(className, ctor);
    }
    app.registerController = registerController;

    /**
    * Register new filter.
    *
    * @param className
    * @param services
    */
    function registerFilter(className, services) {
        if (typeof services === "undefined") { services = []; }
        var filter = "app.filters." + className;
        app.log.debug("Registering filter: " + filter);
        services.push(new app.filters[className]().filter);
        angular.module("app.filters").filter(className, services);
    }
    app.registerFilter = registerFilter;

    /**
    * Register new value.
    
    */
    function registerValue(name, obj) {
        app.global.typesCache.add(name, obj);
        angular.module("app").value(name, obj);
    }
    app.registerValue = registerValue;

    /**
    * Register new directive.
    *
    * @param className
    * @param services
    */
    function registerDirective(className, services) {
        if (typeof services === "undefined") { services = []; }
        //var directive = className[0].toLowerCase() + className.slice(1);
        app.log.debug("Registering directive: " + className);
        services.push(function () {
            return new app.directives[className]();
        });
        angular.module("app.directives").directive(className, function () {
            return new app.directives[className](services);
        });
    }
    app.registerDirective = registerDirective;

    /**
    * Register new service.
    *
    * @param className
    * @param services
    */
    // export function registerService(className: string, services = []) {
    function registerService(ctor, services, name) {
        if (typeof services === "undefined") { services = []; }
        if (typeof name === "undefined") { name = null; }
        try  {
            if (!name || name == "")
                name = app.Describer.getName(ctor);
            app.log.info("Registering Service -- " + name);
            var obj = app.fn.applyConstructor(ctor, services);
            services.push(function () {
                return obj;
            });
            angular.module(app.global.appName).service(name, services);
        } catch (e) {
            app.log.error(e);
        }
    }
    app.registerService = registerService;

    /**
    * Register new factory.
    *
    * @param className
    * @param factory
    */
    function registerFactory(ctor, services) {
        if (typeof services === "undefined") { services = []; }
        var name = app.Describer.getName(ctor);
        var obj = app.InstanceLoader.getInstance(app.services, name);
        obj.activate();
        app.global.typesCache.add(name, obj);
        app.log.info("Registering Factory -- " + name);
        services.push(ctor);
        angular.module(app.global.appServices).factory(name, services);
    }
    app.registerFactory = registerFactory;

    /**
    * Register new Provider.
    *
    * @param className
    * @param services
    */
    ///ssssssssssssssssssssssssssssssssssssssss
    function registerProvider(className, obj) {
        var nice;
        nice = obj;
        app.global.typesCache.add(className, nice);
        angular.module(className + "Provider").provider(className, nice);
    }
    app.registerProvider = registerProvider;

    ///-----------------------------------
    /// global functions
    ///-----------------------------------
    function showRegistrations(mod, r) {
        var inj = angular.element(document).injector().get;
        if (!r)
            r = {};
        angular.forEach(angular.module(mod).requires, function (m) {
            showRegistrations(m, r);
        });
        var queue = angular.module(mod);
        angular.forEach(queue._invokeQueue, function (a) {
            try  {
                r[a[2][0]] = inj(a[2][0]);
                app.log.debug(inj(a[2][0]));
            } catch (e) {
                app.log.debug("Error", e);
            }
        });
        return r;
    }
    app.showRegistrations = showRegistrations;
    ;

    var InstanceLoader = (function () {
        function InstanceLoader() {
        }
        InstanceLoader.getInstance = function (context, name) {
            var args = [];
            for (var _i = 0; _i < (arguments.length - 2); _i++) {
                args[_i] = arguments[_i + 2];
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
    app.InstanceLoader = InstanceLoader;

    // for application health logging  -- for business app logging use app.common.$log.error
    var log = (function () {
        function log() {
        }
        log.debug = function (message) {
            var optionalParams = [];
            for (var _i = 0; _i < (arguments.length - 1); _i++) {
                optionalParams[_i] = arguments[_i + 1];
            }
            // console.debug(message, optionalParams);
        };

        log.info = function (message) {
            var optionalParams = [];
            for (var _i = 0; _i < (arguments.length - 1); _i++) {
                optionalParams[_i] = arguments[_i + 1];
            }
            console.info(message, optionalParams);
        };

        log.warn = function (message) {
            var optionalParams = [];
            for (var _i = 0; _i < (arguments.length - 1); _i++) {
                optionalParams[_i] = arguments[_i + 1];
            }
            console.warn(message, optionalParams);
        };

        log.error = function (message) {
            var optionalParams = [];
            for (var _i = 0; _i < (arguments.length - 1); _i++) {
                optionalParams[_i] = arguments[_i + 1];
            }
            console.error(message, optionalParams);
        };

        log.clear = function () {
            console.log(new Array(24 + 1).join("\n"));
        };
        return log;
    })();
    app.log = log;

    function redirectToRoute(route) {
        app.redirectToUrl(route.url);
    }
    app.redirectToRoute = redirectToRoute;

    function redirectToUrl(url) {
        app.resolveByName("$location").path(url);
    }
    app.redirectToUrl = redirectToUrl;

    function resolveService(svc) {
        var neat = app.Describer.getName(svc);
        var cache = app.global.typesCache.getByKey(neat);
        if (cache) {
            app.log.debug("Found in type cache " + neat);
            return cache;
        }
        var obj = InstanceLoader.getInstance(app.services, neat, services);
        return obj;
    }
    app.resolveService = resolveService;

    function resolveByName(name, services) {
        if (typeof services === "undefined") { services = []; }
        name = name[0].toUpperCase() + name.slice(1);

        app.log.debug("resolveByName:" + name);
        if (app.global.typesCache.getByKey(name)) {
            app.log.debug("Found in type cache " + name);
            return app.global.typesCache.getByKey(name);
        }

        app.log.debug("$injector resolving:" + name);
        if (app.global.$injector) {
            try  {
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

            var obj = InstanceLoader.getInstance(window, name, services);
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
    app.resolveByName = resolveByName;

    ///http://www.stevefenton.co.uk/Content/Blog/Date/201304/Blog/Obtaining-A-Class-Name-At-Runtime-In-TypeScript/
    var Describer = (function () {
        function Describer() {
        }
        Describer.getName = function (ent) {
            if (typeof ent == "string")
                return ent;

            if (ent.constructor && ent.constructor.name != "Function") {
                return ent.constructor.name || (ent.toString().match(/function (.+?)\(/) || [, ''])[1];
            } else {
                return ent.name;
            }
            //var funcNameRegex = /function (.{1,})\(/;
            //var results = (funcNameRegex).exec((<any> inputClass).constructor.toString());
            //return (results && results.length > 1) ? results[1] : "";
        };
        return Describer;
    })();
    app.Describer = Describer;
})(app || (app = {}));
