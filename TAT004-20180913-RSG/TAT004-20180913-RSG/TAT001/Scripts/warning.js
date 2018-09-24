var div = $("#validaciones");
var app = "<script>"
for (var i = 0; i < lista.length; i++) {
    var campo = document.getElementById(lista[i].ID);
    if (campo != undefined) {
        //app += "$('#" + lista[i].ID + "').on('" + lista[i].ACTION + "', function (e) { validar('" + lista[i].ID + "', '" + lista[i].COND + "', '" + lista[i].MSG + "', '" + lista[i].TIPO + "', '" + lista[i].COLOR + "', '" + lista[i].ELEM + "', " + i + ")}); ";
        app += "$('#" + lista[i].ID + "').on('" + lista[i].ACTION + "', function (e) {";
        app += "var ban = ";
        for (var j = 0; j < lista[i].COND.length; j++) {
            app += lista[i].COND[j].andor + "warning($(this).val(), '" + lista[i].COND[j].comp + "', '" + lista[i].COND[j].val2 + "')" + lista[i].COND[j].orand;
        }
        app += ";";
        app += "if($('#tsol_id').val() =='" + lista[i].TSOL+ "' |'"+lista[i].TSOL+"'== ''){";
        app += " validarN('" + lista[i].ID + "', " + lista[i].NUM + ", '" + lista[i].MSG + "', '" + lista[i].TIPO + "', '";
        app += lista[i].COLOR + "', '" + lista[i].ELEM + "', " + i + ", ban)}}); ";

    }
    else {
        //app += "$('body').on('" + lista[i].ACTION + "','." + lista[i].ID + "', function (e) { validarC('" + lista[i].ID + "', '" + lista[i].COND + "', '" + lista[i].MSG + "', '" + lista[i].TIPO + "', '" + lista[i].COLOR + "', '" + lista[i].ELEM + "', " + i + ");e.preventDefault(); e.stopPropagation();});";
        app += "$('body').on('" + lista[i].ACTION + "','." + lista[i].ID + "', function (e) { ";
        app += "var val = $(this).find('input').val(); if(val==undefined){ val = $(this).val();} var ban = ";
        for (var j = 0; j < lista[i].COND.length; j++) {
            app += lista[i].COND[j].andor + "warning(val, '" + lista[i].COND[j].comp + "', '" + lista[i].COND[j].val2 + "')" + lista[i].COND[j].orand;
        }
        app += ";";
        app += " validarNC('" + lista[i].ID + "', " + lista[i].NUM + ", '" + lista[i].MSG + "', '" + lista[i].TIPO + "', '";
        app += lista[i].COLOR + "', '" + lista[i].ELEM + "', " + i + ", ban)";
        app += ";e.preventDefault(); e.stopPropagation();});";
    }
}
app += "$('#tab_soporte').on('click', function (e) {var ban = validarTab(e, 'tab_info','Informacion_cont'); }); ";
app += "$('#tab_dis').on('click', function (e) { var ban = validarTab(e, 'tab_info', 'Informacion_cont');" +
    " if(ban){ ban = validarTab(e, 'tab_soporte', 'Soporte_cont');} }); ";
app += "$('#tab_fin').on('click', function (e) { var ban = validarTab(e, 'tab_info', 'Informacion_cont');" +
    " if(ban){ ban = validarTab(e, 'tab_soporte', 'Soporte_cont');} " +
    " if(ban){ ban = validarTab(e, 'tab_dis', 'Distribucion_cont');} }); ";

