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
/// <reference path="~/Scripts/ViewModels/LoanDetailsViewModel.js"/>

(function (boomer, $) {
    "use strict";
    var model,
        getExpectations = function () {
            var result,
                selectedLoanAmount = model.SelectedLoanAmount(),
                loanDetails = {
                    "6": [
                        { LoanAmount: 100, EstablishmentFee: 20, TotalLoanAmount: 120, NumberOfRepayments: 6, RepaymentFrequency: "Weekly", RepaymentAmounts: 21.33, MonthlyFee: 4, TotalMonthlyFees: 8, CostOfLoan: 28, TotalAmountToRepay: 128, LoanTermDescription: "6 weeks", ComparisonRate: { LoanAmount: 250, LoanTermDescription: "2 weeks", NumberOfRepayments: 2, TotalFeesPercentage: 0.24, TotalFeesDollars: 60, ComparisonRatePercentage: 815.19 } },
                        { LoanAmount: 200, EstablishmentFee: 40, TotalLoanAmount: 240, NumberOfRepayments: 6, RepaymentFrequency: "Weekly", RepaymentAmounts: 42.67, MonthlyFee: 8, TotalMonthlyFees: 16, CostOfLoan: 56, TotalAmountToRepay: 256, LoanTermDescription: "6 weeks", ComparisonRate: { LoanAmount: 250, LoanTermDescription: "2 weeks", NumberOfRepayments: 2, TotalFeesPercentage: 0.24, TotalFeesDollars: 60, ComparisonRatePercentage: 815.19 } },
                        { LoanAmount: 300, EstablishmentFee: 60, TotalLoanAmount: 360, NumberOfRepayments: 6, RepaymentFrequency: "Weekly", RepaymentAmounts: 64, MonthlyFee: 12, TotalMonthlyFees: 24, CostOfLoan: 84, TotalAmountToRepay: 384, LoanTermDescription: "6 weeks", ComparisonRate: { LoanAmount: 250, LoanTermDescription: "2 weeks", NumberOfRepayments: 2, TotalFeesPercentage: 0.24, TotalFeesDollars: 60, ComparisonRatePercentage: 815.19 } },
                        { LoanAmount: 400, EstablishmentFee: 80, TotalLoanAmount: 480, NumberOfRepayments: 6, RepaymentFrequency: "Weekly", RepaymentAmounts: 85.33, MonthlyFee: 16, TotalMonthlyFees: 32, CostOfLoan: 112, TotalAmountToRepay: 512, LoanTermDescription: "6 weeks", ComparisonRate: { LoanAmount: 250, LoanTermDescription: "2 weeks", NumberOfRepayments: 2, TotalFeesPercentage: 0.24, TotalFeesDollars: 60, ComparisonRatePercentage: 815.19 } },
                        { LoanAmount: 500, EstablishmentFee: 100, TotalLoanAmount: 600, NumberOfRepayments: 6, RepaymentFrequency: "Weekly", RepaymentAmounts: 106.67, MonthlyFee: 20, TotalMonthlyFees: 40, CostOfLoan: 140, TotalAmountToRepay: 640, LoanTermDescription: "6 weeks", ComparisonRate: { LoanAmount: 600, LoanTermDescription: "6 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.44, TotalFeesDollars: 264, ComparisonRatePercentage: 152.04 } },
                        { LoanAmount: 600, EstablishmentFee: 120, TotalLoanAmount: 720, NumberOfRepayments: 6, RepaymentFrequency: "Weekly", RepaymentAmounts: 128, MonthlyFee: 24, TotalMonthlyFees: 48, CostOfLoan: 168, TotalAmountToRepay: 768, LoanTermDescription: "6 weeks", ComparisonRate: { LoanAmount: 600, LoanTermDescription: "6 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.44, TotalFeesDollars: 264, ComparisonRatePercentage: 152.04 } },
                        { LoanAmount: 700, EstablishmentFee: 140, TotalLoanAmount: 840, NumberOfRepayments: 6, RepaymentFrequency: "Weekly", RepaymentAmounts: 149.33, MonthlyFee: 28, TotalMonthlyFees: 56, CostOfLoan: 196, TotalAmountToRepay: 896, LoanTermDescription: "6 weeks", ComparisonRate: { LoanAmount: 600, LoanTermDescription: "6 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.44, TotalFeesDollars: 264, ComparisonRatePercentage: 152.04 } },
                        { LoanAmount: 800, EstablishmentFee: 160, TotalLoanAmount: 960, NumberOfRepayments: 6, RepaymentFrequency: "Weekly", RepaymentAmounts: 170.67, MonthlyFee: 32, TotalMonthlyFees: 64, CostOfLoan: 224, TotalAmountToRepay: 1024, LoanTermDescription: "6 weeks", ComparisonRate: { LoanAmount: 1000, LoanTermDescription: "6 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.44, TotalFeesDollars: 440, ComparisonRatePercentage: 152.03 } },
                        { LoanAmount: 900, EstablishmentFee: 180, TotalLoanAmount: 1080, NumberOfRepayments: 6, RepaymentFrequency: "Weekly", RepaymentAmounts: 192, MonthlyFee: 36, TotalMonthlyFees: 72, CostOfLoan: 252, TotalAmountToRepay: 1152, LoanTermDescription: "6 weeks", ComparisonRate: { LoanAmount: 1000, LoanTermDescription: "6 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.44, TotalFeesDollars: 440, ComparisonRatePercentage: 152.03 } },
                        { LoanAmount: 1000, EstablishmentFee: 200, TotalLoanAmount: 1200, NumberOfRepayments: 6, RepaymentFrequency: "Weekly", RepaymentAmounts: 213.33, MonthlyFee: 40, TotalMonthlyFees: 80, CostOfLoan: 280, TotalAmountToRepay: 1280, LoanTermDescription: "6 weeks", ComparisonRate: { LoanAmount: 1000, LoanTermDescription: "6 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.44, TotalFeesDollars: 440, ComparisonRatePercentage: 152.03 } },
                        { LoanAmount: 1100, EstablishmentFee: 220, TotalLoanAmount: 1320, NumberOfRepayments: 6, RepaymentFrequency: "Weekly", RepaymentAmounts: 234.67, MonthlyFee: 44, TotalMonthlyFees: 88, CostOfLoan: 308, TotalAmountToRepay: 1408, LoanTermDescription: "6 weeks", ComparisonRate: { LoanAmount: 1000, LoanTermDescription: "6 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.44, TotalFeesDollars: 440, ComparisonRatePercentage: 152.03 } },
                        { LoanAmount: 1200, EstablishmentFee: 240, TotalLoanAmount: 1440, NumberOfRepayments: 6, RepaymentFrequency: "Weekly", RepaymentAmounts: 256, MonthlyFee: 48, TotalMonthlyFees: 96, CostOfLoan: 336, TotalAmountToRepay: 1536, LoanTermDescription: "6 weeks", ComparisonRate: { LoanAmount: 1000, LoanTermDescription: "6 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.44, TotalFeesDollars: 440, ComparisonRatePercentage: 152.03 } },
                        { LoanAmount: 1300, EstablishmentFee: 260, TotalLoanAmount: 1560, NumberOfRepayments: 6, RepaymentFrequency: "Weekly", RepaymentAmounts: 277.33, MonthlyFee: 52, TotalMonthlyFees: 104, CostOfLoan: 364, TotalAmountToRepay: 1664, LoanTermDescription: "6 weeks", ComparisonRate: { LoanAmount: 1000, LoanTermDescription: "6 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.44, TotalFeesDollars: 440, ComparisonRatePercentage: 152.03 } },
                        { LoanAmount: 1400, EstablishmentFee: 280, TotalLoanAmount: 1680, NumberOfRepayments: 6, RepaymentFrequency: "Weekly", RepaymentAmounts: 298.67, MonthlyFee: 56, TotalMonthlyFees: 112, CostOfLoan: 392, TotalAmountToRepay: 1792, LoanTermDescription: "6 weeks", ComparisonRate: { LoanAmount: 1000, LoanTermDescription: "6 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.44, TotalFeesDollars: 440, ComparisonRatePercentage: 152.03 } },
                        { LoanAmount: 1500, EstablishmentFee: 300, TotalLoanAmount: 1800, NumberOfRepayments: 6, RepaymentFrequency: "Weekly", RepaymentAmounts: 320, MonthlyFee: 60, TotalMonthlyFees: 120, CostOfLoan: 420, TotalAmountToRepay: 1920, LoanTermDescription: "6 weeks", ComparisonRate: { LoanAmount: 1000, LoanTermDescription: "6 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.44, TotalFeesDollars: 440, ComparisonRatePercentage: 152.03 } },
                        { LoanAmount: 1600, EstablishmentFee: 320, TotalLoanAmount: 1920, NumberOfRepayments: 6, RepaymentFrequency: "Weekly", RepaymentAmounts: 341.33, MonthlyFee: 64, TotalMonthlyFees: 128, CostOfLoan: 448, TotalAmountToRepay: 2048, LoanTermDescription: "6 weeks", ComparisonRate: { LoanAmount: 2000, LoanTermDescription: "12 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.68, TotalFeesDollars: 1360, ComparisonRatePercentage: 153.8 } },
                        { LoanAmount: 1700, EstablishmentFee: 340, TotalLoanAmount: 2040, NumberOfRepayments: 6, RepaymentFrequency: "Weekly", RepaymentAmounts: 362.67, MonthlyFee: 68, TotalMonthlyFees: 136, CostOfLoan: 476, TotalAmountToRepay: 2176, LoanTermDescription: "6 weeks", ComparisonRate: { LoanAmount: 2000, LoanTermDescription: "12 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.68, TotalFeesDollars: 1360, ComparisonRatePercentage: 153.8 } },
                        { LoanAmount: 1800, EstablishmentFee: 360, TotalLoanAmount: 2160, NumberOfRepayments: 6, RepaymentFrequency: "Weekly", RepaymentAmounts: 384, MonthlyFee: 72, TotalMonthlyFees: 144, CostOfLoan: 504, TotalAmountToRepay: 2304, LoanTermDescription: "6 weeks", ComparisonRate: { LoanAmount: 2000, LoanTermDescription: "12 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.68, TotalFeesDollars: 1360, ComparisonRatePercentage: 153.8 } },
                        { LoanAmount: 1900, EstablishmentFee: 380, TotalLoanAmount: 2280, NumberOfRepayments: 6, RepaymentFrequency: "Weekly", RepaymentAmounts: 405.33, MonthlyFee: 76, TotalMonthlyFees: 152, CostOfLoan: 532, TotalAmountToRepay: 2432, LoanTermDescription: "6 weeks", ComparisonRate: { LoanAmount: 2000, LoanTermDescription: "12 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.68, TotalFeesDollars: 1360, ComparisonRatePercentage: 153.8 } },
                        { LoanAmount: 2000, EstablishmentFee: 400, TotalLoanAmount: 2400, NumberOfRepayments: 6, RepaymentFrequency: "Weekly", RepaymentAmounts: 426.67, MonthlyFee: 80, TotalMonthlyFees: 160, CostOfLoan: 560, TotalAmountToRepay: 2560, LoanTermDescription: "6 weeks", ComparisonRate: { LoanAmount: 2000, LoanTermDescription: "12 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.68, TotalFeesDollars: 1360, ComparisonRatePercentage: 153.8 } }
                    ],
                    "52": [
                        { LoanAmount: 600, EstablishmentFee: 120, TotalLoanAmount: 720, NumberOfRepayments: 50, RepaymentFrequency: "Weekly", RepaymentAmounts: 20.16, MonthlyFee: 24, TotalMonthlyFees: 288, CostOfLoan: 408, TotalAmountToRepay: 1008, LoanTermDescription: "12 months", ComparisonRate: { LoanAmount: 600, LoanTermDescription: "6 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.44, TotalFeesDollars: 264, ComparisonRatePercentage: 152.04 } },
                        { LoanAmount: 700, EstablishmentFee: 140, TotalLoanAmount: 840, NumberOfRepayments: 50, RepaymentFrequency: "Weekly", RepaymentAmounts: 23.52, MonthlyFee: 28, TotalMonthlyFees: 336, CostOfLoan: 476, TotalAmountToRepay: 1176, LoanTermDescription: "12 months", ComparisonRate: { LoanAmount: 600, LoanTermDescription: "6 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.44, TotalFeesDollars: 264, ComparisonRatePercentage: 152.04 } },
                        { LoanAmount: 800, EstablishmentFee: 160, TotalLoanAmount: 960, NumberOfRepayments: 50, RepaymentFrequency: "Weekly", RepaymentAmounts: 26.88, MonthlyFee: 32, TotalMonthlyFees: 384, CostOfLoan: 544, TotalAmountToRepay: 1344, LoanTermDescription: "12 months", ComparisonRate: { LoanAmount: 1000, LoanTermDescription: "6 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.44, TotalFeesDollars: 440, ComparisonRatePercentage: 152.03 } },
                        { LoanAmount: 900, EstablishmentFee: 180, TotalLoanAmount: 1080, NumberOfRepayments: 50, RepaymentFrequency: "Weekly", RepaymentAmounts: 30.24, MonthlyFee: 36, TotalMonthlyFees: 432, CostOfLoan: 612, TotalAmountToRepay: 1512, LoanTermDescription: "12 months", ComparisonRate: { LoanAmount: 1000, LoanTermDescription: "6 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.44, TotalFeesDollars: 440, ComparisonRatePercentage: 152.03 } },
                        { LoanAmount: 1000, EstablishmentFee: 200, TotalLoanAmount: 1200, NumberOfRepayments: 50, RepaymentFrequency: "Weekly", RepaymentAmounts: 33.6, MonthlyFee: 40, TotalMonthlyFees: 480, CostOfLoan: 680, TotalAmountToRepay: 1680, LoanTermDescription: "12 months", ComparisonRate: { LoanAmount: 1000, LoanTermDescription: "6 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.44, TotalFeesDollars: 440, ComparisonRatePercentage: 152.03 } },
                        { LoanAmount: 1100, EstablishmentFee: 220, TotalLoanAmount: 1320, NumberOfRepayments: 50, RepaymentFrequency: "Weekly", RepaymentAmounts: 36.96, MonthlyFee: 44, TotalMonthlyFees: 528, CostOfLoan: 748, TotalAmountToRepay: 1848, LoanTermDescription: "12 months", ComparisonRate: { LoanAmount: 1000, LoanTermDescription: "6 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.44, TotalFeesDollars: 440, ComparisonRatePercentage: 152.03 } },
                        { LoanAmount: 1200, EstablishmentFee: 240, TotalLoanAmount: 1440, NumberOfRepayments: 50, RepaymentFrequency: "Weekly", RepaymentAmounts: 40.32, MonthlyFee: 48, TotalMonthlyFees: 576, CostOfLoan: 816, TotalAmountToRepay: 2016, LoanTermDescription: "12 months", ComparisonRate: { LoanAmount: 1000, LoanTermDescription: "6 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.44, TotalFeesDollars: 440, ComparisonRatePercentage: 152.03 } },
                        { LoanAmount: 1300, EstablishmentFee: 260, TotalLoanAmount: 1560, NumberOfRepayments: 50, RepaymentFrequency: "Weekly", RepaymentAmounts: 43.68, MonthlyFee: 52, TotalMonthlyFees: 624, CostOfLoan: 884, TotalAmountToRepay: 2184, LoanTermDescription: "12 months", ComparisonRate: { LoanAmount: 1000, LoanTermDescription: "6 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.44, TotalFeesDollars: 440, ComparisonRatePercentage: 152.03 } },
                        { LoanAmount: 1400, EstablishmentFee: 280, TotalLoanAmount: 1680, NumberOfRepayments: 50, RepaymentFrequency: "Weekly", RepaymentAmounts: 47.04, MonthlyFee: 56, TotalMonthlyFees: 672, CostOfLoan: 952, TotalAmountToRepay: 2352, LoanTermDescription: "12 months", ComparisonRate: { LoanAmount: 1000, LoanTermDescription: "6 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.44, TotalFeesDollars: 440, ComparisonRatePercentage: 152.03 } },
                        { LoanAmount: 1500, EstablishmentFee: 300, TotalLoanAmount: 1800, NumberOfRepayments: 50, RepaymentFrequency: "Weekly", RepaymentAmounts: 50.4, MonthlyFee: 60, TotalMonthlyFees: 720, CostOfLoan: 1020, TotalAmountToRepay: 2520, LoanTermDescription: "12 months", ComparisonRate: { LoanAmount: 1000, LoanTermDescription: "6 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.44, TotalFeesDollars: 440, ComparisonRatePercentage: 152.03 } },
                        { LoanAmount: 1600, EstablishmentFee: 320, TotalLoanAmount: 1920, NumberOfRepayments: 50, RepaymentFrequency: "Weekly", RepaymentAmounts: 53.76, MonthlyFee: 64, TotalMonthlyFees: 768, CostOfLoan: 1088, TotalAmountToRepay: 2688, LoanTermDescription: "12 months", ComparisonRate: { LoanAmount: 2000, LoanTermDescription: "12 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.68, TotalFeesDollars: 1360, ComparisonRatePercentage: 153.8 } },
                        { LoanAmount: 1700, EstablishmentFee: 340, TotalLoanAmount: 2040, NumberOfRepayments: 50, RepaymentFrequency: "Weekly", RepaymentAmounts: 57.12, MonthlyFee: 68, TotalMonthlyFees: 816, CostOfLoan: 1156, TotalAmountToRepay: 2856, LoanTermDescription: "12 months", ComparisonRate: { LoanAmount: 2000, LoanTermDescription: "12 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.68, TotalFeesDollars: 1360, ComparisonRatePercentage: 153.8 } },
                        { LoanAmount: 1800, EstablishmentFee: 360, TotalLoanAmount: 2160, NumberOfRepayments: 50, RepaymentFrequency: "Weekly", RepaymentAmounts: 60.48, MonthlyFee: 72, TotalMonthlyFees: 864, CostOfLoan: 1224, TotalAmountToRepay: 3024, LoanTermDescription: "12 months", ComparisonRate: { LoanAmount: 2000, LoanTermDescription: "12 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.68, TotalFeesDollars: 1360, ComparisonRatePercentage: 153.8 } },
                        { LoanAmount: 1900, EstablishmentFee: 380, TotalLoanAmount: 2280, NumberOfRepayments: 50, RepaymentFrequency: "Weekly", RepaymentAmounts: 63.84, MonthlyFee: 76, TotalMonthlyFees: 912, CostOfLoan: 1292, TotalAmountToRepay: 3192, LoanTermDescription: "12 months", ComparisonRate: { LoanAmount: 2000, LoanTermDescription: "12 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.68, TotalFeesDollars: 1360, ComparisonRatePercentage: 153.8 } },
                        { LoanAmount: 2000, EstablishmentFee: 400, TotalLoanAmount: 2400, NumberOfRepayments: 50, RepaymentFrequency: "Weekly", RepaymentAmounts: 67.2, MonthlyFee: 80, TotalMonthlyFees: 960, CostOfLoan: 1360, TotalAmountToRepay: 3360, LoanTermDescription: "12 months", ComparisonRate: { LoanAmount: 2000, LoanTermDescription: "12 months", NumberOfRepayments: 26, TotalFeesPercentage: 0.68, TotalFeesDollars: 1360, ComparisonRatePercentage: 153.8 } }
                    ]
                };
            result = $.map(loanDetails[model.SelectedLoanTerm()], function (item) {
                return item.LoanAmount === selectedLoanAmount ? item : null;
            });
            return result.length > 0 ? result[0] : {};
        },
        testExpectations = function () {
            var selectedLoanDetails = model.SelectedLoanDetails(),
                expectations = getExpectations();
            $.each(expectations, function (key, value) {
                if (key === 'ComparisonRate') {
                    expect(model.ComparisonRateLoanAmount()).toBe('$' + value.LoanAmount);
                    expect(model.ComparisonRateLoanTerm()).toBe(value.LoanTermDescription);
                    expect(model.ComparisonRatePercentage()).toBe(value.ComparisonRatePercentage + '%');
                } else {
                    expect(selectedLoanDetails[key]).toBe(value);
                }
            });
        };

    describe("LoanDetailsViewModel", function () {
        beforeEach(function () {
            model = new boomer.LoanDetailsViewModel();
        });
        it("Cannot select invalid values", function () {
            model.SelectedLoanAmount(-10);
            expect(model.SelectedLoanDetails()).toBeNull();
            model.SelectedLoanAmount(0);
            expect(model.SelectedLoanDetails()).toBeNull();
            model.SelectedLoanAmount(10);
            expect(model.SelectedLoanDetails()).toBeNull();
            model.SelectedLoanAmount(50);
            expect(model.SelectedLoanDetails()).toBeNull();
            model.SelectedLoanAmount(350);
            expect(model.SelectedLoanDetails()).toBeNull();
            model.SelectedLoanAmount(2050);
            expect(model.SelectedLoanDetails()).toBeNull();
            model.SelectedLoanAmount(2100);
            expect(model.SelectedLoanDetails()).toBeNull();
        });

        it("Has default loan value set to 1000", function () {
            expect(model.SelectedLoanAmount()).toBe(1000);
        });
        it("Has min loan value set to 100", function () {
            expect(model.MinLoanAmount).toBe(100);
        });
        it("Has max value set to 2000", function () {
            expect(model.MaxLoanAmount).toBe(2000);
        });
        it("Has loan step amount set to 100", function () {
            expect(model.LoanStepAmount).toBe(100);
        });

        describe("On selecting a 6 week loan", function () {
            beforeEach(function () {
                model.SelectedLoanTerm('6');
            });
            describe("On selecting amount", function () {
                describe("Loan of 100", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(100);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 200", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(200);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 300", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(300);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 400", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(400);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 500", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(500);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 600", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(600);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 700", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(700);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 800", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(800);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 900", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(900);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 1000", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(1000);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 1100", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(1100);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 1200", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(1200);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 1300", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(1300);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 1400", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(1400);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 1500", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(1500);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 1600", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(1600);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 1700", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(1700);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 1800", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(1800);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 1900", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(1900);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 2000", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(2000);
                    });
                    it("has expected values", testExpectations);
                });
            });
        });

        describe("On selecting a 12 month loan", function () {
            beforeEach(function () {
                model.SelectedLoanTerm('52');
            });
            describe("On selecting amount", function () {
                describe("Loan of 600", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(600);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 700", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(700);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 800", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(800);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 900", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(900);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 1000", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(1000);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 1100", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(1100);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 1200", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(1200);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 1300", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(1300);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 1400", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(1400);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 1500", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(1500);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 1600", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(1600);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 1700", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(1700);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 1800", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(1800);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 1900", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(1900);
                    });
                    it("has expected values", testExpectations);
                });
                describe("Loan of 2000", function () {
                    beforeEach(function () {
                        model.SelectedLoanAmount(2000);
                    });
                    it("has expected values", testExpectations);
                });
            });
        });        
    });
}(boomer, jQuery));
