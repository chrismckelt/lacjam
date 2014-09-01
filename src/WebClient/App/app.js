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

modules.push("ngCookies", "ngGrid", "ngCookies", "ngAnimate", "ngSanitize", "ngResource", "ui.router", "ui.bootstrap", "ui.scrollfix", "ui.select2", "common.bootstrap", "breeze.angular", "breeze.directives", "ui.bootstrap", "LocalStorageModule", "multi-select", "ngGrid", "yaru22.directives.hovercard", "angularFileUpload", "angularCharts", "ui.select2", "ui.utils", "ngzWip");

angular.module("app", modules).config([
    "$stateProvider", "$urlRouterProvider", "$injector", "$locationProvider", "$httpProvider",
    function ($stateProvider, $urlRouterProvider, $injector, $locationProvider, $httpProvider) {
        app.log.debug("app.config started...");

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

        app.registerServices();
        app.registerControllers();

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

        app.log.debug("app.config finished...");
    }
]).run([
    "$rootScope", "$log", "$http", "$state", "$stateParams", "$location", "$injector", "$q", "$timeout", "localStorageService", "$window",
    function ($rootScope, $log, $http, $state, $stateParams, $location, $injector, $q, $timeout, localStorageService, $window) {
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
        app.global.typesCache.add("localStorageService", localStorageService);
        app.global.typesCache.add("$q", $q);
        app.global.typesCache.add("$window", $window);

        $rootScope.$on("$stateNotFound", function (event, unfoundState, fromState, fromParams) {
            app.log.debug("State not found"); // "lazy.state"
            app.log.debug("$stateNotFound"); // "lazy.state"
            app.log.debug(unfoundState.to); // "lazy.state"
            app.log.debug(unfoundState.toParams); // {a:1, b:2}
            app.log.debug(unfoundState.options); // {inherit:false} + default options
        });

        ///https://github.com/angular-ui/ui-router/wiki#state-change-events
        $rootScope.$on("$stateChangeStart", function (event, toState, toParams, fromState, fromParams) {
            app.log.debug("$stateChangeStart:" + toState.name);
            app.spinStart();
        });

        $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
            app.log.debug("$stateChangeStart:" + toState.name);
            app.spinStop();
        });

        // app services init
        app.common = new app.services.Common($rootScope, $log, $timeout, $http, $q);
        app.breezeService = new app.services.BreezeService(_this.$rootScope);
        app.storage = localStorageService;

        app.log.debug("activating services");
        angular.forEach(app.global.serviceNames, function (service) {
            var act = app.global.typesCache.getByKey(service);
            act.activate();
        });

        if ($location.path() === "")
            $location.path("/home");

        $state.href(app.Routes.home.name);

        app.log.debug("app.run finished...");

        // $timeout(() => app.log.debug("timeout callback - state name : " + $state.current.name), 5000);
        //app.listAllServices(angular.module("app"));
        app.listAllServices(angular.module("app.services"));
        //app.listAllServices(angular.module("app.controllers"));
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
    app.breezeService;
    app.storage;

    var global = (function () {
        function global() {
        }
        global.getCurrentDate = function () {
            return moment();
        };

        global.textContains = function (text, searchText) {
            return text && -1 !== text.toLowerCase().indexOf(searchText.toLowerCase());
        };

        global.isNumber = function (val) {
            // negative or positive
            return /^[-]?\d+$/.test(val);
        };

        global.prototype.addDays = function (date, days) {
            var result = new Date(date);
            result.setDate(date.getDate() + days);
            return result;
        };

        global.copyProperties = function (source, target) {
            for (var prop in source) {
                if (target[prop] !== undefined) {
                    target[prop] = source[prop];
                } else {
                    console.error("Cannot set undefined property: " + prop);
                }
            }
        };
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

        global.imageSettings = {
            imageBasePath: "../content/images/",
            imagePeoplePath: "../content/images/people/",
            imageProjectsPath: "../content/images/projects/",
            imageVendorsPath: "../content/images/vendors/",
            unknownPersonImageSource: "unknown_person.jpg",
            unknownProjectImageSource: "unknown_project.jpg",
            unknownVendorImageSource: "unknown_company.jpg"
        };

        global.events = {
            controllerActivateSuccess: "controller.activateSuccess",
            hasChangesChanged: "datacontextService.hasChangesChanged",
            spinnerToggle: "spinner.toggle",
            entitiesChanged: "datacontextService.entitiesChanged"
        };

        global.config = {
            appErrorPrefix: "[JOS Error] ",
            docTitle: "JAXON JOS: ",
            version: "2.1.0",
            showToasts: false,
            remoteServiceName: "api/breeze/Breeze"
        };

        global.entityNames = {
            address: "Address",
            addressType: "AddressType",
            annualHSEPerformanceDetail: "AnnualHSEPerformanceDetail",
            approver: "Approver",
            approvalItem: "ApprovalItem",
            bankGuarantee: "BankGuarantee",
            city: "City",
            company: "Company",
            companyAddress: "CompanyAddress",
            companyContactDetail: "CompanyContactDetail",
            companyPerson: "CompanyPerson",
            companyRole: "CompanyRole",
            companyTrade: "CompanyTrade",
            completionStage: "CompletionStage",
            //conditionOfSubcontract: "ConditionOfSubcontract",
            contactType: "ContactType",
            contractType: "ContractType",
            dailyDiaryItem: "DailyDiaryItem",
            equipmentModel: "EquipmentModel",
            equipmentType: "EquipmentType",
            formOfContract: "FormOfContract",
            formOfSecurity: "FormOfSecurity",
            generalQuestion: "GeneralQuestion",
            hseHazardImpact: "HSEHazardImpact",
            hseHazardImpactManagementType: "HSEHazardImpactManagementType",
            hSEIncident: "HSEIncident",
            hSEInjuryLocation: "HSEInjuryLocation",
            hSEInjuryNature: "HSEInjuryNature",
            hSEInjuryMechanism: "HSEInjuryMechanism",
            hSEInjuryType: "HSEInjuryType",
            hseSupplyPurchaseItem: "HSESupplyPurchaseItem",
            inspection: "Inspection",
            inspectionType: "InspectionType",
            internalLabourItem: "InternalLabourItem",
            jobRole: "JobRole",
            labourItem: "LabourItem",
            leadTimeItem: "LeadTimeItem",
            lostTimeHours: "LostTimeHours",
            marketSegment: "MarketSegment",
            meeting: "Meeting",
            meetingType: "MeetingType",
            meetingTypeConstruction: "Construction",
            meetingTypeHSE: "HSE",
            paymentTerm: "PaymentTerm",
            person: "Person",
            personContactDetail: "PersonContactDetail",
            plantEquipmentHire: "PlantEquipmentHire",
            project: "Project",
            projectAddress: "ProjectAddress",
            projectCompany: "ProjectCompany",
            projectCompanyPerson: "ProjectCompanyPerson",
            projectPerson: "ProjectPerson",
            projectType: "ProjectType",
            projectVendor: "ProjectVendor",
            qualityAccreditedSystem: "QualityAccreditedSystem",
            ratingCategory: "RatingCategory",
            ratingCategoryItem: "RatingCategoryItem",
            region: "Region",
            review: "Review",
            reviewRating: "ReviewRating",
            //scopeOfWork: "ScopeOfWork",
            securityType: "SecurityType",
            simpleProject: "SimpleProject",
            trade: "Trade",
            tradeCategory: "TradeCategory",
            user: "User",
            vendor: "Vendor",
            vendorLabourItem: "VendorLabourItem",
            vendorHSEHazardImpact: "VendorHSEHazardImpact",
            vendorHSEHazardImpactManagementType: "VendorHSEHazardImpactManagementType",
            vendorHSESupplyPurchaseItem: "VendorHSESupplyPurchaseItem",
            vendorQuestionAnswer: "VendorQuestionAnswer",
            workSafeNotice: "WorkSafeNotice"
        };

        global.vendorIds = {
            internalId: 7
        };

        global.addressTypes = {
            headOffice: 1
        };

        global.cityIds = {
            perth: 1
        };

        global.companyRoles = {
            clientId: 1,
            consultantId: 2,
            vendorId: 3
        };

        global.contractTypes = {
            other: 4
        };

        global.formsOfContract = {
            specify: 6
        };

        global.formsOfSecurity = {
            cashRetention: 1,
            bankGuarentee: 2,
            insuranceBond: 3
        };

        global.hseInjuryTypes = {
            fai: 1,
            mti: 2,
            lti: 3,
            fatality: 4,
            nmi: 5,
            ei: 6,
            pdi: 7
        };

        global.jobRoles = {
            projectManager: 1,
            generalManager: 2,
            financeManager: 4,
            other: 7,
            ceo: 16,
            hseq: 17
        };
        return global;
    })();
    app.global = global;

    (function (controllers) {
        var ModelBase = (function () {
            function ModelBase() {
            }
            return ModelBase;
        })();
        controllers.ModelBase = ModelBase;

        var ControllerBase = (function () {
            function ControllerBase(controllerId, $scope) {
                var _this = this;
                this.controllerId = controllerId;
                this.$scope = $scope;
                this.activate = function () {
                    app.log.debug("Base controller loaded for " + _this.controllerId);
                    _this.activateController([], _this.controllerId);
                };
                this.activateController = function (promises, controllerId) {
                    app.common.$q.all(promises).then(function (eventArgs) {
                        app.log.debug("Controller loaded : " + _this.controllerId);

                        //app.log.debug(eventArgs);
                        var data = { controllerId: _this.controllerId, eventArgs: eventArgs };
                        app.common.broadcast(app.global.events.controllerActivateSuccess, data);
                    });
                };
                /// promises completed
                this.activateControllerComplete = function () {
                    var args = [];
                    for (var _i = 0; _i < (arguments.length - 0); _i++) {
                        args[_i] = arguments[_i + 0];
                    }
                    app.log.debug("activateControllerComplete - " + _this.controllerId);
                };
                app.log.debug("ControllerBase -- ctor for " + this.controllerId);

                this.$rootScope = app.resolveByName("$rootScope");
                this.$rootScope.$on(app.global.events.controllerActivateSuccess, function (data, args) {
                    if (args[0].controllerId === _this.controllerId) {
                        _this.activateControllerComplete(args[0]);
                    }
                });

                app.log.debug("making model for " + this.controllerId);
                var name = app.capitaliseFirstLetter(this.controllerId) + "Model";
                try  {
                    var model = InstanceLoader.getInstance(app.controllers, name, []);
                    this.$scope.vm = model;
                    app.log.debug("scope made for " + this.controllerId, this.$scope.vm);
                } catch (ex) {
                    app.log.error("ControllerBase - " + this.controllerId, ex);
                }

                try  {
                    this.activate();
                } catch (e) {
                    app.log.error("ControllerBase - activate", e);
                }
            }
            return ControllerBase;
        })();
        controllers.ControllerBase = ControllerBase;
    })(app.controllers || (app.controllers = {}));
    var controllers = app.controllers;

    // *** Modules need to be populated to be correctly defined, otherwise they will give warnings. null fixes this ***/
    (function (directives) {
        null;
    })(app.directives || (app.directives = {}));
    var directives = app.directives;
    (function (filters) {
        null;
    })(app.filters || (app.filters = {}));
    var filters = app.filters;

    (function (model) {
        null;
    })(app.model || (app.model = {}));
    var model = app.model;

    (function (services) {
        var Service = (function () {
            function Service(entity) {
                var _this = this;
                this.entity = entity;
                this.serviceId = "Service";
                this.orderBy = "name";
                this.predicates = {
                    isNotNullo: breeze.Predicate.create("id", "!=", 0),
                    isNullo: breeze.Predicate.create("id", "==", 0)
                };
                //public getAll = () => {
                //    return breeze.EntityQuery.from(this.entityName)
                //        .using(app.breezeService.entityManager)
                //        .execute()
                //        .then(this.querySucceeded, this.queryFailed);
                //}
                this.getAll = function () {
                    return breeze.EntityQuery.from("Lookups").using(app.breezeService.entityManager).execute();
                };
                this.getAllLocal = function (resource, ordering, predicate) {
                    return breeze.EntityQuery.from(resource).orderBy(ordering).where(predicate).using(app.breezeService.entityManager).executeLocally();
                };
                this.getEntityByIdOrFromWip = function (val) {
                    // val could be an ID or a wipKey
                    var wipEntityKey = val;
                    var result;
                    var importedEntity = _this.zStorageWip.loadWipEntity(wipEntityKey);
                    if (importedEntity) {
                        // Need to re-validate the entity we are re-hydrating
                        importedEntity.entityAspect.validateEntity();
                        result = app.common.$q.when({ entity: importedEntity, key: wipEntityKey });
                    }
                    result = app.common.$q.reject({ error: "Couldn\"t find entity for WIP key " + wipEntityKey });

                    return result;
                };
                this.getByIdLocal = function (resource, id) {
                    var results = breeze.EntityQuery.from(resource).where("id", "eq", id).using(app.breezeService.entityManager).executeLocally();

                    if (results && results.length === 1) {
                        return results[0];
                    }

                    return null;
                };
            }
            Service.prototype.activate = function () {
                this.entityName = app.Describer.getName(this.entity);
                this.zStorageWip = app.resolveByName("zStorageWip");
            };

            Service.prototype.getById = function (id, en, forceRemote) {
                if (typeof forceRemote === "undefined") { forceRemote = false; }
                var result;
                if (!forceRemote) {
                    // check cache first
                    var entity = app.breezeService.entityManager.getEntityByKey(app.global.entityNames.project, id);
                    if (entity) {
                        app.log.debug("Retrieved [" + entity.entityType + "] id:" + entity.entityAspect + " from cache.", entity, true);
                        if (entity.entityAspect.entityState.isDeleted()) {
                            entity = null; // hide session marked-for-delete
                        }
                        result = app.common.$q.when(entity);
                    }
                }

                // Hit the server
                // It was not found in cache, so let"s query for it.
                result = app.breezeService.entityManager.fetchEntityByKey(en, id).then(this.querySucceeded).thenReject(toastr.error("Check console", "Error"));

                return result;
            };

            Service.prototype.querySucceeded = function (data) {
                if (!data.entity) {
                    app.log.debug("Could not find [" + data.entity.entityType + "] id:" + data.entity.id, null, true);
                    return null;
                }

                //  entity.isPartial = false;
                app.log.debug("Retrieved [" + data.entity.entityType + "] id " + data.entity.id + " from remote data source", data.entity, true);
                this.zStorageWip.save();
                return data.entity;
            };

            Service.prototype.getLocalEntityCount = function (resource) {
                var entities = breeze.EntityQuery.from(resource).where(this.predicates.isNotNullo).using(app.breezeService.entityManager).executeLocally();
                return entities.length;
            };

            Service.prototype.getInlineCount = function (data) {
                return data.inlineCount;
            };

            Service.prototype.queryFailed = function (error) {
                var msg = app.global.config.appErrorPrefix + "Error retrieving data." + error.message;
                app.common.$log.error(msg, error);
                throw error;
            };

            Service.prototype.setIsPartialTrue = function (entities) {
                for (var i = entities.length; i--;) {
                    entities[i].isPartial = true;
                }
                return entities;
            };
            return Service;
        })();
        services.Service = Service;
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
        //var controller = "app.controllers." + className;
        //app.log.debug("Registering controller: " + controller);
        //var ctrl: ng.IControllerService = InstanceLoader.getInstance<ng.IControllerService>(app.controllers, className, services);
        //angular.module("app.controllers").controller(ctrl);
        angular.module("app.controllers").controller(className, ctor);
    }
    app.registerController = registerController;

    /**
    
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
        services.push(function () {
            return (new app.filters[className]()).filter;
        });
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
    ///ssssssssssssssssssssssssssssssssssssssss
    // export function registerService(className: string, services = []) {
    function registerService(ctor, services) {
        if (typeof services === "undefined") { services = []; }
        try  {
            //var neat = className[0].toUpperCase() + className.slice(1);
            app.log.info(ctor);
            var neat = app.Describer.getName(ctor);
            app.global.serviceNames.push(neat);

            var service = "app.services." + neat;
            app.log.debug("Registering service: " + service);

            var obj = InstanceLoader.getInstance(app.services, neat, services);
            app.log.debug("Created :" + Describer.getName(obj));
            app.global.typesCache.add(neat, obj);
            services.push(obj);
            angular.module("app.services").service(neat, services);
            angular.module("app.services").service(service, services);
        } catch (e) {
            app.log.error("service registration failed - " + neat, e);
        }
    }
    app.registerService = registerService;

    /**
    * Register new factory.
    *
    * @param className
    * @param factory
    */
    ///ssssssssssssssssssssssssssssssssssssssss
    function registerFactory(ctor, services) {
        if (typeof services === "undefined") { services = []; }
        try  {
            var neat = app.Describer.getName(ctor);
            var obj = InstanceLoader.getInstance(app.services, neat, services);
            services.push(obj); // dynamic class creation in typescript
            app.log.debug("Created :" + neat);
            angular.module("app.services").factory(neat, services);
            app.global.typesCache.add(neat, obj);
        } catch (e) {
            app.log.error("factory registration failed - " + neat);
            app.log.warn(e);
        }
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
    function listAllServices(mod, r) {
        if (typeof r === "undefined") { r = {}; }
        angular.forEach(mod._invokeQueue, function (x) {
            app.log.debug(x);
        });

        return r;
    }
    app.listAllServices = listAllServices;
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
            console.debug(message, optionalParams);
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
        return log;
    })();
    app.log = log;

    function redirectToRoute(route) {
        app.resolveByName("$state").go(route);
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

    function capitaliseFirstLetter(str) {
        return str.charAt(0).toUpperCase() + str.slice(1);
    }
    app.capitaliseFirstLetter = capitaliseFirstLetter;

    function spinStart(key) {
        if (typeof key === "undefined") { key = "spinner"; }
        app.common.broadcast("cc-spinner:spin", key);
    }
    app.spinStart = spinStart;

    function spinStop(key) {
        if (typeof key === "undefined") { key = "spinner"; }
        app.common.broadcast("cc-spinner:stop", key);
    }
    app.spinStop = spinStop;

    function replaceLocationUrlGuidWithId(id) {
        // If the current Url is a Guid, then we replace
        // it with the passed in id. Otherwise, we exit.
        var currentPath = app.resolveByName("$location").path();
        var slashPos = currentPath.lastIndexOf("/", currentPath.length - 2);
        var currentParameter = currentPath.substring(slashPos - 1);

        if (app.global.isNumber(currentParameter)) {
            return;
        }

        var newPath = currentPath.substring(0, slashPos + 1) + id;
        app.redirectToUrl(newPath);
    }
    app.replaceLocationUrlGuidWithId = replaceLocationUrlGuidWithId;

    function registerServices() {
        // services
        app.log.debug("Registering services");
    }
    app.registerServices = registerServices;

    function registerControllers() {
    }
    app.registerControllers = registerControllers;
})(app || (app = {}));
//# sourceMappingURL=app.js.map
