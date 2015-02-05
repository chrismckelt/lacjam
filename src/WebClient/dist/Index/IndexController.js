var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var app;
(function (app) {
    /// <reference path="../_references.ts" />
    (function (controllers) {
        var IndexController = (function (_super) {
            __extends(IndexController, _super);
            function IndexController($scope) {
                _super.call(this);
            }
            IndexController.$inject = ["$scope"];
            return IndexController;
        })(app.base.ControllerBase);
        controllers.IndexController = IndexController;
    })(app.controllers || (app.controllers = {}));
    var controllers = app.controllers;
})(app || (app = {}));
//# sourceMappingURL=../src/IndexController.js.map
