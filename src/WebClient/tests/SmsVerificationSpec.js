/*jslint plusplus: true*/
/*global jQuery, boomer, jasmine, describe, beforeEach, it, expect, spyOn*/

/// <reference path="~/Scripts/ThirdParty/jquery-1.10.2.js"/>
/// <reference path="~/Scripts/ThirdParty/knockout-2.2.1.debug.js" />
/// <reference path="~/Scripts/ThirdParty/jasmine/jasmine.js"/>
/// <reference path="~/Scripts/boomer.js" />
/// <reference path="~/Scripts/ViewModels/IdentificationViewModel.js" />

(function (boomer) {
    "use strict";
    var vm, hideSmsCodeEntryCallback, mobileNumberAmendCallback;

    describe("SMS verification suppression and mobile number verification", function () {
        beforeEach(function () {
            hideSmsCodeEntryCallback = jasmine.createSpy();
            mobileNumberAmendCallback = jasmine.createSpy();
            
            vm = new boomer.IdentificationViewModel(
                { RequiresSmsValidation: true, MobilePhone: '0412 345 678' },
               
                function (url, data, success) {
                    var callbackParam = { success: data.MobilePhone && data.MobilePhone.replace(/\s/g, '').length === 10 };
                    if (!callbackParam.success) {
                        callbackParam.error = 'An error occurred';
                    }
                    success(callbackParam);
                },
                mobileNumberAmendCallback,
                hideSmsCodeEntryCallback
            );
        });
        describe("clicking the link to say I haven't received a code", function () {
            beforeEach(function () {
                vm.toggleResendPanel();
            });
            describe("initially", function () {
                it("should display the resend panel", function () {
                    expect(vm.ShouldDisplayMobileNumberCheckPanel()).toBeTruthy();
                });
            });
            describe("clicking the link again", function () {
                beforeEach(function () {
                    vm.toggleResendPanel();
                });
                it("should hide the resend panel", function () {
                    expect(vm.ShouldDisplayMobileNumberCheckPanel()).toBeFalsy();
                });
            });
            describe("when my number is correct", function() {
                beforeEach(function() {
                    vm.MyNumberIsCorrect();
                });
                it("should hide all the sms fields", function() {
                    expect(vm.ShouldDisplayMobileNumberCheckPanel()).toBeFalsy();
                    expect(vm.ShouldDisplayAmendPanel()).toBeFalsy();
                    expect(vm.ShouldShowVerificationFields()).toBeFalsy();
                    expect(vm.SmsCodeIsRequiredField()).toBeFalsy();
                    expect(hideSmsCodeEntryCallback.callCount).toBe(1);
                });
            });
            describe("when my number is incorrect", function () {
                beforeEach(function () {
                    vm.ShowAmendPanel();
                });
                it("should show the amend fields", function () {
                    expect(vm.ShouldDisplayMobileNumberCheckPanel()).toBeTruthy();
                    expect(vm.ShouldDisplayAmendPanel()).toBeTruthy();
                    expect(vm.ShouldShowVerificationFields()).toBeTruthy();
                    expect(vm.SmsCodeIsRequiredField()).toBeFalsy();
                    expect(mobileNumberAmendCallback.callCount).toBe(1);
                    expect(hideSmsCodeEntryCallback.callCount).toBe(1);
                });
            });
            describe("changing to an invalid mobile number and clicking the Submit button", function () {
                beforeEach(function () {
                    vm.CurrentMobilePhone('1234');
                });
                it("should not display the invalid phone number in feedback text", function () {
                    expect(vm.CurrentValidMobilePhone()).toNotBe(vm.CurrentMobilePhone());
                });
                it("should not have called the callback", function () {
                    expect(hideSmsCodeEntryCallback.callCount).toBe(0);
                });
            });
            
        });
    });
}(boomer));
