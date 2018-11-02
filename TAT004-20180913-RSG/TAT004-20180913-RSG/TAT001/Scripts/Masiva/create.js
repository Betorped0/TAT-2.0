//LEEMOS EL ARCHIVO UNA VEZ CARGADO AL ELEMENTO FILE INPUT
$("#miMas").change(function () {
    var filenum = $('#miMas').get(0).files.length;
    if (filenum > 0) {
        var file = document.getElementById("miMas").files[0];
        var filename = file.name;
        //EVALUAMOS LA EXTENSION PARA VER QUE SOLO PERMITA FORMATOS DE EXCEL
        if (evaluarExt(filename)) {
            document.getElementById("loader").style.display = "initial";
            M.toast({ html: 'Cargando ' + filename });
            getExcelMasivas(file);
            procesarHoja1();
            var err = erroresH1();
            if (err.length <= 0) {
                procesarHoja2();
                procesarHoja3();
                procesarHoja4();
                procesarHoja5();
                checkRelacionada();
                var kk = checkRelacionadaMat();
                //checkRelMul()
            }
            else {
                for (var i = 0; i < err.length; i++) {
                    M.toast({ html: 'Datos primarios con error en NUM_DOC: ' + err[i] });
                }
            }
            //var elem = document.querySelectorAll('.miSel');
            //var instance = M.Select.init(elem, []);
            //document.getElementById("loader").style.display = "none";
            clearErrors();
            document.getElementById("loader").style.display = "none";
        } else {
            M.toast({ html: 'Tipo de archivo incorrecto: ' + filename });
        }
    } else {
        M.toast({ html: 'Seleccione un archivo' });
    }
    //$("#miMas").val("");
    clearErrors();
});

function evaluarExt(filename) {
    var exts = ['xls', 'xlsx'];
    var get_ext = filename.split('.');
    get_ext = get_ext.reverse();

    if ($.inArray(get_ext[0].toLowerCase(), exts) > -1) {
        return true;
    } else {
        return false;
    }
}

function getExcelMasivas(file) {
    var formData = new FormData();
    formData.append("FileUpload", file);

    $.ajax({
        type: "POST",
        url: 'loadExcelMasiva',
        data: formData,
        contentType: false,
        processData: false,
        async: true
    }).fail(function () {
        alert("error");
    });
}

/////////////////////////////////////////////////////////HOJA 1 FUNCIONES Y ASIGNACIONES////////////////////////////////////////////////////////
function procesarHoja1() {
    var table = $('#tab_test1').DataTable({ language: { "url": "../Scripts/lang/" + ln + ".json" } });
    table.clear().draw();

    $.ajax({
        type: "POST",
        url: 'validaHoja1',
        dataType: "json",
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {

            if (data !== null || data !== "") {

                if (data.length > 1) {
                    var errores = data.pop();

                    //INICIO DEL CICLO FOR
                    $.each(data, function (i, dataj) {
                        //if (i % 2 == 0) {
                        //    var addedRow = addRowH1(table, dataj.NUM_DOC, dataj.TSOL_ID, dataj.GALL_ID, dataj.SOCIEDAD_ID, dataj.PAIS_ID, dataj.ESTADO, dataj.CIUDAD, dataj.CONCEPTO, dataj.NOTAS, dataj.PAYER_ID, dataj.PAYER_NOMBRE, dataj.CONTACTO_NOMBRE, dataj.CONTACTO_EMAIL, dataj.FECHAI_VIG, dataj.FECHAF_VIG, dataj.MONEDA_ID, dataj.VKORG, dataj.VTWEG);
                        //}
                        var addedRow = addRowH1(table, dataj.NUM_DOC, dataj.TSOL_ID, dataj.GALL_ID, dataj.SOCIEDAD_ID, dataj.PAIS_ID, dataj.ESTADO, dataj.CIUDAD, dataj.CONCEPTO, dataj.NOTAS, dataj.PAYER_ID, dataj.PAYER_NOMBRE, dataj.CONTACTO_NOMBRE, dataj.CONTACTO_EMAIL, dataj.FECHAI_VIG, dataj.FECHAF_VIG, dataj.MONEDA_ID, dataj.VKORG, dataj.VTWEG, errores[i], dataj.PAIS_NAME);
                    }); //FIN DEL FOR

                    $('#tab_test1').css("font-size", "10px");
                    $('#tab_test1').css("display", "table");
                }
                else {
                    M.toast({ html: data });
                }
            }
        },
        complete: function (data) {
            //validarErrores("tab_test1");
        },
        error: function (xhr, httpStatusMessage, customErrorMessage) {
            alert("Request couldn't be processed. Please try again later. the reason " + xhr.status + " : " + httpStatusMessage + " : " + customErrorMessage);
        },
        async: false
    });
}

function addRowH1(t, NUM_DOC, TSOL_ID, GALL_ID, SOCIEDAD_ID, PAIS_ID, ESTADO, CIUDAD, CONCEPTO, NOTAS, PAYER_ID, PAYER_NOMBRE, CONTACTO_NOMBRE, CONTACTO_EMAIL, FECHAI_VIG, FECHAF_VIG, MONEDA_ID, VKORG, VTWEG, ERRORES, PAIS_NAME) {

    //var clasificacion = "<select id=\"clas\" class=\"miSel\">";
    //$.each(arr1, function (i, data) {
    //    if (data == GALL_ID) {
    //        clasificacion += "<option value=\"" + i + "\" selected>" + data + "</option>";
    //    }
    //    else {
    //        clasificacion += "<option value=\"" + i + "\">" + data + "</option>";
    //    }
    //});
    //clasificacion = clasificacion + "</select>";
    var icono, clase = null;

    if (jQuery.inArray('red white-text rojo', ERRORES) !== -1) {
        icono = 'close';
        clase = 'red white-text';
    }
    else {
        icono = 'done';
        clase = 'green white-text';
    }

    var r = t.row.add([
        "<span class='" + clase + " material-icons'>" + icono + "</span>",
        "<input class='" + ERRORES[0] + " input_numdoc' style='font-size:10px; text-align:center;' type='text' id='' name='' disabled value='" + NUM_DOC + "'><span hidden>" + NUM_DOC + "</span>",
        "<input class='" + ERRORES[1] + " input_tsol' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + TSOL_ID + "'><span hidden>" + TSOL_ID + "</span>",
        "<input class='" + ERRORES[2] + " input_clasificacion' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + GALL_ID + "'><span hidden>" + GALL_ID + "</span>",
        "<input class='" + ERRORES[3] + " input_sociedad' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + SOCIEDAD_ID + "'><span hidden>" + SOCIEDAD_ID + "</span>",
        //"<input class='" + ERRORES[4] + " input_pais' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + PAIS_ID + "'><span hidden class='span_pais'>" + PAIS_ID + "</span>",
        "<input class='" + ERRORES[4] + " input_pais' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + PAIS_NAME + "'><span hidden class='span_pais'>" + PAIS_ID + "</span>",
        "<input class='" + ERRORES[5] + " input_estado' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + ESTADO + "'><span hidden>" + ESTADO + "</span>",
        "<input class='" + ERRORES[6] + " input_ciudad' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + CIUDAD + "'><span hidden>" + CIUDAD + "</span>",
        "<input class='" + ERRORES[7] + " input_concepto' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + CONCEPTO + "'><span hidden>" + CONCEPTO + "</span>",
        "<input class='" + ERRORES[8] + " input_notas' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + NOTAS + "'><span hidden>" + NOTAS + "</span>",
        "<input class='" + ERRORES[9] + " input_cliente' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + PAYER_ID + "'><span hidden>" + PAYER_ID + "</span>",
        "<input class='" + ERRORES[10] + "' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + PAYER_NOMBRE + "'><span hidden>" + PAYER_NOMBRE + "</span>",
        "<input class='" + ERRORES[11] + " input_contacto' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + CONTACTO_NOMBRE + "'><span hidden>" + CONTACTO_NOMBRE + "</span>",
        "<input class='" + ERRORES[12] + " input_email' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + CONTACTO_EMAIL + "'><span hidden>" + CONTACTO_EMAIL + "</span>",
        "<input class='" + ERRORES[13] + " input_fechai' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + FECHAI_VIG + "'><span hidden>" + FECHAI_VIG + "</span>",
        "<input class='" + ERRORES[14] + " input_fechaf' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + FECHAF_VIG + "'><span hidden>" + FECHAF_VIG + "</span>",
        "<input class='" + ERRORES[15] + " input_moneda' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + MONEDA_ID + "'><span hidden>" + MONEDA_ID + "</span>",
        "<input id='txt_vkorg' value='" + VKORG + "' hidden>",
        "<input id='txt_vtweg' value='" + VTWEG + "' hidden>"
    ]).draw(false).node();

    var td = $(r).children()[0];
    $(td).css('text-align', 'center');

    return r;
}

$('#tab_test1').on('keydown.autocomplete', '.input_numdoc', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'numdoc',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    if (data) {
                        $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
                    } else {
                        $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
                    }
                    clearErrors();
                }
            });
        },

        change: function () {
            if ($(this).val() === "") {
                $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
            }
            else {
                if ($.isNumeric($(this).val())) {
                    $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
                }
            }
            clearErrors();
        }
    });
});

$('#tab_test1').on('keydown.autocomplete', '.input_tsol', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'tipoSol',
                dataType: "json",
                data: { "Prefix": request.term },

                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.ID, value: item.ID };
                    }));
                }
            });
        },

        messages: {
            noResults: '',
            results: function (resultsCount) { }
        },

        change: function (e, ui) {
            if (!(ui.item)) {
                $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
                clearErrors();
                e.target.value = "";
            }
        },

        select: function (event, ui) {
            $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
            clearErrors();
        }
    });
});

$('#tab_test1').on('keydown.autocomplete', '.input_clasificacion', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'clasificacion',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.DESCRIPCION, value: item.DESCRIPCION };
                    }));
                }
            });
        },

        messages: {
            noResults: '',
            results: function (resultsCount) { }
        },

        change: function (e, ui) {
            if (!(ui.item)) {
                $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
                clearErrors();
                e.target.value = "";
            }
        },

        select: function (event, ui) {
            $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
            clearErrors();
        }
    });
});

$('#tab_test1').on('keydown.autocomplete', '.input_sociedad', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();
    var us = $("#USUARIOC_ID").val(); //ADD RSG 01.11.2018

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'sociedad',
                dataType: "json",
                //data: { "Prefix": request.term },
                data: { "Prefix": request.term, user: us },//ADD RSG 01.11.2018
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.BUKRS, value: item.BUKRS };
                    }));
                }
            });
        },

        messages: {
            noResults: '',
            results: function (resultsCount) { }
        },

        change: function (e, ui) {
            if (!(ui.item)) {
                $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
                clearErrors();
                e.target.value = "";
            }
        },

        select: function (event, ui) {
            $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
            clearErrors();
        }
    });
});

$('#tab_test1').on('keydown.autocomplete', '.input_pais', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();
    var col_index2 = col_index - 1;

    $(tr.find("td:eq(" + col_index2 + ")").children().addClass('' + row_index + 'sociedad' + col_index2));
    var sociedad = $('.' + row_index + 'sociedad' + col_index2).val();
    //ESTO SIRVE
    //var table = $('#tab_test1').DataTable();
    //var fila = table.row(row_index).data();
    //var kk = $('#tab_test1 tr').eq(row_index).find('td').eq(col_index).children;

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'pais',
                dataType: "json",
                data: { "Prefix": request.term, "Sociedad": sociedad.toUpperCase() },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        //return { label: item.LANDX, value: item.LANDX };
                        return { label: item.LANDX, value: item.LAND };
                    }));
                }
            });
        },

        messages: {
            noResults: '',
            results: function (resultsCount) { }
        },

        change: function (e, ui) {
            if (!(ui.item)) {
                $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
                clearErrors();
                e.target.value = "";
            }
        },

        select: function (event, ui) {
            $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
            clearErrors();
        }
    });
});

$('#tab_test1').on('keydown.autocomplete', '.input_estado', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();
    var col_index2 = col_index - 1;

    $(tr.find("td:eq(" + col_index2 + ")").children().addClass('' + row_index + 'pais' + col_index2));
    var pais = $('.' + row_index + 'pais' + col_index2).val();

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'estado',
                dataType: "json",
                data: { "Prefix": request.term, "Pais": pais },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.NAME, value: item.NAME };
                    }));
                }
            });
        },

        messages: {
            noResults: '',
            results: function (resultsCount) { }
        },

        change: function (e, ui) {
            if (!(ui.item)) {
                $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
                clearErrors();
                e.target.value = "";
            }
        },

        select: function (event, ui) {
            $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
            clearErrors();
        }
    });
});

$('#tab_test1').on('keydown.autocomplete', '.input_ciudad', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();
    var col_index2 = col_index - 1;

    $(tr.find("td:eq(" + col_index2 + ")").children().addClass('' + row_index + 'estado' + col_index2));
    var estado = $('.' + row_index + 'estado' + col_index2).val();

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'ciudad',
                dataType: "json",
                data: { "Prefix": request.term, "Estado": estado },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.NAME, value: item.NAME };
                    }));
                }
            });
        },

        messages: {
            noResults: '',
            results: function (resultsCount) { }
        },

        change: function (e, ui) {
            if (!(ui.item)) {
                $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
                clearErrors();
                e.target.value = "";
            }
        },

        select: function (event, ui) {
            $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
            clearErrors();
        }
    });

});

