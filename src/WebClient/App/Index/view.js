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
        var IndexModel = (function () {
            function IndexModel() {
            }
            return IndexModel;
        })();
        controllers.IndexModel = IndexModel;

        var Index = (function (_super) {
            __extends(Index, _super);
            function Index($injector, $scope) {
                _super.call(this, "Index", $scope);
                this.scope = $scope;
                this.scope.vm = new IndexModel();
            }
            return Index;
        })(controllers.ControllerBase);
        controllers.Index = Index;
    })(app.controllers || (app.controllers = {}));
    var controllers = app.controllers;
})(app || (app = {}));

app.registerController("app.controllers.Index", []);
//# sourceMappingURL=view.js.map
