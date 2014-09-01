/// <reference path="../_references.ts" />
var app;
(function (app) {
    (function (services) {
        "use strict";

        var Common = (function () {
            function Common($rootScope, $log, $timeout, $http, $q) {
                this.$rootScope = $rootScope;
                this.$log = $log;
                this.$timeout = $timeout;
                this.$http = $http;
                this.$q = $q;
                this.serviceId = "common";
                this.$get = function ($rootScope, $log, $timeout, $http, $q) {
                    return new Common($rootScope, $log, $timeout, $http, $q);
                };
                app.log.debug("common ctor");

                if (typeof $log === "undefined") {
                    app.log.debug("common -- contructor args not being set -  $log is undefined");
                    app.log.debug("common ----------- needs injection");
                }
            }
            Common.prototype.activate = function () {
            };

            Common.prototype.broadcast = function (notice) {
                var args = [];
                for (var _i = 0; _i < (arguments.length - 1); _i++) {
                    args[_i] = arguments[_i + 1];
                }
                return this.$rootScope.$broadcast(notice, args);
            };
            Common.$inject = ["$rootScope", "$log", "$timeout", "$http", "$q"];

            Common.throttles = [];
            return Common;
        })();
        services.Common = Common;
    })(app.services || (app.services = {}));
    var services = app.services;
})(app || (app = {}));
//# sourceMappingURL=common.js.map
