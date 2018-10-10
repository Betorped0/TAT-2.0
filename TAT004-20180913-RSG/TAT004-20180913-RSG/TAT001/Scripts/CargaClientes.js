$(document).ready(function () {

    $('#table').DataTable({
        "scrollY": "200",
        "scrollX": "true",
        "scrollCollapse": true,
        "order": [],
        "language": {
            "url": "../Scripts/lang/@Session['spras'].ToString()" + ".json",
            "zerorecords": "no hay registros",
            "infoempty": "registros no disponibles"
        },
        "paging": false,
        "info": false,
        "searching": false,

        "columns": [
            {
                "name": 'Borrar',
                "className": 'Borrar'
            },
            {
                "name": 'CoCode',
                "className": 'CoCode'
            },
            {
                "name": 'Pais',
                "className": 'Pais'
            },
            {
                "name": 'Cliente',
                "className": 'Cliente'
            },
            {
                "name": 'Nombre_cliente',
                "className": 'Nombre_cliente',
            },
            {
                "name": 'Nivel_0',
                "className": 'Nivel_0'
            },
            {
                "name": 'Nivel_1',
                "className": 'Nivel_1'
            },
            {
                "name": 'Nivel_2',
                "className": 'Nivel_2'
            },
            {
                "name": 'Nivel_3',
                "className": 'Nivel_3'
            },
            {
                "name": 'Nivel_4',
                "className": 'Nivel_4'
            },
            {
                "name": 'Nivel_5',
                "className": 'Nivel_5'
            },
            {
                "name": 'Nivel_6',
                "className": 'Nivel_6'
            },
            {
                "name": 'Nivel_7',
                "className": 'Nivel_7'
            },
            {
                "name": 'Vendor',
                "className": 'Vendor'
            },
            {
                "name": 'Banner',
                "className": 'Banner'
            },
            {
                "name": 'Banner_Agrupador',
                "className": 'Banner_Agrupador'
            },
            {
                "name": 'Canal',
                "className": 'Canal'
            },
            {
                "name": 'Exportacion',
                "className": 'Exportacion'
            },
            {
                "name": 'Contacto',
                "className": 'Contacto'
            },
            {
                "name": 'Email_Contacto',
                "className": 'Email_Contacto'
            },
            {
                "name": 'Mensaje',
                "className": 'Mensaje'
            },
        ]
    });
});

$("#files").on('change', function() {
    var filenum = $('#files').get(0).files.length;
    if (filenum > 0) {
        var file = document.getElementById("files").files[0];
        var filename = file.name;
        if (evaluarExt(filename)) {
            M.toast({ html: 'Cargando ' + filename });
            loadExcelDis(file);
            onclick = checkoff();
        } else {
            document.getElementById("files").value = "";
            M.toast({ html: 'Tipo de archivo incorrecto: ' + filename });
        }
    } else {
        M.toast({ html: 'Seleccione un archivo' });
        var table = $('#table').DataTable();
        table.clear().draw();
    }
    sessionStorage.setItem("num", filenum);
});

function evaluarExt(filename) {

    var exts = ['xls', 'xlsx'];
    // split file name at dot
    var get_ext = filename.split('.');
    // reverse name to check extension
    get_ext = get_ext.reverse();
    // check file type is valid as given in 'exts' array
    if ($.inArray(get_ext[0].toLowerCase(), exts) > -1) {
        return true;
    } else {
        return false;
    }
}

