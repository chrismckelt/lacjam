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
/// <reference path="~/Scripts/ViewModels/ResidentialAddressViewModel.js"/>

(function (boomer) {
    "use strict";
    var vm,
        noop = function () { /* do nothing */ },
        searchValid = function () {
            return true;
        };

    describe("ResidentialAddressViewModel", function () {
        describe("Address has already been entered", function () {
            var existingAddress = {
                AddressLine1: "Unit 4, 72 Richardson street",
                Suburb: "Gebauerton",
                State: "NSW",
                Postcode: "2020",
                ResidingFrom: "01/11/2012"
            };
            beforeEach(function () {
                vm = new boomer.ResidentialAddressViewModel(existingAddress, null, null, searchValid);
            });
            it("should fill address fields", function () {
                expect(vm.CurrentAddress().AddressLine1).toBe(existingAddress.AddressLine1);
                expect(vm.CurrentAddress().Suburb).toBe(existingAddress.Suburb);
                expect(vm.CurrentAddress().State).toBe(existingAddress.State);
                expect(vm.CurrentAddress().Postcode).toBe(existingAddress.Postcode);
                expect(vm.CurrentAddress().Id).toBe(existingAddress.Id);
            });
            it("should show address search", function () {
                expect(vm.ShouldShowAddressSearch()).toBeTruthy();
            });
            it("should show address fields", function () {
                expect(vm.ShouldShowAddressFields()).toBeTruthy();
            });
            it("should set date values", function () {
                expect(vm.DateText()).toBe("01/11/2012");
                expect(vm.Month()).toBe(11);
                expect(vm.Year()).toBe(2012);
            });
        });

        describe("Address search with one result", function () {
            var searchResult =  [{
                        AddressLine1: "Unit 4, 72 Richardson street",
                        Suburb: "Gebauerton",
                        State: "NSW",
                        Postcode: "2020"
                }],
                search = function (params, callback) {
                    callback(searchResult);
                },
                parse = function (params, callback) {
                    callback({
                        StreetLine:"Unit 4, 72 Richardson street",
                        Suburb: "Gebauerton",
                        State: "NSW",
                        Postcode: "2020"
                    });
                };;

            beforeEach(function () {
                vm = new boomer.ResidentialAddressViewModel('', search, parse, searchValid);
                vm.Search();
            });
            it("should not show search results", function () {
                expect(vm.ShouldShowSearchResults()).toBeFalsy();
            });
            it("should show address fields", function () {
                expect(vm.ShouldShowAddressFields()).toBeTruthy();
            });
            it("should have one address result", function () {
                expect(vm.Addresses().length).toBe(1);
            });
            it("should show data in address fields", function () {
                expect(vm.CurrentAddress().AddressLine1).toBe(searchResult[0].AddressLine1);
                expect(vm.CurrentAddress().Suburb).toBe(searchResult[0].Suburb);
                expect(vm.CurrentAddress().State).toBe(searchResult[0].State);
                expect(vm.CurrentAddress().Postcode).toBe(searchResult[0].Postcode);
            });
            it("should not show empty result message", function () {
                expect(vm.ShouldShowEmptyResultMessage()).toBeFalsy();
            });
        });

        describe("Address search with multiple results", function () {
            var searchResult = [
                        {
                            AddressLine1: "Unit 4, 72 Richardson street",
                            Suburb: "Gebauerton",
                            State: "NSW",
                            Postcode: "2020",
                            Id: "9876"
                        },
                        {
                            AddressLine1: "Unit 5, 72 Richardson street",
                            Suburb: "Gebauerton",
                            State: "NSW",
                            Postcode: "2020",
                            Id: "9876"
                        }
                    ],
                search = function (params, callback) {
                    callback(searchResult);
                };

            beforeEach(function () {
                vm = new boomer.ResidentialAddressViewModel('', search, noop, searchValid);
                vm.Search();
            });
            it("should show search results", function () {
                expect(vm.ShouldShowSearchResults()).toBeTruthy();
            });
            it("should show address fields", function () {
                expect(vm.ShouldShowAddressFields()).toBeTruthy();
            });
            it("should not show data in address fields", function () {
                expect(vm.CurrentAddress().AddressLine1).toBeUndefined();
                 expect(vm.CurrentAddress().Suburb).toBeUndefined();
                expect(vm.CurrentAddress().State).toBeUndefined();
                expect(vm.CurrentAddress().Postcode).toBeUndefined();
                expect(vm.CurrentAddress().Id).toBeUndefined();
            });
            it("should not show empty result message", function () {
                expect(vm.ShouldShowEmptyResultMessage()).toBeFalsy();
            });
        });

        describe("Address search with no results", function () {
            var searchResult = [],
                search = function (params, callback) {
                    callback(searchResult);
                };

            beforeEach(function () {
                vm = new boomer.ResidentialAddressViewModel('', search, null, searchValid);
                vm.Search();
            });
            it("should not show search results", function () {
                expect(vm.ShouldShowSearchResults()).toBeFalsy();
            });
            it("should show address fields", function () {
                expect(vm.ShouldShowAddressFields()).toBeTruthy();
            });
            it("should not show data in address fields", function () {
                expect(vm.CurrentAddress().AddressLine1).toBeUndefined();
                expect(vm.CurrentAddress().Suburb).toBeUndefined();
                expect(vm.CurrentAddress().State).toBeUndefined();
                expect(vm.CurrentAddress().Postcode).toBeUndefined();
                expect(vm.CurrentAddress().Id).toBeUndefined();
            });
            it("should show empty result message", function () {
                expect(vm.ShouldShowEmptyResultMessage()).toBeTruthy();
            });
        });

        describe("Address search with error", function () {
            var search = function (params, callback, errorCallback) {
                    errorCallback();
                };

            beforeEach(function () {
                vm = new boomer.ResidentialAddressViewModel('', search, null, searchValid);
                vm.Search();
            });
            it("should not show search results", function () {
                expect(vm.ShouldShowSearchResults()).toBeFalsy();
            });
            it("should show address fields", function () {
                expect(vm.ShouldShowAddressFields()).toBeTruthy();
            });
            it("should show search error", function () {
                expect(vm.ShouldShowSearchError()).toBeTruthy();
            });
            it("should not show data in address fields", function () {
                expect(vm.CurrentAddress().AddressLine1).toBeUndefined();
                expect(vm.CurrentAddress().Suburb).toBeUndefined();
                expect(vm.CurrentAddress().State).toBeUndefined();
                expect(vm.CurrentAddress().Postcode).toBeUndefined();
                expect(vm.CurrentAddress().Id).toBeUndefined();
            });
        });
    });
}(boomer));
