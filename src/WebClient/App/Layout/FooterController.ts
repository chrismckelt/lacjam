/// <reference path="../_references.ts" />

module app.controllers {

    export class FooterController extends app.base.ControllerBase {

        public static $inject = ["$scope", "$state"];

        public state: ng.ui.IState;
        public location: string;
        public other: string;

        constructor(
            public $scope: ng.IScope,
            public $state: ng.ui.IStateService) {
            super();
            this.state = app.Routes.getRoutes()[0];
            this.$scope.$watch(() => app.global.stateCurrent,
                (oldValue: string, newValue: string) => {
                    if (oldValue !== newValue) {
                        this.state = app.global.stateCurrent;
                    }
                });
        }
    }
    angular.module("app.controllers").controller({ FooterController: FooterController });
}



