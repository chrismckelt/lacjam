/*jslint plusplus: true*/
/*global jQuery, boomer, moment, jasmine, describe, beforeEach, it, expect, spyOn*/

/// <reference path="~/Scripts/ThirdParty/jquery-1.10.2.js"/>
/// <reference path="~/Scripts/ThirdParty/jquery.validate.js"/>
/// <reference path="~/Scripts/ThirdParty/jquery.validate.unobtrusive.js"/>
/// <reference path="~/Scripts/ThirdParty/knockout-2.2.1.debug.js"/>
/// <reference path="~/Scripts/ThirdParty/jasmine/jasmine.js"/>
/// <reference path="~/Scripts/ThirdParty/moment.js" />
/// <reference path="~/Scripts/boomer.js" />
/// <reference path="~/Scripts/ViewModels/DateSelector.js" />

// DEV NOTE javascript and specifically moment js use a 0 based index for its months, however we want to pass back 1 based indexes for the month as that is what .Net consumes

(function (boomer, moment) {
    "use strict";

    describe("Set up", function () {
        it("boomer is defined", function () {
            expect(boomer).toBeDefined();
        });
        it("DateSelector is defined", function () {
            expect(boomer.DateSelector).toBeDefined();
        });
    });

    describe("DateSelector", function () {
        describe("Default", function () {
            var ds = new boomer.DateSelector(moment([2010, 3, 11]), moment([2013, 7, 5]));
            it("should not have defaulted values", function () {
                expect(ds.Year()).toBeUndefined();
                expect(ds.Month()).toBeUndefined();
                expect(ds.Day()).toBeUndefined();
            });
        });

        describe("when changing to earliest year", function () {
            var ds, earliestDate = moment([2010, 2, 11]);
            beforeEach(function () {
                ds = new boomer.DateSelector(earliestDate, moment([2013, 4, 5]));
            });
            describe("given select month is before earliest month", function () {
                it("should set the month to null", function () {
                    ds.Year(2012);
                    ds.Month(2);//FEB - ie one month earlier than the earliest month in the earliest year
                    ds.Year(earliestDate.years());
                    expect(ds.Month()).toBeNull();
                });
            });
            describe("given selected day is before earliest day", function () {
                it("should set the Day to null", function () {
                    ds.Year(2012);
                    ds.Month(3); //earliest month
                    ds.Day(1);//before the earliest day for the earliest month
                    ds.Year(earliestDate.years());
                    expect(ds.Day()).toBeNull();
                });
            });
        });

        describe("when changing to latest year", function () {
            var ds, latestdate = moment([2013, 4, 5]);
            beforeEach(function () {
                ds = new boomer.DateSelector(moment([2010, 2, 11]), latestdate);
            });
            describe("Only valid months are shown", function () {
                it("should only show selectable months", function () {
                    ds.Year(2013);
                    expect(ds.Months().length).toBe(5); //.Net month index
                });
            });
            describe("when changing to latest month", function () {
                describe("Only valid days are shown", function () {
                    it("should only show selectable days", function () {
                        ds.Year(2013);
                        ds.Month(5);
                        expect(ds.Days().length).toBe(5);
                    });
                });
            });
            describe("given select month is after latest month", function () {
                it("should set the month to latest month", function () {
                    ds.Year(2012);
                    ds.Month(6);//JUN - ie one month later than the latest month in the latest year
                    ds.Year(latestdate.years());
                    expect(ds.Month()).toBeNull();
                });
            });
            describe("given selected day is after latest day", function () {
                it("should set the Day to null", function () {
                    ds.Year(2012);
                    ds.Month(5); //latest month
                    ds.Day(6);//after the latest day for the latest month
                    ds.Year(latestdate.years());
                    expect(ds.Day()).toBeNull();
                });
            });
        });

        describe("Days in months", function () {
            it("should only show valid days in each months", function () {
                var ds = new boomer.DateSelector(moment([2000, 1, 1]), moment([2020, 1, 1]));
                ds.Year(2011);//non leap year
                ds.Month(1);
                expect(ds.Days().length).toBe(31);
                ds.Month(2);
                expect(ds.Days().length).toBe(28);
                ds.Month(3);
                expect(ds.Days().length).toBe(31);
                ds.Month(4);
                expect(ds.Days().length).toBe(30);
                ds.Month(5);
                expect(ds.Days().length).toBe(31);
                ds.Month(6);
                expect(ds.Days().length).toBe(30);
                ds.Month(7);
                expect(ds.Days().length).toBe(31);
                ds.Month(8);
                expect(ds.Days().length).toBe(31);
                ds.Month(9);
                expect(ds.Days().length).toBe(30);
                ds.Month(10);
                expect(ds.Days().length).toBe(31);
                ds.Month(11);
                expect(ds.Days().length).toBe(30);
                ds.Month(12);
                expect(ds.Days().length).toBe(31);
                //leap year
                ds.Year(2012);
                ds.Month(2);
                expect(ds.Days().length).toBe(29);
            });
        });

        describe("when changing month", function () {
            it("when current day is greater than last day in selected month, set day to null", function () {
                var ds = new boomer.DateSelector(moment([2000, 1, 1]), moment([2020, 1, 1]));
                ds.Year(2011);//non leap
                ds.Month(3);
                ds.Day(30);
                ds.Month(2);
                expect(ds.Day()).toBeNull();
            });
            it("when setting month to null shows all days", function () {
                var ds = new boomer.DateSelector(moment([2000, 1, 1]), moment([2020, 1, 1]));
                ds.Year(2011);//non leap
                ds.Month(2);
                expect(ds.Days().length).not.toBe(31);
                ds.Month(null);
                expect(ds.Days().length).toBe(31);
            });
        });
        describe("when changing value to null", function () {
            var ds;
            beforeEach(function () {
                ds = new boomer.DateSelector(moment([2010, 2, 11]), moment([2013, 4, 5]));
                ds.Year(2011);//non leap
                ds.Month(3);
                ds.Day(30);
                expect(ds.DateText()).not.toBeNull();
            });
            it("when setting day to null Datetext is null", function () {
                ds.Day(null);
                expect(ds.DateText()).toBeNull();
            });
            it("when setting day to empty Datetext is null", function () {
                ds.Day("");
                expect(ds.DateText()).toBeNull();
            });
            it("when setting month to null Datetext is null", function () {
                ds.Month(null);
                expect(ds.DateText()).toBeNull();
            });
            it("when setting month to empty Datetext is null", function () {
                ds.Month("");
                expect(ds.DateText()).toBeNull();
            });
            it("when setting year to null Datetext is null", function () {
                ds.Year(null);
                expect(ds.DateText()).toBeNull();
            });
            it("when setting year to empty Datetext is null", function () {
                ds.Year("");
                expect(ds.DateText()).toBeNull();
            });
        });
        describe("DateText binding", function () {
            describe("DateText is read correctly", function () {
                it("gives 02/06/2012", function () {
                    var ds = new boomer.DateSelector(moment([2000, 1, 1]), moment([2020, 1, 1]));
                    ds.Year(2012);
                    ds.Month(6);
                    ds.Day(2);
                    expect(ds.DateText()).toBe('02/06/2012');
                });
                it("gives 01/01/2012", function () {
                    var ds = new boomer.DateSelector(moment([2000, 1, 1]), moment([2020, 1, 1]));
                    ds.Year(2012);
                    ds.Month(1);
                    ds.Day(1);
                    expect(ds.DateText()).toBe('01/01/2012');
                });
            });
            describe("DateText is written correctly", function () {
                it("given 02/06/2012 it get correct date", function () {
                    var ds = new boomer.DateSelector(moment([2000, 1, 1]), moment([2020, 1, 1]));
                    ds.DateText('02/06/2012');
                    expect(ds.Year()).toBe(2012);
                    expect(ds.Month()).toBe(6);
                    expect(ds.Day()).toBe(2);
                });
                it("given 12/06/2012 it get correct date", function () {
                    var ds = new boomer.DateSelector(moment([2000, 1, 1]), moment([2020, 1, 1]));
                    ds.DateText('12/06/2012');
                    expect(ds.Year()).toBe(2012);
                    expect(ds.Month()).toBe(6);
                    expect(ds.Day()).toBe(12);
                });
                it("given 01/01/2012 it get correct date", function () {
                    var ds = new boomer.DateSelector(moment([2000, 1, 1]), moment([2020, 1, 1]));
                    ds.DateText('01/01/2012');
                    expect(ds.Year()).toBe(2012);
                    expect(ds.Month()).toBe(1);
                    expect(ds.Day()).toBe(1);
                });
            });
            describe("Invalid datetexts are not assigned", function () {
                it("given invalid string it changes the existing date to undefined", function () {
                    var ds = new boomer.DateSelector(moment([2000, 1, 1]), moment([2020, 1, 1]));
                    ds.Year(2012);
                    ds.Month(6);
                    ds.Day(2);
                    ds.DateText('33/33/2012');
                    expect(ds.Year()).toBeNull();
                    expect(ds.Month()).toBeNull();
                    expect(ds.Day()).toBeNull();
                });
                it("given date before earliest date it changes the existing date to undefined", function () {
                    var ds = new boomer.DateSelector(moment([2000, 1, 1]), moment([2020, 1, 1]));
                    ds.Year(2012);
                    ds.Month(6);
                    ds.Day(2);
                    ds.DateText('01/01/1999');
                    expect(ds.Year()).toBeNull();
                    expect(ds.Month()).toBeNull();
                    expect(ds.Day()).toBeNull();
                });
                it("given date after latest date it changes the existing date to undefined", function () {
                    var ds = new boomer.DateSelector(moment([2000, 1, 1]), moment([2020, 1, 1]));
                    ds.Year(2012);
                    ds.Month(6);
                    ds.Day(2);
                    ds.DateText('01/01/2021');
                    expect(ds.Year()).toBeNull();
                    expect(ds.Month()).toBeNull();
                    expect(ds.Day()).toBeNull();
                });
            });
        });
        describe("Can handle when we want date greater than the max displayable", function () {
            it("Has display of Before + given max year when toggled on", function () {
                var ds = new boomer.MonthYearSelector(moment([2000, 1, 1]), moment([2020, 1, 1]), { allowPreOldestDate: true }),
                    lastElement = ds.Years[ds.Years.length - 1];
                expect(lastElement.Display).toBe('Before 2000');
            });
            it("Has value of previous year when toggled on", function () {
                var ds = new boomer.MonthYearSelector(moment([2000, 1, 1]), moment([2020, 1, 1]), { allowPreOldestDate: true }),
                    lastElement = ds.Years[ds.Years.length - 1];
                expect(lastElement.Value).toBe(1999);
            });
            it("Selecting before will set months drop down to before text too", function () {
                var ds = new boomer.MonthYearSelector(moment([2000, 1, 1]), moment([2020, 1, 1]), { allowPreOldestDate: true });
                ds.Year(1999);
                expect(ds.IsPreLatestYear()).toBeTruthy();
                expect(ds.Months().length).toBe(1);
                expect(ds.Months()[0].Display).toBe('Before 2000');
                expect(ds.Months()[0].Value).toBe(1);//jan
            });
            it("Selecting all months are visible on earliest year", function () {
                //it would be wierd if we can have before a date but not show months after it...
                var ds = new boomer.MonthYearSelector(moment([2000, 5, 1]), moment([2020, 1, 1]), { allowPreOldestDate: true });
                ds.Year(2000);
                expect(ds.Months().length).toBe(12);
            });
        });
    });
}(boomer, moment));
