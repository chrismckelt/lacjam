/// <reference path="_references.ts" />

import aur = require("aurelia-router");
import routes = require("app/routes");

export class App {
  static inject = [aur.Router];

  constructor(private router: aur.Router) {
    this.router.configure((config) => {
      config.title = "Aurelia demo";
      config.map([
        { route: ["", "home"],    moduleId: "app/index",               nav: true,  title: "home" },
        { route: "login",         moduleId: "app/login",              nav: false, title: "login" },
        { route: "accounts", moduleId: routes.Routes.accounts.moduleId,  nav: true,  title: routes.Routes.accounts.title}

      ]);
    });
  }
}