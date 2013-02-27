<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RedirectPT.aspx.cs" Inherits="MemberInfo_RedirectPT" %>
<%@ OutputCache Location="None"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
    <frameset rows="30,*"  border="0" framespacing="0" frameborder="no">
        <frame src="<%=UrlMenu%>" id="frmHeader" name="frmHeader" scrolling="auto" noresize="noresize" frameborder="0"/>
        <frame src="<%=UrlMainPage%>" id="frmmain" name="frmmain" scrolling="auto" noresize="noresize" frameborder="0" />   
    </frameset>
    <noframe>
    </noframe>
</html>
