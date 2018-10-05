var fnCommon = {

    materializeInit: function (component, type, spras_id) {
        switch (type) {
            case 'select':
                var options = {};
                var selects = document.querySelectorAll('select');
                M.FormSelect.init(selects, options);
                break;
            case 'datepicker':
                var options = { format: 'dd/mm/yyyy' };
                if (spras_id=='ES'){
                    options.i18n = {
                        clear: 'Limpiar',
                        today: 'Hoy',
                        done: 'Seleccionar',
                        previousMonth: '‹',
                        nextMonth: '›',
                        months: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                        monthsShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                        weekdaysShort: ['Dom', 'Lun', 'Mar', 'Mie', 'Jue', 'Vie', 'Sab'],
                        weekdays: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
                        weekdaysAbbrev: ['D', 'L', 'M', 'X', 'J', 'V', 'S']
                    };
                }
                var datepickers = document.querySelectorAll('.datepicker');
                M.Datepicker.init(datepickers, options);
                break;
            case 'timepicker':
                $(component).pickatime({
                    twelvehour: false,
                    donetext: 'OK',
                    cleartext: '',
                    autoclose: false,
                    value: ''
                });
                break;
            case 'tabs':
                var options = {};
                var tabs = document.querySelectorAll('.tabs');
                M.Tabs.init(tabs, options);
                break;
            case 'collapsible':
                var options = {};
                var collapsibles = document.querySelectorAll('.collapsible');
                M.Collapsible.init(collapsibles, options);
                break;
            default:
                break;
        }
    },
    selectRequired: function () {
        var combos = document.querySelectorAll('.select-dropdown.dropdown-trigger');
        var i = 0;
        combos.forEach(function (combo) {
            combo.setAttribute('required', '');
            combo.setAttribute('name', 'select_' + i);
            i++;
        });
    },
    formValidation: function (idForm) {
        $.validator.setDefaults({
            errorClass: 'invalid',
            validClass: 'valid'
        });
        $.validator.addMethod(
            'required',
            function (value, element, requiredValue) {
                return !(value === '');
            },
            ''
        );
        $('#' + idForm).validate();
    },
   
    dateRangeValidation: function (idFechaIni, idFechaFin,_idFechaIni, _idFechaFin) {

        fnCommon.setDateRange(idFechaIni, idFechaFin, null);
        fnCommon.setDateRange(idFechaFin, _idFechaIni, idFechaIni);
        if (_idFechaIni && _idFechaFin) { fnCommon.setDateRange(_idFechaIni, _idFechaFin, idFechaFin); }
        if (_idFechaFin && _idFechaFin) { fnCommon.setDateRange(_idFechaFin, null, _idFechaIni); }

       
    },
    setDateRange: function (idFechaFSelect, idFechaFMin, idFechaFMax) {
        var fechaFSelect = $('#' + idFechaFSelect),
            fechaFSelectM = M.Datepicker.getInstance(fechaFSelect),

            fechaFMin = idFechaFMin ? $('#' + idFechaFMin) : null,
            fechaFMinM = fechaFMin ? M.Datepicker.getInstance(fechaFMin) : null,

            fechaFMax = idFechaFMax ? $('#' + idFechaFMax) : null,
            fechaFMaxM = fechaFMax ? M.Datepicker.getInstance(fechaFMax) : null;
        
        fechaFSelectM.options.onSelect = function (val) {
            if (fechaFMinM) { fechaFMinM.options.minDate = val; }
            if (fechaFMaxM) { fechaFMaxM.options.maxDate = val; }
            };
        
          

        if (fechaFSelect.val()) {
            var date = moment(fechaFSelect.val(), 'DD/MM/YYYY');
            if (fechaFMinM) { fechaFMinM.options.minDate = date; }
            if (fechaFMaxM) { fechaFMaxM.options.maxDate = date; }
        }
    },
    configurarTable: function (idTable, scrollY, scrollX, urlLanguage,idSelectPag,idGFilter) {
        var table =  $('#'+idTable).DataTable({
            scrollY: scrollY,
            scrollX: scrollX,
            scrollCollapse: true,
            language: {
                url: urlLanguage
            },
            columnDefs: [{
                targets: [0, 1, 2,4,5,6],
                className: 'mdl-data-table__cell--non-numeric'
            }]
        });

        var selectPagVal = $('#' + idSelectPag).val();
        table.page.len(selectPagVal).draw();
        $('#' + idSelectPag).on('change', function () {
            table.page.len(this.value).draw();
        });

       
        $('#' + idTable + ' tbody').on('click', 'td.details-control', function () {
            var tr = $(this).closest('tr');
            var row = table.row(tr);

            if (row.child.isShown()) {
                // This row is already open - close it
                row.child.hide();
                tr.removeClass('shown');
            }
            else {
                // Open this row
                var child = format(row.data(), row, tr);
                if (child != undefined) {
                }
            }
        });

        $('input.global_filter').on('keyup click', function () {
            fnCommon.filterGlobal(idTable, idGFilter);
        });
    },
    filterGlobal: function (idTable, idGFilter) {
        var filterVal = $('#' + idGFilter).val();
        $('#' + idTable).DataTable().search(filterVal).draw();
    },
    fillOptionsInSelect: function (idSelect, url, idSelectToFill) {
        $('#' + idSelect).change(function () {
            var id = $('#' + idSelect).val();

            $.ajax({
                url: url ,
                data: { id: id },
                cache: false,
                type: 'POST',
                success: function (selectItems) {
                    var options = '<option value=""></option>';
                    selectItems.forEach(function (selectItem) {
                        options += '<option value=' + selectItem.Value + '>' + selectItem.Text + '</option>';
                    });
                    if (selectItems.length > 0) {
                        $('#' + idSelectToFill).html(options);
                        fnCommon.materializeInit('select', 'select');
                        fnCommon.selectRequired();
                    }
                }
            });
        });
    }


};