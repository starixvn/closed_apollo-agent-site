<%@ Page Language="C#" EnableSessionState="ReadOnly" EnableViewState="False" MasterPageFile="~/_ViewLog/ViewLog.master" AutoEventWireup="false" CodeFile="LogCredit.aspx.cs" Inherits="Sunplus.Agent.UI._ViewLog.LogCredit_Page" Title="View Log Credit" %>

<%@ Import Namespace="Sunplus.Agent.UI._ViewLog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentData" runat="Server">
    <div id="content">
        <table width="100%" cellpadding="0" cellspacing="0" id="DataLog">
            <tr class="title">
                <th>
                    <asp:Literal ID="Literal1" Text="<%$ Resources:ViewLog, logid %>" EnableViewState="false" runat="server"></asp:Literal>
                </th>
                <th>
                    <asp:Literal ID="Literal2" Text="<%$ Resources:ViewLog, logdate %>" EnableViewState="false" runat="server"></asp:Literal>
                </th>
                <th>
                    <asp:Literal ID="Literal3" Text="<%$ Resources:ViewLog, usernameEdited %>" EnableViewState="false" runat="server"></asp:Literal>
                </th>
                <th>
                    <asp:Literal ID="Literal4" Text="<%$ Resources:ViewLog, actionname %>" EnableViewState="false" runat="server"></asp:Literal>
                </th>
                <th>
                    <asp:Literal ID="Literal7" Text="<%$ Resources:ViewLog, OldValue %>" EnableViewState="false" runat="server"></asp:Literal>
                </th>
                <th>
                    <asp:Literal ID="Literal8" Text="<%$ Resources:ViewLog, NewValue %>" EnableViewState="false" runat="server"></asp:Literal>
                </th>
                <th>
                    <asp:Literal ID="Literal5" Text="<%$ Resources:ViewLog, actionDesc %>" EnableViewState="false" runat="server"></asp:Literal>
                </th>
                <th>
                    <asp:Literal ID="Literal6" Text="<%$ Resources:ViewLog, ip %>" EnableViewState="false" runat="server"></asp:Literal>
                </th>
            </tr>
            <asp:Repeater ID="rptDataLog" runat="server" EnableViewState="false">
                <ItemTemplate>
                    <tr class="even">
                        <td>
                            <%#Eval("LogID")%>
                        </td>
                        <td class="datetimeInlinde">
                            <%#Eval("LogDate")%>
                        </td>
                        <td class="EditedBy">
                            <%#Eval("UserName")%>
                        </td>
                        <td class="ActionName">
                            <%#Eval("ActionName")%>
                        </td>
                        <td class="Value">
                            <%#Eval("OldValue", "{0:#,##0.00}")%>
                        </td>
                        <td class="Value">
                            <%#Eval("NewValue", "{0:#,##0.00}")%>
                        </td>
                        <td class="Doing">
                            <%#Eval("Doing")%>
                        </td>
                        <td class="Ip">
                            <%#XsltLogFormat.FormatIp(Eval("IP") as string)%>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="odd">
                        <td>
                            <%#Eval("LogID")%>
                        </td>
                        <td class="datetimeInlinde">
                            <%#Eval("LogDate")%>
                        </td>
                        <td class="EditedBy">
                            <%#Eval("UserName")%>
                        </td>
                        <td class="ActionName">
                            <%#Eval("ActionName")%>
                        </td>
                        <td class="Value">
                            <%#Eval("OldValue", "{0:#,##0.00}")%>
                        </td>
                        <td class="Value">
                            <%#Eval("NewValue", "{0:#,##0.00}")%>
                        </td>
                        <td class="Doing">
                            <%#Eval("Doing")%>
                        </td>
                        <td class="Ip">
                            <%#XsltLogFormat.FormatIp(Eval("IP") as string)%>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:Repeater>
            <tr class="noInfo" id="ltrNoInfo" runat="server">
                <td colspan="8">
                    <%= Resources.Lang.noInfo %>
                </td>
            </tr>
        </table>
    </div>
    <%--pager--%>
    <%=Pager%>
    <%--/pager--%>
</asp:Content>
