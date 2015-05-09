
function toggleContainer(container, e) {
    if ($("#" + container).is(":hidden")) {
        $("#" + e).attr('src', '/Content/themes/cloudplayer/images/collapse.png');
    }
    else {
        $("#" + e).attr('src', '/Content/themes/cloudplayer/images/expand.png');
    }

    $("#" + container).slideToggle();
};

function playSong(songId, url, albumCoverPath, songTitle, albumTitle, artistName, deleteCurrentPlaylist, position) {
    addCurrentSongDetails(albumCoverPath, songTitle, albumTitle, artistName);
    var state = makeSongSelected(songId, position);

    if (state != 'pause') {
        var $player = $('#artistSongPlayer')[0];

        if (state == 'play') {
            $player.src = url;
            $player.addEventListener("ended", playNextSong);
        }

        $player.play();
        if ($player.getAttribute('type') == 'advanced') {
            if (!countPlay(songId)) {
                $player.pause();
                alert("Oops. We cannot play this file at present. Please try again later. If the problem persists, please contact us at support@totalresolutionmusic.com!");
            }
        }
    }
}

function countPlay(songId) {
    var formCollection = $("#savePlaylist").serialize();
    var data = { form: formCollection, songId: songId };
    var valid = false;

    $.ajax({
        cache: false,
        type: 'POST',
        async: false,
        dataType: "html",
        url: '/CloudPlayer/SaveSongPlayCount',
        data: data,
        success: function (html) {
            if (html == "success") {
                valid = true;
            }
        },
        error: function (xhr) {
            alert(xhr.statusText);
        }
    });

    return valid;
}

function playNextSong() {
    var _playlist = document.getElementById("playlistSongs");
    var selected = _playlist.querySelector("li.selectedSong");
    if (selected && selected.nextSibling) {
        var $nextSibling = selected.nextSibling;
        $($nextSibling).find("img.play").trigger("click");
    }
}

function makeSongSelected(songId, position) {
    // if the current song's pause button is clicked, then pause but do not change its class and icon
    if ($("li#song_" + songId + ' > img.play').attr('src').indexOf('pause') > -1) {
        var $player = $('#artistSongPlayer')[0];
        $player.pause();
        $("li#song_" + songId + ' > img.play').attr('src', '/Content/themes/trm/images/play.png');
        $("li#playlistsong_" + songId + '_' + position + ' > img.play').attr('src', '/Content/themes/trm/images/play.png');

        return 'pause';
    }
    else if ($("li#song_" + songId + ' > img.play').attr('src').indexOf('play') > -1 && $("li#song_" + songId).attr('class').indexOf('selectedSong') > -1) {
        $("li#song_" + songId + ' > img.play').attr('src', '/Content/themes/cloudplayer/images/pause.png');
        $("li#playlistsong_" + songId + '_' + position + ' > img.play').attr('src', '/Content/themes/cloudplayer/images/pause.png');

        return 'paused';
    }
    else {
        // first remove selected class to all songs and change the icon back to play
        $("li.selectedSong > img.play").attr('src', '/Content/themes/trm/images/play.png');
        $("li.selectedSong").removeClass('selectedSong').addClass('notSelected');

        // then add the class to the current song and change the icon to pause
        $("li#song_" + songId).removeClass('notSelected').addClass('selectedSong');
        $("li#playlistsong_" + songId + '_' + position).removeClass('notSelected').addClass('selectedSong');

        $("li#song_" + songId + ' > img.play').attr('src', '/Content/themes/cloudplayer/images/pause.png');
        $("li#playlistsong_" + songId + '_' + position + ' > img.play').attr('src', '/Content/themes/cloudplayer/images/pause.png');

        return 'play';
    }
}

function addCurrentSongDetails(albumCoverPath, songTitle, albumTitle, artistName) {
    $("img#currentAlbumCover").attr('src', albumCoverPath);
    $("span#currentSongTitle").text(songTitle);
    $("span#currentAlbumTitle").text(albumTitle);
    $("span#currentArtistName").text(artistName);

    $("div#currentlyPlaying").slideDown();
}

function addSongToPlaylist(songId, streamingUrl, albumCoverPath, songTitle, albumTitle, artistName, deleteCurrentPlaylist) {
    var position = $("ul#playlistSongs > li").length;
    var playlistSong = '<li class="notSelected ui-state-default" id="playlistsong_' + songId + '_' + position + '"><input type="hidden" name="song' + songId + '" value="' + songId + '_' + position + '" /><span class="ui-icon ui-icon-arrowthick-2-n-s"></span><img src="/Content/themes/trm/images/play.png" title="Play now" alt="Play now" class="play" onclick="playSong(' + songId + ', \'' + streamingUrl + '\', \'' + albumCoverPath + '\', \'' + escapeCharacter(songTitle) + '\', \'' + albumTitle + '\', \'' + artistName + '\', ' + deleteCurrentPlaylist + ', ' + position + ')" />&nbsp;|&nbsp;<img src="/Content/themes/cloudplayer/images/removefromplaylist.png" title="Remove from playlist" alt="Remove" class="remove" onclick="removeSongFromPlaylist(' + songId + ', ' + position + ')" />&nbsp;|&nbsp;' + songTitle + ' by ' + artistName + '</li>'

    $("ul#playlistSongs").append(playlistSong);

    var savedPlaylist = savePlaylist(position == 0, songId, position, deleteCurrentPlaylist);
    if (savedPlaylist) {
        if (position == 0) {
            playSong(songId, streamingUrl, albumCoverPath, songTitle, albumTitle, artistName, position);
        }
    }
    else {
        cancelSongToPlaylist(songId, position);
    }
}

