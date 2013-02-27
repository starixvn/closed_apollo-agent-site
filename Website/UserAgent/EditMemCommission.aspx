<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditMemCommission.aspx.cs" Inherits="EditMemCommission" %>
<%@ OutputCache Location="None" %>
<script type="text/javascript">

function checkAndSubmitCustInfo()
{
    showHideLoading(1);
}

function showHideLoading(isShow)
{
    if(isShow == 1) DelayReloadPage(null, 300000);
    else HideLoadingDiv();
}

function showInfoMsg(msg, msgType) //msgType=0: error message, =1: info message
{
    showHideLoading(0);
    showErrMsg(msg, msgType);
    $('txtDiscount').focus();
}

</script>