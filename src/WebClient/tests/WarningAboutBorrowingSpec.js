/*jslint plusplus: true*/
/*global jQuery, boomer, jasmine, describe, beforeEach, it, expect, spyOn*/

/// <reference path="~/Scripts/ThirdParty/jquery-1.10.2.js"/>
/// <reference path="~/Scripts/ThirdParty/jquery.validate.js"/>
/// <reference path="~/Scripts/ThirdParty/jquery.validate.unobtrusive.js"/>
/// <reference path="~/Scripts/ThirdParty/knockout-2.2.1.debug.js"/>
/// <reference path="~/Scripts/ThirdParty/bootstrap.js"/>
/// <reference path="~/Scripts/customKnockoutBindings.js"/>
/// <reference path="~/Scripts/ThirdParty/jasmine/jasmine.js"/>
/// <reference path="~/Scripts/boomer.js"/>

(function (boomer, $) {
    "use strict";
    
    var $form = $('<form></form>'),
        $modal = $('<div></div>');

    describe("WarningAboutBorrowing", function () {
        beforeEach(function () {
            boomer.setupBorrowingWarning($form, $modal, 'some-url');
        });
        describe("On submitting the form for new loan", function () {
            beforeEach(function () {
                spyOn(boomer, 'showBorrowingWarningAjax');
                $form.submit();
            });
            it("displays the borrowing warning", function () {
                expect(boomer.showBorrowingWarningAjax).toHaveBeenCalled();
            });
            it("does not automatically accept the warning", function () {
                expect(boomer.hasAcceptedBorrowingWarning()).toBeFalsy();
            });
        });
    });
}(boomer, jQuery));
