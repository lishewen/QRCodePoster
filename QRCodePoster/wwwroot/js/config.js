require.config({
	baseUrl: '/js',
	paths: {
		'jquery': '/lib/jquery/dist/jquery.min',
		'jquery.jplayer': '/lib/jplayer/dist/jplayer/jquery.jplayer.min',
		'contextMenu': '/lib/jQuery-contextMenu/dist/jquery.contextMenu.min',
		'bootstrap': '/lib/bootstrap/dist/js/bootstrap.min',
		'filestyle': '/lib/bootstrap-filestyle/src/bootstrap-filestyle.min',
		'colorpicker': '/lib/spectrum/spectrum',
		'css': '/lib/require-css/css.min',
		'underscore': '/lib/underscore/underscore-min',
		'webuploader': '/js/webuploader.min'
	},
	shim: {
		'jquery.jplayer': {
			exports: "$",
			deps: ['jquery']
		},
		'bootstrap': {
			exports: "$",
			deps: ['jquery']
		},
		'contextMenu':{
			exports: "$",
			deps: ['jquery']
		},
		'filestyle': {
			exports: '$',
			deps: ['bootstrap']
		},
		'colorpicker': {
			exports: '$',
			deps: ['css!/lib/spectrum/spectrum.css']
		},
		'webuploader': {
			deps: ['css!/css/webuploader.css']
		}
	}
});