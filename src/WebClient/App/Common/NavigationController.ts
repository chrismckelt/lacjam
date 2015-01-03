/// <reference path="../_references.ts" />

module app.controllers {

    export class NavigationController extends app.base.ControllerBase {

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

        public isVisible(menuName:string) {
            if (app.global.stateCurrent.name === menuName) return true;

            return false;
        }
    }
    angular.module("app.controllers").controller({ NavigationController: NavigationController });
}



