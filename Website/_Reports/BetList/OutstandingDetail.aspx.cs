// ---------------------------------------------------------------------------------
// <copyright file="OutstandingDetail.aspx.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Thai" date="2011.03.24">Create</change>
//     <change who="Marc.Bui" date="2012.12.13">Apply CoreBetList</change>
//     <change who="Marc.Bui" date="2012.12.13">Rollback old Betlist</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.Age.UI.Reports.BetList
{
    using System;
    using System.Data;
    using System.Text;
    using System.Web.UI;
    using Fanex.Age.UI.Reports.Components;
    using Sunplus.Agent.Model.BL.BetList;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.UI.BetList;
    using Sunplus.Agent.UI.BetList.Factories;
    using Sunplus.Agent.Utilities;

    public partial class OutstandingDetail_Page : WinlossReports_Page
    {
        DataTable dtResult = null;
        BetListBO betList = null;
        DataTable outDataTable = null;

        int custid;
        string username = string.Empty;
        DateTime dateFrom;
        DateTime dateTo;

        OutstandingDetail_Template2 tpl = new OutstandingDetail_Template2();

        int getAll = 0;
        string option = string.Empty;
        string lblBetList = string.Empty;
        string selectedValue = string.Empty;
        const string CHECKED = Fanex.Age.UI.Common.CHECKED;

        string page = null;

        #region Override

        public override void RenderContent(HtmlTextWriter writer)
        {
            // Render Header
            RenderReportHeader(tpl);

            // Render Content
            DataTable betListData = View as DataTable;

            UserTypes userType = UserTypes.Unknown;
            if (CurrentUserSession.IsSuper) userType = UserTypes.Super;
            else if (CurrentUserSession.IsMaster) userType = UserTypes.Master;
            else if (CurrentUserSession.IsAgent) userType = UserTypes.Agent;
            StringBuilder content = new StringBuilder();

            DataTable betTransm = (outDataTable == null ? new DataTable() : outDataTable);

            if (option == "4")//betlist
            {
                tpl.title = Resources.Lang.lblBetLists + " - " + username;
                content = RenderingFactory.Render(BetListType.Default, ref betListData, userType, false, betTransm);
            }
            else if (option == "5")//betlist (running)
            {
                tpl.title = Resources.Lang.lblBetListRunning + " - " + username;
                content = RenderingFactory.Render(BetListType.None, ref betListData, userType, false, betTransm);
            }

            tpl.content = content.ToString();
            RegisterJavascriptObject("fdate", dateFrom.ToShortDateString());
            RegisterJavascriptObject("tdate", dateTo.ToShortDateString());
            RegisterJavascriptObject("page", page);
            RegisterJavascriptObject("option", option);

            if (_exportToExcel)
            {
                ExportToExcel(Response, string.Concat("<b>", tpl.title, "</b>", content.ToString()), "BetList");
            }
            else
            {
                tpl.ToString(writer);
            }
        }

        public override void ProcessRequest(ref System.Collections.Hashtable hash)
        {
            MAX_REPORT_DATE = DateTime.Now.AddDays(7);

            base.ProcessRequest(ref hash);

            if (Request["custid"] != null && Request["username"] != null)
            {
                int.TryParse(Request["custid"] + string.Empty, out custid);
                username = Request["username"] + string.Empty;
            }

            option = (Request["option"] == null) ? "5" : Request["option"].ToString();
            page = (Request["page"] != null ? Request["page"].ToString() : string.Empty);

            #region Date

            switch (option)
            {
                case "4":
                    SetDate_BetList();
                    break;
                case "5":
                    SetDate_BetListRunning();
                    break;
            }

            dateFrom = Sunplus.Agent.Utilities.Formater.ResetStartDate(dateFrom);
            dateTo = Sunplus.Agent.Utilities.Formater.ResetEndDate(dateTo);

            #endregion

            selectedValue = string.Join(null, new string[] { option, "^", lblBetList, "^", custid.ToString(), "^", username });
        }

        private void SetDate_BetListRunning()
        {
            lblBetList = Resources.Lang.lblBetListRunning;

            dateFrom = DateTime.Now.AddDays(-7);
            dateTo = DateTime.Now;

            getAll = (Request["chkgetall"] == null) ? 0 : (Request["chkgetall"].Equals("on") ? 1 : 0);

            if (!string.IsNullOrEmpty(Request["isdate"]))
            {
                if (Request["isdate"] == "-1")
                {
                    dateFrom = DateTime.Now.AddDays(-1);
                    dateTo = DateTime.Now.AddDays(-1);
                    getAll = 0;

                }
                else if (Request["isdate"] == "0")
                {
                    dateFrom = DateTime.Now;
                    dateTo = DateTime.Now;
                    getAll = 0;
                }
                else
                {
                    dateFrom = Cast.AsDateTime(Request["fdate"]);
                    dateTo = Cast.AsDateTime(Request["tdate"]);
                }

            }
            else
            {
                if (getAll == 0)
                {
                    dateFrom = Cast.AsDateTime(Request["fdate"]);
                    dateTo = Cast.AsDateTime(Request["tdate"]);
                }
            }
        }

        private void SetDate_BetList()
        {
            lblBetList = Resources.Lang.lblBetLists;

            dateFrom = DateTime.Now;
            dateTo = DateTime.Now;
            if (!string.IsNullOrEmpty(Request["isdate"]))
            {
                if (Request["isdate"] == "-1")
                {
                    dateFrom = DateTime.Now.AddDays(-1);
                    dateTo = DateTime.Now.AddDays(-1);
                }
                else
                {
                    dateFrom = Cast.AsDateTime(Request["fdate"]);
                    dateTo = Cast.AsDateTime(Request["tdate"]);
                }
            }
            else
            {
                dateFrom = Cast.AsDateTime(Request["fdate"]);
                dateTo = Cast.AsDateTime(Request["tdate"]);

                if (Request["tdate"] == null)//extend ToDate to 7 days
                {
                    dateTo = DateTime.Now.AddDays(7);
                }
            }
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            betList = new BetListBO(CurrentUserSession.NetSessionId);

            if (lblBetList.Equals(Resources.Lang.lblBetListRunning))
            {
                dtResult = betList.GetRunningByDateAndMem(custid, username, dateFrom, dateTo, getAll, _permission, out outDataTable);
            }
            else if (lblBetList.Equals(Resources.Lang.lblBetLists))
            {
                dtResult = betList.GetAllBetsByDateAndMem(custid, username, dateFrom, dateTo, _permission, out outDataTable);
            }
            return dtResult != null ? dtResult : new DataTable();
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }
        #endregion

        #region Helper

        /// <summary>
        /// Render Report Header
        /// </summary>
        /// <param name="template"></param>
        private void RenderReportHeader(OutstandingDetail_Template template)
        {
            //bool isShowed = (_custId == 0 || _custId == CurrentUserSession.CustId);
            bool isShowed = true;//todo
            template.SearchPanel = RenderSearchPanel(isShowed);
        }

        protected new string RenderSearchPanel(bool isShowed)
        {
            RenderFilterElementNames();

            _dateFrom = this.dateFrom;
            _dateTo = this.dateTo;

            WinlossSearchView_Control searchControl = new WinlossSearchView_Control(_filterControl, _dateFrom, _dateTo, isShowed);
            searchControl.IsHistoryReport = _isHistoryReport;
            searchControl.UserType = _userType;
            searchControl._template.BoldExportExcel.Visible = false;
            //searchControl.BoldExportExcel = this.BoldChecked;

            searchControl._template.HistoryMessage.GrossCommMsg.Hide();
            searchControl._template.HistoryMessage.OptionMsg.Hide();
            searchControl._template.HistoryMessage.ContentMessage.SetValue(string.Empty);

            DataTable dt = createBetListOption();

            BetListSections_BetListCombo_Block combo = new BetListSections_BetListCombo_Block();
            combo.BetListComboBox = HtmlGenerator.ComboBox("idoption", selectedValue, dt, "Opvalue", "option", "OnchangeData(this)");

            BetListSections_BetListGetAll_Block getAll = new BetListSections_BetListGetAll_Block();
            if (this.getAll == 1)
            {
                getAll.checkedText = CHECKED;
            }

            searchControl._template.DateRangeSelect.SetDateRange(MIN_REPORT_DATE, MAX_REPORT_DATE);

            searchControl._template.DateRangeSelect._template.BetListOption.Append(combo);
            if (lblBetList.Equals(Resources.Lang.lblBetListRunning))
            {
                searchControl._template.DateRangeSelect._template.BetListGetAll.Append(getAll);
            }
            return searchControl.ToString();

        }

        private DataTable createBetListOption()
        {
            DataTable dt = new DataTable();

            DataColumn column;
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Opvalue";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "option";
            dt.Columns.Add(column);

            DataRow row;

            row = dt.NewRow();
            row["Opvalue"] = "0^";
            row["option"] = "--------------------------------";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Opvalue"] = "1^" + Resources.Lang.dailystatement + "^" + custid + "^" + username;
            row["option"] = Resources.Lang.dailystatement;
            dt.Rows.Add(row);

            //row = dt.NewRow();
            //row["Opvalue"] = "3^" + Resources.Lang.betsummary + "^" + custid + "^" + username;
            //row["option"] = Resources.Lang.betsummary;
            //dt.Rows.Add(row);

            row = dt.NewRow();
            row["Opvalue"] = "4^" + Resources.Lang.lblBetLists + "^" + custid + "^" + username;
            row["option"] = Resources.Lang.lblBetLists;
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Opvalue"] = "5^" + Resources.Lang.lblBetListRunning + "^" + custid + "^" + username;
            row["option"] = Resources.Lang.lblBetListRunning;
            dt.Rows.Add(row);

            return dt;
        }

        #endregion
    }
}
