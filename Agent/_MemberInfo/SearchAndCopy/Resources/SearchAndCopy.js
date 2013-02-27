var isExisting = 0;
var custId = 0;
var errMsg = "";
var userNameOld = "";
var checkExistUserTimer = 0;

var SearchAndCopy = {
    ActiveMenuTab: function (activeID, roleId) {
        var url = "";

        switch (activeID) {
            case 0:
                $("IdCurr0").className = "current";
                $("IdCurr1").className = "";
                url = "SearchUser.aspx";
                break;
            case 1:
                $("IdCurr1").className = "current";
                $("IdCurr0").className = "";
                switch (roleId) {
                    case 2:
                        url = "../PositionTaking/Agent/AddNewMember.aspx";
                        break;
                    case 3:
                        url = "../PositionTaking/Master/AddNewAgent.aspx";
                        break;
                    case 4:
                        url = "../PositionTaking/Super/AddNewMaster.aspx";
                        break;
                }
                break;
            default:
                break;
        }

        window.parent.frmAddNewMember.location.href = url;
    },

    InitSearchAndCopy: function () {
        if ($('txtUserName') != null) {
            SearchAndCopy.InitTagSuggestion();
            $('MessageWarning').style.display = 'none';
            if (_page.userName == '') {
                SearchAndCopy.onclickUser();
            } else {
                $('txtUserName').value = _page.userName;
                $('txtUserName').className = 'text_normal';
            }
        }
        if (_page.errorCS == true) {
            ageMsg.Show(_page.MsgErrorCS);
        }
        if (_page.errorSB == true) {
            ageMsg.Show(_page.msgErrorSB);
        }

        if (top.errCodes.length > 0) {
            ageMsg.Show(top.errCodes.pop());
        }
        if ($('txtCredit')) {
            $('txtCredit').value = '0';
        }
        if ($('txtMaxCredit')) {
            $('txtMaxCredit').value = '0';
        }
    },

    onclickUser: function () {
        var userName = _page.userNameDefault;
        if ($('txtUserName').value == userName) {
            $('txtUserName').className = 'text_normal';
            $('txtUserName').value = '';
        }
        return;
    },

    onblurUser: function () {
        var userName = _page.userNameDefault;
        if ($('txtUserName').value == '') {
            $('txtUserName').className = 'text_italic';
            $('txtUserName').value = userName;
        } else if ($('txtUserName').value != userName) {
            if (userNameOld != $('txtUserName').value) {
                if ($("shadow").style.visibility != "visible") {
                    checkExistUserTimer = setTimeout("SearchAndCopy.CheckExistingUser($('txtUserName').value);", 300);
                    userNameOld = $('txtUserName').value;
                }
            } else {
                $('MessageWarning').style.display = 'inline-block';
            }
        }
    },

    onKeyPressUser: function (event) {
        if (!event) event = window.event;
        $('MessageWarning').style.display = 'none';
        var key = (event.keyCode) ? event.keyCode : event.which;
        if (key == 13) {
            SearchAndCopy.GetSetting(_page.roleId);
            return false;
        }
    },

    GetSetting: function (roleId) {
        var url = location.href;
        var folder = "";
        var txtName = $('txtUserName').value;

        if (txtName == _page.userNameDefault) {
            ageMsg.Show(_page.alertUserName);
            return;
        }

        clearTimeout(checkExistUserTimer);
        SearchAndCopy.CheckExistingUser($('txtUserName').value);

        if (isExisting == 1) {
            switch (roleId) {
                case 2:
                    url = 'Agent/ReviewSettingMember.aspx';

                    break;
                case 3:
                    url = "Master/ReviewSettingAgent.aspx";
                    break;
                case 4:
                    url = "Super/ReviewSettingMaster.aspx";
                    break;
                default:
                    break;
            }

            url = SetParameterValue("custId", custId, url);
            url = SetParameterValue("userName", txtName, url);
            location.href = url;
        } else {
            ageMsg.Show(errMsg);
        }
    },

    CheckExistingUser: function (username) {
        var params = ajax.CreateParams();
        params += 'username=' + username;

        function OnComplete(result) {
            // age.UnLock();
            var errCode = result.errCode;
            $('MessageWarning').style.display = 'inline-block';
            if (errCode == 999) {
                $('MessageWarning').className = 'PwdSuccess';

                isExisting = 1;
                custId = result.errMsg;
            } else {
                $('MessageWarning').className = 'PwdError';
                errMsg = result.errMsg;
                isExisting = 0;
            }
        }
        // age.Lock(false);

        ajax.PostJSON('Handlers/CheckUser.ashx', params, OnComplete, false
        // synchronous
        );
    },

    GetInfo: function (roleId) {
        var txtName = $('txtUserName').value;
        var url = location.href;
        var folder = "";
        if (txtName == _page.userNameDefault) {
            ageMsg.Show(_page.alertUserName);
            return;
        }

        clearTimeout(checkExistUserTimer);
        SearchAndCopy.CheckExistingUser($('txtUserName').value);

        if (isExisting == 1) {
            switch (roleId) {
                case 2:
                    url = "../../_MemberInfo/SearchAndCopy/Agent/InfoMember.aspx";

                    break;
                case 3:
                    url = "../../_MemberInfo/SearchAndCopy/Master/InfoAgent.aspx";
                    break;
                case 4:
                    url = "../../_MemberInfo/SearchAndCopy/Super/MasterInfo.aspx";
                    break;
                default:
                    break;
            }

            url = SetParameterValue("custId", custId, url);
            url = SetParameterValue("userName", txtName, url);
            url = SetParameterValue("isSkip", '1', url);
            location.href = url;
        } else {
            ageMsg.Show(errMsg);
        }
    },

    NextInfo: function (roleId) {
        var url = "";
        var folder = "";

        switch (roleId) {
            case 2:
                url = "../../SearchAndCopy/Agent/InfoMember.aspx";
                break;
            case 3:
                url = "../../SearchAndCopy/Master/InfoAgent.aspx";
                break;
            case 4:
                url = "../../SearchAndCopy/Super/MasterInfo.aspx";
                break;
            default:
                break;
        }

        url = SetParameterValue("userName", _page.userName, url);
        url = SetParameterValue("custId", _page.custId, url);
        url = SetParameterValue("isSkip", '0', url);
        location.href = url;
    },

    SportBookActive: function () {
        var url = location.href;
        url = SetParameterValue("active", 0, url);
        location.href = url;
    },

    RacingActive: function () {
        var url = location.href;
        url = SetParameterValue("active", 1, url);
        location.href = url;
    },

    CasinoActive: function () {
        var url = location.href;
        url = SetParameterValue("active", 2, url);
        location.href = url;
    },

    LiveCasinoActive: function () {
        var url = location.href;
        url = SetParameterValue("active", 3, url);
        location.href = url;
    },

    VirtualSportsActive: function () {
        var url = location.href;
        url = SetParameterValue("active", 4, url);
        location.href = url;
    },

    CheckPass: function (pwdElement, iconElementId) {
        var isPwd = IsPassword(pwdElement.value);
        var iconElement = $(iconElementId);

        if (!iconElement) return false;

        if (isPwd) {
            iconElement.className = 'PwdSuccess';
        } else {
            iconElement.className = 'PwdError';
        }

        return isPwd;
    },

    CheckDecNumber: function (e) {
        var code = (e.which) ? e.which : e.keyCode;
        if ((code < 48 || code > 57) && code != 8 && code != 13 && code != 0 && code != 46 && (code < 37 || code > 40)) {
            return false;
        }

        return true;
    },

    CheckNull: function (elementId) {
        var eId = $(elementId);
        if (!eId) return false;

        if (eId.value == '') {
            eId.value = "0";
        }
    },

    ChangeNumeric: function (value) {
        return parseFloat(SearchAndCopy.ReplaceAll(value, ',', ''));
    },

    ReplaceAll: function (Source, stringToFind, stringToReplace) {
        var temp = Source;
        var index = temp.indexOf(stringToFind);

        while (index != -1) {
            temp = temp.replace(stringToFind, stringToReplace);

            index = temp.indexOf(stringToFind);
        }

        return temp;
    },
    isNumeric: function (x) {
        var RegExp = /^(-)?(\d*)(\.?)(\d*)$/;
        return x.match(RegExp);
    },

    GetFolder: function (roleId) {
        var folder = "";
        switch (roleId) {
            case 2:
                folder = "Agent";
                break;
            case 3:
                folder = "Master";
                break;
            case 4:
                folder = "Super";
                break;
            default:
                break;
        }
        return folder;
    },

    AutoInsertMember: function (errCode, errMsg, custId) {
        var paramsAutoInsert = ajax.CreateParams();
        paramsAutoInsert += 'custid=' + errMsg;
        paramsAutoInsert += '&custIdOld=' + custId;

        // Special code for Bingo
        ajax.PostJSON('../../../_Bingo/Handlers/AutoInsertAccount.ashx?custid=' + errMsg, '', function (result) {
            // Register auto sync request to top window
            top.RegisterAsyncRequest('_MemberInfo/SearchAndCopy/Handlers/Agent/AutoInsertAccountBingoFromExistingMember.ashx', paramsAutoInsert + String.Empty, 'POST', 10000);            
        }, false);

        // For Racing
        ajax.PostJSON('../../../_MemberInfo/SearchAndCopy/Handlers/Agent/AutoInsertAccountRacingFromExistingMember.ashx', paramsAutoInsert, function (result) {
            if (result.errCode == 0) { } else {
                var errMsg = result.errMsg;
            }
        }, false
        // Synchronous
        );

        // For Casino
        ajax.PostJSON('../../../_MemberInfo/SearchAndCopy/Handlers/Agent/AutoInsertAccountCSFromExistingMember.ashx', paramsAutoInsert, function (result) {
            if (result.errCode == 0) { } else {
                var errMsg = result.errMsg;
            }
        }, false
        // Synchronous
        );

        // For Live Casino
        ajax.PostJSON('../../../_MemberInfo/SearchAndCopy/Handlers/Agent/AutoInsertAccountLiveCSFromExistingMember.ashx', paramsAutoInsert, function (result) {
            if (result.errCode == 0) { } else {
                var errMsg = result.errMsg;
            }
        }, false
        // Synchronous
        );

        // For P2P
        ajax.PostJSON('../../../_MemberInfo/SearchAndCopy/Handlers/Agent/AutoInsertAccountP2PFromExistingMember.ashx', paramsAutoInsert, function (result) {
            if (result.errCode == 0) { } else {
                var errMsg = result.errMsg;
            }
        }, false
        // Synchronous
        );

        // For Virtual Sport
        ajax.PostJSON('../../../_MemberInfo/SearchAndCopy/Handlers/Agent/AutoInsertAccountVirtualSportsFromExistingMember.ashx', paramsAutoInsert, function (result) {
            if (result.errCode == 0) { } else {
                var errMsg = result.errMsg;
            }
        }, false
        // Synchronous
        );

        var url = "../../../_MemberInfo/CustomerList/Agent/MemberList.aspx";
        parent.location.href = url;
        age.HideMaskDiv();
    },

    AutoInsert: function (errCode, errMsg, custId, roleID) {
        var paramsAutoInsert = ajax.CreateParams();
        paramsAutoInsert += 'custid=' + errMsg;
        paramsAutoInsert += '&custIdOld=' + custId;

        // For Casino
        ajax.PostJSON('../../../_MemberInfo/SearchAndCopy/Handlers/AutoInsertAccountCSFromExisting.ashx', paramsAutoInsert, function (result) {
            if (result.errCode == 0) { } else {
                var errMsg = result.errMsg;
            }
        }, false
        // synchronous
        );

        // For Live Casino
        ajax.PostJSON('../../../_MemberInfo/SearchAndCopy/Handlers/AutoInsertAccountLiveCSFromExisting.ashx', paramsAutoInsert, function (result) {
            if (result.errCode == 0) { } else {
                var errMsg = result.errMsg;
            }
        }, false
        // synchronous
        );

        // For Bingo
        ajax.PostJSON('../../../_MemberInfo/SearchAndCopy/Handlers/AutoInsertAccountBingoFromExisting.ashx', paramsAutoInsert, function (result) {
            if (result.errCode == 0) { } else {
                var errMsg = result.errMsg;
            }
        }, false
        // synchronous
        );

        var url = "../../../_MemberInfo/CustomerList/Super/MasterList.aspx";
        switch (roleID) {
            case 4:
                // Super
                url = "../../../_MemberInfo/CustomerList/Super/MasterList.aspx";
                break;
            case 3:
                // Master
                url = "../../../_MemberInfo/CustomerList/Master/AgentList.aspx";
                break;
            case 2:
                // Agent
                url = "../../../_MemberInfo/CustomerList/Agent/MemberList.aspx";
                break;
        }
        // age.HideMaskDiv();
        parent.location.href = url;
    },

    InitTagSuggestion: function () {
        initAutoComplete('txtUserName', '../../_GlobalResources/Handlers/QueryUserName.ashx?custid=' + _page.custid + '&isdir=1');
    },

    // Check available username
    CheckUser: function (selectId, select, select2, select3, msgId) {
        if ($(selectId)) {
            $(selectId).onchange = function () {
                $(msgId).className = 'loading2';

                var user = _page.user;

                if (select && select2 && select3) {
                    user += $(select).value + $(select2).value + $(select3).value;
                } else if (select && select2) {
                    user += $(select).value + $(select2).value;
                }

                ajax.PostJSON('../../SearchAndCopy/Handlers/CheckUser.ashx?username=' + user, '',

                function (result) {
                    if (result.errCode == 101) {
                        $(msgId).className = 'successWarning2';
                    } else {
                        $(msgId).className = 'errorWarning2';
                    }
                });
            }
        }
    }
}

RegisterStartUp(SearchAndCopy.InitSearchAndCopy);