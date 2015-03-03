/// <reference path="../../_references.ts" />
"use strict";
define(["require", "exports"], function(require, exports) {
    var DocumentController = (function () {
        function DocumentController($scope, $location) {
            this.$scope = $scope;
            this.$location = $location;
            //  super();
        }
        DocumentController.$inject = ["$scope", "$location"];
        return DocumentController;
    })();
    exports.DocumentController = DocumentController;
});
