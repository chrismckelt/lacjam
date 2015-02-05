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
        "use strict";
        var DocumentController = (function (_super) {
            __extends(DocumentController, _super);
            function DocumentController($scope, $location) {
                _super.call(this);
                this.$scope = $scope;
                this.$location = $location;
            }
            DocumentController.$inject = ["$scope", "$location"];
            return DocumentController;
        })(app.base.ModelBase);
        controllers.DocumentController = DocumentController;
    })(app.controllers || (app.controllers = {}));
    var controllers = app.controllers;
})(app || (app = {}));
//# sourceMappingURL=../src/DocumentController.js.map
