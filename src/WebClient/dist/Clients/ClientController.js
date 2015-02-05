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
        var ClientController = (function (_super) {
            __extends(ClientController, _super);
            function ClientController($scope) {
                _super.call(this);
                this.startTime = moment("12/12/2015 6:00 AM");
                this.end = moment("12/12/2015 11:00 PM");
                this.slots = new Array();
                this.init();
            }
            ClientController.prototype.init = function () {
                var duration = moment.duration(this.end.diff(this.startTime));
                var hours = duration.asHours();

                var current = this.startTime;

                while (current < this.end) {
                    this.slots.push(current);
                    current = current.add(30, 'minutes');
                }
            };
            ClientController.$inject = ["$scope"];
            return ClientController;
        })(app.base.ControllerBase);
        controllers.ClientController = ClientController;
    })(app.controllers || (app.controllers = {}));
    var controllers = app.controllers;
})(app || (app = {}));
//# sourceMappingURL=../src/ClientController.js.map
