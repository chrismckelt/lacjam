/// <reference path="../_references.ts" />
var app;
(function (app) {
    (function (services) {
        "use strict";

        var BreezeService = (function () {
            function BreezeService($rootScope) {
                this.$rootScope = $rootScope;
                this.serviceId = "BreezeService";
                this.$get = function ($rootScope) {
                    return new BreezeService($rootScope);
                };
                app.log.debug("BreezeService ctor");

                // Tell breeze not to validate when we attach a newly created entity to any manager.
                // We could also set this per entityManager
                new breeze.ValidationOptions({ validateOnAttach: false }).setAsDefault();
                breeze.config.initializeAdapterInstance("modelLibrary", "backingStore", true);
                breeze.NamingConvention.camelCase.setAsDefault();

                this.activate();
            }
            BreezeService.prototype.activate = function () {
                if (!this.$rootScope)
                    this.$rootScope = app.resolveByName("$rootScope");

                if (!this.$rootScope)
                    this.$rootScope = app.resolveByName("$rootScope");

                this.entityManager = this.createManager();
            };

            BreezeService.prototype.createManager = function () {
                var _this = this;
                try  {
                    breeze.config.initializeAdapterInstance("modelLibrary", "backingStore", true);
                    breeze.NamingConvention.camelCase.setAsDefault();
                    var options = {
                        serviceName: app.global.config.remoteServiceName,
                        metadataStore: new breeze.MetadataStore()
                    };

                    this.entityManager = new breeze.EntityManager(options);

                    this.entityManager.fetchMetadata(function (cb) {
                        app.log.debug("entityManagerFactory --> fetchMetadata");

                        //  app.log.debug(cb);
                        cb.schema.entityType.forEach(function (type) {
                            //app.log.debug("entityManagerFactory --> registering breeze entity type - " + type.name);
                            _this.entityManager.metadataStore.setEntityTypeForResourceName(type.name, type.name);
                        });
                        toastr.info("metadata fetch complete");
                    });

                    return this.entityManager;
                } catch (e) {
                    app.log.error("Breeze error - metadata not loaded", e);
                    throw e;
                }
            };
            BreezeService.$inject = ["$rootScope"];
            return BreezeService;
        })();
        services.BreezeService = BreezeService;
    })(app.services || (app.services = {}));
    var services = app.services;
})(app || (app = {}));
//# sourceMappingURL=breezeService.js.map