app += "function validarTab(e, tabid, div) {";
app += "var ban = true;";
for (var i = 0; i < lista.length; i++) {
    app += "ban = true;";
    //if (lista[i].TAB == 'tab_info' | true) {
    app += "if ('" + lista[i].TAB + "' == tabid) {";
    var campo = document.getElementById(lista[i].ID);
    if (campo != undefined) {
        //app += "$('#" + lista[i].ID + "').on('" + lista[i].ACTION + "', function (e) {";
        app += "ban = ";
        for (var j = 0; j < lista[i].COND.length; j++) {
            app += lista[i].COND[j].andor + "warning($('#" + lista[i].ID + "').val(), '" + lista[i].COND[j].comp + "', '" + lista[i].COND[j].val2 + "')" + lista[i].COND[j].orand;
        }
        app += ";";
        app += "if($('#tsol_id').val() =='" + lista[i].TSOL + "' |'" + lista[i].TSOL + "'== ''){";
        app += " validarN('" + lista[i].ID + "', " + lista[i].NUM + ", '" + lista[i].MSG + "', '" + lista[i].TIPO + "', '";
        //app += lista[i].COLOR + "', '" + lista[i].ELEM + "', " + i + ", ban)}); ";
        app += lista[i].COLOR + "', '" + lista[i].ELEM + "', " + i + ", ban);}else{ban=true;} ";

    }
    else {
        //app += "$('body').on('" + lista[i].ACTION + "','." + lista[i].ID + "', function (e) { ";
        //app += "var val = $('." + lista[i].ID + "').find('input').val(); if(val==undefined){ val = $('." + lista[i].ID + "').val();} ban = ";
        app += "var val = $('." + lista[i].ID + "').find('input').val();";
        if (lista[i].COND.length > 0 & lista[i].COND[0].comp != 'f') {
            app += "if(val==undefined){ var li = document.querySelectorAll('." + lista[i].ID + "');";
            app += "for(var j= 0; j<li.length;j++){ var par = $(li[j]).prop('tagName');if( par!= 'TH'){";
            app += " val = $(li[j]).text(); ban = ";
            for (var j = 0; j < lista[i].COND.length; j++) {
                app += lista[i].COND[j].andor + "warning(val, '" + lista[i].COND[j].comp + "', '" + lista[i].COND[j].val2 + "')" + lista[i].COND[j].orand;
            }
            app += ";if(!ban){j=li.length;}";
            app += "}}}";
            app += "else{";
            app += "ban = ";
            for (var j = 0; j < lista[i].COND.length; j++) {
                app += lista[i].COND[j].andor + "warning(val, '" + lista[i].COND[j].comp + "', '" + lista[i].COND[j].val2 + "')" + lista[i].COND[j].orand;
            }
            app += "}";
        } else {
            app += "ban = ";
            for (var j = 0; j < lista[i].COND.length; j++) {
                app += lista[i].COND[j].andor + "warning(val, '" + lista[i].COND[j].comp + "', '" + lista[i].COND[j].val2 + "')" + lista[i].COND[j].orand;
            }
        }
        app += ";";
        app += "if($('#tsol_id').val() =='" + lista[i].TSOL + "' |'" + lista[i].TSOL + "'== ''){";
            app += " validarNC('" + lista[i].ID + "', " + lista[i].NUM + ", '" + lista[i].MSG + "', '" + lista[i].TIPO + "', '";
            app += lista[i].COLOR + "', '" + lista[i].ELEM + "', " + i + ", ban);}else{ban=true;}";
        }
    //}
        app += "if('" + lista[i].TIPO + "'=='info'){ban=true;}";
        var tabb = "";
        if (lista[i].TAB == "tab_info") {
            tabb = "Informacion_cont";
        }
        if (lista[i].TAB == "tab_soporte") {
            tabb = "Soporte_cont";
        }
        if (lista[i].TAB == "tab_dist") {
            tabb = "Distribucion_cont";
        }
    app += "if(!ban){selectTab('" + tabb + "', e)}}";
}
app += " return ban;}";


//app += "function validarInfo(){";
//for (var i = 0; i < lista.length; i++) {
//    app += "if(!valido('" + lista[i][0] + "', '" + lista[i][1] + "')){if('" + lista[i][3] + "' == 'error'){selectTab('Informacion_cont')}}";
//}
//app += "}";
//app += " if(!valid('payer_id')){if('error' == 'error'){selectTab('Informacion_cont')}}";

