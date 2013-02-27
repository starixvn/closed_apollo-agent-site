<%@ Page Language="C#" EnableSessionState="ReadOnly" EnableViewState="False" MasterPageFile="~/_ViewLog/ViewLog.master" AutoEventWireup="false" CodeFile="LogCustomerSetting.aspx.cs" Inherits="Sunplus.Agent.UI._ViewLog.LogCustomerSetting_Page" Title="View Log Customer Setting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentData" runat="Server">
    <div id="content">
        <table width="100%" cellpadding="0" cellspacing="0" id="DataLog">
            <tr>
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
                    <asp:Literal ID="Literal5" Text="<%$ Resources:ViewLog, actionDesc %>" EnableViewState="false" runat="server"></asp:Literal>
                </th>
            </tr>
            <asp:Repeater ID="rptDataLog" runat="server" EnableViewState="false">
                <ItemTemplate>
                    <tr>
                        <td class="LogId">
                            <%#Eval("LogID")%>
                        </td>
                        <td class="datetime">
                            <%#Eval("LogDate", "{0:d} <br/> {0:hh:mm:ss tt}")%>
                        </td>
                        <td class="EditedBy">
                            <%#Eval("UserName")%>
                        </td>
                        <td class="ActionName">
                            <%#Eval("ActionName")%>
                        </td>
                        <td class="TrackingValue">
                            <%#GetContentHtml(Eval("TrackingValue"), Eval("ActionID"), Eval("LogID"))%>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr>
                        <td class="LogId">
                            <%#Eval("LogID")%>
                        </td>
                        <td class="datetime">
                            <%#Eval("LogDate", "{0:d} <br/> {0:hh:mm:ss tt}")%>
                        </td>
                        <td class="EditedBy">
                            <%#Eval("UserName")%>
                        </td>
                        <td class="ActionName">
                            <%#Eval("ActionName")%>
                        </td>
                        <td class="TrackingValue">
                            <%#GetContentHtml(Eval("TrackingValue"), Eval("ActionID"), Eval("LogID"))%>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:Repeater>
            <tr class="noInfo" id="ltrNoInfo" runat="server">
                <td colspan="8">
                    <%=GetStringFromGlobalResource("noInfo")%>
                </td>
            </tr>
        </table>
    </div>
    <%--pager--%>
    <%=Pager%>
    <%--/pager--%>
</asp:Content>
