var Racing = {

    AutoPT_OnClick: function (element, id_Upline, id_Self, idMax_Upline, idMax_Self, idMin_Self) {
        var idList = el.id.replace('chkAuto', 'list');
        var idChkAuto = el.id.replace('chk', 'list');
        if ($(idList)) {
            $(idList).disabled = element.checked;
        }
        else {
            var ID = element.id;
            var suffix = ID.substring(ID.indexOf('_'), ID.length);
            if ($('chkCopyFromWin' + suffix)) {
                $('chkCopyFromWin' + suffix).disabled = !element.checked;

                if ($('chkCopyFromWin' + suffix).checked) {
                    $('chkCopyFromWin' + suffix).checked = false;
                    var _arr = "Arr" + suffix;
                    //SetCopy_Control(false, eval(_arr), Getlocation(el));
                }
            }

            if (suffix == "_151_41") {
                if ($('chkCopyAll')) {
                    $('chkCopyAll').disabled = element.checked ? false : true;

                    if ($('chkCopyAll').checked) {
                        $('chkCopyAll').checked = false;
                        //chkCopyAll_OnClick($('chkCopyAll'), ArrExc);
                        $('chkCopyFromWin_151_41').disabled = true;
                    }
                }

            }
        }
        if ($(idChkAuto)) $(idChkAuto).disabled = !element.checked;

        if ($(id_Upline) || $(id_Self)) {            

            if (!idMax_Upline || !idMax_Self) return;

            var isAuto = el.checked;
            if (isAuto) // Auto PT is checked
            {
                var maxVal = Racing.GetValue(idMax_Upline);
                var minVal = Racing.GetValue(idMin);                
            }
            else  // Not Auto
            {
                var maxVal_1 = Racing.GetValue(idMax_Upline);
                var maxVal_2 = Racing.GetValue(idMax_Self);

                var curVal = $(id_Upline).value;
                var minVal = Racing.GetValue(idMin);
                var maxVal = Racing.GetMaxValue(maxVal_1, maxVal_2, curVal);
              
            }
            PT_RenderCombo($(id_Self), minVal, maxVal, 0.1, $(id_Self).value);

        }
    },

    GetValue: function (id) {
        var val = 0;
        if (id != '' && id != 'undefined' && id != null) {
            var idPage = '_page.' + id;
            val = eval(idPage);
        }
        return val;
    },

    GetMaxValue: function (Max_1, Max_2, Curr) {
        var max = 0;
        max = Max_1 - Curr;
        if (max > Max_2)
            max = Max_2;
        return max;

    }

}