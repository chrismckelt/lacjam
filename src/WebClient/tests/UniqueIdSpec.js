/// <reference path="~/Scripts/ThirdParty/jquery-1.10.2.js"/>
/// <reference path="~/Scripts/ThirdParty/jasmine/jasmine.js"/>
/// <reference path="~/Scripts/ThirdParty/knockout-2.2.1.debug.js" />
/// <reference path="~/Scripts/customKnockoutBindings.js" />

describe("Unique Id", function () {
    
    var createuniqueIdInitPayload = function () {
        return {
            element: { setAttribute: function () { } },//required for spies, this is a dom element in the real world
            valueAccessor: function () { return 'myPropertyName'; },
            allBindingsAccessor: {},//not used
            viewModel: { 'myPropertyName': { value: 'someValue' } }
        };
    };
    describe("can assign dom values", function () {
        var payload;
        beforeEach(function () {
            payload = createuniqueIdInitPayload();
        });

        it("should get an id assigned", function () {
            expect(payload.element.id).toBeUndefined();
            ko.bindingHandlers.uniqueId.init(payload.element, payload.valueAccessor, payload.allBindingsAccessor, payload.viewModel);
            expect(payload.element.id).not.toBeUndefined();
        });
        it("assigned id should have no square brackets", function () {
            //invalid dom id to have [] in the id
            ko.bindingHandlers.uniqueId.init(payload.element, payload.valueAccessor, payload.allBindingsAccessor, payload.viewModel);
            expect(payload.element.id.indexOf('[')).toBe(-1);
            expect(payload.element.id.indexOf(']')).toBe(-1);
        });
        it("should get a name assigned", function () {
            expect(payload.element.name).toBeUndefined();
            ko.bindingHandlers.uniqueId.init(payload.element, payload.valueAccessor, payload.allBindingsAccessor, payload.viewModel);
            expect(payload.element.name).not.toBeUndefined();
        });
        it("assigned name should have square brackets", function () {
            //required for MVC model binder to turn form post back into a c# object
            ko.bindingHandlers.uniqueId.init(payload.element, payload.valueAccessor, payload.allBindingsAccessor, payload.viewModel);
            expect(payload.element.name.indexOf('[')).toNotBe(-1);
            expect(payload.element.name.indexOf(']')).toNotBe(-1);
        });
        it("should have same index for Id and Name for the same property", function () {
            ko.bindingHandlers.uniqueId.init(payload.element, payload.valueAccessor, payload.allBindingsAccessor, payload.viewModel);
            var id = payload.element.id;
            var name = payload.element.name;
            var idIndex = id.substring(id.indexOf('_') + 1, id.lastIndexOf('_'));
            var nameIndex = name.substring(name.indexOf('[') + 1, name.indexOf(']'));
            expect(idIndex).toBe(nameIndex);
        });
    });


    describe("Multiple assignments", function () {
        var payload;

        beforeEach(function () {
            payload = createuniqueIdInitPayload();
            ko.bindingHandlers.uniqueId.init(payload.element, payload.valueAccessor, payload.allBindingsAccessor, payload.viewModel);
        });

        it("should get the same id for the same referenced inputs", function () {
            expect(payload.element.id).not.toBeUndefined();
            var existingid = payload.element.id;
            ko.bindingHandlers.uniqueId.init(payload.element, payload.valueAccessor, payload.allBindingsAccessor, payload.viewModel);
            expect(payload.element.id).toBe(existingid);
        });
        it("should get the same name for the same referenced inputs", function () {
            expect(payload.element.name).not.toBeUndefined();
            var existingName = payload.element.name;
            ko.bindingHandlers.uniqueId.init(payload.element, payload.valueAccessor, payload.allBindingsAccessor, payload.viewModel);
            expect(payload.element.name).toBe(existingName);
        });
        it("should not get the same id for different referenced inputs", function () {
            expect(payload.element.id).not.toBeUndefined();
            var payload2 = createuniqueIdInitPayload();
            expect(payload2.element.id).toBeUndefined();

            var existingid = payload.element.id;
            ko.bindingHandlers.uniqueId.init(payload2.element, payload2.valueAccessor, payload2.allBindingsAccessor, payload2.viewModel);
            expect(payload2.element.id).not.toBeUndefined();
            expect(payload2.element.id).not.toBe(existingid);
        });
    });

    //BUG - when we have a colletion of objects and collection item has an input that is not in the previous element the input for the later element will have the id index of the previous elemnt
    describe("Properties on same object", function () {
        var binding1, binding2, binding3;

        var getIdIndex = function(domId) {
            return domId.substring(domId.indexOf('_') + 1, domId.lastIndexOf('_'));
        };
        var getNameIndex = function (name) {
            return name.substring(name.indexOf('[') + 1, name.indexOf(']'));
        };

        beforeEach(function () {
            var viewModel1 = { 'myPropertyName': { value: 'someValue' } };
            var viewModel2 = { 'myPropertyName': { value: 'someValue' },
                               'myOtherPropertyName': { value: 'someOtherValue' }};
         
            binding1 = {
                element: { setAttribute: function () { } },
                valueAccessor: function () { return 'myPropertyName'; },
                allBindingsAccessor: {},
                viewModel: viewModel1
            };
            binding2 = {
                element: { setAttribute: function () { } },
                valueAccessor: function () { return 'myPropertyName'; },//property is in the first vm
                allBindingsAccessor: {},
                viewModel: viewModel2 //vm2
            };
            binding3 = {
                element: { setAttribute: function () { } },
                valueAccessor: function () { return 'myOtherPropertyName'; },//property is NOT in the first vm
                allBindingsAccessor: {},
                viewModel: viewModel2 //vm2
            };
            
            ko.bindingHandlers.uniqueId.init(binding1.element, binding1.valueAccessor, binding1.allBindingsAccessor, binding1.viewModel);
            ko.bindingHandlers.uniqueId.init(binding2.element, binding2.valueAccessor, binding2.allBindingsAccessor, binding2.viewModel);
            ko.bindingHandlers.uniqueId.init(binding3.element, binding3.valueAccessor, binding3.allBindingsAccessor, binding3.viewModel);
        });
        
        it("should get same id index for properties off same vm", function () {
            var binding2Index = getIdIndex(binding2.element.id);
            var binding3Index = getIdIndex(binding3.element.id);
            expect(binding2Index).toBe(binding3Index);
        });
        it("should get same name index for properties off same vm", function () {
            var binding2Index = getNameIndex(binding2.element.name);
            var binding3Index = getNameIndex(binding3.element.name);
            expect(binding2Index).toBe(binding3Index);
        });
        
        it("should not get same id index for properties off different vm", function () {
            var binding1Index = getIdIndex(binding1.element.id);
            var binding2Index = getIdIndex(binding2.element.id);
            expect(binding1Index).not.toBe(binding2Index);
        });
        it("should not get same name index for properties off different vm", function () {
            var binding1Index = getNameIndex(binding1.element.name);
            var binding2Index = getNameIndex(binding2.element.name);
            expect(binding1Index).not.toBe(binding2Index);
        });
    });

    describe("Unique For", function () {
        var payload;

        beforeEach(function () {
            payload = createuniqueIdInitPayload();
            ko.bindingHandlers.uniqueId.init(payload.element, payload.valueAccessor, payload.allBindingsAccessor, payload.viewModel);
        });
        it("Should assign the exisiting id to the for property", function () {
            var otherPayload = createuniqueIdInitPayload();
            spyOn(otherPayload.element, 'setAttribute');
            otherPayload.viewModel = payload.viewModel;//need to have the same backing vm
            ko.bindingHandlers.uniqueFor.init(otherPayload.element, otherPayload.valueAccessor, otherPayload.allBindingsAccessor, otherPayload.viewModel);
            //assert spy expectations on the jQuery setAttribute
            expect(otherPayload.element.setAttribute).toHaveBeenCalledWith('for', payload.element.id);
        });
    });
    
    describe("Validation message for", function () {
        var payload;

        beforeEach(function () {
            payload = createuniqueIdInitPayload();
            ko.bindingHandlers.uniqueId.init(payload.element, payload.valueAccessor, payload.allBindingsAccessor, payload.viewModel);
        });
        it("Should assign the exisiting id to the for property", function () {
            var otherPayload = createuniqueIdInitPayload();
            spyOn(otherPayload.element, 'setAttribute');
            otherPayload.viewModel = payload.viewModel;//need to have the same backing vm
            ko.bindingHandlers.valmsgFor.init(otherPayload.element, otherPayload.valueAccessor, otherPayload.allBindingsAccessor, otherPayload.viewModel);
            //assert spy expectations on the jQuery setAttribute
            expect(otherPayload.element.setAttribute).toHaveBeenCalledWith('data-valmsg-for', payload.viewModel.myPropertyName.formInputName);
        });
    });
    describe("Prefix", function () {
        var expectedPrefix = "Bobbyboy", payload;
        beforeEach(function () {
            ko.bindingHandlers.uniqueId.setPrefix(expectedPrefix);
            payload = createuniqueIdInitPayload();
        });
        it("Should get an id with the prefix", function () {
            ko.bindingHandlers.uniqueId.init(payload.element, payload.valueAccessor, payload.allBindingsAccessor, payload.viewModel);
            expect(payload.element.id.indexOf(expectedPrefix)).toBe(0);
        });
        it("Should get an name with the prefix", function () {
            ko.bindingHandlers.uniqueId.init(payload.element, payload.valueAccessor, payload.allBindingsAccessor, payload.viewModel);
            expect(payload.element.name.indexOf(expectedPrefix)).toBe(0);
        });
    });
})