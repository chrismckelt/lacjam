/// <reference path="../_references.ts" />
module app.controllers {

    export class MetadataDefinitionGroupController extends app.base.ControllerBase {

        public static $inject = ["$scope", "$stateParams"];
        public service = new app.services.MetadataDefinitionGroupService();
        public groups : Array<app.model.MetadataDefinitionGroupResource>;

        constructor(public $scope: any, public $stateParams) {
            super();

            this.service.getAll()
                .then((x:any) =>
                    this.groups = x.data.result
                );
        }

         public edit = (id) => {
            app.redirectToUrl(app.Routes.metadataDefinitionGroupsEdit.url + '/' + id);
        }

        public cancel = () => {
            app.redirectToRoute(app.Routes.home.url);
        }
    }
}

