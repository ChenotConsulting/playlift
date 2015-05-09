
$(document).ready(function () {
    $("ul.navigation > li > a").bind("click", function () {
        $("ul.navigation > li > a").removeClass('selected');
        $(this).addClass('selected');
    });    
});