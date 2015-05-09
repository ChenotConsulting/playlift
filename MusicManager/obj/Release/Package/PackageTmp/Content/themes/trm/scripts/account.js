/// <reference path="../../../Scripts/jquery-1.8.2.js" />

CharacterCount = function (TextArea, FieldToCount) {
    var myField = document.getElementById(TextArea);
    var myLabel = document.getElementById(FieldToCount);
    if (!myField || !myLabel) { return false }; // catches errors
    var MaxChars = myField.maxLengh;
    if (!MaxChars) { MaxChars = myField.getAttribute('maxlength'); }; if (!MaxChars) { return false };
    var remainingChars = MaxChars - myField.value.length
    myLabel.innerHTML = "<em>" + remainingChars + " Characters Remaining of " + MaxChars + "</em>";
}

function toggleContainer(container) {
    $("#" + container).slideToggle();
};

function validateRoyalty(id) {
    var $prs = $("#prs");
    var $cc = $("#cc");

    if (id == "prs" && $prs.is(":checked")) {
        $cc.attr("disabled", "disabled");
    }
    else if (id == "prs" && $prs.not(":checked")) {
        $cc.removeAttr("disabled");
    }
    else if (id == "cc" && $cc.is(":checked")) {
        $prs.attr("disabled", "disabled");
    }
    else if (id == "cc" && $cc.not(":checked")) {
        $prs.removeAttr("disabled");
    }
}

function getAllGenres() {
    //var url = "http://wcf.totalresolutionmusic.com/TRMWCFWebServiceJson.svc/GetAllGenres";
    var url = "http://localhost:51935/TRMWCFWebServiceJson.svc/GetAllGenres";

    $.ajax({
        cache: false,
        type: "POST",
        async: true,
        dataType: "json",
        url: url,
        success: function (result) {
            // jsonify the results
            var json = JSON.stringify(result);
            // parse the json so that we can iterate through it
            var genres = $.parseJSON(json);

            $.each(genres, function (key, val) {
                $.each(val, function (k, v) {
                    $("#genreList").append('<li id="' + v.GenreId + '"><input type="checkbox" id="' + v.GenreId + '" name="genre" />&nbsp;' + v.Name + '</li>');
                });
            });
        },
        error: function (xhr) {
            alert(xhr.statusText);
        }
    });
}

function getGenreList(genreListUrl) {
    $.ajax({
        cache: false,
        type: "POST",
        async: true,
        dataType: "json",
        url: genreListUrl,
        success: function (result) {
            // jsonify the results
            var json = JSON.stringify(result);
            // parse the json so that we can iterate through it
            var songs = $.parseJSON(json);

            $.each(songs, function (key, val) {
                $.each(val, function (k, v) {
                    $('#genreList').append('<li id="' + v.GenreId + '"><input type="checkbox" id="' + v.GenreId + '" name="genre" />&nbsp;' + v.Name + '</li>');
                });
            });
        },
        error: function (xhr) {
            alert(xhr.statusText);
        }
    });
}

function loadView(container, url) {
    var $container = $("#" + container);
    $container.html('<img class="loader" src="/Content/themes/trm/images/loading.gif" alt="loading" />');

    $.ajax({
        cache: false,
        type: 'GET',
        async: true,
        dataType: "html",
        url: url,
        success: function (html) {
            $container.hide().html(html).slideDown(200);
        },
        error: function (xhr) {
            alert(xhr.statusText);
        }
    });

    return false;
}

function loadFullView(container, url) {
    var $container = $("#" + container);
    $container.html('<img class="loader" src="/Content/themes/trm/images/loading.gif" alt="loading" />');

    $.ajax({
        cache: false,
        type: 'GET',
        async: true,
        dataType: "html",
        url: url,
        success: function (html) {
            $container.hide().html(html).slideDown(200);
        },
        error: function (xhr) {
            alert(xhr.statusText);
        }
    });

    return false;
}

