/// <reference path="../_references.d.ts" />
declare module app.controllers {
    class SkeletonController extends base.ModelBase implements IController {
        public $scope: any;
        public $location: ng.ILocationService;
        static $inject: string[];
        public activeOnly: boolean;
        public companySearch: string;
        public companies: any[];
        public filteredPeople: any[];
        public title: string;
        public type: string;
        constructor($scope: any, $location: ng.ILocationService);
    }
}
