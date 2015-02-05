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

        var EntityController = (function (_super) {
            __extends(EntityController, _super);
            function EntityController($scope, $debounce, entityService) {
                _super.call(this);
                this.$debounce = $debounce;
                this.entityService = entityService;

                var textField = { field: 'text', resizable: true, displayName: 'Name', enableCellEdit: false, cellTemplate: '<span ng-bind-html="COL_FIELD"></span>' };
                var groupField = { field: 'group', resizable: true, width: '30%', displayName: 'Definition Group', enableCellEdit: false, cellTemplate: '<span ng-bind-html="COL_FIELD"></span>' };
                var editField = { field: 'id', width: '100px', displayName: 'Edit', enableCellEdit: false, cellTemplate: '<span class="glyphicon glyphicon-edit" ng-click="edit(COL_FIELD)">-</span>' };

                var scope = {
                    q: "",
                    totalHits: 0,
                    entities: null,
                    gridOptions: {
                        data: 'entities',
                        enableCellSelection: false,
                        enableRowSelection: false,
                        enableHighlighting: false,
                        enableCellEditOnFocus: false,
                        columnDefs: [textField, groupField, editField],
                        rowHeight: rowHeight,
                        enablePaging: true,
                        showFooter: true,
                        totalServerItems: 'totalHits',
                        pagingOptions: {
                            pageSizes: [20, 50, 200],
                            pageSize: 20,
                            currentPage: 1
                        }
                    },
                    edit: function (id) {
                        app.redirectToUrl("entityedit" + '/' + id);
                    },
                    cancel: function () {
                        app.redirectToRoute(app.Routes.home.url);
                    }
                };
                scope = $.extend($scope, scope);

                app.fn.spinStart();

                var loadGrid = function () {
                    entityService.list(scope.q, scope.gridOptions.pagingOptions.currentPage, scope.gridOptions.pagingOptions.pageSize).then(function (x) {
                        scope.entities = x.data.hits;
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
            EntityController.$inject = ["$scope", "$debounce", "EntityService"];
            return EntityController;
        })(app.base.ControllerBase);
        controllers.EntityController = EntityController;
    })(app.controllers || (app.controllers = {}));
    var controllers = app.controllers;
})(app || (app = {}));
//# sourceMappingURL=../src/entityController.js.map
