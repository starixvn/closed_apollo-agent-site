/*
 * Created 20111120@Simon - Render Casino Position Taking List
 */
namespace Sunplus.Agent.UI._MemberInfo.PositionTakingList.Racing
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
    using Sunplus.Agent.UI._MemberInfo.PositionTakingList.Racing;
    using Sunplus.Agent.Utilities;

    public partial class RacingPositionTakingList_Page : ContextMenuColumnsBase
    {
        #region Variable       
        private const string RacingPtList = "RacingPositionTakingList";
        private int custId = 0;
        private string type = string.Empty, viewType = string.Empty;
        private string[] wildcard = new string[] { "a" };
        private string url = string.Empty;
        private int pageSize = Sunplus.Agent.Model.BL.Definitions.AppSettings.PageSize;
        private int pageIndex = 1;
        private int totalPage = 0;
        private RacingPositionTakingList_Template tpl = new RacingPositionTakingList_Template();                        
        #endregion

        #region Override
        public override void RenderContent(HtmlTextWriter writer)
        {
            Hashtable hash = new Hashtable();
            hash.Add("type", this.type);
            hash.Add("custid", this.custId.ToString());
            hash.Add("USER", Request["USER"]);
            hash.Add("ParentForm", "RacingPositionTakingList.aspx");
            AppSession.RequestQueryString = hash;
            
            ArrayList arrTextShowHide = new ArrayList();
            ArrayList arrCssShowHide = new ArrayList();

            int hrcols = 6;

            if (!UserPermission.IsShowRacing_151_31)
            {
                this.tpl.SAHorse = null;
                hrcols -= 2;
            }

            if (!UserPermission.IsShowRacing_151_41)
            {
                this.tpl.UKHorse = null;
                hrcols -= 2;
            }

            if (!UserPermission.IsShowRacing_151_51)
            {
                this.tpl.AUHorse = null;
                hrcols -= 2;
            }

            this.tpl.Horse.hrcols = hrcols.ToString();
            if (hrcols == 0 || !UserPermission.IsShowRacing)
            {
                this.tpl.Horse = null;
            }
            else
            {
                arrTextShowHide.Add(Resources.Lang.horseRacing);
                arrCssShowHide.Add("HO");
            }

            int countGreycols = 4;

            if (!UserPermission.IsShowRacing_152_41)
            {
                this.tpl.UKGreyhound = null;
                countGreycols -= 2;
            }

            if (!UserPermission.IsShowRacing_152_51)
            {
                this.tpl.AUGreyhound = null;
                countGreycols -= 2;
            }
            
            this.tpl.Greyhound.Greycols = countGreycols.ToString();
            if (countGreycols == 0)
            {
                this.tpl.Greyhound = null;
            }
            else
            {
                arrTextShowHide.Add(Resources.Lang.greyHound);
                arrCssShowHide.Add("GH");
            }

            if (!UserPermission.IsShowRacing_153_51)
            {
                this.tpl.AUHarness = null;
                this.tpl.AUHarness1 = null;
            }
            else
            {
                arrTextShowHide.Add(Resources.Lang.harness);
                arrCssShowHide.Add("HN");
            }

            // For Show Hide Column
            string[] strArrayText = (string[])arrTextShowHide.ToArray(typeof(string));
            string[] strArrayCss = (string[])arrCssShowHide.ToArray(typeof(string));
            RegisterCommonScriptObjects("tblMain", new string[] { "headerTop", "headerBottom" }, "ctxContainer", this);
            this.tpl.ContextMenu = RenderContextMenu(strArrayText, strArrayCss);
            this.tpl.ShowHideColumnClasses = RenderTableClasses(strArrayCss);

            CommonGroup_Template tplGroup = new CommonGroup_Template();

            tplGroup.CheckedSP = string.Empty;
            tplGroup.CheckedRC = "checked='checked'";
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
            this.tpl.Rows.SetValue(this.GetPositionTakingRacing(this.type, ref rowcount));

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
                positionTaking = bo.GetData_PositionTakingRacing(this.custId, this.pageIndex, this.pageSize, ref this.totalPage);
            }
            else
            {
                if (CurrentUserSession.RoleId == (int)UserTypes.Master)
                {
                    this.wildcard = new string[] { "m", "a" };
                    if (this.type == string.Empty)
                    {
                        positionTaking = bo.GetData_Upline_PositionTakingRacing(this.custId, CurrentUserSession.RoleId, this.pageIndex, this.pageSize, ref this.totalPage);
                        this.url = "javascript:ViewRacing('agent'";
                    }
                    else if (this.type == "agent")
                    {
                        positionTaking = bo.GetData_PositionTakingRacing(this.custId, this.pageIndex, this.pageSize, ref this.totalPage);
                    }
                }
                else if (CurrentUserSession.RoleId == (int)UserTypes.Super)
                {
                    if (this.type == string.Empty)
                    {
                        this.wildcard = new string[] { "s" };
                        positionTaking = bo.GetData_Upline_PositionTakingRacing(this.custId, CurrentUserSession.RoleId, this.pageIndex, this.pageSize, ref this.totalPage);
                        this.url = "javascript:ViewRacing('master'";
                    }
                    else if (this.type == "master")
                    {
                        this.wildcard = new string[] { "m", "a" };
                        positionTaking = bo.GetData_Upline_PositionTakingRacing(this.custId, 3, this.pageIndex, this.pageSize, ref this.totalPage);
                        this.url = "javascript:ViewRacing('agent'";
                    }
                    else if (this.type == "agent")
                    {
                        this.wildcard = new string[] { "s", "m", "a" };
                        positionTaking = bo.GetData_PositionTakingRacing(this.custId, this.pageIndex, this.pageSize, ref this.totalPage);
                    }
                }
            }

            view.Add(RacingPtList, positionTaking);
            return view;
        }
        #endregion

        #region Render Utility
        public string GetPositionTakingRacing(string type, ref int rowcount)
        {
            string result = string.Empty;
            StringBuilder stbResult = new StringBuilder();

            DataTable dtbPositionTaking = (View as Dictionary<string, DataTable>)[RacingPtList];
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

                RacingPositionTakingList_Rows_Block tplRow = new RacingPositionTakingList_Rows_Block();

                if (this.wildcard.Length == 1)
                {
                    tplRow.Role.Hide();
                }

                if (this.type == "agent")
                {
                    tplRow.NonRepeat.ViewDownLine.Hide();
                }
                else
                {
                    tplRow.NonRepeat.EditMember.Hide();
                }

                if (!UserPermission.IsShowRacing_151_31)
                {
                    tplRow.SAHorse.Hide();
                }

                if (!UserPermission.IsShowRacing_151_41)
                {
                    tplRow.UKHorse.Hide();
                }

                if (!UserPermission.IsShowRacing_151_51)
                {
                    tplRow.AUHorse.Hide();
                }

                if (!UserPermission.IsShowRacing_152_41)
                {
                    tplRow.UKGreyhound.Hide();
                }

                if (!UserPermission.IsShowRacing_152_51)
                {
                    tplRow.AUGreyhound.Hide();
                }

                if (!UserPermission.IsShowRacing_153_51)
                {
                    tplRow.AUHarness.Hide();
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
                            tplRow.NonRepeat.EditMember.custid_upline = this.custId.ToString();
                            tplRow.NonRepeat.EditMember.custid_value = valueCustId;
                            tplRow.NonRepeat.EditMember.username_value = tplRow.NonRepeat.ViewDownLine.UserName = tplRow.NonRepeat.EditMember.UserName = userName;
                            tplRow.NonRepeat.EditMember.roleId = CurrentUserSession.RoleId.ToString();
                            tplRow.NonRepeat.ViewDownLine.url = this.url + string.Format(",{0},'{1}')", valueCustId, userName);
                            tplRow.NonRepeat.rowspan = "rowspan='" + this.wildcard.Length.ToString() + "'";
                            tplRow.NonRepeat.Show();
                        }
                        else
                        {
                            tplRow.NonRepeat.Hide();
                        }

                        // Horse
                        if (UserPermission.IsShowRacing_151_31)
                        {
                            tplRow.SAHorse.hr_win = Formater.DecFormat(r["v151" + this.wildcard[i] + "positiontaking_31"], 2);
                            tplRow.SAHorse.hr_place = Formater.DecFormat(r["v151" + this.wildcard[i] + "positiontaking_32"], 2);
                        }

                        tplRow.UKHorse.UK_Win_Horse = Formater.DecFormat(r["v151" + this.wildcard[i] + "positiontaking_41"], 2);
                        tplRow.UKHorse.UK_Place_Horse = Formater.DecFormat(r["v151" + this.wildcard[i] + "positiontaking_42"], 2);
                        tplRow.AUHorse.AU_win_Horse = Formater.DecFormat(r["v151" + this.wildcard[i] + "positiontaking_51"], 2);
                        tplRow.AUHorse.AU_place_Horse = Formater.DecFormat(r["v151" + this.wildcard[i] + "positiontaking_52"], 2);

                        // Greyhound
                        tplRow.UKGreyhound.UK_Win_Greyhound = Formater.DecFormat(r["v152" + this.wildcard[i] + "positiontaking_41"], 2);
                        tplRow.UKGreyhound.UK_Place_Greyhound = Formater.DecFormat(r["v152" + this.wildcard[i] + "positiontaking_42"], 2);
                        tplRow.AUGreyhound.AU_win_Greyhound = Formater.DecFormat(r["v152" + this.wildcard[i] + "positiontaking_51"], 2);
                        tplRow.AUGreyhound.AU_place_Greyhound = Formater.DecFormat(r["v152" + this.wildcard[i] + "positiontaking_52"], 2);

                        // Harness
                        tplRow.AUHarness.AU_win_Harness = Formater.DecFormat(r["v153" + this.wildcard[i] + "positiontaking_51"], 2);
                        tplRow.AUHarness.AU_place_Harness = Formater.DecFormat(r["v153" + this.wildcard[i] + "positiontaking_52"], 2);

                        tplRow.Role.Role = this.wildcard[i].ToUpperInvariant();

                        stbResult.Append(tplRow);
                    }
                }

                result = stbResult.ToString();
            }
            else
            {
                int colno = 20;
                NoInfo_Template noInfoRow = new NoInfo_Template();
                if (this.wildcard.Length > 1)
                {
                    colno++;
                }

                if (!UserPermission.IsShowRacing_151_31)
                {
                    colno -= 2;
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