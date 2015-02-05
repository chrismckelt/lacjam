/// Model.ts
var app;
(function (app) {
    /// <reference path="_references.ts" />
    (function (model) {
        var MetadataDefinitionGroupResource = (function () {
            function MetadataDefinitionGroupResource() {
            }
            return MetadataDefinitionGroupResource;
        })();
        model.MetadataDefinitionGroupResource = MetadataDefinitionGroupResource;

        var MetadataDefinitionResource = (function () {
            function MetadataDefinitionResource() {
            }
            return MetadataDefinitionResource;
        })();
        model.MetadataDefinitionResource = MetadataDefinitionResource;

        var TrackingBase = (function () {
            function TrackingBase() {
            }
            return TrackingBase;
        })();
        model.TrackingBase = TrackingBase;

        var EntityListResource = (function () {
            function EntityListResource() {
            }
            return EntityListResource;
        })();
        model.EntityListResource = EntityListResource;

        var EntityListResourceItem = (function () {
            function EntityListResourceItem() {
            }
            return EntityListResourceItem;
        })();
        model.EntityListResourceItem = EntityListResourceItem;

        var EntityResource = (function () {
            function EntityResource() {
            }
            return EntityResource;
        })();
        model.EntityResource = EntityResource;

        var EntityMetadataDefintionResource = (function () {
            function EntityMetadataDefintionResource() {
            }
            return EntityMetadataDefintionResource;
        })();
        model.EntityMetadataDefintionResource = EntityMetadataDefintionResource;
    })(app.model || (app.model = {}));
    var model = app.model;
})(app || (app = {}));

app.registerValue('metadataDefinitionGroupResource', app.model.MetadataDefinitionGroupResource);
app.registerValue('trackingBase', app.model.TrackingBase);
//# sourceMappingURL=../src/model.js.map
