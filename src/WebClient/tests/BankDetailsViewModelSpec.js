/*jslint plusplus: true*/
/*global jQuery, boomer, jasmine, describe, beforeEach, it, expect, spyOn*/

/// <reference path="~/Scripts/ThirdParty/jquery-1.10.2.js"/>
/// <reference path="~/Scripts/ThirdParty/jquery.validate.js"/>
/// <reference path="~/Scripts/ThirdParty/jquery.validate.unobtrusive.js"/>
/// <reference path="~/Scripts/ThirdParty/knockout-2.2.1.debug.js"/>
/// <reference path="~/Scripts/ThirdParty/jasmine/jasmine.js"/>
/// <reference path="~/Scripts/boomer.js" />
/// <reference path="~/Scripts/ViewModels/DateSelector.js"/>
/// <reference path="~/Scripts/ViewModels/BankDetailsViewModel.js" />

(function (boomer) {
    "use strict";
    var vm, callbackHasBeenCalled,
        validBsbMessage = 'Bank: thieves-r-us, State: solid, Branch: twig',
        invalidBsbMessage = 'We are unable to verify your BSB, please check it and try again',
        data = {
            BankDetails: {
                Bsb: "123-456",
                AccountNumber: "123456789",
                AccountName: "Gebauerton"
            }
        };

    describe("BankDetailsViewModel", function () {
        describe("with previously persisted data", function () {
            beforeEach(function () {
                vm = new boomer.BankDetailsViewModel(data, function () {});
            });
            it("sets the existing values as observables", function () {
                expect(vm.Bsb()).toBe(data.BankDetails.Bsb);
                expect(vm.AccountNumber()).toBe(data.BankDetails.AccountNumber);
                expect(vm.AccountName()).toBe(data.BankDetails.AccountName);
            });
        });
        describe("callback with valid BSB", function () {
            beforeEach(function () {
                callbackHasBeenCalled = false;
                vm = new boomer.BankDetailsViewModel(data, function (ajaxData, successCallback) {
                    successCallback({ valid: true, message: validBsbMessage });
                    callbackHasBeenCalled = true;
                });
                vm.validateBsbWithBackend();
            });
            it("calls the callback", function () {
                expect(callbackHasBeenCalled).toBeTruthy();
            });
            it("displays the details element", function () {
                expect(vm.BsbDetailsVisible()).toBeTruthy();
            });
            it("doesn't display the error element", function () {
                expect(vm.BsbValidationErrorVisible()).toBeFalsy();
            });
            it("returns a success message", function () {
                expect(vm.BsbDetailsMarkup()).toBe(validBsbMessage);
            });
        });
        describe("callback with invalid BSB", function () {
            beforeEach(function () {
                callbackHasBeenCalled = false;
                vm = new boomer.BankDetailsViewModel(data, function (ajaxData, successCallback) {
                    successCallback({ valid: false, message: invalidBsbMessage });
                    callbackHasBeenCalled = true;
                });
                vm.validateBsbWithBackend();
            });
            it("calls the callback", function () {
                expect(callbackHasBeenCalled).toBeTruthy();
            });
            it("doesn't display the details element", function () {
                expect(vm.BsbDetailsVisible()).toBeFalsy();
            });
            it("displays the error element", function () {
                expect(vm.BsbValidationErrorVisible()).toBeTruthy();
            });
            it("returns an error message", function () {
                expect(vm.BsbValidationErrorMarkup()).toBe(invalidBsbMessage);
            });
        });
    });
}(boomer));
