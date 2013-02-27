/*globals _comma_separated_list_of_variables_*/

RegisterStartUp(function () {
    SearchAndCopy.CheckUser('Number1', 'Number1', 'Number2', null, 'msgIcon');
    SearchAndCopy.CheckUser('Number2', 'Number1', 'Number2', null, 'msgIcon');
});

SearchAndCopy.Master = {
    BackSearch: function () {
        var url = "../../SearchAndCopy/SearchUser.aspx";
        url = SetParameterValue("userName", _page.userName, url);
        location.href = url;
    },

    Back: function () {
        var url = '';
        if (_page.isSkip == '0') {
            url = '../../SearchAndCopy/Super/ReviewSettingMaster.aspx';
            url = SetParameterValue("userName", _page.userName, url);
            url = SetParameterValue("custId", _page.custId, url);
            location.href = url;
        }
        else {
            url = "../../SearchAndCopy/SearchUser.aspx";
            url = SetParameterValue("userName", _page.userName, url);
            location.href = url;
        }
    },

    CopyUser: function () {
        var txtName = $('Number1').value + $('Number2').value;
        var custId = _page.custId;

        if (txtName == _page.userNameDefault) {
            return;
        }

        if ($('txtPwd').value == '') {
            ageMsg.Show(_page.alertConfirmPasswordEmpty);
            return;
        }

        if (!IsPassword($('txtPwd').value)) {
            ageMsg.Show(_page.lblPasswordRequire);
            return;
        }

        if ($('txtCredit').value == '') {
            ageMsg.Show(_page.lblCreditInvalid);
            return;
        }

        if (SearchAndCopy.ChangeNumeric($('txtCredit').value) > SearchAndCopy.ChangeNumeric(_page.maxCredit)) {
            ageMsg.Show(_page.lblCreditInvalid);
            return;
        }

        var params = ajax.CreateParams();
        params += 'userName=' + txtName;
        params += '&password=' + encodeURIComponent($('txtPwd').value);
        params += '&firstName=' + encodeURIComponent($('txtFirstName').value);
        params += '&lastName=' + encodeURIComponent($('txtLastName').value);
        params += '&phone=' + encodeURIComponent($('txtPhone').value);
        params += '&mobilePhone=' + encodeURIComponent($('txtMobilePhone').value);
        params += '&fax=' + encodeURIComponent($('txtFax').value);
        params += '&giveCredit=' + $('txtCredit').value;
        params += '&custId=' + custId;

        function OnComplete(result) {
            if (result.errCode == 999) {
                setTimeout("SearchAndCopy.AutoInsert('" + result.errCode + "','" + result.errMsg + "','" + custId + "', 4);", 3000);
            }
            else {
                setTimeout("age.HideMaskDiv();ageMsg.Show('" + result.errMsg + "');", 500);
            }
        }
        ageMsg.Hide();
        age.ShowMaskDiv(true);
        // Post with return result in JSON format
        ajax.PostJSON(
                '../../SearchAndCopy/Handlers/Super/CopyMaster.ashx',
                params,
                OnComplete,
                false // synchronous
        );
    }
};

