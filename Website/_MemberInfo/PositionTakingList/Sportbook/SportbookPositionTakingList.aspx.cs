/*
 * Created 20111120@Simon - Render Casino Position Taking List
 */
namespace Sunplus.Agent.UI._MemberInfo.PositionTakingList.Sportbook
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web.UI;
    using Fanex.Age.UI.Components;
    using Fanex.Age.UI.Reports.Components;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.BL.MemberInfo.PositionTakingList;
    using Sunplus.Agent.Model.Entities.Permission;
    using Sunplus.Agent.UI._MemberInfo.PositionTakingList;
    using Sunplus.Agent.UI._MemberInfo.PositionTakingList.Sportbook;
    using Sunplus.Agent.Utilities;

    public partial class SportbookPositionTakingList_Page : ContextMenuColumnsBase
    {
        #region Variable
        private const string SportbookPositionTakingList = "SportbookPositionTakingList";
        private int custId = 0;
        private string type = string.Empty, viewType = string.Empty;
        private string[] wildcard = new string[] { "a" };
        private string url = string.Empty;
        private int pageSize = Sunplus.Agent.Model.BL.Definitions.AppSettings.PageSize;
        private int pageIndex = 1;
        private int totalPage = 0;
        private SportbookPositionTakingList_Template tpl = new SportbookPositionTakingList_Template();
        #endregion
        
        #region Override
        public override void RenderContent(HtmlTextWriter writer)
        {
            // For Show Hide Column
            string[] strArrayText = { Resources.Lang.soccer, Resources.Lang.soccer + " " + Resources.Lang.lblLive, Resources.Lang.Basketball, Resources.Lang.Football, Resources.Lang.Tennis, Resources.Lang.Baseball, Resources.Lang.Golf, Resources.Lang.MotorSports, Resources.Lang.OtherSports, Resources.Lang.mixsports };
            string[] cssClasses = { "SC", "SL", "BK", "FB", "TN", "BB", "GF", "MS", "OS", "MP" };
            RegisterCommonScriptObjects("tblMain", new string[] { "headerTop", "headerBottom" }, "ctxContainer", this);
            this.tpl.ContextMenu = this.RenderContextMenu(strArrayText, cssClasses);
            this.tpl.ShowHideColumnClasses = RenderTableClasses(cssClasses);

            Hashtable hash = new Hashtable();
            hash.Add("type", this.type);
            hash.Add("custid", this.custId.ToString());
            hash.Add("USER", Request["USER"]);
            hash.Add("ParentForm", "SportbookPositionTakingList.aspx");
            AppSession.RequestQueryString = hash;

            CommonGroup_Template tplGroup = new CommonGroup_Template();

            tplGroup.CheckedSP = "checked='checked'";
            tplGroup.CheckedRC = string.Empty;
            tplGroup.NumberGame.CheckedNG = string.Empty;
            tplGroup.LiveCasino.CheckedLC = string.Empty;
            tplGroup.VirtualSports.CheckedVS = string.Empty;

            if (!UserPermission.IsShowNumberGame)
            {
                tplGroup.NumberGame = null;
            }
            
            if (!UserPermission.IsShowLiveCasino)
            {
                tplGroup.LiveCasino = null;
            }

            if (!UserPermission.IsShowVirtualSports)
            {
                tplGroup.VirtualSports = null;
            }
            
            this.tpl.CommonGroup = tplGroup;

            // Mixed Sport
            if (!UserPermission.IsShowMixedSport)
            {
                this.tpl.MixSportsParlay = null;
                this.tpl.MixSportsParlay1 = null;
            }

            if (CurrentUserSession.RoleId == (int)UserTypes.Agent || (CurrentUserSession.RoleId == (int)UserTypes.Super && this.type == string.Empty))
            {
                this.tpl.AdditionalRow = null;
                this.tpl.colNum = "2";
            }
            else
            {
                this.tpl.colNum = "3";
            }

            int rowcount = 0;
            this.tpl.Rows.SetValue(this.GetPositionTaking(this.type, ref rowcount));

            this.tpl.rowcount = rowcount.ToString();

            this.RenderPaging();

            writer.Write(this.tpl);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            this.type = (Request.QueryString["type"] == null) ? string.Empty : Request.QueryString["type"].ToString();
            this.custId = (Request.QueryString["custid"] == null) ? CurrentUserSession.CustId : Convert.ToInt32(Request.QueryString["custid"]);

            this.pageSize = GetIntRequest("pagesize", this.pageSize);
            this.pageIndex = GetIntRequest("pageindex", this.pageIndex);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            Dictionary<string, DataTable> view = new Dictionary<string, DataTable>();
            PositionTakingListBO bo = new PositionTakingListBO(CurrentUserSession.NetSessionId);
            DataTable positionTaking = null;

            if (CurrentUserSession.RoleId == (int)UserTypes.Agent)
            {
                this.type = "agent";
                this.wildcard = new string[] { "a" };
                positionTaking = bo.GetData_PositionTaking(this.custId, this.pageIndex, this.pageSize, ref this.totalPage);
                this.url = "../UserAgent/EditMember.aspx?m=0";
            }
            else
            {
                if (CurrentUserSession.RoleId == (int)UserTypes.Master)
                {
                    this.wildcard = new string[] { "m", "a" };
                    if (this.type == string.Empty)
                    {
                        positionTaking = bo.GetData_Upline_PositionTaking(this.custId, CurrentUserSession.RoleId, this.pageIndex, this.pageSize, ref this.totalPage);
                        this.url = "javascript:ViewSB('agent'";
                    }
                    else if (this.type == "agent")
                    {
                        positionTaking = bo.GetData_PositionTaking(this.custId, this.pageIndex, this.pageSize, ref this.totalPage);
                        this.url = "../UserAgent/EditMember.aspx?m=0";
                    }
                }
                else if (CurrentUserSession.RoleId == (int)UserTypes.Super)
                {
                    if (this.type == string.Empty)
                    {
                        this.wildcard = new string[] { "s" };
                        positionTaking = bo.GetData_Upline_PositionTaking(this.custId, CurrentUserSession.RoleId, this.pageIndex, this.pageSize, ref this.totalPage);
                        this.url = "javascript:ViewSB('master'";
                    }
                    else if (this.type == "master")
                    {
                        this.wildcard = new string[] { "m", "a" };
                        positionTaking = bo.GetData_Upline_PositionTaking(this.custId, 3, this.pageIndex, this.pageSize, ref this.totalPage);
                        this.url = "javascript:ViewSB('agent'";
                    }
                    else if (this.type == "agent")
                    {
                        this.wildcard = new string[] { "s", "m", "a" };
                        positionTaking = bo.GetData_PositionTaking(this.custId, this.pageIndex, this.pageSize, ref this.totalPage);
                        this.url = "../UserAgent/EditMember.aspx?m=0";
                    }
                }
            }

            view.Add(SportbookPositionTakingList, positionTaking);
            return view;
        }
        #endregion

        #region Render Utility
        public string GetPositionTaking(string type, ref int rowcount)
        {
            string result = string.Empty;
            StringBuilder stbResult = new StringBuilder();

            DataTable dtbPositionTaking = (View as Dictionary<string, DataTable>)[SportbookPositionTakingList];
            rowcount = dtbPositionTaking.Rows.Count;
            if (rowcount > 0)
            {
                string valueCustId = string.Empty;
                string userName = string.Empty;

                string closedBg = Fanex.Age.UI.Common.RowCssClosed;
                string suspendedBg = Fanex.Age.UI.Common.RowCssSuspended;
                string disabledBg = Fanex.Age.UI.Common.RowCssDisabled;
                string evenBg = Fanex.Age.UI.Common.RowCssEven;
                string oddBg = Fanex.Age.UI.Common.RowCssOdd;

                SportbookPositionTakingList_Rows_Block tplRow = new SportbookPositionTakingList_Rows_Block();

                // Hide Column
                if (!UserPermission.IsShowMixedSport)
                {
                    tplRow.MixSportsParlay.Hide();
                }

                if (this.wildcard.Length == 1)
                {
                    tplRow.Role.Hide();
                }

                if (this.type == "agent")
                {
                    tplRow.NonRepeat.ViewDownLine.Hide();
                    tplRow.NonRepeat1.ViewDownLine1.Hide();
                }
                else
                {
                    tplRow.NonRepeat.EditMember.Hide();
                    tplRow.NonRepeat1.EditMember1.Hide();
                }

                DataRowCollection dtr = dtbPositionTaking.Rows;
                foreach (DataRow r in dtr)
                {
                    valueCustId = r["custid"].ToString();
                    userName = r["username"].ToString();

                    for (int i = 0; i < this.wildcard.Length; i++)
                    {
                        if (Convert.ToInt32(r["enabled"]) != 1)
                        {
                            tplRow.BgRow = disabledBg;
                        }
                        else if (Convert.ToInt32(r["closed"]) != 0)
                        {
                            tplRow.BgRow = closedBg;
                        }
                        else if (Convert.ToInt32(r["suspended"]) != 0)
                        {
                            tplRow.BgRow = suspendedBg;
                        }
                        else if (i % 2 != 0)
                        {
                            tplRow.BgRow = oddBg;
                        }
                        else
                        {
                            tplRow.BgRow = evenBg;
                        }

                        if (i == 0)
                        {
                            tplRow.NonRepeat.No = r["rownumber"].ToString();
                            tplRow.NonRepeat.EditMember.custid_upline = tplRow.NonRepeat1.EditMember1.custid_upline = this.custId.ToString();
                            tplRow.NonRepeat.EditMember.custid_value = tplRow.NonRepeat1.EditMember1.custid_value = valueCustId;
                            tplRow.NonRepeat.EditMember.username_value = tplRow.NonRepeat1.EditMember1.username_value = tplRow.NonRepeat.ViewDownLine.UserName = tplRow.NonRepeat1.ViewDownLine1.UserName = tplRow.NonRepeat.EditMember.UserName = tplRow.NonRepeat1.EditMember1.UserName = userName;
                            tplRow.NonRepeat.EditMember.roleId = tplRow.NonRepeat1.EditMember1.roleId = CurrentUserSession.RoleId.ToString();
                            tplRow.NonRepeat.ViewDownLine.url = tplRow.NonRepeat1.ViewDownLine1.url = this.url + string.Format(",{0},'{1}')", valueCustId, userName);
                            tplRow.NonRepeat.rowspan = tplRow.NonRepeat1.rowspan = "rowspan='" + this.wildcard.Length.ToString() + "'";
                            tplRow.NonRepeat.Show();
                            tplRow.NonRepeat1.Show();
                        }
                        else
                        {
                            tplRow.NonRepeat.Hide();
                            tplRow.NonRepeat1.Hide();
                        }

                        // Soccer
                        tplRow.Soccer_hdp = Formater.DecFormat(r[this.wildcard[i] + "positiontaking_1"], 2);
                        tplRow.Soccer_ou = Formater.DecFormat(r[this.wildcard[i] + "positiontaking_3"], 2);
                        tplRow.Soccer_1sthdp = Formater.DecFormat(r[this.wildcard[i] + "positiontaking_7"], 2);
                        tplRow.Soccer_1stou = Formater.DecFormat(r[this.wildcard[i] + "positiontaking_8"], 2);
                        tplRow.Soccer_oe = Formater.DecFormat(r[this.wildcard[i] + "positiontaking_2"], 2);
                        tplRow.Soccer_1x2 = Formater.DecFormat(r[this.wildcard[i] + "positiontaking_5"], 2);
                        tplRow.Soccer_cs = Formater.DecFormat(r[this.wildcard[i] + "positiontaking_4"], 2);
                        tplRow.Soccer_tg = Formater.DecFormat(r[this.wildcard[i] + "positiontaking_6"], 2);
                        tplRow.Soccer_mp = Formater.DecFormat(r[this.wildcard[i] + "positiontaking_9"], 2);
                        tplRow.Soccer_outright = Formater.DecFormat(r[this.wildcard[i] + "positiontaking_10"], 2);

                        // Soccer Live
                        tplRow.SoccerLive_hdp = Formater.DecFormat(r[this.wildcard[i] + "livepositiontaking_1"], 2);
                        tplRow.SoccerLive_ou = Formater.DecFormat(r[this.wildcard[i] + "livepositiontaking_3"], 2);
                        tplRow.SoccerLive_1sthdp = Formater.DecFormat(r[this.wildcard[i] + "livepositiontaking_7"], 2);
                        tplRow.SoccerLive_1stou = Formater.DecFormat(r[this.wildcard[i] + "livepositiontaking_8"], 2);

                        // baseball
                        tplRow.Basketball_hdp = Formater.DecFormat(r["v2" + this.wildcard[i] + "positiontaking_1"], 2);
                        tplRow.Basketball_ou = Formater.DecFormat(r["v2" + this.wildcard[i] + "positiontaking_3"], 2);
                        tplRow.Basketball_oe = Formater.DecFormat(r["v2" + this.wildcard[i] + "positiontaking_2"], 2);
                        tplRow.Basketball_ml = Formater.DecFormat(r["v2" + this.wildcard[i] + "positiontaking_20"], 2);

                        // football
                        tplRow.Football_hdp = Formater.DecFormat(r["v3" + this.wildcard[i] + "positiontaking_1"], 2);
                        tplRow.Football_ou = Formater.DecFormat(r["v3" + this.wildcard[i] + "positiontaking_3"], 2);
                        tplRow.Football_oe = Formater.DecFormat(r["v3" + this.wildcard[i] + "positiontaking_2"], 2);
                        tplRow.Football_ml = Formater.DecFormat(r["v3" + this.wildcard[i] + "positiontaking_20"], 2);

                        // tennis
                        tplRow.Tennis_hdp = Formater.DecFormat(r["v5" + this.wildcard[i] + "positiontaking_1"], 2);
                        tplRow.Tennis_ou = Formater.DecFormat(r["v5" + this.wildcard[i] + "positiontaking_3"], 2);
                        tplRow.Tennis_oe = Formater.DecFormat(r["v5" + this.wildcard[i] + "positiontaking_2"], 2);
                        tplRow.Tennis_ml = Formater.DecFormat(r["v5" + this.wildcard[i] + "positiontaking_20"], 2);

                        // baseball
                        tplRow.Baseball_hdp = Formater.DecFormat(r["v8" + this.wildcard[i] + "positiontaking_1"], 2);
                        tplRow.Baseball_ou = Formater.DecFormat(r["v8" + this.wildcard[i] + "positiontaking_3"], 2);
                        tplRow.Baseball_ml = Formater.DecFormat(r["v8" + this.wildcard[i] + "positiontaking_20"], 2);

                        // golf
                        tplRow.Golf_hdp = Formater.DecFormat(r["v10" + this.wildcard[i] + "positiontaking_1"], 2);
                        tplRow.Golf_ou = Formater.DecFormat(r["v10" + this.wildcard[i] + "positiontaking_3"], 2);
                        tplRow.Golf_oe = Formater.DecFormat(r["v10" + this.wildcard[i] + "positiontaking_2"], 2);
                        tplRow.Golf_ml = Formater.DecFormat(r["v10" + this.wildcard[i] + "positiontaking_20"], 2);

                        // motor sports
                        tplRow.MotorSport_hdp = Formater.DecFormat(r["v11" + this.wildcard[i] + "positiontaking_1"], 2);
                        tplRow.MotorSport_ml = Formater.DecFormat(r["v11" + this.wildcard[i] + "positiontaking_20"], 2);

                        // other sports
                        tplRow.OtherSports_hdp = Formater.DecFormat(r["v99" + this.wildcard[i] + "positiontaking_1"], 2);
                        tplRow.OtherSports_ou = Formater.DecFormat(r["v99" + this.wildcard[i] + "positiontaking_3"], 2);
                        tplRow.OtherSports_oe = Formater.DecFormat(r["v99" + this.wildcard[i] + "positiontaking_2"], 2);
                        tplRow.OtherSports_ml = Formater.DecFormat(r["v99" + this.wildcard[i] + "positiontaking_20"], 2);

                        // Parlay Mixed Sports
                        tplRow.MixSportsParlay.ParlayMixedSports_ml = Formater.DecFormat(r["v99" + this.wildcard[i] + "positiontaking_9"], 2);

                        tplRow.Role.Role = this.wildcard[i].ToUpperInvariant();

                        stbResult.Append(tplRow);
                    }
                }

                result = stbResult.ToString();
            }
            else
            {
                int colno = 52;
                NoInfo_Template noInfoRow = new NoInfo_Template();
                if (this.wildcard.Length > 1)
                {
                    colno++;
                }

                noInfoRow.NumCols = colno.ToString();
                result = noInfoRow;
            }

            return result;
        }

        private void RenderPaging()
        {
            string pagingControlName_top = "_PagingTop";
            string pagingControlName_bottom = "MemberList" + CurrentUserSession.SiteName + "_PagingBottom";

            // Paging on Top
            List<object> pageSizeList = new List<object>(new object[] { 10, 50, 100, 500 });
            this.tpl.PagingHeader.pagingControl = new Paging_Control(this.tpl, pagingControlName_top, this.pageSize, Request.Url.AbsoluteUri, this.totalPage * this.pageSize, false);
            this.tpl.PagingHeader.selPageSize = Resources.Lang.pagesize + Utilities.HtmlGenerator.ComboBox("sel" + pagingControlName_top, (object)this.pageSize, pageSizeList, pagingControlName_top + ".SetPageSize(this.value)");
            this.tpl.PagingHeader.selected = null;
            this.tpl.PagingHeader.button = null;

            // Paging on bottom
            if (this.totalPage > 1)
            {
                this.tpl.PagingFooter.pagingControl = new Paging_Control(this.tpl, pagingControlName_bottom, this.pageSize, Request.Url.AbsoluteUri, this.totalPage * this.pageSize, false);
            }
            else
            {
                this.tpl.PagingFooter = null;
            }
        }
        #endregion
    }
}