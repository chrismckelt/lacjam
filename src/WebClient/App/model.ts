/// <reference path="_references.ts" />

module app.model {
    "use strict";


    declare module Lacjam.WebApi.Controllers.MetadataDefinition {
        interface MetadataDefinitionResource {
            identity: System.Guid;
            name: string;
            description: string;
        }
    }
    declare module System {
        interface Guid {
        }
    }
    declare module Lacjam.WebApi.Controllers.MetadataDefinitionGroup {
        interface MetadataDefinitionGroupResource {
            identity: System.Guid;
            name: string;
            description: string;
        }
    }






}
