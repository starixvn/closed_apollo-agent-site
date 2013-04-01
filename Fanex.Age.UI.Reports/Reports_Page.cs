using System;
using System.Data;
using System.IO;
using System.Resources;
using System.Web;
using Fanex.Age.UI;
using Fanex.Age.UI.Components;
using Fanex.Age.UI.Reports.Components;
using Sunplus.Agent.Model.BL.Reports;

/*
 * Created 20100527@Jacob - Support render full mode or body mode
 * Revision:
 * ?@? -
 *
 */

/// </summary>
namespace Fanex.Age.UI.Reports
{
    public abstract class Reports_Page : TplPage
    {
        #region Variable

        protected const string EVEN_ROW_CLASS = Fanex.Age.UI.Common.RowBgEven;
        protected const string ODD_ROW_CLASS = Fanex.Age.UI.Common.RowBgOdd;
        protected const string CLOSED_ROW_CLASS = Fanex.Age.UI.Common.RowBgClosed;
        protected DateTime MIN_REPORT_DATE = DateTime.Today;
        protected DateTime MAX_REPORT_DATE = DateTime.Today;
        protected bool _exportToExcel = false;
        protected DateTime _dateFrom = DateTime.Today;
        protected DateTime _dateTo = DateTime.Today;
        protected int _custId = 0;
        protected int _roleId = 0;

        #region Sorting variable

        protected string _sortingColumn = "";
        protected bool _sortingUp = true;
        protected int _reportRowCount = 0;
        protected bool _sortingEnabled = true;

        #endregion Sorting variable

        private ReportsBO _reportBO = null;

        protected ReportsBO _ReportBO
        {
            get
            {
                if (_reportBO == null) _reportBO = new ReportsBO(CurrentUserSession.NetSessionId);
                return _reportBO;
            }
        }

        #endregion Variable

        public Reports_Page()
        {
            MIN_REPORT_DATE = new DateTime(DateTime.Today.AddMonths(-3).Year, DateTime.Today.AddMonths(-3).Month, 1);
            MAX_REPORT_DATE = MAX_REPORT_DATE.AddDays(7);
        }

        #region Cookies

        /// <summary>
        /// Return the class name boldrow if the cookie value rpt_bold is "1"
        /// </summary>
        protected String BoldRowClass
        {
            get
            {
                String value = HttpContext.Current.Request.Cookies["rpt_bold"] == null ? "0" : HttpContext.Current.Request.Cookies["rpt_bold"].Value;
                return value == "1" ? "boldrow" : "";
            }
        }

        /// <summary>
        /// Return checked='checked' if cookie value rpt_bold is "1"
        /// </summary>
        protected String BoldChecked
        {
            get
            {
                String value = HttpContext.Current.Request.Cookies["rpt_bold"] == null ? "0" : HttpContext.Current.Request.Cookies["rpt_bold"].Value;
                return value == "1" ? "checked='checked'" : "";
            }
        }