div.append(app);


function condiciones(ii) {
    var ban = true;
    var cont = 0;
    if (lista[ii].CONDICION != undefined)
        if (lista[ii].CONDICION != "undefined")
            if (lista[ii].CONDICION.length > 0) {
                for (var i = 0; i < lista[ii].CONDICION.length; i++) {
                    var ccc = lista[ii].CONDICION;
                    if (document.getElementById(ccc[i].id).value == ccc[i].val)
                        cont++;
                }
                if (cont != lista[ii].CONDICION.length)
                    ban = false;
            }
    return ban;
}


function toast(clases, dura, icon, mensaje, color) {
    dismiss(clases);
    M.toast({
        classes: clases,
        displayLength: dura,
        html: '<span style="padding-right:15px;"><i class="material-icons ' + color + '-text">' + icon + '</i></span>  ' + mensaje
        + '<button class="btn-small btn-flat toast-action" onclick="dismiss(\'toast\')">Aceptar</button>'
    });
}

function dismiss(clase) {
    var toastElement = document.querySelectorAll('.' + clase);
    for (var i = 0; i < toastElement.length; i++) {
        var toastInstance = M.Toast.getInstance(toastElement[i]);
        toastInstance.dismiss();
    }
}

function selectTab(tab, e) {
    e.preventDefault();
    e.stopPropagation();
    var ell = document.getElementById("tabs");
    var instances = M.Tabs.getInstance(ell);
    instances.select(tab);
}

function esValido(campo) {
    campo.classList.add("valid");
    campo.classList.remove("invalid");
    campo.classList.remove("warn");
}
function esInvalido(campo) {
    campo.classList.add("invalid");
    campo.classList.remove("valid");
    campo.classList.remove("warn");
}
function esWarning(campo) {
    campo.classList.add("warn");
    campo.classList.remove("valid");
    campo.classList.remove("invalid");
}
function esValidoC(campo) {
    $(campo).find('input').addClass("valid");
    $(campo).find('input').removeClass("invalid");
    $(campo).find('input').removeClass("warn");
}
function esInvalidoC(campo) {
    $(campo).find('input').addClass("invalid");
    $(campo).find('input').removeClass("valid");
    $(campo).find('input').removeClass("warn");
}
function esWarningC(campo) {
    campo.classList.add("warn");
    campo.classList.remove("valid");
    campo.classList.remove("invalid");
}



