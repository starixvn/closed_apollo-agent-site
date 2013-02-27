/*
 * Created 20111120@Simon - Render Casino Position Taking List
 */
namespace Sunplus.Agent.UI._MemberInfo.PositionTakingList.LiveCasino
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
    using Sunplus.Agent.UI._MemberInfo.PositionTakingList.LiveCasino;
    using Sunplus.Agent.Utilities;

    public partial class LiveCasinoPositionTakingList_Page : ContextMenuColumnsBase
    {
        #region Variable        
        private const string LiveCasinoPtList = "LiveCasinoPositionTakingList";
        private int custId = 0;
        private string type = string.Empty, viewType = string.Empty;
        private string[] wildcard = new string[] { "a" };
        private string url = string.Empty;
        private int pageSize = Sunplus.Agent.Model.BL.Definitions.AppSettings.PageSize;
        private int pageIndex = 1;
        private int totalPage = 0;
        private LiveCasinoPositionTakingList_Template tpl = new LiveCasinoPositionTakingList_Template();
        #endregion

        #region Override
        public override void RenderContent(HtmlTextWriter writer)
        {
            Hashtable hash = new Hashtable();
            hash.Add("type", this.type);
            hash.Add("custid", this.custId.ToString());
            hash.Add("USER", Request["USER"]);
            hash.Add("ParentForm", "LiveCasinoPositionTakingList.aspx");
            AppSession.RequestQueryString = hash;

            CommonGroup_Template tplGroup = new CommonGroup_Template();

            tplGroup.CheckedSP = string.Empty;
            tplGroup.CheckedRC = string.Empty;
            tplGroup.NumberGame.CheckedNG = string.Empty;
            tplGroup.LiveCasino.CheckedLC = "checked='checked'";
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
            this.tpl.Rows.SetValue(this.GetPositionTakingLiveCasino(this.type, this.custId.ToString(), ref rowcount));

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
                positionTaking = bo.GetData_PositionTakingLiveCasino(this.custId, this.pageIndex, this.pageSize, ref this.totalPage);
            }
            else
            {
                if (CurrentUserSession.RoleId == (int)UserTypes.Master)
                {
                    this.wildcard = new string[] { "m", "a" };
                    if (this.type == string.Empty)
                    {
                        positionTaking = bo.GetData_Upline_PositionTakingLiveCasino(this.custId, CurrentUserSession.RoleId, this.pageIndex, this.pageSize, ref this.totalPage);
                        this.url = "javascript:ViewLC('agent'";
                    }
                    else if (this.type == "agent")
                    {
                        positionTaking = bo.GetData_PositionTakingLiveCasino(this.custId, this.pageIndex, this.pageSize, ref this.totalPage);
                    }
                }
                else if (CurrentUserSession.RoleId == (int)UserTypes.Super)
                {
                    if (this.type == string.Empty)
                    {
                        this.wildcard = new string[] { "s" };
                        positionTaking = bo.GetData_Upline_PositionTakingLiveCasino(this.custId, CurrentUserSession.RoleId, this.pageIndex, this.pageSize, ref this.totalPage);
                        this.url = "javascript:ViewLC('master'";
                    }
                    else if (this.type == "master")
                    {
                        this.wildcard = new string[] { "m", "a" };
                        positionTaking = bo.GetData_Upline_PositionTakingLiveCasino(this.custId, 3, this.pageIndex, this.pageSize, ref this.totalPage);
                        this.url = "javascript:ViewLC('agent'";
                    }
                    else if (this.type == "agent")
                    {
                        this.wildcard = new string[] { "s", "m", "a" };
                        positionTaking = bo.GetData_PositionTakingLiveCasino(this.custId, this.pageIndex, this.pageSize, ref this.totalPage);
                    }
                }
            }

            view.Add(LiveCasinoPtList, positionTaking);
            return view;
        }
        #endregion

        #region Render Utility
        public string GetPositionTakingLiveCasino(string type, string custid, ref int rowcount)
        {
            string result = string.Empty;
            StringBuilder stbResult = new StringBuilder();
            RegisterJavascriptObject("RoleId", CurrentUserSession.RoleId.ToString());

            DataTable dtbPositionTaking = (View as Dictionary<string, DataTable>)[LiveCasinoPtList];
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

                LiveCasinoPositionTakingList_Rows_Block tplRow = new LiveCasinoPositionTakingList_Rows_Block();
                if (this.wildcard.Length == 1)
                {
                    tplRow.Role.Hide();
                }

                DataRowCollection dtr = dtbPositionTaking.Rows;
                foreach (DataRow r in dtr)
                {
                    tplRow.NonRepeat.OnlyTextUser.Hide();
                    tplRow.NonRepeat.ViewDownLine.Hide();
                    tplRow.NonRepeat.EditMember.Hide();

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
                            tplRow.NonRepeat.EditMember.custid_value = valueCustId;
                            tplRow.NonRepeat.ViewDownLine.UserName = tplRow.NonRepeat.EditMember.UserName = tplRow.NonRepeat.OnlyTextUser.UserName = userName;

                            if (CurrentUserSession.RoleId == (int)UserTypes.Agent)
                            {
                                tplRow.NonRepeat.EditMember.Show();
                            }
                            else
                            {
                                if (Convert.ToInt16(r["synclivecasino"]) != 1)
                                {
                                    tplRow.NonRepeat.OnlyTextUser.Show();
                                }
                                else
                                {
                                    if (this.type == "agent")
                                    {
                                        tplRow.NonRepeat.EditMember.Show();
                                    }
                                    else
                                    {
                                        tplRow.NonRepeat.ViewDownLine.Show();
                                        tplRow.NonRepeat.ViewDownLine.url = this.url + string.Format(",{0},'{1}')", valueCustId, userName);;
                                    }
                                }
                            }

                            tplRow.NonRepeat.rowspan = "rowspan='" + this.wildcard.Length.ToString() + "'";
                            tplRow.NonRepeat.Show();
                        }
                        else
                        {
                            tplRow.NonRepeat.Hide();
                        }

                        tplRow.DeadBall_1stLastOU = Formater.DecFormat(r["v162" + this.wildcard[i] + "positiontaking"], 2);
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

                if (!UserPermission.IsShowNumberGroup)
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