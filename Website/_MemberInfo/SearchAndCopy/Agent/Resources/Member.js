/*globals _comma_separated_list_of_variables_*/
RegisterStartUp(function () {
    SearchAndCopy.CheckUser('Number1', 'Number1', 'Number2', 'Number3', 'msgIcon');
    SearchAndCopy.CheckUser('Number2', 'Number1', 'Number2', 'Number3', 'msgIcon');
    SearchAndCopy.CheckUser('Number3', 'Number1', 'Number2', 'Number3', 'msgIcon');
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
            url = '../../SearchAndCopy/Agent/ReviewSettingMember.aspx';
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

    GetValidateImage: function () {
        var objDate = new Date();
        $("imgCode").src = "../../../_Authorization/Handlers/Captcha.ashx?code=" + objDate.getMilliseconds();
    },

    CopyUser: function () {
        var txtName = $('Number1').value + $('Number2').value + $('Number3').value;
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

        if ($('txtCaptchaCopyMember')) {
            if ($('txtCaptchaCopyMember').value == '') {
                ageMsg.Show(_page.errRequireValidationCode);
                $('txtCaptchaCopyMember').focus();
                return;
            }
        }

        var credit = SearchAndCopy.ChangeNumeric($('txtCredit').value);
        var memberMaxCredit = SearchAndCopy.ChangeNumeric(_page.maxCredit);

        if (credit > memberMaxCredit) {
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
        if ($('txtCaptchaCopyMember')) {
            params += '&code=' + $('txtCaptchaCopyMember').value;
        }

        function OnComplete(result) {
            if (result.errCode == 999) {
                setTimeout("SearchAndCopy.AutoInsertMember('" + result.errCode + "','" + result.errMsg + "','" + custId + "')", 4000);
            }
            else {
                setTimeout("age.HideMaskDiv();ageMsg.Show('" + result.errMsg + "');", 500);
                if ($('txtCaptchaCopyMember')) {
                    SearchAndCopy.Agent.GetValidateImage();
                    $('txtCaptchaCopyMember').value = '';
                    $('txtCaptchaCopyMember').focus();
                }
            }
        }

        // Post with return result in JSON format
        ageMsg.Hide();
        age.ShowMaskDiv(true);
        ajax.PostJSON(
                '../../SearchAndCopy/Handlers/Agent/CopyMember.ashx',
                params,
                OnComplete,
                false // synchronous
        );
    }
};