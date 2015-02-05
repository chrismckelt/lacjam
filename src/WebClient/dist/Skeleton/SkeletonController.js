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
        var SkeletonController = (function (_super) {
            __extends(SkeletonController, _super);
            function SkeletonController($scope, $location) {
                _super.call(this);
                this.$scope = $scope;
                this.$location = $location;
                // Bindable properties and functions are placed on vm.
                this.activeOnly = true;
                //canEditCompany = authService.authentication.isContributor;
                this.companySearch = '';
                this.companies = [];
                this.filteredPeople = [];
                this.title = 'Company';
                this.type = 'company';
            }
            SkeletonController.$inject = ["$scope", "$location"];
            return SkeletonController;
        })(app.base.ModelBase);
        controllers.SkeletonController = SkeletonController;
    })(app.controllers || (app.controllers = {}));
    var controllers = app.controllers;
})(app || (app = {}));
//# sourceMappingURL=../src/SkeletonController.js.map
