/*global jQuery, boomer, jasmine, describe, beforeEach, it, expect, spyOn*/
/*jslint plusplus: true*/

/// <reference path="~/Scripts/ThirdParty/jquery-1.10.2.js"/>
/// <reference path="~/Scripts/ThirdParty/jasmine/jasmine.js"/>
/// <reference path="../boomer.js"/>

(function ($, boomer) {
    "use strict";
    describe("Prevent non-numeric keystrokes not allowing white space", function () {
        var createKeyPress = function (input, index) {
            var press = jQuery.Event("keypress");
            press.ctrlKey = false;
            press.which = input.charCodeAt(index);
            return press;
        },
            testGoodInput = function (input) {
                var i;
                for (i = 0; i < input.length; i++) {
                    expect(boomer.preventNonNumericKeyEntryNoWhitespace(createKeyPress(input, i))).toBeTruthy();
                }
            },
            testBadInput = function (input) {
                var i;
                for (i = 0; i < input.length; i++) {
                    expect(boomer.preventNonNumericKeyEntryNoWhitespace(createKeyPress(input, i))).toBeFalsy();
                }
            };

        describe("Happy path", function () {
            it("Will allow numbers", function () {
                testGoodInput('0123456789');
            });
            it("Will not allow spaces", function () {
                testBadInput(' ');
            });
            it("Will not allow full stop", function () {
                testBadInput('.');
            });
        });

        describe("Unhappy path", function () {
            it("Will not allow lowercase letters", function () {
                testBadInput('abcdefghijklmnopqrstuvwxyz');
            });
            it("Will not allow uppercase letters", function () {
                testBadInput('ABCDEFGHIJKLMNOPQRSTUVWXYZ');
            });
            it("Will not allow special chars", function () {
                testBadInput('!@#$%^&*()`~-_=+[]{}|;:"\',<>/?\\');
            });

        });
    });
}(jQuery, boomer));
