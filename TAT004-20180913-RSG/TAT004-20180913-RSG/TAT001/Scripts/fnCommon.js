var fnCommon = {

    materializeInit: function (component,type) {
        switch (type) {
            case 'select':
                var options = {};
                var selects = document.querySelectorAll('select');
                M.FormSelect.init(selects, options);
                break;
            case 'datepicker':           
                var options = { format: 'dd/mm/yyyy' };
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
    selectRequired:function () {
        var combos = document.querySelectorAll('.select-dropdown.dropdown-trigger');
        var i = 0;
        combos.forEach(function (combo) {
            combo.setAttribute('required', '');
            combo.setAttribute('name', 'select_' + i);
            i++;
        });
    },
    formValidation:function (idForm) {
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
    dateRangeValidation: function (idFechaIni, idFechaFin) {
            var inicio = $('#' + idFechaIni),
                inicioM = M.Datepicker.getInstance(inicio),
                final = $('#' + idFechaFin),
                finalM = M.Datepicker.getInstance(final);

            if (inicio.val()) {
                var date = moment(inicio.val()).format('DD/MM/YYYY');
                finalM.options.minDate = new Date(date);
            }
            if (final.val()) {
                var date = moment(final.val()).format('DD/MM/YYYY');
                inicioM.options.maxDate = new Date(date);
            }
            inicioM.options.onSelect = function (val) {
                finalM.options.minDate = val;
            };
            finalM.options.onSelect = function (val) {
                inicioM.options.maxDate = val;
            };
    },
    configurarTable: function (idTable, scrollY, scrollX, language,idSelectPag,idGFilter) {
        var table =  $('#'+idTable).DataTable({
            scrollY: scrollY,
            scrollX: scrollX,
            scrollCollapse: true,
            language: {
                url: language
            },
            columnDefs: [{
                targets: [0, 1, 2],
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
    }


};