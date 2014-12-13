/// <reference path="../_references.ts" />
module app.controllers {

    export class MetadataDefinitionGroupEditController extends app.base.ControllerBase {

        public static $inject = ["$scope", "$stateParams", "$modal", "dialogs", "$templateCache"];
        public metadataDefinitionGroupService = new app.services.MetadataDefinitionGroupService();
        public metadataDefinitionService = new app.services.MetadataDefinitionService();
        public editMode = false;
        public originalName: string;
        public model = new app.model.MetadataDefinitionGroupResource()
        
        public definitionsSelect2: Select2Options;

        constructor(
            public $scope: any,
            public $stateParams,
            public $modal,
            public $dialogs,
            public $templateCache
            ) {
            super();

            if ($stateParams.identity) {
                this.metadataDefinitionGroupService.get($stateParams.identity)
                    .then((res:any)
                        => {
                            this.title = 'Editing ' + res.data.name;
                            this.model = res.data;
                            this.originalName = res.data.name;
                            this.editMode = true;
                    });
            } else {
                this.title = 'Create new group ';
                this.model.identity = app.fn.createGuid();
                this.model.name = ''; //"test-" + app.fn.createGuid();
                this.model.description = ''; //"description-" + app.fn.createGuid();
                this.model.definitions = [];
            }

            this.definitionsSelect2 = {
                ajax: {
                    url: this.metadataDefinitionService.getSearchDefinitionsUrl(),
                    data: (term, page) => {
                        return { q: term, pageSize: 10, page: page };
                    },
                    results: (data, page) => {
                        return { results: data, more: data.length >= 10 };
                    }
                },
                initSelection: (data, callback) => {
                    callback(this.model.definitions);
                },
                multiple: true

            };
        }

         public save = () => {
             if (this.$stateParams.identity) {
                 this.update();
             } else {
                 this.create();
             }
         }

        public create = () => {
            app.fn.spinStart();
            this.model.identity = app.fn.createGuid();
            this.model.name = this.model.name;
            this.model.description = this.model.description;
            this.metadataDefinitionGroupService.create(this.model)
                .then(pro => {
                    toastr.success(this.model.name, "Created");
                    app.fn.spinStop();
                    app.redirectToRoute(app.Routes.documents);
                },
                (err => {
                    app.log.error(err);
                    var msg = '';
                    angular.forEach(err.data.modelState, resource => {
                        msg += resource + '\n';
                        }
                        );
                    if (msg.length>0) {
                        toastr.error(msg, "Error");
                    } else {
                        toastr.error(err.data.message, "Error");
                    }
                    app.fn.spinStop();
                })
                );
        }


        public update = () => {
            app.fn.spinStart();
         
            this.metadataDefinitionGroupService.update(this.model, this.$stateParams.identity)
                .then(pro => {
                    toastr.success(this.model.name, "Saved");
                    app.fn.spinStop();
                    app.redirectToRoute(app.Routes.documents);
                },
                (err => {
                    app.log.error(err);
                    var msg = '';
                    angular.forEach(err.data.modelState, resource => {
                        msg += resource + '\n';
                    }
                        );
                    if (msg.length) {
                        toastr.error(msg, "Error");
                    } else {
                        toastr.error(err.data.message, "Error");
                    }
                    app.fn.spinStop();
                })
                );
        }


         public doDelete()  {
             //var dialog = app.resolveService<app.services.Dialog>(app.services.Dialog.prototype);
             var msg = 'Are you sure you wish to delete this item?';
             //var confirm = window.confirm(msg);
             ////var confirm = dialog.showModal('Yes', 'No','Are you sure you wish to delete this item?');
             //if (!confirm) return;

             var opts:any = {};
             opts.kb = true;
             opts.bd = true;
             opts.ws = 'sm';// values: 'sm', 'lg', 'md'

             var dlg = this.$dialogs.confirm('Confirm', msg,opts);
             dlg.result.then(btn => {
                 app.fn.spinStart();
                 this.metadataDefinitionGroupService.doDelete(this.model.identity)
                     .then(pro => {
                         toastr.success(this.originalName, "Deleted");
                         app.fn.spinStop();
                         app.redirectToRoute(app.Routes.documents);
                         //  e.preventDefault();
                     },
                     (err => {
                         app.log.error(err);
                         var msg = '';
                         angular.forEach(err.data.modelState, resource => {
                             msg += resource + '\n';
                         }
                             );
                         toastr.error(msg, "Error");
                         app.fn.spinStop();
                     })
                     );
             }, btn => {
                 app.log.info("Delete metadata definition group cancelled");
             });
         }

        public cancel = (e) => {
            app.redirectToRoute(app.Routes.documents);
            e.preventDefault();
        }

    }
}

