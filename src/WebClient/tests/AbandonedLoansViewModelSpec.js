/*jslint plusplus: true*/
/*global jQuery, boomer, jasmine, describe, beforeEach, it, expect, spyOn*/

/// <reference path="~/Scripts/ThirdParty/jquery-1.10.2.js"/>
/// <reference path="~/Scripts/ThirdParty/jquery.validate.js"/>
/// <reference path="~/Scripts/ThirdParty/jquery.validate.unobtrusive.js"/>
/// <reference path="~/Scripts/ThirdParty/knockout-2.2.1.debug.js"/>
/// <reference path="~/Scripts/ThirdParty/ko.simplegrid.js"/>
/// <reference path="~/Scripts/ThirdParty/moment.js"/>
/// <reference path="~/Scripts/ThirdParty/jasmine/jasmine.js"/>
/// <reference path="~/Scripts/boomer.js" />
/// <reference path="~/Scripts/ViewModels/DateSelector.js" />
/// <reference path="~/Scripts/ViewModels/FilteredViewModel.js"/>
/// <reference path="~/Scripts/ViewModels/SortableViewModel.js"/>
/// <reference path="~/Scripts/ViewModels/AbandonedLoansViewModel.js" />
/// <reference path="~/Scripts/ViewModels/ResurrectionViewModel.js" />
/// <reference path="~/Scripts/ViewModels/WithdrawalViewModel.js" />
/// <reference path="~/Scripts/ViewModels/LeadChaseViewModel.js" />

