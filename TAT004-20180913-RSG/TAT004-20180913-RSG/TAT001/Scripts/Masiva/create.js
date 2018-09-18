//LEEMOS EL ARCHIVO UNA VEZ CARGADO AL ELEMENTO FILE INPUT
$("#miMas").change(function () {
    var filenum = $('#miMas').get(0).files.length;
    if (filenum > 0) {
        var file = document.getElementById("miMas").files[0];
        var filename = file.name;
        //EVALUAMOS LA EXTENSION PARA VER QUE SOLO PERMITA FORMATOS DE EXCEL
        if (evaluarExt(filename)) {
            M.toast({ html: 'Cargando ' + filename });
            document.getElementById("loader").style.display = "initial";
            getExcelMasivas(file);
            procesarHoja1();
            procesarHoja2();
            procesarHoja3();
            //procesarHoja4();
            //procesarHoja5();
            var elem = document.querySelectorAll('.miSel');
            var instance = M.Select.init(elem, []);
            document.getElementById("loader").style.display = "none";
        } else {
            M.toast({ html: 'Tipo de archivo incorrecto: ' + filename });
        }
    } else {
        M.toast({ html: 'Seleccione un archivo' });
    }
});

//$(document).ready(function () {
//    $('.miSel').;
//});



/////////////////////////////////////////////////////////SECCION DE FUNCIONES////////////////////////////////////////////////////////
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
        processData: false
    }).fail(function () {
        alert("error");
    });
};

function procesarHoja1() {
    var table = $('#tab_test1').DataTable();
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

                var arr2 = data.pop();
                var arr1 = data.pop();
                //INICIO DEL CICLO FOR
                $.each(data, function (i, dataj) {
                    var addedRow = addRowH1(table, dataj.NUM_DOC, dataj.TSOL_ID, dataj.GALL_ID, dataj.SOCIEDAD_ID, dataj.PAIS_ID, dataj.ESTADO, dataj.CIUDAD, dataj.CONCEPTO, dataj.NOTAS, dataj.PAYER_ID, dataj.VKORG, dataj.VTWEG, dataj.PAYER_NOMBRE, dataj.PAYER_EMAIL, dataj.FECHAI_VIG, dataj.FECHAF_VIG, dataj.MONEDA_ID, arr1, arr2);
                }); //FIN DEL FOR

                $('#tab_test1').css("font-size", "10px");
                $('#tab_test1').css("display", "table");
            }
        },
        error: function (xhr, httpStatusMessage, customErrorMessage) {
            alert("Request couldn't be processed. Please try again later. the reason " + xhr.status + " : " + httpStatusMessage + " : " + customErrorMessage);
        },
        async: false
    });

    //Actualizar los valores en la tabla
    //updateTable();
}

function addRowH1(t, NUM_DOC, TSOL_ID, GALL_ID, SOCIEDAD_ID, PAIS_ID, ESTADO, CIUDAD, CONCEPTO, NOTAS, PAYER_ID, VKORG, VTWEG, PAYER_NOMBRE, PAYER_EMAIL, FECHAI_VIG, FECHAF_VIG, MONEDA_ID, arr1, arr2){

    var clasificacion = "<select id=\"clas\" class=\"miSel\">";
    $.each(arr1, function (i, data) {
        if (data == GALL_ID){
            clasificacion += "<option value=\"" + i + "\" selected>" + data + "</option>";
        }
        else {
            clasificacion += "<option value=\"" + i + "\">" + data + "</option>";
        }
    });
    clasificacion = clasificacion + "</select>";

    var moneda = "<select style=\"font-size:10px;\" id=\"mon\" class=\"miSel\">";
    $.each(arr2, function (i, data2) {
        if (data2 == MONEDA_ID) {
            moneda += "<option value=\"" + i + "\" selected>" + data2 + "</option>";
        }
        else {
            moneda += "<option value=\"" + i + "\">" + data2 + "</option>";
        }
    });

    moneda = moneda + "</select>";

    var r = t.row.add([
        "<input class='numberd' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + NUM_DOC + "'>",
        "<input class='' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + TSOL_ID + "'>",
        //clasificacion,
        "<input class='input_clasificacion' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + GALL_ID + "'>",
        "<input class='' style='font-size:10px; text-align:center;' type='text' id='soc' name='' value='" + SOCIEDAD_ID + "'>",
        "<input class='input_pais' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + PAIS_ID + "'>",
        "<input class='' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + ESTADO + "'>",
        "<input class='' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + CIUDAD + "'>",
        "<input class='' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + CONCEPTO + "'>",
        "<input class='' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + NOTAS + "'>",
        "<input class='' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + PAYER_ID + "'>",
        "<input class='' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + PAYER_NOMBRE + "'>",
        "<input class='' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + PAYER_EMAIL + "'>",
        "<input class='' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + FECHAI_VIG + "'>",
        "<input class='' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + FECHAF_VIG + "'>",
        "<input class='input_moneda' style='font-size:10px; text-align:center;' type='text' id='' name='' value='" + MONEDA_ID + "'>",
        //moneda,
        "<input id='txt_vkorg' value='" + VKORG + "' hidden>",
        "<input id='txt_vtweg' value='" + VTWEG + "' hidden>"
    ]).draw(false).node();

    return r;
}

