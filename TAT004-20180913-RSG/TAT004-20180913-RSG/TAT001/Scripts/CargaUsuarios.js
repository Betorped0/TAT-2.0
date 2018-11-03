$(document).ready(function () {

    $('#table').DataTable({
        "scrollY": "200",
        "scrollX": "true",
        "scrollCollapse": true,
        "order": false,
        "language": {
            //"url": "../Scripts/lang/@Session['spras'].ToString()" + ".json"
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
                "name": 'KUNNR',
                "className": 'KUNNR'
            },
            {
                "name": 'BUNIT',
                "className": 'BUNIT'
            },
            {
                "name": 'PUESTO_ID',
                "className": 'PUESTO_ID'
            },
            {
                "name": 'ID',
                "className": 'ID',
            },
            {
                "name": 'NOMBRE',
                "className": 'NOMBRE'
            },
            {
                "name": 'APELLIDO_P',
                "className": 'APELLIDO_P'
            },
            {
                "name": 'APELLIDO_M',
                "className": 'APELLIDO_M'
            },
            {
                "name": 'EMAIL',
                "className": 'EMAIL'
            },
            {
                "name": 'SPRAS_ID',
                "className": 'SPRAS_ID'
            },
            {
                "name": 'PASS',
                "className": 'PASS'
            },
            {
                "name": 'mess',
                "className": 'mess'
            }
        ]
    });
});

