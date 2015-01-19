/// <reference path="../_references.ts" />
module app.controllers {

    export class ClientController extends app.base.ControllerBase {
        public static $inject = ["$scope"];
        public startTime = moment("19/01/2015 6:00 AM")
        public end = moment("19/01/2015 11:00 PM")
        public slots = new Array<any>();

        constructor($scope: any) {
            super();
            var duration = moment.duration(this.end.diff(this.startTime));
            var hours = duration.asHours();
       
            var current = this.startTime;
     
            while (current < this.end) {
                this.slots.push(current);
                current = current.add(30, 'minutes');
            }

        }
    }
}