$('body').on('keydown.autocomplete', '.input_clasificacion', function () {

    //var tr = $(this).closest('tr'); //Obtener el row
    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'clasificacion',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.DESCRIPCION };
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
                e.target.value = "";
            }
        },

        select: function (event, ui) {
            //var label = ui.item.label;
            //var value = ui.item.value;
        }
    });
});

$('body').on('keydown.autocomplete', '.input_moneda', function () {

    //var tr = $(this).closest('tr'); //Obtener el row
    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'moneda',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.WAERS };
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
                e.target.value = "";
            }
        },

        select: function (event, ui) {
            //var label = ui.item.label;
            //var value = ui.item.value;
        }
    });
});

$('body').on('keydown.autocomplete', '.input_pais', function () {

    var row_index = $(this).parent('tr').index();
    var col_index = $(this).index();
    console.log(this);


    var fila = $('#tab_test1 tbody tr').index();
    var soc = document.getElementById("soc").value;

    if (soc.toUpperCase() == 'LPKP') {
        auto(this).autocomplete({
            source: function (request, response) {
                auto.ajax({
                    type: "POST",
                    url: 'pais',
                    dataType: "json",
                    data: { "Prefix": request.term },
                    success: function (data) {
                        response(auto.map(data, function (item) {
                            return { label: item.WAERS };
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
                    e.target.value = "";
                }
            },

            select: function (event, ui) {
                //var label = ui.item.label;
                //var value = ui.item.value;
            }
        });
    }
});






function procesarHoja2() {
    var table = $('#tab_test2').DataTable();
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

                var arr1 = data.pop();

                //INICIO DEL CICLO FOR
                $.each(data, function (i, dataj) {

                    var addedRow = addRowH2(table, dataj.NUM_DOC, dataj.FACTURA, dataj.FECHA, dataj.PROVEEDOR, dataj.PROVEEDOR_NOMBRE, dataj.AUTORIZACION, dataj.VENCIMIENTO, dataj.FACTURAK, dataj.EJERCICIOK, arr1);

                }); //FIN DEL FOR

                $('#tab_test2').css("font-size", "10px");
                $('#tab_test2').css("display", "table");
            }
        },
        error: function (xhr, httpStatusMessage, customErrorMessage) {
            alert("Request couldn't be processed. Please try again later. the reason " + xhr.status + " : " + httpStatusMessage + " : " + customErrorMessage);
        },
        async: false
    });

    //Actualizar los valores en la tabla
    //updateTable();
}

function addRowH2(t, NUM_DOC, FACTURA, FECHA, PROVEEDOR, PROVEEDOR_NOMBRE, AUTORIZACION, VENCIMIENTO, FACTURAK, EJERCICIOK, arr1) {

    var proveedor = "<select id=\"pro\" class=\"miSel\">";
    $.each(arr1, function (i, data) {
        if (data == PROVEEDOR) {
            proveedor += "<option value=\"" + i + "\" selected>" + data + "</option>";
        }
        else {
            proveedor += "<option value=\"" + i + "\">" + data + "</option>";
        }
    });
    proveedor = proveedor + "</select>";

    var r = t.row.add([
        "<input class=\"input_oper numberd \" style=\"font-size:12px; text-align:center;\" type=\"text\" id=\"\" name=\"\" value=\"" + NUM_DOC + "\">",
        "<input class=\"input_oper \" style=\"font-size:10px; text-align:center;\" type=\"text\" id=\"\" name=\"\" value=\"" + FACTURA + "\">",
        "<input class=\"input_oper \" style=\"font-size:10px; text-align:center;\" type=\"text\" id=\"\" name=\"\" value=\"" + FECHA + "\">",
        proveedor,
        "<input class=\"input_oper \" style=\"font-size:10px; text-align:center;\" type=\"text\" id=\"\" name=\"\" value=\"" + PROVEEDOR_NOMBRE + "\">",
        "<input class=\"input_oper \" style=\"font-size:10px; text-align:center;\" type=\"text\" id=\"\" name=\"\" value=\"" + AUTORIZACION + "\">",
        "<input class=\"input_oper \" style=\"font-size:10px; text-align:center;\" type=\"text\" id=\"\" name=\"\" value=\"" + VENCIMIENTO + "\">",
        "<input class=\"input_oper \" style=\"font-size:10px; text-align:center;\" type=\"text\" id=\"\" name=\"\" value=\"" + FACTURAK + "\">",
        "<input class=\"input_oper \" style=\"font-size:10px; text-align:center;\" type=\"text\" id=\"\" name=\"\" value=\"" + EJERCICIOK + "\">"
    ]).draw(false).node();

    return r;
}

function procesarHoja3() {
    var table = $('#tab_test3').DataTable();
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

                //INICIO DEL CICLO FOR
                $.each(data, function (i, dataj) {

                    var addedRow = addRowH3(table, dataj.NUM_DOC, dataj.FACTURA, dataj.BILL_DOC, dataj.EJERCICIOK, dataj.PAYER, dataj.IMPORTE_FAC, dataj.BELNR);

                }); //FIN DEL FOR

                $('#tab_test3').css("font-size", "10px");
                $('#tab_test3').css("display", "table");
            }
        },
        error: function (xhr, httpStatusMessage, customErrorMessage) {
            alert("Request couldn't be processed. Please try again later. the reason " + xhr.status + " : " + httpStatusMessage + " : " + customErrorMessage);
        },
        async: false
    });

    //Actualizar los valores en la tabla
    //updateTable();
}

