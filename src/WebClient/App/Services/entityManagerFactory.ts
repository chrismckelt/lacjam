/// <reference path="../_references.ts" />
"use strict";

module app.services {

    export class EntityManagerFactory implements app.IService {
        public serviceId = "entityManagerFactory";
        public static $inject = ["$rootScope"];
        public entityManager: breeze.EntityManager;

        constructor(public $rootScope : ng.IRootScopeService) {

        }

        public activate() {

            // Tell breeze not to validate when we attach a newly created entity to any manager.
            // We could also set this per entityManager
            new breeze.ValidationOptions({ validateOnAttach: false }).setAsDefault();
            breeze.config.initializeAdapterInstance("modelLibrary", "backingStore", true);
            breeze.NamingConvention.camelCase.setAsDefault();


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

        private register(name: string, obj: any, resource?: string) {
            if (this.entityManager) {
                app.log.debug("entityManagerFactory registering --> " + name);

            } else {
                app.log.warn("FAILED entityManagerFactory registering --> " + name);
            }
        }


    }

}
