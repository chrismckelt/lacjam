/// <reference path="../_references.ts" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var app;
(function (app) {
    (function (controllers) {
        var NavigationController = (function (_super) {
            __extends(NavigationController, _super);
            function NavigationController($scope, $state) {
                var _this = this;
                _super.call(this);
                this.$scope = $scope;
                this.$state = $state;
                this.state = app.Routes.getRoutes()[0];
                this.$scope.$watch(function () {
                    return app.global.stateCurrent;
                }, function (oldValue, newValue) {
                    if (oldValue !== newValue) {
                        _this.state = app.global.stateCurrent;
                    }
                });
            }
            NavigationController.prototype.isVisible = function (menuName) {
                if (!app.global.stateCurrent)
                    return false;

                if (app.global.stateCurrent.name === menuName)
                    return true;

                return false;
            };
            NavigationController.$inject = ["$scope", "$state"];
            return NavigationController;
        })(app.base.ControllerBase);
        controllers.NavigationController = NavigationController;
        angular.module("app.controllers").controller({ NavigationController: NavigationController });
    })(app.controllers || (app.controllers = {}));
    var controllers = app.controllers;
})(app || (app = {}));
//# sourceMappingURL=../src/NavigationController.js.map
