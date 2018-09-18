
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

function selectMaterial(val, desc, tr) {
    var index = getIndex();
    desc = $.trim(desc);

    //Add MGC B20180705 2018.07.09 Validar que los materiales no existan duplicados en la tabla
    var matExist = valmaterial(val);

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