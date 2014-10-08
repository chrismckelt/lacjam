/// <reference path="../_references.ts" />
module app.controllers {
    var rowHeight = 50;
    
    export class MetadataDefinitionController extends app.base.ControllerBase {
        public static $inject = ["$scope", "$debounce"];
        public service = new app.services.MetadataDefinitionService();
        public model = new app.model.MetadataDefinitionResource();
       
        constructor(public $scope: ng.IScope, public $debounce) {
            super();

            var nameField = { field: 'name', resizable: true, width: '20%', displayName: 'Name', enableCellEdit: false };
            var descriptionField = { field: 'description', resizable: true, displayName: 'Description', enableCellEdit: false };
            var dataTypeField = { field: 'dataType', resizable: true, width: '8%', displayName: 'Type', enableCellEdit: false };
            var editField = { field: 'id', width: '50px', displayName: 'Edit', enableCellEdit: false, cellTemplate: '<span class="glyphicon glyphicon-edit" ng-click="edit(COL_FIELD)">-</span>' };
        
            var scope = {
                q: "",
                totalHits: 0,
                definitions: <app.model.MetadataDefinitionResource[]>null,
                edit: (id) => {
                    app.redirectToUrl(app.Routes.metadataDefinitionEdit.url + '/' + id);
                },
                cancel: () => {
                    app.redirectToRoute(app.Routes.home.url);
                },
                gridOptions: <ngGrid.IGridOptions>{
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
                    // rowTemplate: '<div ng-repeat="col in renderedColumns" ng-class="col.colIndex()" class="ngCell { { col.cellClass } }"><div class="ngVerticalBar" ng-style=" { height: rowHeight }" ng-class=" { ngVerticalBarVisible: !$last }">&nbsp;</div><div ng-cell></div></div>'
                }
            };

            scope = $.extend($scope, scope);

            app.fn.spinStart();

            var loadGrid = () => {
                this.service.list(scope.q, scope.gridOptions.pagingOptions.currentPage, scope.gridOptions.pagingOptions.pageSize)
                    .then((x: any) => {
                        scope.definitions = x.data.hits;
                        scope.totalHits = x.data.totalHits;
                        app.fn.spinStop();
                    });
            }

            $scope.$watch("q", $debounce(300, (newVal, oldVal) => {
                if (newVal !== oldVal)
                    scope.gridOptions.pagingOptions.currentPage = 1;
                loadGrid();
            }), true);

            $scope.$watch("gridOptions.pagingOptions", (newVal, oldVal) => {
                if (newVal !== oldVal)
                    loadGrid();
            }, true);

            loadGrid();
        }
    }
}