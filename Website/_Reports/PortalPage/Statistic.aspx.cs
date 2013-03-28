using System;
using System.Web.UI;
using Fanex.Age.UI;
using Fanex.Age.UI.Reports.PortalPage;
using System.Data;
using Sunplus.Agent.Model.Entities;
using Resources;
using Sunplus.Agent.Model.BL.Reports;
using System.Collections;
using Sunplus.Agent.Model.Entities.Permission;

namespace Fanex.Age.UI.Reports
{
    public partial class Statistic : TplPage
    {
        private ReportsBO report = null;
        private int uid = 0;
        private string type = null;

        public override void RenderContent(HtmlTextWriter writer)
        {
            var tpl = new Statistic_Template();
            var dt = View as DataTable;
            var index = 0;

            if (dt == null) return;
            
            tpl.NewMember.UserName = Lang.username;
            tpl.NewMember.GivenCredit = Lang.givencredit;
            tpl.NewMember.CreatedDate = Lang.CreatedDate;

            if (type == null)//Load general data
            {
                var row = dt.Rows[0];

                tpl.Statistic = Lang.statistic.ToUpper();
                tpl.TotalOutstanding = Lang.lblTotalOutstanding;

                if (Sunplus.Agent.Model.BL.Definitions.AppSettings.IsEnableP2P)
                {
                    if (UserPermission.IsShowCasino)
                    {
                        tpl.NewUI.Casino.TotalCasinoTransferOutstanding = Lang.lblTotalCSTransferOutstanding;
                        tpl.NewUI.Casino.CasinoTransfer = this.ConvertToDecimal(row["total_csTransfer"]);
                        tpl.NewUI.Casino.CasinoOutstanding = this.ConvertToDecimal(row["total_csOutStanding"]);
                    }
                    else
                    {
                        tpl.NewUI.Casino.Clear();
                    }

                    tpl.NewUI.P2P.TotalP2PTransferOutstanding = Lang.lblTotalP2PTransferOutstanding;
                    tpl.NewUI.P2P.P2PTransfer = this.ConvertToDecimal(row["total_P2PTransfer"]);
                    tpl.NewUI.P2P.P2POutstanding = this.ConvertToDecimal(row["total_P2POutStanding"]);

                    tpl.OldUI.Clear();
                }
                else
                {
                    if (UserPermission.IsShowCasino)
                    {
                        tpl.OldUI.Casino.TotalCasinoOutstanding = Lang.lblTotalCSOutstanding;
                        tpl.OldUI.Casino.TotalCasinoTransfer = Lang.lblTotalCSTransfer;
                        tpl.OldUI.Casino.TotalCasinoTransfer_vl = this.ConvertToDecimal(row["total_csTransfer"]);
                        tpl.OldUI.Casino.TotalCasinoOutstanding_vl = this.ConvertToDecimal(row["total_csOutStanding"]);
                    }
                    else
                    {
                        tpl.OldUI.Casino.Clear();
                    }

                    tpl.NewUI.Clear();
                }

                tpl.Top10WinningOfMonth = Lang.top10winningmembers;
                tpl.HighestTurnoverOfMonth = Lang.highestturnoverofthemonth;
                tpl.NewMember.NewMemberOfMonth = Lang.newmemberofthemonth;

                tpl.Top10WinningDetail = string.Empty;
                tpl.HighestTurnoverDetail = string.Empty;
                tpl.NewMember.NewMemberDetail = string.Empty;

                tpl.TotalMemberACS = Lang.lblTotalActiveClosedSuspended;

                if (UserSession.Current.IsSuper)
                {
                    tpl.AgentOutstanding = Lang.lblSuperOutstanding;

                    var masterACS =
                    "<span class='active'>" +
                    row["total_masterActive"] + "</span>|<span class='closed'>" +
                    row["total_masterClosed"] + "</span>|<span class='suspended'>" +
                    row["total_masterSuspended"] + "</span>";

                    tpl.NewMember.NewMemberOfMonth_vl = row["number_newMembers"].ToString();

                    if (tpl.NewMember.NewMemberOfMonth_vl.Equals("0"))
                        tpl.NewMember.NoRow.NoRowInfo = Lang.noInfo;
                    else
                        tpl.NewMember.NoRow.Clear();

                    var agentACS = "<span class='active b'>" +
                    row["total_agentActive"] + "</span>|<span class='closed b'>" +
                    row["total_agentClosed"] + "</span>|<span class='suspended b'>" +
                    row["total_agentSuspended"] + "</span>";

                    var memberACS = "<span class='active b'>" +
                    row["total_memberActive"] + "</span>|<span class='closed b'>" +
                    row["total_memberClosed"] + "</span>|<span class='suspended b'>" +
                    row["total_memberSuspended"] + "</span>";

                    var acs = new ACS_Template();
                    acs.Rows.Clear();

                    var acsRow = new ACS_Rows_Block();

                    acsRow.Detail = Lang.master;
                    acsRow.Detail_vl = masterACS;
                    acsRow.oddEvenRow = "oddRow";
                    acsRow.ID = "1";
                    acs.Rows.Append(acsRow);

                    acsRow.Detail = Lang.agent;
                    acsRow.Detail_vl = agentACS;
                    acsRow.oddEvenRow = "evenRow";
                    acsRow.ID = "2";
                    acs.Rows.Append(acsRow);

                    acsRow.Detail = Lang.member;
                    acsRow.Detail_vl = memberACS;
                    acsRow.oddEvenRow = "oddRow";
                    acsRow.ID = "3";
                    acs.Rows.Append(acsRow);

                    tpl.ACS_Detail = acs;
                }
                else if (UserSession.Current.IsMaster)
                {
                    tpl.AgentOutstanding = Lang.lblMasterOutstanding;

                    var agentACS =
                    "<span class='active'>" +
                    row["total_agentActive"] + "</span>|<span class='closed'>" +
                    row["total_agentClosed"] + "</span>|<span class='suspended'>" +
                    row["total_agentSuspended"] + "</span>";

                    tpl.NewMember.NewMemberOfMonth_vl = row["number_newMembers"].ToString();

                    if (tpl.NewMember.NewMemberOfMonth_vl.Equals("0"))
                        tpl.NewMember.NoRow.NoRowInfo = Lang.noInfo;
                    else
                        tpl.NewMember.NoRow.Clear();

                    var memberACS = "<span class='active b'>" +
                    row["total_memberActive"] + "</span>|<span class='closed b'>" +
                    row["total_memberClosed"] + "</span>|<span class='suspended b'>" +
                    row["total_memberSuspended"] + "</span>";

                    var acs = new ACS_Template();
                    acs.Rows.Clear();

                    var acsRow = new ACS_Rows_Block();

                    acsRow.Detail = Lang.agent;
                    acsRow.Detail_vl = agentACS;
                    acsRow.oddEvenRow = "oddRow";
                    acsRow.ID = "1";
                    acs.Rows.Append(acsRow);

                    acsRow.Detail = Lang.member;
                    acsRow.Detail_vl = memberACS;
                    acsRow.oddEvenRow = "evenRow";
                    acsRow.ID = "2";
                    acs.Rows.Append(acsRow);

                    tpl.ACS_Detail = acs;

                }
                else if (UserSession.Current.IsAgent)
                {
                    tpl.AgentOutstanding = Lang.lblAgentOutstanding;

                    var memberACS =
                    "<span class='active'>" +
                    row["total_memberActive"] + "</span>|<span class='closed'>" +
                    row["total_memberClosed"] + "</span>|<span class='suspended'>" +
                    row["total_memberSuspended"] + "</span>";

                    tpl.NewMember.NewMemberOfMonth_vl = row["number_newMembers"].ToString();

                    if (tpl.NewMember.NewMemberOfMonth_vl.Equals("0"))
                        tpl.NewMember.NoRow.NoRowInfo = Lang.noInfo;
                    else
                        tpl.NewMember.NoRow.Clear();

                    var acs = new ACS_Template();
                    acs.Rows.Clear();

                    var acsRow = new ACS_Rows_Block();

                    acsRow.Detail = Lang.member;
                    acsRow.Detail_vl = memberACS;
                    acsRow.oddEvenRow = "oddRow";
                    acsRow.ID = "1";
                    acs.Rows.Append(acsRow);

                    tpl.ACS_Detail = acs;
                }

                tpl.AgentOutstanding_vl = this.ConvertToDecimal(row["outStanding"]);
                tpl.TotalOutstanding_vl = this.ConvertToDecimal(row["total_outstanding"]);

                writer.Write(tpl);
            }
            else if (Convert.ToInt32(type) == 1)//Load top 10 winning members
            {
                //Response.Cache.SetMaxAge(new TimeSpan(1, 0, 0));

                var top10 = new StatisticDiv_Template();
                top10.UserName = Lang.username;
                top10.Detail = Lang.winningamount;
                top10.Rows.Clear();

                if (dt != null && dt.Rows.Count > 0)
                {
                    top10.NoRow.Clear();
                    var top10Row = new StatisticDiv_Rows_Block();

                    index = 0;

                    foreach (DataRow dr in dt.Rows)
                    {
                        index++;

                        top10Row.UserName_vl = dr["username"].ToString();
                        top10Row.Detail_vl = this.ConvertToDecimal(dr["winlost"]);
                        top10Row.oddEvenRow = index % 2 == 0 ? "evenRow" : "oddRow";

                        top10.Rows.Append(top10Row);
                    }
                }
                else
                    top10.NoRow.NoRowInfo = Lang.noInfo;

                writer.Write(top10);
            }
            else if (Convert.ToInt32(type) == 2)//Load highest turnover members
            {
                //Response.Cache.SetMaxAge(new TimeSpan(1, 0, 0));

                var highest = new StatisticDiv_Template();
                highest.UserName = Lang.username;
                highest.Detail = Lang.turnover;
                highest.Rows.Clear();

                if (dt != null && dt.Rows.Count > 0)
                {
                    highest.NoRow.Clear();
                    var highestRow = new StatisticDiv_Rows_Block();

                    index = 0;

                    foreach (DataRow dr in dt.Rows)
                    {
                        index++;

                        highestRow.UserName_vl = dr["username"].ToString();
                        highestRow.Detail_vl = this.ConvertToDecimal(dr["turnover"]);
                        highestRow.oddEvenRow = index % 2 == 0 ? "evenRow" : "oddRow";

                        highest.Rows.Append(highestRow);
                    }
                }
                else
                    highest.NoRow.NoRowInfo = Lang.noInfo;

                writer.Write(highest);
            }
            else if (Convert.ToInt32(type) == 3)//Load new members of the month
            {
                //Response.Cache.SetMaxAge(new TimeSpan(1, 0, 0));

                var newmem = new StatisticDiv2_Template();
                newmem.Rows.Clear();

                if (dt != null && dt.Rows.Count > 0)
                {
                    newmem.NoRow.Clear();
                    var newmemRow = new StatisticDiv2_Rows_Block();
                    index = 0;

                    foreach (DataRow dr in dt.Rows)
                    {
                        index++;

                        newmemRow.UserName_vl = dr["username"].ToString();
                        newmemRow.GivenCredit_vl = this.ConvertToDecimal(dr["credit"]);
                        newmemRow.Detail_vl = dr["creatdate"].ToString();
                        newmemRow.oddEvenRow = index % 2 == 0 ? "evenRow" : "oddRow";

                        newmem.Rows.Append(newmemRow);
                    }
                }
                else
                    newmem.NoRow.NoRowInfo = Lang.noInfo;

                writer.Write(newmem);                
            }
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            var dt = new DataTable();

            try
            {
                if (type == null)
                {
                    dt = report.GetStatisticInfo(uid);
                }
                else
                {
                    var typeId = Convert.ToInt32(type);

                    if (typeId != 0)
                    {
                        if (typeId == 1)
                            dt = report.GetTopWinningMembers(uid);
                        else if (typeId == 2)
                            dt = report.GetTopTurnoverMembers(uid);
                        else if (typeId == 3)
                            dt = report.GetNewMembers(uid);
                    }
                }

                return dt;
            }
            catch
            {
                return null;
            }
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            if (this.report == null)
                this.report = new ReportsBO(UserSession.Current.NetSessionId);

            if (this.uid == 0)
                this.uid = UserSession.Current.CustId;

            type = Request.QueryString["type"];
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        private string ConvertToDecimal(object value)
        {
            var format = "<span class='{0}'>{1}</span>";
            var num = decimal.Round(Convert.ToDecimal(value == DBNull.Value ? 0 : value), 2);
            var style = num < .00m ? "bl_favorite" : "bl_underdog";

            var ret = string.Format(format, style, num.ToString("#,##0.00"));
            return ret;
        }
    }
}