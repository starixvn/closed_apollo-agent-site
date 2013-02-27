<%@ Page EnableViewState="false" Language="C#" AutoEventWireup="true" CodeFile="Sitemap.aspx.cs" Inherits="Sunplus.Agent.UI.Navigation.Sitemap_Page" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sitemap</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:TreeView ID="TreeView1" runat="server" OnTreeNodeDataBound="TreeView1_TreeNodeDataBound" ImageSet="Arrows">
        <ParentNodeStyle Font-Bold="False" />
        <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
        <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px" VerticalPadding="0px" />
        <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
    </asp:TreeView>
    </form>
</body>
</html>
