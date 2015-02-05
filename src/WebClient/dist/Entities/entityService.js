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
        var EntityService = (function (_super) {
            __extends(EntityService, _super);
            function EntityService(http) {
                var _this = this;
                _super.call(this, "Entity", "Entity");
                this.http = http;
                this.list = function (q, page, pageSize) {
                    return _this.$http({
                        method: 'GET',
                        url: app.fn.appUrl(_this.serviceUri + '/list'),
                        params: { q: q, page: page, pageSize: pageSize }
                    });
                };
                this.publicSearch = function (q, page, pageSize) {
                    return _this.$http({
                        method: 'GET',
                        url: app.fn.appUrl('search/entities'),
                        params: { q: q, page: page, pageSize: pageSize }
                    });
                };
                this.defaultSelections = function () {
                    return _this.$http({
                        method: 'GET',
                        url: app.fn.appUrl('search/entities/defaults')
                    });
                };
                this.$http = http;
            }
            EntityService.$inject = ["$http"];
            return EntityService;
        })(app.base.ServiceBase);
        services.EntityService = EntityService;

        angular.module("app.services").service({ EntityService: EntityService });
    })(app.services || (app.services = {}));
    var services = app.services;
})(app || (app = {}));
//# sourceMappingURL=../src/EntityService.js.map
