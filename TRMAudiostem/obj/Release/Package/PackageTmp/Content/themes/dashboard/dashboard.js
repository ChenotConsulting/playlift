

function applyDataTableToDashboard(container, orderable) {
    $('#' + container).DataTable({
        "columnDefs": [
          { "orderable": orderable, "targets": 0 }
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
