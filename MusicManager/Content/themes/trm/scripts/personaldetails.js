/// <reference path="../../../Scripts/jquery-1.8.2.js" />

function loadArtistDetailsForm() {
    var artistName = $("#ArtistName"),
      email = $("#Email"),
      website = $("#Website"),
      bio = $("#bio"),
      soundcloud = $("#SoundCloud"),
      facebook = $("#Facebook"),
      twitter = $("#Twitter"),
      myspace = $("#MySpace"),
      prs = $("#PRS"),
      creativecommonslicence = $("#CreativeCommonsLicence"),
      profileImagePath = $("#ProfileImagePath"),
      allFields = $([]).add(artistName).add(email).add(website).add(bio).add(soundcloud).add(facebook).add(twitter).add(myspace).add(prs).add(creativecommonslicence),
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

    function checkMediaAsset(o, n) {
        if (o == null && n.val() == null) {
            $("#ProfileImage").addClass("ui-state-error");
            updateTips("You must select a song to upload.");
            return false;
        } else {
            $("#ProfileImage").removeClass("ui-state-error");
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
            $("#artistGenreList").addClass("ui-state-error");
            updateTips("You must select at least one genre.");
        }
        else {
            $("#artistGenreList").removeClass("ui-state-error");
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

    function saveArtistDetails() {
        // validate the form before submitting it
        var bValid = true;
        var formCollection = $("#artistDetails").serialize();
        var input = $("#ProfileImage")[0].files[0];

        allFields.removeClass("ui-state-error");

        bValid = bValid && checkMediaAsset(input, profileImagePath);
        bValid = bValid && checkLength(artistName, "artist name", 2, 250);
        bValid = bValid && checkLength(email, "email", 2, 250);
        bValid = bValid && checkLength(bio, "biography", 1, 500);
        bValid = bValid && checkGenre(formCollection);

        if (bValid) {
            //check whether client browser fully supports all File API
            if (window.File && window.FileReader && window.FileList && window.Blob) {
                var data = { form: formCollection, fileStream: '', fileName: '', fileType: '' };

                if (input != null) {
                    var reader = new FileReader();

                    reader.onload = function (e) {
                        rawData = reader.result;
                        var b64 = rawData.split("base64,");
                        data = { form: formCollection, fileStream: b64[1], fileName: input.name, fileType: input.type };

                        makeAjaxCall(data);
                    }

                    reader.readAsDataURL(input);
                }                
                else {
                    makeAjaxCall(data);
                }
            }
            else {
                //Error for older unsupported browsers that doesn't support HTML5 File API
                alert("Please upgrade your browser, because your current browser lacks some new features we need!");
            }
        }
        else {
            $(".ui-state-error").first().focus();
        }
    }

    function makeAjaxCall(data) {
        $.ajax({
            cache: false,
            type: 'POST',
            async: true,
            dataType: "html",
            data: data,
            url: '/Account/ArtistDetails',
            data: data,
            success: function (html) {
                alert(html);
                loadView('profilePlaceholder', '/Account/ArtistDetails/');
                loadView('artist-profile', '/Account/ArtistProfile/');
            },
            error: function (xhr) {
                alert(xhr.statusText);
            }
        });
    }

    $("#save-artist-details").click(function (event) {
        saveArtistDetails();
        event.preventDefault();
    });
}