$(window).on('load', function () {

    var kunnr = $('#payer_id').val();

    asignarPresupuesto(kunnr);
});


function asignarPresupuesto(kunnr) {

    $.ajax({
        type: "POST",
        url: '../../Solicitudes/getPresupuesto',
        dataType: "json",
        data: { "kunnr": kunnr },

        success: function (data) {

            if (data !== null || data !== "") {

                var pcan = (data.P_CANAL / 1).toFixed(2);
                var pban = (data.P_BANNER / 1).toFixed(2);
                var pcc = (data.PC_C / 1).toFixed(2);
                var pca = (data.PC_A / 1).toFixed(2);
                var pcp = (data.PC_P / 1).toFixed(2);
                var pct = (data.PC_T / 1).toFixed(2);
                var consu = (data.CONSU / 1).toFixed(2);
                var _xdec = $("#dec").val();
                var _xm = $("#miles").val();
                if (_xdec === '.') {
                    $('#p_canal').text('$' + (pcan.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")));
                    $('#p_banner').text('$' + (pban.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")));
                    $('#pc_c').text('$' + (pcc.toString().replace(/\B(?=(\d{3})+(?!\d))/g, _xm)));
                    $('#pc_a').text('$' + (pca.toString().replace(/\B(?=(\d{3})+(?!\d))/g, _xm)));
                    $('#pc_p').text('$' + (pcp.toString().replace(/\B(?=(\d{3})+(?!\d))/g, _xm)));
                    $('#pc_t').text('$' + (pct.toString().replace(/\B(?=(\d{3})+(?!\d))/g, _xm)));
                    var _xcs = (consu.toString().replace(/\B(?=(\d{3})+(?!\d))/g, _xm));
                    if (_xcs.indexOf("-") >= 0) {
                        var _dsARx = _xcs;
                        _dsARx = _dsARx.replace('-', '(');
                        _dsARx += ")";
                        _xcs = _dsARx;
                    }
                    $('#consu').text('$' + _xcs);
                } else
                    if (_xdec === ',') {
                        pcan = pcan.replace('.', ',');
                        pban = pban.replace('.', ',');
                        pcc = pcc.replace('.', ',');
                        pca = pca.replace('.', ',');
                        pcp = pcp.replace('.', ',');
                        pct = pct.replace('.', ',');
                        consu = consu.replace('.', ',');
                        $('#p_canal').text('$' + (pcan.toString().replace(/\B(?=(\d{3})+(?!\d))/g, _xm)));
                        $('#p_banner').text('$' + (pban.toString().replace(/\B(?=(\d{3})+(?!\d))/g, _xm)));
                        $('#pc_c').text('$' + (pcc.toString().replace(/\B(?=(\d{3})+(?!\d))/g, _xm)));
                        $('#pc_a').text('$' + (pca.toString().replace(/\B(?=(\d{3})+(?!\d))/g, _xm)));
                        $('#pc_p').text('$' + (pcp.toString().replace(/\B(?=(\d{3})+(?!\d))/g, _xm)));
                        $('#pc_t').text('$' + (pct.toString().replace(/\B(?=(\d{3})+(?!\d))/g, _xm)));
                        var _xcs = (consu.toString().replace(/\B(?=(\d{3})+(?!\d))/g, _xm));
                        if (_xcs.indexOf("-") >= 0) {
                            var _dsARx = _xcs;
                            _dsARx = _dsARx.replace('-', '(');
                            _dsARx += ")";
                            _xcs = _dsARx;
                        }
                        $('#consu').text('$' + _xcs);
                    }
            }
            //LEJ 09.07.18-----------------------------------------------
        },
        error: function (xhr, httpStatusMessage, customErrorMessage) {
            M.toast({ html: httpStatusMessage });
        },
        async: false
    });

}