        protected bool IsBold
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["rpt_bold"] != null && HttpContext.Current.Request.Cookies["rpt_bold"].Value == "1")
                    return true;
                return false;
            }
        }

        #endregion Cookies

        #region Utility

        public bool AllowCasino
        {
            get
            {
                return Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowCasino;
            }
        }

        public DateTime ResetStartDate(DateTime dt)
        {
            return dt.AddMilliseconds(-(((dt.Hour * 60 + dt.Minute) * 60 + dt.Second) * 1000 + dt.Millisecond));
        }

        public DateTime ResetEndDate(DateTime dt)
        {
            return ResetStartDate(dt).Add(new TimeSpan(23, 59, 59));
        }

        /// <summary>
        /// Get winloss title
        /// </summary>
        /// <returns></returns>
        public String GetWinlossTitle()
        {
            if (CurrentUserSession.IsAgent) return Resources.Lang.memberwinloss;
            else if (CurrentUserSession.IsMaster) return Resources.Lang.agentwinloss;
            else if (CurrentUserSession.IsSuper) return Resources.Lang.masterwinloss;
            else return "";
        }

        /// <summary>
        /// Get winloss detail title
        /// </summary>
        /// <returns></returns>
        public String GetWinlossDetailTitle()
        {
            if (CurrentUserSession.IsAgent) return Resources.Lang.memberwinlossdetail;
            else if (CurrentUserSession.IsMaster) return Resources.Lang.agentwinlostdetail;
            else if (CurrentUserSession.IsSuper) return Resources.Lang.masterwinlostdetail;
            else return "";
        }

        #endregion Utility

        #region Override

        /// <summary>
        ///
        /// </summary>
        /// <param name="p"></param>
        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            _dateFrom = GetDateTimeRequest("fdate", DateTime.Today);
            //_dateFrom = new DateTime(_dateFrom.Year, _dateFrom.Month, 1);
            _dateTo = GetDateTimeRequest("tdate", DateTime.Today);
            if (_dateFrom < MIN_REPORT_DATE) _dateFrom = MIN_REPORT_DATE;
            if (_dateTo < MIN_REPORT_DATE) _dateTo = MIN_REPORT_DATE;
            // export to excel
            _exportToExcel = String.IsNullOrEmpty(Request["exporttoexcel.x"]) ? false : true;

            _custId = GetIntRequest("custid", CurrentUserSession.CustId);
            _roleId = GetIntRequest("roleid", CurrentUserSession.RoleId);

            _sortingColumn = GetStringRequest("sortingcolumn", "");
            _sortingUp = GetBooleanRequest("sortingup", true);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            //Hard-code to allow SB8R view in 4 weeks only
            if (CurrentUserSession.SRecommendId == 955286)
            {
                if (_dateFrom.AddDays(28) < DateTime.Now)
                {
                    _dateFrom = DateTime.Now.AddDays(-28);
                }
            }

            _dateFrom = ResetStartDate(_dateFrom);
            _dateTo = ResetStartDate(_dateTo);

            return null;
        }

        #endregion Override

        #region Render Utility

        /// <summary>
        /// Prevent agent user the role of super and master.
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        private int CheckRoleIdConstrain(int roleId)
        {
            return roleId > CurrentUserSession.RoleId ? CurrentUserSession.RoleId : roleId;
        }

        /// <summary>
        /// Get outstanding title
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="positionTitle"></param>
        /// <returns></returns>
        public String GetOutstandingTilte(int roleId, out String positionTitle)
        {
            positionTitle = "";
            String outstandingTitle = "";
            roleId = CheckRoleIdConstrain(roleId);
            if (CurrentUserSession.IsAgent || roleId == (int)(Sunplus.Agent.Model.BL.Definitions.UserTypes.Agent))
            {
                positionTitle = Resources.Lang.agentposition;
                outstandingTitle = Resources.Lang.memberoutstanding;
            }
            else if (CurrentUserSession.IsMaster || roleId == (int)(Sunplus.Agent.Model.BL.Definitions.UserTypes.Master))
            {
                positionTitle = Resources.Lang.masterposition;
                outstandingTitle = Resources.Lang.lblAgentOutstanding;
            }
            else
            {
                positionTitle = Resources.Lang.superposition;
                outstandingTitle = Resources.Lang.lblMasterOutstanding;
            }
            return outstandingTitle;
        }

        /// <summary>
        /// Export to excel
        /// </summary>
        /// <param name="Response"></param>
        /// <param name="data"></param>
        /// <param name="filename"></param>
        [Obsolete]
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
                            tr.RptHeader, td.RptHeader,tr.RptTitle,td.RptTitle, tr.RptTB, tr.RptFooter, tr.RptHeader02, td.RptHeader02 {color:#000000; background:#aaccee;font-size:12px;font-weight:bold}
                            td{text-align:center;font-size:12px;}
                            font, div, span{font-size:12px;text-decoration:none;}
                            .bl_underdog{font-weight:bold}
                            .bl_match{white-space: nowrap; width: 200px}
                            .bl_favorite{color: red;font-weight:bold}
                            a{font-size:12px;text-decoration: none;}
                            .r,td.r{text-align:right;font-size:12px;}
                            .l,td.l{text-align:left;font-size:12px;}
                            .c,td.c{text-align:center;font-size:12px;}
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

            Response.Write(excelHtmlHeader);
            Response.Write(data);
            Response.Write(excelHtmlFooter);

            Response.End();
        }

        protected void ExportToExcel(MemoryStream excelStream, string fileName)
        {
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", fileName));
            Response.Clear();
            Response.BinaryWrite(excelStream.GetBuffer());
            Response.End();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reportSearchHeader"></param>
        protected void RenderDateRange(ReportSearchHeader_Control reportSearchHeader)
        {
            reportSearchHeader.DateFrom = _dateFrom;
            reportSearchHeader.DateTo = _dateTo;
            reportSearchHeader.SetDateRange(MIN_REPORT_DATE, MAX_REPORT_DATE);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dateRangeSelect"></param>
        protected virtual void RenderDateRange(DateRangeSelect_Control dateRangeSelect)
        {
            dateRangeSelect.DateFrom = _dateFrom;
            dateRangeSelect.DateTo = _dateTo;
            dateRangeSelect.SetDateRange(MIN_REPORT_DATE, MAX_REPORT_DATE);
        }

        protected virtual void RenderDateRangeV2(DateRangeSelect_Control dateRangeSelect, int totalMonths)
        {
            dateRangeSelect.InitializeDropdownListV2(_dateFrom, _dateTo, totalMonths);
        }

        #endregion Render Utility

        #region RequestUtility

        /// <summary>
        /// Get int request value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public int GetIntRequest(String name, int defaultValue)
        {
            if (String.IsNullOrEmpty(Request[name])) return defaultValue;
            else
            {
                int value = 0;
                if (!int.TryParse(Request[name], out value)) return defaultValue;
                return value;
            }
        }

        /// <summary>
        /// Get DateTime request
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public DateTime GetDateTimeRequest(String name, DateTime defaultValue)
        {
            if (String.IsNullOrEmpty(Request[name])) return defaultValue;
            else
            {
                DateTime value = DateTime.Now;
                if (!DateTime.TryParse(Request[name], out value)) return defaultValue;
                return value;
            }
        }

        /// <summary>
        /// Get boolean request
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public bool GetBooleanRequest(String name, bool defaultValue)
        {
            if (String.IsNullOrEmpty(Request[name])) return defaultValue;
            else
            {
                return Request[name].Equals("1") || Request[name].Equals("true") || Request[name].Equals("on") ? true : false;
            }
        }

        public string GetStringRequest(String name, String defaultValue)
        {
            if (String.IsNullOrEmpty(Request[name])) return defaultValue;
            else return Request[name];
        }

        #endregion RequestUtility

        #region GetDBValue

        /// <summary>
        /// Get DB boolean value
        /// If null, return false
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected bool GetDBBooleanValue(object data)
        {
            if (Convert.IsDBNull(data)) return false;
            return (bool)data;
        }

        /// <summary>
        /// Get int value from database
        /// return 0 if the object is DBNull
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected int GetDBIntValue(object data)
        {
            if (Convert.IsDBNull(data)) return 0;
            return (int)data;
        }

        /// <summary>
        /// Get decimal value from database
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected decimal GetDBDecimalValue(object data)
        {
            if (Convert.IsDBNull(data)) return 0;
            return (decimal)data;
        }

        /// <summary>
        /// Get string value from database
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected string GetDBStringValue(object data)
        {
            if (Convert.IsDBNull(data)) return string.Empty;
            return data.ToString();
        }

        #endregion GetDBValue

        #region Sorting

        /// <summary>
        /// Sort data table by _sortingColumn and _sortingOrder
        /// </summary>
        /// <param name="data"></param>
        protected virtual DataTable SortData(DataTable data)
        {
            _reportRowCount = data == null ? 0 : data.Rows.Count;
            if (_sortingColumn.Length > 0 && _reportRowCount > 1 && data.Columns.Contains(_sortingColumn))
            {
                data.DefaultView.Sort = _sortingColumn + (_sortingUp ? " ASC" : " DESC");
                return data.DefaultView.ToTable();
            }
            return data;
        }

        /// <summary>
        /// Register Sorting javascript object
        /// ReportRowCount
        /// SortingColumn
        /// SortingUp
        /// SortingEnabled
        /// </summary>
        protected virtual void RegisterSortingJavascriptObject()
        {
            RegisterJavascriptObject("ReportRowCount", _reportRowCount);
            RegisterJavascriptObject("SortingColumn", _sortingColumn);
            RegisterJavascriptObject("SortingUp", _sortingUp);
            RegisterJavascriptObject("SortingEnabled", _sortingEnabled);
            RegisterJavascriptObject("_date", DateTime.Now.ToShortDateString());
            RegisterJavascriptObject("RoleId", CurrentUserSession.RoleId);
        }

        #endregion Sorting

        private ResourceManager _resourceManager = null;
        static object locker = new object();

        public ResourceManager GetLangResourceManager()
        {
            if (_resourceManager == null)
            {
                lock (locker)
                {
                    if (_resourceManager == null)
                    {
                        _resourceManager = new ResourceManager(typeof(Resources.Lang));
                        _resourceManager.IgnoreCase = true;
                    }
                }
            }

            return _resourceManager;
        }
    }
}