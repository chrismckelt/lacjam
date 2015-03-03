define(["require", "exports", "aurelia-framework"], function(require, exports, auf) {
    var NavigationTop = (function () {
        function NavigationTop() {
        }
        NavigationTop.metadata = auf.Behavior.withProperty("router");
        return NavigationTop;
    })();
    exports.NavigationTop = NavigationTop;
});
