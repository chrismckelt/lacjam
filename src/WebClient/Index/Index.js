define(["require", "exports"], function(require, exports) {
    /// <reference path="../../_references.ts" />
    var Index = (function () {
        function Index() {
            this.heading = "Welcome to smsf minder!";
            this.firstName = "chris";
            this.lastName = "mckelt";
        }
        Object.defineProperty(Index.prototype, "fullName", {
            get: function () {
                return this.firstName + " " + this.lastName;
            },
            enumerable: true,
            configurable: true
        });

        Index.prototype.welcome = function () {
            alert("Welcome, " + this.fullName + "!");
        };
        return Index;
    })();
    exports.Index = Index;

    var UpperValueConverter = (function () {
        function UpperValueConverter() {
        }
        UpperValueConverter.prototype.toView = function (value) {
            return value && value.toUpperCase();
        };
        return UpperValueConverter;
    })();
    exports.UpperValueConverter = UpperValueConverter;
});
