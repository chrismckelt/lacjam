/*jslint plusplus: true*/
/*global jQuery, boomer, jasmine, describe, beforeEach, it, expect, spyOn*/

/// <reference path="~/Scripts/ThirdParty/jquery-1.10.2.js"/>
/// <reference path="~/Scripts/ThirdParty/jquery.validate.js"/>
/// <reference path="~/Scripts/ThirdParty/jquery.validate.unobtrusive.js"/>
/// <reference path="~/Scripts/ThirdParty/knockout-2.2.1.debug.js"/>
/// <reference path="~/Scripts/ThirdParty/bootstrap.js"/>
/// <reference path="~/Scripts/ThirdParty/moment.js" />
/// <reference path="~/Scripts/ThirdParty/jasmine/jasmine.js"/>
/// <reference path="~/Scripts/boomer.js" />
/// <reference path="~/Scripts/ViewModels/DateSelector.js"/>
/// <reference path="~/Scripts/ViewModels/PersonalDetailsViewModel.js"/>

(function (boomer) {
    "use strict";
    var vm, i;

    describe("PersonalDetailsViewModel", function () {
        describe("With no values", function() {
            beforeEach(function () {
                vm = new boomer.PersonalDetailsViewModel();
            });
            describe("When page is loaded", function () {
                it("Does not preselect a value for loan amount", function () {
                    expect(vm.LoanAmount()).toBeUndefined();
                });
                it("Does not preselect a value for loan duration", function () {
                    expect(vm.LoanDuration()).toBeUndefined();
                });
                it("Presents all options for loan amount", function () {
                    expect(vm.availableAmounts().length).toBe(20);
                    for (i = 0; i < 20; ++i) {
                        expect(vm.availableAmounts()[i].value).toBe(100 * (i + 1));
                    }
                });
                it("Presents all options for loan duration", function () {
                    expect(vm.availableDurations().length).toBe(2);
                    expect(vm.availableDurations()[0].value).toBe(6);
                    expect(vm.availableDurations()[1].value).toBe(52);
                });
                it("Does not display the Australian residents only notice", function () {
                    expect(vm.ShouldShowPermanentResidentWarning()).toBeFalsy();
                });
                it("Does not display the Benefits only notice", function () {
                    expect(vm.ShouldShowBenefitsOnlyWarning()).toBeFalsy();
                });
            });
            describe("When selecting a 12-month loan duration", function () {
                beforeEach(function () {
                    vm.LoanDuration(52);
                });
                it("Displays the Australian residents only notice", function () {
                    expect(vm.ShouldShowPermanentResidentWarning()).toBeTruthy();
                });
                it("Does not display the Benefits only notice", function () {
                    expect(vm.ShouldShowBenefitsOnlyWarning()).toBeFalsy();
                });
            });
        });
        describe("With $600 Personal Loan", function () {
            describe("With benefits only = true", function () {
                beforeEach(function () {
                    vm = new boomer.PersonalDetailsViewModel('06/07/1989', 600, 52, true);
                });
                describe("When page is loaded", function () {
                    it("Preselects the value for loan amount", function () {
                        expect(vm.LoanAmount()).toBe(600);
                    });
                    it("Preselects the value for loan duration", function () {
                        expect(vm.LoanDuration()).toBe(52);
                    });
                    it("Presents all options for loan amount", function () {
                        expect(vm.availableAmounts().length).toBe(20);
                        for (i = 0; i < 20; ++i) {
                            expect(vm.availableAmounts()[i].value).toBe(100 * (i + 1));
                        }
                    });
                    it("Presents all options for loan duration", function () {
                        expect(vm.availableDurations().length).toBe(2);
                        expect(vm.availableDurations()[0].value).toBe(6);
                        expect(vm.availableDurations()[1].value).toBe(52);
                    });
                    it("Displays the Australian residents only notice", function () {
                        expect(vm.ShouldShowPermanentResidentWarning()).toBeTruthy();
                    });
                    it("Displays the Benefits only notice", function () {
                        expect(vm.ShouldShowBenefitsOnlyWarning()).toBeTruthy();
                    });
                });
                describe("When selecting an amount less than $600", function () {
                    beforeEach(function () {
                        vm.LoanAmount(500);
                        vm.LoanDuration(null); // This happens automatically in a browser because the selected value is no longer an option
                    });
                    it("No longer presents the 12-month loan duration", function () {
                        expect(vm.availableDurations().length).toBe(1);
                        expect(vm.availableDurations()[0].value).toBe(6);
                    });
                    it("No longer displays the Australian residents only notice", function () {
                        expect(vm.ShouldShowPermanentResidentWarning()).toBeFalsy();
                    });
                    it("No longer displays the Benefits only notice", function () {
                        expect(vm.ShouldShowBenefitsOnlyWarning()).toBeFalsy();
                    });
                });
                describe("When selecting an amount greater than $600", function () {
                    beforeEach(function () {
                        vm.LoanAmount(700);
                        vm.LoanDuration(null); // This happens automatically in a browser because the selected value is no longer an option
                    });
                    it("No longer presents the 12-month loan duration", function () {
                        expect(vm.availableDurations().length).toBe(1);
                        expect(vm.availableDurations()[0].value).toBe(6);
                    });
                    it("No longer displays the Australian residents only notice", function () {
                        expect(vm.ShouldShowPermanentResidentWarning()).toBeFalsy();
                    });
                    it("No longer displays the Benefits only notice", function () {
                        expect(vm.ShouldShowBenefitsOnlyWarning()).toBeFalsy();
                    });
                });
            });
            describe("With benefits only = false", function () {
                beforeEach(function () {
                    vm = new boomer.PersonalDetailsViewModel('06/07/1989', 600, 52, false);
                });
                describe("When page is loaded", function () {
                    it("Preselects the value for loan amount", function () {
                        expect(vm.LoanAmount()).toBe(600);
                    });
                    it("Preselects the value for loan duration", function () {
                        expect(vm.LoanDuration()).toBe(52);
                    });
                    it("Presents all options for loan amount", function () {
                        expect(vm.availableAmounts().length).toBe(20);
                        for (i = 0; i < 20; ++i) {
                            expect(vm.availableAmounts()[i].value).toBe(100 * (i + 1));
                        }
                    });
                    it("Presents all options for loan duration", function () {
                        expect(vm.availableDurations().length).toBe(2);
                        expect(vm.availableDurations()[0].value).toBe(6);
                        expect(vm.availableDurations()[1].value).toBe(52);
                    });
                    it("Displays the Australian residents only notice", function () {
                        expect(vm.ShouldShowPermanentResidentWarning()).toBeTruthy();
                    });
                    it("Does not display the Benefits only notice", function () {
                        expect(vm.ShouldShowBenefitsOnlyWarning()).toBeFalsy();
                    });
                });
                describe("When selecting an amount less than $600", function () {
                    beforeEach(function () {
                        vm.LoanAmount(500);
                        vm.LoanDuration(null); // This happens automatically in a browser because the selected value is no longer an option
                    });
                    it("No longer presents the 12-month loan duration", function () {
                        expect(vm.availableDurations().length).toBe(1);
                        expect(vm.availableDurations()[0].value).toBe(6);
                    });
                    it("No longer displays the Australian residents only notice", function () {
                        expect(vm.ShouldShowPermanentResidentWarning()).toBeFalsy();
                    });
                    it("Does not display the Benefits only notice", function () {
                        expect(vm.ShouldShowBenefitsOnlyWarning()).toBeFalsy();
                    });
                });
                describe("When selecting an amount greater than $600", function () {
                    beforeEach(function () {
                        vm.LoanAmount(700);
                    });
                    it("Still presents the 12-month loan duration", function () {
                        expect(vm.availableDurations().length).toBe(2);
                        expect(vm.availableDurations()[0].value).toBe(6);
                        expect(vm.availableDurations()[1].value).toBe(52);
                    });
                    it("Still displays the Australian residents only notice", function () {
                        expect(vm.ShouldShowPermanentResidentWarning()).toBeTruthy();
                    });
                    it("Does not display the Benefits only notice", function () {
                        expect(vm.ShouldShowBenefitsOnlyWarning()).toBeFalsy();
                    });
                });
            });
        });
        describe("With $700 Personal Loan", function () {
            beforeEach(function () {
                vm = new boomer.PersonalDetailsViewModel('06/07/1989', 700, 52);
            });
            describe("When page is loaded", function () {
                it("Preselects the value for loan amount", function () {
                    expect(vm.LoanAmount()).toBe(700);
                });
                it("Preselects the value for loan duration", function () {
                    expect(vm.LoanDuration()).toBe(52);
                });
                it("Presents all options for loan amount", function () {
                    expect(vm.availableAmounts().length).toBe(20);
                    for (i = 0; i < 20; ++i) {
                        expect(vm.availableAmounts()[i].value).toBe(100 * (i + 1));
                    }
                });
                it("Presents all options for loan duration", function () {
                    expect(vm.availableDurations().length).toBe(2);
                    expect(vm.availableDurations()[0].value).toBe(6);
                    expect(vm.availableDurations()[1].value).toBe(52);
                });
                it("Displays the Australian residents only notice", function () {
                    expect(vm.ShouldShowPermanentResidentWarning()).toBeTruthy();
                });
                it("Does not display the Benefits only notice", function () {
                    expect(vm.ShouldShowBenefitsOnlyWarning()).toBeFalsy();
                });
            });
        });
        describe("With $600 Cash Advance", function () {
            beforeEach(function () {
                vm = new boomer.PersonalDetailsViewModel('06/07/1989', 600, 6);
            });
            describe("When page is loaded", function () {
                it("Preselects the value for loan amount", function () {
                    expect(vm.LoanAmount()).toBe(600);
                });
                it("Preselects the value for loan duration", function () {
                    expect(vm.LoanDuration()).toBe(6);
                });
                it("Presents all options for loan amount", function () {
                    expect(vm.availableAmounts().length).toBe(20);
                    for (i = 0; i < 20; ++i) {
                        expect(vm.availableAmounts()[i].value).toBe(100 * (i + 1));
                    }
                });
                it("Presents all options for loan duration", function () {
                    expect(vm.availableDurations().length).toBe(2);
                    expect(vm.availableDurations()[0].value).toBe(6);
                    expect(vm.availableDurations()[1].value).toBe(52);
                });
                it("Does not display the Australian residents only notice", function () {
                    expect(vm.ShouldShowPermanentResidentWarning()).toBeFalsy();
                });
                it("Does not display the Benefits only notice", function () {
                    expect(vm.ShouldShowBenefitsOnlyWarning()).toBeFalsy();
                });
            });
        });
        describe("With $500 Cash Advance", function () {
            beforeEach(function () {
                vm = new boomer.PersonalDetailsViewModel('06/07/1989', 500, 6);
            });
            describe("When page is loaded", function () {
                it("Preselects the value for loan amount", function () {
                    expect(vm.LoanAmount()).toBe(500);
                });
                it("Preselects the value for loan duration", function () {
                    expect(vm.LoanDuration()).toBe(6);
                });
                it("Presents all options for loan amount", function () {
                    expect(vm.availableAmounts().length).toBe(20);
                    for (i = 0; i < 20; ++i) {
                        expect(vm.availableAmounts()[i].value).toBe(100 * (i + 1));
                    }
                });
                it("Does not present the 12-month loan duration", function () {
                    expect(vm.availableDurations().length).toBe(1);
                    expect(vm.availableDurations()[0].value).toBe(6);
                });
                it("Does not display the Australian residents only notice", function () {
                    expect(vm.ShouldShowPermanentResidentWarning()).toBeFalsy();
                });
                it("Does not display the Benefits only notice", function () {
                    expect(vm.ShouldShowBenefitsOnlyWarning()).toBeFalsy();
                });
            });
        });
    });
}(boomer));
