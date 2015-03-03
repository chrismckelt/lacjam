/// <reference path="../../_references.ts" />
"use strict";
define(["require", "exports"], function(require, exports) {
    var Common = (function () {
        function Common($rootScope, $log, $timeout, $http, $q) {
            this.$rootScope = $rootScope;
            this.$log = $log;
            this.$timeout = $timeout;
            this.$http = $http;
            this.$q = $q;
            this.serviceUri = "common";
            this.$get = function ($rootScope, $log, $timeout, $http, $q) {
                return new Common($rootScope, $log, $timeout, $http, $q);
            };
            //  this.common.log.debug("common ctor");
            if (typeof $log === "undefined") {
                //lacjam.log.debug("common -- contructor args not being set -  $log is undefined");
                //lacjam.log.debug("common ----------- needs injection");
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
    exports.Common = Common;
});