function loadExcelDis(file) {

    document.getElementById("loader").style.display = "initial";
    var formData = new FormData();

    formData.append("FileUpload", file);

    var table = $('#table').DataTable();
    table.clear().draw();
    $.ajax({
        type: "POST",
        url: 'LoadExcel',
        data: formData,
        dataType: "json",
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {

            if (data !== null || data !== "") {

                $.each(data, function (i, dataj) {

                    var bor = i;
                    var coc = dataj.BUKRS;
                    var pai = dataj.LAND;
                    var cli = dataj.KUNNR;
                    var noc = dataj.CLIENTE_N;
                    var ni0 = dataj.ID_US0;
                    var ni1 = dataj.ID_US1;
                    var ni2 = dataj.ID_US2;
                    var ni3 = dataj.ID_US3;
                    var ni4 = dataj.ID_US4;
                    var ni5 = dataj.ID_US5;
                    var ni6 = dataj.ID_US6;
                    var ni7 = dataj.ID_US7;
                    var ven = dataj.ID_PROVEEDOR;
                    var ban = dataj.BANNER;
                    var baa = dataj.BANNERG;
                    var can = dataj.CANAL;
                    var exp = dataj.EXPORTACION;
                    var con = dataj.CONTACTO;
                    var eco = dataj.CONTACTOE;
                    var mes = dataj.MESS;

                    //identificacion de error
                    if (coc.indexOf('?') != -1) {
                        coc = coc.slice(0, -1);
                        var cocx = true;
                    }
                    if (pai.indexOf('?') != -1) {
                        pai = pai.slice(0, -1);
                        var paix = true;
                    }
                    if (cli.indexOf('?') != -1) {
                        cli = cli.slice(0, -1);
                        var clix = true;
                    }
                    if (ni0.indexOf('?') != -1) {
                        ni0 = ni0.slice(0, -1);
                        var ni0x = true;
                    }
                    if (ni1.indexOf('?') != -1) {
                        ni1 = ni1.slice(0, -1);
                        var ni1x = true;
                    }
                    if (ni2.indexOf('?') != -1) {
                        ni2 = ni2.slice(0, -1);
                        var ni2x = true;
                    }
                    if (ni3.indexOf('?') != -1) {
                        ni3 = ni3.slice(0, -1);
                        var ni3x = true;
                    }
                    if (ni4.indexOf('?') != -1) {
                        ni4 = ni4.slice(0, -1);
                        var ni4x = true;
                    }
                    if (ni5.indexOf('?') != -1) {
                        ni5 = ni5.slice(0, -1);
                        var ni5x = true;
                    }
                    if (ni6.indexOf('?') != -1) {
                        ni6 = ni6.slice(0, -1);
                        var ni6x = true;
                    }
                    if (ni7.indexOf('?') != -1) {
                        ni7 = ni7.slice(0, -1);
                        var ni7x = true;
                    }
                    if (ven.indexOf('?') != -1) {
                        ven = ven.slice(0, -1);
                        var venx = true;
                    }
                    if (can.indexOf('?') != -1) {
                        can = can.slice(0, -1);
                        var canx = true;
                    }
                    if (eco.indexOf('?') != -1) {
                        eco = eco.slice(0, -1);
                        var ecox = true;
                    }//Termina identificacion

                    var addedRow = addRow(table, dataj.POS, bor, coc, pai, cli, noc, ni0, ni1, ni2, ni3, ni4, ni5, ni6, ni7, ven, ban, baa, can, exp, con, eco, mes);

                    //Pintar de rojo las celdas
                    var cols = addedRow.cells[1];
                    if (cocx == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[2];
                    if (paix == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[3];
                    if (clix == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[5];
                    if (ni0x == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[6];
                    if (ni1x == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[7];
                    if (ni2x == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[8];
                    if (ni3x == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[9];
                    if (ni4x == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[10];
                    if (ni5x == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[11];
                    if (ni6x == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[12];
                    if (ni7x == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[13];
                    if (venx == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[16];
                    if (canx == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[19];
                    if (ecox == true) {
                        $(cols).addClass("red");
                    }
                });
                $('#table_dis').css("font-size", "12px");
                $('#table_dis').css("display", "table");
                $('#tfoot_dis').css("display", "table-footer-group");
                document.getElementById("loader").style.display = "none";
            }
        },
        error: function (xhr, httpStatusMessage, customErrorMessage) {
            M.toast({
                html: "Request couldn't be processed. Please try again later. the reason        " + xhr.status + " : " + httpStatusMessage + ": " + customErrorMessage
            });
            document.getElementById("loader").style.display = "none";
        },
        async: true
    });
    $("#table > tbody  > tr[role='row']").each(function () {
        alert("as");
    });
}

function addRow(t, POS, br, cc, p, c, nc, n0, n1, n2, n3, n4, n5, n6, n7, v, b, ba, ca, ex, co, ec, me) {
    var r = addRowl(
        t,
        POS,
        "<label><input class='input_bor' type='checkbox' id='' name='bor' onclick='checkoff();' value='" + br + "'><span></span></label>",
        "<input class='input_coc' style='font-size:12px;' type='text' id='' name='coc' value='" + cc + "' onchange='Comprobar()'><span  style='display: none;'>" + cc + "</span>",
        "<input class='input_pai' style='font-size:12px;' type='text' id='' name='pai' value='" + p + "' onchange='Comprobar()'><span  style='display: none;'>" + p + "</span>",
        "<input class='input_cli' style='font-size:12px;' type='text' id='' name='cli' value='" + c + "' onchange='Comprobar()'><span  style='display: none;'>" + c + "</span>",
        "<input class='input_noc' style='font-size:12px;' type='text' id='' name='noc' value='" + nc + "' onchange='Comprobar()'><span  style='display: none;'>" + nc + "</span>",
        "<input class='input_ni0' style='font-size:12px;' type='text' id='' name='ni0' value='" + n0 + "' onchange='Comprobar()'><span  style='display: none;'>" + n0 + "</span>",
        "<input class='input_ni1' style='font-size:12px;' type='text' id='' name='ni1' value='" + n1 + "' onchange='Comprobar()'><span  style='display: none;'>" + n1 + "</span>",
        "<input class='input_ni2' style='font-size:12px;' type='text' id='' name='ni2' value='" + n2 + "' onchange='Comprobar()'><span  style='display: none;'>" + n2 + "</span>",
        "<input class='input_ni3' style='font-size:12px;' type='text' id='' name='ni3' value='" + n3 + "' onchange='Comprobar()'><span  style='display: none;'>" + n3 + "</span>",
        "<input class='input_ni4' style='font-size:12px;' type='text' id='' name='ni4' value='" + n4 + "' onchange='Comprobar()'><span  style='display: none;'>" + n4 + "</span>",
        "<input class='input_ni5' style='font-size:12px;' type='text' id='' name='ni5' value='" + n5 + "' onchange='Comprobar()'><span  style='display: none;'>" + n5 + "</span>",
        "<input class='input_ni6' style='font-size:12px;' type='text' id='' name='ni6' value='" + n6 + "' onchange='Comprobar()'><span  style='display: none;'>" + n6 + "</span>",
        "<input class='input_ni7' style='font-size:12px;' type='text' id='' name='ni7' value='" + n7 + "' onchange='Comprobar()'><span  style='display: none;'>" + n7 + "</span>",
        "<input class='input_ven' style='font-size:12px;' type='text' id='' name='ven' value='" + v + "' onchange='Comprobar()'><span  style='display: none;'>" + v + "</span>",
        "<input class='input_ban' style='font-size:12px;' type='text' id='' name='ban' value='" + b + "' onchange='Comprobar()'><span  style='display: none;'>" + b + "</span>",
        "<input class='input_baa' style='font-size:12px;' type='text' id='' name='baa' value='" + ba + "' onchange='Comprobar()'><span  style='display: none;'>" + ba + "</span>",
        "<input class='input_can' style='font-size:12px;' type='text' id='' name='can' value='" + ca + "' onchange='Comprobar()'><span  style='display: none;'>" + ca+ "</span>",
        "<input class='input_exp' style='font-size:12px;' type='text' id='' name='exp' value='" + ex + "' onchange='Comprobar()'><span  style='display: none;'>" + ex + "</span>",
        "<input class='input_con' style='font-size:12px;' type='text' id='' name='con' value='" + co + "' onchange='Comprobar()'><span  style='display: none;'>" + co + "</span>",
        "<input class='input_eco' style='font-size:12px;' type='text' id='' name='eco' value='" + ec + "' onchange='Comprobar()'><span  style='display: none;'>" + ec + "</span>",
        "<input class='input_mes' type='hidden' name='mes' value='" + me + "'><span class='input_mes' style='font-size:12px;'>" + me + "</span>"
    );
    return r;
}

function addRowl(t, pos, br, cc, p, c, nc, n0, n1, n2, n3, n4, n5, n6, n7, v, b, ba, ca, ex, co, ec, me) {
    var r = t.row.add([
        br,
        cc,
        p,
        c,
        nc,
        n0,
        n1,
        n2,
        n3,
        n4,
        n5,
        n6,
        n7,
        v,
        b,
        ba,
        ca,
        ex,
        co,
        ec,
        me
    ]).draw(false).node();
    return r;
}

function Carga() {
    var datos = $('#tabla').serializeArray();
    var message = $('.input_mes').serialize();
    var cliente = $('.input_cli').serialize();
    console.log(datos);
    console.log(message);
    var us1 = $('.input_ni1').serialize();
    var us6 = $('.input_ni6').serialize();
    var doc = sessionStorage.getItem("num");
    if (doc > 0 || cliente != null || cliente != "cli=") {
        if (message == "" || message == null || message == "mes=") {
            if (us1 != "" && us1 != "ni1=" && us1 != null && us6 != "" && us6 != "ni6=" && us6 != null) {
                $.ajax({
                    type: "POST",
                    url: 'Agregar',
                    data: datos,
                    dataType: "json",
                    success: function () {

                    },
                    error: function (request, status, error) {
                        //alert(request.responseText);
                    }
                });
                M.toast({ html: 'Se agregaron los nuevos registros' });
                window.location.replace("/Clientes/Index");
            }
            else
                M.toast({ html: 'Los niveles 1 y 6 no pueden quedar vacios' });
        }
        else
            M.toast({ html: 'Hay errores por corregir' });
    }
    else
        M.toast({ html: 'Seleccione un archivo' });
}

function Comprobar() {
    var datos = $('#tabla').serializeArray();
    creart('Comprobar', datos);
    M.toast({ html: 'Registros Actualizados' });
}

function Borrar() {
    var table = $('#table').DataTable();
    var rows = $('.input_bor').serializeArray();
    for (var i = rows.length; i > 0; i--) {
        var num = rows[i-1].value;
        table.row(num).remove().draw();
    }
    Comprobar();
}

function Actualizar() {
    var datos = $('#tabla').serializeArray();
    creart('Actualizar', datos);
}

function creart(metodo, datos) {
    var table = $('#table').DataTable();
    $.ajax({
        type: "POST",
        url: metodo,
        dataType: "json",
        data: datos,
        success: function (data) {
            if (data !== null || data !== "") {

                table.clear().draw();

                $.each(data, function (i, dataj) {

                    var bor = i;
                    var coc = dataj.BUKRS;
                    var pai = dataj.LAND;
                    var cli = dataj.KUNNR;
                    var noc = dataj.CLIENTE_N;
                    var ni0 = dataj.ID_US0;
                    var ni1 = dataj.ID_US1;
                    var ni2 = dataj.ID_US2;
                    var ni3 = dataj.ID_US3;
                    var ni4 = dataj.ID_US4;
                    var ni5 = dataj.ID_US5;
                    var ni6 = dataj.ID_US6;
                    var ni7 = dataj.ID_US7;
                    var ven = dataj.ID_PROVEEDOR;
                    var ban = dataj.BANNER;
                    var baa = dataj.BANNERG;
                    var can = dataj.CANAL;
                    var exp = dataj.EXPORTACION;
                    var con = dataj.CONTACTO;
                    var eco = dataj.CONTACTOE;
                    var mes = dataj.MESS;

                    //identificacion de error
                    if (coc.indexOf('?') != -1) {
                        coc = coc.slice(0, -1);
                        var cocx = true;
                    }
                    if (pai.indexOf('?') != -1) {
                        pai = pai.slice(0, -1);
                        var paix = true;
                    }
                    if (cli.indexOf('?') != -1) {
                        cli = cli.slice(0, -1);
                        var clix = true;
                    }
                    if (ni0.indexOf('?') != -1) {
                        ni0 = ni0.slice(0, -1);
                        var ni0x = true;
                    }
                    if (ni1.indexOf('?') != -1) {
                        ni1 = ni1.slice(0, -1);
                        var ni1x = true;
                    }
                    if (ni2.indexOf('?') != -1) {
                        ni2 = ni2.slice(0, -1);
                        var ni2x = true;
                    }
                    if (ni3.indexOf('?') != -1) {
                        ni3 = ni3.slice(0, -1);
                        var ni3x = true;
                    }
                    if (ni4.indexOf('?') != -1) {
                        ni4 = ni4.slice(0, -1);
                        var ni4x = true;
                    }
                    if (ni5.indexOf('?') != -1) {
                        ni5 = ni5.slice(0, -1);
                        var ni5x = true;
                    }
                    if (ni6.indexOf('?') != -1) {
                        ni6 = ni6.slice(0, -1);
                        var ni6x = true;
                    }
                    if (ni7.indexOf('?') != -1) {
                        ni7 = ni7.slice(0, -1);
                        var ni7x = true;
                    }
                    if (ven.indexOf('?') != -1) {
                        ven = ven.slice(0, -1);
                        var venx = true;
                    }
                    if (can.indexOf('?') != -1) {
                        can = can.slice(0, -1);
                        var canx = true;
                    }
                    if (eco.indexOf('?') != -1) {
                        eco = eco.slice(0, -1);
                        var ecox = true;
                    }//Termina identificacion

                    var addedRow = addRow(table, dataj.POS, bor, coc, pai, cli, noc, ni0, ni1, ni2, ni3, ni4, ni5, ni6, ni7, ven, ban, baa, can, exp, con, eco, mes);

                    //Pintar de rojo las celdas
                    var cols = addedRow.cells[1];
                    if (cocx == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[2];
                    if (paix == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[3];
                    if (clix == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[5];
                    if (ni0x == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[6];
                    if (ni1x == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[7];
                    if (ni2x == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[8];
                    if (ni3x == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[9];
                    if (ni4x == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[10];
                    if (ni5x == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[11];
                    if (ni6x == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[12];
                    if (ni7x == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[13];
                    if (venx == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[16];
                    if (canx == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[19];
                    if (ecox == true) {
                        $(cols).addClass("red");
                    }
                });
                $('#table_dis').css("font-size", "12px");
                $('#table_dis').css("display", "table");
                $('#tfoot_dis').css("display", "table-footer-group");
                document.getElementById("loader").style.display = "none";
            }
        },
        error: function (xhr, httpStatusMessage, customErrorMessage) {
            M.toast({
                html: "Request couldn't be processed. Please try again later. the reason        " + xhr.status + " : " + httpStatusMessage + ": " + customErrorMessage
            });
            document.getElementById("loader").style.display = "none";
        },
        async: true
    });
}

function check() {
    if ($("#borrar").prop('checked'))
        $(".input_bor").prop('checked', true);
    else
        $(".input_bor").prop('checked', false);
}

function checkoff() {
    $("#borrar").prop('checked', false);
}

$('body').on('keydown.autocomplete', '.input_ven', function () {

    //var tr = $(this).closest('tr'); //Obtener el row
    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'Vendor',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.ID + " | " + item.NOMBRE, value: item.ID };
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

$('body').on('keydown.autocomplete', '.input_can', function () {

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'Canal',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.CANAL1 + " | " + item.CDESCRIPCION, value: item.CANAL1 };
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
        }
    });
});

$('body').on('keydown.autocomplete', '.input_cli', function () {

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'Cliente',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.KUNNR + " | " + item.NAME1, value: item.KUNNR };
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
        }
    });
});

$('body').on('keydown.autocomplete', '.input_coc', function () {

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'Company',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.BUKRS + " | " + item.NAME1, value: item.BUKRS };
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
        }
    });
});

$('body').on('keydown.autocomplete', '.input_ni0', function () {

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'Usuario',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.ID + " | " + item.NOMBRE + " " + item.APELLIDO_P, value: item.ID };
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
        }
    });
});

$('body').on('keydown.autocomplete', '.input_ni1', function () {

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'Usuario',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.ID + " | " + item.NOMBRE + " " + item.APELLIDO_P, value: item.ID };
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
        }
    });
});

$('body').on('keydown.autocomplete', '.input_ni2', function () {

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'Usuario',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.ID + " | " + item.NOMBRE + " " + item.APELLIDO_P, value: item.ID };
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
        }
    });
});

$('body').on('keydown.autocomplete', '.input_ni3', function () {

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'Usuario',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.ID + " | " + item.NOMBRE + " " + item.APELLIDO_P, value: item.ID };
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
        }
    });
});

$('body').on('keydown.autocomplete', '.input_ni4', function () {

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'Usuario',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.ID + " | " + item.NOMBRE + " " + item.APELLIDO_P, value: item.ID };
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
        }
    });
});

$('body').on('keydown.autocomplete', '.input_ni5', function () {

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'Usuario',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.ID + " | " + item.NOMBRE + " " + item.APELLIDO_P, value: item.ID };
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
        }
    });
});

$('body').on('keydown.autocomplete', '.input_ni6', function () {

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'Usuario',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.ID + " | " + item.NOMBRE + " " + item.APELLIDO_P, value: item.ID };
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
        }
    });
});

$('body').on('keydown.autocomplete', '.input_ni7', function () {

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'Usuario',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.ID + " | " + item.NOMBRE + " " + item.APELLIDO_P, value: item.ID };
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
        }
    });
});


