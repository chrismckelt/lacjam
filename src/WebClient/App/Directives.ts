/// <reference path="_references.ts" />


module app.directives {
    "use strict";

    /////http://stackoverflow.com/questions/17944419/angularjs-typescript-directive


    // Description:
    //  Creates a new Spinner and sets its options
    // Usage:
    //  <div data-cc-spinner="vm.spinnerOptions"></div>
    export class ccSpinner implements app.IDirective {

        public restrict: string = "A";

        public static $inject = ["$window"];


        constructor(
            public $window: ng.IWindowService) {
        }

        public link(scope, element: JQuery, attrs: any) {
            scope.spinner = null;
            scope.key = angular.isDefined(attrs.spinnerKey) ? attrs.spinnerKey : false;
            // scope.startActive = angular.isDefined(attrs.spinnerStartActive) ? attrs.spinnerStartActive : !(scope.key);
            app.log.debug("----------------------------------------");
            scope.$watch(attrs.ccSpinner, options => {
                if (app.global.spinner) {
                    app.global.spinner.stop();
                }
                app.global.spinner = new Spinner(options);

            }, true);

            scope.$on('cc-spinner:spin', (event, key) => {
                if (key[0] === scope.key) {
                    app.global.spinner.spin(angular.element("#" + key)[0]);
                }
                //else {
                //    app.global.spinner.spin(angular.element("#spinner")[0]);
                //}
            });

            scope.$on('cc-spinner:stop', (event, key) => {
                app.global.spinner.stop();
            });
            scope.$on('$destroy', () => {
                app.global.spinner.stop();
                app.global.spinner = null;
            });
        }
    }

}

//directives
app.log.debug("Registering directives");
app.registerDirective("ccSpinner", []);


angular.module('app').directive('autoFocus', function ($timeout) {
    return {
        restrict: 'AC',
        link: function (_scope, _element) {
            $timeout(function () {
                _element[0].focus();
            }, 0);
        }
    };
});
