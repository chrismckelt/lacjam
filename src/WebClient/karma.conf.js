
// Karma configuration
// Generated on Thu Jun 26 2014 21:20:13 GMT+0800 (W. Australia Standard Time)

module.exports = function(config) {
    config.set({

        // base path that will be used to resolve all patterns (eg. files, exclude)
        basePath: '',


        // frameworks to use
        // available frameworks: https://npmjs.org/browse/keyword/karma-adapter
        frameworks: ['jasmine'],


        // list of files / patterns to load in the browser
        files: [
            'app/_References.js',
           
    'scripts/respond.js',
    'scripts/underscore.js',
    'scripts/jquery/moment.min.js',
    'scripts/jquery/jquery-2.1.1.min.js',
    'scripts/jquery/jquery-ui-1.10.4.min.js',
    'scripts/q.js',
    'scripts/spin.js',
    'scripts/toastr.js',
    'scripts/plugins/metisMenu/jquery.metisMenu.js',
    'scripts/plugins/slimscroll/jquery.slimscroll.min.js',
    'scripts/plugins/flot/jquery.flot.js',
    'scripts/plugins/flot/jquery.flot.tooltip.min.js',
    'scripts/plugins/flot/jquery.flot.spline.js',
    'scripts/plugins/flot/jquery.flot.resize.js',
    'scripts/plugins/flot/jquery.flot.pie.js',
    'scripts/plugins/flot/curvedLines.js',

    'scripts/plugins/peity/jquery.peity.min.js',

    'scripts/plugins/morris/raphael-2.1.0.min.js',
    'scripts/plugins/morris/morris.js',

    'scripts/plugins/iCheck/icheck.min.js',

    'scripts/plugins/chosen/chosen.jquery.js',

    'scripts/plugins/pace/pace.min.js',
    'scripts/plugins/fancybox/jquery.fancybox.js',
    'scripts/plugins/rickshaw/vendor/d3.v3.js',
    'scripts/plugins/rickshaw/rickshaw.min.js',
    'scripts/plugins/ionRangeSlider/ion.rangeSlider.min.js',
    'scripts/plugins/nouslider/jquery.nouislider.min.js',
    'scripts/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js',
    'scripts/plugins/jvectormap/jquery-jvectormap-world-mill-en.js',
    'scripts/plugins/jasny/jasny-bootstrap.min.js',
    'scripts/plugins/switchery/switchery.js',
    'scripts/plugins/dataTables/jquery.dataTables.js',
    'scripts/plugins/dataTables/dataTables.bootstrap.js',
    'scripts/plugins/easypiechart/easypiechart.js',
    'scripts/plugins/sparkline/jquery.sparkline.min.js',
    'scripts/plugins/dropzone/dropzone.js',
    'scripts/plugins/chartJs/Chart.min.js',
    'scripts/plugins/jsKnob/jquery.knob.js',
    'scripts/plugins/summernote/summernote.min.js',
    'scripts/plugins/fullcalendar/fullcalendar.min.js',
    'scripts/plugins/codemirror/codemirror.js',
    'scripts/plugins/codemirror/mode/javascript/javascript.js',
    'scripts/angular/angular.js',
    'scripts/angular/angular-ui-router.js',
    'scripts/angular/angular-ui.js',
    'scripts/angular/angular-animate.js',
    'scripts/angular/angular-aria.js',
    'scripts/angular/angular-cookies.js',
    'scripts/angular/angular-loader.js',
    'scripts/angular/angular-messages.js',
    'scripts/angular/angular-resource.js',
    'scripts/angular/angular-sanitize.js',
    'scripts/angular/angular-translate.js',

    'scripts/bootstrap/ui-bootstrap-tpls-0.11.0.min.js',
    'scripts/plugins/peity/angular-peity.js',
    'scripts/plugins/easypiechart/angular.easypiechart.js',
    'scripts/plugins/flot/angular-flot.js',
    'scripts/plugins/rickshaw/angular-rickshaw.js',
    'scripts/plugins/summernote/angular-summernote.min.js',
    'scripts/bootstrap/angular-bootstrap-checkbox.js',
    'scripts/plugins/jsKnob/angular-knob.js',
    'scripts/plugins/switchery/ng-switchery.js',
    'scripts/plugins/nouslider/angular-nouislider.js',
    'scripts/plugins/datapicker/datePicker.js',
    'scripts/plugins/chosen/chosen.js',
    'scripts/plugins/dataTables/angular-datatables.min.js',
    'scripts/plugins/fullcalendar/gcal.js',
    'scripts/plugins/fullcalendar/calendar.js',
    'scripts/plugins/chartJs/angles.js',
    'scripts/plugins/uievents/event.js',
    'scripts/plugins/nggrid/ng-grid-2.0.3.min.js',
    'scripts/plugins/ui-codemirror/ui-codemirror.min.js',
    'scripts/plugins/uiTree/angular-ui-tree.min.js',
    'scripts/plugins/angular-notify/angular-notify.min.js',
    'scripts/plugins/colorpicker/bootstrap-colorpicker-module.js',

    'scripts/angular-dialog-service-5.1.2/dialogs-default-translations.js',
    'scripts/angular-dialog-service-5.1.2/dialogs.js',
    'scripts/typings/jasmine/jasmine-html.js',
    'scripts/typings/jasmine/jasmine.js',
    'app/app.js',
    'app/Directives.js',
    'app/Filters.js',
    'app/model.js',
    'app/publicApp.js',
    'app/routes.js',
    'app/Services.js',
    'app/types.js',
    'app/Common/common.js',
    'app/Entities/EntityController.js',
    'app/Entities/EntityEditController.js',
    'app/Entities/EntityService.js',
    'app/Index/IndexController.js',
    'app/MetadataDefinitionGroups/MetadataDefinitionGroupController.js',
    'app/MetadataDefinitionGroups/MetadataDefinitionGroupEditController.js',
    'app/MetadataDefinitionGroups/MetadataDefinitionGroupService.js',
    'app/MetadataDefinitions/MetadataDefinitionController.js',
    'app/MetadataDefinitions/MetadataDefinitionEditController.js',
    'app/MetadataDefinitions/MetadataDefinitionService.js',
    'app/Public/SearchController.js',
    'app/Z/registrations.js',

    'tests/appFixture.js',
    
	{pattern: 'tests/*.js', watched: false, included: false, served: false}
    ],


    // list of files to exclude
    exclude: [
      'scripts/angular-scenario.js',
    ],

    // preprocess matching files before serving them to the browser
    // available preprocessors: https://npmjs.org/browse/keyword/karma-preprocessor
    preprocessors: {
    
    },


    // test results reporter to use
    // possible values: 'dots', 'progress'
    // available reporters: https://npmjs.org/browse/keyword/karma-reporter
    reporters: ['progress'],


    // web server port
    port: 9876,


    // enable / disable colors in the output (reporters and logs)
    colors: true,


    // level of logging
    // possible values: config.LOG_DISABLE || config.LOG_ERROR || config.LOG_WARN || config.LOG_INFO || config.LOG_DEBUG
    logLevel: config.LOG_INFO,


    // enable / disable watching file and executing tests whenever any file changes
    autoWatch: true,


    // start these browsers
    // available browser launchers: https://npmjs.org/browse/keyword/karma-launcher
    browsers: ['PhantomJS'],


    // Continuous Integration mode
    // if true, Karma captures browsers, runs the tests and exits
    singleRun: false
  });
};
