// ---------------------------------------------------------------------------------
// <copyright file="BetList.aspx.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Jacob.Phan" date="2010.05.29">Render BetList page</change>
//     <change who="Thai" date="2011.03.08">Render BetList page</change>
//     <change who="Marc.Bui" date="2012.12.19">Apply CoreBetList</change>
//     <change who="Marc.Bui" date="2012.12.19">Rollback old Betlist</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Sunplus.Agent.UI._BetList
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using Fanex.Age.UI.Components;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.UI.BetList;
    using Sunplus.Agent.UI.BetList.Factories;
    using Sunplus.Agent.Utilities;

    public partial class BetList_Page : Fanex.Age.UI.Reports.WinlossReports_Page
    {
        string type = string.Empty;
        string title = string.Empty;
        int custId;
        int bettype;
        DateTime dateFrom, dateTo;

        bool exportExcel = false;
        IBetListCmd command = null;

        public override void RenderContent(HtmlTextWriter writer)
        {
            DataTable betListData = View as DataTable;
            BetList_Template2 tpl = new BetList_Template2();
            UserTypes userType = UserTypes.Unknown;
            if (CurrentUserSession.IsSuper) userType = UserTypes.Super;
            else if (CurrentUserSession.IsMaster) userType = UserTypes.Master;
            else if (CurrentUserSession.IsAgent) userType = UserTypes.Agent;

            DataTable betTransm = command.OutDataTable();
            if (type.Equals("MixParlay"))
            {
                if (exportExcel) tpl.title = Resources.Lang.lblMixParlay;
                else
                {
                    tpl.title = string.Join(null, new string[]{Resources.Lang.lblMixParlay,
                                                         " | ",
                                                         "<a href=\"BetList.aspx?type=CombinationMixParlay\">",                                                         
                                                         Resources.Lang.lblSystemParlay,
                                                         "</a>"});
                }
            }
            else if (type.Equals("CombinationMixParlay"))
            {
                if (exportExcel) tpl.title = Resources.Lang.lblSystemParlay;
                else
                {
                    tpl.title = string.Join(null, new string[]{ "<a href=\"BetList.aspx?type=MixParlay\">",
                                                         Resources.Lang.lblMixParlay,
                                                         "</a>",
                                                         " | ",
                                                         Resources.Lang.lblSystemParlay});
                }
            }
            else
            {
                tpl.title = string.IsNullOrEmpty(command.Title) ? Resources.Lang.BetList : command.Title;
            }

            RegisterJavascriptObject("type", type);
            RegisterJavascriptObject("fdate", dateFrom.ToShortDateString());
            RegisterJavascriptObject("tdate", dateTo.ToShortDateString());
            RegisterJavascriptObject("isHistoryReport", base._isHistoryReport == true ? 1 : 0);
            if (exportExcel)
            {


                var resMan = GetLangResourceManager();
                var builder = new Fanex.Age.Excel.BetList.BetListReportBuilder(setBetListType(), betListData, userType, tpl.title.ToUpper(), betTransm);
                var excelStream = builder.Build();
                if (excelStream != null)
                {
                    ExportToExcel(excelStream, "BetList.xls");
                    excelStream.Dispose();
                    return;
                }

                //this.ExportToExcel(Response, string.Concat("<b>", tpl.title, "</b>", tpl.content), "BetList");
            }
            else
            {
                // Estimate size of the content
                StringBuilder content = new StringBuilder(1024 * 2 * (betListData.Rows.Count + 3));
                RenderingFactory.Render(setBetListType(), ref betListData, userType, content, exportExcel, betTransm);
                tpl.content = content.ToString();
                RenderFilterElementNames();
                tpl.ToString(writer);
            }
        }

        public override object InvokeData(Hashtable hash)
        {
            DataTable dtResult = null;

            command = BetListFactory.CreateBetList(type, CurrentUserSession.NetSessionId, _permission);
            if (command != null)
            {
                //MixParlay list
                if (type.Equals("MixParlay"))
                {
                    hash.Add("bettype", 9);
                }
                //Combination MP list
                else if (type.Equals("CombinationMixParlay"))
                {
                    hash.Add("bettype", 29);
                }
                else
                {
                    hash.Add("custid", custId);
                    hash.Add("fdate", dateFrom);
                    hash.Add("tdate", dateTo);
                    hash.Add("bettype", bettype);
                }

                command.SetParams(hash);
                dtResult = command.Execute() as DataTable;
            }
            return dtResult;
        }

        public override void ProcessRequest(ref Hashtable hash)
        {
            base.ProcessRequest(ref hash);

            type = Request["type"];

            if (!type.Equals("MixParlay") && !type.Equals("CombinationMixParlay"))
            {
                if (Request["fdate"] == null || Request["tdate"] == null)
                {
                    hash.Add("DateIsNull", true);
                }

                int.TryParse(Request["custid"] + string.Empty, out custId);
                int.TryParse(Request["bettype"] + string.Empty, out bettype);
                dateFrom = Cast.AsDateTime(Request["fdate"]);
                dateTo = Cast.AsDateTime(Request["tdate"]);

                hash.Add("language", Request["lang"] + string.Empty);
                hash.Add("username", Request["username"] + string.Empty);
                hash.Add("race", Request["race"] + string.Empty);
                hash.Add("matchid", Request["matchid"] != null ? int.Parse(Request["matchid"]) : 0);
                hash.Add("leagueid", Request["leagueid"] != null ? int.Parse(Request["leagueid"]) : 0);
                hash.Add("islive", Request["islive"] != null ? int.Parse(Request["islive"]) : 0);
                hash.Add("transtype", Request["transtype"] != null ? int.Parse(Request["transtype"]) : 0);
                hash.Add("betteam", Request["betteam"] + string.Empty);
            }
            else
            {
                dateFrom = DateTime.Now;
                dateTo = DateTime.Now;
            }

            exportExcel = Request["exporttoexcel.x"] != null ? true : false;
        }

        public override object ValidateRequest(Hashtable p)
        {
            if (Request["custid"] != null)
            {
                if (int.Parse(Request["custid"].ToString()) <= 0)
                {
                    return new ErrorPage_Page(Resources.Lang.strNotAccessPage);
                }
            }

            if (Request["bettype"] != null)
            {
                if (int.Parse(Request["bettype"].ToString()) < 0)
                {
                    return new ErrorPage_Page(Resources.Lang.strNotAccessPage);
                }
            }

            DateTime minDate = new DateTime(2000, 1, 1);
            DateTime maxDate = DateTime.Now.AddDays(30);

            if (dateFrom < minDate || dateTo < minDate || dateTo > maxDate) return new ErrorPage_Page(Resources.Lang.strNotAccessPage);

            return null;
        }

        private BetListType setBetListType()
        {
            BetListType betListType = command.BetListType;

            if (Request["racing"] != null && Request["racing"].ToString().Equals("1"))
            {
                betListType |= BetListType.Winlost;
            }
            return betListType;
        }

        public new void ExportToExcel(HttpResponse Response, string data, string filename)
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
                            .bl_footertotal{font-weight:bold}                            
                            .bl_match{white-space: nowrap; width: 200px}
                            .bl_favorite{color: red;font-weight:bold}
                            a{font-size:12px;text-decoration: none; color: #000}                                                        
                            .r,td.r,td.div{text-align:right;font-size:12px;}
                            .l,td.l{text-align:left;font-size:12px;}
                            .c,td.c{text-align:center;font-size:12px;}
                            .bl_footertotal{background-color:#aaccee}
                            thead tr th{background-color:#aaccee; font-size:12px;  border-right: thin solid #7C98A4;}
                            .endTbl{border-color:#ffffff;}
                            .div_result{color: rgb(27, 164, 237);}
                            .strike_through{text-decoration: line-through;}
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


            Response.Write(excelHtmlHeader);
            Response.Write(data);
            Response.Write(excelHtmlFooter);

            Response.End();
        }

    }
}
