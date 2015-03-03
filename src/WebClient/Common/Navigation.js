define(["require", "exports", "aurelia-framework"], function(require, exports, auf) {
    var Navigation = (function () {
        function Navigation() {
        }
        Navigation.metadata = auf.Behavior.withProperty("router");
        return Navigation;
    })();
    exports.Navigation = Navigation;
});
