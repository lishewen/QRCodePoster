require.config({
	baseUrl: '/js',
	paths: {
		'jquery': '/lib/jquery/dist/jquery.min',
		'jquery.jplayer': '/lib/jplayer/jplayer/jquery.jplayer.min',
		'contextMenu': '/lib/jquery-contextmenu/jquery.contextMenu.min',
		'bootstrap': '/lib/bootstrap/dist/js/bootstrap.min',
		'popper': '/lib/popper.js/umd/popper.min',
		'colorpicker': '/lib/spectrum/spectrum',
		'css': '/lib/require-css/css.min',
		'underscore': '/lib/underscore.js/underscore-min',
		'webuploader': '/lib/webuploader/webuploader.min'
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
		'contextMenu': {
			exports: "$",
			deps: ['jquery']
		},
		'colorpicker': {
			exports: '$',
			deps: ['css!/lib/spectrum/spectrum.css']
		},
		'webuploader': {
			deps: ['css!/lib/webuploader/webuploader.css']
		}
	},
	map: {
		'*': {
			'popper.js': 'popper'
		}
	}
});