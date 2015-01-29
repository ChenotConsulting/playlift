

function applyDataTableToDashboard(container) {
    $('#' + container).DataTable({
        "columnDefs": [
          { "orderable": false, "targets": 0 }
        ]
    });
}