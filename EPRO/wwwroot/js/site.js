
function initDataTablesSearch(dtSettings) {
    // Search form events
    var initSearchForm = $('.search-form');

    //var initTable = $('.dataTable');
    var initWrapper = $(dtSettings.nTableWrapper);
    var initTable = $(dtSettings.nTable);

    if (initSearchForm.length > 0 && initTable.length > 0) {
        initSearchForm.on('submit', function () {
            var t = initTable.DataTable();
            t.state.clear();
        });
    }

    var secondCount = 0;
    var keysPressed = -1;
    var searchQuery = '';
    var timer = '';

    //var $searchInput = $('div.dataTables_filter input');
    var $searchInput = initWrapper.find('div.dataTables_filter input');
    $searchInput.unbind();
    $searchInput.bind('keyup', function (e) {
        if (this.value.length > 2 || this.value === '') {
            secondCount = 0;
        }
        keysPressed = this.value.length;
        searchQuery = this.value;
    });
    $searchInput.bind('keydown', function (e) {
        if (!timer) {
            timer = setInterval(function () {
                if (secondCount >= 1 && (keysPressed > 2 || keysPressed === 0)) {
                    keysPressed = -1;
                    SearchDataTable(searchQuery, initTable);
                    clearInterval(timer);
                    timer = '';
                } else {
                    secondCount += 1;
                }
            }, 1000);
        }
    });
}

function JsonBGdate(value) {
    if (!value) {
        return '';
    }

    return moment(value).format("DD.MM.YYYY");
}

function JsonBGdatetime(value) {
    if (!value) {
        return '';
    }

    return moment(value).format("DD.MM.YYYY HH:mm:ss");
}

//Преобразува handlebars template, който е съдържание в контейнер с подадено име
function TemplateToHtml(countainer, data) {
    var source = $(countainer).html();

    return HandlebarsToHtml(source, data);
}

//Преобразува handlebars template, 
function HandlebarsToHtml(hbTemplate, data) {
    var template = Handlebars.compile(hbTemplate);

    return template(data);
}

Handlebars.registerHelper('eachData', function (context, options) {
    var fn = options.fn, inverse = options.inverse, ctx;
    var ret = "";

    if (context && context.length > 0) {
        for (var i = 0, j = context.length; i < j; i++) {
            ctx = Object.create(context[i]);
            ctx.index = i;
            ret = ret + fn(ctx);
        }
    } else {
        ret = inverse(this);
    }
    return ret;
});

Handlebars.registerHelper("math", function (lvalue, operator, rvalue, options) {
    lvalue = parseFloat(lvalue);
    rvalue = parseFloat(rvalue);

    return {
        "+": lvalue + rvalue
    }[operator];
});

Handlebars.registerHelper("date", function (date) {
    dateValue = date;

    return moment(dateValue).format("DD.MM.YYYY");
})

Handlebars.registerHelper("dateTime", function (date) {
    dateValue = date;

    return moment(dateValue).format("DD.MM.YYYY HH:mm:ss");
})

// Показва съобщения от JS
var messageHelper = (function () {
    function ShowErrorMessage(message) {
        toastr.error(message);
    }

    function ShowSuccessMessage(message) {
        toastr.success(message);
    }

    function ShowWarning(message) {
        toastr.warning(message);
    }

    return {
        ShowErrorMessage: ShowErrorMessage,
        ShowSuccessMessage: ShowSuccessMessage,
        ShowWarning: ShowWarning
    };
})();

function refreshTable(dataTableID) {
    $(dataTableID).DataTable().ajax.reload(null, true);
    return true;
}

function fillCombo(items, combo, selected) {
    var tmlp = '{{#each this}}<option value="{{value}}" {{#if selected}}selected="selected"{{/if}}>{{text}}</option>{{/each}}';
    $(combo).html(HandlebarsToHtml(tmlp, setSetSelected(items, selected)));
}
function requestCombo(url, data, combo, selected, callback) {
    requestGET_Json(url, data, function (items) {
        fillCombo(items, combo, selected);
        if (callback) {
            callback(combo);
        }
    });
}
function setSetSelected(items, selected) {
    if (items && (selected !== undefined)) {
        for (var i = 0; i < items.length; i++) {
            if (items[i].value === selected.toString()) {
                items[i].selected = true;
            }
        }
    }

    return items;
}

function postContent(url, data, callback) {
    $.ajax({
        type: 'POST',
        async: true,
        cache: false,
        url: url,
        data: data,
        success: function (data) {
            if (callback) {
                callback(data);
            }
        }
    });
}

//Зарежда съдържанието на резултата от PartialView в div-елемент
function requestContent(url, data, callback) {

    $.ajax({
        type: 'GET',
        //async: true,
        cache: false,
        url: url,
        data: data,
        success: function (data) {
            callback(data);
        }
    });
}

function requestGET_Json(url, data, callback) {
    $.ajax({
        type: 'GET',
        async: true,
        cache: false,
        contentType: "application/json;charset=utf-8",
        dataType: 'json',
        url: url,
        data: data,
        success: function (data) {
            if (callback) {
                callback(data);
            }
        }
    });
}

function swalOk(text, callback) {
    swal({
        text: text,
        icon: "warning"
    })
        .then((result) => {
            if (result) {
                callback();
            } else {
                return false;
            }
        });
}
function swalSubmit(sender, text) {
    swal({
        title: 'Потвърди',
        text: text,
        icon: "warning",
        buttons: ["Отказ", "Потвърди"]
        //dangerMode: true
    })
        .then((result) => {
            if (result) {
                $(sender).parents('form:first').trigger('submit');
            } else {
                return false;
            }
        });
}
function swalConfirm(text, callback, cancelCallback) {
    swal({
        title: 'Потвърди',
        text: text,
        icon: "warning",
        buttons: ["Отказ", "Потвърди"]
        //dangerMode: true
    })
        .then((result) => {
            if (result) {
                callback();
            } else if (cancelCallback) {
                cancelCallback();
            } else {
                return false;
            }
        });
}
function swalConfirmClick(e, sender, text) {
    if ($(sender).data('confirmed') === true) {
        return true;
    }
    swal({
        title: 'Потвърди',
        text: text,
        icon: "warning",
        buttons: ["Отказ", "Потвърди"]
        //dangerMode: true
    })
        .then((result) => {
            if (result) {
                $(sender).data('confirmed', true);
                $(sender).trigger('click');
            } else {
                return false;
            }
        });

    e.preventDefault();
    return false;
}

function checkFilterFormHasData(filterContainer, minFilledCount) {
    if (!minFilledCount) {
        minFilledCount = 1;
    }
    let filledCount = 0;
    $(filterContainer).find('input[type="text"],input[type="number"]').each(function (i, e) {
        if ($(e).val().length > 0 && $(e).val() != '0') {
            filledCount++;
        }
    });
    $(filterContainer).find('input.ui-autocomplete-input').parent().find('input[type="hidden"]').each(function (i, e) {
        if ($(e).val().length > 0 && $(e).val() != '0') {
            filledCount++;
        }
    });
    $(filterContainer).find('select').each(function (i, e) {
        if ($(e).val().length > 0 && $(e).val() > 0) {
            filledCount++;
        }
    });
    return filledCount >= minFilledCount;
}
