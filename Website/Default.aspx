<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>
<html lang="en-us">
<head>
	<title>Default Page</title>
	<meta charset="utf-8" />
	<meta name="viewport" content="width=device-width, initial-scale=1.0" />

	<script src="<%= Page.ResolveUrl("~/_GlobalResources/Js/head.min.js")%>" type="text/javascript"></script>
	<link href="<%= Page.ResolveUrl("~/_GlobalResources/Css/Index.less")%>" rel="stylesheet" type="text/css" />

	<script type="text/javascript">
		// For caching in child frames
		window.cachePage = new Array();
		window.cacheData = new Array();
	</script>
</head>
<body>
	
	<form id="agentSite" runat="server">
		<div class="header">
			<iframe src="<%= Page.ResolveUrl("~/Account/TopBar")%>" id="frHeader" name="frHeader"
				width="100%" height="100%" frameborder="0" scrolling="no"></iframe>
		</div>
		<div class="menu">
			<iframe src="<%= Page.ResolveUrl("~/Account/Menu")%>" id="menu" name="menu"
				width="100%" height="99%" frameborder="0" scrolling="no"></iframe>
		</div>
		<div class="mainpage">
			<iframe src="<%= Page.ResolveUrl("~/Message/Portal")%>" id="main" name="main"
				width="100%" height="99%" frameborder="0"></iframe>
		</div>
	</form>
	<input type="hidden" id="ApoRoot" value="<%= Page.ResolveUrl("~/")%>" />
	
	<script type="text/javascript">
		head.js(
			"<%= Page.ResolveUrl("_GlobalResources/Js/jquery.js")%>",
			"<%= Page.ResolveUrl("_GlobalResources/Js/Core.js")%>"
		);
	</script>

</body>
</html>