function addRowH3(t, NUM_DOC, FACTURA, BILL_DOC, EJERCICIOK, PAYER, IMPORTE_FAC, BELNR) {
    var r = t.row.add([
        "<input class=\" numberd \" style=\"font-size:12px; text-align:center;\" type=\"text\" id=\"\" name=\"\" value=\"" + NUM_DOC + "\">",
        "<input class=\" \" style=\"font-size:10px; text-align:center;\" type=\"text\" id=\"\" name=\"\" value=\"" + FACTURA + "\">",
        "<input class=\" \" style=\"font-size:10px; text-align:center;\" type=\"text\" id=\"\" name=\"\" value=\"" + BILL_DOC + "\">",
        "<input class=\" \" style=\"font-size:10px; text-align:center;\" type=\"text\" id=\"\" name=\"\" value=\"" + EJERCICIOK + "\">",
        "<input class=\" \" style=\"font-size:10px; text-align:center;\" type=\"text\" id=\"\" name=\"\" value=\"" + PAYER + "\">",
        "<input class=\" \" style=\"font-size:10px; text-align:center;\" type=\"text\" id=\"\" name=\"\" value=\"" + IMPORTE_FAC + "\">",
        "<input class=\" \" style=\"font-size:10px; text-align:center;\" type=\"text\" id=\"\" name=\"\" value=\"" + BELNR + "\">"
    ]).draw(false).node();

    return r;
}

































//FUNCION PARA MANTENER CEROS A LA IZQUIERDA
function trimStart(character, string) {//RSG 07.06.2018
    var startIndex = 0;

    while (string[startIndex] === character) {
        startIndex++;
    }

    return string.substr(startIndex);
}













































//function procesarHoja2() {
//    var table = $('#tab_test2').DataTable();
//    table.clear().draw();

//    $.ajax({
//        type: "POST",
//        url: 'validaHoja2',
//        dataType: "json",
//        cache: false,
//        contentType: false,
//        processData: false,
//        success: function (data) {

//            if (data !== null || data !== "") {

//                //INICIO DEL CICLO FOR
//                $.each(data, function (i, dataj) {

//                    var addedRow = addRowH2(table, dataj.NUM_DOC, dataj.VIGENCIA_DE, dataj.VIGENCIA_AL, dataj.MATNR, dataj.MATKL, dataj.MONTO, dataj.PORC_APOYO, dataj.PRECIO_SUG, dataj.VOLUMEN_REAL, dataj.APOYO);

//                }); //FIN DEL FOR

//                $('#tab_test2').css("font-size", "10px");
//                $('#tab_test2').css("display", "table");
//            }
//        },
//        error: function (xhr, httpStatusMessage, customErrorMessage) {
//            alert("Request couldn't be processed. Please try again later. the reason " + xhr.status + " : " + httpStatusMessage + " : " + customErrorMessage);
//        },
//        async: false
//    });

//    //Actualizar los valores en la tabla
//    //updateTable();
//}

