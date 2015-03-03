/// <reference path="_references.ts" />

import aur = require("aurelia-router");
import bootstrap = require("bootstrap");
import routes = require("app/routes");

export class App {
  static inject = [aur.Router];

  constructor(private router: aur.Router) {
    this.router.configure((config) => {
      config.title = "Aurelia demo";
      config.map(routes.Routes.getRoutes());
    });
  }
}