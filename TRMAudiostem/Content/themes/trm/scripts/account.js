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

function viewAlbum(container, url, data) {
    var $container = $("#" + container);
    $container.html('<img class="loader" src="/Content/themes/trm/images/loading.gif" alt="loading" />');
    var albumId = { albumId: data };

    $.ajax({
        cache: false,
        type: 'GET',
        async: true,
        dataType: "html",
        url: url,
        data: albumId,
        success: function (html) {
            $container.hide().html(html).slideDown(200);
        },
        error: function (xhr) {
            alert(xhr.statusText);
        }
    });

    return false;
}

function deleteAlbum(container, url, data, returnUrl) {
    if (window.confirm('Are you sure you want to delete this album?')) {
        var $container = $("#" + container);
        var albumId = { albumId: data };

        $.ajax({
            cache: false,
            type: 'GET',
            async: true,
            dataType: "html",
            url: url,
            data: albumId,
            success: function (html) {
                loadView(container, returnUrl);
            },
            error: function (xhr) {
                alert(xhr.statusText);
            }
        });
    }

    return false;
}

function loadAlbumUploadForm() {
    // set release date to date picker
    $("#AlbumReleaseDate").datepicker();

    var albumTitle = $("#AlbumTitle"),
      albumReleaseDate = $("#AlbumReleaseDate"),
      albumProducer = $("#AlbumProducer"),
      albumLabel = $("#AlbumLabel"),
      albumCoverPath = $("#AlbumCoverPath"),
      input = $("#AlbumCover")[0].files[0];
      allFields = $([]).add(albumTitle).add(albumReleaseDate).add(albumProducer).add(albumLabel).add(input),
      tips = $(".validateTips");

    function updateTips(t) {
        tips
          .text(t)
          .addClass("ui-state-highlight");
        setTimeout(function () {
            tips.removeClass("ui-state-highlight", 1500);
        }, 500);
    }

    function checkLength(o, n, min, max) {
        if (o.val().length > max || o.val().length < min) {
            o.addClass("ui-state-error");
            updateTips("Length of " + n + " must be between " +
              min + " and " + max + ".");
            return false;
        } else {
            return true;
        }
    }

    function checkAlbumCover(o, n) {
        if (o == null && n.val() == null) {
            $("#AlbumCover").addClass("ui-state-error");
            updateTips("You must select an album cover.");
            return false;
        } else {
            return true;
        }
    }

    function checkRegexp(o, regexp, n) {
        if (!(regexp.test(o.val()))) {
            o.addClass("ui-state-error");
            updateTips(n);
            return false;
        } else {
            return true;
        }
    }

    $("#dialog-form").dialog({
        autoOpen: false,
        height: 500,
        width: 450,
        modal: true,
        buttons: {
            "Save Album Details": function () {
                // validate the form before submitting it
                var bValid = true;

                allFields.removeClass("ui-state-error");

                bValid = bValid && checkAlbumCover(input, albumCoverPath);
                bValid = bValid && checkLength(albumTitle, "album title", 2, 250);
                bValid = bValid && checkRegexp(albumReleaseDate, /^(0?\d|1[012])\/([012]?\d|3[01])\/(\d{2}|\d{4})$/i, "The release date must be in the format mm/dd/yyyy.");
                bValid = bValid && checkLength(albumProducer, "album producer", 2, 250);
                bValid = bValid && checkLength(albumLabel, "album label", 2, 250);

                if (bValid) {
                    //check whether client browser fully supports all File API
                    if (window.File && window.FileReader && window.FileList && window.Blob) {                      

                        var reader = new FileReader();

                        reader.onload = function (e) {
                            rawData = reader.result;
                            var b64 = rawData.split("base64,");

                            var formCollection = $("#editAlbum").serialize();
                            var data = { form: formCollection, fileStream: b64[1], fileName: input.name, fileType: input.type };

                            $.ajax({
                                cache: false,
                                type: 'POST',
                                async: true,
                                dataType: "html",
                                //data: data,
                                url: '/Account/EditAlbum',
                                data: data,
                                success: function (html) {
                                    alert(html);
                                },
                                error: function (xhr) {
                                    alert(xhr.statusText);
                                }
                            });

                            $(this).dialog("close");
                        }

                        reader.readAsDataURL(input);
                    }
                    else {
                        //Error for older unsupported browsers that doesn't support HTML5 File API
                        alert("Please upgrade your browser, because your current browser lacks some new features we need!");
                    }
                }
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        },
        close: function () {
            loadView('profilePlaceholder', '/Account/ArtistAlbums/');
        }
    });

    $("#new-album")
    .click(function () {
        $("#dialog-form").dialog("open");
    });
}

function showUploadedItem(source) {
    var list = document.getElementById("image-list"),
        li = document.createElement("li"),
        img = document.createElement("img");
    img.src = source;
    li.appendChild(img);
    list.appendChild(li);
}

function uploadAlbum(container, returnUrl, url, action, data) {
    var $container = $("#" + container);
    $container.html('<img class="loader" src="/Content/themes/trm/images/loading.gif" alt="loading" />');

    $.ajax({
        cache: false,
        type: action,
        async: true,
        dataType: "html",
        url: url,
        data: data,
        success: function (html) {
            $container.hide().html(html).slideDown(200);
        },
        error: function (xhr) {
            alert(xhr.statusText);
        }
    });

    if (returnUrl != '') {
        loadView(container, returnUrl);
    }

    return false;
}