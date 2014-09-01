/// <reference path="_references.ts" />
/* tslint:disable */
//https://gist.github.com/scottmcarthur/9051681

"use strict";


// Create and register modules
var modules = ["app.directives", "app.filters", "app.services", "app.controllers"];

modules.forEach((m) => angular.module(m, []));

modules.push(
    "ngCookies",
    "ngGrid",
    "ngCookies",
    "ngAnimate", // animations
    "ngSanitize", // sanitizes html bindings (ex: Sidebar.js)
    "ngResource",
    "ui.router",
    "ui.bootstrap",
    "ui.scrollfix",
    "ui.select2",
    "common.bootstrap",


// 3rd Party Modules
    "breeze.angular", // configures breeze for an angular app
    "breeze.directives", // contains the breeze validation directive (zValidate)
    "ui.bootstrap", // ui-bootstrap (ex: carousel, pagination, dialog) 
    "LocalStorageModule", //Store data locally,


//"kendo.directives", //Kendo angular
    "multi-select", //multi-select control (eg 2 sets of boxes and pass data from one to other)
    "ngGrid", //Used in settings screens
    "yaru22.directives.hovercard", //Hovercard for displaying contacts etc https://github.com/yaru22/angular-hovercard
    "angularFileUpload", //File upload https://github.com/danialfarid/angular-file-upload
    "angularCharts", //Charts with D3 https://github.com/chinmaymk/angular-charts
    "ui.select2", //Select more than 1 item in select dropdown https://github.com/angular-ui/ui-select2"
    "ui.utils" //UI utility classes http://angular-ui.github.io/ui-utils/
    , "ngzWip" // local storage and WIP module
//   "angularSpinner"
);


