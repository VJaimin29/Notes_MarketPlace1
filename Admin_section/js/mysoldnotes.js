/*eslint-env es6*/
/* eslint-disable*/
/*eslint-env browser*/

/* ===========================
        Mobile Menu
============================ */

$(function() {

    //Show mobile navigation
    $("#mobile-nav-open-btn").click(function() {
        
        $("#mobile-nav").css("height", "100%");
        
    });
    
    //Hide mobile navigation
    $("#mobile-nav-close-btn, #mobile-nav a").click(function() {
        
        $("#mobile-nav").css("height", "0%");
        
    });
    
});



$(document).ready(function () {
	$('#tableData').paging({
		limit: 6
	});
});
