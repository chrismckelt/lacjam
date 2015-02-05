var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var app;
(function (app) {
    /// <reference path="../_references.ts" />
    (function (controllers) {
        var MetadataDefinitionEditController = (function (_super) {
            __extends(MetadataDefinitionEditController, _super);
            function MetadataDefinitionEditController($scope, $stateParams, $modal, $dialogs, $templateCache) {
                var _this = this;
                _super.call(this);
                this.$scope = $scope;
                this.$stateParams = $stateParams;
                this.$modal = $modal;
                this.$dialogs = $dialogs;
                this.$templateCache = $templateCache;
                this.metadataDefinitionService = new app.services.MetadataDefinitionService();
                this.editMode = false;
                this.model = new app.model.MetadataDefinitionResource();
                this.isSingleValue = true;
                this.valueDisplay = 'Value';
                this.save = function () {
                    if (!_this.selectedDataType || _this.selectedDataType === '') {
                        toastr.warning('Type is required');
                        return;
                    }

                    if (_this.isInvalidValue()) {
                        toastr.warning('Default value does not match rules');
                        return;
                    }

                    if (_this.$stateParams.identity) {
                        _this.update();
                    } else {
                        _this.create();
                    }
                };
                this.create = function () {
                    app.fn.spinStart();
                    _this.model.identity = app.fn.createGuid();
                    _this.metadataDefinitionService.create(_this.model).then(function (pro) {
                        toastr.success(_this.model.name, "Created");
                        app.fn.spinStop();
                        app.redirectToRoute(app.Routes.metadataDefinitions);
                    }, (function (err) {
                        app.log.error(err);
                        var msg = '';
                        angular.forEach(err.data.modelState, function (resource) {
                            msg += resource + '\n';
                        });
                        if (msg.length > 0) {
                            toastr.error(msg, "Error");
                        } else {
                            toastr.error(err.data.message, "Error");
                        }
                        app.fn.spinStop();
                    }));
                };
                this.update = function () {
                    app.fn.spinStart();

                    _this.metadataDefinitionService.update(_this.model, _this.$stateParams.identity).then(function (pro) {
                        toastr.success(_this.model.name, "Saved");
                        app.fn.spinStop();
                        app.redirectToRoute(app.Routes.metadataDefinitions);
                    }, (function (err) {
                        app.log.error(err);
                        var msg = '';
                        angular.forEach(err.data.modelState, function (resource) {
                            msg += resource + '\n';
                        });
                        if (msg.length) {
                            toastr.error(msg, "Error");
                        } else {
                            toastr.error(err.data.message, "Error");
                        }
                        app.fn.spinStop();
                    }));
                };
                this.cancel = function (e) {
                    app.redirectToRoute(app.Routes.metadataDefinitions);
                    e.preventDefault();
                };
                this.dataTypeSelected = function () {
                    _this.model.dataType = _this.selectedDataType.dataType;
                    _this.model.regex = '';

                    if (_this.selectedDataType.dataType === 'YesNo') {
                        _this.model.values = null;
                    }

                    if (_this.selectedDataType.dataType === 'ComboBox' || _this.selectedDataType.dataType === 'PickList') {
                        _this.model.values = [];
                        _this.addLine();
                    } else {
                        _this.model.values = [];
                        _this.addLine();
                    }

                    _this.setIsSingleValue();
                };
                this.addLine = function () {
                    if (_this.model.values)
                        _this.model.values.push('');
                };
                this.getmask = function () {
                    if (_this.model.regex)
                        return _this.model.regex;

                    return "(**: AAA-999)";
                };

                this.metadataDefinitionService.getdatatypes().then(function (x) {
                    _this.datatypes = x.data;
                    _this.bindData();
                });
            }
            MetadataDefinitionEditController.prototype.bindData = function () {
                var _this = this;
                if (this.$stateParams.identity) {
                    this.metadataDefinitionService.get(this.$stateParams.identity).then(function (res) {
                        _this.title = 'Editing ' + res.data.name;
                        _this.model = res.data;
                        _this.originalName = res.data.name;
                        _this.editMode = true;

                        app.fn.safeApply(function () {
                            for (var x = 0; x < _this.datatypes.length; x++) {
                                if (_this.datatypes[x].dataType == res.data.dataType) {
                                    $('#datatypeSelect').val(x.toLocaleString());
                                    _this.selectedDataType = _this.datatypes[x];
                                    _this.setIsSingleValue();
                                    if (_this.model.values.length == 0)
                                        _this.addLine();
                                }
                            }
                        });
                    });
                } else {
                    this.title = 'Create new group ';
                    this.model.identity = app.fn.createGuid();
                    this.model.name = '';
                    this.model.description = '';
                    this.model.values = [];
                    this.model.values.push('');
                }
            };

            MetadataDefinitionEditController.prototype.doDelete = function () {
                var _this = this;
                //var dialog = app.resolveService<app.services.Dialog>(app.services.Dialog.prototype);
                var msg = 'Are you sure you wish to delete this item?';

                //var confirm = window.confirm(msg);
                ////var confirm = dialog.showModal('Yes', 'No','Are you sure you wish to delete this item?');
                //if (!confirm) return;
                var opts = {};
                opts.kb = true;
                opts.bd = true;
                opts.ws = 'sm'; // values: 'sm', 'lg', 'md'

                var dlg = this.$dialogs.confirm('Confirm', msg, opts);
                dlg.result.then(function (btn) {
                    app.fn.spinStart();
                    _this.metadataDefinitionService.doDelete(_this.model.identity).then(function (pro) {
                        toastr.success(_this.originalName, "Deleted");
                        app.fn.spinStop();
                        app.redirectToRoute(app.Routes.metadataDefinitions);
                        //  e.preventDefault();
                    }, (function (err) {
                        app.log.error(err);
                        var msg = '';
                        angular.forEach(err.data.modelState, function (resource) {
                            msg += resource + '\n';
                        });
                        toastr.error(msg, "Error");
                        app.fn.spinStop();
                    }));
                }, function (btn) {
                    app.log.info("Delete metadata definition group cancelled");
                });
            };

            MetadataDefinitionEditController.prototype.removeItem = function (i) {
                delete this.model.values[i];
                this.model.values = _.filter(this.model.values, function (x) {
                    return x != undefined && x !== '';
                });
            };

            MetadataDefinitionEditController.prototype.show = function (i) {
                if (this.isSingleValue)
                    return true;

                if (this.model.values[i] || this.model.values[i] === '') {
                    return true;
                }

                return false;
            };

            MetadataDefinitionEditController.prototype.setIsSingleValue = function () {
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
            };

            MetadataDefinitionEditController.prototype.getDefaultRegex = function () {
                if (this.selectedDataType) {
                    return this.selectedDataType.regex;
                }
                return "";
            };

            MetadataDefinitionEditController.prototype.isInvalidValue = function () {
                if (!this.isSingleValue)
                    return false;

                if (this.model.values[0] === '' || this.model.values[0] === undefined)
                    return false;

                var regEx = this.model.regex;
                if (regEx === "" || regEx === undefined)
                    regEx = this.getDefaultRegex();

                return !(new RegExp(regEx).test(this.model.values[0]));
            };
            MetadataDefinitionEditController.$inject = ["$scope", "$stateParams", "$modal", "dialogs", "$templateCache"];
            return MetadataDefinitionEditController;
        })(app.base.ControllerBase);
        controllers.MetadataDefinitionEditController = MetadataDefinitionEditController;
    })(app.controllers || (app.controllers = {}));
    var controllers = app.controllers;
})(app || (app = {}));
//# sourceMappingURL=../src/metadataDefinitionEditController.js.map