$('#tab_test1').on('keydown.autocomplete', '.input_concepto', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'concepto',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    if (data) {
                        $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
                    } else {
                        $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
                    }
                    clearErrors();
                }
            });
        },

        change: function () {
            if ($(this).val() === "") {
                $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
            }
            clearErrors();
        }
    });
});

$('#tab_test1').on('keydown.autocomplete', '.input_notas', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'notas',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    if (data) {
                        $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
                    } else {
                        $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
                    }
                    clearErrors();
                }
            })
        }
    });
});

$('body').on('keydown.autocomplete', '.input_cliente', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();
    var col_index2 = col_index + 1;
    var numTabla = $(this).parents()[3];
    numTabla = $(numTabla).attr('id');
    var us = $("#USUARIOC_ID").val(); //ADD RSG 01.11.2018
    var pais = $(this).parent().parent().find(".span_pais").text(); //ADD RSG 01.11.2018
        //pais = $(this).parent().parent().find(".input_pais").val(); //ADD RSG 01.11.2018

    var defClase = null;

    if (numTabla === 'tab_test1') {
        defClase = 'clienteNumH1';
    } else if (numTabla === 'tab_test3') {
        defClase = 'clienteNumH3';
    }

    $(tr.find("td:eq(" + col_index + ")").children().addClass('' + row_index + defClase + col_index));
    $(tr.find("td:eq(" + col_index2 + ")").children().addClass('' + row_index + defClase + col_index2));
    var clase2 = '.' + row_index + defClase + col_index2;

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'cliente',
                dataType: "json",
                //data: { "Prefix": request.term },
                data: { "Prefix": request.term, "usuario": us, "pais": pais }, //ADD RSG 01.11.2018
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: trimStart('0', item.KUNNR) + '-' + item.NAME1, value: trimStart('0', item.KUNNR) };
                    }));
                }
                ,error: function (e, er) {
                    alert(e);
                }
            });
        },

        messages: {
            noResults: '',
            results: function (resultsCount) { }
        },

        change: function (e, ui) {
            if (!(ui.item)) {
                $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
                //$(tr.find("td:eq(" + col_index2 + ")").children().addClass("red white-text rojo"));
                $(tr.find("td:eq(" + col_index2 + ")").children().val(""));
                clearErrors();
                e.target.value = "";
            }
        },

        select: function (event, ui) {
            var label = ui.item.label;
            label = label.split('-');
            $(clase2).val(label[1]);
            $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
            //$(tr.find("td:eq(" + col_index2 + ")").children().removeClass("red white-text rojo"));
            clearErrors();
        }
    });
});

$('#tab_test1').on('keydown.autocomplete', '.input_contacto', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'contactoNombre',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    if (data) {
                        $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
                    } else {
                        $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
                    }
                    clearErrors();
                }
            });
        },

        change: function () {
            if ($(this).val() === "") {
                $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
            }
            clearErrors();
        }
    });
});

$('#tab_test1').on('keydown.autocomplete', '.input_email', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'contactoEmail',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    if (data) {
                        $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
                    } else {
                        $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
                    }
                    clearErrors();
                }
            });
        },

        change: function () {
            if ($(this).val() === "") {
                $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
            }
            clearErrors();
        }
    });
});

$('#tab_test1').on('keydown.autocomplete', '.input_fechai', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();
    var col_index2 = col_index + 1;

    var fecha2 = $(tr.find("td:eq(" + col_index2 + ")").children()).val();

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'validaFechaH1',
                dataType: "json",
                data: { "Fecha1": request.term, "Fecha2": fecha2 },
                success: function (data) {
                    if (data) {
                        $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
                        $(tr.find("td:eq(" + col_index2 + ")").children().removeClass("red white-text rojo"));
                    } else {
                        $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
                        $(tr.find("td:eq(" + col_index2 + ")").children().addClass("red white-text rojo"));
                    }
                    clearErrors();
                }
            });
        }
    });
});

$('#tab_test1').on('keydown.autocomplete', '.input_fechaf', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();
    var col_index2 = col_index - 1;

    var fecha1 = $(tr.find("td:eq(" + col_index2 + ")").children()).val();

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'validaFechaH1',
                dataType: "json",
                data: { "Fecha1": fecha1, "Fecha2": request.term },
                success: function (data) {
                    if (data) {
                        $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
                        $(tr.find("td:eq(" + col_index2 + ")").children().removeClass("red white-text rojo"));
                    } else {
                        $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
                        $(tr.find("td:eq(" + col_index2 + ")").children().addClass("red white-text rojo"));
                    }
                    clearErrors();
                }
            });
        }
    });
});

$('#tab_test1').on('keydown.autocomplete', '.input_moneda', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();
    var sociedad = tr.find('td:eq(4)').children().val();

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'moneda',
                dataType: "json",
                data: { "Prefix": request.term, "SociedadH1": sociedad },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.WAERS, value: item.WAERS };
                    }));
                }
            });
        },

        messages: {
            noResults: '',
            results: function (resultsCount) { }
        },

        change: function (e, ui) {
            if (!(ui.item)) {
                $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
                clearErrors();
                e.target.value = "";
            }
        },

        select: function (event, ui) {
            $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
            clearErrors();
        }
    });
});

$("#tab_info").click(function () {
    var tbody = $("#tab_test1 tbody");
    var arregloNumDoc = [];

    if (tbody.children().length !== 0) {
        arregloNumDoc = checkRelacionada();

        if (arregloNumDoc.length > 0) {
            var tablaH3 = $('#tab_test3').DataTable();

            for (var a = 0; a < tablaH3.rows().data().length; a++) {
                var rowH3 = tablaH3.row(a).node();
                var num_docH3 = $(rowH3).children().eq(1).children().val();

                if (jQuery.inArray(num_docH3, arregloNumDoc) !== -1) {
                    $(rowH3).children().eq(1).children().addClass("red white-text rojo");
                }
            }
        }
        checkRelacionadaMat();
    }
});

/////////////////////////////////////////////////////////HOJA 2 FUNCIONES Y ASIGNACIONES////////////////////////////////////////////////////////
function procesarHoja2() {
    var table = $('#tab_test2').DataTable({ language: { "url": "../Scripts/lang/" + ln + ".json" } });
    table.clear().draw();

    $.ajax({
        type: "POST",
        url: 'validaHoja2',
        dataType: "json",
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {

            if (data !== null || data !== "") {

                if (data.length > 1) {
                    var warnings = data.pop();
                    var errores = data.pop();

                    //INICIO DEL CICLO FOR
                    $.each(data, function (i, dataj) {

                        var addedRow = addRowH2(table, dataj.NUM_DOC, dataj.FACTURA, dataj.FECHA, dataj.PROVEEDOR, dataj.PROVEEDOR_NOMBRE, dataj.AUTORIZACION, dataj.VENCIMIENTO, dataj.FACTURAK, dataj.EJERCICIOK, errores[i], warnings[i]);

                    }); //FIN DEL FOR

                    $('#tab_test2').css("font-size", "10px");
                    $('#tab_test2').css("display", "table");
                }
                else {
                    M.toast({ html: data });
                }
            }
        },
        complete: function (data) {
            //validarErrores("tab_test2");
        },
        error: function (xhr, httpStatusMessage, customErrorMessage) {
            alert("Request couldn't be processed. Please try again later. the reason " + xhr.status + " : " + httpStatusMessage + " : " + customErrorMessage);
        },
        async: false
    });

    //Actualizar los valores en la tabla
    //updateTable();
}

function addRowH2(t, NUM_DOC, FACTURA, FECHA, PROVEEDOR, PROVEEDOR_NOMBRE, AUTORIZACION, VENCIMIENTO, FACTURAK, EJERCICIOK, ERRORES, WARNINGS) {
    var icono, clase = null;

    if (jQuery.inArray('red white-text rojo', ERRORES) !== -1) {
        icono = 'close';
        clase = 'red white-text';
    }
    else {
        icono = 'done';
        clase = 'green white-text';
    }

    var r = t.row.add([
        "<span class='" + clase + " material-icons'>" + icono + "</span>",
        "<input class='" + ERRORES[0] + WARNINGS[0] + " input_numdoc' style='font-size:12px; text-align:center;' type='text' id='' name='' disabled value='" + NUM_DOC + "'><span hidden>" + NUM_DOC + "</span>",
        "<input class='" + ERRORES[1] + WARNINGS[1] + " input_factura' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + FACTURA + "'><span hidden>" + FACTURA + "</span>",
        "<input class='" + ERRORES[2] + WARNINGS[2] + " input_fechaH2' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + FECHA + "'><span hidden>" + FECHA + "</span>",
        "<input class='" + ERRORES[3] + WARNINGS[3] + " input_proveedor' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + PROVEEDOR + "'><span hidden>" + PROVEEDOR + "</span>",
        "<input class='" + ERRORES[4] + WARNINGS[4] + "' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + PROVEEDOR_NOMBRE + "'><span hidden>" + PROVEEDOR_NOMBRE + "</span>",
        "<input class='" + ERRORES[5] + WARNINGS[5] + " input_autorizacion' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + AUTORIZACION + "'><span hidden>" + AUTORIZACION + "</span>",
        "<input class='" + ERRORES[6] + WARNINGS[6] + " input_fechaH2' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + VENCIMIENTO + "'><span hidden>" + VENCIMIENTO + "</span>",
        "<input class='" + ERRORES[7] + WARNINGS[7] + " input_facturak' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + FACTURAK + "'><span hidden>" + FACTURAK + "</span>",
        "<input class='" + ERRORES[8] + WARNINGS[8] + " input_ejerciciok' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + EJERCICIOK + "'><span hidden>" + EJERCICIOK + "</span>"
    ]).draw(false).node();

    var td = $(r).children()[0];
    $(td).css('text-align', 'center');

    return r;
}

$('#tab_test2').on('keydown.autocomplete', '.input_factura', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();

    var amarillo = $(tr.find("td:eq(" + col_index + ")").children());
    amarillo = $(amarillo).hasClass("yelloww");

    if (amarillo) {
        amarillo = "blue";
    }
    else {
        amarillo = "";
    }

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'factura',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    if (data) {
                        $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
                        $(tr.find("td:eq(" + col_index + ")").children().addClass(amarillo));
                    }
                    else {
                        $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
                        $(tr.find("td:eq(" + col_index + ")").children().removeClass(amarillo));
                    }
                    clearErrors();
                }
            });
        }
    });
});

$('#tab_test2').on('keydown.autocomplete', '.input_fechaH2', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();

    var amarillo = $(tr.find("td:eq(" + col_index + ")").children());
    amarillo = $(amarillo).hasClass("yelloww");

    if (amarillo) {
        amarillo = "blue";
    }
    else {
        amarillo = "";
    }

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'validaFechaH2',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    if (data) {
                        $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
                        $(tr.find("td:eq(" + col_index + ")").children().addClass(amarillo));
                    }
                    else {
                        $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
                        $(tr.find("td:eq(" + col_index + ")").children().removeClass(amarillo));
                    }
                    clearErrors();
                }
            });
        }
    });
});//APLICA PARA AMBAS FECHAS EN H2 FECHA FACTURA Y FECHA VENCIMIENTO

$('#tab_test2').on('keydown.autocomplete', '.input_proveedor', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();
    var col_index2 = col_index + 1;
    var num_docH1 = null;
    var sociedadH1 = null;
    var tablaH1 = $('#tab_test1').DataTable();

    var amarillo = $(tr.find("td:eq(" + col_index + ")").children());
    amarillo = $(amarillo).hasClass("yelloww");

    if (amarillo) {
        amarillo = "blue";
    }
    else {
        amarillo = "";
    }

    $(tr.find('td:eq(1)').children().addClass('' + row_index + 'numDoc1'));
    $(tr.find("td:eq(" + col_index + ")").children().addClass('' + row_index + 'proveedorId' + col_index));
    $(tr.find("td:eq(" + col_index2 + ")").children().addClass('' + row_index + 'proveedorNom' + col_index2));
    var num_doc = $('.' + row_index + 'numDoc1').val();
    var clase2 = '.' + row_index + 'proveedorNom' + col_index2;

    for (var a = 0; a < tablaH1.rows().data().length; a++) {
        var rowH1 = tablaH1.row(a).node();
        num_docH1 = $(rowH1).children().eq(1).children().val();

        if (num_docH1 === num_doc) {
            sociedadH1 = $(rowH1).find('td:eq(4)').children().val();
        }
    }

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'proveedor',
                dataType: "json",
                data: { "Prefix": request.term, "Sociedad": sociedadH1 },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: trimStart('0', item.ID) + '-' + item.NOMBRE, value: trimStart('0', item.ID) };
                    }));
                }
            });
        },

        messages: {
            noResults: '',
            results: function (resultsCount) { }
        },

        change: function (e, ui) {
            if (!(ui.item)) {
                $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
                $(tr.find("td:eq(" + col_index + ")").children().removeClass(amarillo));
                clearErrors();
                e.target.value = "";
            }
        },

        select: function (event, ui) {
            var label = ui.item.label;
            label = label.split('-');
            $(clase2).val(label[1]);
            $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
            $(tr.find("td:eq(" + col_index + ")").children().addClass(amarillo));
            clearErrors();
        }
    });
});

