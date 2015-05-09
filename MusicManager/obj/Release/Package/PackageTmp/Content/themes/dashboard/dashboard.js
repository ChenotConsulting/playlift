
function applyDataTableToDashboard(container, orderable) {
    $('#' + container).DataTable({
        "columnDefs": [
          { "orderable": orderable, "targets": 0 }
        ]
    });
}

function applySongTimesDataTableToDashboard(container) {
    $('#' + container).DataTable({
        "columnDefs": [
          {
              "orderable": true, "targets": 0
          }
        ]
    });
}

function applyArtistPlaysDataTableToDashboard(container) {
    $('#' + container).DataTable({
        "columnDefs": [
          { "orderable": false, "targets": 0 },
          { "orderable": false, "targets": 3 }
        ]
    });
}

function applyBasicDataTableToDashboard(container) {
    $('#' + container).dataTable({
        "paging": false,
        "ordering": false,
        "searching": false,
        "info": false
    });
}

function loadSongTimesView(container, url, songId) {
    var $container = $("#" + container);
    $container.html('<img class="loader" src="/Content/themes/trm/images/loading.gif" alt="loading" />');

    $.ajax({
        cache: false,
        type: 'GET',
        async: true,
        dataType: "html",
        data: {songId: songId},
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

function loadArtistPlaysView(container, url, userId, artistName) {
    var $container = $("#" + container);
    $container.html('<img class="loader" src="/Content/themes/trm/images/loading.gif" alt="loading" />');

    $.ajax({
        cache: false,
        type: 'GET',
        async: true,
        dataType: "html",
        data: { userId: userId, artistName: artistName },
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
