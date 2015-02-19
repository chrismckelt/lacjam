/// <reference path="_references.ts" />

import aur = require("aurelia-router");
import routes = require("app/routes");

export class App {
  static inject = [aur.Router];

  constructor(private router: aur.Router) {
    this.router.configure((config) => {
      config.title = "smsf minder";
      config.map(routes.Routes.getRoutes());
    });
  }
}