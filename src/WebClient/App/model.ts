/// Model.ts

/// <reference path="_references.ts" />

module Model {
    export class MetadataDefinitionGroupResource {
        identity: any;
        name: string;
        description: string;
        tracking: any;
        definitions: Array<any>;
    }

    export class MetadataDefinitionResource {
        identity: any;
        name: string;
        description: string;
        dataType: string;
        regex: string;
        values: Array<string>;
        tracking: any;
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
        metadataDefinitionIdentity: any;
        name: string;
        dataType: string;
        regex: string;
        values: string[];
    }
}

//lacjam.registerValue('metadataDefinitionGroupResource', lacjam.model.MetadataDefinitionGroupResource);
//lacjam.registerValue('trackingBase', any);
