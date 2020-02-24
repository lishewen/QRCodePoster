require.config({
	baseUrl: '/js',
	paths: {
		'jquery': '/lib/jquery/dist/jquery.min',
		'contextMenu': '/lib/jquery-contextmenu/jquery.contextMenu.min',
		'bootstrap': '/lib/bootstrap/dist/js/bootstrap.min',
		'colorpicker': '/lib/spectrum/spectrum',
		'css': '/lib/require-css/css.min'
	},
	shim: {
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
		}
	}
});