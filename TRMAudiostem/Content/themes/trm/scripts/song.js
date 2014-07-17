/// <reference path="../../../Scripts/jquery-1.8.2.js" />

function deleteSong(container, url, songId, albumId, returnUrl) {
    if (window.confirm('Are you sure you want to delete this song?')) {
        var $container = $("#" + container);
        var data = { songId: songId, albumId: albumId };

        $.ajax({
            cache: false,
            type: 'POST',
            async: true,
            dataType: "html",
            url: url,
            data: data,
            success: function (html) {
                viewAlbum(container, returnUrl, albumId);
            },
            error: function (xhr) {
                alert(xhr.statusText);
            }
        });
    }

    return false;
}

function loadSongUploadForm() {
    // set release date to date picker
    $("#SongReleaseDate").datepicker({
        changeMonth: true,
        changeYear: true
    });

    var songTitle = $("#SongTitle"),
      songReleaseDate = $("#SongReleaseDate"),
      songComposer = $("#SongComposer"),
      mediaAssetPath = $("#MediaAssetPath"),
      allFields = $([]).add(songTitle).add(songReleaseDate).add(songComposer),
      tips = $(".validateTips");

    var albumId = $("#AlbumId").val();

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

    function checkMediaAsset(o, n) {
        if (o == null && n.val() == null) {
            $("#MediaAsset").addClass("ui-state-error");
            updateTips("You must select a song to upload.");
            return false;
        } else {
            $("#MediaAsset").removeClass("ui-state-error");
            return true;
        }
    }

    function checkGenre(o) {
        var formCollection = o.split('&');
        var bvalid = false;

        for (var i = 0; i < formCollection.length; i++) {
            if (formCollection[i].indexOf('genre') == -1) {
                bvalid = false;

            } else {
                bvalid = true;
            }
        }

        if (bvalid == false) {
            $("#songGenreList").addClass("ui-state-error");
            updateTips("You must select at least one genre.");
        }
        else {
            $("#songGenreList").removeClass("ui-state-error");
        }

        return bvalid;
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

    function checkFileType(type) {
        switch(type) {
            case 'audio/x-m4a':
            case 'audio/x-mp3':
            case 'audio/x-aac':
            case 'audio/x-aiff':
            case 'audio/x-wav':
            case 'audio/x-flac':
            case 'audio/m4a':
            case 'audio/mp3':
            case 'audio/aac':
            case 'audio/aiff':
            case 'audio/wav':
            case 'audio/flac':
                $("#MediaAsset").removeClass("ui-state-error");

                return true;
            default:
                $("#MediaAsset").addClass("ui-state-error");
                updateTips("The file type for this song is not allowed. Please save it in one of the allowed formats and try again.");

                return false;
        }
    }

    $("#song-dialog-form").dialog({
        autoOpen: false,
        height: 500,
        width: 450,
        modal: true,
        buttons: {
            "Save Song Details": function () {
                // validate the form before submitting it
                var bValid = true;
                var formCollection = $("#editSong").serialize();
                var input = $("#MediaAsset")[0].files[0];

                allFields.removeClass("ui-state-error");

                bValid = bValid && checkMediaAsset(input, mediaAssetPath);
                bValid = bValid && checkLength(songTitle, "song title", 2, 250);
                bValid = bValid && checkRegexp(songReleaseDate, /^(0?\d|1[012])\/([012]?\d|3[01])\/(\d{2}|\d{4})$/i, "The release date must be in the format mm/dd/yyyy.");
                bValid = bValid && checkLength(songComposer, "song composer", 2, 250);
                bValid = bValid && checkGenre(formCollection);
                bValid = bValid && checkFileType(input.type);

                if (bValid) {
                    // update the upload button to indicate some activity
                    var $uploadButton = $(".ui-dialog-buttonset").find('button').first();
                    $uploadButton.html('<span class="ui-button-text">Uploading...</span>');
                    $uploadButton.text('Uploading...');
                    $uploadButton.attr('disabled', 'disabled');

                    //check whether client browser fully supports all File API
                    if (window.File && window.FileReader && window.FileList && window.Blob) {

                        var reader = new FileReader();

                        reader.onload = function (e) {
                            rawData = reader.result;
                            var b64 = rawData.split("base64,");
                            var data = { form: formCollection, fileStream: b64[1], fileName: input.name, fileType: input.type };

                            $.ajax({
                                cache: false,
                                type: 'POST',
                                async: true,
                                dataType: "html",
                                url: '/Account/EditSong',
                                data: data,
                                success: function (html) {
                                    alert(html);
                                    $uploadButton.html('<span class="ui-button-text">Save Album Details</span>');
                                    $uploadButton.text('Save Album Details');
                                    $uploadButton.removeAttr('disabled');
                                    $(this).dialog("close");
                                },
                                error: function (xhr) {
                                    alert(xhr.statusText);
                                    $uploadButton.html('<span class="ui-button-text">Save Album Details</span>');
                                    $uploadButton.removeAttr('disabled');
                                }
                            });
                        }

                        reader.readAsDataURL(input);
                    }
                    else {
                        //Error for older unsupported browsers that doesn't support HTML5 File API
                        alert("Please upgrade your browser, because your current browser lacks some new features we need!");
                    }
                }
                else {
                    $(".ui-state-error").first().focus();
                }
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        },
        close: function () {
            viewAlbum('profilePlaceholder', '/Account/ViewAlbum', albumId);
        }
    });

    $("#new-song")
    .click(function () {
        $("#song-dialog-form").dialog("open");
    });
}