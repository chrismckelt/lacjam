/*jslint plusplus: true*/
/*global jQuery, boomer, ko, moment, jasmine, describe, beforeEach, it, expect, spyOn*/

/// <reference path="~/Scripts/ThirdParty/jquery-1.10.2.js"/>
/// <reference path="~/Scripts/ThirdParty/jquery.validate.js"/>
/// <reference path="~/Scripts/ThirdParty/jquery.validate.unobtrusive.js"/>
/// <reference path="~/Scripts/ThirdParty/knockout-2.2.1.debug.js"/>
/// <reference path="~/Scripts/ThirdParty/moment.js" />
/// <reference path="~/Scripts/ThirdParty/jasmine/jasmine.js"/>
/// <reference path="~/Scripts/boomer.js"/>
/// <reference path="~/Scripts/ViewModels/DateSelector.js"/>
/// <reference path="~/Scripts/ViewModels/EmploymentDetailsViewModel.js"/>

(function (boomer, ko, $, moment) {
    "use strict";
    var edViewModel,
        getItemDisplay = function (item) {
            return item.Display;
        },
        getItemValue = function (item) {
            return item.Value;
        };

    describe("EmploymentDetailsViewModel and children", function () {
        describe("EmploymentDetailsViewModel", function () {
            describe("Is initialised correctly", function () {
                beforeEach(function () {
                    edViewModel = new boomer.EmploymentDetailsViewModel();
                });
                it("Has Income sources", function () {
                    expect(edViewModel.IncomeSources).toBeDefined();
                });
                describe("income sources", function () {
                    var incomeSourceNames;

                    beforeEach(function () {
                        incomeSourceNames = edViewModel.IncomeSources;
                    });

                    it("Has Employed Income source", function () {
                        expect(incomeSourceNames).toContain({ Text: "Employed", Value: "Employed" });
                    });
                    it("Has Self-employed Income source", function () {
                        expect(incomeSourceNames).toContain({ Text: "Self-employed", Value: "Self-employed" });
                    });
                    it("Has Centrelink benefits Income source", function () {
                        expect(incomeSourceNames).toContain({ Text: "Centrelink benefits", Value: "Benefits" });
                    });
                    it("Has Other Income source", function () {
                        expect(incomeSourceNames).toContain({ Text: "Other", Value: "Other" });
                    });
                });

                it("Has an empty EmploymentHistory", function () {
                    edViewModel.Customer.IsBenefitsOnly = true;
                    expect(edViewModel.Customer.EmploymentHistory().length).toBe(0);
                });
                it("Can not submit by default", function () {
                    expect(edViewModel.Scope.CanSubmit()).toBeFalsy();
                });
            });

            describe("Handles 'Employed' Template", function () {
                beforeEach(function () {
                    edViewModel = new boomer.EmploymentDetailsViewModel();
                    var employedSource = ko.utils.arrayFirst(edViewModel.IncomeSources, function (incomeSourceItem) { return incomeSourceItem.Value === "Employed"; });
                    expect(employedSource.Value).toBe("Employed");
                    edViewModel.Customer.IncomeSource(employedSource.Value);
                });
                it("Has IncomeSource set as Employed", function () {
                    expect(edViewModel.Customer.IncomeSource()).toBe("Employed");
                });
                describe("Creating from template", function () {
                    beforeEach(function () {
                        edViewModel.addEmployment();
                    });
                    it("Has a single EmploymentHistory", function () {
                        expect(edViewModel.Customer.EmploymentHistory().length).toBe(1);
                    });
                    it("Item is not marked as removed", function () {
                        expect(edViewModel.Customer.EmploymentHistory()[0].IsRemoved()).toBeFalsy();
                    });
                    it("Has model with template of 'Employed' as first record of EmploymentHistory", function () {
                        expect(edViewModel.Customer.EmploymentHistory()[0].TemplateName()).toBe("Employed");
                    });
                    it("Can submit", function () {
                        expect(edViewModel.Scope.CanSubmit()).toBeTruthy();
                    });
                });
                describe("Removing an employment history record", function () {
                    beforeEach(function () {
                        edViewModel.addEmployment();
                        edViewModel.Customer.EmploymentHistory()[0].Remove();
                    });
                    it("remains in EmploymentHistory and is marked as deleted", function () {
                        expect(edViewModel.Customer.EmploymentHistory().length).not.toBe(0);
                        expect(edViewModel.Customer.EmploymentHistory()[0].IsRemoved()).toBeTruthy();
                    });
                    it("Can not submit", function () {
                        expect(edViewModel.Scope.CanSubmit()).toBeFalsy();
                    });
                });
            });

            describe("Handles 'Self Employed' Template", function () {
                beforeEach(function () {
                    edViewModel = new boomer.EmploymentDetailsViewModel();
                    var employedSource = ko.utils.arrayFirst(edViewModel.IncomeSources, function (incomeSourceItem) { return incomeSourceItem.Value === "Self-employed"; });
                    expect(employedSource.Value).toBe("Self-employed");
                    edViewModel.Customer.IncomeSource(employedSource.Value);
                });
                it("Has IncomeSource set as SelfEmployed", function () {
                    expect(edViewModel.Customer.IncomeSource()).toBe("Self-employed");
                });
                describe("Creating from template", function () {
                    beforeEach(function () {
                        edViewModel.addEmployment();
                    });
                    it("Has a single EmploymentHistory", function () {
                        expect(edViewModel.Customer.EmploymentHistory().length).toBe(1);
                    });
                    it("Item is not marked as removed", function () {
                        expect(edViewModel.Customer.EmploymentHistory()[0].IsRemoved()).toBeFalsy();
                    });
                    it("Has model with template of 'Self-Employed' as first record of EmploymentHistory", function () {
                        expect(edViewModel.Customer.EmploymentHistory()[0].TemplateName()).toBe("Self-employed");
                    });
                    it("Can submit", function () {
                        expect(edViewModel.Scope.CanSubmit()).toBeTruthy();
                    });
                });
                describe("Removing an employment history record", function () {
                    beforeEach(function () {
                        edViewModel.addEmployment();
                        edViewModel.Customer.EmploymentHistory()[0].Remove();
                    });
                    it("remains in EmploymentHistory and is marked as deleted", function () {
                        expect(edViewModel.Customer.EmploymentHistory().length).not.toBe(0);
                        expect(edViewModel.Customer.EmploymentHistory()[0].IsRemoved()).toBeTruthy();
                    });
                    it("Can not submit", function () {
                        expect(edViewModel.Scope.CanSubmit()).toBeFalsy();
                    });
                });
            });

            describe("Handles 'Benefits' Template", function () {
                beforeEach(function () {
                    //note the drop down value is not the same as the template name
                    edViewModel = new boomer.EmploymentDetailsViewModel();
                    var employedSource = ko.utils.arrayFirst(edViewModel.IncomeSources, function (incomeSourceItem) { return incomeSourceItem.Value === "Benefits"; });
                    expect(employedSource.Value).toBe("Benefits");
                    edViewModel.Customer.IncomeSource(employedSource.Value);
                });
                it("Has IncomeSource set as On benefits", function () {
                    expect(edViewModel.Customer.IncomeSource()).toBe("Benefits");
                });
                describe("Creating from template", function () {
                    beforeEach(function () {
                        edViewModel.addEmployment();
                    });
                    it("Has a single EmploymentHistory", function () {
                        expect(edViewModel.Customer.EmploymentHistory().length).toBe(1);
                    });
                    it("Item is not marked as removed", function () {
                        expect(edViewModel.Customer.EmploymentHistory()[0].IsRemoved()).toBeFalsy();
                    });
                    it("Has model with template of 'Benefits' as first record of EmploymentHistory", function () {
                        expect(edViewModel.Customer.EmploymentHistory()[0].TemplateName()).toBe("Benefits");
                    });
                    it("Can submit", function () {
                        expect(edViewModel.Scope.CanSubmit()).toBeTruthy();
                    });
                });
                describe("Removing an employment history record", function () {
                    beforeEach(function () {
                        edViewModel.addEmployment();
                        edViewModel.Customer.EmploymentHistory()[0].Remove();
                    });
                    it("remains in EmploymentHistory and is marked as deleted", function () {
                        expect(edViewModel.Customer.EmploymentHistory().length).not.toBe(0);
                        expect(edViewModel.Customer.EmploymentHistory()[0].IsRemoved()).toBeTruthy();
                    });
                    it("Can not submit", function () {
                        expect(edViewModel.Scope.CanSubmit()).toBeFalsy();
                    });
                });
            });

            describe("Handles 'Other' Template", function () {
                beforeEach(function () {
                    //note the drop down value is not the same as the template name
                    edViewModel = new boomer.EmploymentDetailsViewModel();
                    var employedSource = ko.utils.arrayFirst(edViewModel.IncomeSources, function (incomeSourceItem) { return incomeSourceItem.Value === "Other"; });
                    expect(employedSource.Value).toBe("Other");
                    edViewModel.Customer.IncomeSource(employedSource.Value);
                });
                it("Has IncomeSource set as Other", function () {
                    expect(edViewModel.Customer.IncomeSource()).toBe("Other");
                });
                describe("Creating from template", function () {
                    beforeEach(function () {
                        edViewModel.addEmployment();
                    });
                    it("Has a single EmploymentHistory", function () {
                        expect(edViewModel.Customer.EmploymentHistory().length).toBe(1);
                    });
                    it("Item is not marked as removed", function () {
                        expect(edViewModel.Customer.EmploymentHistory()[0].IsRemoved()).toBeFalsy();
                    });
                    it("Has model with template of 'Other' as first record of EmploymentHistory", function () {
                        expect(edViewModel.Customer.EmploymentHistory()[0].TemplateName()).toBe("Other");
                    });
                    it("Can submit", function () {
                        expect(edViewModel.Scope.CanSubmit()).toBeTruthy();
                    });
                });
                describe("Removing an employment history record", function () {
                    beforeEach(function () {
                        edViewModel.addEmployment();
                        edViewModel.Customer.EmploymentHistory()[0].Remove();
                    });
                    it("remains in EmploymentHistory and is marked as deleted", function () {
                        expect(edViewModel.Customer.EmploymentHistory().length).not.toBe(0);
                        expect(edViewModel.Customer.EmploymentHistory()[0].IsRemoved()).toBeTruthy();
                    });
                    it("Can not submit", function () {
                        expect(edViewModel.Scope.CanSubmit()).toBeFalsy();
                    });
                });
            });
            describe("Handles personal loans with income under 300 per week", function () {
                var 
                    generateLoanAmount,
                    generateLoanTerm,
                    originalLoanAmount,
                    onSubmitReturnValue,
                    pickRandom = function (list) {
                        return list[Math.floor(Math.random() * list.length)];
                    },
                    addBenefits = function () {
                        edViewModel.Customer.IncomeSource('Benefits');
                        edViewModel.addEmployment();
                        var benefits = edViewModel.Customer.EmploymentHistory().pop();
                        benefits.EmployerName(pickRandom(benefits.CentrelinkBenefitTypes));
                        benefits.StartDateMonth(pickRandom(benefits.Months));
                        benefits.StartDateYear(pickRandom(benefits.Years));
                        benefits.PayFrequency("Weekly");
                        benefits.NetIncome(5);
                        edViewModel.Customer.EmploymentHistory.push(benefits);
                    };

                beforeEach(function () {
                    generateLoanAmount = function () {
                        return (Math.floor(Math.random() * 14) + 7) * 100;
                    };
                    generateLoanTerm = function () {
                        return 52;
                    };
                });
                describe("When user submits form and then accepts cash advance warning", function () {
                    beforeEach(function () {
                        edViewModel = new boomer.EmploymentDetailsViewModel(null, 250, 6, null, null);
                        addBenefits();
                        onSubmitReturnValue = edViewModel.onSubmit();
                       

                    });
   
                    it("should display showWarningForBenefitsOnlyCashAdvance", function () {
                        expect(edViewModel.Scope.Modal()).toBe(edViewModel.Modals.showWarningForBenefitsOnlyCashAdvance);
                    });
                });

                describe("When user submits form and then accepts cash advance warning", function () {
                    beforeEach(function () {
                        edViewModel = new boomer.EmploymentDetailsViewModel(null, generateLoanAmount(), generateLoanTerm(), null, null);
                        addBenefits();
                        onSubmitReturnValue = edViewModel.onSubmit();

                    });

                    it("Allows form submission", function () {
                        
                        onSubmitReturnValue = edViewModel.setToCashAdvance();
                        expect(onSubmitReturnValue).toBe(true);
                    });
                });

                describe("When user submits form and then accepts cash advance warning", function () {
                    beforeEach(function () {
                        edViewModel = new boomer.EmploymentDetailsViewModel(null, generateLoanAmount(), generateLoanTerm(), null, null);
                        addBenefits();
                        onSubmitReturnValue = edViewModel.onSubmit();
                        edViewModel.setToCashAdvance();

                    });

                    it("Sets the loan amount to 200", function () {
                        expect(edViewModel.Loan.LoanAmount()).toBe(200);
                    });
                    it("Sets the loan term to 6 weeks", function () {
                        expect(edViewModel.Loan.LoanTerm()).toBe(6);
                    });
                });

                describe("When user submits form and then rejects / cancels modification of loan type", function () {
                    beforeEach(function () {
                        edViewModel = new boomer.EmploymentDetailsViewModel(null, 250, 52, null, null);
                        addBenefits();
                        onSubmitReturnValue = edViewModel.onSubmit();
                    });

                    it("sets Modal option to showWarningForPersonalLoanUnder300", function () {
                        expect(edViewModel.Scope.Modal()).toBe(edViewModel.Modals.showWarningForPersonalLoanUnder300);
                    });

                    it("Prevents form submission", function () {
                        expect(onSubmitReturnValue).toBe(false);
                    });
                });

                describe("When user submits form and then rejects / cancels modification of loan type", function () {
                    beforeEach(function () {
                        edViewModel = new boomer.EmploymentDetailsViewModel(null, generateLoanAmount(), generateLoanTerm(), null, null); addBenefits();
                        originalLoanAmount = edViewModel.Loan.LoanAmount();
                        onSubmitReturnValue = edViewModel.onSubmit();
                        
                    });
                    it("Does not change loan amount", function () {
                        expect(edViewModel.Loan.LoanAmount()).toBe(originalLoanAmount);
                    });
                    it("Does not change loan term", function () {
                        expect(edViewModel.Loan.LoanTerm()).toBe(52);
                    });

                });

            });

            describe("Handles benefits-only warning correctly", function () {
                var generateLoanAmount, generateLoanTerm, originalLoanAmount, onSubmitReturnValue,
                    pickRandom = function (list) {
                        return list[Math.floor(Math.random() * list.length)];
                    },
                    addBenefits = function () {
                        edViewModel.Customer.IncomeSource('Benefits');
                        edViewModel.addEmployment();
                        var benefits = edViewModel.Customer.EmploymentHistory().pop();
                        benefits.EmployerName(pickRandom(benefits.CentrelinkBenefitTypes));
                        benefits.StartDateMonth(pickRandom(benefits.Months));
                        benefits.StartDateYear(pickRandom(benefits.Years));
                        benefits.PayFrequency(pickRandom(benefits.PayFrequencies));
                        benefits.NetIncome(getRandomInt(1, 300));
                        edViewModel.Customer.EmploymentHistory.push(benefits);
                    },
                    addOther = function () {
                        edViewModel.Customer.IncomeSource('Other');
                        edViewModel.addEmployment();
                        var other = edViewModel.Customer.EmploymentHistory().pop();
                        other.EmployerName('Test');
                        other.StartDateMonth(pickRandom(other.Months));
                        other.StartDateYear(pickRandom(other.Years));
                        other.PayFrequency(pickRandom(other.PayFrequencies));
                        other.NetIncome(getRandomInt(1,300));
                        edViewModel.Customer.EmploymentHistory.push(other);
                    };
                describe("With loan type of 'Personal Loan' and loan amount over $600", function () {
                    beforeEach(function () {
                        generateLoanAmount = function () {
                            return (Math.floor(Math.random() * 14) + 7) * 100;
                        };
                        generateLoanTerm = function () {
                            return 52;
                        };
                    });
                    describe("With a single benefits income", function () {
                        describe("When user submits form and then accepts benefits-only warning", function () {
                            beforeEach(function () {
                                edViewModel = new boomer.EmploymentDetailsViewModel(null, generateLoanAmount(), generateLoanTerm(), null, null);
                                addBenefits();
                                onSubmitReturnValue = edViewModel.onSubmit();
                                
                            });
                            it("sets modal to showWarningForBenefitsOnly", function () {
                                expect(edViewModel.Scope.Modal()).toNotBe(null);
                            });
                            it("Sets the loan amount to 600", function () {
                                edViewModel.setToPersonalLoan();
                                expect(edViewModel.Loan.LoanAmount()).toBe(600);
                            });
                            it("Prevents form submission", function () {
                                onSubmitReturnValue = edViewModel.onSubmit();
                                expect(onSubmitReturnValue).toBe(false);
                            });
                        });
                        describe("When user submits form and then rejects / cancels modification of loan amount", function () {
                            beforeEach(function () {
  
                                edViewModel = new boomer.EmploymentDetailsViewModel(null, generateLoanAmount(), generateLoanTerm(), null, null);
                                originalLoanAmount = edViewModel.Loan.LoanAmount();
                                addBenefits();
                                onSubmitReturnValue = edViewModel.onSubmit();
                                
                            });
                            it("Does not change loan amount", function () {
                                expect(edViewModel.Loan.LoanAmount()).toBe(originalLoanAmount);
                            });
                            it("Prevents form submission", function () {
                                onSubmitReturnValue = edViewModel.onSubmit();
                                expect(onSubmitReturnValue).toBe(false);
                            });
                        });
                    });
                    describe("With multiple benefits incomes", function () {
                        describe("When user submits form and then accepts benefits-only warning", function () {
                            beforeEach(function () {
                                 edViewModel = new boomer.EmploymentDetailsViewModel(null, generateLoanAmount(), generateLoanTerm(),null, null);
                                addBenefits();
                                addBenefits();
                                addBenefits();
                                onSubmitReturnValue = edViewModel.onSubmit();
                            });
                            it("sets modal options toshowWarningForBenefitsOnly", function () {
                                expect(edViewModel.Scope.Modal()).toNotBe(null);
                            });
                            it("Sets the loan amount to 600", function () {
                                edViewModel.setToPersonalLoan();
                                expect(edViewModel.Loan.LoanAmount()).toBe(600);
                            });
                            it("Prevents form submission", function () {
                                onSubmitReturnValue = edViewModel.onSubmit();
                                expect(onSubmitReturnValue).toBe(false);
                            });
                        });
                        describe("When user submits form and then rejects / cancels modification of loan amount", function () {
                            beforeEach(function () {
                                edViewModel = new boomer.EmploymentDetailsViewModel(null, generateLoanAmount(), generateLoanTerm(),null, null);
                                originalLoanAmount = edViewModel.Loan.LoanAmount();
                                addBenefits();
                                addBenefits();
                                addBenefits();
                                onSubmitReturnValue = edViewModel.onSubmit();
                            });
                            it("sets modal options to showWarningForBenefitsOnly", function () {
                                expect(edViewModel.Scope.Modal()).toNotBe(null);
                            });
                            it("Does not change loan amount", function () {
                                expect(edViewModel.Loan.LoanAmount()).toBe(originalLoanAmount);
                            });
                            it("Prevents form submission", function () {
                                expect(onSubmitReturnValue).toBe(false);
                            });
                        });
                    });
                    describe("With combination of benefit incomes and other incomes", function () {
                        describe("When user submits form", function () {
                            beforeEach(function () {
                                 edViewModel = new boomer.EmploymentDetailsViewModel(null, generateLoanAmount(), generateLoanTerm(),null, null);
                                originalLoanAmount = edViewModel.Loan.LoanAmount();
                                addBenefits();
                                addOther();
                                onSubmitReturnValue = edViewModel.onSubmit();
                            });
                            it("Does not call showWarningForBenefitsOnly", function () {
                                expect(edViewModel.Scope.Modal()).toNotBe(edViewModel.Modals.showWarningForBenefitsOnly);
                            });
                            it("Does not change loan amount", function () {
                                expect(edViewModel.Loan.LoanAmount()).toBe(originalLoanAmount);
                            });
                            it("Prevents form submission", function () {
                                expect(onSubmitReturnValue).toBe(false);
                            });
                        });
                    });
                    describe("With only other incomes", function () {
                        describe("When user submits form", function () {
                            beforeEach(function () {
                                 edViewModel = new boomer.EmploymentDetailsViewModel(null, generateLoanAmount(), generateLoanTerm(),null, null);
                                originalLoanAmount = edViewModel.Loan.LoanAmount();
                                addOther();
                                onSubmitReturnValue = edViewModel.onSubmit();
                            });
                            it("Does not call showWarningForBenefitsOnly", function () {
                                expect(edViewModel.Scope.Modal()).toNotBe(edViewModel.Modals.showWarningForBenefitsOnly);
                            });
                            it("Does not change loan amount", function () {
                                expect(edViewModel.Loan.LoanAmount()).toBe(originalLoanAmount);
                            });
                            it("Prevents form submission", function () {
                                expect(onSubmitReturnValue).toBe(false);
                            });
                        });
                    });
                });
                describe("With loan type of 'Personal Loan' and loan amount of $600", function () {
                    beforeEach(function () {
                        generateLoanAmount = function () {
                            return 600;
                        };
                        generateLoanTerm = function () {
                            return 52;
                        };
                    });
                    describe("With a single benefits income", function () {
                        describe("When user submits form", function () {
                            beforeEach(function () {
                                
                                
                                 edViewModel = new boomer.EmploymentDetailsViewModel(null, generateLoanAmount(), generateLoanTerm(),null, null);
                                originalLoanAmount = edViewModel.Loan.LoanAmount();
                                addBenefits();
                                onSubmitReturnValue = edViewModel.onSubmit();
                            });
                            it("Does not call showWarningForBenefitsOnly", function () {
                                expect(edViewModel.Scope.Modal()).toNotBe(edViewModel.Modals.showWarningForBenefitsOnly);
                            });
                            it("Does not change loan amount", function () {
                                expect(edViewModel.Loan.LoanAmount()).toBe(originalLoanAmount);
                            });
                            it("Prevents form submission", function () {
                                expect(onSubmitReturnValue).toBe(false);
                            });
                        });
                    });
                    describe("With multiple benefits incomes", function () {
                        describe("When user submits form", function () {
                            beforeEach(function () {
                                
                                 edViewModel = new boomer.EmploymentDetailsViewModel(null, generateLoanAmount(), generateLoanTerm(),null, null);
                                originalLoanAmount = edViewModel.Loan.LoanAmount();
                                addBenefits();
                                addBenefits();
                                addBenefits();
                                edViewModel.onSubmit();
                            });
                            it("Does not call showWarningForBenefitsOnly", function () {
                                expect(edViewModel.Scope.Modal()).toNotBe(edViewModel.Modals.showWarningForBenefitsOnly);
                            });
                            it("Does not change loan amount", function () {
                                expect(edViewModel.Loan.LoanAmount()).toBe(originalLoanAmount);
                            });
                            it("Prevents form submission", function () {
                                expect(onSubmitReturnValue).toBe(false);
                            });
                        });
                    });
                    describe("With a combination of benefits incomes and other incomes", function () {
                        describe("When user submits form", function () {
                            beforeEach(function () {  
                                edViewModel = new boomer.EmploymentDetailsViewModel(null, generateLoanAmount(), generateLoanTerm(),null, null);
                                originalLoanAmount = edViewModel.Loan.LoanAmount();
                                addBenefits();
                                addOther();
                                onSubmitReturnValue = edViewModel.onSubmit();
                            });
                            it("Does not call showWarningForBenefitsOnly", function () {
                                expect(edViewModel.Scope.Modal()).toNotBe(edViewModel.Modals.showWarningForBenefitsOnly);
                            });
                            it("Does not change loan amount", function () {
                                expect(edViewModel.Loan.LoanAmount()).toBe(originalLoanAmount);
                            });
                            it("Prevents form submission", function () {
                                expect(onSubmitReturnValue).toBe(false);
                            });
                        });
                    });
                    describe("With only other incomes", function () {
                        describe("When user submits form", function () {
                            beforeEach(function () {
                                
                                
                                 edViewModel = new boomer.EmploymentDetailsViewModel(null, generateLoanAmount(), generateLoanTerm(),null, null);
                                originalLoanAmount = edViewModel.Loan.LoanAmount();
                                addOther();
                                onSubmitReturnValue = edViewModel.onSubmit(); 
                            });
                            it("Does not call showWarningForBenefitsOnly", function () {
                                expect(edViewModel.Scope.Modal()).toNotBe(edViewModel.Modals.showWarningForBenefitsOnly);
                            });
                            it("Does not change loan amount", function () {
                                expect(edViewModel.Loan.LoanAmount()).toBe(originalLoanAmount);
                            });
                            it("Prevents form submission", function () {
                                expect(onSubmitReturnValue).toBe(false);
                            });
                        });
                    });
                });
                describe("With loan type of 'Cash Advance' and loan amount over $200", function () {
                    beforeEach(function () {
                        generateLoanAmount = function () {
                            return getRandomInt(200, 2000);
                        };
                        generateLoanTerm = function () {
                            return 6;
                        };
                    });
                    describe("With a single benefits income", function () {
                        describe("When user submits form", function () {
                            beforeEach(function () {
                                edViewModel = new boomer.EmploymentDetailsViewModel(null, generateLoanAmount(), generateLoanTerm(),null, null);
                                originalLoanAmount = edViewModel.Loan.LoanAmount();
                                addBenefits();
                                onSubmitReturnValue = edViewModel.onSubmit();
                                
                            });
                            it("should display showWarningForBenefitsOnlyCashAdvance", function () {
                                expect(edViewModel.Scope.Modal()).toNotMatch(1, 2, 3);
                            });
                            it("Sets the loan amount to $200", function () {
                                onSubmitReturnValue = edViewModel.setToCashAdvance();
                                expect(edViewModel.Loan.LoanAmount()).toBe(200);
                            });
                            it("Prevents form submission", function () {
  
                                expect(onSubmitReturnValue).toBe(false);
                            });
                        });
                    });
                    describe("With multiple benefits incomes", function () {
                        describe("When user submits form", function () {
                            beforeEach(function () {
                                edViewModel = new boomer.EmploymentDetailsViewModel(null, generateLoanAmount(), generateLoanTerm(), null, null);
                                originalLoanAmount = edViewModel.Loan.LoanAmount();
                                addBenefits();
                                addBenefits();
                                addBenefits();
                                onSubmitReturnValue = edViewModel.onSubmit();
                            });
                            it("Does not call showWarningForBenefitsOnly", function () {
                                expect(edViewModel.Scope.Modal()).toNotBe(edViewModel.Modals.showWarningForBenefitsOnly);
                            });
                            it("sets modal options toshowWarningForBenefitsOnlyCashAdvance", function () {
                                expect(edViewModel.Scope.Modal()).toBe(edViewModel.Modals.showWarningForBenefitsOnlyCashAdvance);
                            });
                            it("Prevents form submission", function () {
                                expect(onSubmitReturnValue).toBe(false);
                            });
                        });
                    });
                    describe("With multiple benefits incomes", function () {
                        describe("When user submits form", function () {
                            beforeEach(function () {
                                edViewModel = new boomer.EmploymentDetailsViewModel(null, generateLoanAmount(), generateLoanTerm(), null, null);
                                originalLoanAmount = edViewModel.Loan.LoanAmount();
                                addBenefits();
                                addBenefits();
                                addBenefits();
                                onSubmitReturnValue = edViewModel.onSubmit();
                                edViewModel.setToCashAdvance();
                            });
                            it("Sets the loan amount to $200", function () {
                                expect(edViewModel.Loan.LoanAmount()).toBe(200);
                            });
                        });
                    });
                    describe("With a combination of benefits incomes and other incomes", function () {
                        describe("When user submits form", function () {
                            beforeEach(function () {
                                edViewModel = new boomer.EmploymentDetailsViewModel(null, generateLoanAmount(), generateLoanTerm(), null, null);
                                
                                originalLoanAmount = edViewModel.Loan.LoanAmount();
                                addBenefits();
                                addOther();
                                onSubmitReturnValue = edViewModel.onSubmit();
                            });
                            it("Does not call showWarningForBenefitsOnly", function() {
                                expect(edViewModel.Scope.Modal()).toBeNull();
                            });
                            it("Does not change loan amount", function () {
                                expect(edViewModel.Loan.LoanAmount()).toBe(originalLoanAmount);
                            });
                            it("Allows form submission", function () {
                                expect(onSubmitReturnValue).toBe(true);
                            });
                        });
                    });
                    describe("With only other incomes", function () {
                        describe("When user submits form", function () {
                            beforeEach(function () {
                                edViewModel = new boomer.EmploymentDetailsViewModel(null, generateLoanAmount(), generateLoanTerm(), null, null);
                                originalLoanAmount = edViewModel.Loan.LoanAmount();
                                addOther();
                                onSubmitReturnValue = edViewModel.onSubmit();
                            });
                            it("Does not call showWarningForBenefitsOnly", function () {
                                expect(edViewModel.Scope.Modal()).toBeNull();
                            });
                            it("Does not change loan amount", function () {
                                expect(edViewModel.Loan.LoanAmount()).toBe(originalLoanAmount);
                            });
                            it("Allows form submission", function () {
                                expect(onSubmitReturnValue).toBe(true);
                            });
                        });
                    });
                });
            });

            describe("Handled total income correctly", function () {
                var pickRandom = function (list) {
                    return list[Math.floor(Math.random() * list.length)];
                };
                beforeEach(function () {
                    edViewModel = new boomer.EmploymentDetailsViewModel(null, getRandomInt(100,2000), 52);
                });
                describe("With single weekly income source", function () {
                    beforeEach(function () {
                        edViewModel.Customer.IncomeSource('Benefits');
                        edViewModel.addEmployment();
                        var benefits = edViewModel.Customer.EmploymentHistory().pop();
                        benefits.EmployerName(pickRandom(benefits.CentrelinkBenefitTypes));
                        benefits.StartDateMonth(pickRandom(benefits.Months));
                        benefits.StartDateYear(pickRandom(benefits.Years));
                        benefits.PayFrequency("Weekly");
                        benefits.NetIncome(100);//Weekly income
                        edViewModel.Customer.EmploymentHistory.push(benefits);
                    });
                    it("has total income set to that income", function () {
                        expect(edViewModel.Customer.TotalWeeklyIncome()).toBe(100);
                    });
                    describe("With additional income source", function () {
                        beforeEach(function () {
                            edViewModel.Customer.IncomeSource('Benefits');
                            edViewModel.addEmployment();
                            var benefits = edViewModel.Customer.EmploymentHistory().pop();
                            benefits.EmployerName(pickRandom(benefits.CentrelinkBenefitTypes));
                            benefits.StartDateMonth(pickRandom(benefits.Months));
                            benefits.StartDateYear(pickRandom(benefits.Years));
                            benefits.PayFrequency("Weekly");
                            benefits.NetIncome(100);//Weekly income
                            edViewModel.Customer.EmploymentHistory.push(benefits);
                        });
                        it("has total income set correctly", function () {
                            expect(edViewModel.Customer.TotalWeeklyIncome()).toBe(200);
                        });
                    });
                });
                describe("With single monthly income source", function () {
                    beforeEach(function () {
                        edViewModel.Customer.IncomeSource('Benefits');
                        edViewModel.addEmployment();
                        var benefits = edViewModel.Customer.EmploymentHistory().pop();
                        benefits.EmployerName(pickRandom(benefits.CentrelinkBenefitTypes));
                        benefits.StartDateMonth(pickRandom(benefits.Months));
                        benefits.StartDateYear(pickRandom(benefits.Years));
                        benefits.PayFrequency("Monthly");
                        benefits.NetIncome(390);//Monthly income
                        edViewModel.Customer.EmploymentHistory.push(benefits);
                    });
                    it("has total income set to weekly equivilent of monthly income", function () {
                        expect(edViewModel.Customer.TotalWeeklyIncome()).toBe(90);//390 * 12 / 52
                    });
                    describe("With single monthly income source", function () {
                        beforeEach(function () {
                            edViewModel.Customer.IncomeSource('Benefits');
                            edViewModel.addEmployment();
                            var benefits = edViewModel.Customer.EmploymentHistory().pop();
                            benefits.EmployerName(pickRandom(benefits.CentrelinkBenefitTypes));
                            benefits.StartDateMonth(pickRandom(benefits.Months));
                            benefits.StartDateYear(pickRandom(benefits.Years));
                            benefits.PayFrequency("Monthly");
                            benefits.NetIncome(390);//Monthly income
                            edViewModel.Customer.EmploymentHistory.push(benefits);
                        });
                        it("has total income set correctly", function () {
                            expect(edViewModel.Customer.TotalWeeklyIncome()).toBe(180);
                        });
                    });
                });
            });
        });
        describe("Emploment type specific view models", function () {
            describe("EmployedViewModel", function () {
                var allMonths = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

                describe("months should depend on current date", function () {
                    var givenMonthIndex, monthIndex, missingMonthIndex, momentToUse, employedVm, months,
                        baseMonth = moment("1-1-2000", "DD-MM-YYYY");
                    it("Should only contain previous for the current year", function () {
                        for (givenMonthIndex = 0; givenMonthIndex < 12; givenMonthIndex++) {
                            momentToUse = moment(baseMonth).add('month', givenMonthIndex);
                            employedVm = new boomer.EmployedViewModel(null, momentToUse);
                            employedVm.StartDateYear(baseMonth.years());//set the current year on the model
                            months = $.map(employedVm.Months(), getItemDisplay);
                            for (monthIndex = 0; monthIndex <= givenMonthIndex; monthIndex++) {
                                expect(months).toContain(allMonths[monthIndex]);
                            }
                            for (missingMonthIndex = givenMonthIndex + 1; missingMonthIndex < 12; missingMonthIndex++) {
                                expect(months).toNotContain(allMonths[missingMonthIndex]);
                            }
                        }
                    });
                });
                describe("Defaults on model", function () {
                    var employedVm;
                    beforeEach(function () {
                        employedVm = new boomer.EmployedViewModel(null, moment());
                    });
                    it("Has model with template of 'Employed' as first record of EmploymentHistory", function () {
                        expect(employedVm.TemplateName()).toBe("Employed");
                    });
                    it("has all pay frequencies", function () {
                        expect(employedVm.PayFrequencies.length).toBe(3);
                        expect(employedVm.PayFrequencies).toContain("Weekly");
                        expect(employedVm.PayFrequencies).toContain("Fortnightly");
                        expect(employedVm.PayFrequencies).toContain("Monthly");
                    });
                    describe("Start date", function () {
                        it("Has this no year specified by default", function () {
                            expect(employedVm.StartDateYear()).toBeFalsy();
                        });

                        it("has last 10 years", function () {
                            var currentYear = moment().years(),
                                years = $.map(employedVm.Years, function (item) { return item.Value; });
                            expect(years).toContain(currentYear);
                            expect(years).toContain(currentYear - 1);
                            expect(years).toContain(currentYear - 2);
                            expect(years).toContain(currentYear - 3);
                            expect(years).toContain(currentYear - 4);
                            expect(years).toContain(currentYear - 5);
                            expect(years).toContain(currentYear - 6);
                            expect(years).toContain(currentYear - 7);
                            expect(years).toContain(currentYear - 8);
                            expect(years).toContain(currentYear - 9);
                        });
                        it("has some months", function () {
                            expect(employedVm.Months()).toBeDefined();
                        });
                    });
                });

                describe("existing model", function () {
                    var json = {
                        Id: 4235,
                        EmployerName: 'TrucksRUs',
                        SuburbOrBranch: 'Guilford',
                        ContactPhoneNumber: '07456456456',
                        StartDate: '01/08/2010',
                        PayFrequency: 'Fortnightly',
                        NetIncome: 23000,
                        IsRemoved: false
                    };
                    it("sets the existing values as observables", function () {
                        var employedVm = new boomer.EmployedViewModel(null, moment(), json);
                        expect(employedVm.EmployerName()).toBe(json.EmployerName);
                        expect(employedVm.SuburbOrBranch()).toBe(json.SuburbOrBranch);
                        expect(employedVm.ContactPhoneNumber()).toBe(json.ContactPhoneNumber);
                        expect(employedVm.StartDate()).toBe(json.StartDate);
                        expect(employedVm.PayFrequency()).toBe(json.PayFrequency);
                        expect(employedVm.NetIncome()).toBe(json.NetIncome);
                        expect(employedVm.Id()).toBe(json.Id);
                        expect(employedVm.IsRemoved()).toBe(json.IsRemoved);//for when model is in invalid state on postback
                    });
                });

            });

            describe("SelfEmployedViewModel", function () {
                var allMonths = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

                describe("months should depend on current date", function () {
                    var givenMonthIndex, monthIndex, missingMonthIndex, momentToUse, selfEmployedVm, months,
                        baseMonth = moment("1-1-2000", "DD-MM-YYYY");
                    it("Should only contain previous for the current year", function () {
                        for (givenMonthIndex = 0; givenMonthIndex < 12; givenMonthIndex++) {
                            momentToUse = moment(baseMonth).add('month', givenMonthIndex);
                            selfEmployedVm = new boomer.SelfEmployedViewModel(null, momentToUse);
                            selfEmployedVm.StartDateYear(baseMonth.years());//set the current year on the model
                            months = $.map(selfEmployedVm.Months(), getItemDisplay);

                            for (monthIndex = 0; monthIndex <= givenMonthIndex; monthIndex++) {
                                expect(months).toContain(allMonths[monthIndex]);
                            }
                            for (missingMonthIndex = givenMonthIndex + 1; missingMonthIndex < 12; missingMonthIndex++) {
                                expect(months).toNotContain(allMonths[missingMonthIndex]);
                            }
                        }
                    });
                });
                describe("Defaults on model", function () {
                    var selfEmployedVm;
                    beforeEach(function () {
                        selfEmployedVm = new boomer.SelfEmployedViewModel(null, moment());
                    });
                    it("Has model with template of 'Self-employed' as first record of EmploymentHistory", function () {
                        expect(selfEmployedVm.TemplateName()).toBe("Self-employed");
                    });
                    it("has all pay frequencies", function () {
                        expect(selfEmployedVm.PayFrequencies.length).toBe(3);
                        expect(selfEmployedVm.PayFrequencies).toContain("Weekly");
                        expect(selfEmployedVm.PayFrequencies).toContain("Fortnightly");
                        expect(selfEmployedVm.PayFrequencies).toContain("Monthly");
                    });
                    describe("Start date", function () {
                        it("Has this no year specified by default", function () {
                            expect(selfEmployedVm.StartDateYear()).toBeFalsy();
                        });

                        it("has last 10 years", function () {
                            var currentYear = moment().years(),
                                years = $.map(selfEmployedVm.Years, getItemValue);
                            expect(years).toContain(currentYear);
                            expect(years).toContain(currentYear - 1);
                            expect(years).toContain(currentYear - 2);
                            expect(years).toContain(currentYear - 3);
                            expect(years).toContain(currentYear - 4);
                            expect(years).toContain(currentYear - 5);
                            expect(years).toContain(currentYear - 6);
                            expect(years).toContain(currentYear - 7);
                            expect(years).toContain(currentYear - 8);
                            expect(years).toContain(currentYear - 9);
                        });
                        it("has some months", function () {
                            expect(selfEmployedVm.Months()).toBeDefined();
                        });
                    });
                });

                describe("existing model", function () {
                    var json = {
                        Id: 5465,
                        EmployerName: 'TrucksRUs',
                        SuburbOrBranch: 'Guilford',
                        ContactPhoneNumber: '07456456456',
                        StartDate: '01/05/2010',
                        PayFrequency: 'Fortnightly',
                        NetIncome: 23000,
                        IsRemoved: false
                    };
                    it("sets the existing values as observables", function () {
                        var selfEmployedVm = new boomer.SelfEmployedViewModel(null, moment(), json);
                        expect(selfEmployedVm.EmployerName()).toBe(json.EmployerName);
                        expect(selfEmployedVm.ContactPhoneNumber()).toBe(json.ContactPhoneNumber);
                        expect(selfEmployedVm.StartDate()).toBe(json.StartDate);
                        expect(selfEmployedVm.PayFrequency()).toBe(json.PayFrequency);
                        expect(selfEmployedVm.NetIncome()).toBe(json.NetIncome);
                        expect(selfEmployedVm.Id()).toBe(json.Id);
                        expect(selfEmployedVm.IsRemoved()).toBe(json.IsRemoved);
                    });
                    it("does not have unnessecary observables", function () {
                        var selfEmployedVm = new boomer.SelfEmployedViewModel(null, moment(), json);
                        expect(selfEmployedVm.SuburbOrBranch).toBeUndefined();//Not on self employed
                    });
                });

            });

            describe("BenefitsViewModel", function () {
                var allMonths = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

                describe("months should depend on current date", function () {
                    var baseMonth = moment("1-1-2000", "DD-MM-YYYY");
                    it("Should only contain previous for the current year", function () {
                        var givenMonthIndex, monthIndex, missingMonthIndex, momentToUse, benefitsVm, months;
                        for (givenMonthIndex = 0; givenMonthIndex < 12; givenMonthIndex++) {
                            momentToUse = moment(baseMonth).add('month', givenMonthIndex);
                            benefitsVm = new boomer.CentrelinkBenefitsViewModel(null, momentToUse);
                            benefitsVm.StartDateYear(baseMonth.years());//set the current year on the model
                            months = $.map(benefitsVm.Months(), getItemDisplay);

                            for (monthIndex = 0; monthIndex <= givenMonthIndex; monthIndex++) {
                                expect(months).toContain(allMonths[monthIndex]);
                            }
                            for (missingMonthIndex = givenMonthIndex + 1; missingMonthIndex < 12; missingMonthIndex++) {
                                expect(months).toNotContain(allMonths[missingMonthIndex]);
                            }
                        }
                    });
                });
                describe("Defaults on model", function () {
                    var benefitsVm;
                    beforeEach(function () {
                        benefitsVm = new boomer.CentrelinkBenefitsViewModel(null, moment());
                    });
                    it("Has model with template of 'Benefits' as first record of EmploymentHistory", function () {
                        expect(benefitsVm.TemplateName()).toBe("Benefits");
                    });
                    it("Has all centrelink benefits", function () {
                        expect(benefitsVm.CentrelinkBenefitTypes.length).toBe(8);
                        expect(benefitsVm.CentrelinkBenefitTypes).toContain("Centrelink Pension");
                        expect(benefitsVm.CentrelinkBenefitTypes).toContain("Centrelink Family Tax A&B");
                        expect(benefitsVm.CentrelinkBenefitTypes).toContain("Centrelink Newstart");
                        expect(benefitsVm.CentrelinkBenefitTypes).toContain("Centrelink Aus Study");
                        expect(benefitsVm.CentrelinkBenefitTypes).toContain("Centrelink Ab Study");
                        expect(benefitsVm.CentrelinkBenefitTypes).toContain("Centrelink Youth Allowance");
                        expect(benefitsVm.CentrelinkBenefitTypes).toContain("Centrelink Parenting Payment");
                        expect(benefitsVm.CentrelinkBenefitTypes).toContain("Centrelink Carer Payment");
                    });
                    it("has all pay frequencies", function () {
                        expect(benefitsVm.PayFrequencies.length).toBe(3);
                        expect(benefitsVm.PayFrequencies).toContain("Weekly");
                        expect(benefitsVm.PayFrequencies).toContain("Fortnightly");
                        expect(benefitsVm.PayFrequencies).toContain("Monthly");
                    });
                    describe("Start date", function () {
                        it("Has this no year specified by default", function () {
                            expect(benefitsVm.StartDateYear()).toBeFalsy();
                        });

                        it("has last 10 years", function () {
                            var currentYear = moment().years(),
                                years = $.map(benefitsVm.Years, getItemValue);
                            expect(years).toContain(currentYear);
                            expect(years).toContain(currentYear - 1);
                            expect(years).toContain(currentYear - 2);
                            expect(years).toContain(currentYear - 3);
                            expect(years).toContain(currentYear - 4);
                            expect(years).toContain(currentYear - 5);
                            expect(years).toContain(currentYear - 6);
                            expect(years).toContain(currentYear - 7);
                            expect(years).toContain(currentYear - 8);
                            expect(years).toContain(currentYear - 9);
                        });
                        it("has some months", function () {
                            expect(benefitsVm.Months()).toBeDefined();
                        });
                    });
                });

                describe("existing model", function () {
                    var json = {
                        Id: 9885,
                        EmployerName: 'TrucksRUs',
                        SuburbOrBranch: 'Guilford',
                        ContactPhoneNumber: '07456456456',
                        StartDate: '01/03/2010',
                        PayFrequency: 'Fortnightly',
                        NetIncome: 23000,
                        IsRemoved: false
                    };
                    it("sets the existing values as observables", function () {
                        var benefitsVm = new boomer.CentrelinkBenefitsViewModel(null, moment(), json);
                        expect(benefitsVm.EmployerName()).toBe(json.EmployerName);
                        expect(benefitsVm.StartDate()).toBe(json.StartDate);
                        expect(benefitsVm.PayFrequency()).toBe(json.PayFrequency);
                        expect(benefitsVm.NetIncome()).toBe(json.NetIncome);
                        expect(benefitsVm.Id()).toBe(json.Id);
                        expect(benefitsVm.IsRemoved()).toBe(json.IsRemoved);
                    });
                    it("does not have unnessecary observables", function () {
                        var benefitsVm = new boomer.CentrelinkBenefitsViewModel(null, moment(), json);
                        //Not on Benefits
                        expect(benefitsVm.SuburbOrBranch).toBeUndefined();
                        expect(benefitsVm.ContactPhoneNumber).toBeUndefined();
                    });
                });

            });

            describe("OtherViewModel", function () {
                var allMonths = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

                describe("months should depend on current date", function () {
                    var baseMonth = moment("1-1-2000", "DD-MM-YYYY");
                    it("Should only contain previous for the current year", function () {
                        var givenMonthIndex, monthIndex, missingMonthIndex, momentToUse, otherVm, months;
                        for (givenMonthIndex = 0; givenMonthIndex < 12; givenMonthIndex++) {
                            momentToUse = moment(baseMonth).add('month', givenMonthIndex);
                            otherVm = new boomer.OtherViewModel(null, momentToUse);
                            otherVm.StartDateYear(baseMonth.years());//set the current year on the model
                            months = $.map(otherVm.Months(), getItemDisplay);

                            for (monthIndex = 0; monthIndex <= givenMonthIndex; monthIndex++) {
                                expect(months).toContain(allMonths[monthIndex]);
                            }
                            for (missingMonthIndex = givenMonthIndex + 1; missingMonthIndex < 12; missingMonthIndex++) {
                                expect(months).toNotContain(allMonths[missingMonthIndex]);
                            }
                        }
                    });
                });
                describe("Defaults on model", function () {
                    var otherVm;
                    beforeEach(function () {
                        otherVm = new boomer.OtherViewModel(null, moment());
                    });
                    it("Has model with template of 'Other' as first record of EmploymentHistory", function () {
                        expect(otherVm.TemplateName()).toBe("Other");
                    });
                    it("has all pay frequencies", function () {
                        expect(otherVm.PayFrequencies.length).toBe(3);
                        expect(otherVm.PayFrequencies).toContain("Weekly");
                        expect(otherVm.PayFrequencies).toContain("Fortnightly");
                        expect(otherVm.PayFrequencies).toContain("Monthly");
                    });
                    describe("Start date", function () {
                        it("Has this no year specified by default", function () {
                            expect(otherVm.StartDateYear()).toBeFalsy();
                        });

                        it("has last 10 years", function () {
                            var currentYear = moment().years(),
                                years = $.map(otherVm.Years, getItemValue);
                            expect(years).toContain(currentYear);
                            expect(years).toContain(currentYear - 1);
                            expect(years).toContain(currentYear - 2);
                            expect(years).toContain(currentYear - 3);
                            expect(years).toContain(currentYear - 4);
                            expect(years).toContain(currentYear - 5);
                            expect(years).toContain(currentYear - 6);
                            expect(years).toContain(currentYear - 7);
                            expect(years).toContain(currentYear - 8);
                            expect(years).toContain(currentYear - 9);
                        });
                        it("has some months", function () {
                            expect(otherVm.Months()).toBeDefined();
                        });
                    });
                });

                describe("existing model", function () {
                    var json = {
                        Id: 7843,
                        EmployerName: 'Government',
                        SuburbOrBranch: 'Guilford',
                        ContactPhoneNumber: '07456456456',
                        StartDate: '01/06/2010',
                        PayFrequency: 'Fortnightly',
                        NetIncome: 23000,
                        IsRemoved: false
                    };
                    it("sets the existing values as observables", function () {
                        var otherVm = new boomer.OtherViewModel(null, moment(), json);
                        expect(otherVm.EmployerName()).toBe(json.EmployerName);
                        expect(otherVm.StartDate()).toBe(json.StartDate);
                        expect(otherVm.PayFrequency()).toBe(json.PayFrequency);
                        expect(otherVm.NetIncome()).toBe(json.NetIncome);
                        expect(otherVm.Id()).toBe(json.Id);
                        expect(otherVm.IsRemoved()).toBe(json.IsRemoved);
                    });
                    it("does not have unnessecary observables", function () {
                        var otherVm = new boomer.OtherViewModel(null, moment(), json);
                        //Not on Benefits
                        expect(otherVm.SuburbOrBranch).toBeUndefined();
                        expect(otherVm.ContactPhoneNumber).toBeUndefined();
                    });
                });
            });
        });
    });
}(boomer, ko, jQuery, moment));


function getRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}