function validarN(id, num, mensaje, icon, color, elem, ii, ban) {
    var campoVal = document.getElementById(id);
    var campo = document.getElementById(elem);

    if (campo != undefined & campoVal != undefined) {
        var value = campoVal.value;
        if (condiciones(ii)) {

            //var valid = valido(value, condicion, ii);
            var valid = ban;
            //var tipo_condicion = condicion.split("-")[0];
            var tipo_condicion = num;
            //var valor_condicion = condicion.split("-")[1];

            if (!valid) {
                //dismiss(id + "-" + tipo_condicion);
                //M.toast({ classes: id + "-" + tipo_condicion, displayLength: 1000000, html: '<span style="padding-right:15px;"><i class="material-icons">' + icon + '</i></span>  ' + mensaje + '<button class="btn-flat toast-action" onclick="dismiss(\'toast\')">Aceptar</button>' });
                toast(id + "-" + tipo_condicion, 1000000, icon, mensaje, color);
                if (color == "red")
                    esInvalido(campo);
                else
                    esWarning(campo);
            } else {
                esValido(campo);
                dismiss(id + "-" + tipo_condicion);
            }
        }
    }
}
function validarNC(id, num, mensaje, icon, color, elem, ii, ban) {
    //var campoVal = document.getElementById(id);
    //var campo = document.getElementById(elem);
    var campoVal = document.querySelectorAll("." + id);
    var campo = document.querySelectorAll("." + elem);
    for (var i = 0; i < campoVal.length; i++) {
        if (campo != undefined & campoVal != undefined & campo.length == campoVal.length) {
            var value = $(campoVal[i]).find('input').val();
            if (lista[i].COND.length > 0 & lista[i].COND[0].comp != 'f') {
                if (value == undefined)
                    value = $(campoVal[i]).text();
            }
            var valid = ban;
            var tipo_condicion = num;
            //if (value == undefined) {
            //    var inputid = $(campoVal[i]).attr('id');
            //    if (!valid)
            //        toast(id + "-" + inputid, 1000000, icon, mensaje, color);
            //} else {
            if (!valid) {
            } else {
            }
            //}
            if (condiciones(ii) & value != undefined) {

                //var valid = valido(value, condicion, ii);
                //var tipo_condicion = condicion.split("-")[0];
                //var valor_condicion = condicion.split("-")[1];

                if (!valid) {
                    //dismiss(id + "-" + tipo_condicion);
                    //M.toast({ classes: id + "-" + tipo_condicion, displayLength: 1000000, html: '<span style="padding-right:15px;"><i class="material-icons">' + icon + '</i></span>  ' + mensaje + '<button class="btn-flat toast-action" onclick="dismiss(\'toast\')">Aceptar</button>' });
                    //toast(id + "-" + tipo_condicion, 1000000, icon, mensaje, color);
                    toast(id + "-" + num, 1000000, icon, mensaje, color);
                    if (color == "red")
                        esInvalidoC(campo[i]);
                    else
                        esWarningC(campo[i]);
                    //for (var j = 0; j < campo.length; j++) {
                    //    esInvalido(campo[j]);
                    //}
                    i = campoVal.length;
                } else {
                    esValidoC(campo[i]);
                    //for (var j = 0; j < campo.length; j++) {
                    //    esValido(campo[j]);
                    //}
                    dismiss(id + "-" + tipo_condicion);
                }
            }
        }
    }
}


function warning(val1, comp, val2) {
    var ban = false;
    if (comp == '=') {
        if (val1 == val2)
            return true;
    } if (comp == '!=') {
        if (val1 != val2)
            return true;
    } if (comp == '&') {
        if (val1 & val2)
            return true;
        else
            return false;
    } if (comp == '|') {
        if (val1 | val2)
            return true;
        else
            return false;
    } if (comp == 'n') {
        if ($.isNumeric(toNum(val1)))
            return true;
    } if (comp == 'e') {
        if ($.isNumeric(parseFloat(toNum(val1))) & Number.isInteger(parseFloat(toNum(val1))))
            return true;
    } if (comp == 'dec') {
        if (!Number.isInteger(parseFloat(toNum(val1))) & $.isNumeric(parseFloat(toNum(val1))))
            return true;
    }

    if ($.isNumeric(toNum(val1))) {
        if (comp == '>') {
            if (parseFloat(toNum(val1)) > parseFloat(toNum(val2)))
                return true;
        } if (comp == '<') {
            if (parseFloat(toNum(val1)) < parseFloat(toNum(val2)))
                return true;
        } if (comp == '>=') {
            if (parseFloat(toNum(val1)) >= parseFloat(toNum(val2)))
                return true;
        } if (comp == '<=') {
            if (parseFloat(toNum(val1)) <= parseFloat(toNum(val2)))
                return true;
        }
    }
    if (comp == "l") {
        var longitud = parseInt(val2);
        if (longitud > 0) {
            if (val1.length == longitud)
                return true;
        }
    }
    else if (comp == "f") {
        ban =  evaluarFiles();
    }
    else if (comp == "d") {
        ban = isDate(val1);
    }
    else if (comp == "M") {
        ban = ($("#select_neg").val() == "M");
    }
    else if (comp == "P") {
        ban = ($("#select_neg").val() == "P");
    }
    else if (comp == "MA") {
        ban = ($("#select_dis").val() == "M");
    }
    else if (comp == "CA") {
        ban = ($("#select_dis").val() == "C");
    }
    else if (comp == "TOT") {
        ban =  (toNum(val1) == toNum($("#total_dis").text()))
    }
    else if (comp == "T") {
        ban =  (parseFloat(toNum($("#total_dis").text()))>0)
    }
    else if (comp == "DIS") {
        evaluarDisTable();
        var len = $("#table_dis > tbody  > tr[role='row']").length;
        if (len > 0) {
            var cont = 0;
            $('#table_dis > tbody  > tr').each(function () {
                var mat = $(this).find("td:eq(" + (3) + ") input").val();
                var cat = $(this).find("td:eq(" + (4) + ")").text();
                if (mat != undefined & mat != "")
                    cont++;
                else if(cat != undefined & cat != "")
                    cont++;

            });
        }
        ban =  cont > 0;
    }
    else if (comp == "INI") {
        ban = (val1.startsWith(val2));
    }
    else if (comp == "mail") {
        ban =  validateEmail(val1);
    }
    else if (comp == "L") {
        ban =  ligada();
    }
    else if (comp == "nL") {
        ban =  !ligada();
    }
    return ban;
}

