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
        var MetadataDefinitionService = (function (_super) {
            __extends(MetadataDefinitionService, _super);
            function MetadataDefinitionService() {
                var _this = this;
                _super.call(this, "MetadataDefinition", "MetadataDefinition");
                this.getdatatypes = function () {
                    return app.common.$http({
                        method: 'GET',
                        url: app.fn.appUrl(_this.serviceUri + '/getdatatypes')
                    });
                };
                this.list = function (q, page, pageSize) {
                    return app.common.$http({
                        method: 'GET',
                        url: app.fn.appUrl(_this.serviceUri + '/list'),
                        params: { q: q, page: page, pageSize: pageSize }
                    });
                };
                this.getSearchDefinitionsUrl = function () {
                    return app.fn.appUrl(_this.serviceUri + "/select");
                };
            }
            return MetadataDefinitionService;
        })(app.base.ServiceBase);
        services.MetadataDefinitionService = MetadataDefinitionService;
    })(app.services || (app.services = {}));
    var services = app.services;
})(app || (app = {}));
//# sourceMappingURL=../src/metadataDefinitionService.js.map
