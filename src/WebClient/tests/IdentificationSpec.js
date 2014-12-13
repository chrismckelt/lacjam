/*jslint plusplus: true*/
/*global jQuery, boomer, jasmine, describe, beforeEach, it, expect, spyOn*/

/// <reference path="~/Scripts/ThirdParty/jquery-1.10.2.js"/>
/// <reference path="~/Scripts/ThirdParty/jquery.validate.js"/>
/// <reference path="~/Scripts/ThirdParty/jquery.validate.unobtrusive.js"/>
/// <reference path="~/Scripts/ThirdParty/knockout-2.2.1.debug.js"/>
/// <reference path="~/Scripts/ThirdParty/jasmine/jasmine.js"/>
/// <reference path="~/Scripts/boomer.js" />
/// <reference path="~/Scripts/ViewModels/IdentificationViewModel.js" />

(function (boomer) {
    "use strict";
    var vm;

    describe("IdentificationViewModel", function () {
        beforeEach(function () {
            vm = new boomer.IdentificationViewModel({});
        });
        describe("by default", function () {
            it("has secondary identification types", function () {
                expect(vm.SecondaryIdentificationTypes).toBeTruthy();
            });
            it("has drivers licence, passport and birth certificate as secondary identification types", function () {
                var dl = vm.SecondaryIdentificationTypes[0];
                expect(dl.Id).toBe("DriversLicence");
                expect(dl.Name).toBe("Driver's Licence");
                var pass = vm.SecondaryIdentificationTypes[1];
                expect(pass.Id).toBe("Passport");
                expect(pass.Name).toBe("Passport");
                var bc = vm.SecondaryIdentificationTypes[2];
                expect(bc.Id).toBe("BirthCertificate");
                expect(bc.Name).toBe("Birth Certificate");
            });
            it("has drivers licence selected by default in secondary identification types", function () {
                expect(vm.SecondaryIdentificationType()).toBe("DriversLicence");
            });
            it("should not show country input", function () {
                expect(vm.IsSecondaryCountryVisible()).toBeFalsy();
            });
        });
        describe("Changing the secondary id type", function () {
            describe("When selecting Passport", function () {
                beforeEach(function () {
                    vm.SecondaryIdentificationType("Passport");
                });
                it("label is Document number", function () {
                    expect(vm.SecondaryIdLabelText()).toContain("Document number");
                });
            });
            describe("When selecting Drivers Licence", function () {
                beforeEach(function () {
                    vm.SecondaryIdentificationType("DriversLicence");
                });
                it("label is Australian Driver's Licence number", function () {
                    expect(vm.SecondaryIdLabelText()).toContain("Australian Driver's Licence number");
                });
            });

        });
        describe("primary id validation", function () {
            it("can't take letters", function () {
                vm.PrimaryId("A123456789");
                expect(vm.PrimaryIdIsValid()).toBeFalsy();
            });
            it("can't take 9 digits", function () {
                vm.PrimaryId("123456789");
                expect(vm.PrimaryIdIsValid()).toBeFalsy();
            });
            it("can't take 12 digits", function () {
                vm.PrimaryId("123456789012");
                expect(vm.PrimaryIdIsValid()).toBeFalsy();
            });
            it("can't take empty string", function () {
                vm.PrimaryId("");
                expect(vm.PrimaryIdIsValid()).toBeFalsy();
            });
            it("can take 11 digits", function () {
                vm.PrimaryId("12345678910");
                expect(vm.PrimaryIdIsValid()).toBeTruthy();
            });
        });

        describe("given a invalid primary id", function () {
            beforeEach(function () {
                vm.PrimaryId("A234567891");
            });
            describe("given a valid passport and country as secondary id", function () {
                beforeEach(function () {
                    vm.SecondaryIdentificationType("Passport");
                    vm.SecondaryId("A234567891");
                    vm.SecondaryCountry("Australia");
                });

                it("should not be valid at the form level", function () {
                    expect(vm.PrimaryIdNumberIsValid()).toBeFalsy();
                    expect(vm.SecondaryIdIsValid()).toBeTruthy();
                    expect(vm.IsValid()).toBeFalsy();
                });
            });
            describe("given a valid drivers licence as secondary id", function () {
                beforeEach(function () {
                    vm.SecondaryIdentificationType("DriversLicence");
                    vm.SecondaryId("AB3456789");
                });

                it("should not be valid at the form level", function () {
                    expect(vm.PrimaryIdNumberIsValid()).toBeFalsy();
                    expect(vm.SecondaryIdIsValid()).toBeTruthy();
                    expect(vm.IsValid()).toBeFalsy();
                });
            });

        });


        describe("given a valid primary id", function () {
            beforeEach(function () {
                vm.PrimaryId("12345678901");
            });

            describe("when selecting passport as secondary Identification", function () {
                beforeEach(function () {
                    vm.SecondaryIdentificationType("Passport");
                });
                //input val, is country, 
                it("should show country input", function () {
                    expect(vm.IsSecondaryCountryVisible()).toBeTruthy();
                });
                describe("and no input provided", function () {
                    it("should be invalid", function () {
                        expect(vm.IsValid()).toBeFalsy();
                        expect(vm.SecondaryIdIsValid()).toBeFalsy();
                        expect(vm.SecondaryCountryIsValid()).toBeFalsy();
                    });
                });
                describe("and only country selected", function () {
                    beforeEach(function () {
                        vm.SecondaryCountry("Australia");
                    });
                    it("should be invalid", function () {
                        expect(vm.SecondaryIdIsValid()).toBeFalsy();
                        expect(vm.SecondaryCountryIsValid()).toBeTruthy();
                        expect(vm.IsValid()).toBeFalsy();
                    });
                });
                describe("and invalid document id provided", function () {
                    beforeEach(function () {
                        // Must have at least 1 alphanumeric char. Note: rules for doc. number are vague
                        vm.SecondaryId("");
                    });

                    describe("and country selected", function () {
                        beforeEach(function () {
                            vm.SecondaryCountry("Australia");
                        });
                        it("should be invalid", function () {
                            expect(vm.SecondaryIdIsValid()).toBeFalsy();
                            expect(vm.SecondaryCountryIsValid()).toBeTruthy();
                            expect(vm.IsValid()).toBeFalsy();
                        });
                    });
                    describe("and country not selected", function () {
                        beforeEach(function () {
                            vm.SecondaryCountry("");
                        });
                        it("should be invalid", function () {
                            expect(vm.SecondaryIdIsValid()).toBeFalsy();
                            expect(vm.SecondaryCountryIsValid()).toBeFalsy();
                            expect(vm.IsValid()).toBeFalsy();
                        });
                    });
                });
                describe("and valid document id provided", function () {
                    beforeEach(function () {
                        vm.SecondaryId("123456789A");
                    });
                    describe("and country not selected", function () {
                        beforeEach(function () {
                            vm.SecondaryCountry("");
                        });
                        it("should be invalid", function () {
                            expect(vm.SecondaryIdIsValid()).toBeTruthy();
                            expect(vm.SecondaryCountryIsValid()).toBeFalsy();
                            expect(vm.IsValid()).toBeFalsy();
                        });
                    });
                    describe("and country is selected", function () {
                        beforeEach(function () {
                            vm.SecondaryCountry("Australia");
                        });
                        it("should be valid", function () {
                            expect(vm.SecondaryIdIsValid()).toBeTruthy();
                            expect(vm.SecondaryCountryIsValid()).toBeTruthy();
                            expect(vm.IsValid()).toBeTruthy();
                        });
                    });
                });
            });

            describe("when selecting birth certificate as secondary Identification", function () {
                beforeEach(function () {
                    vm.SecondaryIdentificationType("BirthCertificate");
                });
                //input val, is country, 
                it("should show country input", function () {
                    expect(vm.IsSecondaryCountryVisible()).toBeTruthy();
                });
                describe("and no input provided", function () {
                    it("should be invalid", function () {
                        expect(vm.IsValid()).toBeFalsy();
                        expect(vm.SecondaryIdIsValid()).toBeFalsy();
                        expect(vm.SecondaryCountryIsValid()).toBeFalsy();
                    });
                });
                describe("and only country selected", function () {
                    beforeEach(function () {
                        vm.SecondaryCountry("Australia");
                    });
                    it("should be invalid", function () {
                        expect(vm.SecondaryIdIsValid()).toBeFalsy();
                        expect(vm.SecondaryCountryIsValid()).toBeTruthy();
                        expect(vm.IsValid()).toBeFalsy();
                    });
                });
                describe("and invalid document id provided", function () {
                    beforeEach(function () {
                        // Must have at least 1 alphanumeric char. Note: rules for doc. number are vague
                        vm.SecondaryId("");
                    });

                    describe("and country selected", function () {
                        beforeEach(function () {
                            vm.SecondaryCountry("Australia");
                        });
                        it("should be invalid", function () {
                            expect(vm.SecondaryIdIsValid()).toBeFalsy();
                            expect(vm.SecondaryCountryIsValid()).toBeTruthy();
                            expect(vm.IsValid()).toBeFalsy();
                        });
                    });
                    describe("and country not selected", function () {
                        beforeEach(function () {
                            vm.SecondaryCountry("");
                        });
                        it("should be invalid", function () {
                            expect(vm.SecondaryIdIsValid()).toBeFalsy();
                            expect(vm.SecondaryCountryIsValid()).toBeFalsy();
                            expect(vm.IsValid()).toBeFalsy();
                        });
                    });
                });
                describe("Invalid inputs", function () {
                    it("should be invalid with blank", function () {
                        vm.SecondaryId("");
                        expect(vm.SecondaryIdIsValid()).toBeFalsy();
                        expect(vm.IsValid()).toBeFalsy();
                    });
                    it("should be invalid with non alphanum chars", function () {
                        vm.SecondaryId("12345#");
                        expect(vm.SecondaryIdIsValid()).toBeFalsy();
                        expect(vm.IsValid()).toBeFalsy();
                    });
                    it("should be invalid with all non alphanum chars", function () {
                        vm.SecondaryId("$%^#");
                        expect(vm.SecondaryIdIsValid()).toBeFalsy();
                        expect(vm.IsValid()).toBeFalsy();
                    });
                });
                describe("and valid document id provided", function () {
                    beforeEach(function () {
                        vm.SecondaryId("123456789A");
                    });
                    describe("and country not selected", function () {
                        beforeEach(function () {
                            vm.SecondaryCountry("");
                        });
                        it("should be invalid", function () {
                            expect(vm.SecondaryIdIsValid()).toBeTruthy();
                            expect(vm.SecondaryCountryIsValid()).toBeFalsy();
                            expect(vm.IsValid()).toBeFalsy();
                        });
                    });
                    describe("and country is selected", function () {
                        beforeEach(function () {
                            vm.SecondaryCountry("Australia");
                        });
                        it("should be valid", function () {
                            expect(vm.SecondaryIdIsValid()).toBeTruthy();
                            expect(vm.SecondaryCountryIsValid()).toBeTruthy();
                            expect(vm.IsValid()).toBeTruthy();
                        });
                    });
                });
            });


            describe("when selecting drivers licence as secondary identification", function () {
                beforeEach(function () {
                    vm.SecondaryIdentificationType("DriversLicence");
                });
                //input val, is country, 
                it("should not show country input", function () {
                    expect(vm.IsSecondaryCountryVisible()).toBeFalsy();
                });
                it("and country input should be null", function () {
                    expect(vm.SecondaryCountry()).toBeFalsy();
                });
                describe("and no input provided", function () {
                    it("should be invalid", function () {
                        expect(vm.SecondaryIdIsValid()).toBeFalsy();
                        expect(vm.IsValid()).toBeFalsy();
                    });
                });

                //Licence Number rules
                //  Maximum length of 9 characters. 
                //  Alphanumeric characters only. 
                //  Must have at least 4 numeric characters. 
                //  Must have no more than 2 alphabetic characters. 
                //  The third and fourth characters must be numeric
                describe("Valid drivers licence input", function () {
                    it("should be valid with AB3456789", function () {
                        vm.SecondaryState('WA');
                        vm.SecondaryId("AB3456789");
                        expect(vm.SecondaryIdIsValid()).toBeTruthy();
                        expect(vm.IsValid()).toBeTruthy();
                    });
                    it("should be valid with AB345678", function () {
                        vm.SecondaryState('WA');
                        vm.SecondaryId("AB345678");
                        expect(vm.SecondaryIdIsValid()).toBeTruthy();
                        expect(vm.IsValid()).toBeTruthy();
                    });
                    it("should be valid with A345678B", function () {
                        vm.SecondaryState('WA');
                        vm.SecondaryId("A345678B");
                        expect(vm.SecondaryIdIsValid()).toBeTruthy();
                        expect(vm.IsValid()).toBeTruthy();
                    });
                    it("should be valid with 123456789", function () {
                        vm.SecondaryState('WA');
                        vm.SecondaryId("123456789");
                        expect(vm.SecondaryIdIsValid()).toBeTruthy();
                        expect(vm.IsValid()).toBeTruthy();
                    });
                });
                describe("Invalid drivers licence input", function () {
                    it("should be invalid with AB34567890", function () {
                        vm.SecondaryId("AB34567890"); //Too long
                        expect(vm.SecondaryIdIsValid()).toBeFalsy();
                        expect(vm.IsValid()).toBeFalsy();
                    });
                    it("should be invalid with ABCDEFGHI", function () {
                        vm.SecondaryId("ABCDEFGHI"); //No numbers
                        expect(vm.SecondaryIdIsValid()).toBeFalsy();
                        expect(vm.IsValid()).toBeFalsy();
                    });
                    it("should be invalid with 12CD5678", function () {
                        vm.SecondaryId("12CD5678"); //Numbers not in 3rd and 4th position
                        expect(vm.SecondaryIdIsValid()).toBeFalsy();
                        expect(vm.IsValid()).toBeFalsy();
                    });
                    it("should be invalid with 12CDEFG8", function () {
                        vm.SecondaryId("12CDEFG8"); //Not at least 4 numbers
                        expect(vm.SecondaryIdIsValid()).toBeFalsy();
                        expect(vm.IsValid()).toBeFalsy();
                    });
                    it("should be invalid with $*CDEFG8", function () {
                        vm.SecondaryId("$*CDEFG8"); //Illegal characters
                        expect(vm.SecondaryIdIsValid()).toBeFalsy();
                        expect(vm.IsValid()).toBeFalsy();
                    });
                    it("should be invalid with AB12", function () {
                        vm.SecondaryId("AB12"); //Minimum of four numbers
                        expect(vm.SecondaryIdIsValid()).toBeFalsy();
                        expect(vm.IsValid()).toBeFalsy();
                    });
                });
            });
        });
        
        describe("changing secondary identification type from Passport to DriversLicence", function () {
            beforeEach(function () {
                vm.PrimaryId("1234567891");
                vm.SecondaryIdentificationType("Passport");
                vm.SecondaryId("AB123456");
                vm.SecondaryCountry("Australia");
            });
            it("should clear all secondary id fields", function () {
                vm.SecondaryIdentificationType("DriversLicence");
                expect(vm.SecondaryId()).toBe("");
                expect(vm.SecondaryCountry()).toBeNull();
                expect(vm.SecondaryState()).toBeNull();
            });
        });

        describe("changing secondary identification type from DriversLicence to Passport", function () {
            beforeEach(function () {
                vm.PrimaryId("1234567891");
                vm.SecondaryIdentificationType("DriversLicence");
                vm.SecondaryId("1234567");
                vm.SecondaryState("WA");
            });
            it("should clear all secondary id fields", function () {
                vm.SecondaryIdentificationType("Passport");
                expect(vm.SecondaryId()).toBe("");
                expect(vm.SecondaryCountry()).toBeNull();
                expect(vm.SecondaryState()).toBeNull();
            });
        });
    });
}(boomer));