function validateEmail(email) {
    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(String(email).toLowerCase());
}

                //var val = 'asdasdasd';
                ////var qq = warning(warning(val, "n", 0), '|', warning(warning(val, ">", 0), "&", warning(val, "<=", 100)));
                //var qq = warning(warning(val, "n", 0), '|', warning(warning(val, "==", "200c"), "|", warning(val, "==", "201c")));
                //alert(qq);
        
//function validarTab(e, tabId, tab) {
//    //M.Toast.dismissAll();
//    var ban = true;
//    for (var i = 0; i < lista.length; i++) {
//        if (lista[i].TAB == tabId) {
//            var condicion = lista[i].COND;
//            var id = lista[i].ID;
//            var campoVal = document.getElementById(lista[i].ID);
//            var campo = document.getElementById(lista[i].ELEM);

//            var clase = false;
//            if (campoVal == undefined & campo == undefined) {
//                campoVal = document.querySelectorAll("." + lista[i].ID);
//                campo = document.querySelectorAll("." + lista[i].ELEM);
//                clase = true;
//            }

//            if (campo != undefined & campoVal != undefined) {
//                var icon = lista[i].TIPO;
//                var color = lista[i].COLOR;
//                var mensaje = lista[i].MSG;
//                //var tipo_condicion = condicion.split("-")[0];
//                //var valor_condicion = condicion.split("-")[1];

//                if (!clase) {
//                    if (condiciones(i)) {
//                        //var b = valido(campoVal.value, lista[i].COND, i);
//                        var b = valido(campoVal.value, lista[i].COND, i);
//                        if (!b) {
//                            toast(id + "-" + tipo_condicion, 1000000, icon, mensaje, color);
//                            if (lista[i].TIPO == 'error') {
//                                esInvalido(campo);
//                                selectTab(tab, e);
//                                campo.focus();
//                                ban = false;
//                            }
//                        } else {
//                            if (lista[i].TIPO == 'error')
//                                dismiss(id + "-" + tipo_condicion);
//                        }
//                    }
//                } else {
//                    for (var j = 0; j < campoVal.length; j++) {
//                        var value = $(campoVal[j]).find('input').val();
//                        if (value != undefined | tipo_condicion == 'f') {
//                            if (tipo_condicion == 'f')
//                                value = '';
//                            if (condiciones(i)) {
//                                var valid = valido(value, condicion, i);
//                                if (!valid) {
//                                    toast(id + "-" + tipo_condicion, 1000000, icon, mensaje, color);
//                                    if (lista[i].TIPO == 'error') {
//                                        esInvalidoC(campo[i]);
//                                        selectTab(tab, e);
//                                        $(campo).find('input').focus();
//                                        ban = false;
//                                    }
//                                } else {
//                                    dismiss(id + "-" + tipo_condicion);
//                                }
//                                j = campoVal.length;
//                            }
//                        } else {

//                        }
//                    }
//                }
//            }
//        }
//    }
//    return ban;
//}

