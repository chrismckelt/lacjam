/*global jQuery, boomer, jasmine, describe, beforeEach, it, expect, spyOn*/
/*jslint plusplus: true*/

/// <reference path="~/Scripts/ThirdParty/jquery-1.10.2.js"/>
/// <reference path="~/Scripts/ThirdParty/jasmine/jasmine.js" />
/// <reference path="~/Scripts/boomer.js"/>

(function ($, boomer) {
    "use strict";
    describe("Prevent non-alphanumeric keystrokes", function () {
        var createKeyPress = function (input, index) {
            var press = $.Event("keypress");
            press.ctrlKey = false;
            press.which = input.charCodeAt(index);
            return press;
        },
           testGoodInput = function (input) {
               var i;
               for (i = 0; i < input.length; i++) {
                   expect(boomer.preventNonAlphaNumericKeyEntry(createKeyPress(input, i))).toBeTruthy();
               }
           },
            testBadInput = function (input) {
                var i;
                for (i = 0; i < input.length; i++) {
                    expect(boomer.preventNonAlphaNumericKeyEntry(createKeyPress(input, i))).toBeFalsy();
                }
            };

        describe("Happy path", function () {
            it("Will allow lowercase letters", function () {
                testGoodInput('abcdefghijklmnopqrstuvwxyz');
            });
            it("Will allow uppercase letters", function () {
                testGoodInput('ABCDEFGHIJKLMNOPQRSTUVWXYZ');
            });
            it("Will allow numbers", function () {
                testGoodInput('0123456789');
            });
            it("Will allow spaces", function () {
                testGoodInput(' ');
            });
            it("Will allow full stop", function () {
                testGoodInput('.');
            });
        });

        describe("Unhappy path", function () {
            it("Will not allow special chars", function () {
                testBadInput('!@#$%^&*()`~-_=+[]{}|;:"\',<>/?\\');
            });
        });
    });
}(jQuery, boomer));