$('#tab_test2').on('keydown.autocomplete', '.input_autorizacion', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();

    var amarillo = $(tr.find("td:eq(" + col_index + ")").children());
    amarillo = $(amarillo).hasClass("yelloww");

    if (amarillo) {
        amarillo = "blue";
    }
    else {
        amarillo = "";
    }

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'autorizacion',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    if (data) {
                        $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
                        $(tr.find("td:eq(" + col_index + ")").children().addClass(amarillo));
                    }
                    else {
                        $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
                        $(tr.find("td:eq(" + col_index + ")").children().removeClass(amarillo));
                    }
                    clearErrors();
                }
            });
        }
    });
});

$('#tab_test2').on('keydown.autocomplete', '.input_facturak', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();

    var amarillo = $(tr.find("td:eq(" + col_index + ")").children());
    amarillo = $(amarillo).hasClass("yelloww");

    if (amarillo) {
        amarillo = "blue";
    }
    else {
        amarillo = "";
    }

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'facturak',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    if (data) {
                        $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
                        $(tr.find("td:eq(" + col_index + ")").children().addClass(amarillo));
                    }
                    else {
                        $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
                        $(tr.find("td:eq(" + col_index + ")").children().removeClass(amarillo));
                    }
                    clearErrors();
                }
            });
        }
    });
});

$('#tab_test2').on('keydown.autocomplete', '.input_ejerciciok', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();

    var amarillo = $(tr.find("td:eq(" + col_index + ")").children());
    amarillo = $(amarillo).hasClass("yelloww");

    if (amarillo) {
        amarillo = "blue";
    }
    else {
        amarillo = "";
    }

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'ejerciciok',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    if (data) {
                        $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
                        $(tr.find("td:eq(" + col_index + ")").children().addClass(amarillo));
                    } else {
                        $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
                        $(tr.find("td:eq(" + col_index + ")").children().removeClass(amarillo));
                    }
                    clearErrors();
                }
            });
        },

        change: function () {
            if ($(this).val() === "") {
                $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
                $(tr.find("td:eq(" + col_index + ")").children().removeClass(amarillo));
            }
            clearErrors();
        }
    });
});

$("#tab_rel").click(function () {
    var tbody = $("#tab_test2 tbody");
    var arregloNumDoc = [];

    if (tbody.children().length !== 0) {
        arregloNumDoc = checkRelacionada();

        if (arregloNumDoc.length > 0) {
            var tablaH3 = $('#tab_test3').DataTable();

            for (var a = 0; a < tablaH3.rows().data().length; a++) {
                var rowH3 = tablaH3.row(a).node();
                var num_docH3 = $(rowH3).children().eq(1).children().val();

                if (jQuery.inArray(num_docH3, arregloNumDoc) !== -1) {
                    $(rowH3).children().eq(1).children().addClass("red white-text rojo");
                }
            }
        }
        checkRelacionadaMat();
    }
});
/////////////////////////////////////////////////////////HOJA 3 FUNCIONES Y ASIGNACIONES////////////////////////////////////////////////////////
function procesarHoja3() {
    var table = $('#tab_test3').DataTable({ language: { "url": "../Scripts/lang/" + ln + ".json" } });
    table.clear().draw();

    $.ajax({
        type: "POST",
        url: 'validaHoja3',
        dataType: "json",
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {

            if (data !== null || data !== "") {

                if (data.length > 1) {
                    var errores = data.pop();

                    //INICIO DEL CICLO FOR
                    $.each(data, function (i, dataj) {
                        var addedRow = addRowH3(table, dataj.NUM_DOC, dataj.FACTURA, dataj.BILL_DOC, dataj.EJERCICIOK, dataj.PAYER, dataj.PAYER_NOMBRE, dataj.IMPORTE_FAC, dataj.BELNR, errores[i]);
                    }); //FIN DEL FOR

                    $('#tab_test3').css("font-size", "10px");
                    $('#tab_test3').css("display", "table");
                }
                else {
                    M.toast({ html: data });
                }
            }
        },
        complete: function (data) {
            //validarErrores("tab_test3");
        },
        error: function (xhr, httpStatusMessage, customErrorMessage) {
            alert("Request couldn't be processed. Please try again later. the reason " + xhr.status + " : " + httpStatusMessage + " : " + customErrorMessage);
        },
        async: false
    });

    //Actualizar los valores en la tabla
    //updateTable();
}

function addRowH3(t, NUM_DOC, FACTURA, BILL_DOC, EJERCICIOK, PAYER, PAYER_NOMBRE, IMPORTE_FAC, BELNR, ERRORES) {
    var icono, clase = null;

    if (jQuery.inArray('red white-text rojo', ERRORES) !== -1) {
        icono = 'close';
        clase = 'red white-text';
    }
    else {
        icono = 'done';
        clase = 'green white-text';
    }

    var r = t.row.add([
        "<span class='" + clase + " material-icons'>" + icono + "</span>",
        "<input class='" + ERRORES[0] + " input_numdoc' style='font-size:12px; text-align:center;' type='text' id='' name='' disabled value='" + NUM_DOC + "'><span hidden>" + NUM_DOC + "</span>",
        "<input class='" + ERRORES[1] + " input_facturaH3' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + FACTURA + "'><span hidden>" + FACTURA + "</span>",
        "<input class='" + ERRORES[2] + " input_bill' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + BILL_DOC + "'><span hidden>" + BILL_DOC + "</span>",
        "<input class='" + ERRORES[3] + " input_ejerciciokH3' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + EJERCICIOK + "'><span hidden>" + EJERCICIOK + "</span>",
        "<input class='" + ERRORES[4] + " input_cliente' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + PAYER + "'><span hidden>" + PAYER + "</span>",
        "<input class='" + ERRORES[5] + "' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + PAYER_NOMBRE + "'><span hidden>" + PAYER_NOMBRE + "</span>",
        "<input class='" + ERRORES[6] + " input_importe' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + IMPORTE_FAC + "'><span hidden>" + IMPORTE_FAC + "</span>",
        "<input class='" + ERRORES[7] + " input_belnr' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + BELNR + "'><span hidden>" + BELNR + "</span>"
    ]).draw(false).node();

    var td = $(r).children()[0];
    $(td).css('text-align', 'center');

    return r;
}

$('#tab_test3').on('keydown.autocomplete', '.input_facturaH3', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'factura',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    if (data) {
                        $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
                        clearErrors();
                        //validarErrores("tab_test2");
                    } else {
                        $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
                        clearErrors();
                        //validarErrores("tab_test2");
                    }
                }
            });
        },

        change: function () {
            if ($(this).val() == "") {
                $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
            }
            clearErrors();
        }
    });
});

$('#tab_test3').on('keydown.autocomplete', '.input_bill', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'bill',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    if (data) {
                        $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
                    } else {
                        $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
                    }
                    clearErrors();
                }
            });
        },

        change: function () {
            if ($(this).val() == "") {
                $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
            }
            clearErrors();
        }
    });
});

$('#tab_test3').on('keydown.autocomplete', '.input_ejerciciokH3', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'ejerciciok',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    if (data) {
                        $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
                        clearErrors();
                        //validarErrores("tab_test2");
                    } else {
                        $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
                        clearErrors();
                        //validarErrores("tab_test2");
                    }
                }
            });
        },

        change: function () {
            if ($(this).val() == "") {
                $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
            }
            clearErrors();
        }
    });
});

//PAYER Y NOMBRE PAYER APUNTA A INPUT CLIENTE EN ASIGNACIONES DE HOJA1

$('#tab_test3').on('keydown.autocomplete', '.input_importe', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();
    var num_docH1 = null;
    var pais = null;
    var tablaH1 = $('#tab_test1').DataTable();

    $(tr.find('td:eq(1)').children().addClass('' + row_index + 'numDoc3'));
    var num_doc = $('.' + row_index + 'numDoc3').val();

    for (var a = 0; a < tablaH1.rows().data().length; a++) {
        var rowH1 = tablaH1.row(a).node();
        num_docH1 = $(rowH1).children().eq(1).children().val();

        if (num_docH1 == num_doc) {
            pais = $(rowH1).find('td:eq(5)').children().val();
        }
    }

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'importe',
                dataType: "json",
                data: { "Prefix": request.term.replace('$', ''), "Pais": pais },
                success: function (data) {

                    var color = data.pop();

                    if (color) {
                        $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
                        $(tr.find("td:eq(" + col_index + ")").children().val(data));
                    } else {
                        $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
                    }
                    clearErrors();
                }
            });
        },

        change: function () {
            if ($(this).val() == "") {
                $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
            }
            clearErrors();
        }
    });
});

$('#tab_test3').on('keydown.autocomplete', '.input_belnr', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'belnr',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    if (data) {
                        $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
                    } else {
                        $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
                    }
                    clearErrors();
                }
            });
        },

        change: function () {
            if ($(this).val() == "") {
                $(tr.find("td:eq(" + col_index + ")").children().addClass("red white-text rojo"));
            }
            clearErrors();
        }
    });
});

$("#tab_mul").click(function () {
    var tbody = $("#tab_test3 tbody");
    var arregloNumDoc = [];

    if (tbody.children().length !== 0) {
        arregloNumDoc = checkRelacionada();

        if (arregloNumDoc.length > 0) {
            var tablaH3 = $('#tab_test3').DataTable();

            for (var a = 0; a < tablaH3.rows().data().length; a++) {
                var rowH3 = tablaH3.row(a).node();
                var num_docH3 = $(rowH3).children().eq(1).children().val();

                if (jQuery.inArray(num_docH3, arregloNumDoc) !== -1) {
                    $(rowH3).children().eq(1).children().addClass("red white-text rojo");
                }
            }
        }
        checkRelacionadaMat();
    }
});

/////////////////////////////////////////////////////////HOJA 4 FUNCIONES Y ASIGNACIONES////////////////////////////////////////////////////////
function procesarHoja4() {
    var table = $('#tab_test4').DataTable({ language: { "url": "../Scripts/lang/" + ln + ".json" } });
    table.clear().draw();

    $.ajax({
        type: "POST",
        url: 'validaHoja4',
        dataType: "json",
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {

            if (data !== null || data !== "") {

                if (data.length > 1) {
                    var errores = data.pop();

                    //INICIO DEL CICLO FOR
                    $.each(data, function (i, dataj) {
                        var addedRow = addRowH4(table, dataj.NUM_DOC, dataj.LIGADA, dataj.VIGENCIA_DE, dataj.VIGENCIA_AL, dataj.MATNR, dataj.MATKL, dataj.DESCRIPCION, dataj.MONTO, dataj.PORC_APOYO, dataj.APOYO_PIEZA, dataj.COSTO_APOYO, dataj.PRECIO_SUG, dataj.VOLUMEN_REAL, dataj.APOYO, errores[i]);
                    }); //FIN DEL FOR

                    $('#tab_test4').css("font-size", "10px");
                    $('#tab_test4').css("display", "table");
                }
                else {
                    M.toast({ html: data });
                }
            }
        },
        complete: function (data) {
            //validarErrores("tab_test4");
        },
        error: function (xhr, httpStatusMessage, customErrorMessage) {
            alert("Request couldn't be processed. Please try again later. the reason " + xhr.status + " : " + httpStatusMessage + " : " + customErrorMessage);
        },
        async: false
    });

    //Actualizar los valores en la tabla
    //updateTable();
}

function addRowH4(t, NUM_DOC, LIGADA, VIGENCIA_DE, VIGENCIA_AL, MATNR, MATKL, DESCRIPCION, MONTO, PORC_APOYO, APOYO_PIEZA, COSTO_APOYO, PRECIO_SUG, VOLUMEN_REAL, APOYO, ERRORES) {
    var check, bloqueo, icono, clase = null;

    if (LIGADA !== "") {
        check = "checked='checked'";
        bloqueo = "disabled";
    }
    else {
        check = "";
    }

    if (jQuery.inArray('red white-text rojo', ERRORES) !== -1) {
        icono = 'close';
        clase = 'red white-text';
    }
    else {
        icono = 'done';
        clase = 'green white-text';
    }

    var r = t.row.add([
        "<span class='" + clase + " material-icons'>" + icono + "</span>",
        "<input class='" + ERRORES[0] + " input_numdoc' style='font-size:12px; text-align:center;' type='text' id='' name='' disabled value='" + NUM_DOC + "'><span hidden>" + NUM_DOC + "</span>",
        "<p style='text-align:center;'><label><input type='checkbox' class='filled-in ligada' " + check + " onchange='ligada(this);'/><span></span></label></p>",
        "<input class='" + ERRORES[1] + " input_fechai' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + VIGENCIA_DE + "'><span hidden>" + VIGENCIA_DE + "</span>",
        "<input class='" + ERRORES[2] + " input_fechaf' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + VIGENCIA_AL + "'><span hidden>" + VIGENCIA_AL + "</span>",
        "<input class='" + ERRORES[3] + " input_material' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + MATNR + "'><span hidden>" + MATNR + "</span>",
        "<input class='" + ERRORES[4] + " input_categoria' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + MATKL + "'><span hidden>" + MATKL + "</span>",
        "<input class='" + ERRORES[5] + "' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + DESCRIPCION + "'><span hidden>" + DESCRIPCION + "</span>",
        "<input class='" + ERRORES[6] + " input_cantidades' style='font-size:10px; text-align:center;' type='text' id='' name='' " + bloqueo + " value='" + MONTO + "'><span hidden>" + MONTO + "</span>",
        "<input class='" + ERRORES[7] + " input_cantidades' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + PORC_APOYO + "'><span hidden>" + PORC_APOYO + "</span>",
        "<input class='" + ERRORES[8] + " input_cantidades' style='font-size:10px; text-align:center;' type='text' id='' name='' disabled value='" + APOYO_PIEZA + "'><span hidden>" + APOYO_PIEZA + "</span>",
        "<input class='" + ERRORES[9] + " input_cantidades' style='font-size:10px; text-align:center;' type='text' id='' name='' disabled value='" + COSTO_APOYO + "'><span hidden>" + COSTO_APOYO + "</span>",
        "<input class='" + ERRORES[10] + " input_cantidades' style='font-size:10px; text-align:center;' type='text' id='' name='' " + bloqueo + " value='" + PRECIO_SUG + "'><span hidden>" + PRECIO_SUG + "</span>",
        "<input class='" + ERRORES[11] + " input_cantidades' style='font-size:10px; text-align:center;' type='text' id='' name='' " + bloqueo + " value='" + VOLUMEN_REAL + "'><span hidden>" + VOLUMEN_REAL + "</span>",
        "<input class='" + ERRORES[12] + " input_cantidades input_apoyo' style='font-size:10px; text-align:center;' type='text' id='' name='' " + bloqueo + " value='" + APOYO + "'><span hidden>" + APOYO + "</span>"
    ]).draw(false).node();

    var td = $(r).children()[0];
    $(td).css('text-align', 'center');

    return r;
}

