<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AgentWinLoss.aspx.cs" Inherits="_Reports_Winloss_AgentWinLoss" %>

<!DOCTYPE HTML>

<html>
<head>
    <title></title>
    <link href="../../_GlobalResources/Css/Agent.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Reports.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="frmmain" method="get" action="">
    <div id="page_main">
        <div id="header_main" style="width:740px;">
            <span id="test" style="float:left"> <%= Resources.Lang.agentwinloss %> </span> 
        </div>

        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td><%= Resources.Lang.from %></td>
                <td> <input type="text"/> </td>

                <td> <%= Resources.Lang.to %> </td>
                <td>
                <% Fanex.Age.UI.Components.DateTimeControl.Calendar.Calendar_Control CalendarFrom = new Fanex.Age.UI.Components.DateTimeControl.Calendar.Calendar_Control(this,"fdate","fdate_trigger",DateTime.Today) %> 
                <%--<input type="text" /> --%>
                </td>
                <td>
                    <input type="submit" value="Submit">
                    <input type="submit" value="Today">
                    <input type="submit" value="Yesterday">
                </td>
            </tr>
            <tr>
            
            </tr>
        </table>

    </div>
    </form>
</body>
</html>
<script src="../../_GlobalResources/Js/Core.js" type="text/javascript"></script>
<script src="../Resources/Reports.js" type="text/javascript"></script>
