/// <reference path="../_references.ts" />
module app.controllers {
    "use strict";
    export class DocumentController
        extends app.base.ModelBase
        implements IController {

        public static $inject = ["$scope", "$location"];
  
        constructor(
            public $scope: any,
            public $location: ng.ILocationService
            ) {

            super();

        }
    }
}