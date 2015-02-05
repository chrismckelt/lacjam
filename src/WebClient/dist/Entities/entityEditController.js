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
        var EntityEditController = (function (_super) {
            __extends(EntityEditController, _super);
            function EntityEditController($scope, $stateParams, $state, $debounce, $dialogs, entityService) {
                var _this = this;
                _super.call(this);
                this.$stateParams = $stateParams;
                this.$state = $state;
                this.$debounce = $debounce;
                this.$dialogs = $dialogs;
                this.entityService = entityService;
                this.groupService = new app.services.MetadataDefinitionGroupService();

                var duplicate = $state.current.data.duplicate;

                var scope = {
                    title: "",
                    originalName: "",
                    editMode: false,
                    cancel: function (e) {
                        e.preventDefault();
                        app.redirectToRoute(app.Routes.entities);
                    },
                    model: null,
                    groupSelect2: {
                        ajax: {
                            url: this.groupService.getSearchUrl(),
                            data: function (term, page) {
                                return { q: term, pageSize: 10, page: page };
                            },
                            results: function (data, page) {
                                return { results: data, more: data.length >= 10 };
                            }
                        },
                        initSelection: function (data, callback) {
                            callback(scope.model.definitionGroup);
                        }
                    },
                    regex: function (def) {
                        return new RegExp(def.regex);
                    },
                    doDelete: function () {
                        //var dialog = app.resolveService<app.services.Dialog>(app.services.Dialog.prototype);
                        var msg = 'Are you sure you wish to delete this item?';

                        //var confirm = window.confirm(msg);
                        ////var confirm = dialog.showModal('Yes', 'No','Are you sure you wish to delete this item?');
                        //if (!confirm) return;
                        var opts = {};
                        opts.kb = true;
                        opts.bd = true;
                        opts.ws = 'sm'; // values: 'sm', 'lg', 'md'

                        var dlg = _this.$dialogs.confirm('Confirm', msg, opts);
                        dlg.result.then(function (btn) {
                            app.fn.spinStart();
                            entityService.doDelete(scope.model.identity).then(function (pro) {
                                toastr.success(scope.originalName, "Deleted");
                                app.fn.spinStop();
                                app.redirectToRoute(app.Routes.entities);
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
                    },
                    hasSelectedValue: function (select2Value) {
                        if (select2Value === undefined)
                            return false;

                        if ($.isArray(select2Value))
                            return select2Value.length > 0;

                        return select2Value.text != undefined && select2Value.text != '';
                    },
                    save: function () {
                        var hasSelectionValidationErrors = false;
                        for (var d in scope.model.definitionValues) {
                            var def = scope.model.definitionValues[d];
                            if (def.isSelection) {
                                if (!scope.hasSelectedValue(def.select2Value)) {
                                    toastr.error(def.name, "No Value Selected for field");
                                    hasSelectionValidationErrors = true;
                                    continue;
                                }

                                if ($.isArray(def.select2Value)) {
                                    def.values = $.map(def.select2Value, function (x) {
                                        return x.text;
                                    });
                                } else {
                                    def.values[0] = def.select2Value.text;
                                }
                            }
                        }

                        if (hasSelectionValidationErrors)
                            return;

                        if (scope.editMode) {
                            scope.update();
                        } else {
                            scope.create();
                        }
                    },
                    create: function () {
                        app.fn.spinStart();
                        scope.model.identity = app.fn.createGuid();
                        entityService.create(scope.model).then(function (pro) {
                            toastr.success(scope.model.name, "Created");
                            app.fn.spinStop();
                            app.redirectToRoute(app.Routes.entities);
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
                    },
                    update: function () {
                        app.fn.spinStart();

                        entityService.update(scope.model, _this.$stateParams.identity).then(function (pro) {
                            toastr.success(scope.model.name, "Saved");
                            app.fn.spinStop();
                            app.redirectToRoute(app.Routes.entities);
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
                    },
                    originalValues: [],
                    toBeDeletedValues: []
                };

                if ($stateParams.identity) {
                    scope.editMode = !duplicate;
                    entityService.get($stateParams.identity).then(function (res) {
                        scope.model = res.data;
                        if (duplicate) {
                            scope.title = "Create new entity";
                            scope.model.name = "";
                        } else {
                            scope.title = scope.model.name;
                        }
                        scope.originalName = scope.model.name;
                        scope.originalValues = scope.model.definitionValues;
                    });
                } else {
                    scope.title = "Create new entity";
                    scope.model = new app.model.EntityResource();
                    scope.model.identity = app.fn.createGuid();
                }

                scope = $.extend($scope, scope);

                var loadGroup = function (groupId) {
                    if (groupId) {
                        var oldDefValues = $.extend([], scope.originalValues, scope.model.definitionValues);
                        scope.model.definitionValues = [];

                        _this.groupService.getDefinitions(scope.model.definitionGroup.id).then(function (res) {
                            var definitions = res.data;
                            var definitionIds = $.map(definitions, function (d) {
                                return d.identity;
                            });

                            for (var i in definitions) {
                                var def = definitions[i];

                                var entityDef = new app.model.EntityMetadataDefintionResource;
                                entityDef.metadataDefinitionIdentity = def.identity;
                                entityDef.name = def.name;
                                entityDef.regex = def.regex;
                                entityDef.dataType = def.dataType;

                                for (var j in oldDefValues) {
                                    if (oldDefValues[j].metadataDefinitionIdentity == def.identity) {
                                        entityDef.values = $.extend([], oldDefValues[j].values);
                                        break;
                                    }
                                }

                                var isSelection = def.dataType === "ComboBox" || def.dataType === "PickList";
                                $.extend(entityDef, { isSelection: isSelection });

                                if (!entityDef.values) {
                                    if (isSelection) {
                                        entityDef.values = [];
                                    } else {
                                        entityDef.values = def.values;
                                    }
                                }

                                if (isSelection) {
                                    (function () {
                                        var isMultiple = def.dataType === "PickList";
                                        var select2Value = $.map(entityDef.values, function (x) {
                                            return { id: x, text: x };
                                        });
                                        var val = isMultiple ? select2Value : select2Value[0];
                                        var values = $.map(def.values, function (s) {
                                            return { id: s, text: s };
                                        });

                                        var select2Options = {
                                            multiple: isMultiple,
                                            initSelection: function (data, callback) {
                                                console.log(val);
                                                callback(val);
                                            }
                                        };
                                        if (isMultiple) {
                                            select2Options.tags = values;
                                        } else {
                                            select2Options.data = values;
                                        }
                                        $.extend(entityDef, {
                                            select2Options: select2Options,
                                            select2Value: val
                                        });
                                    })();
                                }

                                scope.model.definitionValues.push(entityDef);
                            }

                            scope.toBeDeletedValues = [];
                            for (var i in scope.originalValues) {
                                var defValue = scope.originalValues[i];
                                if ($.inArray(defValue.metadataDefinitionIdentity, definitionIds) < 0) {
                                    scope.toBeDeletedValues.push({
                                        name: defValue.name,
                                        value: defValue.values.join(", ")
                                    });
                                }
                            }
                        });
                    }
                };

                $scope.$watch("model.definitionGroup.id", loadGroup);
            }
            EntityEditController.$inject = ["$scope", "$stateParams", "$state", "$debounce", "dialogs", "EntityService"];
            return EntityEditController;
        })(app.base.ControllerBase);
        controllers.EntityEditController = EntityEditController;
    })(app.controllers || (app.controllers = {}));
    var controllers = app.controllers;
})(app || (app = {}));
//# sourceMappingURL=../src/entityEditController.js.map
