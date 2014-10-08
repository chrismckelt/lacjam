/// <reference path="../_references.ts" />
module app.services {

    export class MetadataDefinitionService extends app.base.ServiceBase<app.model.MetadataDefinitionResource> {
     
        constructor() {
            super("MetadataDefinition", "MetadataDefinition");
        }

        public getdatatypes = () => {
            return app.common.$http({
                method: 'GET',
                url: app.fn.appUrl(this.serviceUri + '/getdatatypes')
            });
        }

        public list = (q: string, page: number, pageSize: number) => {
            return app.common.$http({
                method: 'GET',
                url: app.fn.appUrl(this.serviceUri + '/list'),
                params: { q: q, page: page, pageSize: pageSize }
            });
        }

        public getSearchDefinitionsUrl = () => {
            return app.fn.appUrl(this.serviceUri + "/select");
        }

    }
}

