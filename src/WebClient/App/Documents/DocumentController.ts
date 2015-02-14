/// <reference path="../../_references.ts" />
"use strict";
export class DocumentController
//extends LacjamModule.Base.ControllerBase
// implements LacjamModule.IController
{

    public static $inject = ["$scope", "$location"];

    constructor(
        public $scope: any,
        public $location: any
        ) {

        //  super();

    }
}