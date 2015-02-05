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
        var MetadataDefinitionGroupController = (function (_super) {
            __extends(MetadataDefinitionGroupController, _super);
            function MetadataDefinitionGroupController($scope, $stateParams) {
                var _this = this;
                _super.call(this);
                this.$scope = $scope;
                this.$stateParams = $stateParams;
                this.service = new app.services.MetadataDefinitionGroupService();
                this.edit = function (id) {
                    app.redirectToUrl(app.Routes.documents.url + '/' + id);
                };
                this.cancel = function () {
                    app.redirectToRoute(app.Routes.home.url);
                };

                this.service.getAll().then(function (x) {
                    return _this.groups = x.data.result;
                });
            }
            MetadataDefinitionGroupController.$inject = ["$scope", "$stateParams"];
            return MetadataDefinitionGroupController;
        })(app.base.ControllerBase);
        controllers.MetadataDefinitionGroupController = MetadataDefinitionGroupController;
    })(app.controllers || (app.controllers = {}));
    var controllers = app.controllers;
})(app || (app = {}));
//# sourceMappingURL=../src/MetadataDefinitionGroupController.js.map
