$(function () {
    $('.epro-dtp').dtpckr();

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
            ['10 реда', '25 реда', '50 реда', '100 реда ', 'Покажи всички']
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

    $('.form-validate').on('submit', function (e) {
        $('[data-validate]').each(function () {
            var input = $(this);
            if (!input.length || input.prop('disabled') || input.prop('readonly')) {
                return true;
            }
            var value = input.val(),
                field = input.removeClass('error'),
                rules = $.extend(true, {}, field.data('validate')),
                errors = [],
                validator = new Validator(),
                i, rel, tmp;
            field.closest('div').find('.invalid-feedback').remove();
    
            if (rules && field.is(':visible')) {
                for (i in rules) {
                    if (rules.hasOwnProperty(i)) {
                        tmp = $.isArray(rules[i]) ? rules[i] : [ rules[i] ];
                        if (i.indexOf('Relation') !== -1) {
                            rel = input.closest('.validate-form').find(':input[name="'+rules[i][0]+'"]');
                            if (!rel.length) {
                                errors = errors.concat([rules[i][1] || '']);
                                continue;
                            } else {
                                tmp[0] = rel.val();
                                i = i.replace('Relation', '');
                            }
                        }
                        if (validator[i] && (i === 'required' || value !== '')) {
                            tmp.unshift(value);
                            errors = errors.concat(validator[i].apply(validator, tmp));
                        }
                    }
                }
            }
            if (errors.length) {
                e.preventDefault();
                errors = errors.filter(function (v) { return v !== ''; });
                if (errors.length) {
                    field.closest('div').append('<div class="invalid-feedback">' + errors.join('<br />') + '</div>');
                }
                field.addClass('error');
            }
        });
    });

    $('.select2').select2();

    //Main menu
    $("header button.menu").click(function () {
        if ($(".main-menu").hasClass('mobile-open')) $(this).removeClass('close');
        else $(this).addClass('close');
        $(".main-menu").toggleClass('mobile-open');
    });
});

//GO TO TOP::
setTimeout(function () {
    var viewport = $('html, body');
    //animate to top:
    $('.gototop').click(function () {
        viewport.animate({ scrollTop: 0 }, 1400);
    });
    //stop animation on scroll
    viewport.bind("scroll mousedown DOMMouseScroll mousewheel keyup", function (e) {
        viewport.stop();
    });
}, 1000);

//function refreshTable(dataTableID) {
//    $(dataTableID).DataTable().ajax.reload(null, true);
//    return true;
//}

//function JsonBGdate(value) {
//    if (!value) {
//        return '';
//    }
//    return moment(value).format("DD.MM.YYYY");
//}

//function fillCombo(items, combo, selected) {
//    var tmlp = '{{#each this}}<option value="{{value}}" {{#if selected}}selected="selected"{{/if}}>{{text}}</option>{{/each}}';
//    $(combo).html(HandlebarsToHtml(tmlp, setSetSelected(items, selected)));
//}
//function requestCombo(url, data, combo, selected, callback) {
//    requestGET_Json(url, data, function (items) {
//        fillCombo(items, combo, selected);
//        if (callback) {
//            callback(combo);
//        }
//    });
//}

//function requestGET_Json(url, data, callback) {
//    $.ajax({
//        type: 'GET',
//        async: true,
//        cache: false,
//        contentType: "application/json;charset=utf-8",
//        dataType: 'json',
//        url: url,
//        data: data,
//        success: function (data) {
//            if (callback) {
//                callback(data);
//            }
//        }
//    });
//}

////Преобразува handlebars template, който е съдържание в контейнер с подадено име
//function TemplateToHtml(countainer, data) {
//    var source = $(countainer).html();

//    return HandlebarsToHtml(source, data);
//}

////Преобразува handlebars template, 
//function HandlebarsToHtml(hbTemplate, data) {
//    var template = Handlebars.compile(hbTemplate);

//    return template(data);
//}


//function SearchDataTable(searchQuery, initTable) {
//    if (searchQuery.length > 2 || searchQuery === '') {
//        initTable.DataTable().search(searchQuery).draw();
//    }
//}