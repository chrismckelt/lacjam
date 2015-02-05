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
        var rowHeight = 50;

        var MetadataDefinitionController = (function (_super) {
            __extends(MetadataDefinitionController, _super);
            function MetadataDefinitionController($scope, $debounce) {
                var _this = this;
                _super.call(this);
                this.$scope = $scope;
                this.$debounce = $debounce;
                this.service = new app.services.MetadataDefinitionService();
                this.model = new app.model.MetadataDefinitionResource();

                var nameField = { field: 'name', resizable: true, width: '20%', displayName: 'Name', enableCellEdit: false, cellTemplate: '<span ng-bind-html="COL_FIELD"></span>' };
                var descriptionField = { field: 'description', resizable: true, displayName: 'Description', enableCellEdit: false, cellTemplate: '<span ng-bind-html="COL_FIELD"></span>' };
                var dataTypeField = { field: 'dataType', resizable: true, width: '8%', displayName: 'Type', enableCellEdit: false, cellTemplate: '<span ng-bind-html="COL_FIELD"></span>' };
                var editField = { field: 'id', width: '50px', displayName: 'Edit', enableCellEdit: false, cellTemplate: '<span class="glyphicon glyphicon-edit" ng-click="edit(COL_FIELD)">-</span>' };

                var scope = {
                    q: "",
                    totalHits: 0,
                    definitions: null,
                    edit: function (id) {
                        app.redirectToUrl(app.Routes.metadataDefinitionEdit.url + '/' + id);
                    },
                    cancel: function () {
                        app.redirectToRoute(app.Routes.home.url);
                    },
                    gridOptions: {
                        data: 'definitions',
                        enableCellSelection: false,
                        enableRowSelection: false,
                        enableHighlighting: false,
                        enableCellEditOnFocus: false,
                        columnDefs: [nameField, dataTypeField, descriptionField, editField],
                        rowHeight: rowHeight,
                        enablePaging: true,
                        showFooter: true,
                        totalServerItems: 'totalHits',
                        pagingOptions: {
                            pageSizes: [20, 50, 200],
                            pageSize: 20,
                            currentPage: 1
                        }
                    }
                };

                scope = $.extend($scope, scope);

                app.fn.spinStart();

                var loadGrid = function () {
                    _this.service.list(scope.q, scope.gridOptions.pagingOptions.currentPage, scope.gridOptions.pagingOptions.pageSize).then(function (x) {
                        scope.definitions = x.data.hits;
                        scope.totalHits = x.data.totalHits;
                        app.fn.spinStop();
                    });
                };

                $scope.$watch("q", $debounce(300, function (newVal, oldVal) {
                    if (newVal !== oldVal)
                        scope.gridOptions.pagingOptions.currentPage = 1;
                    loadGrid();
                }), true);

                $scope.$watch("gridOptions.pagingOptions", function (newVal, oldVal) {
                    if (newVal !== oldVal)
                        loadGrid();
                }, true);

                loadGrid();
            }
            MetadataDefinitionController.$inject = ["$scope", "$debounce"];
            return MetadataDefinitionController;
        })(app.base.ControllerBase);
        controllers.MetadataDefinitionController = MetadataDefinitionController;
    })(app.controllers || (app.controllers = {}));
    var controllers = app.controllers;
})(app || (app = {}));
//# sourceMappingURL=../src/metadataDefinitionController.js.map
