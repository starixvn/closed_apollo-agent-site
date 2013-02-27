<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditMemInfo.aspx.cs" Inherits="EditMemInfo" %>
<%@ OutputCache Location="None" %>
<script type="text/javascript">

function checkAndSubmitCustInfo()
{
    txtEmail = $('txtEmail');
    if (txtEmail.value != '' && !validateEmail(txtEmail.value) ) 
    {
        showErrMsg(emailnotvalid);
        txtEmail.focus();
        return false;
    }
    showHideLoading(1);
}

function showHideLoading(isShow) {
    if(isShow==1) DelayReloadPage(null, 300000);
    else HideLoadingDiv();       
}

function showInfoMsg(msg, type)
{
    showHideLoading(0);
    showErrMsg(msg, type);
    $('txtFName').focus();
}

</script>