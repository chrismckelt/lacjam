/// <reference path="../_references.ts" />
module app.services {

    export class MetadataDefinitionGroupService extends app.base.ServiceBase<app.model.MetadataDefinitionGroupResource> {
     
        constructor() {
            super("MetadataDefinitionGroup", "MetadataDefinitionGroup");
        }   

        public getDefinitions = (id: app.model.Guid) => {
            return app.common.$http({ method: 'GET', url: app.fn.appUrl(this.serviceUri + "/" + id + "/definitions") });
        }

        public getSearchUrl() {
            return app.fn.appUrl(this.serviceUri + "/select");
        }
    }
}

