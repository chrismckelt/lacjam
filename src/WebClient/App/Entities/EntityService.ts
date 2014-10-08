/// <reference path="../_references.ts" />
module app.services {

    export class EntityService extends app.base.ServiceBase<app.model.EntityResource> {
     
        constructor() {
            super("Entity", "Entity");
        }

        public list = (q: string, page: number, pageSize: number) => {
            return app.common.$http({
                method: 'GET',
                url: app.fn.appUrl(this.serviceUri + '/list'),
                params: { q:q, page:page, pageSize: pageSize }
            });
        }
    }
}

