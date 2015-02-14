/// <reference path="../_references.ts" />
define(["require", "exports"], function (require, exports) {
    var Navigation = (function () {
        function Navigation() {
        }
        Navigation.prototype.isVisible = function (menuName) {
            //if (!lacjam.global.stateCurrent) return false;
            //   if (lacjam.global.stateCurrent.name === menuName) return true;
            return false;
        };
        Navigation.$inject = ["$scope", "$state"];
        return Navigation;
    })();
    exports.Navigation = Navigation;
});
//# sourceMappingURL=navigation.js.map