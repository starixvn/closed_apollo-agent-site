<%@ Page Language="C#" EnableSessionState="False" AutoEventWireup="false"
	CodeFile="Error.aspx.cs" Inherits="Error" %>

<!DOCTYPE HTML>
<html>
<head>
	<title>Error</title>
	<style type="text/css">
		body {
		    margin: 0;
		    padding: 0;
		    font-family: Arial, Tahoma, Sans-Serif;
		    color: #333;
		}
		.err-block {
			background: url(<%= Page.ResolveUrl("_GlobalResources/Images/error2.png") %>) no-repeat center;
			width: 800px;
			margin: 100px auto 0;
			padding-top: 180px;
		}
	</style>
</head>
<body>
	<div class="err-block">
		<h3><%= ErrorMessage %></h3>
	</div>
</body>
</html>