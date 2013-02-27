window.errmsg = new Array();
var custIds = [];

function CheckAll() {
    var arrChkItem = document.getElementsByName('chkitem');
    var arrCheckAll = document.getElementsByName('chkall');

    if (!arrChkItem) return;
    for(var i=0;i<arrChkItem.length;i++)
    {
        var arrAmt = arrChkItem[i].value.split('_');
        if(arrAmt[0] != 0)
        {
            arrChkItem[i].checked = arrCheckAll[0].checked;
        }
    }
    if(arrCheckAll[0].checked)
    {
        $("bttTransfer").className = 'btnSubmit';
    }
    else
    {
        $("bttTransfer").className = 'btnSubmitDisable';
    }
}

function chkChecked()
{
   var arrChkItem = document.getElementsByName('chkitem');
   var arrCheckAll = document.getElementsByName('chkall');
   var count = 0;
   if (!arrChkItem) return;

   for (count; count < arrChkItem.length; count++) {
        if (arrChkItem[count].checked) {
            $("bttTransfer").className = 'btnSubmit';
            break;
        }
    }
    if(count == arrChkItem.length)
    {
        $("bttTransfer").className = 'btnSubmitDisable';
    }
   for (var i = 0; i < arrChkItem.length; i++) {
        if (!arrChkItem[i].checked) {
            arrCheckAll[0].checked = false;
            return;
        }
    }

    arrCheckAll[0].checked = true;
}

function OnkeyUpAmt(e)
{
    var isok=true;
    if (!e) e =window.event;
	var key =(e.keyCode) ? e.keyCode : e.which;

	if ((key < 48 || key > 57) && key != 8 && key != 13 && key != 45 && key != 46 && key != 0 && (key < 37 || key > 40))
    {
       return false;
    }
    else
    {
        if(key==13)
        {
            UpdYesBalance();
            return false;
        }
    }
 }

function InitTransfer() {
    if($('txtUserName') != null)
    {
        initAutoComplete('txtUserName', '../../_GlobalResources/Handlers/QueryUserName.ashx?custid=' + _page.custid + '&isdir=1');
    }

    if($('bttTransfer') != null)
    {
        $("bttTransfer").className = 'btnSubmitDisable';
    }
    if($('chkall') != null && $('chkitem') != null)
    {
        statusChkAll();
    }
    if(top.errCodes.length)
    {
        ageMsg.Show(top.errCodes.pop());
    }

    if (parent.ageWnd) { // In pop up
    }
    else {
        // Collapse layout
        document.body.style.backgroundImage = 'url("../../_GlobalResources/Images/bg_conts.jpg")';
    }
}

function GetTransferList(url) {
    var txtname = $('txtUserName').value;
    if (txtname == _page.userNameDefault)
    {
        txtname = '';
    }
    url = SetParameterValue("custid", _page.custid, url);
    url = SetParameterValue("username", txtname, url);
    url = SetParameterValue("status", $('statusFilter').value, url);
    url = SetParameterValue("allYesBal", $('chkYesterdayBalance').checked ? 'yes': 'no', url);
    url = SetParameterValue("pageSize", $('sel_PagingTop').value, url);

    location.href = url;
}

function CheckingFilter() {
    var url = location.href;
    var txtname = $('txtUserName').value;
    if (txtname == _page.userNameDefault)
    {
        txtname = '';
    }
    url = SetParameterValue("custid", _page.custid, url);
    url = SetParameterValue("username", txtname, url);
    url = SetParameterValue("status", $('statusFilter').value, url);
    url = SetParameterValue("allYesBal", $('chkYesterdayBalance').checked ? 'yes': 'no', url);
    url = SetParameterValue("pageSize", $('sel_PagingTop').value, url);

    location.href = url;
}
function statusChkAll()
{
    var arrChkItem = document.getElementsByName('chkitem');
    var arrCheckAll = document.getElementsByName('chkall');

    var count = 0;
    if (arrChkItem.length == 0)
    {
        arrCheckAll[0].disabled = true;
        return;
    }

    for(var i=0;i<arrChkItem.length;i++)
    {
        if(arrChkItem[i].disabled)
        {
            count += 1;
        }
    }

    if(count !=0 && count == arrChkItem.length)
    {
        arrCheckAll[0].disabled = true;
    }
}