(function (boomer) {
    "use strict";
    var vm,
        data = [
            { FirstName: "Frank", MiddleName: "Vincent", LastName: "Zappa", DateOfBirth: "21/12/1940", Mobile: "0412 345 678", Other: "08 3333 4444", Email: "frank@zappa.net", State: "WA", CurrentApplicationStep: "Income", LastModified: "04/12/2013 04:11:12" },
            { FirstName: "John", MiddleName: "Henry", LastName: "Bonham", DateOfBirth: "31/05/1948", Mobile: "0423456789", Other: "08 3210 9876", Email: "jb@ledzeppelin.com", State: "NSW", CurrentApplicationStep: "PersonalDetails", LastModified: "04/12/2013 04:32:45" },
            { FirstName: "Victor", MiddleName: "Lemonte", LastName: "Wooten", DateOfBirth: "11/09/1964", Mobile: "04 1111 2222", Other: "0832415069", Email: "victor.wooten@palmystery.org", State: "VIC", CurrentApplicationStep: "Income", LastModified: "04/12/2013 05:01:00" },
            { FirstName: "Robert", MiddleName: "James", LastName: "Smith", DateOfBirth: "21/04/1959", Mobile: "0412987654", Other: null, Email: "fatbob@thecure.com", State: "WA", CurrentApplicationStep: "ResidentialAddress", LastModified: "04/12/2013 05:05:05" }
        ],
        resurrectionReasons = [
            { Id: 0, Reason: 'Other', Active: true, DisplaySequence: 9999 },
            { Id: 4, Reason: 'Too difficult to fill out application', Active: true, DisplaySequence: 400 },
            { Id: 5, Reason: 'Does not want to provide employment details', Active: true, DisplaySequence: 500 },
            { Id: 6, Reason: 'Does not want to provide documentation', Active: true, DisplaySequence: 600 }
        ],
        withdrawalReasons = [
            { Id: 0, Reason: 'Other', Active: true, DisplaySequence: 9999 },
            { Id: 1, Reason: 'Client not interested', Active: true, DisplaySequence: 100 },
            { Id: 2, Reason: 'Client already a customer', Active: true, DisplaySequence: 200 },
            { Id: 3, Reason: 'Cannot contact', Active: true, DisplaySequence: 300 },
            { Id: 4, Reason: 'Gone elsewhere', Active: true, DisplaySequence: 400 },
            { Id: 5, Reason: 'Does not qualify', Active: true, DisplaySequence: 500 },
            { Id: 6, Reason: 'No Medicare card', Active: true, DisplaySequence: 50 },
            { Id: 7, Reason: 'No secondary identification', Active: true, DisplaySequence: 60 },
            { Id: 8, Reason: 'No bank details', Active: true, DisplaySequence: 70 }
        ];

    describe("AbandonedLoansViewModel", function () {
        beforeEach(function () {
            vm = new boomer.AbandonedLoansViewModel(data, resurrectionReasons, withdrawalReasons, 'LastModified', false, $.noop, $.noop, $.noop, $.noop);
        });
        it("does not filter (exclude) any records by default", function () {
            expect(ko.utils.unwrapObservable(vm.data).length).toBe(data.length); // 4
        });
        it("contains all columns", function () {
            expect(vm.columns.length).toBe(13);
        });
        it("displays the 'Actions' column at the right", function () {
            expect(vm.columns[vm.columns.length - 1].headerText).toBe('Actions');
        });
        it("contains all filters", function () {
            expect(ko.utils.unwrapObservable(vm.filteredViewModel.filters).length).toBe(12);
        });
        it("does not apply any filters by default", function () {
            expect(ko.utils.unwrapObservable(vm.filteredViewModel.activeFilters).length).toBe(0);
        });
        it("does not display any filter fields by default", function () {
            var i, filters = ko.utils.unwrapObservable(vm.filteredViewModel.filters);
            for (i = 0; i < filters.length; ++i) {
                expect(ko.utils.unwrapObservable(filters[i].visible)).toBeFalsy();
            }
        });

        describe("when making a filter field visible", function () {
            beforeEach(function () {
                ko.utils.unwrapObservable(vm.filteredViewModel.filters)[0].toggleVisible();
            });
            it("displays the filter field", function () {
                expect(ko.utils.unwrapObservable(ko.utils.unwrapObservable(vm.filteredViewModel.filters)[0].visible)).toBeTruthy();
            });
            it("does not filter any other fields", function () {
                var i;
                for (i = 1; i < ko.utils.unwrapObservable(vm.filteredViewModel.filters).length; ++i) {
                    expect(ko.utils.unwrapObservable(ko.utils.unwrapObservable(vm.filteredViewModel.filters)[i].visible)).toBeFalsy();
                }
            });

            describe("when entering a filter value that matches multiple records (case insensitive)", function () {
                beforeEach(function () {
                    ko.utils.unwrapObservable(vm.filteredViewModel.filters)[0].value("r");
                });
                it("remembers the value", function () {
                    expect(ko.utils.unwrapObservable(vm.filteredViewModel.filters)[0].value()).toBe("r");
                });
                it("marks the filter as active", function () {
                    expect(ko.utils.unwrapObservable(vm.filteredViewModel.activeFilters).length).toBe(1);
                });
                it("does not immediately filter the view", function () {
                    expect(ko.utils.unwrapObservable(vm.data).length).toBe(4);
                });
                it("filters the view to contain only matching records, after a delay", function () {
                    waits(210);
                    runs(function () {
                        expect(ko.utils.unwrapObservable(vm.data).length).toBe(3);
                    });
                });

                describe("when entering a filter value in a different filter field so that the combination matches a single record, but each filter on its own matches multiple records", function () {
                    beforeEach(function () {
                        ko.utils.unwrapObservable(vm.filteredViewModel.filters)[2].value("a");
                    });
                    it("remembers the values for both filters", function () {
                        expect(ko.utils.unwrapObservable(vm.filteredViewModel.filters)[0].value()).toBe("r");
                        expect(ko.utils.unwrapObservable(vm.filteredViewModel.filters)[2].value()).toBe("a");
                    });
                    it("marks both filters as active", function () {
                        expect(ko.utils.unwrapObservable(vm.filteredViewModel.activeFilters).length).toBe(2);
                    });
                    it("filters the view to contain only records that match both filters, after a delay", function () {
                        waits(210);
                        runs(function () {
                            expect(ko.utils.unwrapObservable(vm.data).length).toBe(1);
                        });
                    });
                });
            });

            describe("when entering a filter value that matches a single record (case insensitive)", function () {
                beforeEach(function () {
                    ko.utils.unwrapObservable(vm.filteredViewModel.filters)[0].value("frank");
                });
                it("remembers the value", function () {
                    expect(ko.utils.unwrapObservable(vm.filteredViewModel.filters)[0].value()).toBe("frank");
                });
                it("marks the filter as active", function () {
                    expect(ko.utils.unwrapObservable(vm.filteredViewModel.activeFilters).length).toBe(1);
                });
                it("does not immediately filter the view", function () {
                    expect(ko.utils.unwrapObservable(vm.data).length).toBe(4);
                });
                it("filters the view to contain only matching records, after a delay", function () {
                    waits(210);
                    runs(function () {
                        expect(ko.utils.unwrapObservable(vm.data).length).toBe(1);
                    });
                });

                describe("when hiding the filter", function () {
                    beforeEach(function () {
                        ko.utils.unwrapObservable(vm.filteredViewModel.filters)[0].toggleVisible();
                    });
                    it("no longer displays the filter field", function () {
                        expect(ko.utils.unwrapObservable(ko.utils.unwrapObservable(vm.filteredViewModel.filters)[0].visible)).toBeFalsy();
                    });
                    it("clears the filter value", function () {
                        expect(ko.utils.unwrapObservable(vm.filteredViewModel.filters)[0].value()).toBe("");
                    });
                    it("displays all records (no additional filters are active)", function () {
                        expect(ko.utils.unwrapObservable(vm.data).length).toBe(data.length);
                    });
                });
            });

            describe("when entering a filter value that doesn't match anything (case insensitive)", function () {
                beforeEach(function () {
                    ko.utils.unwrapObservable(vm.filteredViewModel.filters)[0].value("zzz");
                });
                it("remembers the value", function () {
                    expect(ko.utils.unwrapObservable(vm.filteredViewModel.filters)[0].value()).toBe("zzz");
                });
                it("marks the filter as active", function () {
                    expect(ko.utils.unwrapObservable(vm.filteredViewModel.activeFilters).length).toBe(1);
                });
                it("does not immediately filter the view", function () {
                    expect(ko.utils.unwrapObservable(vm.data).length).toBe(4);
                });
                it("filters the view to contain no records, after a delay", function () {
                    waits(210);
                    runs(function () {
                        expect(ko.utils.unwrapObservable(vm.data).length).toBe(0);
                    });
                });

                describe("when hiding the filter", function () {
                    beforeEach(function () {
                        ko.utils.unwrapObservable(vm.filteredViewModel.filters)[0].toggleVisible();
                    });
                    it("no longer displays the filter field", function () {
                        expect(ko.utils.unwrapObservable(ko.utils.unwrapObservable(vm.filteredViewModel.filters)[0].visible)).toBeFalsy();
                    });
                    it("clears the filter value", function () {
                        expect(ko.utils.unwrapObservable(vm.filteredViewModel.filters)[0].value()).toBe("");
                    });
                    it("displays all records (no additional filters are active)", function () {
                        expect(ko.utils.unwrapObservable(vm.data).length).toBe(data.length);
                    });
                });
            });
        });

        describe("initially", function () {
            it("sorts using the specified field", function () {
                expect(vm.sortableViewModel.currentSortOption()).toBe(ko.utils.unwrapObservable(vm.sortableViewModel.sortOptions)[9]); // column index 9 is LastModified
            });
            it("sorts in the specified direction", function () {
                expect(vm.sortableViewModel.currentSortDirection()).toBeFalsy();
            });
        });

        describe("when applying a sort option", function () {
            beforeEach(function () {
                ko.utils.unwrapObservable(vm.sortableViewModel.sortOptions)[2].toggleSort();
            });
            it("applies ascending sort", function () {
                waits(5);
                runs(function () {
                    expect(vm.sortableViewModel.currentSortOption()).toBe(ko.utils.unwrapObservable(vm.sortableViewModel.sortOptions)[2]);
                    expect(vm.sortableViewModel.currentSortDirection()).toBeFalsy(); // false is ascending
                });
            });
            it("sorts the records correctly", function () {
                waits(5);
                runs(function () {
                    var records = ko.utils.unwrapObservable(vm.data);
                    expect(records.length).toBe(4);
                    expect(records[0].LastName).toBe('Bonham');
                    expect(records[1].LastName).toBe('Smith');
                    expect(records[2].LastName).toBe('Wooten');
                    expect(records[3].LastName).toBe('Zappa');
                });
            });

            describe("when reversing the direction of the currently selected sort option", function () {
                beforeEach(function () {
                    ko.utils.unwrapObservable(vm.sortableViewModel.sortOptions)[2].toggleSort();
                });
                it("applies descending sort", function () {
                    waits(5);
                    runs(function () {
                        expect(vm.sortableViewModel.currentSortOption()).toBe(ko.utils.unwrapObservable(vm.sortableViewModel.sortOptions)[2]);
                        expect(vm.sortableViewModel.currentSortDirection()).toBeTruthy(); // true is descending
                    });
                });
                it("sorts the records correctly", function () {
                    waits(5);
                    runs(function () {
                        var records = ko.utils.unwrapObservable(vm.data);
                        expect(records.length).toBe(4);
                        expect(records[0].LastName).toBe('Zappa');
                        expect(records[1].LastName).toBe('Wooten');
                        expect(records[2].LastName).toBe('Smith');
                        expect(records[3].LastName).toBe('Bonham');
                    });
                });

                describe("when reversing again the direction of the currently selected sort option", function () {
                    beforeEach(function () {
                        ko.utils.unwrapObservable(vm.sortableViewModel.sortOptions)[2].toggleSort();
                    });
                    it("applies ascending sort", function () {
                        waits(5);
                        runs(function () {
                            expect(vm.sortableViewModel.currentSortOption()).toBe(ko.utils.unwrapObservable(vm.sortableViewModel.sortOptions)[2]);
                            expect(vm.sortableViewModel.currentSortDirection()).toBeFalsy(); // false is ascending
                        });
                    });
                    it("sorts the records correctly", function () {
                        waits(5);
                        runs(function () {
                            var records = ko.utils.unwrapObservable(vm.data);
                            expect(records.length).toBe(4);
                            expect(records[0].LastName).toBe('Bonham');
                            expect(records[1].LastName).toBe('Smith');
                            expect(records[2].LastName).toBe('Wooten');
                            expect(records[3].LastName).toBe('Zappa');
                        });
                    });
                });

                describe("when selecting a different sort option", function () {
                    beforeEach(function () {
                        ko.utils.unwrapObservable(vm.sortableViewModel.sortOptions)[0].toggleSort();
                    });
                    it("applies ascending sort", function () {
                        waits(5);
                        runs(function () {
                            expect(vm.sortableViewModel.currentSortOption()).toBe(ko.utils.unwrapObservable(vm.sortableViewModel.sortOptions)[0]);
                            expect(vm.sortableViewModel.currentSortDirection()).toBeFalsy(); // false is ascending
                        });
                    });
                });
            });
        });
    });
}(boomer));
