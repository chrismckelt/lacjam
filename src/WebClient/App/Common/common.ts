/// <reference path="../_references.ts" />

module app.services {
    "use strict";


    export interface ICommonService extends IService {
        $get;
        $log: ng.ILogService;
        $timeout: ng.ITimeoutService;
        $http: ng.IHttpService;
        $q: ng.IQService;
        broadcast(notice: any, args: any);
    }


    export class Common implements ICommonService {
        public serviceId = "common";
        public static $inject: string[] = ["$rootScope", "$log", "$timeout", "$http", "$q"];
        //public static $inject = ["$log", "$timeout", "$http", "$q", "$state"]
        private static throttles = [];

        public $get = (
            $rootScope: ng.IRootScopeService,
            $log: ng.ILogService,
            $timeout: ng.ITimeoutService,
            $http: ng.IHttpService,
            $q: ng.IQService
            ) => new Common($rootScope, $log, $timeout, $http, $q);

        constructor(
            public $rootScope: ng.IRootScopeService,
            public $log: ng.ILogService,
            public $timeout: ng.ITimeoutService,
            public $http: ng.IHttpService,
            public $q: ng.IQService

            ) {

            app.log.debug("common ctor");

            if (typeof $log === "undefined") {
                app.log.debug("common -- contructor args not being set -  $log is undefined");
                app.log.debug("common ----------- needs injection");
            }


        }


        public activate() {

        }

        public broadcast(notice: string, ...args: any[]) {
            return this.$rootScope.$broadcast(notice, args);
        }

    }
}