//function validar(id, condicion, mensaje, icon, color, elem, ii) {
//    var campoVal = document.getElementById(id);
//    var campo = document.getElementById(elem);

//    if (campo != undefined & campoVal != undefined) {
//        var value = campoVal.value;
//        if (condiciones(ii)) {
//            var valid = valido(value, condicion, ii);
//            var tipo_condicion = condicion.split("-")[0];
//            var valor_condicion = condicion.split("-")[1];

//            if (!valid) {
//                //dismiss(id + "-" + tipo_condicion);
//                //M.toast({ classes: id + "-" + tipo_condicion, displayLength: 1000000, html: '<span style="padding-right:15px;"><i class="material-icons">' + icon + '</i></span>  ' + mensaje + '<button class="btn-flat toast-action" onclick="dismiss(\'toast\')">Aceptar</button>' });
//                toast(id + "-" + tipo_condicion, 1000000, icon, mensaje, color);
//                esInvalido(campo);
//            } else {
//                esValido(campo);
//                dismiss(id + "-" + tipo_condicion);
//            }
//        }
//    }
//}
//function validarC(id, condicion, mensaje, icon, color, elem, ii) {
//    var campoVal = document.querySelectorAll("." + id);
//    var campo = document.querySelectorAll("." + elem);
//    if (campo != undefined & campoVal != undefined & campo.length == campoVal.length) {
//        for (var i = 0; i < campoVal.length; i++) {
//            var value = $(campoVal[i]).find('input').val();
//            if (value != undefined) {
//                if (condiciones(ii)) {
//                    var valid = valido(value, condicion, ii);
//                    var tipo_condicion = condicion.split("-")[0];
//                    var valor_condicion = condicion.split("-")[1];

//                    if (!valid) {
//                        //dismiss(id + "-" + tipo_condicion);
//                        //M.toast({ classes: id + "-" + tipo_condicion, displayLength: 1000000, html: '<span style="padding-right:15px;"><i class="material-icons">' + icon + '</i></span>  ' + mensaje + '<button class="btn-flat toast-action" onclick="dismiss(\'toast\')">Aceptar</button>' });
//                        toast(id + "-" + tipo_condicion, 1000000, icon, mensaje, color);
//                        esInvalidoC(campo[i]);
//                    } else {
//                        esValidoC(campo[i]);
//                        dismiss(id + "-" + tipo_condicion);
//                    }
//                    i = campoVal.length;
//                }
//            }
//        }
//    }
//}
//function valido(value, condicion, ii) {
//    var ban = true;
//    var valid = true;
//    if (ban) {
//        var tipo_condicion = condicion.split("-")[0];
//        var valor_condicion = condicion.split("-")[1];

//        if (tipo_condicion != "") {
//            if (tipo_condicion == "c") {
//                if (valor_condicion == "!=null") {
//                    if (value == "")
//                        valid = false;
//                } else if (valor_condicion == "!=") {
//                    var compara = $("#" + condicion.split("-")[2]).text();
//                    if (parseFloat(toNum(value)) != parseFloat(toNum(compara)))
//                        valid = false
//                } else if (valor_condicion == ">") {
//                    var compara = condicion.split("-")[2];
//                    if (parseFloat(toNum(value)) <= parseFloat(toNum(compara)))
//                        valid = false
//                }
//            } else if (tipo_condicion == "n") {
//                if (tipo_condicion == 'n') {
//                    if (!$.isNumeric(value))
//                        valid = false;
//                }
//            } else if (tipo_condicion == "l") {
//                var longitud = parseInt(valor_condicion);
//                if (longitud > 0) {
//                    if (value.length < longitud)
//                        valid = false;
//                }
//            } else if (tipo_condicion == "lm") {
//                var longitud = parseInt(valor_condicion);
//                if (longitud > 0) {
//                    if (value.length > longitud)
//                        valid = false;
//                }
//            } else if (tipo_condicion == "f") {
//                valid = evaluarFiles();
//            }
//        }
//    }
//    return valid;
//}