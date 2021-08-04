function fancybox_init(){


	$("a[data-fancybox]").fancybox({
		loop: true,
		buttons: [
	        "zoom",
	        "share",
	        "slideShow",
	        "fullScreen",
	        "download",
	        "thumbs",
	        "close"
	    ]		
	});

};