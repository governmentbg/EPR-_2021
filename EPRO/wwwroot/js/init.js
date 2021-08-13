(function () {
    // DataTables global settings

    $.fn.dataTable.ext.buttons.io_excel = {
        extend: 'excel',
        text: '<i class="far fa-file-excel"></i>',
        titleAttr: 'Excel',
        className: 'btn-success',
        exportOptions: {
            "columns": "thead th:not(.noExport)"
        }
    };

    $.fn.dataTable.ext.buttons.io_pdf = {
        extend: 'collection',
        text: '<i class="far fa-file-pdf"></i>',
        titleAttr: 'Pdf',
        className: 'btn-danger',
        autoClose: true,
        buttons: [
            {
                extend: 'pdf',
                text: 'Портретно',
                exportOptions: {
                    "columns": "thead th:not(.noExport)"
                },
                orientation: 'portrait'
            },
            {
                extend: 'pdf',
                text: 'Пейзажно',
                exportOptions: {
                    "columns": "thead th:not(.noExport)"
                },
                orientation: 'landscape'
            },
        ]
    };

    $.fn.dataTable.ext.buttons.io_print = {
        extend: 'print',
        text: '<i class="fa fa-print"></i>',
        titleAttr: 'Печат',
        className: 'btn-primary',
        exportOptions: {
            "columns": "thead th:not(.noExport)"
        }
    };

    $.fn.dataTable.ext.buttons.io_colvis = {
        extend: 'colvis',
        text: '<i class="fa  fa-eye-slash"></i>',
        titleAttr: 'Видими Колони',
        className: 'btn-info'
    };

    $.extend(true, $.fn.dataTable.defaults, {
        "initComplete": function (settings, json) {
            initDataTablesSearch(settings);
        },
        dom: '<"row"<"col-md-8 dataTables_buttons"B<"custom-filter d-inline">><"col-md-4"f>>rtip',
        buttons: {
            dom: {
                button: {
                    tag: 'button',
                    className: 'btn btn-sm'
                },
                container: {
                    className: 'd-inline'
                }
            },
            buttons: ['pageLength', 'io_colvis', 'io_excel', 'io_pdf', 'io_print']
        },
        "lengthMenu": [
            [10, 25, 50, 100, -1],
            ['10 реда', '25 реда', '50 реда', '100 реда', 'Покажи всички']
        ],
        "bAutoWidth": false,
        "language": {
            "url": "/lib/adminlte/plugins/datatables/dataTables.bgBG.json"
        },
        filter: true,
        "bLengthChange": false,
        "serverSide": true,
        "processing": true,
        "paging": true,
        "pageLength": 20,
        "stateSave": true,
        "stateDuration": -1
    });

    $('.date-picker').datepicker({
        todayHighlight: true,
        autoclose: true,
        format: 'dd.mm.yyyy',
        language: 'bg-BG'
    });

    $('.datetime-picker').datetimepicker({
        format: 'DD.MM.YYYY HH:mm:SS',
        locale: 'bg-BG'
    });

    $('.date-range-picker').daterangepicker({
        locale: {
            direction: 'ltr',
            format: 'DD.MM.YYYY',
            separator: ' - ',
            applyLabel: 'Избери',
            cancelLabel: 'Отказ',
            weekLabel: 'С',
            customRangeLabel: 'Период',
            daysOfWeek: ["П", "В", "С", "Ч", "П", "С", "Н"],
            monthNames: ["Януари", "Февруари", "Март", "Април", "Май", "Юни", "Юли", "Август", "Септември", "Октомври", "Ноември", "Декември"],
            firstDay: moment.localeData().firstDayOfWeek()
        }
    });

    //$(".textarea").wysihtml5();
    $('.select2').select2();

    $(document).on('click', 'a.modal-loader', function () {
        let _url = $(this).data('modal-url');
        let _title = $(this).data('modal-title');
        let bigModal = $(this).hasClass('modal-big');
        requestContent(_url, null, function (html) {
            ShowModalDialog(_title, html, bigModal);
        });
        return false;
    });

    toastr.options = {
        timeOut: 6000,
        extendedTimeOut: 500,
        tapToDismiss: true
    };
})();


function SearchDataTable(searchQuery, initTable) {
    if (searchQuery.length > 2 || searchQuery === '') {
        initTable.DataTable().search(searchQuery).draw();
    }
}

function modalPopUpValidation(add) {
    var forms = document.getElementsByClassName('needs-validation');
    // Loop over forms and prevent submission
    var validation = Array.prototype.filter.call(forms, function (form) {
        form.addEventListener('submit', function (event) {
            if (form.checkValidity() === false) {
                event.preventDefault();
                event.stopPropagation();
            } else {
                add();
                event.preventDefault();
            }
            form.classList.add('was-validated');
        }, false);
    });
}


function requestModal(url, data, title) {
    requestContent(url, data, function (html) {
        ShowModalDialog(title, html);
    });
}