/// <reference path="../_references.ts" />

module app.services {
    "use strict";

    export class BreezeService implements IService {
        public serviceId = "BreezeService";
        public static $inject = ["$rootScope"];
        public entityManager: breeze.EntityManager;

        public $get = (

            $rootScope: ng.IRootScopeService

            ) => new BreezeService($rootScope);


        constructor(
            private $rootScope: ng.IRootScopeService
            ) {
            app.log.debug("BreezeService ctor");
            // Tell breeze not to validate when we attach a newly created entity to any manager.
            // We could also set this per entityManager
            new breeze.ValidationOptions({ validateOnAttach: false }).setAsDefault();
            breeze.config.initializeAdapterInstance("modelLibrary", "backingStore", true);
            breeze.NamingConvention.camelCase.setAsDefault();

            this.activate();
        }

        public activate() {

            if (!this.$rootScope)
                this.$rootScope = app.resolveByName("$rootScope");

            if (!this.$rootScope)
                this.$rootScope = app.resolveByName("$rootScope");

            this.entityManager = this.createManager();

        }

        public createManager(): breeze.EntityManager {

            try {
                breeze.config.initializeAdapterInstance("modelLibrary", "backingStore", true);
                breeze.NamingConvention.camelCase.setAsDefault();
                var options: breeze.EntityManagerOptions = {
                    serviceName: app.global.config.remoteServiceName,
                    metadataStore: new breeze.MetadataStore(),

                };

                this.entityManager = new breeze.EntityManager(options);

                this.entityManager.fetchMetadata(cb => {
                    app.log.debug("entityManagerFactory --> fetchMetadata");
                    //  app.log.debug(cb);
                    cb.schema.entityType.forEach((type: any) => {
                        //app.log.debug("entityManagerFactory --> registering breeze entity type - " + type.name);
                        this.entityManager.metadataStore.setEntityTypeForResourceName(type.name, type.name);

                    });
                    toastr.info("metadata fetch complete");
                });

                return this.entityManager;
            } catch (e) {
                app.log.error("Breeze error - metadata not loaded", e);
                throw e;
            }
        }

    }
}

