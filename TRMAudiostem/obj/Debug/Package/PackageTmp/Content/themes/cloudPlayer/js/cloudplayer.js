
function toggleContainer(container, e) {
    if ($("#" + container).is(":hidden")) {
        $("#" + e).attr('src', '/Content/themes/cloudplayer/images/collapse.png');
    }
    else {
        $("#" + e).attr('src', '/Content/themes/cloudplayer/images/expand.png');
    }

    $("#" + container).slideToggle();
};

function playSong(songId, url, albumCoverPath, songTitle, albumTitle, artistName) {
    addCurrentSongDetails(albumCoverPath, songTitle, albumTitle, artistName);
    var result = makeSongSelected(songId);

    if (result != 'pause') {
        var $player = $('#artistSongPlayer')[0];

        if (result == 'play') {
            $player.src = url;
            $player.addEventListener("ended", playNextSong);
        }

        $player.play();
    }
}

function playNextSong() {
    var _playlist = document.getElementById("playlistSongs");
    var selected = _playlist.querySelector("li.selectedSong");
    if (selected && selected.nextSibling) {
        var $nextSibling = selected.nextSibling;
        $($nextSibling).find("img.play").trigger("click");
    }
}

function makeSongSelected(songId) {
    // if the current song's pause button is clicked, then pause but do not change its class and icon
    if ($("li#song_" + songId + ' > img.play').attr('src').indexOf('pause') > -1) {
        var $player = $('#artistSongPlayer')[0];
        $player.pause();
        $("li#song_" + songId + ' > img.play').attr('src', '/Content/themes/trm/images/play.png');
        $("li#playlistsong_" + songId + ' > img.play').attr('src', '/Content/themes/trm/images/play.png');

        return 'pause';
    }
    else if ($("li#song_" + songId + ' > img.play').attr('src').indexOf('play') > -1 && $("li#song_" + songId).attr('class').indexOf('selectedSong') > -1) {
        $("li#song_" + songId + ' > img.play').attr('src', '/Content/themes/cloudplayer/images/pause.png');
        $("li#playlistsong_" + songId + ' > img.play').attr('src', '/Content/themes/cloudplayer/images/pause.png');

        return 'paused';
    }
    else {
        // first remove selected class to all songs and change the icon back to play
        $("li.selectedSong > img.play").attr('src', '/Content/themes/trm/images/play.png');
        $("li.selectedSong").removeClass('selectedSong').addClass('notSelected');

        // then add the class to the current song and change the icon to pause
        $("li#song_" + songId).removeClass('notSelected').addClass('selectedSong');
        $("li#playlistsong_" + songId).removeClass('notSelected').addClass('selectedSong');

        $("li#song_" + songId + ' > img.play').attr('src', '/Content/themes/cloudplayer/images/pause.png');
        $("li#playlistsong_" + songId + ' > img.play').attr('src', '/Content/themes/cloudplayer/images/pause.png');

        return 'play';
    }
}

function addCurrentSongDetails(albumCoverPath, songTitle, albumTitle, artistName) {
    $("img#currentAlbumCover").attr('src', albumCoverPath);
    $("span#currentSongTitle").text(songTitle);
    $("span#currentAlbumTitle").text(albumTitle);
    $("span#currentArtistName").text(artistName);

    //$("div#currentlyPlaying").slideUp('fast');
    $("div#currentlyPlaying").slideDown();
}

function addSongToPlaylist(songId, streamingUrl, albumCoverPath, songTitle, albumTitle, artistName) {
    var playlistSong = '<li class="notSelected ui-state-default" id="playlistsong_' + songId + '"><span class="ui-icon ui-icon-arrowthick-2-n-s"></span><img src="/Content/themes/trm/images/play.png" title="Play now" alt="Play now" class="play" onclick="playSong(' + songId + ', \'' + streamingUrl + '\', \'' + albumCoverPath + '\', \'' + escapeCharacter(songTitle) + '\', \'' + albumTitle + '\', \'' + artistName + '\')" />&nbsp;|&nbsp;<img src="/Content/themes/cloudplayer/images/removefromplaylist.png" title="Remove from playlist" alt="Remove" class="remove" onclick="removeSongFromPlaylist(' + songId + ')" />&nbsp;|&nbsp;' + songTitle + ' by ' + artistName + '</li>'

    $("ul#playlistSongs").append(playlistSong);

    if ($("ul#playlistSongs").is(":hidden")) {
        playSong(songId, streamingUrl, albumCoverPath, songTitle, albumTitle, artistName);
    }

    $("ul#playlistSongs").slideDown();
}

function removeSongFromPlaylist(songId) {
    if ($("li#playlistsong_" + songId + ' > img.play').attr('src').indexOf('pause') > -1) {
        alert('This song cannot be removed while it is playing.');
    }
    else {
        $("li#playlistsong_" + songId).remove();
    }
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