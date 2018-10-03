
$('body').on('keydown.autocomplete', '.input_material', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    var vk = '0152';
    vk = document.getElementById("txt_vkorg").value;
    var vt = '50';
    vt = document.getElementById("txt_vtweg").value;
    var sp = 'ES';
    
    sp = document.getElementById("txt_spras").value;
    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                //url: 'materiales',//Anterior
                url: '../Listas/materiales',
                dataType: "json",
                data: { "Prefix": request.term, vkorg: vk, vtweg: vt, spras: sp },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        //return { label: item.ID + " - " + item.MAKTX, value: item.ID };
                        return { label: trimStart('0', item.ID) + " - " + item.MAKTX, value: trimStart('0', item.ID) };//RSG 07.06.2018
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

            var label = ui.item.label;
            var value = ui.item.value;
            var desc = label.split('-')
            selectMaterial(value, desc[1], tr);
        }
    });
});

function trimStart(character, string) {//RSG 07.06.2018
    var startIndex = 0;

    while (string[startIndex] === character) {
        startIndex++;
    }

    return string.substr(startIndex);
}
//function allowAddrow(table)
//{
//    var index = getIndex();
//    var forMat = 3;
//    var mat2 = ('#table_dis').find('tr').find('>td:eq(' + forMat + ')');
//    console.log(mat2);
//    if (!unica) {

//        var addedRow = addRowMat(t, "", "", "", "", "", "", "", "", "", "", "", relacionada, relacionadaed, reversa, ddate, adate, "", "");
//        $('#table_dis').css("font-size", "12px");
//        $('#table_dis').css("display", "table");
//        t.column(0).visible(false);
//        t.column(1).visible(false);
//    }
//    else if (unica)
//    {
//        M.toast({ html: 'Uno de los materiales no se puede mezclar con otros' });
//        tr.find('td').eq((5 + index)).addClass("errorMaterial");
//    }

//}
function selectMaterial(val, desc, tr) {
    var index = getIndex();
    var cat = getCategoria(val);    
    desc = $.trim(desc);
    if (index == -2) {
        unica1 = false;
        if (cat.UNICA)
        {
            tr.addClass("unica");
            M.toast({ html: cat.TXT50 + ' Advertencia este material es único' });
            tr.find("td:eq(" + (6 + index) + ")").text(cat.TXT50);
            //Descripción
            tr.find("td:eq(" + (7 + index) + ")").text(desc);
        }
        if (!cat.UNICA)
        {
            tr.addClass("nounica");
            tr.find("td:eq(" + (6 + index) + ")").text(cat.TXT50);
            //Descripción
            tr.find("td:eq(" + (7 + index) + ")").text(desc);
        }

    } else { 
    //Add MGC B20180705 2018.07.09 Validar que los materiales no existan duplicados en la tabla
    var matExist = valMaterial(val);
   
    //Categoría
    var cat = getCategoria(val);

    tr.find("td:eq(" + (6 + index) + ")").text(cat.TXT50);
    //Descripción
    tr.find("td:eq(" + (7 + index) + ")").text(desc);

    //Remove background a celda de material
    //Add MGC B20180705 2018.07.09 Validar que los materiales no existan duplicados en la tabla
    if (matExist) {
        M.toast({ html: 'Ya hay un material con ese mismo identificador' });
        tr.find('td').eq((5 + index)).addClass("errorMaterial");
    } else {
        tr.find('td').eq((5 + index)).removeClass("errorMaterial");
    }
    }
}

$('body').on('keydown.autocomplete', '.input_proveedor', function () {
    var tr = $(this).closest('tr'); //Obtener el row
    auto(this).autocomplete({
        source: function (request, response) {
            auto.ajax({
                type: "POST",
                url: 'proveedores',
                dataType: "json",
                data: { "Prefix": request.term },
                success: function (data) {
                    response(auto.map(data, function (item) {
                        return { label: item.ID + " - " + item.NOMBRE, value: item.ID };
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

            var label = ui.item.label;
            var value = ui.item.value;
            var desc = label.split('-')
            selectProveedor(value, desc[1], tr);
        }
    });
});

function selectProveedor(val, desc, tr) {

    desc = $.trim(desc);
    //Descripción
    tr.find("td.PROVEEDOR_TXT").text(desc);

    //Remove background a celda de proveedor
    tr.find("td.PROVEEDOR").removeClass("errorProveedor");
}