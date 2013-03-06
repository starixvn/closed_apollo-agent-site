using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fanex.Age.UI;
using Fanex.Age.UI.Components;
using Fanex.Age.UI.Reports.BetList;
using Sunplus.Agent.Model.BL.Definitions;
using Sunplus.Agent.UI._BetList;
using Sunplus.Agent.UI.BetList;
using Sunplus.Agent.UI.BetList.Factories;
using Sunplus.Agent.Utilities;

/*
 * Created 20110308@Thai - Cancel BetList page
 * Revision:
 * Modified
 *
 */

namespace Fanex.Age.UI.Reports.BetList
{
    public partial class CancelBetList : TplPage
    {
        int masterId = 0;
        int agentId = 0;
        int memberId = 0;

        string option = string.Empty;
        string title = string.Empty;

        CancelBetList_Template tpl = new CancelBetList_Template();
        IBetListCmd command = null;

        bool exportExcel = false;

        public override void RenderContent(HtmlTextWriter writer)
        {
            Dictionary<string, DataTable> results = View as Dictionary<string, DataTable>;
            UserTypes userType = UserTypes.Unknown;

            DataTable dtDeletedBets = results["dtDeletedBets"];

            Show_HideComboBox(results, userType);
            DataTable betTransm = command.OutDataTable();

            //if (exportExcel)
            //{
            //    //var builder = new Fanex.Age.Excel.BetList.BetListReportBuilder(BetListType.Canceled, dtDeletedBets, userType, Resources.Lang.voidandreject.ToUpper(), betTransm);
            //    //var excelStream = builder.Build();
            //    //if (excelStream != null)
            //    //{
            //    //    ExportToExcel(excelStream, "CancelBetList.xls");
            //    //    excelStream.Dispose();
            //    //    return;
            //    //}

            //    //StringBuilder content = RenderingFactory.Render(BetListType.Canceled, ref dtDeletedBets, userType, false, betTransm);
            //    //ExportToExcel(Response, content.ToString(), "CancelBetList");
            //}
            //else
            //{
            StringBuilder content = RenderingFactory.Render(BetListType.Canceled, ref dtDeletedBets, userType, false, betTransm);

            if (dtDeletedBets.Rows.Count == 0)
            {
                tpl.Options.SetValue("");
            }
            tpl.content = content.ToString();

            writer.Write(tpl);
            //}
        }

        protected void ExportToExcel(System.IO.MemoryStream excelStream, string fileName)
        {
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
            Response.Clear();
            Response.BinaryWrite(excelStream.GetBuffer());
            Response.End();
        }

        private void Show_HideComboBox(Dictionary<string, DataTable> results, UserTypes userType)
        {
            if (results["dtCombo1"] != null && results["dtCombo1"].Rows.Count > 0)
            {
                if (CurrentUserSession.IsSuper)
                {
                    userType = UserTypes.Super;

                    //modify tables=> new datatable has 2 columns "custid" & "text" (text=username+betcount+Resources.Lang.bet)
                    results["dtCombo1"] = HtmlGenerator.ModifyDataTable(results["dtCombo1"], string.Empty, "custid", "username", "betcount", Resources.Lang.bet);
                    results["dtCombo2"] = HtmlGenerator.ModifyDataTable(results["dtCombo2"], Resources.Lang.allagent, "custid", "username", "betcount", Resources.Lang.bet);
                    results["dtCombo3"] = HtmlGenerator.ModifyDataTable(results["dtCombo3"], Resources.Lang.allmember, "custid", "username", "betcount", Resources.Lang.bet);

                    tpl.Options.Master.option_master = HtmlGenerator.ComboBox("masterId", masterId, results["dtCombo1"], "custid", "text", "selected_Option_master(this)");
                    tpl.Options.Agent.option_agent = HtmlGenerator.ComboBox("agentId", agentId, results["dtCombo2"], "custid", "text", "selected_Option_agent(this)");

                    if (agentId > 0 && results["dtCombo2"].Rows.Count > 0 && results["dtCombo2"].Select("custid=" + agentId).Length > 0)
                    {
                        tpl.Options.Member.option_member = HtmlGenerator.ComboBox("memberId", memberId, results["dtCombo3"], "custid", "text", "selected_Option_member(this)");
                    }
                    else
                    {
                        tpl.Options.Member.Visible = false;
                    }
                }
                else if (CurrentUserSession.IsMaster)
                {
                    tpl.Options.Master.Visible = false;
                    userType = UserTypes.Master;

                    //modify tables=> new datatable has 2 columns "custid" & "text" (text=username+betcount+Resources.Lang.bet)
                    results["dtCombo1"] = HtmlGenerator.ModifyDataTable(results["dtCombo1"], string.Empty, "custid", "username", "betcount", Resources.Lang.bet);
                    results["dtCombo2"] = HtmlGenerator.ModifyDataTable(results["dtCombo2"], Resources.Lang.allmember, "custid", "username", "betcount", Resources.Lang.bet);

                    tpl.Options.Agent.option_agent = HtmlGenerator.ComboBox("agentId", agentId, results["dtCombo1"], "custid", "text", "selected_Option_agent(this)");
                    tpl.Options.Member.option_member = HtmlGenerator.ComboBox("memberId", memberId, results["dtCombo2"], "custid", "text", "selected_Option_member(this)");
                }
                else if (CurrentUserSession.IsAgent)
                {
                    tpl.Options.Master.Visible = false;
                    tpl.Options.Agent.Visible = false;
                    userType = UserTypes.Agent;

                    //modify tables=> new datatable has 2 columns "custid" & "text" (text=username+betcount+Resources.Lang.bet)
                    results["dtCombo1"] = HtmlGenerator.ModifyDataTable(results["dtCombo1"], string.Empty, "custid", "username", "betcount", Resources.Lang.bet);

                    tpl.Options.Member.option_member = HtmlGenerator.ComboBox("memberId", memberId, results["dtCombo1"], "custid", "text", "selected_Option_member(this)");
                }
            }
            else //has no item of cancelBet
            {
                tpl.Options.Master.Visible = false;
                tpl.Options.Agent.Visible = false;
                tpl.Options.Member.Visible = false;
            }
        }

