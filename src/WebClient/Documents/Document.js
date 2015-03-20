/// <reference path="../../_references.ts" />
"use strict";
define(["require", "exports"], function(require, exports) {
    var Document = (function () {
        function Document($scope, $location) {
            this.$scope = $scope;
            this.$location = $location;
            //  super();
        }
        Document.$inject = ["$scope", "$location"];
        return Document;
    })();
    exports.Document = Document;
});