function ShowPopupTransfer(url, amt, custid) {
    var popH = 120, popW = 400;
    url += '?amt=' + amt;
    url += '&custid=' + custid;
    //url += '&usn=' + username;
    url += '&status=' + $('statusFilter').value;
    //url += "&CallBack=parent.OnPopupComplete(true)";
    ageWnd.open(url, '', 350, 100, popW, popH);
}

function UpdYesBalance() {
    var params = ajax.CreateParams();
    arrAmount = document.getElementsByName('txtamount');

    if (!isNumeric(arrAmount[0].value)) {
        ageMsg.Show(_page.invalidValue);
        AddPopupHeight(50);
        return false;
    }
    params += '&custid=' + _page.custid;
    params += '&isclose=' + _page.isclose;
    //params += '&oldamount=' + _page.amt;
    params += '&txtamount=' + arrAmount[0].value;
    function OnComplete(result)
    {
        age.UnLock();
        var errCode = result.errCode;
        if (errCode == 0)
        {
            if (_page.CallBack != 'false') {
                eval(_page.CallBack);
            }
            else {
                //ageMsg.Show(_page.wrnTransferSuccessful, true);
                alert(_page.wrnTransferSuccessful);
                age.DelayReloadPage();
                top.menu.checkAndSubmitUI();
            }
        }
        else
        {
            ageMsg.Show(result.errMsg);
            AddPopupHeight(50);
        }
    }

    // Post with return result in JSON format
    age.Lock(false);
    if (confirm(_page.wrnTransfer))
    {
        ajax.PostJSON(
            '../../_Transfer/Handler/SingleTransfer.ashx',
            params,
            OnComplete
        );
    }

    age.UnLock();
}

function OnPopupComplete(reload)
{
    ageWnd.close(); // Close popup
    alert(_page.wrnTransferSuccessful);

    age.DelayReloadPage(); // Reload page after 2s
}

function TransferMulti()
{
    var warning = "";
    var checkID = document.getElementsByName('chkitem');
    var custIdList = "";
    var custidSync = "";
    var counter = 0;
    var amt = "";
    var custid = "";

    if (!checkID) return;

    for (var i = 0; i < checkID.length; i++) {
        if (checkID[i].checked == true) {
            counter++;

            var arrPara = checkID[i].value.split('_');

            if(counter == 1) {
                amt = arrPara[0];
                custid = arrPara[1];
            }
            if (counter >= 400) {
                custIdList += arrPara[1] + "_";
                counter = 0;
            }
            else {
                custIdList += arrPara[1] + ",";
            }
        }
    }

    warning = _page.wrnTransfer;

    if (custIdList.length > 0) {
        custIdList = custIdList.substring(0, custIdList.length - 1);
        var _arr = custIdList.split('_');
        for (var i = 0; i < _arr.length; i++) {
            custIds.push(_arr[i]);
        }
        var params = ajax.CreateParams();

        function OnComplete(result)
        {
            age.UnLock();
            var errCode = result.errCode;
            if (errCode == 0)
            {
                if(_page.CallBack)
                {
                    eval(_page.CallBack);
                }
            }
            else if (errCode == '106') {
                showErrMsg(_page.lblconfirmclosesubacc, 0);
            }
            else if (errCode == '105') // kick out
            {
                alert(_page.lblConfirmClosed);
                location.href = '../Authorization/Logout.ashx';
            }
            else
            {
                ageMsg.Show(result.errMsg);
            }
        }

        // Post with return result in JSON format
        if (confirm(warning)) {
            age.DelayReloadPage(null, 1000000);
            if(counter == 1)
            {
                params += '&custid=' + custid;
                params += '&isclose=' + _page.status;
                params += '&oldamount=' + amt;
                params += '&txtamount=' + amt;
                ajax.PostJSON(
                    '../../_Transfer/Handler/SingleTransfer.ashx',
                    params,
                    OnComplete
                );
            }
            else
            {
                OnUpdateYesterdayBalance(_page.userName, _page.status, _page.isYesterdayBalance, _page.pageSize, _page.totalPage);
            }
         }
        custIdList = '';
        custidSync = '';
        $("chkall").checked = false;
        for (var i = 0; i < checkID.length; i++) {
            if (checkID[i].checked == true) {
                checkID[i].checked = false;
            }
        }
    }
}

