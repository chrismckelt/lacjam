describe("Slots", () => {
    var $rootScope;
    var $controller;
    var $scope = new Object();
    var controller:app.controllers.ClientController;

    beforeEach(module("app"));
    beforeEach(module("app.filters"));
    beforeEach(module("app.services"));
    beforeEach(module("app.directives"));
    beforeEach(module("app.controllers"));

    beforeEach(inject(function ($injector) {
        app.registerController("ClientController", app.controllers.ClientController);  

        $rootScope = $injector.get('$rootScope');
        $controller = $injector.get('$controller');
        $scope = $rootScope.$new();
       

    }));
    beforeEach(inject(function($controller) {
        controller = app.resolveByName('ClientController');
    }));

    it("should be filled in 30 minute segments", () => {
        
        console.info('slots count = ' + controller.slots.length);
        expect(controller.slots.length).toBeGreaterThan(0);

    });
}); 