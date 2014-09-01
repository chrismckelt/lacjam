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
    'scripts/jquery-1.10.2.js',
	'scripts/angular.js',
	'scripts/angular-animate.js',
	'scripts/angular-cookies.js',
	'scripts/angular-loader.js',
	'scripts/angular-mocks.js',
	'scripts/angular-resource.js',
	'scripts/angular-route.js',
	'scripts/angular-sanitize.js',
	'scripts/angular-touch.js',
	'scripts/angular-ui-router.js',
	'scripts/bootstrap.js',
	'scripts/modernizr-2.6.2.js',
	'scripts/r.js',
	'scripts/require.js',
	'scripts/respond.js',
	'scripts/underscore.js',
	'scripts/angular-ui/ui-bootstrap.js',
	'scripts/angular-ui/ui-bootstrap-tpls.js',
	'app/_references.js',
	'app/app.js',
	'app/services.js',
	'app/directives.js',
	'tests/app.js',

	
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
    browsers: ['Chrome'],


    // Continuous Integration mode
    // if true, Karma captures browsers, runs the tests and exits
    singleRun: false
  });
};