function UpdateYesterdayBalance(custid, userName, status, isYesterdayBalance, pageSize, totalPage) {
    // Post with return result in JSON format
    var params = ajax.CreateParams();
    params += '&ischeck=false';
    params += '&custid=' + custid;
    params += '&status=' + status;
    params += '&totalPage=' + totalPage;
    params += '&pagesize=' + pageSize;
    params += '&username=' + userName;
    params += '&isYesterdayBalance=' + isYesterdayBalance;
    ajax.PostJSON(
                    '../../_Transfer/Handler/MultipleTransfer.ashx',
                    params,
                    OnComplete
                );

    function OnComplete(result) {
        age.UnLock();
        var errCode = result.errCode;
        if (errCode == 0) {
            if (_page.CallBack) {
                eval(_page.CallBack);
            }
        }
        else if (errCode == '106') {
            showErrMsg(_page.lblconfirmclosesubacc, 0);
        }
        else if (errCode == '105') // kick out
        {
            alert(_page.lblConfirmClosed);
            location.href = '../Authorization/Logout.ashx';
        }
        else {
            ageMsg.Show(result.errMsg);
            AddPopupHeight(50);
        }

        OnUpdateYesterdayBalance(userName, status, isYesterdayBalance, pageSize, totalPage);
    }
}

function OnUpdateYesterdayBalance(userName, status, isYesterdayBalance, pageSize, totalPage) {
    if (custIds.length > 0) {
        var custid = custIds.pop();
        UpdateYesterdayBalance(custid, userName, status, isYesterdayBalance, pageSize, totalPage);
    }
}

function TransferAll()
{
    var warning = "";
    var checkID = document.getElementsByName('chkitem');
    var counter = 0;
    if (!checkID) return;

    var txtname = $('txtUserName').value;
    if (txtname == _page.userNameDefault)
    {
        txtname = '';
    }
    var params = ajax.CreateParams();

    params += '&ischeck=true';
    params += '&custid=0';
    params += '&status=' + _page.status;
    params += '&totalPage=' + _page.totalPage;
    params += '&pagesize=' + _page.pageSize;
    params += '&username=' + txtname;
    params += '&isYesterdayBalance=' + _page.isYesterdayBalance;

    function OnComplete(result)
    {
        var errCode = result.errCode;
        if (errCode == 0)
        {
            if(_page.CallBack)
            {
                eval(_page.CallBack);
            }
        }
        else if (errCode == '106') {
            showErrMsg(_page.lblconfirmclosesubacc, 0);
        }
        else if (errCode == '105') // kick out
        {
            alert(_page.lblConfirmClosed);
            location.href = '../Authorization/Logout.ashx';
        }
        else
        {
            ageMsg.Show(result.errMsg);
        }
    }

    warning = _page.wrnTransfer;
    // Post with return result in JSON format
    if (confirm(warning)) {
        age.DelayReloadPage(null, 1000000);
                ajax.PostJSON(
                    '../../_Transfer/Handler/MultipleTransfer.ashx',
                    params,
                    OnComplete
                );
     }

    $("chkall").checked = false;
    for (var i = 0; i < checkID.length; i++) {
        if (checkID[i].checked == true) {
            checkID[i].checked = false;
        }
    }
}

function OnTransferComplete()
{
    alert(_page.wrnTransferSuccessful);
    age.DelayReloadPage();
    //age.DelayReloadPage(null, 4000);
}

function ShowFrmTransferMulti(el, divID ) {
    var div = $(divID);
    if (!div) return false;

    if(!EnableBttTransfer())
    {
        return false;
    }
    el.style.position = 'relative';
    var pos = GetPosition(el);
    div.style.top = (pos.offsetTop + el.offsetHeight) + 'px';
    div.style.left = (pos.offsetLeft ) + 'px';

    div.style.display = 'block';
    clearTimeout(div.timerID); div.timerID = 0;
    div.timerID = setTimeout("HidePopupGeneral('" + divID + "');", 2000);
    age.addEvent(div, "mouseover", function() { clearTimeout(div.timerID); div.timerID = 0; }, true);

    age.addEvent(div, "mouseout", function() { clearTimeout(div.timerID); div.timerID = 0; div.timerID = setTimeout("HidePopupGeneral('" + divID + "');", 100); }, true);
}

