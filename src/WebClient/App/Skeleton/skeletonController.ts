/// <reference path="../_references.ts" />
module app.controllers {
    "use strict";
    export class SkeletonController
        extends app.base.ModelBase
        implements IController {

        public static $inject = ["$scope", "$location"];
        // Bindable properties and functions are placed on vm.
        activeOnly = true;
        //canEditCompany = authService.authentication.isContributor;
        companySearch = '';
        companies = [];
        filteredPeople = [];
        title = 'Company';
        type = 'company';
       

        constructor(
            public $scope: any,
            public $location : ng.ILocationService
        ) {

            super();

        }
    }
}