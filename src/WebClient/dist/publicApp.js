/// <reference path="_references.ts" />
/* tslint:disable */
//https://gist.github.com/scottmcarthur/9051681
"use strict";
// Create and register modules
var modules = ["publicApp.directives", "publicApp.filters", "publicApp.services", "publicApp.controllers"];
angular.module("publicApp.directives", []);
angular.module("publicApp.filters", []);
angular.module("publicApp.services", []);
angular.module("publicApp.controllers", []);

///modules.forEach((m) => angular.module(m, []));
modules.push("ui", "ngCookies", "ngGrid", "ngCookies", "ngAnimate", "ngSanitize", "ngResource", "ui.router", "ui.bootstrap", "dialogs.main", "dialogs.default-translations", "pascalprecht.translate");

angular.module("publicApp", modules);
//# sourceMappingURL=../src/publicApp.js.map