$('#tab_test4').on('keydown.autocomplete', '.input_material', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();
    col_index = col_index + 2;

    $(tr.find("td:eq(" + col_index + ")").children().addClass('' + row_index + 'materialDes' + col_index));
    var materialDes = '.' + row_index + 'materialDes' + col_index;

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'material',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: trimStart('0', item.ID) + '-' + item.DESCRIPCION, value: trimStart('0', item.ID) };
                    }))
                }
            })
        },

        messages: {
            noResults: '',
            results: function (resultsCount) { }
        },

        change: function (e, ui) {
            if (!(ui.item)) {
                $(this).addClass("red white-text rojo");
                $(tr.find("td:eq(" + col_index + ")").children().val(""));
                clearErrors();
                //validarErrores("tab_test4");
                e.target.value = "";
            }
        },

        select: function (event, ui) {
            var label = ui.item.label;
            label = label.split('-');
            $(materialDes).val(label[1]);
            $(this).removeClass("red white-text rojo");
            $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
            clearErrors();
            //validarErrores("tab_test4");
        }
    });
});

$('#tab_test4').on('keydown.autocomplete', '.input_categoria', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();

    $(tr.find("td:eq(" + col_index + ")").children().addClass('' + row_index + 'categoriaDes' + col_index));

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'categoria',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: trimStart('0', item.ID) + '-' + item.DESCRIPCION, value: item.DESCRIPCION };
                    }));
                }
            });
        },

        messages: {
            noResults: '',
            results: function (resultsCount) { }
        },

        change: function (e, ui) {
            if (!(ui.item)) {
                $(this).addClass("red white-text rojo");
                clearErrors();
                e.target.value = "";
            }
        },

        select: function (event, ui) {
            $(tr.find("td:eq(" + col_index + ")").children().removeClass("red white-text rojo"));
            clearErrors();
        }
    });

});

$('#tab_test4').on('focusout', '.input_cantidades', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();
    var num_docH1 = null, pais = null, getDec = null;
    var num1 = null, num2 = null;
    var tablaH1 = $('#tab_test1').DataTable();

    var monto = null, porApoyo = null, pieApoyo = null, cosApoyo = null, preSugerido = null, volReal = null, apoyo = null;

    var colMonto = tr.find('td:eq(8) input');
    var colPorApoyo = tr.find('td:eq(9) input');
    var colPieApoyo = tr.find('td:eq(10) input');
    var colCosApoyo = tr.find('td:eq(11) input');
    var colPreSugerido = tr.find('td:eq(12) input');
    var colVolReal = tr.find('td:eq(13) input');
    var colApoyo = tr.find('td:eq(14) input');

    $(tr.find('td:eq(1)').children().addClass('' + row_index + 'numDoc4'));
    var num_doc = $('.' + row_index + 'numDoc4').val();

    for (var a = 0; a < tablaH1.rows().data().length; a++) {
        var rowH1 = tablaH1.row(a).node();
        num_docH1 = $(rowH1).children().eq(1).children().val();

        if (num_docH1 == num_doc) {
            pais = $(rowH1).find('td:eq(5)').children().val();
        }
    }

    $.ajax({
        type: "POST",
        url: 'getDecimal',
        dataType: "json",
        data: { "Pais": pais },
        async: false,
        success: function (data) {
            getDec = data;

            if (getDec == '.') {
                monto = tr.find('td:eq(8) input').val().replace('$', '').replace(',', '');
                porApoyo = tr.find('td:eq(9) input').val().replace('%', '').replace(',', '');
                pieApoyo = tr.find('td:eq(10) input').val().replace('$', '').replace(',', '');
                cosApoyo = tr.find('td:eq(11) input').val().replace('$', '').replace(',', '');
                preSugerido = tr.find('td:eq(12) input').val().replace('$', '').replace(',', '');
                volReal = tr.find('td:eq(13) input').val().replace('$', '').replace(',', '');
                apoyo = tr.find('td:eq(14) input').val().replace('$', '').replace(',', '');
            }
            else {
                monto = tr.find('td:eq(8) input').val().replace('$', '').replace('.', '').replace(',', '.');
                porApoyo = tr.find('td:eq(9) input').val().replace('%', '').replace('.', '').replace(',', '.');
                pieApoyo = tr.find('td:eq(10) input').val().replace('$', '').replace('.', '').replace(',', '.');
                cosApoyo = tr.find('td:eq(11) input').val().replace('$', '').replace('.', '').replace(',', '.');
                preSugerido = tr.find('td:eq(12) input').val().replace('$', '').replace('.', '').replace(',', '.');
                volReal = tr.find('td:eq(13) input').val().replace('$', '').replace('.', '').replace(',', '.');
                apoyo = tr.find('td:eq(14) input').val().replace('$', '').replace('.', '').replace(',', '.');
            }

            //VISTA PARA EL MONTO
            if ($.isNumeric(monto)) {
                if (monto > 0) {
                    colMonto.val(toShow(monto, getDec));
                    colMonto.removeClass("red white-text rojo");
                }
                else {
                    colMonto.val(toShow(monto, getDec));
                    colMonto.removeClass("red white-text rojo");
                }
            }
            else if (monto == '') {
                colMonto.val(toShow('0', getDec));
                colMonto.removeClass("red white-text rojo");
            }
            else {
                colMonto.addClass("red white-text rojo");
            }

            //VISTA PARA EL PORCENTAJE DE APOYO
            if ($.isNumeric(porApoyo)) {
                if (porApoyo > 0) {
                    colPorApoyo.val(toShowPorc(porApoyo, getDec));
                    colPorApoyo.removeClass("red white-text rojo");
                }
                else {
                    colPorApoyo.val(toShowPorc(porApoyo, getDec));
                    colPorApoyo.removeClass("red white-text rojo");
                }
            }
            else if (porApoyo == '') {
                colPorApoyo.val(toShowPorc('0', getDec));
                colPorApoyo.removeClass("red white-text rojo");
            }
            else {
                colPorApoyo.addClass("red white-text rojo");
            }

            //VISTA PARA EL APOYO POR PIEZA
            if ($.isNumeric(pieApoyo)) {
                if (pieApoyo > 0) {
                    colPieApoyo.val(toShow(pieApoyo, getDec));
                    colPieApoyo.removeClass("red white-text rojo");
                }
                else {
                    colPieApoyo.val(toShow(pieApoyo, getDec));
                    colPieApoyo.removeClass("red white-text rojo");
                }
            }
            else if (pieApoyo == '') {
                colPieApoyo.val(toShow('0', getDec));
                colPieApoyo.removeClass("red white-text rojo");
            }
            else {
                colPieApoyo.addClass("red white-text rojo");
            }

            //VISTA PARA EL COSTO CON APOYO
            if ($.isNumeric(cosApoyo)) {
                if (cosApoyo > 0) {
                    colCosApoyo.val(toShow(cosApoyo, getDec));
                    colCosApoyo.removeClass("red white-text rojo");
                }
                else {
                    colCosApoyo.val(toShow(cosApoyo, getDec));
                    colCosApoyo.removeClass("red white-text rojo");
                }
            }
            else if (cosApoyo == '') {
                colCosApoyo.val(toShow('0', getDec));
                colCosApoyo.removeClass("red white-text rojo");
            }
            else {
                colCosApoyo.addClass("red white-text rojo");
            }

            //VISTA PARA EL PRECIO SUGERIDO
            if ($.isNumeric(preSugerido)) {
                if (preSugerido > 0) {
                    colPreSugerido.val(toShow(preSugerido, getDec));
                    colPreSugerido.removeClass("red white-text rojo");
                }
                else {
                    colPreSugerido.val(toShow(preSugerido, getDec));
                    colPreSugerido.removeClass("red white-text rojo");
                }
            }
            else if (preSugerido == '') {
                colPreSugerido.val(toShow('0', getDec));
                colPreSugerido.removeClass("red white-text rojo");
            }
            else {
                colPreSugerido.addClass("red white-text rojo");
            }

            //VISTA PARA EL VOLUMEN REAL
            if ($.isNumeric(volReal)) {
                if (volReal > 0) {
                    colVolReal.val(toShowNum(volReal, getDec));
                    colVolReal.removeClass("red white-text rojo");
                }
                else {
                    colVolReal.val(toShowNum(volReal, getDec));
                    colVolReal.removeClass("red white-text rojo");
                }
            }
            else if (volReal == '') {
                colVolReal.val(toShowNum('0', getDec));
                colVolReal.removeClass("red white-text rojo");
            }
            else {
                colVolReal.addClass("red white-text rojo");
            }

            //VISTA PARA EL APOYO
            if ($.isNumeric(apoyo)) {
                if (apoyo > 0) {
                    if (($.isNumeric(monto) & monto !== '0.00') & ($.isNumeric(porApoyo) & porApoyo !== '0.00') & ($.isNumeric(volReal) & volReal !== '0.00')) {
                        num1 = monto * (porApoyo / 100);
                        num2 = num1 * volReal;

                        colPieApoyo.val(toShow(num1, getDec));
                        colCosApoyo.val(toShow(monto - num1, getDec));
                        colApoyo.val(toShow(num2, getDec));

                        validaApoyo(num2, colApoyo);
                    }
                    else if ((monto == "" | porApoyo == "" | volReal == "") | (monto == "0.00" | porApoyo == "0.00" | volReal == "0.00")) {
                        if ((monto == "" & porApoyo == "" & volReal == "") | (monto == "0.00" & porApoyo == "0.00" & volReal == "0.00") & apoyo !== "") {
                            colMonto.val(toShow('0', getDec));
                            colPorApoyo.val(toShowPorc('0', getDec));
                            colPieApoyo.val(toShow('0', getDec));
                            colCosApoyo.val(toShow('0', getDec));
                            colPreSugerido.val(toShow('0', getDec));
                            colVolReal.val(toShowNum('0', getDec));
                            colApoyo.val(toShow(apoyo, getDec));
                            checkRelacionadaMat();
                        }
                        else {
                            num1 = monto * (porApoyo / 100);
                            num2 = num1 * volReal;

                            colPieApoyo.val(toShow(num1, getDec));
                            colCosApoyo.val(toShow(monto - num1, getDec));
                            colApoyo.val(toShow(num2, getDec));

                            validaApoyo(num2, colApoyo);
                        }
                    }
                }
                else {
                    if ($.isNumeric(monto) & $.isNumeric(porApoyo) & $.isNumeric(volReal)) {
                        num1 = monto * (porApoyo / 100);
                        num2 = num1 * volReal;

                        colPieApoyo.val(toShow(num1, getDec));
                        colCosApoyo.val(toShow(monto - num1, getDec));
                        colApoyo.val(toShow(num2, getDec));

                        validaApoyo(num2, colApoyo);
                    }
                }
            }
            else if (apoyo == '') {
                if ($.isNumeric(monto) & $.isNumeric(porApoyo) & $.isNumeric(volReal)) {
                    num1 = monto * (porApoyo / 100);
                    num2 = num1 * volReal;

                    colPieApoyo.val(toShow(num1, getDec));
                    colCosApoyo.val(toShow(monto - num1, getDec));
                    colApoyo.val(toShow(num2, getDec));

                    validaApoyo(num2, colApoyo);
                }
            }
        }
    });
});

