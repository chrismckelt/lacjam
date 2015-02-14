/// <reference path="../_references.ts" />

export class Navigation {

    public static $inject = ["$scope", "$state"];

    public state: any;
    public location: string;
    public other: string;

    constructor(){
    }

    public isVisible(menuName: string) {

        //if (!lacjam.global.stateCurrent) return false;

        //   if (lacjam.global.stateCurrent.name === menuName) return true;

        return false;
    }
}



