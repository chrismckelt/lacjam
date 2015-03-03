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
        var FooterController = (function (_super) {
            __extends(FooterController, _super);
            function FooterController($scope, $state) {
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
            FooterController.$inject = ["$scope", "$state"];
            return FooterController;
        })(app.base.ControllerBase);
        controllers.FooterController = FooterController;
        angular.module("app.controllers").controller({ FooterController: FooterController });
    })(app.controllers || (app.controllers = {}));
    var controllers = app.controllers;
})(app || (app = {}));
//# sourceMappingURL=../src/FooterController.js.map