//function addRowH2(t, NUM_DOC, VIGENCIA_DE, VIGENCIA_AL, MATNR, MATKL, MONTO, PORC_APOYO, PRECIO_SUG, VOLUMEN_REAL, APOYO) {
//    var r = t.row.add([
//        "<input class=\"input_oper numberd \" style=\"font-size:12px; text-align:center;\" type=\"text\" id=\"\" name=\"\" value=\"" + NUM_DOC + "\">",
//        "<input class=\"input_oper \" style=\"font-size:10px; text-align:center;\" type=\"text\" id=\"\" name=\"\" value=\"" + VIGENCIA_DE + "\">",
//        "<input class=\"input_oper \" style=\"font-size:10px; text-align:center;\" type=\"text\" id=\"\" name=\"\" value=\"" + VIGENCIA_AL + "\">",
//        "<input class=\"input_oper \" style=\"font-size:10px; text-align:center;\" type=\"text\" id=\"\" name=\"\" value=\"" + MATNR + "\">",
//        "<input class=\"input_oper \" style=\"font-size:10px; text-align:center;\" type=\"text\" id=\"\" name=\"\" value=\"" + MATKL + "\">",
//        "<input class=\"input_oper \" style=\"font-size:10px; text-align:center;\" type=\"text\" id=\"\" name=\"\" value=\"" + MONTO + "\">",
//        "<input class=\"input_oper \" style=\"font-size:10px; text-align:center;\" type=\"text\" id=\"\" name=\"\" value=\"" + PORC_APOYO + "\">",
//        "<input class=\"input_oper \" style=\"font-size:10px; text-align:center;\" type=\"text\" id=\"\" name=\"\" value=\"" + PRECIO_SUG + "\">",
//        "<input class=\"input_oper \" style=\"font-size:10px; text-align:center;\" type=\"text\" id=\"\" name=\"\" value=\"" + VOLUMEN_REAL + "\">",
//        "<input class=\"input_oper \" style=\"font-size:10px; text-align:center;\" type=\"text\" id=\"\" name=\"\" value=\"" + APOYO + "\">"
//    ]).draw(false).node();

//    return r;
//}



//function procesarHoja4() {
//    var table = $('#tab_test4').DataTable();
//    table.clear().draw();

//    $.ajax({
//        type: "POST",
//        url: 'validaHoja4',
//        dataType: "json",
//        cache: false,
//        contentType: false,
//        processData: false,
//        success: function (data) {

//            if (data !== null || data !== "") {

//                var textos = data.pop();
//                //INICIO DEL CICLO FOR
//                for (var i = 0; i < data.length; i++) {

//                    for (var j = 0; j < textos.length; j++){

//                        var addedRow = addRowH4(table, data[i]);

//                    }
//                }
//                //FIN DEL FOR

//                $('#tab_test4').css("font-size", "10px");
//                $('#tab_test4').css("display", "table");
//            }
//        },
//        error: function (xhr, httpStatusMessage, customErrorMessage) {
//            alert("Request couldn't be processed. Please try again later. the reason " + xhr.status + " : " + httpStatusMessage + " : " + customErrorMessage);
//        },
//        async: false
//    });

//    //Actualizar los valores en la tabla
//    //updateTable();
//}

//function addRowH4(t, NUM_DOC) {
//    var r = t.row.add([
//        "<input class=\"input_oper numberd \" style=\"font-size:12px; text-align:center;\" type=\"text\" id=\"\" name=\"\" value=\"" + NUM_DOC + "\">",
//        "<input class=\"input_oper \" style=\"font-size:10px; text-align:center;\" type=\"text\" id=\"\" name=\"\" value=\"\">",
//        "<div class=\"file-field input-field \"><div class=\"btn\"><span>Seleccionar archivo</span><input type=\"file\" name=\"\" id=\"\"></div></div>"
//        //"<input class=\"btn \" style=\"font-size:10px; text-align:center;\" type=\"file\" id=\"\" name=\"\" value=\"\">",
//    ]).draw(false).node();

//    return r;
//}


//function addRowH11(t, NUM_DOC, TSOL_ID, GALL_ID, SOCIEDAD_ID, PAIS_ID, ESTADO, CIUDAD, CONCEPTO, NOTAS, PAYER_ID, PAYER_NOMBRE, PAYER_EMAIL, FECHAI_VIG, FECHAF_VIG, MONEDA_ID) {
//    var r = t.row.add([
//        NUM_DOC,
//        TSOL_ID,
//        GALL_ID,
//        SOCIEDAD_ID,
//        PAIS_ID,
//        ESTADO,
//        CIUDAD,
//        CONCEPTO,
//        NOTAS,
//        PAYER_ID,
//        PAYER_NOMBRE,
//        PAYER_EMAIL,
//        FECHAI_VIG,
//        FECHAF_VIG,
//        MONEDA_ID
//    ]).draw(false).node();

//    return r;
//}

//function addRowH22(t, NUM_DOC, VIGENCIA_DE, VIGENCIA_AL, MATNR, MATKL, MONTO, PORC_APOYO, PRECIO_SUG, VOLUMEN_REAL, APOYO) {
//    var r = t.row.add([
//        NUM_DOC,
//        VIGENCIA_DE,
//        VIGENCIA_AL,
//        MATNR,
//        MATKL,
//        MONTO,
//        PORC_APOYO,
//        PRECIO_SUG,
//        VOLUMEN_REAL,
//        APOYO
//    ]).draw(false).node();

//    return r;
//}


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