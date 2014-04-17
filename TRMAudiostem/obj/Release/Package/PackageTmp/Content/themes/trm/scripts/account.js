/// <reference path="../../../Scripts/jquery-1.8.2.js" />

function toggleContainer(container) {
    $("#" + container).slideToggle();
};

function showAndDisableControl(ctrlToShow, ctrlToUpdate, ctrlMessage) {
    $("." + ctrlToShow).show();
    $("." + ctrlToUpdate).val(ctrlMessage);
}

function playSong(url) {
    var $player = $('#artistSongPlayer')[0];

    $player.src = url;
    //$player.src = 'http://d1cwmr47wk7tco.cloudfront.net/Sean_Taylor/Love_Against_Death/STAND_UP_128.mp3';
    $player.play();
}

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