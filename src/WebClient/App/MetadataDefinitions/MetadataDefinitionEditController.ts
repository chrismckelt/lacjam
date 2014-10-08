/// <reference path="../_references.ts" />
module app.controllers {

    export class MetadataDefinitionEditController extends app.base.ControllerBase {

        public static $inject = ["$scope", "$stateParams", "$modal", "dialogs", "$templateCache"];
        public metadataDefinitionService = new app.services.MetadataDefinitionService();
        public editMode = false;
        public originalName: string;
        public model = new app.model.MetadataDefinitionResource()
        public datatypes;
        public selectedDataType;
        public isSingleValue = true;
        public valueDisplay = 'Value';

        constructor(
            public $scope: any,
            public $stateParams,
            public $modal,
            public $dialogs,
            public $templateCache
        ) {
            super();

            this.metadataDefinitionService.getdatatypes()
                .then(x => {
                    this.datatypes = x.data;
                    this.bindData();
                });

        }

        public bindData() {
            if (this.$stateParams.identity) {
                this.metadataDefinitionService.get(this.$stateParams.identity)
                    .then((res: any)
                        => {
                            this.title = 'Editing ' + res.data.name;
                            this.model = res.data;
                            this.originalName = res.data.name;
                            this.editMode = true;

                            app.fn.safeApply(() => {
                                    for (var x = 0; x < this.datatypes.length; x++) {
                                        if (this.datatypes[x].dataType == res.data.dataType) {
                                            $('#datatypeSelect').val(x.toLocaleString());
                                            this.selectedDataType = this.datatypes[x];
                                            //this.dataTypeSelected();
                                            this.setIsSingleValue();
                                        }
                                    }
                                }
                                );

                });
            } else {
                this.title = 'Create new group ';
                this.model.identity = app.fn.createGuid();
                this.model.name = ''; //"test-" + app.fn.createGuid();
                this.model.description = '';
                this.model.values = [];
                this.model.values.push('');
            }
        }

        public save = () => {
            if (!this.selectedDataType || this.selectedDataType === '') {
                toastr.warning('Type is required');
                return;
            }

            if (this.$stateParams.identity) {
                this.update();
            } else {
                this.create();
            }
        }

        public create = () => {
            app.fn.spinStart();
            this.model.identity = app.fn.createGuid();
            this.metadataDefinitionService.create(this.model)
                .then(pro => {
                        toastr.success(this.model.name, "Created");
                        app.fn.spinStop();
                        app.redirectToRoute(app.Routes.metadataDefinitions);
                    },
                    (err => {
                        app.log.error(err);
                        var msg = '';
                        angular.forEach(err.data.modelState, resource => {
                                msg += resource + '\n';
                            }
                        );
                        if (msg.length > 0) {
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

            this.metadataDefinitionService.update(this.model, this.$stateParams.identity)
                .then(pro => {
                        toastr.success(this.model.name, "Saved");
                        app.fn.spinStop();
                        app.redirectToRoute(app.Routes.metadataDefinitions);
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


        public doDelete() {
            //var dialog = app.resolveService<app.services.Dialog>(app.services.Dialog.prototype);
            var msg = 'Are you sure you wish to delete this item?';
            //var confirm = window.confirm(msg);
            ////var confirm = dialog.showModal('Yes', 'No','Are you sure you wish to delete this item?');
            //if (!confirm) return;

            var opts: any = {};
            opts.kb = true;
            opts.bd = true;
            opts.ws = 'sm'; // values: 'sm', 'lg', 'md'

            var dlg = this.$dialogs.confirm('Confirm', msg, opts);
            dlg.result.then(btn => {
                app.fn.spinStart();
                this.metadataDefinitionService.doDelete(this.model.identity)
                    .then(pro => {
                            toastr.success(this.originalName, "Deleted");
                            app.fn.spinStop();
                            app.redirectToRoute(app.Routes.metadataDefinitions);
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
            app.redirectToRoute(app.Routes.metadataDefinitions);
            e.preventDefault();
        }


        public dataTypeSelected = () => {
            this.model.dataType = this.selectedDataType.dataType;
            this.model.regex = '';
            
            if (this.selectedDataType.dataType === 'YesNo') {
               
                this.model.values = null;
            }

            if (this.selectedDataType.dataType === 'ComboBox' || this.selectedDataType.dataType === 'PickList') {
                this.model.values = [];
                this.addLine();
             
            } else {
                this.model.values = [];
                this.addLine();
            }

            this.setIsSingleValue();
        }

        public addLine = () => {
            if (this.model.values)this.model.values.push(' ');
        }

        public removeItem(i: number) {
            delete this.model.values[i];
            this.model.values = _.filter(this.model.values, x => { return x != undefined && x !==''; });
        }

        public show(i: number) {
            if (this.model.values[i] || this.model.values[i] ==='' ) {
                return true;
            }

            return false;
        }

        public getmask = () => {
            if (this.model.regex) return this.model.regex;

            return "(**: AAA-999)";
        }

        private setIsSingleValue() {

            if (this.selectedDataType.dataType === 'YesNo') {
                this.valueDisplay = 'Value';
                this.isSingleValue = true;
            }

            if (this.selectedDataType.dataType === 'ComboBox' || this.selectedDataType.dataType === 'PickList') {
                this.valueDisplay = 'Values';
                this.isSingleValue = false;
            } else {
                this.valueDisplay = 'Value';
                this.isSingleValue = true;
                
            }
        }

        public getDefaultRegex() {
            if (this.selectedDataType) {
                return this.selectedDataType.regex;    
            }
            return "";
        }
    }
}