function cancelSongToPlaylist(songId, position) {
    $("#playlistsong_" + songId + "_" + position + "").remove();

    $('#playlist').text('New Playlist');
    $('#playlistName').val('');
}

function removeSongFromPlaylist(songId, position) {
    if ($("li#playlistsong_" + songId + '_' + position + ' > img.play').attr('src').indexOf('pause') > -1) {
        alert('This song cannot be removed while it is playing.');
    }
    else {
        if (confirm("Do you really want to remove this song from the playlist?")) {
            deleteSong(songId, $("#playlistId").val(), position);
        }
    }
}

function deleteSong(songId, playlistId, position) {
    var data = { songId: songId, playlistId: playlistId, position: position };

    $.ajax({
        cache: false,
        type: 'POST',
        async: false,
        dataType: "html",
        url: '/CloudPlayer/RemoveSongFromPlaylist',
        data: data,
        success: function (html) {
            if (html.indexOf("Warning") == -1) {
                $("#playlistsong_" + songId + "_" + position + "").remove();
            }
            else {
                alert(html);
            }
        },
        error: function (xhr) {
            alert(xhr.statusText);
        }
    });
}

function escapeCharacter(content) {
    var newString = '';
    for (var i = 0, len = content.length; i < len; i++) {
        if (content[i] == "'") {
            newString += "\\'";
        }
        else {
            newString += content[i];
        }
    }

    return newString;
}

function loadPlaylist(playlistId, playlistName) {
    // first clear currently loaded playlist
    $("ul#playlistSongs > li").remove();

    // then display the name of the playlist
    $('#playlist').text(playlistName);
    $('#playlistName').val(playlistName);
    $('#playlistId').val(playlistId);

    makePlaylistSelected(playlistId);

    var data = { playlistId: playlistId };

    $.ajax({
        cache: false,
        type: "GET",
        async: true,
        data: data,
        contentType: "application/json",
        dataType: "json",
        url: '/CloudPlayer/GetPlaylistSongs',
        success: function (result) {
            // jsonify the results
            var json = JSON.stringify(result);
            // parse the json so that we can iterate through it
            var songs = $.parseJSON(json);

            $.each(songs, function (key, val) {
                addPlaylistSong(val.SongId, val.SongTitle, val.AlbumCover, val.AlbumTitle, val.MediaAssetPath, val.ArtistName, true);
            });
        },
        error: function (xhr) {
            alert(xhr.statusText);
        }
    });
}

function addPlaylistSong(songId, songTitle, albumCoverPath, albumTitle, mediaAssetPath, artistName) {
    var position = $("ul#playlistSongs > li").length;
    var playlistSong = '<li class="notSelected ui-state-default" id="playlistsong_' + songId + '_' + position + '"><input type="hidden" name="song' + songId + '" value="' + songId + '_' + position + '" /><span class="ui-icon ui-icon-arrowthick-2-n-s"></span><img src="/Content/themes/trm/images/play.png" title="Play now" alt="Play now" class="play" onclick="playSong(' + songId + ', \'' + mediaAssetPath + '\', \'' + albumCoverPath + '\', \'' + escapeCharacter(songTitle) + '\', \'' + albumTitle + '\', \'' + artistName + '\', ' + false + ', ' + position + ')" />&nbsp;|&nbsp;<img src="/Content/themes/cloudplayer/images/removefromplaylist.png" title="Remove from playlist" alt="Remove" class="remove" onclick="removeSongFromPlaylist(' + songId + ', ' + position + ')" />&nbsp;|&nbsp;' + songTitle + ' by ' + artistName + '</li>'

    $("ul#playlistSongs").append(playlistSong);

    if (position == 0) {
        playSong(songId, mediaAssetPath, albumCoverPath, songTitle, albumTitle, artistName, false, position)
    }
}

function makePlaylistSelected(playlistId) {
    // first change icon to play for all playlists
    $("ul#userPlaylists > li > img").attr("src", "/Content/themes/trm/images/play.png");

    if ($("#userPlaylist_" + playlistId + " > img").attr('src').indexOf('play') > -1) {
        $("#userPlaylist_" + playlistId + " > img").attr("src", "/Content/themes/cloudplayer/images/pause.png");
    }
}

function savePlaylist(newPlaylist, songId, position, deleteCurrentPlaylist) {
    // first clear currently loaded playlist if we are loading a new playlist or if it is a new playlist
    if (deleteCurrentPlaylist) {
        $("ul#playlistSongs > li").remove();
    }

    var valid = true;
    if ($('#playlistName').val() == '') {
        var name = prompt('Please enter a name for this playlist!');
        if (name != null) {
            $('#playlist').text(name);
            $('#playlistName').val(name);
        }
    }

    var formCollection = $("#savePlaylist").serialize();
    var data = { form: formCollection, newPlaylist: newPlaylist };

    $.ajax({
        cache: false,
        type: 'POST',
        async: false,
        dataType: "html",
        url: '/CloudPlayer/SavePlaylist',
        data: data,
        success: function (html) {
            if (html.indexOf("Warning") > -1) {
                alert(html);
                valid = false;
            }
            else {
                $('#playlistId').val(extractPlaylistId(html));
                if (newPlaylist) {
                    alert(removePlaylistId(html));
                }
            }
        },
        error: function (xhr) {
            alert(xhr.statusText);
            valid = false;
        }
    });

    if (valid) {
        return true;
    }
    else {
        return false;
    }
}

function extractPlaylistId(str) {
    var index = str.indexOf('&');
    var playlistId = str.substr(index + 1);

    return playlistId;
}

function removePlaylistId(str) {
    var index = str.indexOf('&');
    var result = str.substr(0, index);

    return result;
}