angular.module("app", modules)
    .config([
        "$stateProvider", "$urlRouterProvider", "$injector", "$locationProvider", "$httpProvider",
        (
            $stateProvider: ng.ui.IStateProvider,
            $urlRouterProvider: ng.ui.IUrlRouterProvider,
            $injector: ng.auto.IInjectorService,
            $locationProvider: ng.ILocationProvider,
            $httpProvider: ng.IHttpProvider
        ) => {

            app.log.debug("app.config started...");

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

            app.registerServices();
            app.registerControllers();


            // state provider
            app.log.debug("Registering routes with state provider");
            // routing
            angular.forEach(app.Routes.getRoutes(), (x: ng.ui.IState) => $stateProvider.state(x.name, x));

            $stateProvider.state("otherwise", {
                url: "*path",
                templateUrl: "app/404.cshtml",
                name: "404"
            });


            app.log.debug("app.config finished...");
        }
    ])
    .run([
        "$rootScope", "$log", "$http", "$state", "$stateParams", "$location", "$injector", "$q", "$timeout", "localStorageService", "$window",
        ($rootScope: ng.IRootScopeService,
            $log: ng.ILogService,
            $http: ng.IHttpService,
            $state: ng.ui.IStateService,
            $stateParams: ng.ui.IStateParamsService,
            $location: ng.ILocationService,
            $injector: ng.auto.IInjectorService,
            $q: ng.IQService,
            $timeout: ng.ITimeoutService,
            localStorageService: ng.localStorage.ILocalStorageService,
            $window: ng.IWindowService
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
            app.global.typesCache.add("localStorageService", localStorageService);
            app.global.typesCache.add("$q", $q);
            app.global.typesCache.add("$window", $window);


            $rootScope.$on("$stateNotFound",
            (event, unfoundState, fromState, fromParams) => {
                app.log.debug("State not found"); // "lazy.state"
                app.log.debug("$stateNotFound"); // "lazy.state"
                app.log.debug(unfoundState.to); // "lazy.state"
                app.log.debug(unfoundState.toParams); // {a:1, b:2}
                app.log.debug(unfoundState.options); // {inherit:false} + default options
            });
            ///https://github.com/angular-ui/ui-router/wiki#state-change-events
            $rootScope.$on("$stateChangeStart",
            (event, toState, toParams, fromState, fromParams) => {
                app.log.debug("$stateChangeStart:" + toState.name);
                app.spinStart();
            });

            $rootScope.$on('$stateChangeSuccess',
            (event, toState, toParams, fromState, fromParams) => {
                app.log.debug("$stateChangeStart:" + toState.name);
                app.spinStop();
            });

            // app services init
            app.common = new app.services.Common($rootScope, $log, $timeout, $http, $q);
            app.breezeService = new app.services.BreezeService(this.$rootScope);
            app.storage = localStorageService;

            app.log.debug("activating services");
            angular.forEach(app.global.serviceNames, (service) => {
                    var act = app.global.typesCache.getByKey(service);
                    act.activate();
                }
            );


            if ($location.path() === "") $location.path("/home");

            $state.href(app.Routes.home.name);

            app.log.debug("app.run finished...");
            // $timeout(() => app.log.debug("timeout callback - state name : " + $state.current.name), 5000);

            //app.listAllServices(angular.module("app"));
            app.listAllServices(angular.module("app.services"));
            //app.listAllServices(angular.module("app.controllers"));


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
    export var breezeService: app.services.BreezeService;
    export var storage: ng.localStorage.ILocalStorageService;

    export class global {
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


        public static imageSettings = {
            imageBasePath: "../content/images/",
            imagePeoplePath: "../content/images/people/",
            imageProjectsPath: "../content/images/projects/",
            imageVendorsPath: "../content/images/vendors/",
            unknownPersonImageSource: "unknown_person.jpg",
            unknownProjectImageSource: "unknown_project.jpg",
            unknownVendorImageSource: "unknown_company.jpg"
        };

        public static events = {
            controllerActivateSuccess: "controller.activateSuccess",
            hasChangesChanged: "datacontextService.hasChangesChanged",
            spinnerToggle: "spinner.toggle",
            entitiesChanged: "datacontextService.entitiesChanged",
        };

        public static config = {
            appErrorPrefix: "[JOS Error] ", //Configure the exceptionHandler decorator
            docTitle: "JAXON JOS: ",
            version: "2.1.0",
            showToasts: false,
            remoteServiceName: "api/breeze/Breeze" //// For use with the HotTowel-Angular-Breeze add-on that uses Breeze
        };

        // Define the functions and properties to reveal.
        public static entityNames = {
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

        public static vendorIds = {
            internalId: 7
        }

        public static addressTypes = {
            headOffice: 1
        }

        public static cityIds = {
            perth: 1
        }

        public static companyRoles = {
            clientId: 1,
            consultantId: 2,
            vendorId: 3
        };

        public static contractTypes = {
            other: 4
        };

        public static formsOfContract = {
            specify: 6
        };

        public static formsOfSecurity = {
            cashRetention: 1,
            bankGuarentee: 2,
            insuranceBond: 3
        };

        public static hseInjuryTypes = {
            fai: 1,
            mti: 2,
            lti: 3,
            fatality: 4,
            nmi: 5,
            ei: 6,
            pdi: 7
        };

        public static jobRoles = {
            projectManager: 1,
            generalManager: 2,
            financeManager: 4,
            other: 7,
            ceo: 16,
            hseq: 17,
        };


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

        public addDays(date, days) {
            var result = new Date(date);
            result.setDate(date.getDate() + days);
            return result;
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
    }


    export module controllers {

        export interface IScopeBase extends ng.IScope {
            vm: any;
        }

        export class ModelBase {
            //  routes = app.routes;
        }

        export class ControllerBase<T extends ModelBase> {
            public $rootScope: ng.IRootScopeService;
            constructor(public controllerId: string, public $scope: IScopeBase) {
                app.log.debug("ControllerBase -- ctor for " + this.controllerId);

                this.$rootScope = app.resolveByName("$rootScope");
                this.$rootScope.$on(app.global.events.controllerActivateSuccess,
                    (data, args) => {
                        if (args[0].controllerId === this.controllerId) {
                            this.activateControllerComplete(args[0]);
                        }
                    }
                    );

                app.log.debug("making model for " + this.controllerId);
                var name = app.capitaliseFirstLetter(this.controllerId) + "Model";
                try {
                    var model = InstanceLoader.getInstance<T>(app.controllers, name, []);
                    this.$scope.vm = model;
                    app.log.debug("scope made for " + this.controllerId, this.$scope.vm);
                } catch (ex) {
                    app.log.error("ControllerBase - " + this.controllerId, ex);
                }

                try {
                    this.activate();
                } catch (e) {
                    app.log.error("ControllerBase - activate", e);
                }

            }


            public activate = () => {
                app.log.debug("Base controller loaded for " + this.controllerId);
                this.activateController([], this.controllerId);
            }

            public activateController = (promises: any[], controllerId: string): any => {

                app.common.$q.all(promises)
                    .then(eventArgs => {
                        app.log.debug("Controller loaded : " + this.controllerId);
                        //app.log.debug(eventArgs);
                        var data = { controllerId: this.controllerId, eventArgs: eventArgs };
                        app.common.broadcast(app.global.events.controllerActivateSuccess, data);
                
                });
            }

            /// promises completed
            public activateControllerComplete = (...args: any[]) => {
                app.log.debug("activateControllerComplete - " + this.controllerId);
            }
        }
    }

    // *** Modules need to be populated to be correctly defined, otherwise they will give warnings. null fixes this ***/

    export module directives {
        null;
    } // put null in so compiler does not remove 
    export module filters {
        null;
    }

    export module model {
        null;
    }

    export module services {

        export class Service<T> //implements IService
            //export class Service implements  IService
            {
                public serviceId = "Service";
                public entityName: string;
                public entityAspect: breeze.EntityAspect;
                public entityType: breeze.EntityType;
                public orderBy = "name";
                public zStorageWip: any;
                public predicates = {
                    isNotNullo: breeze.Predicate.create("id", "!=", 0),
                    isNullo: breeze.Predicate.create("id", "==", 0)
                };

                constructor(public entity: T) {

                }

                public activate() {
                    this.entityName = app.Describer.getName(this.entity);
                    this.zStorageWip = app.resolveByName("zStorageWip");

                }


                //public getAll = () => {

                //    return breeze.EntityQuery.from(this.entityName)
                //        .using(app.breezeService.entityManager)
                //        .execute()
                //        .then(this.querySucceeded, this.queryFailed);

                //}

             public getAll = () => {
                return breeze.EntityQuery.from("Lookups")
                    .using(app.breezeService.entityManager)
                //   .toType(resource)
                    .execute();
            }

                public getAllLocal = (resource, ordering, predicate) => {
                    return breeze.EntityQuery.from(resource)
                        .orderBy(ordering)
                        .where(predicate)
                        .using(app.breezeService.entityManager)
                        //   .toType(resource)
                        .executeLocally();
                }

                public getEntityByIdOrFromWip = (val) => {
                    // val could be an ID or a wipKey
                    var wipEntityKey = val;
                    var result: any;
                    var importedEntity = this.zStorageWip.loadWipEntity(wipEntityKey);
                    if (importedEntity) {
                        // Need to re-validate the entity we are re-hydrating
                        importedEntity.entityAspect.validateEntity();
                        result = app.common.$q.when({ entity: importedEntity, key: wipEntityKey });
                    }
                    result = app.common.$q.reject({ error: "Couldn\"t find entity for WIP key " + wipEntityKey });

                    return result;
                }

                public getByIdLocal = (resource, id) => {
                    var results = breeze.EntityQuery.from(resource)
                        .where("id", "eq", id)
                        .using(app.breezeService.entityManager)
                        .executeLocally();

                    if (results && results.length === 1) {
                        return results[0];
                    }

                    return null;
                }


                public getById(id, en: any, forceRemote: boolean= false) {
                    var result: any;
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
                    result = app.breezeService.entityManager.fetchEntityByKey(en, id)
                        .then(this.querySucceeded)
                        .thenReject(toastr.error("Check console", "Error"));

                    return result;
                }

                public querySucceeded(data) {

                    if (!data.entity) {
                        app.log.debug("Could not find [" + data.entity.entityType + "] id:" + data.entity.id, null, true);
                        return null;
                    }
                    //  entity.isPartial = false;
                    app.log.debug("Retrieved [" + data.entity.entityType + "] id " + data.entity.id
                        + " from remote data source", data.entity, true);
                    this.zStorageWip.save();
                    return data.entity;
                }


                public getLocalEntityCount(resource) {
                    var entities = breeze.EntityQuery.from(resource)
                        .where(this.predicates.isNotNullo)
                        .using(app.breezeService.entityManager)
                        .executeLocally();
                    return entities.length;
                }

                public getInlineCount(data) { return data.inlineCount; }

                public queryFailed(error) {
                    var msg = app.global.config.appErrorPrefix + "Error retrieving data." + error.message;
                    app.common.$log.error(msg, error);
                    throw error;
                }

                public setIsPartialTrue(entities) {
                    for (var i = entities.length; i--;) {
                        entities[i].isPartial = true;
                    }
                    return entities;
                }
            }
    }

    export interface IController {
        controllerId : string;
    }

    export interface IDirective extends ng.IDirective {
        restrict: string;
        link($scope: ng.IScope, element: JQuery, attrs: ng.IAttributes): any;
    }

    export interface IFilter {
        filter(input: any, ...args: any[]): any;
    }

    export interface IService {
        serviceId: string;
        activate: () => void;
    }



    /**
    * Register new controller.
    *
    * @param className
    * @param services
    */
    export function registerController(className: string, ctor: any = null) {
        //var controller = "app.controllers." + className;
        //app.log.debug("Registering controller: " + controller);
        //var ctrl: ng.IControllerService = InstanceLoader.getInstance<ng.IControllerService>(app.controllers, className, services);
        //angular.module("app.controllers").controller(ctrl);
        angular.module("app.controllers").controller(className, ctor);

    }

    /**

    /**
     * Register new filter.
     *
     * @param className
     * @param services
     */
    export function registerFilter(className: string, services = []) {
        var filter = "app.filters." + className;
        app.log.debug("Registering filter: " + filter);
        services.push(() => (new app.filters[className]()).filter);
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
    ///ssssssssssssssssssssssssssssssssssssssss
    // export function registerService(className: string, services = []) {
    export function registerService(ctor: any, services = []) {

        try {
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


    /**
 * Register new factory.
 *
 * @param className
 * @param factory
 */
    ///ssssssssssssssssssssssssssssssssssssssss
    export function registerFactory(ctor: any, services: any = []) {

        //var neat = className[0].toUpperCase() + className.slice(1);
        //app.global.serviceNames.push(neat);
        //var service = "app.services." + className;
        //app.log.debug("Registering factory: " + service);

        //var neat = className[0].toUpperCase() + className.slice(1);

        //      app.global.serviceNames.push(ctor);


        try {
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


    export function listAllServices(mod, r = {}) {

        angular.forEach(mod._invokeQueue, (x) => {
            app.log.debug(x);
        });

        return r;
    };


    export class InstanceLoader {
        static getInstance<T>(context: Object, name: string, ...args: any[]): T {
            var instance = Object.create(context[name].prototype);
            instance.constructor.apply(instance, args);
            return <T> instance;
        }
    }

    // for application health logging  -- for business app logging use app.common.$log.error
    export class log {

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
    }

    export function redirectToRoute(route: ng.ui.IState) {
        app.resolveByName<ng.ui.IStateService>("$state").go(route);
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

    export function capitaliseFirstLetter(str: string) {
        return str.charAt(0).toUpperCase() + str.slice(1);
    }


    export function spinStart(key= "spinner") {
        app.common.broadcast("cc-spinner:spin", key);
    }


    export function spinStop(key= "spinner") {
        app.common.broadcast("cc-spinner:stop", key);
    }


    export function replaceLocationUrlGuidWithId(id) {
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


    export function registerServices() {
        // services
        app.log.debug("Registering services");
    }


    export function registerControllers() {


    }

}


