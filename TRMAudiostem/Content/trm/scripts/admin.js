/// <reference path="../../../Scripts/jquery-1.8.2.js" />

function ActivateArtist(userId, action) {
    var url = '/Admin/ActivateArtist';

    $.ajax({
        cache: false,
        type: "POST",
        async: true,
        data: { userId: userId },
        url: url,
        success: function (result) {
            alert('You have successfully activated this artist.');
            $("dt#" + userId + " > img").replaceWith("<img class='activated' src='/Content/themes/trm/images/activated.png' alt='Artist is active' title='Artist is active. Click to deactivate artist account' onclick='DeactivateArtist(" + userId + ");' />");
        },
        error: function (xhr) {
            alert(xhr.statusText);
        }
    });
}

function DeactivateArtist(userId, action) {
    var url = '/Admin/DeactivateArtist';

    $.ajax({
        cache: false,
        type: "POST",
        async: true,
        data: { userId: userId },
        url: url,
        success: function (result) {
            alert('You have successfully deactivated this artist.');
            $("dt#" + userId + " > img").replaceWith("<img class='deactivated' src='/Content/themes/trm/images/deactivated.png' alt='Artist is not active' title='Artist is not active. Click to activate artist account' onclick='ActivateArtist(" + userId + ");' />");
        },
        error: function (xhr) {
            alert(xhr.statusText);
        }
    });
}