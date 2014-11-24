/// <reference path="../_references.ts" />

module publicApp.controllers {

    export class SearchController extends app.base.ControllerBase {

        public static $inject = ["$scope", "EntityService", "$debounce"];
        
        constructor(public $scope: ng.IScope, public entityService: app.services.EntityService, public $debounce) {
            super();

            var scope = {
                q: "",
                searchResults: [],
                selectedResults: [],
                select: (result) => {
                    if (!scope.isSelected(result)) {
                        scope.selectedResults.push(result);
                    }
                },
                deselect: (result) => {
                    var index = scope.selectedResults.indexOf(result);
                    scope.selectedResults.splice(index, 1);     
                },
                isSelected: (result) => {
                    for (var i in scope.selectedResults) {
                        if (scope.selectedResults[i].id == result.id) {
                            return true;
                        }
                    }
                    return false;
                },
                isNotSelected: (result) => !scope.isSelected(result)
            };

            scope = $.extend($scope, scope);

            $scope.$watch("q", $debounce(300, () => {
                entityService.publicSearch(scope.q, 1, 50).then(res => {
                    scope.searchResults = res.data.hits;
                });
            }));

            entityService.defaultSelections().then(res => {
                for (var i in res.data) {
                    scope.selectedResults.push(res.data[i]);
                }
            });
        }
    }

    angular.module("publicApp.controllers").controller({ SearchController: SearchController });
}

