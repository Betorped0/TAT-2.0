function cierre() {
    //string sociedad_id, string tsol_id, int periodo_id, string usuario_id = null
    var soc = $("#sociedad_id").val();
    var tsol = $("#TSOL_ID").val();
    var periodo = $("#periodo").val();
    var user = $("#USUARIOD_ID").val();
    var bool = false
    $.ajax({
        type: "POST",
        url: root+'Listas/cierre',
        dataType: "text",
        data: { sociedad_id: soc, tsol_id: tsol, periodo_id: periodo, usuario_id: user },

        success: function (data) {
            if (data !== "X") {
                M.toast({
                    classes: "guardarWarnning",
                    displayLength: 1000000,
                    html: '<span style="padding-right:15px;"><i class="material-icons yellow-text">info</i></span> Periodo cerrado'
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