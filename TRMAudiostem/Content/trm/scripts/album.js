/// <reference path="../../../Scripts/jquery-1.8.2.js" />

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
            type: 'POST',
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
    $("#AlbumReleaseDate").datepicker({
        changeMonth: true,
        changeYear: true
    });

    var albumTitle = $("#AlbumTitle"),
      albumReleaseDate = $("#AlbumReleaseDate"),
      albumProducer = $("#AlbumProducer"),
      albumLabel = $("#AlbumLabel"),
      albumCoverPath = $("#AlbumCoverPath"),
      allFields = $([]).add(albumTitle).add(albumReleaseDate).add(albumProducer).add(albumLabel),
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
            $("#AlbumCover").removeClass("ui-state-error");
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
            $("#genreList").addClass("ui-state-error");
            updateTips("You must select at least one genre.");
        }
        else {
            $("#genreList").removeClass("ui-state-error");
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
        switch (type) {
            case 'image/jpeg':
            case 'image/png':
            case 'image/gif':
                return true;
            default:
                return false;
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
                var formCollection = $("#editAlbum").serialize();
                var input = $("#AlbumCover")[0].files[0];

                allFields.removeClass("ui-state-error");

                bValid = bValid && checkAlbumCover(input, albumCoverPath);
                bValid = bValid && checkLength(albumTitle, "album title", 2, 250);
                bValid = bValid && checkRegexp(albumReleaseDate, /^(0?\d|1[012])\/([012]?\d|3[01])\/(\d{2}|\d{4})$/i, "The release date must be in the format mm/dd/yyyy.");
                bValid = bValid && checkLength(albumProducer, "album producer", 2, 250);
                bValid = bValid && checkLength(albumLabel, "album label", 2, 250);
                bValid = bValid && checkGenre(formCollection);
                bValid = bValid && checkFileType(input.type);

                if (bValid) {
                    // update the upload button to indicate some activity
                    var $uploadButton = $(".ui-dialog-buttonset").find('button').first();
                    $uploadButton.html('<span class="ui-button-text">Uploading...</span>');
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
                                url: '/Account/EditAlbum',
                                data: data,
                                success: function (html) {
                                    alert(html);
                                    $uploadButton.html('<span class="ui-button-text">Save Album Details</span>');
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
            loadView('profilePlaceholder', '/Account/ArtistAlbums/');
        }
    });

    $("#new-album")
    .click(function () {
        $("#dialog-form").dialog("open");
    });
}