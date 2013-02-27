<%@ Page Language="C#" EnableSessionState="ReadOnly" AutoEventWireup="true" CodeFile="NewMessageForm.aspx.cs" Inherits="zTest_NewMessageForm" %>

<script type="text/javascript">
    var sobjDate=null;
    var eobjDate=null;
    var fromDateControlID = "fdate";
    var toDateControlID = "tdate";
    var agentNumber = <%=AgentNumber%>;
    
    function splitDate(){
	    var s_date=$(fromDateControlID);
	    var e_date=$(toDateControlID);
	    var s_dataAry = new Array(2);
	    var e_dataAry = new Array(2);
    	
	    var kk1=s_date.value.split("/");
	    for(var i=0;i<kk1.length ;i++){
		    s_dataAry[i]=kk1[i];
	    }
	    var kk2=e_date.value.split("/");
	    for(var i=0;i<kk2.length ;i++){
		    e_dataAry[i]=kk2[i];
	    }
    	
	    sobjDate = new Date(s_dataAry[2],s_dataAry[0],s_dataAry[1]);
	    eobjDate = new Date(e_dataAry[2],e_dataAry[0],e_dataAry[1]);
	}
	function showMsg(msgType) {
	    splitDate();
	    if (eobjDate < sobjDate) {
	        if (msgType != 'special' && msgType != 'allSpecial') {
	            alert($("eroMeg").value);
	            return;
	        }
	        else {
	            setDDate();
	        }
	    }

	    if (msgType == '') {
	        msgType = $("msgType").value;
	    }
	    var str1 = msgType;
	    var str2 = $(fromDateControlID).value;
	    var str3 = $(toDateControlID).value;
	    var extraParam = "";
	    // update count of private messages in header frame
	    if ((msgType == 'private') && (agentNumber > 0)) {
	        var open = "(", close = ")";
	        var countPriMsg = "0";

	        var countText = parent.frHeader.$('privateMsgLnk').innerHTML;
	        if (countText.indexOf(open) >= 0) {
	            var temp = countText.substring(0, countText.indexOf(open) + 1) + countPriMsg + ")";
	            var temp2 = countText.substring(countText.indexOf(open) + 1, countText.indexOf(close));
	            extraParam = (parseInt(temp2) > 0 ? "&extra=1" : "");
	            parent.frHeader.$('privateMsgLnk').innerHTML = temp;
	        }
	    }
	    
	    location.href = "NewMessageForm.aspx?pageQuery=" + str1 + "&s_date=" + str2 + "&e_date=" + str3 + extraParam;
	}
    function setDDate() {
        var timeNow = new Date();
        var year = timeNow.getFullYear();
        var mon = timeNow.getMonth() + 1;
        var ddate = timeNow.getDate();
        var yesterday = new Date(Date.parse(timeNow) - (3 * 1000 * 60 * 60 * 24));
        $(fromDateControlID).value = mon + "/" + yesterday.getDate() + "/" + year;
        $(toDateControlID).value = mon + "/" + ddate + "/" + year;
    }
    function enableCalendar(IsDisabled)
    {
        $(fromDateControlID + "_tg").disabled = IsDisabled;
        $(toDateControlID + "_tg").disabled = IsDisabled;
        $("dSubmit").disabled = IsDisabled;
    }
    function setStatusCalendar() {
        var spMes = $("msgType").value;
        if (spMes == 'special' || spMes == 'allSpecial') {
            enableCalendar(true);
        }
        else {
            enableCalendar(false);
        }
    }
</script>
