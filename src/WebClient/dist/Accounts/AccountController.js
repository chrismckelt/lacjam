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
        var AccountController = (function (_super) {
            __extends(AccountController, _super);
            function AccountController($scope) {
                _super.call(this);
            }
            AccountController.$inject = ["$scope"];
            return AccountController;
        })(app.base.ControllerBase);
        controllers.AccountController = AccountController;
    })(app.controllers || (app.controllers = {}));
    var controllers = app.controllers;
})(app || (app = {}));
//# sourceMappingURL=../src/AccountController.js.map
