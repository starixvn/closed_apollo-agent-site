/*globals _comma_separated_list_of_variables_*/
RegisterStartUp(function () {
    SearchAndCopy.CheckUser('Number1', 'Number1', 'Number2', null, 'msgIcon');
    SearchAndCopy.CheckUser('Number2', 'Number1', 'Number2', null, 'msgIcon');
});

SearchAndCopy.Agent = {
    BackSearch: function () {
        var url = "../../SearchAndCopy/SearchUser.aspx";
        url = SetParameterValue("userName", _page.userName, url);
        location.href = url;
    },

    Back: function () {
        var url = '';
        if (_page.isSkip == '0') {
            url = '../../SearchAndCopy/Master/ReviewSettingAgent.aspx';
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

    OnblurTxtCredit: function (event) {
        age.FormatNumber(event, true);
        $('spnCreditMax').innerHTML = $('txtCredit').value;
    },

    addCommas: function (nStr) {
        nStr += '';
        x = nStr.split('.');
        x1 = x[0];
        x2 = x.length > 1 ? '.' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        }
        return x1 + x2;
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
            ageMsg.Show(_page.alertMemberCredit);
            return;
        }

        var credit = SearchAndCopy.ChangeNumeric($('txtCredit').value);
        var agentMaxCredit = SearchAndCopy.ChangeNumeric(_page.maxCredit);
        if (credit > agentMaxCredit) {
            ageMsg.Show(_page.lblCreditInvalidAgent);
            return;
        }

        var memberMaxCredit = SearchAndCopy.ChangeNumeric($('txtCredit').value);
        var membertCredit = SearchAndCopy.ChangeNumeric($('txtMaxCredit').value);
        if (membertCredit > memberMaxCredit) {
            var creditInvalid = _page.lblCreditInvalidMember.replace('{1}', SearchAndCopy.Agent.addCommas($('txtCredit').value))
            ageMsg.Show(creditInvalid);
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
        params += '&credit=' + $('txtCredit').value;
        params += '&maxCredit=' + $('txtMaxCredit').value;
        params += '&custId=' + custId;

        function OnComplete(result) {
            if (result.errCode == 999) {
                setTimeout("SearchAndCopy.AutoInsert('" + result.errCode + "','" + result.errMsg + "','" + custId + "',3);age.HideMaskDiv();", 3000);
            }
            else {
                setTimeout("age.HideMaskDiv();ageMsg.Show('" + result.errMsg + "');", 500);
            }
        }

        // Post with return result in JSON format
        ageMsg.Hide();
        age.ShowMaskDiv(true);
        ajax.PostJSON(
                '../../SearchAndCopy/Handlers/Master/CopyAgent.ashx',
                params,
                OnComplete,
                false // synchronous
        );
    }
};