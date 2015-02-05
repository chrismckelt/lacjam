/// <reference path="../_references.ts" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var publicApp;
(function (publicApp) {
    (function (controllers) {
        var SearchController = (function (_super) {
            __extends(SearchController, _super);
            function SearchController($scope, entityService, $debounce) {
                _super.call(this);
                this.$scope = $scope;
                this.entityService = entityService;
                this.$debounce = $debounce;

                var scope = {
                    q: "",
                    searchResults: [],
                    selectedResults: [],
                    select: function (result) {
                        if (!scope.isSelected(result)) {
                            scope.selectedResults.push(result);
                        }
                    },
                    deselect: function (result) {
                        var index = scope.selectedResults.indexOf(result);
                        scope.selectedResults.splice(index, 1);
                    },
                    isSelected: function (result) {
                        for (var i in scope.selectedResults) {
                            if (scope.selectedResults[i].id == result.id) {
                                return true;
                            }
                        }
                        return false;
                    },
                    isNotSelected: function (result) {
                        return !scope.isSelected(result);
                    }
                };

                scope = $.extend($scope, scope);

                $scope.$watch("q", $debounce(300, function () {
                    entityService.publicSearch(scope.q, 1, 50).then(function (res) {
                        scope.searchResults = res.data.hits;
                    });
                }));

                entityService.defaultSelections().then(function (res) {
                    for (var i in res.data) {
                        scope.selectedResults.push(res.data[i]);
                    }
                });
            }
            SearchController.$inject = ["$scope", "EntityService", "$debounce"];
            return SearchController;
        })(app.base.ControllerBase);
        controllers.SearchController = SearchController;

        angular.module("publicApp.controllers").controller({ SearchController: SearchController });
    })(publicApp.controllers || (publicApp.controllers = {}));
    var controllers = publicApp.controllers;
})(publicApp || (publicApp = {}));
//# sourceMappingURL=../src/searchController.js.map
