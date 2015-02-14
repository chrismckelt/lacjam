/// <reference path="../../_references.ts" />
"use strict";
export interface ICommonService {
    $get;
    $log: any;
    $timeout: any;
    $http: any;
    $q: any;
    broadcast(notice: any, args: any);
}

export class Common implements ICommonService {
    public serviceUri = "common";
    public static $inject: string[] = ["$rootScope", "$log", "$timeout", "$http", "$q"];
    //public static $inject = ["$log", "$timeout", "$http", "$q", "$state"]
    private static throttles = [];

    public $get = (
        $rootScope: any,
        $log: any,
        $timeout: any,
        $http: any,
        $q: any
        ) => new Common($rootScope, $log, $timeout, $http, $q);

    constructor(
        public $rootScope: any,
        public $log: any,
        public $timeout: any,
        public $http: any,
        public $q: any

        ) {

        //  this.common.log.debug("common ctor");

        if (typeof $log === "undefined") {
            //lacjam.log.debug("common -- contructor args not being set -  $log is undefined");
            //lacjam.log.debug("common ----------- needs injection");
        }


    }


    public activate() {

    }

    public broadcast(notice: string, ...args: any[]) {
        return this.$rootScope.$broadcast(notice, args);
    }
}