$('#tab_test4').on('keydown', '.input_apoyo', function (e) {
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var num_docH1 = null, pais = null, getDec = null;
    var tablaH1 = $('#tab_test1').DataTable();
    var apoyo = null;

    var colMonto = tr.find('td:eq(8) input');
    var colPorApoyo = tr.find('td:eq(9) input');
    var colPieApoyo = tr.find('td:eq(10) input');
    var colCosApoyo = tr.find('td:eq(11) input');
    var colPreSugerido = tr.find('td:eq(12) input');
    var colVolReal = tr.find('td:eq(13) input');
    var colApoyo = tr.find('td:eq(14) input');

    $(tr.find('td:eq(1)').children().addClass(row_index + 'numDoc4'));
    var num_doc = $('.' + row_index + 'numDoc4').val();

    for (var a = 0; a < tablaH1.rows().data().length; a++) {
        var rowH1 = tablaH1.row(a).node();
        num_docH1 = $(rowH1).children().eq(1).children().val();

        if (num_docH1 == num_doc) {
            pais = $(rowH1).find('td:eq(5)').children().val();
        }
    }

    $.ajax({
        type: "POST",
        url: 'getDecimal',
        dataType: "json",
        data: { "Pais": pais },
        async: false,
        success: function (data) {
            getDec = data;

            if (getDec == '.') {
                apoyo = tr.find('td:eq(14) input').val().replace('$', '').replace(',', '');
            }
            else {
                apoyo = tr.find('td:eq(14) input').val().replace('$', '').replace('.', '').replace(',', '.');
            }

            //VISTA PARA EL APOYO
            if (e.keyCode == 13) {
                e.preventDefault();

                if ($.isNumeric(apoyo)) {
                    colMonto.val(toShow('0', getDec));
                    colPorApoyo.val(toShowPorc('0', getDec));
                    colPieApoyo.val(toShow('0', getDec));
                    colCosApoyo.val(toShow('0', getDec));
                    colPreSugerido.val(toShow('0', getDec));
                    colVolReal.val(toShowNum('0', getDec));
                    colApoyo.val(toShow(apoyo, getDec));
                }

                colApoyo.blur();
                checkRelacionadaMat();
            }

            validaApoyo(apoyo, colApoyo);
        }
    });
});

function validaApoyo(apoyo, colApoyo) {
    var tr = $(colApoyo).closest('tr'); //Obtener el row
    var errorSumMateriales = tr.find('td:eq(14) input').hasClass("errorCantidades");

    if (apoyo !== "") {
        if ($.isNumeric(apoyo)) {
            if (apoyo !== '0.00' & apoyo > '0.00') {
                //if (!errorSumMateriales) {

                //}
                colApoyo.removeClass("red white-text rojo");
                clearErrors();
            }
            else {
                colApoyo.addClass("red white-text rojo");
                clearErrors();
            }
        }
        else {
            colApoyo.addClass("red white-text rojo");
            clearErrors();
        }
    }
    else {
        colApoyo.addClass("red white-text rojo");
        clearErrors();
    }

    checkRelacionadaMat();
}

function ligada(check) {
    var tr = $(check).closest('tr');//Obtener el row
    var tablaH1 = $('#tab_test1').DataTable();

    var num_docH1 = null, pais = null, getDec = null;
    var colMonto = tr.find('td:eq(8) input');
    var colPieApoyo = tr.find('td:eq(10) input');
    var colCosApoyo = tr.find('td:eq(11) input');
    var colPreSugerido = tr.find('td:eq(12) input');
    var colVolReal = tr.find('td:eq(13) input');
    var colApoyo = tr.find('td:eq(14) input');
    var num_doc = tr.find('td:eq(1) input').val();

    for (var a = 0; a < tablaH1.rows().data().length; a++) {
        var rowH1 = tablaH1.row(a).node();
        num_docH1 = $(rowH1).children().eq(1).children().val();

        if (num_docH1 == num_doc) {
            pais = $(rowH1).find('td:eq(5)').children().val();
        }
    }

    $.ajax({
        type: "POST",
        url: 'getDecimal',
        dataType: "json",
        data: { "Pais": pais },
        success: function (data) {
            getDec = data;

            if (getDec == '.') {
                apoyo = tr.find('td:eq(14) input').val().replace('$', '').replace(',', '');
            }
            else {
                apoyo = tr.find('td:eq(14) input').val().replace('$', '').replace('.', '').replace(',', '.');
            }

            if ($(check).is(":checked")) {
                colMonto.val(toShow('0', getDec)).attr("disabled", true);
                colPieApoyo.val(toShow('0', getDec)).attr("disabled", true);
                colCosApoyo.val(toShow('0', getDec)).attr("disabled", true);
                colPreSugerido.val(toShow('0', getDec)).attr("disabled", true);
                colVolReal.val(toShowNum('0', getDec)).attr("disabled", true);
                colApoyo.val(toShow('0', getDec)).attr("disabled", true);
            } else {
                colMonto.val(toShow('0', getDec)).attr("disabled", false);
                colPieApoyo.val(toShow('0', getDec)).attr("disabled", false);
                colCosApoyo.val(toShow('0', getDec)).attr("disabled", false);
                colPreSugerido.val(toShow('0', getDec)).attr("disabled", false);
                colVolReal.val(toShowNum('0', getDec)).attr("disabled", false);
                colApoyo.val(toShow('0', getDec)).attr("disabled", false);
            }
        },
        async: false
    });
}

$("#tab_dis").click(function () {
    var tbody = $("#tab_test4 tbody");
    var arregloNumDoc = [];

    if (tbody.children().length !== 0) {
        arregloNumDoc = checkRelacionada();

        if (arregloNumDoc.length > 0) {
            var tablaH3 = $('#tab_test3').DataTable();

            for (var a = 0; a < tablaH3.rows().data().length; a++) {
                var rowH3 = tablaH3.row(a).node();
                var num_docH3 = $(rowH3).children().eq(1).children().val();

                if (jQuery.inArray(num_docH3, arregloNumDoc) !== -1) {
                    $(rowH3).children().eq(1).children().addClass("red white-text rojo");
                }
            }
        }
        checkRelacionadaMat();
    }
});

/////////////////////////////////////////////////////////HOJA 5 FUNCIONES Y ASIGNACIONES////////////////////////////////////////////////////////
function procesarHoja5() {
    var table = $('#tab_test5').DataTable(
        {
            language: { "url": "../Scripts/lang/" + ln + ".json" },
            "paging": false
        });
    table.clear().draw();

    $.ajax({
        type: "POST",
        url: 'validaHoja5',
        dataType: "json",
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {

            if (data !== null || data !== "") {

                var textos = data.pop();
                var tsol = data.pop();
                //INICIO DEL CICLO FOR
                for (var i = 0; i < data.length; i++) {
                    addRowH5(table, data[i], textos[i], tsol[i]);
                }
                //FIN DEL FOR

                $('#tab_test5').css("font-size", "10px");
                $('#tab_test5').css("display", "table");
            }
        },
        complete: function (data) {
            //validarErrores("tab_test5"); //RMG
            $(document).ready(function () {
                $('#global_filter').removeAttr('disabled');
                var tablaH5 = $('#tab_test5').DataTable();
                $("#excelBtn").removeAttr("disabled");
                cloneTables();
                $("#tab_test5").on("change", ".requiredfile", function () {
                    var data = table.row(this).data();
                    var val = $(this).val();
                    if ($(this).hasClass("valid") | val !== "") {//ADD RSG 29.10.2018
                        $(this).closest('tr').children().eq(0).children().removeClass("red rojo");
                        $(this).closest('tr').children().eq(0).children().addClass("green");
                        $(this).closest('tr').children().eq(0).children().text("done");
                        clearErrors();
                    } else {
                        $(this).closest('tr').children().eq(0).children().removeClass("green");
                        $(this).closest('tr').children().eq(0).children().addClass("red rojo");
                        $(this).closest('tr').children().eq(0).children().text("close");
                        clearErrors();//ADD RSG 29.10.2018
                    }
                });

                $("#tab_test5").on("change", ".outRequiredfile", function () {
                    if ($(this).hasClass("valid")) {
                        var id = $(this).closest('tr').children().eq(1).children().val();
                        var tipo = $(this).closest('tr').children().eq(2).children().val();
                        $(this).closest('tr').children().eq(3).children().children().eq(0).children().eq(1).attr('id', id + tipo);
                    } else {
                        $(this).closest('tr').children().eq(3).children().children().eq(0).children().eq(1).removeAttr('id');
                    }
                });
                $("#tablesToexcel").prop("disabled", "false");
                $("#tablesToexcel").removeAttr("disabled");
            });
        },
        error: function (xhr, httpStatusMessage, customErrorMessage) {
            alert("Request couldn't be processed. Please try again later. the reason " + xhr.status + " : " + httpStatusMessage + " : " + customErrorMessage);
        },
        async: false
    });
}

function addRowH5(t, NUM_DOC, TIPO, OBLIGATORIO) {
    var CHECK = null;
    var CLASE = null;

    if (OBLIGATORIO == true) {
        CHECK = "<span class='red white-text rojo material-icons'>close</span>";
        TIPO = '*' + TIPO;//RMG
        CLASE = 'rojo';
        fileInpt = "<div class='file-field input-field isrequired'><div class='btn-small' style='float: left;'><span>Examinar</span><input type='file' name='" + NUM_DOC + TIPO + "' id='" + NUM_DOC + TIPO + "'></div><div class='file-path-wrapper'><input class='file-path validate requiredfile' type='text'></div></div>";

    }
    else {
        CHECK = "<span class='green white-text material-icons'>done</span>";//RMG
        fileInpt = "<div class='file-field input-field'><div class='btn-small' style='float: left;'><span>Examinar</span><input type='file'></div><div class='file-path-wrapper'><input class='file-path validate outRequiredfile' type='text'></div></div>";

    }

    var r = t.row.add([
        CHECK,
        "<input class='' style='font-size:12px; text-align:center;' type='text' id='' name='' disabled value='" + NUM_DOC + "'><span hidden>" + NUM_DOC + "</span>",
        "<input class='' style='font-size:12px; text-align:center;' type='text' id='' name='' value='" + TIPO + "' disabled><span hidden>" + TIPO + "</span>",
        fileInpt, //RMG
        "<input class='NOTAS" + NUM_DOC + TIPO + "' style='font-size:12px; text-align:center;' type='text' id='' name='' value=''><span hidden>NOTAS" + NUM_DOC + TIPO + "</span>",
        //"<input class='' style='font-size:10px; text-align:center;' type='text' id='' name='' value=''>",
        //"<div class='file-field input-field col s8 offset-s3'><div class='btn'><span>Seleccionar archivo</span><input type='file' class='btnF' name='' id='"+NUM_DOC+TIPO+"'></div></div>"
    ]).draw(false).node();

    var td = $(r).children()[0];
    $(td).css('text-align', 'center');

    return r;
}

$("#tab_arc").click(function () {
    var tbody = $("#tab_test5 tbody");
    var arregloNumDoc = [];

    if (tbody.children().length !== 0) {
        arregloNumDoc = checkRelacionada();

        if (arregloNumDoc.length > 0) {
            var tablaH3 = $('#tab_test3').DataTable();

            for (var a = 0; a < tablaH3.rows().data().length; a++) {
                var rowH3 = tablaH3.row(a).node();
                var num_docH3 = $(rowH3).children().eq(1).children().val();

                if (jQuery.inArray(num_docH3, arregloNumDoc) !== -1) {
                    $(rowH3).children().eq(1).children().addClass("red white-text rojo");
                }
            }
        }
        checkRelacionadaMat();
    }
});

/////////////////////////////////////////////FUNCIONES GENERALES////////////////////////////
function checkRelMul() {
    var tablaH1 = $('#tab_test1').DataTable();
    var tablaH2 = $('#tab_test2').DataTable();
    var tablaH3 = $('#tab_test3').DataTable();

    for (var a = 0; a < tablaH1.rows().data().length; a++) {
        var rowH1 = tablaH1.row(a).node();
        var num_docH1 = $(rowH1).children().eq(1).children().val();
        var banderaH2 = false, banderaH3 = false;

        for (var b = 0; b < tablaH2.rows().data().length; b++) {
            var rowH2 = tablaH2.row(b).node();
            var num_docH2 = $(rowH2).children().eq(1).children().val();

            if (num_docH1 == num_docH2) {
                banderaH2 = true;
                break;
            }
        }

        for (var c = 0; c < tablaH3.rows().data().length; c++) {
            var rowH3 = tablaH3.row(c).node();
            var num_docH3 = $(rowH3).children().eq(1).children().val();

            if (num_docH1 == num_docH3) {
                banderaH3 = true;
                break;
            }
        }

        if (!banderaH2 & !banderaH3) {
            $(rowH1).children().eq(1).children().addClass("red white-text rojo");
        }
    }
}

function checkRelacionada() {
    var tablaH2 = $('#tab_test2').DataTable();
    var tablaH3 = $('#tab_test3').DataTable();
    var arregloNumDoc = [];
    var contador = 0;

    for (var a = 0; a < tablaH2.rows().data().length; a++) {
        var rowH2 = tablaH2.row(a).node();
        var num_docH2 = $(rowH2).children().eq(1).children().val();

        for (var b = 0; b < tablaH3.rows().data().length; b++) {
            var rowH3 = tablaH3.row(b).node();
            var num_docH3 = $(rowH3).children().eq(1).children().val();

            if (num_docH2 == num_docH3) {
                if (jQuery.inArray(num_docH2, arregloNumDoc) == -1) {
                    arregloNumDoc[contador] = num_docH2;
                    contador++;
                }
            }
        }
    }

    if (arregloNumDoc.length > 0) {
        for (var c = 0; c < tablaH3.rows().data().length; c++) {
            var rowH33 = tablaH3.row(c).node();
            var num_docH33 = $(rowH33).children().eq(1).children().val();

            if (jQuery.inArray(num_docH33, arregloNumDoc) !== -1) {
                $(rowH33).children().eq(1).children().addClass("red white-text rojo");
            }
        }
    }
    return arregloNumDoc;
}

