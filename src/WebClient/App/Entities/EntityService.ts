/// <reference path="../_references.ts" />
module app.services {

    export class EntityService extends app.base.ServiceBase<app.model.EntityResource> {
        public static $inject = ["$http"];
        private $http;

        constructor(public http) {
            super("Entity", "Entity");
            this.$http = http;
        }

        public list = (q: string, page: number, pageSize: number) => {
            return this.$http({
                method: 'GET',
                url: app.fn.appUrl(this.serviceUri + '/list'),
                params: { q:q, page:page, pageSize: pageSize }
            });
        }

        public publicSearch = (q: string, page: number, pageSize: number) => {
            return this.$http({
                method: 'GET',
                url: app.fn.appUrl('search/entities'),
                params: { q: q, page: page, pageSize: pageSize }
            });
        }

        public defaultSelections = () => {
            return this.$http({
                method: 'GET',
                url: app.fn.appUrl('search/entities/defaults')
            });
        }
    }

    angular.module("app.services").service({ EntityService: EntityService });
    angular.module("publicApp.services").service({ EntityService: EntityService });
}