function EnableBttTransfer() {
    var arrChkItem = document.getElementsByName('chkitem');
    var arrCheckAll = document.getElementsByName('chkall');
    IsOK = false;
    if (!arrChkItem) return false;

    if (arrCheckAll[0].checked && _page.totalPage > 1)
    {
        var urlTranfer = 'TransferAll()';
        SetAttributeURL('transferAll', urlTranfer);
        ChangedIcon('transferAll', '1');
    }
    else
    {
        SetAttributeURL('transferAll', '');
        ChangedIcon('transferAll', '2');
    }
    for(var i=0;i<arrChkItem.length;i++)
    {
        var arrAmt = arrChkItem[i].value.split('_');
        if(arrAmt[0] != 0)
        {
            if(arrChkItem[i].checked)
            {
                var urlTranfer = 'TransferMulti()';
                SetAttributeURL('transfer', urlTranfer);
                IsOK = true;
                ChangedIcon('transfer', '1');
                break;
            }
        }
    }
    if(!arrCheckAll[0].checked && !IsOK)
    {
        return false;
    }
    if(!IsOK)
    {
        SetAttributeURL('transfer', '');
        ChangedIcon('transfer', '2');
    }
    return true;
}
function GetPosition(element) {
    var left = 0;
    var top = 0;
    if (element.offsetParent) {
        while (element) {
            left += element.offsetLeft;
            top += element.offsetTop;
            element = element.offsetParent;
        }
    }
    return {
        offsetLeft: left,
        offsetTop: top
    };
}

function HidePopupGeneral(divID) {
    var div = $(divID);
    if (!div) return false;
    div.style.display = 'none';
}

function SetAttributeURL(Id, url) {
   var elm = $(Id);
   if(elm)
        elm.href = 'javascript:' + url;
}

function ChangedIcon(Id, Icon) {
    var className = "LinkPopup";
    switch (Icon) {
        case "1":
            className = "Enable";
            break;
        case "2":
            className = "Disable";
            break;
    }
    $(Id).className = className;
}

function getPrint(print_area) {
    var printContent = $(print_area);
    var printWindow = window.open('', '', 'left=500,top=400,width=200,height=5');
    printWindow.document.write("<html>");
    printWindow.document.write("<head>");
    printWindow.document.write("</head>");
    printWindow.document.write("<body style='margin-top: 100px'>");
    printWindow.document.write(printContent.innerHTML);
    printWindow.document.write("</body></html>");
    printWindow.document.close();
    printWindow.focus();
    printWindow.print();
    printWindow.close();
}

function showErrMsg(msg, type) {
    if (typeof type == 'undefined') type = 0;
    var objMsgDiv = $('diverrmsg');
    var objMsgText = $('spmsgerr');

    if (objMsgDiv == null) { //is Enrich Msg
        objMsgDiv = $('diverr');
        objMsgText = $('err');
        objMsgText.className = 'msgerr';
    }
    objMsgDiv.style.display = window.attachEvent ? 'block' : 'table';
    objMsgText.innerHTML = msg;
    objMsgText.className = (type == 1) ? 'msgscc' : 'msgerr';
    var cname = (type == 1) ? "succ" : "emsg";
    $('1_1').className = cname + "_1_1";
    $('1_2').className = cname + "_1_2";
    $('1_3').className = cname + "_1_3";
    $('2_1').className = cname + "_2_1";
    $('2_2').className = cname + "_2_2";
    $('3_1').className = cname + "_3_1";
    $('3_2').className = cname + "_3_2";
    $('3_3').className = cname + "_3_3";
    HideLoadingDiv();
}

RegisterStartUp(InitTransfer);

function isNumeric(x) {
    var RegExp = /^(-)?(\d*)(\.?)(\d*)$/; // Note: this WILL allow a number that ends in a decimal: -452.
    return x.match(RegExp);
}