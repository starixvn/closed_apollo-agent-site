<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TabPT.aspx.cs" Inherits="MemberInfo_TabPT" %>

<%@ OutputCache Location="None" %>
<script type="text/javascript">

    var isSettingMinPT = "<%= isSettingMinPT%>";
    if (isSettingMinPT == "True") {
        //Create ToolTip remain setting Sportbook and Racing Min PT
        var strTooltip = "<div class='minptleft'></div><div class='minptbg'><div class='text'><%= Resources.Lang.wannaSetMinPT %></div><div class='minptclose' onclick='RemoveDiv(\"toolTipSettingMinPt\");'></div></div><div class='minptright'></div><div class='minptarrow arrow1'></div><div class='minptarrow arrow2'></div>";
        parent.parent.parent.CreateDiv("toolTipSettingMinPt", strTooltip, "toolTipSettingMinPT", "225px", "18px", "")
        setTimeout("CloseToolTipSettingMinPT();", 15000);

        //Remove Tooltip Setting MinPT when click different the pages.
        window.onbeforeunload = function () {
            CloseToolTipSettingMinPT();
        }
        function CloseToolTipSettingMinPT() {
            parent.parent.parent.RemoveDiv("toolTipSettingMinPt");
        }
        //End
    }
    function ActiveMenuTab(url, id, idlength, isedit) {

        for (var i = 0; i < idlength; i++) {
            if ($(id).id == "IdCurr" + i) {
                if ($("IdCurr" + i))
                    $("IdCurr" + i).className = "current";

            }
            else {
                if ($("IdCurr" + i))
                    $("IdCurr" + i).className = "";
            }
        }
        parent.frmmain.location.href = url + "&isedit=" + $("IdIsEdit" + isedit).value;
    }

    // Call _Casino/Handlers/AutoInsertAccount.ashx
    ajax.PostJSON(
        '../_Casino/Handlers/AutoInsertAccount.ashx',
        'custid=' + GetParameterValue('custid', location.href),
        function (result) {
            if (result.errCode != 0) {

            }
        }
        );

    // Call _Bingo/Handlers/AutoInsertAccount.ashx
    ajax.PostJSON(
        '../_Bingo/Handlers/AutoInsertAccount.ashx',
        'custid=' + GetParameterValue('custid', location.href),
        function (result) {
            if (result.errCode != 0) {

            }
        }
        );

    // Call _MemberInfo/LiveCasino/Handlers/AutoInsertAccount.ashx
    ajax.PostJSON(
    '../_MemberInfo/LiveCasino/Handlers/AutoInsertAccount.ashx',
    'custid=' + GetParameterValue('custid', location.href),
    function (result) {
        if (result.errCode != 0) {

        }
    }
    );
</script>