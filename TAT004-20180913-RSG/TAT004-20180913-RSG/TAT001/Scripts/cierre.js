function cierre() {
    //string sociedad_id, string tsol_id, int periodo_id, string usuario_id = null
    var soc = $("#sociedad_id").val();
    var tsol = $("#tsol_id").val();
    var periodo = $("#periodo").val();
    var user = $("#USUARIOD_ID").val();
    var bool = false
    $.ajax({
        type: "POST",
        url: '../../Listas/cierre',
        dataType: "text",
        data: { sociedad_id: soc, tsol_id: tsol, periodo_id: periodo, usuario_id: user },

        success: function (data) {
            if (data == "X") {
                M.toast({ html: "Periodo cerrado" });
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