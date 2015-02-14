/// <reference path="../_references.ts" />
"use strict";
define(["require", "exports"], function (require, exports) {
    var DocumentController = (function () {
        function DocumentController($scope, $location) {
            //  super();
            this.$scope = $scope;
            this.$location = $location;
        }
        DocumentController.$inject = ["$scope", "$location"];
        return DocumentController;
    })();
    exports.DocumentController = DocumentController;
});
//# sourceMappingURL=../src/documentcontroller.js.map