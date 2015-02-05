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
        var MetadataDefinitionGroupEditController = (function (_super) {
            __extends(MetadataDefinitionGroupEditController, _super);
            function MetadataDefinitionGroupEditController($scope, $stateParams, $modal, $dialogs, $templateCache) {
                var _this = this;
                _super.call(this);
                this.$scope = $scope;
                this.$stateParams = $stateParams;
                this.$modal = $modal;
                this.$dialogs = $dialogs;
                this.$templateCache = $templateCache;
                this.metadataDefinitionGroupService = new app.services.MetadataDefinitionGroupService();
                this.metadataDefinitionService = new app.services.MetadataDefinitionService();
                this.editMode = false;
                this.model = new app.model.MetadataDefinitionGroupResource();
                this.save = function () {
                    if (_this.$stateParams.identity) {
                        _this.update();
                    } else {
                        _this.create();
                    }
                };
                this.create = function () {
                    app.fn.spinStart();
                    _this.model.identity = app.fn.createGuid();
                    _this.model.name = _this.model.name;
                    _this.model.description = _this.model.description;
                    _this.metadataDefinitionGroupService.create(_this.model).then(function (pro) {
                        toastr.success(_this.model.name, "Created");
                        app.fn.spinStop();
                        app.redirectToRoute(app.Routes.documents);
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

                    _this.metadataDefinitionGroupService.update(_this.model, _this.$stateParams.identity).then(function (pro) {
                        toastr.success(_this.model.name, "Saved");
                        app.fn.spinStop();
                        app.redirectToRoute(app.Routes.documents);
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
                    app.redirectToRoute(app.Routes.documents);
                    e.preventDefault();
                };

                if ($stateParams.identity) {
                    this.metadataDefinitionGroupService.get($stateParams.identity).then(function (res) {
                        _this.title = 'Editing ' + res.data.name;
                        _this.model = res.data;
                        _this.originalName = res.data.name;
                        _this.editMode = true;
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
                        data: function (term, page) {
                            return { q: term, pageSize: 10, page: page };
                        },
                        results: function (data, page) {
                            return { results: data, more: data.length >= 10 };
                        }
                    },
                    initSelection: function (data, callback) {
                        callback(_this.model.definitions);
                    },
                    multiple: true
                };
            }
            MetadataDefinitionGroupEditController.prototype.doDelete = function () {
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
                    _this.metadataDefinitionGroupService.doDelete(_this.model.identity).then(function (pro) {
                        toastr.success(_this.originalName, "Deleted");
                        app.fn.spinStop();
                        app.redirectToRoute(app.Routes.documents);
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
            MetadataDefinitionGroupEditController.$inject = ["$scope", "$stateParams", "$modal", "dialogs", "$templateCache"];
            return MetadataDefinitionGroupEditController;
        })(app.base.ControllerBase);
        controllers.MetadataDefinitionGroupEditController = MetadataDefinitionGroupEditController;
    })(app.controllers || (app.controllers = {}));
    var controllers = app.controllers;
})(app || (app = {}));
//# sourceMappingURL=../src/MetadataDefinitionGroupEditController.js.map
