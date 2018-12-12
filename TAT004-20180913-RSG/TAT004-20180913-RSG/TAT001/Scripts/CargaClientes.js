$(document).ready(function () {
    formato();
});
var esFile = false;
function formato() {
    $('#table').DataTable({
        "order": [],
        "language": {
            "url": root + "Scripts/lang/" + spras + ".json",
            "zerorecords": "no hay registros",
            "infoempty": "registros no disponibles"
        },
        "paging": "full_numbers",
        "info": false,
        "searching": false,
        "scrollY": "200",
        "scrollX": "true",
        "scrollCollapse": true,

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
                "className": 'Nombre_cliente'
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
            }
        ]
    });
}

$("#files").on('change', function() {
    var filenum = $('#files').get(0).files.length;
    if (filenum > 0) {
        var file = document.getElementById("files").files[0];
        var filename = file.name;
        if (evaluarExt(filename)) {
            mostrarAlerta("info", "B", 'Archivo: ' + filename);
            loadExcelDis(file);
            onclick = checkoff();
            sessionStorage.setItem("num", filenum);
            $('#files').val("");
        } else {
            document.getElementById("files").value = "";
            mostrarAlerta("info", "E", 'Tipo de archivo incorrecto: ' + filename );
            sessionStorage.setItem("num", filenum);
            $('#files').val("");
        }
    } else {
        mostrarAlerta("info", "E", 'Seleccione un archivo');
        var table = $('#table').DataTable();
        table.clear().draw();
    }
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
    esFile = true;
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
            if (data === "NO VALIDO"){
                M.toast({
                    html: "Archivo con numero de columnas incorrecto"
                });
                document.getElementById("loader").style.display = "none";
            } else if ((data !== null || data !== "") && !data.isRedirect) {
               // $('#table tbody').html(data);
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
                    var contac = dataj.CONTACTO;
                    var eco = dataj.CONTACTOE;
                    var mes = dataj.MESS;

                    if (mes === null)
                        mes = "";

                    //identificacion de error
                    if (coc.indexOf('?') !== -1) {
                        coc = coc.slice(0, -1);
                        var cocx = true;
                    }
                    if (pai.indexOf('?') !== -1) {
                        pai = pai.slice(0, -1);
                        var paix = true;
                    }
                    if (cli.indexOf('?') !== -1) {
                        cli = cli.slice(0, -1);
                        var clix = true;
                    }
                    if (noc.indexOf('?') !== -1) {
                        noc = noc.slice(0, -1);
                        var nocx = true;
                    }
                    if (ni0.indexOf('?') !== -1) {
                        ni0 = ni0.slice(0, -1);
                        var ni0x = true;
                    }
                    if (ni1.indexOf('?') !== -1) {
                        ni1 = ni1.slice(0, -1);
                        var ni1x = true;
                    }
                    if (ni2.indexOf('?') !== -1) {
                        ni2 = ni2.slice(0, -1);
                        var ni2x = true;
                    }
                    if (ni3.indexOf('?') !== -1) {
                        ni3 = ni3.slice(0, -1);
                        var ni3x = true;
                    }
                    if (ni4.indexOf('?') !== -1) {
                        ni4 = ni4.slice(0, -1);
                        var ni4x = true;
                    }
                    if (ni5.indexOf('?') !== -1) {
                        ni5 = ni5.slice(0, -1);
                        var ni5x = true;
                    }
                    if (ni6.indexOf('?') !== -1) {
                        ni6 = ni6.slice(0, -1);
                        var ni6x = true;
                    }
                    if (ni7.indexOf('?') !== -1) {
                        ni7 = ni7.slice(0, -1);
                        var ni7x = true;
                    }
                    if (ven.indexOf('?') !== -1) {
                        ven = ven.slice(0, -1);
                        var venx = true;
                    }
                    if (can.indexOf('?') !== -1) {
                        can = can.slice(0, -1);
                        var canx = true;
                    }
                    if (eco.indexOf('?') !== -1) {
                        eco = eco.slice(0, -1);
                        var ecox = true;
                    }//Termina identificacion

                    //Quitar Ceros
                    var cam = cli.split("");
                    var con = 0;
                    for (var i = 0; i < 10; i++) {
                        if (cam[i] === 0) {
                            con++;
                        }
                        else {
                            i = 9;
                        }
                    }
                    cli = cli.substring(con, i);

                    cam = ban.split("");
                    con = 0;
                    for (var i = 0; i < 10; i++) {
                        if (cam[i] == 0) {
                            con++;
                        }
                        else {
                            i = 9;
                        }
                    }
                    ban = ban.substring(con, i);

                    cam = baa.split("");
                    con = 0;
                    for (var i = 0; i < 10; i++) {
                        if (cam[i] === 0) {
                            con++;
                        }
                        else {
                            i = 9;
                        }
                    }
                    baa = baa.substring(con, i);

                    cam = ven.split("");
                    con = 0;
                    for (var i = 0; i < 10; i++) {
                        if (cam[i] === 0) {
                            con++;
                        }
                        else {
                            i = 9;
                        }
                    }
                    ven = ven.substring(con, i);
                    //Termina quitar ceros

                    var addedRow = addRow(table, dataj.POS, bor, coc, pai, cli, noc, ni0, ni1, ni2, ni3, ni4, ni5, ni6, ni7, ven, ban, baa, can, exp, contac, eco, mes);

                    //Pintar de rojo las celdas
                    var cols = addedRow.cells[1];
                    if (cocx === true) {
                        $(cols).addClass("red");
                    }
                    cols = addedRow.cells[2];
                    if (paix === true) {
                        $(cols).addClass("red");
                    }
                    cols = addedRow.cells[3];
                    if (clix === true) {
                        $(cols).addClass("red");
                    }
                    cols = addedRow.cells[4];
                    if (nocx === true) {
                        $(cols).addClass("red");
                    }
                    cols = addedRow.cells[5];
                    if (ni0x === true) {
                        $(cols).addClass("red");
                    }
                    cols = addedRow.cells[6];
                    if (ni1x === true) {
                        $(cols).addClass("red");
                    }
                    cols = addedRow.cells[7];
                    if (ni2x === true) {
                        $(cols).addClass("red");
                    }
                    cols = addedRow.cells[8];
                    if (ni3x === true) {
                        $(cols).addClass("red");
                    }
                    cols = addedRow.cells[9];
                    if (ni4x === true) {
                        $(cols).addClass("red");
                    }
                    cols = addedRow.cells[10];
                    if (ni5x === true) {
                        $(cols).addClass("red");
                    }
                    cols = addedRow.cells[11];
                    if (ni6x === true) {
                        $(cols).addClass("red");
                    }
                    cols = addedRow.cells[12];
                    if (ni7x === true) {
                        $(cols).addClass("red");
                    }
                    cols = addedRow.cells[13];
                    if (venx === true) {
                        $(cols).addClass("red");
                    }
                    cols = addedRow.cells[16];
                    if (canx === true) {
                        $(cols).addClass("red");
                    }
                    cols = addedRow.cells[19];
                    if (ecox === true) {
                        $(cols).addClass("red");
                    }
                });
                $('#table').css("font-size", "12px");
                $('#table').css("display", "table");
                document.getElementById("loader").style.display = "none";
            }
            else {
                window.location.href = data.redirectUrl;
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

function addRow(t, POS, br, cc, p, c, nc, n0, n1, n2, n3, n4, n5, n6, n7, v, b, ba, ca, ex, co, ec, me) {
    var r = addRowl(
        t,
        POS,
        "<label><input class='input_bor' type='checkbox' id='' name='bor' onclick='checkoff();' value='" + br + "'><span></span></label>",
        "<input class='input_coc' style='font-size:12px;' type='text' id='' name='coc' value='" + cc + "' onchange='Comprobar()'><span  style='display: none;'>" + cc + "</span>",
        "<input class='input_pai' style='font-size:12px;' type='text' id='' name='pai' value='" + p + "' onchange='Comprobar()'><span  style='display: none;'>" + p + "</span>",
        "<input class='input_cli' disabled='true' style='font-size:12px;' type='text' id='' name='cli' value='" + c + "' onchange='Comprobar()'><span  style='display: none;'>" + c + "</span>",
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
        "<input class='input_ban' disabled='true' style='font-size:12px;' type='text' id='' name='ban' value='" + b + "' onchange='Comprobar()'><span  style='display: none;'>" + b + "</span>",
        "<input class='input_baa' style='font-size:12px;' type='text' id='' name='baa' value='" + ba + "' onchange='Comprobar()'><span  style='display: none;'>" + ba + "</span>",
        "<input class='input_can' style='font-size:12px;' type='text' id='' name='can' value='" + ca + "' onchange='Comprobar()'><span  style='display: none;'>" + ca+ "</span>",
        "<input class='input_exp' style='font-size:12px;' type='text' id='' name='exp' value='" + ex + "' onchange='Comprobar()'><span  style='display: none;'>" + ex + "</span>",
        "<input class='input_con' style='font-size:12px;' type='text' id='' name='con' value='" + co + "' onchange='Comprobar()'><span  style='display: none;'>" + co + "</span>",
        "<input class='input_eco' type='email' style='font-size:12px;' type='text' id='' name='eco' value='" + ec + "' onchange='Comprobar()'><span  style='display: none;'>" + ec + "</span>",
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
    var rowNum = $('#table>tbody').find('.input_bor').length;
    if (rowNum === 0) {
        return;
    }
    var table = $('#table').DataTable();
    table.destroy();
    habilitar();
    var datos = $('#tabla').serializeArray();
    var message = $('.input_mes').serialize();
    var cliente = $('.input_cli').serialize();
    //console.log(datos);
    //console.log(message);
    var us1 = $('.input_ni1').serialize();
    var us6 = $('.input_ni6').serialize();
    var doc = sessionStorage.getItem("num");
    if (doc > 0 || cliente !== null || cliente !== "cli=") {
        if (message === "" || message === null || message.endsWith('mes=')) {
            if (us1 !== "" && us1 !== "ni1=" && us1 !== null && us6 !== "" && us6 !== "ni6=" && us6 !== null) {
                document.getElementById("loader").style.display = "flex";
                $.ajax({
                    type: "POST",
                    url: 'Agregar',
                    data: datos,
                    dataType: "json",
                    success: function (data) {
                        if (data.isRedirect) {
                            window.location.href = data.redirectUrl;
                        } else {
                            mostrarAlerta("info", "A", "Se agregaron los nuevos registros");
                            if (esFile) {
                                window.location = root + "Clientes";
                            } else {
                                Limpiar();
                                document.getElementById("loader").style.display = "none";
                            }
                        }
                    },
                    error: function (request, status, error) {
                        document.getElementById("loader").style.display = "none";
                        console.log(request.responseText);
                    },
                    async: true
                });
            }
            else
                mostrarAlerta("info", "E", "Los niveles 1 y 6 no pueden quedar vacios");
        }
        else
            mostrarAlerta("info", "E", "Hay errores por corregir");
    }
    else
        mostrarAlerta("info", "E", "Seleccione un archivo");
}

function Comprobar() {
    habilitar();
    var datos = $('#tabla').serializeArray();
    creart('Comprobar', datos);
    //mostrarAlerta("info", "A", "Vista Actualizada");
}

function Borrar() {
    var rowNum = $('#table>tbody').find('.input_bor').length;
    var check = false;
    $('#table>tbody').find('.input_bor').each(function (indx, checkInput) {
        if (checkInput.checked) {
            check = checkInput.checked;
        }
    });
    if (rowNum === 0 || !check) {
        return;
    }
    var table = $('#table').DataTable();
    var rows = $('.input_bor').serializeArray();
    for (var i = rows.length; i > 0; i--) {
        var num = rows[i-1].value;
        table.row(num).remove().draw();
    }
    rowNum = $('#table>tbody').find('.input_bor').length;
    if (rowNum >0) {
        Comprobar();
        $('#idBorrar').attr('disabled', 'disabled');
    } else {
        Iniciar();
    }
}

function Limpiar() {
    var table = $('#table').DataTable();
    table.clear().draw();
    Iniciar(); 
}

function Iniciar() {
    var htmlTr = '<tr>'
        + '<td></td>'
        + '<td></td>'
        + '<td></td>'
        + '<td><input class="input_cli" style="font-size:12px;" type="text" id="" name="cli" value="" onkeyup="if(event.keyCode == 13) Actualizar()"/></td>'
        + '<td></td>'
        + '<td></td>'
        + '<td></td>'
        + '<td></td>'
        + '<td></td>'
        + '<td></td>'
        + '<td></td>'
        + '<td></td>'
        + '<td></td>'
        + '<td></td>'
        + '<td></td>'
        + '<td></td>'
        + '<td></td>'
        + '<td></td>'
        + '<td></td>'
        + '<td></td>'
        + '<td></td>'
        + '</tr>';
    $('#table').DataTable().destroy();
    $('#table').append(htmlTr);
    formato();
    $('#idBorrar').attr('disabled', 'disabled');
    $("#borrar").prop('checked', false);

}

function Actualizar() {
    var datos = $('#tabla').serializeArray();
    if (datos.length > 0 && datos[1].value.length === 10) {
        esFile = false;
        habilitar();
        creart('Actualizar', datos);
    }
}

function creart(metodo, datos) {
    $.ajax({
        type: "POST",
        url: metodo,
        dataType: "json",
        data: datos,
        success: function (data) {
            if ((data !== null || data !== "") && !data.isRedirect) {
                
                var table = $('#table').DataTable();
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
                    var cont = dataj.CONTACTO;
                    var eco = dataj.CONTACTOE;
                    var mes = dataj.MESS;

                    if (mes === null)
                        mes = "";

                    //identificacion de error
                    if (coc.indexOf('?') !== -1) {
                        coc = coc.slice(0, -1);
                        var cocx = true;
                    }
                    if (pai.indexOf('?') !== -1) {
                        pai = pai.slice(0, -1);
                        var paix = true;
                    }
                    if (cli.indexOf('?') !== -1) {
                        cli = cli.slice(0, -1);
                        var clix = true;
                    }
                    if (noc.indexOf('?') !== -1) {
                        noc = noc.slice(0, -1);
                        var nocx = true;
                    }
                    if (ni0.indexOf('?') !== -1) {
                        ni0 = ni0.slice(0, -1);
                        var ni0x = true;
                    }
                    if (ni1.indexOf('?') !== -1) {
                        ni1 = ni1.slice(0, -1);
                        var ni1x = true;
                    }
                    if (ni2.indexOf('?') !== -1) {
                        ni2 = ni2.slice(0, -1);
                        var ni2x = true;
                    }
                    if (ni3.indexOf('?') !== -1) {
                        ni3 = ni3.slice(0, -1);
                        var ni3x = true;
                    }
                    if (ni4.indexOf('?') !== -1) {
                        ni4 = ni4.slice(0, -1);
                        var ni4x = true;
                    }
                    if (ni5.indexOf('?') !== -1) {
                        ni5 = ni5.slice(0, -1);
                        var ni5x = true;
                    }
                    if (ni6.indexOf('?') !== -1) {
                        ni6 = ni6.slice(0, -1);
                        var ni6x = true;
                    }
                    if (ni7.indexOf('?') !== -1) {
                        ni7 = ni7.slice(0, -1);
                        var ni7x = true;
                    }
                    if (ven.indexOf('?') !== -1) {
                        ven = ven.slice(0, -1);
                        var venx = true;
                    }
                    if (can.indexOf('?') !== -1) {
                        can = can.slice(0, -1);
                        var canx = true;
                    }
                    if (eco.indexOf('?') !== -1) {
                        eco = eco.slice(0, -1);
                        var ecox = true;
                    }//Termina identificacion

                    //Quitar Ceros
                    var cam = cli.split("");
                    var con = 0;
                    for (var i = 0; i < 10; i++) {
                        if (cam[i] == 0) {
                            con++;
                        }
                        else {
                            i = 9;
                        }
                    }
                    cli = cli.substring(con, i);

                    cam = ban.split("");
                    con = 0;
                    for (var i = 0; i < 10; i++) {
                        if (cam[i] == 0) {
                            con++;
                        }
                        else {
                            i = 9;
                        }
                    }
                    ban = ban.substring(con, i);

                    cam = baa.split("");
                    con = 0;
                    for (var i = 0; i < 10; i++) {
                        if (cam[i] == 0) {
                            con++;
                        }
                        else {
                            i = 9;
                        }
                    }
                    baa = baa.substring(con, i);

                    cam = ven.split("");
                    con = 0;
                    for (var i = 0; i < 10; i++) {
                        if (cam[i] == 0) {
                            con++;
                        }
                        else {
                            i = 9;
                        }
                    }
                    ven = ven.substring(con, i);
                    //Termina quitar ceros

                    var addedRow = addRow(table, dataj.POS, bor, coc, pai, cli, noc, ni0, ni1, ni2, ni3, ni4, ni5, ni6, ni7, ven, ban, baa, can, exp, cont, eco, mes);

                    //Pintar de rojo las celdas
                    var cols = addedRow.cells[1];
                    if (cocx === true) {
                        $(cols).addClass("red");
                    }
                    cols = addedRow.cells[2];
                    if (paix === true) {
                        $(cols).addClass("red");
                    }
                    cols = addedRow.cells[3];
                    if (clix === true) {
                        $(cols).addClass("red");
                    }
                    cols = addedRow.cells[4];
                    if (nocx === true) {
                        $(cols).addClass("red");
                    }
                    cols = addedRow.cells[5];
                    if (ni0x === true) {
                        $(cols).addClass("red");
                    }
                    cols = addedRow.cells[6];
                    if (ni1x === true) {
                        $(cols).addClass("red");
                    }
                    cols = addedRow.cells[7];
                    if (ni2x === true) {
                        $(cols).addClass("red");
                    }
                    cols = addedRow.cells[8];
                    if (ni3x === true) {
                        $(cols).addClass("red");
                    }
                    cols = addedRow.cells[9];
                    if (ni4x === true) {
                        $(cols).addClass("red");
                    }
                    cols = addedRow.cells[10];
                    if (ni5x === true) {
                        $(cols).addClass("red");
                    }
                    cols = addedRow.cells[11];
                    if (ni6x === true) {
                        $(cols).addClass("red");
                    }
                    cols = addedRow.cells[12];
                    if (ni7x === true) {
                        $(cols).addClass("red");
                    }
                    cols = addedRow.cells[13];
                    if (venx === true) {
                        $(cols).addClass("red");
                    }
                    cols = addedRow.cells[16];
                    if (canx === true) {
                        $(cols).addClass("red");
                    }
                    cols = addedRow.cells[19];
                    if (ecox === true) {
                        $(cols).addClass("red");
                    }
                });
                $('#table').css("font-size", "12px");
                $('#table').css("display", "table");
                document.getElementById("loader").style.display = "none";
            }
            else {
                window.location.href = data.redirectUrl;
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
    if ($("#borrar").prop('checked') && $('#table>tbody').find('.input_bor').length>0) {
        $('#idBorrar').removeAttr('disabled');
        $(".input_bor").prop('checked', true);
    } else {
        $(".input_bor").prop('checked', false);
        $('#idBorrar').attr('disabled', 'disabled');
    }

}

function checkoff() {
    var check = false;
    $('#table>tbody').find('.input_bor').each(function (indx, checkInput) {
        if (checkInput.checked) {
            check = checkInput.checked;
        }
    });
    if (!check) {
        $("#borrar").prop('checked', false);
        $('#idBorrar').attr('disabled', 'disabled');
    } else {
        $('#idBorrar').removeAttr('disabled');
    }
}

function mostrarAlerta(warning_id, tipo, mensaje) {
    var dura = 1000000,
        color = 'yellow',
        icon = 'info',
        classe = 'toast';
    if (tipo === "E") {
        color = 'red';
        icon = 'error';
    }
    else if (tipo === "B") {
        color = 'green';
        icon = 'done';
    }
    dismiss(classe);
    M.toast({
        classes: classe,
        displayLength: dura,
        html: '<span style="padding-right:15px;"><i class="material-icons ' + color + '-text">' + icon + '</i></span>  ' + mensaje
            + '<button class="btn-small btn-flat toast-action" onclick="dismiss(\'toast\')">Aceptar</button>'
    });
}

function dismiss(classe) {
    var toastElement = document.querySelectorAll('.' + classe);
    for (var i = 0; i < toastElement.length; i++) {
        var toastInstance = M.Toast.getInstance(toastElement[i]);
        toastInstance.dismiss();
    }
}

function habilitar() {
    $(".input_cli").prop('disabled', false);
    $(".input_noc").prop('disabled', false);
    $(".input_ban").prop('disabled', false);
    habi = true;
}
$('body').on('keydown.autocomplete', '.input_pai', function () {
    
    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "GET",
                url: root + 'Listas/Paises',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.LAND + " | " + item.LANDX, value: item.LAND };
                    }));
                }
            });
        },

        messages: {
            noResults: '',
            results: function (resultsCount) { }
        },

        change: function (e, ui) {
            if (!(ui.item) && $(".input_pai").val() === "") {
                e.target.value = "";
            }
        },

        select: function (event, ui) {
            //var label = ui.item.label;
            //var value = ui.item.value;
        }
    });
});
$('body').on('keydown.autocomplete', '.input_ven', function () {

    //var tr = $(this).closest('tr'); //Obtener el row
    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "GET",
                url: root+'Listas/Vendors',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.ID + " | " + item.NOMBRE, value: item.ID };
                    }));
                }
            });
        },

        messages: {
            noResults: '',
            results: function (resultsCount) { }
        },

        change: function (e, ui) {
            if (!(ui.item) && $(".input_ven").val() === "") {
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
                type: "GET",
                url: root+'Listas/Canales',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.CANAL1 + " | " + item.CDESCRIPCION, value: item.CANAL1 };
                    }));
                }
            });
        },

        messages: {
            noResults: '',
            results: function (resultsCount) { }
        },

        change: function (e, ui) {
            if (!(ui.item) && $(".input_can").val() === "") {
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
                type: "GET",
                url: root + 'Listas/Clientes',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.KUNNR + " | " + item.NAME1, value: item.KUNNR };
                    }));
                }
            });
        },

        messages: {
            noResults: '',
            results: function (resultsCount) { }
        },

        change: function (e, ui) {
            if (!(ui.item) && $(".input_cli").val()==="") {
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
                type: "GET",
                url: root+'Listas/Sociedades',
                dataType: "json",
                data: { Prefix: request.term },
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
            if (!(ui.item) && $(".input_coc").val()==="") {
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
                type: "GET",
                url: root+'Listas/Usuarios',
                dataType: "json",
                data: { "Prefix": request.term, autorizador:2 },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.ID + " | " + item.NOMBRE + " " + item.APELLIDO_P, value: item.ID };
                    }));
                }
            });
        },

        messages: {
            noResults: '',
            results: function (resultsCount) { }
        },

        change: function (e, ui) {
            if (!(ui.item) && $(".input_ni0").val() === "") {
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
                type: "GET",
                url: root + 'Listas/Usuarios',
                dataType: "json",
                data: { "Prefix": request.term, autorizador: 2 },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.ID + " | " + item.NOMBRE + " " + item.APELLIDO_P, value: item.ID };
                    }));
                }
            });
        },

        messages: {
            noResults: '',
            results: function (resultsCount) { }
        },

        change: function (e, ui) {
            if (!(ui.item) && $(".input_ni1").val()==="") {
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
                type: "GET",
                url: root + 'Listas/Usuarios',
                dataType: "json",
                data: { "Prefix": request.term, autorizador: 2 },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.ID + " | " + item.NOMBRE + " " + item.APELLIDO_P, value: item.ID };
                    }));
                }
            });
        },

        messages: {
            noResults: '',
            results: function (resultsCount) { }
        },

        change: function (e, ui) {
            if (!(ui.item) && $(".input_ni2").val() === "") {
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
                type: "GET",
                url: root + 'Listas/Usuarios',
                dataType: "json",
                data: { "Prefix": request.term, autorizador: 2 },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.ID + " | " + item.NOMBRE + " " + item.APELLIDO_P, value: item.ID };
                    }));
                }
            });
        },

        messages: {
            noResults: '',
            results: function (resultsCount) { }
        },

        change: function (e, ui) {
            if (!(ui.item) && $(".input_ni3").val() === "") {
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
                type: "GET",
                url: root + 'Listas/Usuarios',
                dataType: "json",
                data: { "Prefix": request.term, autorizador: 2 },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.ID + " | " + item.NOMBRE + " " + item.APELLIDO_P, value: item.ID };
                    }));
                }
            });
        },

        messages: {
            noResults: '',
            results: function (resultsCount) { }
        },

        change: function (e, ui) {
            if (!(ui.item) && $(".input_ni4").val() === "") {
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
                type: "GET",
                url: root + 'Listas/Usuarios',
                dataType: "json",
                data: { "Prefix": request.term, autorizador: 2 },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.ID + " | " + item.NOMBRE + " " + item.APELLIDO_P, value: item.ID };
                    }));
                }
            });
        },

        messages: {
            noResults: '',
            results: function (resultsCount) { }
        },

        change: function (e, ui) {
            if (!(ui.item) && $(".input_ni5").val() === "") {
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
                url: 'Usuario8',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.ID + " | " + item.NOMBRE + " " + item.APELLIDO_P, value: item.ID };
                    }));
                }
            });
        },

        messages: {
            noResults: '',
            results: function (resultsCount) { }
        },

        change: function (e, ui) {
            if (!(ui.item) && $(".input_ni6").val() === "") {
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
                url: 'Usuario9',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.ID + " | " + item.NOMBRE + " " + item.APELLIDO_P, value: item.ID };
                    }));
                }
            });
        },

        messages: {
            noResults: '',
            results: function (resultsCount) { }
        },

        change: function (e, ui) {
            if (!(ui.item) && $(".input_ni7").val() === "") {
                e.target.value = "";
            }
        },

        select: function (event, ui) {
        }
    });
});


