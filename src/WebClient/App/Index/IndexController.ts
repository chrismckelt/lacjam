/// <reference path="../_references.ts" />
module app.controllers {

    export class IndexController extends app.base.ControllerBase {
        public static $inject = ["$scope"];

        constructor($scope: any) {
            super();
            var pageSize = 20;

            $scope.searchSelect2 = <Select2Options>{
                ajax: {
                    url: "/api/search/entity",
                    data: (term, page) => {
                        return { q: term, pageSize: pageSize, page: page };
                    },
                    results: (data, page) => {
                        return { results: data.hits, more: data.totalHits > pageSize * page };
                    }
                },
                multiple: true
            };
        }
    }
}