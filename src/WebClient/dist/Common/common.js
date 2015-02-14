/// <reference path="../_references.ts" />
"use strict";
define(["require", "exports"], function (require, exports) {
    var Common = (function () {
        function Common($rootScope, $log, $timeout, $http, $q) {
            //  this.common.log.debug("common ctor");
            this.$rootScope = $rootScope;
            this.$log = $log;
            this.$timeout = $timeout;
            this.$http = $http;
            this.$q = $q;
            this.serviceUri = "common";
            this.$get = function ($rootScope, $log, $timeout, $http, $q) { return new Common($rootScope, $log, $timeout, $http, $q); };
            if (typeof $log === "undefined") {
            }
        }
        Common.prototype.activate = function () {
        };
        Common.prototype.broadcast = function (notice) {
            var args = [];
            for (var _i = 1; _i < arguments.length; _i++) {
                args[_i - 1] = arguments[_i];
            }
            return this.$rootScope.$broadcast(notice, args);
        };
        Common.$inject = ["$rootScope", "$log", "$timeout", "$http", "$q"];
        //public static $inject = ["$log", "$timeout", "$http", "$q", "$state"]
        Common.throttles = [];
        return Common;
    })();
    exports.Common = Common;
});
//# sourceMappingURL=../src/Common.js.map