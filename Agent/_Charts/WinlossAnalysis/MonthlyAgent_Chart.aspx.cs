using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using Fanex.Age.UI.Components;
using Fanex.Caching;
using Sunplus.Agent.Model.BL.Charts;
using Sunplus.Agent.Model.Entities;
using Sunplus.Agent.UI._Chart;

/*
 * Created 20111114@Kaden - Charting
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.UI._Charts.WinlossAnalysis
{
    public partial class MonthlyAgent_Chart : System.Web.UI.Page
    {
        #region Define Variables

        protected ICollection<KeyValuePair<string, bool>> _permission = new Dictionary<string, bool>(); // permission to call data
        DateTime _dateFrom, _dateTo;

        #endregion Define Variables

        protected override void InitializeCulture()
        {
            // No cache
            Response.Cache.SetAllowResponseInBrowserHistory(false);
            Response.Cache.SetNoStore();
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            // use english date time format
            CultureInfo def = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(UserSession.Current.Language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(UserSession.Current.Language);

            // use english date time format
            System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat = def.DateTimeFormat;
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat = def.NumberFormat;
            base.InitializeCulture();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _dateFrom = GetDateTimeRequest("fdate", DateTime.Today);
            _dateTo = GetDateTimeRequest("tdate", DateTime.Today);
            _permission.Add(new KeyValuePair<string, bool>(Filter_Control.SHOWSB, GetBooleanRequest("chk_showsb", false)));
            _permission.Add(new KeyValuePair<string, bool>(Filter_Control.SHOWCASINO, GetBooleanRequest("chk_showcasino", false)));
            _permission.Add(new KeyValuePair<string, bool>(Filter_Control.SHOWRB, GetBooleanRequest("chk_showrb", false)));
            _permission.Add(new KeyValuePair<string, bool>(Filter_Control.SHOW_NUMBER_GAME, GetBooleanRequest("chk_showng", false)));
            _permission.Add(new KeyValuePair<string, bool>(Filter_Control.SHOW_IBC_BINGO, GetBooleanRequest("chk_showbi", false)));
            _permission.Add(new KeyValuePair<string, bool>(Filter_Control.SHOW_P2P, GetBooleanRequest("chk_showp2p", false)));
            _permission.Add(new KeyValuePair<string, bool>(Filter_Control.SHOW_LIVECASINO, GetBooleanRequest("chk_showlivecasino", false)));
            _permission.Add(new KeyValuePair<string, bool>(Filter_Control.SHOW_VIRTUALSPORTS, GetBooleanRequest("chk_showvs", false)));
            this.RenderChart();
        }

        #region Render data to chart

        private void RenderChart()
        {
            DataTable reportData = null;
            ChartsBO _chartsBO = new ChartsBO(UserSession.Current.NetSessionId);
            _dateTo = new DateTime(_dateTo.Year, _dateTo.Month, _dateTo.ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd") ? DateTime.Now.Day : this.DayOfMonth(_dateTo.Year, _dateTo.Month));
            reportData = _chartsBO.GetChartWinlossAnalysis(UserSession.Current.CustId, _dateFrom, _dateTo, _permission);

            string chartTitle = string.Format("{0} {1} {2} {3} {4}", Resources.Lang.monthlyWinlossChart,
                                                        Resources.Lang.from.ToLower(), _dateFrom.ToString("MM/dd/yyyy"), Resources.Lang.to.ToLower(), _dateTo.ToString("MM/dd/yyyy"));

            var chartData = ChartRepository.GetBrowserShares(reportData, chartTitle);
            Response.ContentType = "image/png";
            Response.BinaryWrite(chartData.ChartImageStream().GetBuffer());
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
                return Request[name].Equals("1") || Request[name].ToLower().Equals("true") || Request[name].ToLower().Equals("on") ? true : false;
            }
        }

        private int DayOfMonth(int year, int month)
        {
            var day30 = new int[] { 4, 6, 9, 11 };

            if (month == 2)
            {
                return ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0) ? 29 : 28;
            }
            else
            {
                return day30.Contains(month) ? 30 : 31;
            }
        }

        #endregion Render data to chart
    }
}