function checkRelacionadaMat() {
    var tablaH1 = $('#tab_test1').DataTable();
    var tablaH3 = $('#tab_test3').DataTable();
    var tablaH4 = $('#tab_test4').DataTable();
    var pais, miles, decimales = null;
    var cantidadesH3 = [], cantidadesH3Num = [], cantidadesUniH3 = [];
    var contadorH1 = 0, contadorH3 = 0, cantidad = 0, cantidadRel = 0;


    ///////////////////////////////////////////PROCEDIMIENTO PARA SACAR LAS CANTIDADES DE MULTIPLES HOJA 3////////////////////////////////////////////////
    for (var a = 0; a < tablaH3.rows().data().length; a++) {
        var rowH3 = tablaH3.row(a).node();
        var num_docH3 = $(rowH3).children().eq(1).children().val();
        cantidad = $(rowH3).children().eq(7).children().val();
        cantidadRel = cantidad;

        for (var b = 0; b < tablaH1.rows().data().length; b++) {
            var rowH1 = tablaH1.row(b).node();
            var num_docH1 = $(rowH1).children().eq(1).children().val();

            if (num_docH1 == num_docH3) {
                pais = $(rowH1).find('td:eq(5)').children().val();
            }
        }

        cantidad = cantidad.substr(cantidad.length - 3);
        decimales = cantidad.charAt(0);

        if (decimales == ".") {
            miles = ",";
        }
        else if (decimales == ",") {
            miles = ".";
        }

        cantidadRel = toNum(cantidadRel, miles, decimales);

        if (jQuery.inArray(num_docH3, cantidadesH3) == -1) {
            cantidadesH3[contadorH3] = cantidadRel;
            cantidadesH3Num[contadorH3] = num_docH3;
            contadorH3++;
        }
    }

    for (var c = 0; c < tablaH1.rows().data().length; c++) {
        var rowH11 = tablaH1.row(c).node();
        var num_docH11 = $(rowH11).children().eq(1).children().val();
        var sumH3 = 0;

        for (var d = 0; d < cantidadesH3.length; d++) {
            if (num_docH11 == cantidadesH3Num[d]) {
                sumH3 += cantidadesH3[d];
            }
        }

        if (jQuery.inArray(num_docH11, cantidadesH3Num) !== -1) {
            cantidadesUniH3[contadorH1] = num_docH11 + '-' + sumH3;
            contadorH1++;
        }
    }
    /////////////////////////////////////////////////////////////////////////FIN//////////////////////////////////////////////////////////////////////////
    pais, miles, decimales = null;
    var contadorH4 = 0;
    contadorH1 = 0, cantidad = 0, cantidadRel = 0;
    var cantidadesH4 = [], cantidadesH4Num = [], cantidadesUniH4 = [];

    ///////////////////////////////////////////PROCEDIMIENTO PARA SACAR LAS CANTIDADES DE MATERIALES HOJA 4////////////////////////////////////////////////

    for (var e = 0; e < tablaH4.rows().data().length; e++) {
        var rowH4 = tablaH4.row(e).node();
        var num_docH4 = $(rowH4).children().eq(1).children().val();
        cantidad = $(rowH4).children().eq(14).children().val();
        cantidadRel = cantidad;

        for (var f = 0; f < tablaH1.rows().data().length; f++) {
            var rowH111 = tablaH1.row(f).node();
            var num_docH111 = $(rowH111).children().eq(1).children().val();

            if (num_docH111 == num_docH4) {
                pais = $(rowH111).find('td:eq(5)').children().val();
            }
        }

        cantidad = cantidad.substr(cantidad.length - 3);
        decimales = cantidad.charAt(0);

        if (decimales == ".") {
            miles = ",";
        }
        else if (decimales == ",") {
            miles = ".";
        }

        cantidadRel = toNum(cantidadRel, miles, decimales);

        if (jQuery.inArray(num_docH4, cantidadesH4) == -1) {
            cantidadesH4[contadorH4] = cantidadRel;
            cantidadesH4Num[contadorH4] = num_docH4;
            contadorH4++;
        }
    }

    for (var g = 0; g < tablaH1.rows().data().length; g++) {
        var rowH1111 = tablaH1.row(g).node();
        var num_docH1111 = $(rowH1111).children().eq(1).children().val();
        var sumH4 = 0;

        for (var h = 0; h < cantidadesH4.length; h++) {
            if (num_docH1111 == cantidadesH4Num[h]) {
                sumH4 += cantidadesH4[h];
            }
        }

        if (jQuery.inArray(num_docH1111, cantidadesH4Num) !== -1) {
            cantidadesUniH4[contadorH1] = num_docH1111 + '-' + sumH4;
            contadorH1++;
        }
    }
    /////////////////////////////////////////////////////////////////////////FIN/////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////PROCEDIMIENTO PARA SACAR LOS ID CON ERROR DE LAS CANTIDADES QUE NO CUADRAN////////////////////////////////////////////////
    var cantidadesError = [], idError = [];
    var contadorErr = 0;

    for (var i = 0; i < cantidadesUniH3.length; i++) {
        if (jQuery.inArray(cantidadesUniH3[i], cantidadesUniH4) == -1) {
            cantidadesError[contadorErr] = cantidadesUniH3[i];
            contadorErr++;
        }
    }

    for (j = 0; j < cantidadesError.length; j++) {
        idError[j] = cantidadesError[j].charAt(0);
    }
    /////////////////////////////////////////////////////////////////////////FIN/////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////PINTAMOS DE COLOR ROJO LOS MONTOS DE APOYO EN MATERIALES POR NO CUADRAR EN CANTIDADES///////////////////////////////////////////
    for (var k = 0; k < tablaH4.rows().data().length; k++) {
        var rowH44 = tablaH4.row(k).node();
        var num_docH44 = $(rowH44).children().eq(1).children().val();
        var tieneRojo = $(rowH44).children().eq(14).children().hasClass("red white-text rojo");

        for (var l = 0; l < idError.length; l++) {
            if (num_docH44 == idError[l]) {
                $(rowH44).children().eq(1).children().addClass("red white-text rojo errorCantidades");
                //if (tieneRojo) {
                //    $(rowH44).children().eq(14).children().addClass("errorCantidades");
                //}
                //else {
                //    $(rowH44).children().eq(14).children().addClass("red white-text rojo errorCantidades");
                //}
            }
        }
    }
    /////////////////////////////////////////////////////////////////////////FIN/////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////DESPINTAMOS DE COLOR ROJO LOS MONTOS DE APOYO EN MATERIALES QUE CUADRAR EN CANTIDADES///////////////////////////////////////////
    var correctos = [];
    var contadorCorrectos = 0;

    for (var m = 0; m < tablaH1.rows().data().length; m++) {
        var rowH1C = tablaH1.row(m).node();
        var num_docH1C = $(rowH1C).children().eq(1).children().val();

        if (jQuery.inArray(num_docH1C, idError) == -1) {
            correctos[contadorCorrectos] = num_docH1C;
            contadorCorrectos++;
        }
    }

    for (var n = 0; n < tablaH4.rows().data().length; n++) {
        var rowH4C = tablaH4.row(n).node();
        var num_docH4C = $(rowH4C).children().eq(1).children().val();
        //var tieneRojo = $(rowH4C).children().eq(14).children().hasClass("red white-text rojo");

        for (var o = 0; o < correctos.length; o++) {
            if (num_docH4C == correctos[o]) {
                $(rowH4C).children().eq(1).children().removeClass("red white-text rojo errorCantidades");
            }
        }
    }

    return correctos;
}

function obtenDecimalMil(pais, cantidad) {
    var mil, decimal, total = null;

    $.ajax({
        type: "POST",
        url: 'getDecimalMil',
        dataType: "json",
        data: { "Pais": pais },
        success: function (data) {
            if (data !== null | data !== "") {
                decimal = data.pop();
                mil = data;
            }
            total = toNum(cantidad, mil, decimal);
        },
        async: false
    });

    return total;
}

function erroresH1() {
    var tablaH1 = $('#tab_test1').DataTable();
    var banderaH1 = false;
    var tabla1 = [];

    for (var a = 0; a < tablaH1.rows().data().length; a++) {
        var rowH1 = tablaH1.row(a).node();
        var num_docH1 = $(rowH1).children().eq(1).children().val();
        var statusH1 = $(rowH1).children().children().hasClass('rojo');
        var tsol = $(rowH1).children().eq(2).children().hasClass('rojo');
        var sociedad = $(rowH1).children().eq(4).children().hasClass('rojo');
        var pais = $(rowH1).children().eq(5).children().hasClass('rojo');

        if (!$.isNumeric(num_docH1) | tsol | sociedad | pais) {
            tabla1[a] = num_docH1;
            banderaH1 = true;
        }
        else {
            banderaH1 = false;
        }
    }

    ////console.log(tabla1);
    return tabla1;
}

function trimStart(character, string) {//RSG 07.06.2018
    var startIndex = 0;

    while (string[startIndex] === character) {
        startIndex++;
    }

    return string.substr(startIndex);
}

function clearErrors() {
    var tablaH1 = $('#tab_test1').DataTable();
    var tablaH2 = $('#tab_test2').DataTable();
    var tablaH3 = $('#tab_test3').DataTable();
    var tablaH4 = $('#tab_test4').DataTable();
    //var tablaH5 = $('#tab_test5').DataTable();//DELETE RSG 29.10.2018

    var tabla1 = [], tabla2 = [], tabla3 = [], tabla4 = [], tabla5 = [], archivos = [];

    for (var a = 0; a < tablaH1.rows().data().length; a++) {
        var rowH1 = tablaH1.row(a).node();
        var num_docH1 = $(rowH1).children().eq(1).children().val();
        var statusH1 = $(rowH1).children().children().hasClass('rojo');

        tabla1[a] = num_docH1 + statusH1;
    }

    for (var b = 0; b < tablaH2.rows().data().length; b++) {
        var rowH2 = tablaH2.row(b).node();
        var num_docH2 = $(rowH2).children().eq(1).children().val();
        var statusH2 = $(rowH2).children().children().hasClass('rojo');

        tabla2[b] = num_docH2 + statusH2;
    }

    for (var c = 0; c < tablaH3.rows().data().length; c++) {
        var rowH3 = tablaH3.row(c).node();
        var num_docH3 = $(rowH3).children().eq(1).children().val();
        var statusH3 = $(rowH3).children().children().hasClass('rojo');

        tabla3[c] = num_docH3 + statusH3;
    }

    for (var d = 0; d < tablaH4.rows().data().length; d++) {
        var rowH4 = tablaH4.row(d).node();
        var num_docH4 = $(rowH4).children().eq(1).children().val();
        var statusH4 = $(rowH4).children().children().hasClass('rojo');

        tabla4[d] = num_docH4 + statusH4;
    }

    //for (var e = 0; e < tablaH5.rows().data().length; e++) {//DELETE RSG 29.10.2018
    //    var rowH5 = tablaH5.row(e).node();
    //    var num_docH5 = $(rowH5).children().eq(1).children().val();
    //    var statusH5 = $(rowH5).children().children().hasClass('rojo');

    //    tabla5[e] = num_docH5 + statusH5;
    //}

    var banderaH1, banderaH2, banderaH3, banderaH4, banderaH5 = false;

    for (var aa = 0; aa < tablaH1.rows().data().length; aa++) {
        var rowH11 = tablaH1.row(aa).node();
        var num_docH11 = $(rowH11).children().eq(1).children().val();
        clearErrorsN(num_docH11);//ADD RSG 29.10.2018

        var tablaH5 = $('#tab_test5').DataTable();                      //ADD RSG 29.10.2018
        for (var e = 0; e < tablaH5.rows().data().length; e++) {        //ADD RSG 29.10.2018
            var rowH5 = tablaH5.row(e).node();
            var num_docH5 = $(rowH5).children().eq(1).children().val();
            var statusH5 = $(rowH5).children().children().hasClass('rojo');

            tabla5[e] = num_docH5 + statusH5;
        }

        //SI TIENE TRUE TIENE ERROR
        if (jQuery.inArray(num_docH11 + true, tabla1) !== -1) {
            banderaH1 = true;
        }
        else {
            banderaH1 = false;
        }

        for (var bb = 0; bb < tablaH2.rows().data().length; bb++) {
            var rowH22 = tablaH2.row(bb).node();
            var num_docH22 = $(rowH22).children().eq(1).children().val();

            if (num_docH11 == num_docH22) {
                //SI TIENE TRUE TIENE ERROR
                if (jQuery.inArray(num_docH22 + true, tabla2) !== -1) {
                    banderaH2 = true;
                    break;
                }
                else {
                    banderaH2 = false;
                }
            }
            else {
                banderaH2 = false;
            }
        }

        for (var cc = 0; cc < tablaH3.rows().data().length; cc++) {
            var rowH33 = tablaH3.row(cc).node();
            var num_docH33 = $(rowH33).children().eq(1).children().val();

            if (num_docH11 == num_docH33) {
                //SI TIENE TRUE TIENE ERROR
                if (jQuery.inArray(num_docH33 + true, tabla3) !== -1) {
                    banderaH3 = true;
                    break;
                }
                else {
                    banderaH3 = false;
                }
            }
            else {
                banderaH3 = false;
            }
        }

        for (var dd = 0; dd < tablaH4.rows().data().length; dd++) {
            var rowH44 = tablaH4.row(dd).node();
            var num_docH44 = $(rowH44).children().eq(1).children().val();

            if (num_docH11 == num_docH44) {
                //SI TIENE TRUE TIENE ERROR
                if (jQuery.inArray(num_docH44 + true, tabla4) !== -1) {
                    banderaH4 = true;
                    break;
                }
                else {
                    banderaH4 = false;
                }
            }
            else {
                banderaH4 = false;
            }
        }

        for (var ee = 0; ee < tablaH5.rows().data().length; ee++) {
            var rowH55 = tablaH5.row(ee).node();
            var num_docH55 = $(rowH55).children().eq(1).children().val();

            if (num_docH11 == num_docH55) {
                //SI TIENE TRUE TIENE ERROR
                if (jQuery.inArray(num_docH55 + true, tabla5) !== -1) {
                    banderaH5 = true;
                    break;
                }
                else {
                    banderaH5 = false;
                }
            }
            else {
                banderaH5 = false;
            }
        }

        //console.log(banderaH1, banderaH2, banderaH3, banderaH4, banderaH5, num_docH11);
        //console.log(validaErrores(banderaH1, banderaH2, banderaH3, banderaH4, banderaH5, num_docH11));
        validaErrores(banderaH1, banderaH2, banderaH3, banderaH4, banderaH5, num_docH11);
    }
}

