/// <reference path="../_references.ts" />
module app.controllers {

    export class DashboardController extends app.base.ControllerBase {
        public static $inject = ["$scope"];

        constructor($scope: any) {
            super();
        }
    }
}