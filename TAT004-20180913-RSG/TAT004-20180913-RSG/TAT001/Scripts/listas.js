﻿function periodo() {
    var soc = $("#sociedad_id").val();
    var tsol = (!$("#TSOL_ID").val() ? $("#tsol_id").val() : $("#TSOL_ID").val());
    var user = $("#USUARIOD_ID").val();
    $.ajax({
        type: "POST",
        url: root + 'Listas/periodo',
        dataType: "text",
        data: { sociedad_id: soc, tsol_id: tsol, usuario_id: user },

        success: function (data) {
            if (data === 0) {
                M.toast({
                    classes: "pWarnning",
                    displayLength: 1000000,
                    html: '<span style="padding-right:15px;"><i class="material-icons yellow-text">info</i></span> No hay periodo abierto '
                    + '<button class="btn-small btn-flat toast-action" onclick="dismiss(\'pWarnning\')">Aceptar</button>'
                });
            } else {
                $("#PERIODO").val(data);
                $("#periodo").val(data);
            }
        },
        error: function (xhr, httpStatusMessage, customErrorMessage) {
            M.toast({ html: httpStatusMessage });
        },
        async: false
    });
}
function cuentas(reverso) {
    var bool = false;
    clearing = [];
    var bukrs = ($("#sociedad_id").val() ? $("#sociedad_id").val():$("#D_SOCIEDAD_ID").val()),
        land = $("#pais_id").val(),
        gall = $("#tall_id").val(),
        ejercicio = ($("#ejercicio").val() ? $("#ejercicio").val():$("#D_EJERCICIO").val());
    $.ajax({
        type: "POST",
        url: root+'Listas/clearing',
        dataType: "json",
        data: { bukrs: bukrs, land: land, gall: gall, ejercicio: ejercicio },

        success: function (data) {
            if (data !== null && data !== "") {
                if (reverso) {
                    clearing.push(data.CARGO);
                    //DRS 28.09.2018
                    clearing.push(data.NOMBREC);
                    clearing.push(data.ABONO);
                    clearing.push(data.NOMBREA);
                } else {
                    //RSG 26.04.2018----------------
                    clearing.push(data.ABONO);
                    //DRS 28.09.2018
                    clearing.push(data.NOMBREA);
                    clearing.push(data.CARGO);
                    clearing.push(data.NOMBREC);
                }
                clearing.push(data.CLEARING);
                clearing.push(data.LIMITE);
                //RSG 26.04.2018----------------
                bool = true;
            } else {
                M.toast({
                    classes: "cWarnning",
                    displayLength: 1000000,
                    html: '<span style="padding-right:15px;"><i class="material-icons yellow-text">info</i></span> Sin Cuentas relacionadas '
                    + '<button class="btn-small btn-flat toast-action" onclick="dismiss(\'cWarnning\')">Aceptar</button>'
                });
                bool = false;
            }

        },
        error: function (xhr, httpStatusMessage, customErrorMessage) {
            M.toast({ html: httpStatusMessage });
        },
        async: false
    });
    if (!bool) {
        $("#btn_guardarBorr").addClass("disabled");
        $("#btn_guardarh").addClass("disabled");
    } else {
        $("#btn_guardarBorr").removeClass("disabled");
    }
}
function asignarPresupuesto(kunnr) {
    var periodo= $("#periodo").val();
    $.ajax({
        type: "POST",
        url: root+'Listas/getPresupuesto',
        dataType: "json",
        data: {
            kunnr: kunnr, periodo: periodo
        },
        success: function (data) {

            if (data !== null || data !== "") {
               
                //LEJ 09.07.18------------------------------------------
                var pcan = (data.P_CANAL / 1).toFixed(2);
                var pban = (data.P_BANNER / 1).toFixed(2);
                var pcc = (data.PC_C / 1).toFixed(2) * -1;
                var pca = (data.PC_A / 1).toFixed(2) * -1;
                var pcp = (data.PC_P / 1).toFixed(2) * -1;
                var pct = (data.PC_T / 1).toFixed(2) * -1;
                var consu = (data.CONSU / 1).toFixed(2);
                var _xdec = $("#dec").val();
                var _xm = $("#miles").val();
            
                $('#p_canal').text(toShowG(pcan.toString()));
                $('#p_banner').text(toShowG(pban.toString()));
                $('#pc_c').text(toShowG(pcc.toString()));
                $('#pc_a').text(toShowG(pca.toString()));
                $('#pc_p').text(toShowG(pcp.toString()));
                $('#pc_t').text(toShowG(pct.toString()));

                $('#consu').text(toShowG(consu.toString()));
            }
            //LEJ 09.07.18-----------------------------------------------
        },
        error: function (xhr, httpStatusMessage, customErrorMessage) {
            M.toast({ html: httpStatusMessage });
        },
        async: false
    });

}
function cierre() {
    //string sociedad_id, string tsol_id, int periodo_id, string usuario_id = null
    var soc = $("#sociedad_id").val();
    var tsol = (!$("#TSOL_ID").val() ? $("#tsol_id").val() : $("#TSOL_ID").val());
    var periodo = $("#periodo").val();
    var user = $("#USUARIOD_ID").val();
    var bool = false;
    $.ajax({
        type: "POST",
        url: root + 'Listas/cierre',
        dataType: "text",
        data: { sociedad_id: soc, tsol_id: tsol, periodo_id: periodo, usuario_id: user },

        success: function (data) {
            if (data !== "X") {
                M.toast({
                    classes: "guardarWarnning",
                    displayLength: 1000000,
                    html: '<span style="padding-right:15px;"><i class="material-icons yellow-text">info</i></span> Periodo ' + periodo + ' cerrado '
                    + '<button class="btn-small btn-flat toast-action" onclick="dismiss(\'guardarWarnning\')">Aceptar</button>'
                });
                bool = true;
            } else {
                bool = false;
            }
        },
        error: function (xhr, httpStatusMessage, customErrorMessage) {
            M.toast({ html: httpStatusMessage });
        },
        async: false
    });
    return bool;
}
function asignarSolicitud(num, num2,edit) {
    num = toNum(num);
    num2 = toNum(num2);
    var soc  = ($("#sociedad_id").val() ? $("#sociedad_id").val() : $("#D_SOCIEDAD_ID").val());
    var tsol = (!$("#TSOL_ID").val() ? $("#tsol_id").val() : $("#TSOL_ID").val());
    var esCategoriaUnica = false;
    if ($("#select_dis").val()==="C"){
        $("#table_dis > tbody  > tr[role='row']").each(function (indx, row) {
            var cat = row.cells.item(5).textContent;
            var _xxx = $.parseJSON($('#catmat').val());
            for (var i = 0; i < _xxx.length; i++) {
                if (cat === _xxx[i].DESCRIPCION && _xxx[i].UNICA === true) {
                        esCategoriaUnica = true;
                }
            }
        });
    }
        $.ajax({
            type: "POST",
            url: 'getSolicitud',
            dataType: "json",
            data: { num: num, monto: num2, tsol_id: tsol, sociedad_id: soc, esCategoriaUnica: esCategoriaUnica, edit: edit },

            success: function (data) {

                if (data !== null || data !== "") {
                    if (data.S_NUM === "") {
                        var num2 = $('#monto_doc_md').val();
                        $('#s_montob').text(num2);
                        $('#s_montop').text("-");
                        $('#s_montoa').text("-");
                        $('#s_rema').text("-");
                        if (data.S_IMPA !== null && data.S_IMPA !== "-"){
                            $('#s_impa').text(toShow(data.S_IMPA));
                        } else {
                            $('#s_impa').text("-");
                        }
                        $('#s_impb').text("-");
                        $('#s_impc').text("-");
                        $('#s_ret').text("-");
                        $('#s_total').text(num2);
                    } else if (edit) {
                        $('#s_montob').text(data.S_MONTOB);
                        $('#s_montop').text(data.S_MONTOP);
                        $('#s_montoa').text(data.S_MONTOA);
                        if (data.S_REMA.indexOf("(")>-1){
                            document.getElementById("a4").classList.add("red");
                            document.getElementById("a4").classList.add("white-text");
                        }
                        $('#s_rema').text(data.S_REMA);
                        if (data.S_IMPA !== null && data.S_IMPA !== "-") {
                            $('#s_impa').text(toShow(data.S_IMPA));
                        } else {
                            $('#s_impa').text("-");
                        }
                        $('#s_impb').text("-");
                        $('#s_impc').text("-");
                        $('#s_ret').text("-");
                        $('#s_total').text(data.S_TOTAL);
                    }
                    else {
                        $('#s_montob').text(toShow(data.S_MONTOB));
                        $('#s_montop').text(toShow(data.S_MONTOP));
                        if (data.S_MONTOA !== "-")
                            $('#s_montoa').text(toShow(data.S_MONTOA));
                        if (data.S_REMA !== "-") {
                            if (data.S_REMA < 0) {
                                $('#s_rema').text(toShow(data.S_REMA));
                                document.getElementById("a4").classList.add("red");
                                document.getElementById("a4").classList.add("white-text");
                            }
                            else {
                                $('#s_rema').text(toShow(data.S_REMA));
                                document.getElementById("a4").classList.remove("red");
                                document.getElementById("a4").classList.remove("white-text");
                            }
                        }
                        if (data.S_IMPA !== null && data.S_IMPA !== "-") {
                            $('#s_impa').text(toShow(data.S_IMPA));
                        } else {
                            $('#s_impa').text("-");
                        }
                        $('#s_impb').text(data.S_IMPB);
                        $('#s_impc').text(data.S_IMPC);
                        $('#s_ret').text(toShow(data.S_RET));
                        $('#s_total').text(toShow(data.S_TOTAL));
                    }
                }
            },
            error: function (xhr, httpStatusMessage, customErrorMessage) {
                M.toast({ html: httpStatusMessage });
            },
            async: false
        });
    
}