        public override object InvokeData(Hashtable hash)
        {
            command = BetListFactory.CreateBetList("CancelBetList", CurrentUserSession.NetSessionId, null);

            if (command != null)
            {
                hash.Add("memberId", memberId);
                hash.Add("agentId", agentId);
                hash.Add("masterId", masterId);

                command.SetParams(hash);

                return command.Execute();
            }

            return null;
        }

        public override void ProcessRequest(ref Hashtable hash)
        {
            int.TryParse(Request["masterId"] + string.Empty, out masterId);
            int.TryParse(Request["agentId"] + string.Empty, out agentId);
            int.TryParse(Request["memberId"] + string.Empty, out memberId);

            exportExcel = Request["exporttoexcel.x"] != null ? true : false;
        }

        public override object ValidateRequest(Hashtable hash)
        {
            if (memberId < 0 || agentId < 0 || masterId < 0)
            {
                return new ErrorPage_Page(Resources.Lang.strNotAccessPage);
            }

            return null;
        }

        public void ExportToExcel(HttpResponse Response, string data, string filename)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + filename.Replace(" ", "") + ".xls");
            Response.Charset = "utf-8";
            Response.ContentType = "application/vnd.xls";

            const String excelHtmlHeader = @"<html>
                    <head>
                        <META http-EQUIV='Content-Type' CONTENT='text/html; charset=unicode-utf8'>
                        <style>
                            body{font-family:Tahoma; font-size:12px; color:#000000; background:#ffffff;}
                            td,table{border-collapse:collapse;border-color:#7C98A4; border-style:Solid; border-width:thin;}
                            .league{color: #000000; background: #aacccc;}
                            td{vertical-align:top;text-align:right;font-size:12px;}
                            font, div, span{font-size:12px;text-decoration:none;}
                            .bl_underdog{font-weight:bold}
                            .bl_match{white-space: nowrap; width: 200px}
                            .bl_favorite{color: red;font-weight:bold}
                            a{font-size:12px;text-decoration: none; color: #000}
                            .r,td.r,td.div{text-align:right;font-size:12px;}
                            .l,td.l{text-align:left;font-size:12px;}
                            .c,td.c{text-align:center;font-size:12px;}
                            .bl_footertotal{background-color:#aaccee}
                            thead tr th{background-color:#aaccee; font-size:12px;  border-right: thin solid #7C98A4;}
                            .endTbl{border-color:#ffffff;}
                        </style>
                    </head>
                    <body>
                    ";
            const String excelHtmlFooter = @"
                        <table class='endTbl'>
                            <tr>
                                <td></td>
                            </tr>
                        </table>
                    </body>
                </html>";

            StringBuilder content = new StringBuilder();
            content.Append(excelHtmlHeader);
            content.AppendLine(data);
            content.Append(excelHtmlFooter);

            Response.Write(content);
            Response.End();
        }
    }
}