
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
            'scripts/jquery-1.10.2.js',
            'scripts/underscore.js',
            'scripts/modernizr-2.6.2.js',
            'scripts/q.js',
            'scripts/respond.js',
            'scripts/angular.js',
            'scripts/spin.js',
            'scripts/toastr.js',
            'scripts/angular-ui-router.js',
            'scripts/angular-ui.js',
            'scripts/bootstrap.js',
            'scripts/select2.js',
            'scripts/respond.matchmedia.addListener.js',
            'scripts/angular-animate.js',
            'scripts/angular-aria.js',
            'scripts/angular-cookies.js',
            'scripts/angular-loader.js',
            'scripts/angular-messages.js',
            'scripts/angular-resource.js',
            'scripts/angular-route.js',
            'scripts/angular-sanitize.js',
            'scripts/angular-touch.js',
            'scripts/angular-translate.js',
            'scripts/angular-ui-ieshiv.js',
            'scripts/angular-ui/ui-bootstrap-tpls.js',
            'scripts/angular-ui/ui-bootstrap.js',
            'scripts/ng-grid-flexible-height.js',
            'scripts/ng-grid.js',
            'scripts/r.js',
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

            //tests

            'tests/appFixture.js',
    
	{pattern: 'tests/*.js', watched: false, included: false, served: false}
    ],


    // list of files to exclude
    exclude: [
      '/scripts/angular-scenario.js',
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
