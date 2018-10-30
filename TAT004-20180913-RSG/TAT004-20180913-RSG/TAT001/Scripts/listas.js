function periodo() {
    var soc = $("#sociedad_id").val();
    var tsol = (!$("#TSOL_ID").val() ? $("#tsol_id").val() : $("#TSOL_ID").val());
    var user = $("#USUARIOD_ID").val();
    $.ajax({
        type: "POST",
        url: root + 'Listas/periodo',
        dataType: "text",
        data: { sociedad_id: soc, tsol_id: tsol, usuario_id: user },

        success: function (data) {
            if (data ==0) {
                M.toast({
                    classes: "pWarnning",
                    displayLength: 1000000,
                    html: '<span style="padding-right:15px;"><i class="material-icons yellow-text">info</i></span> No hay periodo abierto '
                    + '<button class="btn-small btn-flat toast-action" onclick="dismiss(\'pWarnning\')">Aceptar</button>'
                });
            } else {
                $("#periodo").val(data);
            }
        },
        error: function (xhr, httpStatusMessage, customErrorMessage) {
            M.toast({ html: httpStatusMessage });
        },
        async: true
    });
}
function cuentas() {
    clearing = [];
    var bukrs = $("#sociedad_id").val(),
        land = $("#pais_id").val(),
        gall = $("#tall_id").val(),
        ejercicio = $("#ejercicio").val();
    $.ajax({
        type: "POST",
        url: root+'Listas/clearing',
        dataType: "json",
        data: { bukrs: bukrs, land: land, gall: gall, ejercicio: ejercicio },

        success: function (data) {
            if (data !== null && data !== "") {
                //RSG 26.04.2018----------------
                clearing.push(data.ABONO);
                //DRS 28.09.2018
                clearing.push(data.NOMBREA);
                clearing.push(data.CARGO);
                clearing.push(data.NOMBREC);
                clearing.push(data.CLEARING);
                clearing.push(data.LIMITE);
                //RSG 26.04.2018----------------
                
            } else {
                M.toast({
                    classes: "cWarnning",
                    displayLength: 1000000,
                    html: '<span style="padding-right:15px;"><i class="material-icons yellow-text">info</i></span> Sin Cuentas relacionadas '
                    + '<button class="btn-small btn-flat toast-action" onclick="dismiss(\'cWarnning\')">Aceptar</button>'
                });
            }

        },
        error: function (xhr, httpStatusMessage, customErrorMessage) {
            M.toast({ html: httpStatusMessage });
        },
        async: false
    });
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
    var bool = false
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