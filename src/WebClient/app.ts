/// <reference path="App/_references.ts" />
//
import aur = require("aurelia-router");

export class App {
  static inject = [aur.Router];

  constructor(private router: aur.Router) {
    this.router.configure((config) => {
      config.title = "Aurelia demo";
      config.map([
        { route: ["", "home"],    moduleId: "app/index",               nav: true,  title: "home" },
        { route: "login",         moduleId: "views/login",              nav: false, title: "login" },
        { route: "accounts",      moduleId: "app/accounts/accountsview",  nav: true,  title: "navigation test" },
        { route: "flickr",        moduleId: "views/flickr/flickr",      nav: true,  title: "flickr" },
        { route: "admin",         moduleId: "views/admin/admin",        nav: true,  title: "admin" }
      ]);
    });
  }
}