function validaErrores(h1, h2, h3, h4, h5, num_doc) {
    var tablaH1 = $('#tab_test1').DataTable();
    var tablaH2 = $('#tab_test2').DataTable();
    var tablaH3 = $('#tab_test3').DataTable();
    var tablaH4 = $('#tab_test4').DataTable();

    if (!h1 && !h2 && !h3 && !h4 && !h5) {

        for (var a = 0; a < tablaH1.rows().data().length; a++) {
            var rowH1 = tablaH1.row(a).node();
            var num_docH1 = $(rowH1).children().eq(1).children().val();

            if (num_docH1 == num_doc) {
                $(rowH1).children().eq(0).children().removeClass("red");
                $(rowH1).children().eq(0).children().addClass("green");
                $(rowH1).children().eq(0).children().text("done");
            }
        }

        for (var b = 0; b < tablaH2.rows().data().length; b++) {
            var rowH2 = tablaH2.row(b).node();
            var num_docH2 = $(rowH2).children().eq(1).children().val();

            if (num_docH2 == num_doc) {
                $(rowH2).children().eq(0).children().removeClass("red");
                $(rowH2).children().eq(0).children().addClass("green");
                $(rowH2).children().eq(0).children().text("done");
            }
        }

        for (var c = 0; c < tablaH3.rows().data().length; c++) {
            var rowH3 = tablaH3.row(c).node();
            var num_docH3 = $(rowH3).children().eq(1).children().val();

            if (num_docH3 == num_doc) {
                $(rowH3).children().eq(0).children().removeClass("red");
                $(rowH3).children().eq(0).children().addClass("green");
                $(rowH3).children().eq(0).children().text("done");
            }
        }

        for (var d = 0; d < tablaH4.rows().data().length; d++) {
            var rowH4 = tablaH4.row(d).node();
            var num_docH4 = $(rowH4).children().eq(1).children().val();

            if (num_docH4 == num_doc) {
                $(rowH4).children().eq(0).children().removeClass("red");
                $(rowH4).children().eq(0).children().addClass("green");
                $(rowH4).children().eq(0).children().text("done");
            }
        }
        return "SE MODIFICARON LOS REGISTROS: " + num_doc;
    }
    else {
        for (var aa = 0; aa < tablaH1.rows().data().length; aa++) {
            var rowH11 = tablaH1.row(aa).node();
            var num_docH11 = $(rowH11).children().eq(1).children().val();

            if (num_docH11 == num_doc) {
                $(rowH11).children().eq(0).children().removeClass("green");
                $(rowH11).children().eq(0).children().addClass("red");
                $(rowH11).children().eq(0).children().text("close");
            }
        }

        for (var bb = 0; bb < tablaH2.rows().data().length; bb++) {
            var rowH22 = tablaH2.row(bb).node();
            var num_docH22 = $(rowH22).children().eq(1).children().val();

            if (num_docH22 == num_doc) {
                $(rowH22).children().eq(0).children().removeClass("green");
                $(rowH22).children().eq(0).children().addClass("red");
                $(rowH22).children().eq(0).children().text("close");
            }
        }

        for (var cc = 0; cc < tablaH3.rows().data().length; cc++) {
            var rowH33 = tablaH3.row(cc).node();
            var num_docH33 = $(rowH33).children().eq(1).children().val();

            if (num_docH33 == num_doc) {
                $(rowH33).children().eq(0).children().removeClass("green");
                $(rowH33).children().eq(0).children().addClass("red");
                $(rowH33).children().eq(0).children().text("close");
            }
        }

        for (var dd = 0; dd < tablaH4.rows().data().length; dd++) {
            var rowH44 = tablaH4.row(dd).node();
            var num_docH44 = $(rowH44).children().eq(1).children().val();

            if (num_docH44 == num_doc) {
                $(rowH44).children().eq(0).children().removeClass("green");
                $(rowH44).children().eq(0).children().addClass("red");
                $(rowH44).children().eq(0).children().text("close");
            }
        }

        //for (var ee = 0; ee < tablaH5.rows().data().length; ee++) {
        //    var rowH55 = tablaH5.row(ee).node();
        //    var num_docH55 = $(rowH55).children().eq(1).children().val();
        //    var esRequerido2 = $(rowH55).children().eq(3).children().hasClass("isrequired");

        //    if (num_docH55 == num_doc & esRequerido2) {
        //        //console.log($(rowH55).children().eq(0).children().attr("class"));
        //        $(rowH55).children().eq(0).children().removeClass("green");
        //        $(rowH55).children().eq(0).children().addClass("red");
        //        $(rowH55).children().eq(0).children().text("close");
        //    }
        //}
    }
    return "WRANGGGGGGGGGGGGGGGGGGGG ERROR";
}

function guardaDatos() {
    document.getElementById("loader").style.display = "initial";

    var tablaH1 = $('#tab_test1').DataTable();
    var tablaH2 = $('#tab_test2').DataTable();
    var tablaH3 = $('#tab_test3').DataTable();
    var tablaH4 = $('#tab_test4').DataTable();
    var tablaH5 = $('#tab_test5').DataTable();

    var tabla1 = [], tabla2 = [], tabla3 = [], tabla4 = [], tabla5 = [], notasArr = [];
    var rowsH1 = [], rowsH2 = [], rowsH3 = [], rowsH4 = [], rowsH5 = [], rowsArc = [];
    var a = 0, b = 0, c = 0, d = 0, e = 0;
    var formData = new FormData();

    for (var aa = 0; aa < tablaH1.rows().data().length; aa++) {
        var rowH1 = tablaH1.row(aa).node();
        var status = $(rowH1).children().eq(0).children().text();

        if (status == "done") {
            var num_doc = $(rowH1).children().eq(1).children().val();
            var t_sol = $(rowH1).children().eq(2).children().val();
            var gall_id = $(rowH1).children().eq(3).children().val();
            var bukrs = $(rowH1).children().eq(4).children().val();
            var land = $(rowH1).children().eq(5).children().val();
            var estado = $(rowH1).children().eq(6).children().val();
            var ciudad = $(rowH1).children().eq(7).children().val();
            var concepto = $(rowH1).children().eq(8).children().val();
            var notas = $(rowH1).children().eq(9).children().val();
            var payer_id = $(rowH1).children().eq(10).children().val();
            var payer_nombre = $(rowH1).children().eq(11).children().val();
            var contacto_nombre = $(rowH1).children().eq(12).children().val();
            var contacto_email = $(rowH1).children().eq(13).children().val();
            var fechai_vig = $(rowH1).children().eq(14).children().val();
            var fechaf_vig = $(rowH1).children().eq(15).children().val();
            var moneda_id = $(rowH1).children().eq(16).children().val();

            rowsH1[a] = [num_doc, t_sol, gall_id, bukrs, land, estado, ciudad, concepto, notas, payer_id, payer_nombre, contacto_nombre, contacto_email, fechai_vig, fechaf_vig, moneda_id];
            a++;
        }
    }

    for (var bb = 0; bb < tablaH2.rows().data().length; bb++) {
        var rowH2 = tablaH2.row(bb).node();
        var statusH2 = $(rowH2).children().eq(0).children().text();

        if (statusH2 == "done") {
            var num_docH2 = $(rowH2).children().eq(1).children().val();
            var facturaH2 = $(rowH2).children().eq(2).children().val();
            var fecha_facturaH2 = $(rowH2).children().eq(3).children().val();
            var proveedorH2 = $(rowH2).children().eq(4).children().val();
            var proveedor_nombreH2 = $(rowH2).children().eq(5).children().val();
            var autorizacionH2 = $(rowH2).children().eq(6).children().val();
            var vencimientoH2 = $(rowH2).children().eq(7).children().val();
            var facturakH2 = $(rowH2).children().eq(8).children().val();
            var ejerciciokH2 = $(rowH2).children().eq(9).children().val();

            rowsH2[b] = [num_docH2, facturaH2, fecha_facturaH2, proveedorH2, proveedor_nombreH2, autorizacionH2, vencimientoH2, facturakH2, ejerciciokH2];
            b++;
        }
    }

    for (var cc = 0; cc < tablaH3.rows().data().length; cc++) {
        var rowH3 = tablaH3.row(cc).node();
        var statusH3 = $(rowH3).children().eq(0).children().text();

        if (statusH3 == "done") {
            var num_docH3 = $(rowH3).children().eq(1).children().val();
            var facturaH3 = $(rowH3).children().eq(2).children().val();
            var bill_docH3 = $(rowH3).children().eq(3).children().val();
            var ejerciciokH3 = $(rowH3).children().eq(4).children().val();
            var payer_idH3 = $(rowH3).children().eq(5).children().val();
            var payer_nombreH3 = $(rowH3).children().eq(6).children().val();
            var importe_facH3 = $(rowH3).children().eq(7).children().val();
            var belnrH3 = $(rowH3).children().eq(8).children().val();

            rowsH3[c] = [num_docH3, facturaH3, bill_docH3, ejerciciokH3, payer_idH3, payer_nombreH3, importe_facH3, belnrH3];
            c++;
        }
    }

    for (var dd = 0; dd < tablaH4.rows().data().length; dd++) {
        var rowH4 = tablaH4.row(dd).node();
        var statusH4 = $(rowH4).children().eq(0).children().text();

        if (statusH4 == "done") {
            var num_docH4 = $(rowH4).children().eq(1).children().val();
            var checkH4 = $(rowH4).children().eq(2).children().children().children().is(':checked');
            var vigencia_deH4 = $(rowH4).children().eq(3).children().val();
            var vigencia_alH4 = $(rowH4).children().eq(4).children().val();
            var matnrH4 = $(rowH4).children().eq(5).children().val();
            var matklH4 = $(rowH4).children().eq(6).children().val();
            var descripcionH4 = $(rowH4).children().eq(7).children().val();
            var montoH4 = $(rowH4).children().eq(8).children().val();
            var porc_apoyoH4 = $(rowH4).children().eq(9).children().val();
            var apoyo_piezaH4 = $(rowH4).children().eq(10).children().val();
            var costo_apoyoH4 = $(rowH4).children().eq(11).children().val();
            var precio_sugH4 = $(rowH4).children().eq(12).children().val();
            var volumen_realH4 = $(rowH4).children().eq(13).children().val();
            var apoyoH4 = $(rowH4).children().eq(14).children().val();

            rowsH4[d] = [num_docH4, checkH4, vigencia_deH4, vigencia_alH4, matnrH4, matklH4, descripcionH4, montoH4, porc_apoyoH4, apoyo_piezaH4, costo_apoyoH4, precio_sugH4, volumen_realH4, apoyoH4];
            d++;
        }
    }

    for (var ee = 0; ee < tablaH5.rows().data().length; ee++) {
        var rowH5 = tablaH5.row(ee).node();
        var statusH5 = $(rowH5).children().eq(0).children().text();

        if (statusH5 == "done") {
            var num_docH5 = $(rowH5).children().eq(1).children().val();
            var descripcionH5 = $(rowH5).children().eq(2).children().val();
            var idArchivoH5 = $(rowH5).children().eq(3).children().children().eq(0).children().eq(1).attr('id');


            if (idArchivoH5 == (num_docH5 + descripcionH5)) {
                var archivo = document.getElementById(num_docH5 + descripcionH5).files[0];

                if (idArchivoH5.includes("*")) {
                    rowsH5[e] = [num_docH5 + descripcionH5 + "*" + archivo.name + archivo.size];
                    rowsArc[e] = archivo;
                    formData.append("archivos[]", archivo);
                    e++;
                }
                else {
                    rowsH5[e] = [num_docH5 + "*" + descripcionH5 + "*" + archivo.name + archivo.size];
                    rowsArc[e] = archivo;
                    formData.append("archivos[]", archivo);
                    e++;
                }
            }
        }
    }

    var contadorNota = 0;
    for (var f = 0; f < tablaH1.rows().data().length; f++) {
        var rowH1 = tablaH1.row(f).node();
        var num_docH1 = $(rowH1).children().eq(1).children().val();
        var notaFinal = "";

        for (var g = 0; g < tablaH5.rows().data().length; g++) {
            var rowH5 = tablaH5.row(g).node();
            var statusH5 = $(rowH5).children().eq(0).children().text();

            if (statusH5 == "done") {
                var num_docH5 = $(rowH5).children().eq(1).children().val();
                var nota = $(rowH5).children().eq(4).children().val();


                if (num_docH1 == num_docH5) {
                    if (notaFinal == "") {
                        notaFinal = notaFinal + nota;
                    }
                    else if (nota != "") {
                        notaFinal = notaFinal + "," + nota;
                    }
                }
            }
        }

        if (notaFinal != "") {
            notasArr[contadorNota] = num_docH1 + "*" + notaFinal;
            contadorNota++;
        }
    }


    tabla1 = rowsH1;
    tabla2 = rowsH2;
    tabla3 = rowsH3;
    tabla4 = rowsH4;
    tabla5 = rowsH5;

    $.ajax({
        type: "POST",
        url: 'setArchivos',
        dataType: "json",
        processData: false,
        cache: false,
        contentType: false,
        data: formData,
        success: function (data) {
            getDec = data;
        },
        error: function (xhr, status, p3, p4) {
            var err = "Error " + " " + status + " " + p3 + " " + p4;
            if (xhr.responseText && xhr.responseText[0] == "{")
                err = JSON.parse(xhr.responseText).Message;
            //console.log(err);
        },
        async: true
    });

    $.ajax({
        type: "POST",
        url: 'setDatos',
        dataType: "json",
        data: { "h1": tabla1, "h2": tabla2, "h3": tabla3, "h4": tabla4, "h5": tabla5, "notas": notasArr },
        async: true,
        success: function (data) {
            if (data !== null | data !== "") {
                var eliminarId = [];
                var listaIds = [];

                listaIds = data.pop();

                eliminarId = data;
                var tablaH1 = $('#tab_test1').DataTable();
                var tablaH2 = $('#tab_test2').DataTable();
                var tablaH3 = $('#tab_test3').DataTable();
                var tablaH4 = $('#tab_test4').DataTable();
                var tablaH5 = $('#tab_test5').DataTable();

                for (var a = 0; a < tablaH1.rows().data().length; a++) {
                    var rowH1 = tablaH1.row(a).node();
                    var rowH11 = tablaH1.row(a);
                    var num_docH1 = $(rowH1).children().eq(1).children().val();

                    for (var b = 0; b < eliminarId.length; b++) {
                        num_doc = eliminarId[b];

                        if (num_doc == num_docH1) {
                            rowH11.remove().draw();
                            a--;
                        }
                    }
                }

                for (var c = 0; c < tablaH2.rows().data().length; c++) {
                    var rowH2 = tablaH2.row(c).node();
                    var rowH22 = tablaH2.row(c);
                    var num_docH2 = $(rowH2).children().eq(1).children().val();

                    for (var d = 0; d < eliminarId.length; d++) {
                        num_doc = eliminarId[d];

                        if (num_doc == num_docH2) {
                            rowH22.remove().draw();
                            c--;
                        }
                    }
                }

                for (var e = 0; e < tablaH3.rows().data().length; e++) {
                    var rowH3 = tablaH3.row(e).node();
                    var rowH33 = tablaH3.row(e);
                    var num_docH3 = $(rowH3).children().eq(1).children().val();

                    for (var f = 0; f < eliminarId.length; f++) {
                        num_doc = eliminarId[f];

                        if (num_doc == num_docH3) {
                            rowH33.remove().draw();
                            e--;
                        }
                    }
                }

                for (var g = 0; g < tablaH4.rows().data().length; g++) {
                    var rowH4 = tablaH4.row(g).node();
                    var rowH44 = tablaH4.row(g);
                    var num_docH4 = $(rowH4).children().eq(1).children().val();

                    for (var h = 0; h < eliminarId.length; h++) {
                        num_doc = eliminarId[h];

                        if (num_doc == num_docH4) {
                            rowH44.remove().draw();
                            g--;
                        }
                    }
                }

                for (var i = 0; i < tablaH5.rows().data().length; i++) {
                    var rowH5 = tablaH5.row(i).node();
                    var rowH55 = tablaH5.row(i);
                    var num_docH5 = $(rowH5).children().eq(1).children().val();

                    for (var j = 0; j < eliminarId.length; j++) {
                        num_doc = eliminarId[j];

                        if (num_doc == num_docH5) {
                            rowH55.remove().draw();
                            i--;
                        }
                    }
                }

                document.getElementById("loader").style.display = "none";

                for (var k = 0; k < listaIds.length; k++) {
                    M.toast({ html: 'Documento ' + listaIds[k] + ' fue creado' });
                }

                //var table = $('#tab_test1').DataTable();

                //if (!table.data().any()) {
                //    location.reload();
                //}
            }
        }
    });
}

