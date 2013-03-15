<%@ Page Language="C#" EnableSessionState="ReadOnly" EnableViewState="False" MasterPageFile="~/_ViewLog/ViewLog.master" AutoEventWireup="false" CodeFile="LogLogin.aspx.cs" Inherits="Sunplus.Agent.UI._ViewLog.LogLogin_Page" Title="ViewLogLogin" %>

<%@ Import Namespace="Sunplus.Agent.UI._ViewLog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentData" runat="Server">
    <div id="content">
        <table width="100%" cellpadding="0" cellspacing="0" id="DataLog">
            <tr class="title">
                <th>
                    <asp:Literal ID="Literal1" Text="<%$ Resources:ViewLog, logid %>" EnableViewState="false" runat="server" />
                </th>
                <th>
                    <asp:Literal ID="Literal2" Text="<%$ Resources:ViewLog, logdate %>" EnableViewState="false" runat="server" />
                </th>
                <th>
                    <asp:Literal ID="Literal3" Text="<%$ Resources:ViewLog, username %>" EnableViewState="false" runat="server" />
                </th>
                <th>
                    <asp:Literal ID="Literal4" Text="<%$ Resources:ViewLog, actionname %>" EnableViewState="false" runat="server" />
                </th>
                <th>
                    <asp:Literal ID="Literal5" Text="<%$ Resources:ViewLog, actionDesc %>" EnableViewState="false" runat="server" />
                </th>
                <th>
                    <asp:Literal ID="Literal6" Text="<%$ Resources:ViewLog, Domain %>" EnableViewState="false" runat="server" />
                </th>
                <th>
                    <asp:Literal ID="Literal7" Text="<%$ Resources:ViewLog, Browser %>" EnableViewState="false" runat="server" />
                </th>
                <th>
                    <asp:Literal ID="Literal9" Text="<%$ Resources:ViewLog, country %>" EnableViewState="false" runat="server" />
                </th>
                <th>
                    <asp:Literal ID="Literal10" Text="<%$ Resources:ViewLog, ip %>" EnableViewState="false" runat="server" />
                </th>
            </tr>
            <asp:Repeater ID="rptDataLog" runat="server" EnableViewState="false">
                <ItemTemplate>
                    <tr class="even">
                        <td>
                            <%#Eval("LogID")%>
                        </td>
                        <td class="datetimeInlinde">
                            <%#Eval("LogDate", "{0:d} <br/> {0:hh:mm:ss tt}")%>
                        </td>
                        <td class="EditedBy">
                            <%#Eval("UserName")%>
                        </td>
                        <td class="ActionName">
                            <%#Eval("ActionName")%>
                        </td>
                        <td class="actionDesc">
                            <%#Eval("Doing")%>
                        </td>
                        <td>
                            <%#Eval("Domain")%>
                        </td>
                        <td class="Browser">
                            <%#Eval("Browser")%>
                        </td>
                        <td>
                            <%#XsltLogFormat.FormatCountryCode(Eval("IP") as string, Eval("CountryCode") as string)%>
                        </td>
                        <td class="Ip">
                            <%# XsltLogFormat.FormatIp(Eval("IP") as string)%>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="odd">
                        <td>
                            <%#Eval("LogID")%>
                        </td>
                        <td class="datetimeInlinde">
                            <%#Eval("LogDate", "{0:d} <br/> {0:hh:mm:ss tt}")%>
                        </td>
                        <td class="EditedBy">
                            <%#Eval("UserName")%>
                        </td>
                        <td class="ActionName">
                            <%#Eval("ActionName")%>
                        </td>
                        <td class="actionDesc">
                            <%#Eval("Doing")%>
                        </td>
                        <td>
                            <%#Eval("Domain")%>
                        </td>
                        <td class="Browser">
                            <%#Eval("Browser")%>
                        </td>
                        <td>
                            <%#XsltLogFormat.FormatCountryCode(Eval("IP") as string, Eval("CountryCode") as string)%>
                        </td>
                        <td class="Ip">
                            <%# XsltLogFormat.FormatIp(Eval("IP") as string)%>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:Repeater>
            <tr class="noInfo" id="ltrNoInfo" runat="server">
                <td colspan="10">
                    <%=GetStringFromGlobalResource("noInfo")%>
                </td>
            </tr>
        </table>
    </div>
    <%--pager--%>
    <%=Pager%>
    <%--/pager--%>
</asp:Content>
