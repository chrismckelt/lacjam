var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var app;
(function (app) {
    /// <reference path="../_references.ts" />
    (function (services) {
        var MetadataDefinitionGroupService = (function (_super) {
            __extends(MetadataDefinitionGroupService, _super);
            function MetadataDefinitionGroupService() {
                var _this = this;
                _super.call(this, "MetadataDefinitionGroup", "MetadataDefinitionGroup");
                this.getDefinitions = function (id) {
                    return app.common.$http({ method: 'GET', url: app.fn.appUrl(_this.serviceUri + "/" + id + "/definitions") });
                };
            }
            MetadataDefinitionGroupService.prototype.getSearchUrl = function () {
                return app.fn.appUrl(this.serviceUri + "/select");
            };
            return MetadataDefinitionGroupService;
        })(app.base.ServiceBase);
        services.MetadataDefinitionGroupService = MetadataDefinitionGroupService;
    })(app.services || (app.services = {}));
    var services = app.services;
})(app || (app = {}));
//# sourceMappingURL=../src/metadataDefinitionGroupService.js.map