function cloneTables() {
    var tablaH1c = $('#tab_test1').DataTable();
    var tablaH2c = $('#tab_test2').DataTable();
    var tablaH3c = $('#tab_test3').DataTable();
    var tablaH4c = $('#tab_test4').DataTable();

    ////TAB1Data////
    $('#tab_test1').append("<table id='tab_test1clond' style='display:none'><thead id='tabclon1hd'></thead><tbody id='tabclon1bd'></tbody></table>");
    $('#tabclon1hd').append("<tr id='titles1d'></tr>");

    $('#tab_test1 > thead > tr > th').each(function () {
        if ($(this).text() !== "LABEL" && $(this).text() !== "ESTATUS") {
            $('#titles1d').append("<th>" + $(this).text() + "</th>");
        } else {
        }

    });
    $('#tabclon1bd').append("<tr><td colspan='16'></td></tr>");
    for (var aa = 0; aa < tablaH1c.rows().data().length; aa++) {
        var rowH1c = tablaH1c.row(aa).node();
        $('#tabclon1bd').append("<tr id='trd" + aa + "'></tr>");
        $(rowH1c).children().each(function (td) {
            if (td !== 18 && td !== 19 && td !== 0) {
                $("#trd" + aa).append("<td>" + $(this).find('span:first').text().replace(/[^a-z0-9-/\s]/gi, '') + "</td>");
            }
        });
    }

    ////TAB2////
    $('#tab_test1').append("<table id='tab_test2clon' style='display:none'><thead id='tabclon2h'></thead><tbody id='tabclon2b'></tbody></table>");
    $('#tabclon2h').append("<tr id='titles2'></tr>");

    $('#tab_test2 > thead > tr > th').each(function () {
        if ($(this).text() !== "LABEL" && $(this).text() !== "ESTATUS") {
            $('#titles2').append("<th>" + $(this).text() + "</th>");
        } else {
        }

    });
    $('#tabclon2b').append("<tr><td colspan='9'></td></tr>");
    for (var bb = 0; bb < tablaH2c.rows().data().length; bb++) {
        var rowH2c = tablaH2c.row(bb).node();
        $('#tabclon2b').append("<tr id='tr2" + bb + "'></tr>");
        $(rowH2c).children().each(function (td2) {
            if (td2 !== 0) {
                $("#tr2" + bb).append("<td>" + $(this).find('span:first').text().replace(/[^a-z0-9-/\s]/gi, '') + "</td>");
            }
        });
    }
    ////TAB3////
    $('#tab_test1').append("<table id='tab_test3clon' style='display:none'><thead id='tabclon3h'></thead><tbody id='tabclon3b'></tbody></table>");
    $('#tabclon3h').append("<tr id='titles3'></tr>");

    $('#tab_test3 > thead > tr > th').each(function () {
        if ($(this).text() !== "LABEL" && $(this).text() !== "ESTATUS") {
            $('#titles3').append("<th>" + $(this).text() + "</th>");
        } else {
        }

    });
    $('#tabclon3b').append("<tr><td colspan='8'></td></tr>");
    for (var cc = 0; cc < tablaH3c.rows().data().length; cc++) {
        var rowH3c = tablaH3c.row(cc).node();
        $('#tabclon3b').append("<tr id='tr3" + cc + "'></tr>");
        $(rowH3c).children().each(function (td3) {
            if (td3 !== 0) {
                $("#tr3" + cc).append("<td>" + $(this).find('span:first').text().replace(/[^a-z0-9-/\s]/gi, '') + "</td>");
            }
        });
    }
    ////TAB4////
    $('#tab_test1').append("<table id='tab_test4clon' style='display:none'><thead id='tabclon4h'></thead><tbody id='tabclon4b'></tbody></table>");
    $('#tabclon4h').append("<tr id='titles4'></tr>");

    $('#tab_test4 > thead > tr > th').each(function () {
        if ($(this).text() !== "LABEL" && $(this).text() !== "ESTATUS") {
            $('#titles4').append("<th>" + $(this).text() + "</th>");
        } else {
        }

    });
    $('#tabclon4b').append("<tr><td colspan='14'></td></tr>");
    for (var dd = 0; dd < tablaH4c.rows().data().length; dd++) {
        var rowH4c = tablaH4c.row(dd).node();
        $('#tabclon4b').append("<tr id='tr4" + dd + "'></tr>");
        $(rowH4c).children().each(function (td4) {
            if (td4 !== 0) {
                $("#tr4" + dd).append("<td>" + $(this).find('span:first').text().replace(/[^a-z0-9-/\s]/gi, '') + "</td>");
            }
        });
    }
}

var tablesToExcel = (function () {
    var uri = 'data:application/vnd.ms-excel;base64,'
        , tmplWorkbookXML = '<?xml version="1.0"?><?mso-application progid="Excel.Sheet"?><Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet" xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet">'
            + '<DocumentProperties xmlns="urn:schemas-microsoft-com:office:office"><Author>Axel Richter</Author><Created>{created}</Created></DocumentProperties>'
            + '<Styles>'
            + '<Style ss:ID="Currency"><NumberFormat ss:Format="Currency"></NumberFormat></Style>'
            + '<Style ss:ID="Date"><NumberFormat ss:Format="Medium Date"></NumberFormat></Style>'
            + '</Styles>'
            + '{worksheets}</Workbook>'
        , tmplWorksheetXML = '<Worksheet ss:Name="{nameWS}"><Table>{rows}</Table></Worksheet>'
        , tmplCellXML = '<Cell{attributeStyleID}{attributeFormula}><Data ss:Type="{nameType}">{data}</Data></Cell>'
        , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
        , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }
    return function (tables, wsnames, wbname, appname) {
        var ctx = "";
        var workbookXML = "";
        var worksheetsXML = "";
        var rowsXML = "";

        for (var i = 0; i < tables.length; i++) {
            if (!tables[i].nodeType) tables[i] = document.getElementById(tables[i]);
            for (var j = 0; j < tables[i].rows.length; j++) {
                rowsXML += '<Row>'
                for (var k = 0; k < tables[i].rows[j].cells.length; k++) {
                    var dataType = tables[i].rows[j].cells[k].getAttribute("data-type");
                    var dataStyle = tables[i].rows[j].cells[k].getAttribute("data-style");
                    var dataValue = tables[i].rows[j].cells[k].getAttribute("data-value");
                    dataValue = (dataValue) ? dataValue : tables[i].rows[j].cells[k].innerHTML;
                    var dataFormula = tables[i].rows[j].cells[k].getAttribute("data-formula");
                    dataFormula = (dataFormula) ? dataFormula : (appname == 'Calc' && dataType == 'DateTime') ? dataValue : null;
                    ctx = {
                        attributeStyleID: (dataStyle == 'Currency' || dataStyle == 'Date') ? ' ss:StyleID="' + dataStyle + '"' : ''
                        , nameType: (dataType == 'Number' || dataType == 'DateTime' || dataType == 'Boolean' || dataType == 'Error') ? dataType : 'String'
                        , data: (dataFormula) ? '' : dataValue
                        , attributeFormula: (dataFormula) ? ' ss:Formula="' + dataFormula + '"' : ''
                    };
                    rowsXML += format(tmplCellXML, ctx);
                }
                rowsXML += '</Row>'
            }
            ctx = { rows: rowsXML, nameWS: wsnames[i] || 'Sheet' + i };
            worksheetsXML += format(tmplWorksheetXML, ctx);
            rowsXML = "";
        }

        ctx = { created: (new Date()).getTime(), worksheets: worksheetsXML };
        workbookXML = format(tmplWorkbookXML, ctx);



        var link = document.createElement("A");
        link.href = uri + base64(workbookXML);
        link.download = wbname || 'TestBook.xls';
        link.target = '_blank';
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }
})();
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


//function mandaArc(file) {
//    var formData = new FormData();
//    formData.append("FileUpload", file);
//    $.ajax({
//        type: "POST",
//        url: 'getDatosMasiva',
//        data: formData,
//    contentType: false,
//    processData: false
//}).fail(function () {
//    alert("error");
//});
//};

//function validaHoja1() {
//    $.ajax({
//        url: 'validaHoja1',
//    }).done(function (data) {
//        //$("table#test1 tbody").append(data);
//        $("#test1").append(data);
//    }).fail(function () {
//        alert("error");
//    });
//};

//function validaHoja2() {
//    $.ajax({
//        url: 'validaHoja2',
//    }).done(function (data) {
//        $('#test2').html(data);
//    }).fail(function () {
//        alert("error");
//    });
//};

//function validaHoja3() {
//    $.ajax({
//        url: 'validaHoja3',
//    }).done(function (data) {
//        $('#test3').html(data);
//    }).fail(function () {
//        alert("error");
//    });
//};

//function validaHoja4() {
//    $.ajax({
//        url: 'validaHoja4',
//    }).done(function (data) {
//        $('#test4').html(data);
//    }).fail(function () {
//        alert("error");
//    });
//};

//function validaHoja5() {
//    $.ajax({
//        url: 'validaHoja5',
//    }).done(function (data) {
//        $('#test5').html(data);
//    }).fail(function () {
//        alert("error");
//    });
//};