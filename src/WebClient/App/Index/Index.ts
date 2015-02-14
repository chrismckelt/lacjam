/// <reference path="../../_references.ts" />
﻿export class Index {
    public heading: string;
    public firstName: string;
    public lastName: string;

    constructor() {
        this.heading = "Welcome to smsf minder!";
        this.firstName = "chris";
        this.lastName = "mckelt";
    }

    get fullName() {
        return this.firstName + " " + this.lastName;
    }

    welcome() {
        alert("Welcome, " + this.fullName + "!");
    }
}

export class UpperValueConverter {
    toView(value) {
        return value && value.toUpperCase();
    }
}