function subeExcel() {
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
            mostrarAlerta("info", "E", 'Tipo de archivo incorrecto: ' + filename)
            //M.toast({ html: 'Tipo de archivo incorrecto: ' + filename });
        }
    } else {
        mostrarAlerta("info", "A", 'Seleccione un archivo')
        //M.toast({ html: 'Seleccione un archivo' });
        var table = $('#table').DataTable();
        table.clear().draw();
    }
    sessionStorage.setItem("num", filenum);
}

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

                    var cli = dataj.KUNNR;
                    var com = dataj.BUNIT;
                    var niv = dataj.PUESTO_ID;
                    var usc = dataj.ID;
                    var spr = dataj.SPRAS_ID;
                    var ema = dataj.EMAIL;
                    var bor = i;
                    var mes = dataj.mess;

                    if (mes == null)
                        mes = "";

                    //identificacion de error
                    if (cli.indexOf('?') != -1) {
                        cli = cli.slice(0, -1);
                        var clix = true;
                    }
                    if (com.indexOf('?') != -1) {
                        com = com.slice(0, -1);
                        var comx = true;
                    }
                    if (niv.indexOf('?') != -1) {
                        niv = niv.slice(0, -1);
                        var nivx = true;
                    }
                    if (usc.indexOf('?') != -1) {
                        usc = usc.slice(0, -1);
                        var uscx = true;
                    }
                    if (usc.indexOf('!') != -1) {
                        usc = usc.slice(0, -1);
                        var uscy = true;
                    }
                    if (spr.indexOf('?') != -1) {
                        spr = spr.slice(0, -1);
                        var sprx = true;
                    }
                    if (ema.indexOf('?') != -1) {
                        ema = ema.slice(0, -1);
                        var emax = true;
                    }//Termina identificacion

                    var addedRow = addRow(table, dataj.POS, bor, cli, com, niv, usc, dataj.NOMBRE, dataj.APELLIDO_P, dataj.APELLIDO_M, ema, spr, dataj.PASS, mes);

                    var cols = addedRow.cells[1];
                    if (clix == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[2];
                    if (comx == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[3];
                    if (nivx == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[4];
                    if (uscx == true) {
                        $(cols).addClass("red");
                    }
                    if (uscy == true) {
                        $(cols).addClass("yellow");
                    }
                    var cols = addedRow.cells[8];
                    if (emax == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[9];
                    if (sprx == true) {
                        $(cols).addClass("red");
                    }
                });
                $('#table_dis').css("font-size", "12px");
                $('#table_dis').css("display", "table");
                $('#tfoot_dis').css("display", "table-footer-group");
                document.getElementById("loader").style.display = "none";
            }
        },
        complete: function () {

            var num = $("#table tr").length - 1;
            addRow(table, num, num, "", "", "", "", "", "", "", "", "", "", "");
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

function addRow(t, POS, br, k, b, pi, id, n, ap, am, e, si, sp, me) {
    var ppr = "";
    var r = addRowl(
        t,
        POS,
        "<label><input class='input_bor' type='checkbox' id='' name='bor' onclick='checkoff();' value='" + br + "'><span></span></label>",
        "<input class='input_cli' style='font-size:12px;' type='text' id='' name='cli' value='" + k + "' onchange='Comprobar()' onkeyup='if(event.keyCode == 13) foco()'>",
        "<input class='input_com' style='font-size:12px;' type='text' id='' name='com' value='" + b + "' onchange='Comprobar()' onkeyup='if(event.keyCode == 13) foco()'>",
        "<input class='input_niv' style='font-size:12px;' type='text' id='' name='niv' value='" + pi + "' onchange='Comprobar()' onkeyup='if(event.keyCode == 13) foco()'>",
        "<input class='input_usc' style='font-size:12px;' type='text' id='' name='usc' value='" + id + "' onchange='Comprobar()' onkeyup='if(event.keyCode == 13) foco()'>",
        "<input class='input_nom' style='font-size:12px;' type='text' id='' name='nom' value='" + n + "' onchange='Comprobar()' onkeyup='if(event.keyCode == 13) foco()'>",
        "<input class='input_app' style='font-size:12px;' type='text' id='' name='app' value='" + ap + "' onchange='Comprobar()' onkeyup='if(event.keyCode == 13) foco()'>",
        "<input class='input_apm' style='font-size:12px;' type='text' id='' name='apm' value='" + am + "' onchange='Comprobar()' onkeyup='if(event.keyCode == 13) foco()'>",
        "<input class='input_ema' style='font-size:12px;' type='text' id='' name='ema' value='" + e + "' onchange='Comprobar()' onkeyup='if(event.keyCode == 13) foco()'>",
        "<input class='input_idi' style='font-size:12px;' type='text' id='' name='idi' value='" + si + "' onchange='Comprobar()' onkeyup='if(event.keyCode == 13) foco()'>",
        "<input class='input_pas' style='font-size:12px;' type='text' id=
        "<input class='input_mes' type='hidden' name='mes' value='" + me + "'><span class='input_mes' style='font-size:12px;'>" + me + "</span>"
    );
    return r;
}

function addRowl(t, pos, br, k, b, pi, id, n, ap, am, e, si, sp, me) {
    var r = t.row.add([
        br,
        k,
        b,
        pi,
        id,
        n,
        ap,
        am,
        e,
        si,
        sp,
        me
    ]).draw(false).node();
    return r;
}

function Carga() {
    var message = $('.input_mes').serialize();
    var doc = sessionStorage.getItem("num");
    if (doc > 0) {
        if (message.indexOf('1.') < 0) {
            $.ajax({
                type: "POST",
                url: 'Agregar',
                data: null,
                dataType: "json",
                success: function () {

                },
                error: function (request, status, error) {
                    //alert(request.responseText);
                }
            });
            mostrarAlerta("info", "A", "Se agregaron los nuevos usuarios");
            //M.toast({ html: 'Se agregaron los nuevos usuarios' });
            window.location = root + "Usuarios/Index";
        }
        else
            mostrarAlerta("info", "E", "Hay errores por corregir");
        //M.toast({ html: 'Hay errores por corregir' });
    }
    else
        mostrarAlerta("info", "E", "Seleccione un archivo");
        //M.toast({ html: 'Seleccione un archivo' });

}

function Comprobar() {
    var datos = $('#tabla').serializeArray();
    creart('Comprobar', datos); 
    mostrarAlerta("info", "A", "Registro Actualizado");
}

function Borrar() {
    var table = $('#table').DataTable();
    var rows = $('.input_bor').serializeArray();
    for (var i = rows.length; i > 0; i--) {
        var num = rows[i - 1].value;
        table.row(num).remove().draw();
    }
    Comprobar();
}

function Actualizar() {
    var message = $('.input_mes').serialize();
    if (message.indexOf('existe') > -1) {
        $.ajax({
            type: "POST",
            url: 'Actualizar',
            data: null,
            dataType: "json",
            success: function () {

            },
            error: function (request, status, error) {
                //alert(request.responseText);
            }
        });
        mostrarAlerta("info", "A", "Se actualizaron los usuarios")
        //M.toast({ html: 'Se actualizaron los usuarios' });
        window.location = root + "Usuarios/Index";
    }
    else
        mostrarAlerta("info", "E", "No hay usuarios por actualizar")
        //M.toast({ html: 'No hay usuarios por actualizar' });
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

                    var cli = dataj.KUNNR;
                    var com = dataj.BUNIT;
                    var niv = dataj.PUESTO_ID;
                    var usc = dataj.ID;
                    var spr = dataj.SPRAS_ID;
                    var ema = dataj.EMAIL;
                    var bor = i;
                    var clix = false;
                    var mes = dataj.mess;

                    if (mes == null)
                        mes = "";

                    //identificacion de error
                    if (cli.indexOf('?') != -1) {
                        cli = cli.slice(0, -1);
                        var clix = true;
                    }
                    if (com.indexOf('?') != -1) {
                        com = com.slice(0, -1);
                        var comx = true;
                    }
                    if (niv.indexOf('?') != -1) {
                        niv = niv.slice(0, -1);
                        var nivx = true;
                    }
                    if (usc.indexOf('?') != -1) {
                        usc = usc.slice(0, -1);
                        var uscx = true;
                    }
                    if (usc.indexOf('!') != -1) {
                        usc = usc.slice(0, -1);
                        var uscy = true;
                    }
                    if (spr.indexOf('?') != -1) {
                        spr = spr.slice(0, -1);
                        var sprx = true;
                    }
                    if (ema.indexOf('?') != -1) {
                        ema = ema.slice(0, -1);
                        var emax = true;
                    }//Termina identificacion

                    var addedRow = addRow(table, dataj.POS, bor, cli, com, niv, usc, dataj.NOMBRE, dataj.APELLIDO_P, dataj.APELLIDO_M, ema, spr, dataj.PASS, mes);

                    var cols = addedRow.cells[1];
                    if (clix == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[2];
                    if (comx == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[3];
                    if (nivx == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[4];
                    if (uscx == true) {
                        $(cols).addClass("red");
                    }
                    if (uscy == true) {
                        $(cols).addClass("yellow");
                    }
                    var cols = addedRow.cells[8];
                    if (emax == true) {
                        $(cols).addClass("red");
                    }
                    var cols = addedRow.cells[9];
                    if (sprx == true) {
                        $(cols).addClass("red");
                    }
                });
                $('#table_dis').css("font-size", "12px");
                $('#table_dis').css("display", "table");
                $('#tfoot_dis').css("display", "table-footer-group");
                document.getElementById("loader").style.display = "none";
            }
        },
        complete: function () {

            var num = $("#table tr").length - 1;
            addRow(table, num, num, "", "", "", "", "", "", "", "", "", "", "");
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

function Agregar() {
    var datos = $('#tabla').serializeArray();
    var table = $('#table').DataTable();
    creart('AgregarT', datos);
}

function foco() {
    $("input").blur();
}

function mostrarAlerta(warning_id, tipo, mensaje) {
    var dura = 1000000,
        color = 'yellow',
        icon = 'info',
        classe = 'toast';
    if (tipo == "E") {
        color = 'red';
        icon = 'error';
    }
    dismiss(classe)
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

$('body').on('keydown.autocomplete', '.input_cli1', function () {

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

$('body').on('keydown.autocomplete', '.input_cli', function () {
    var table = $("#table").DataTable();
    var tr = $(this).closest('tr'); //Obtener el row
    var row_index = $(this).parent().parent().index();
    var col_index = $(this).parent().index();
    var col_index2 = col_index + 1;

    for (var d = (table.rows().data().length) - 1; d > -1; d--) {
        var row = table.row(d).node();
        var bukrs = $(row).children().eq(col_index2).children().val();
        if (bukrs != "") {
            break;
        }

    }

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'Cliente1',
                dataType: "json",
                data: { "Prefix": request.term, "BUKRS": bukrs },
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

$('body').on('keydown.autocomplete', '.input_usc', function () {

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

$('body').on('keydown.autocomplete', '.input_idi', function () {

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'Idioma',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.ID + " | " + item.DESCRIPCION, value: item.ID };
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

$('body').on('keydown.autocomplete', '.input_com', function () {

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'Sociedad',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.BUKRS + " | " + item.BUTXT, value: item.BUKRS };
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

$('body').on('keydown.autocomplete', '.input_niv', function () {

    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'Nivel',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.ID + " | " + item.TEXTO, value: item.ID };
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
