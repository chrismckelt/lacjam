/// Model.ts

/// <reference path="_references.ts" />

module app.model {
    export class MetadataDefinitionGroupResource {
        identity: app.model.Guid;
        name: string;
        description: string;
        tracking: app.model.TrackingBase;
        definitions: Array<any>;
    }

    export class MetadataDefinitionResource {
        identity: app.model.Guid;
        name: string;
        description: string;
        dataType: string;
        regex: string;
        values: Array<string>;
        tracking: app.model.TrackingBase;
    }

    export interface Guid extends String {
    }

    export class TrackingBase {
        isDeleted: boolean;
        createdUtcDate: Date;
        lastModifiedUtcDate: Date;
    }

    export class EntityListResource {
        totalItems: number;
        items: Array<EntityListResourceItem>;
    }

    export class EntityListResourceItem {
        id: Guid;
        name: string;
        group: string;
    }

    export class EntityResource {
        identity: Guid;
        definitionGroup: any;
        name: string;
        definitionValues: EntityMetadataDefintionResource[];
    }

    export class EntityMetadataDefintionResource {
        metadataDefinitionIdentity: app.model.Guid;
        name: string;
        dataType: string;
        regex: string;
        values: string[];
    }
}

app.registerValue('metadataDefinitionGroupResource', app.model.MetadataDefinitionGroupResource);
app.registerValue('trackingBase', app.model.TrackingBase);
