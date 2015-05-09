
function addSongToPlaylist(songId, albumCoverUrl, songUrl, songTitle, songArtist) {
    $('#sortable').append(constructPlaylistItem(songId, albumCoverUrl, songUrl, songTitle, songArtist));
}

function constructPlaylistItem(id, albumCoverUrl, songUrl, songTitle, songArtist) {
    var openingDiv = '<div id="' + id + '" class="music-item-container" data-toggle="context" style="background-image: url(\'' + albumCoverUrl + '\')">';
    var musicDiv = '<div id="file_' + id + '" class="music-play" data-path="' + songUrl + '"></div>';
    var musicInfoOpeningDiv = '<div class="music-info">';
    var songDetails = '<div class="music-song">' + songTitle + '</div><small> by </small><div class="music-artist">' + songArtist + '</div>';
    var closingDiv1 = '</div>';
    var interactionDiv1 = '<div class="music-action music-artistProfile" data-toggle="artist-page" title="Artist\'s Page"><i class="fa fa-user"></i></div>';
    var interactionDiv2 = '<div class="music-action music-love" data-toggle="love-this" title="Love this track"><i class="fa fa-heart"></i></div>';
    var interactionDiv3 = '<div class="music-action music-removeFrom-currentPlaylist" data-toggle="remove-this" title="Remove from current playlist"><i class="fa fa-times-circle"></i></div>';
    var interactionDiv4 = '<div class="music-action music-addTo-currentPlaylist" data-toggle="add-this" title="Add to current playlist"><i class="fa fa-plus-circle"></i></div>';
    var interactionDiv5 = '<div class="glare"></div>';
    var closingDiv2 = '</div>';

    return openingDiv + musicDiv + musicInfoOpeningDiv + songDetails + closingDiv1 + interactionDiv1 + interactionDiv2 + interactionDiv3 + interactionDiv4 + interactionDiv5 + closingDiv2;
}



//<div id="001" class="music-item-container" data-toggle="context" style="background-image: url('https://s3-eu-west-1.amazonaws.com/trmstreamtest/Sean_Taylor/Love_Against_Death/SeanTaylor-Chase_the_Night.jpg')">
//<div id="file_001" class="music-play" data-path="http://s3-eu-west-1.amazonaws.com/trmstreamtest/Sean_Taylor/Love_Against_Death/STAND_UP_128.mp3"></div>
//<div class="music-info">

//<div class="music-song">Stand Up</div>
//<small>by </small>
//<div class="music-artist">Sean Taylor</div>

//</div>
//<!-- tile interaction -->
//<div class="music-action music-artistProfile" data-toggle="artist-page" title="Artist's Page"><i class="fa fa-user"></i></div>
//<div class="music-action music-love" data-toggle="love-this" title="Love this track"><i class="fa fa-heart"></i></div>
//<div class="music-action music-removeFrom-currentPlaylist" data-toggle="remove-this" title="Remove from current playlist"><i class="fa fa-times-circle"></i></div>
//<div class="music-action music-addTo-currentPlaylist" data-toggle="add-this" title="Add to current playlist"><i class="fa fa-plus-circle"></i></div>
//<!-- tile interaction -->
//<div class="glare"></div>
//</div>