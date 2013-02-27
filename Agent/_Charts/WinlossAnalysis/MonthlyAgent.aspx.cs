using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Fanex.Age.UI.Components;
using Fanex.Age.UI.Reports;
using Fanex.Caching;
using Sunplus.Agent.Model.BL.Charts;
using Sunplus.Agent.Model.Entities;
using Sunplus.Agent.UI._Chart;
using Sunplus.Agent.Utilities;

/*
 * Created 20111114@Kaden - Charting
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.UI._Charts.WinlossAnalysis
{
    public partial class MonthlyAgent_Page : WinlossReports_Page
    {
        #region Define Variables

        protected string urlChartPage = string.Empty;
        private bool _chkShowSB = false, _chkShowCasino = false;

        private bool _chkShowRacing = false, _chkShowNG = false, _chkShowBG = false, _chkShowP2P = false,
            _chkShowLiveCasino = false, _chkShowVirtualSports = false;

        #endregion Define Variables

        private string TitleWinloss
        {
            get
            {
                if (UserSession.Current.IsAgent) return Resources.Lang.agentwinloss;
                else if (UserSession.Current.IsMaster) return Resources.Lang.masterwinloss;
                else
                    return Resources.Lang.superwinloss;
            }
        }

        public MonthlyAgent_Page()
        {
            DateTime newdate = DateTime.Today.AddMonths(-18);
            MIN_REPORT_DATE = new DateTime(newdate.Year, newdate.Month, 1);
        }

        #region Override

        public override object InvokeData(System.Collections.Hashtable p)
        {
            DataTable reportData = null;
            ChartsBO _chartsBO = new ChartsBO(UserSession.Current.NetSessionId);
            reportData = _chartsBO.GetChartWinlossAnalysis(UserSession.Current.CustId, _dateFrom, _dateTo, _permission);
            //For test
            //ReportBusiness reportBusiness = new ReportBusiness ();
            //IEnumerable<WinlossReport> list = reportBusiness.GetChartWinlossAnalysis(UserSession.Current.CustId, _dateFrom, _dateTo, _permission);
            //End
            return reportData != null ? reportData : new DataTable();
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            _isHistoryReport = false;
            base.ProcessRequest(ref p);

            if (Request["fdate"] == null || Request["tdate"] == null)
            {
                if (this.IsEndOfThisMonth(DateTime.Now))
                {
                    _dateFrom = GetDateTimeRequest("fdate", DateTime.Today.AddMonths(-2));
                    _dateFrom = new DateTime(_dateFrom.Year, _dateFrom.Month, 1);
                    _dateTo = GetDateTimeRequest("tdate", DateTime.Today);
                }
                else
                {
                    _dateFrom = GetDateTimeRequest("fdate", DateTime.Today.AddMonths(-3));
                    _dateFrom = new DateTime(_dateFrom.Year, _dateFrom.Month, 1);
                    _dateTo = GetDateTimeRequest("tdate", DateTime.Today.AddMonths(-1));
                    _dateTo = new DateTime(_dateTo.Year, _dateTo.Month, this.DayOfMonth(_dateTo.Year, _dateTo.Month));
                }
            }
            else
            {
                _dateFrom = GetDateTimeRequest("fdate", DateTime.Today.AddMonths(-2));
                _dateFrom = new DateTime(_dateFrom.Year, _dateFrom.Month, 1);
                _dateTo = GetDateTimeRequest("tdate", DateTime.Today);
            }

            foreach (KeyValuePair<string, bool> element in _permission)
            {
                if (element.Key == Filter_Control.SHOWSB) _chkShowSB = element.Value;
                else if (element.Key == Filter_Control.SHOWCASINO) _chkShowCasino = element.Value;
                else if (element.Key == Filter_Control.SHOWRB) _chkShowRacing = element.Value;
                else if (element.Key == Filter_Control.SHOW_NUMBER_GAME) _chkShowNG = element.Value;
                else if (element.Key == Filter_Control.SHOW_IBC_BINGO) _chkShowBG = element.Value;
                else if (element.Key == Filter_Control.SHOW_P2P) _chkShowP2P = element.Value;
                else if (element.Key == Filter_Control.SHOW_LIVECASINO) _chkShowLiveCasino = element.Value;
                else if (element.Key == Filter_Control.SHOW_VIRTUALSPORTS) _chkShowVirtualSports = element.Value;
            }
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        #endregion Override

        #region Render Content

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            MonthlyAgent_Template tpl = new MonthlyAgent_Template();
            tpl.actionForm = "action='MonthlyAgent.aspx?" + Request.QueryString.ToString() + "'";
            //Render Report Header
            RenderReportHeader(tpl);
            //Render Content
            RenderReportContent(tpl);
            RenderFilterElementNames();
            RegisterJavascriptObject("least3monthtochart", Resources.Lang.least3monthtochart);
            RegisterJavascriptObject("ServerTime", DateTime.Now.ToString("M/d/yyyy"));
            writer.Write(tpl);
        }

        /// <summary>
        /// Render Header
        /// </summary>
        /// <param name="tpl"></param>
        private void RenderReportHeader(MonthlyAgent_Template tpl)
        {
            tpl.TableWinLoss.titlewinloss = TitleWinloss;
            tpl.title = Resources.Lang.monthlyWinlossChart;
            tpl.TableWinLoss.BoldRow = this.BoldRowClass;
            optionIsChecked = new bool[] { true, false, false };
            tpl.SearchPanel = RenderSearchPanelOnChart();
            //Set URL chart page
            urlChartPage = string.Format("MonthlyAgent_Chart.aspx?fdate={0}&tdate={1}&chk_showsb={2}&chk_showcasino={3}&chk_showrb={4}&chk_showng={5}&chk_showbi={6}&chk_showp2p={7}&chk_showlivecasino={8}&chk_showvs={9}",
                                      _dateFrom, _dateTo, _chkShowSB, _chkShowCasino, _chkShowRacing,
                                      _chkShowNG, _chkShowBG, _chkShowP2P, _chkShowLiveCasino, _chkShowVirtualSports
                                       );
        }

        #endregion Render Content

        #region Render Report Content

        private string FormatDateTime(string DateTimeString)
        {
            int year = Convert.ToInt32(DateTimeString.Substring(0, 4));
            int month = Convert.ToInt32(DateTimeString.Substring(4, 2));
            DateTime dt = new DateTime(year, month, 1);
            return String.Format("{0:MMM - yyyy}", dt);
        }

        private void RenderReportContent(MonthlyAgent_Template tpl)
        {
            DataTable reportData = View as DataTable;
            MonthlyAgent_TableWinLoss_Block tableBlock = tpl.TableWinLoss;
            tableBlock.Content.Clear();
            int numRow = reportData.Rows.Count;
            if (numRow > 1)
            {
                //Render Image Chart and ImageMap
                var chartData = ChartRepository.GetBrowserShares(reportData, null);
                var chart = new ShareChart(chartData);
                tpl.ChartImage.pathChart = urlChartPage;
                tpl.ChartImage.ChartImageMap = chart.GetChartImageMap(780, 500, "ChartImageMap");
                //End Render
                tableBlock.width = tpl.width = "250px";

                MonthlyAgent_TableWinLoss_Content_Block contentBlock = new MonthlyAgent_TableWinLoss_Content_Block();
                foreach (DataRow row in reportData.Rows)
                {
                    contentBlock.ColorClass = (numRow % 2) == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS; ;
                    numRow--;
                    contentBlock.monthly = FormatDateTime(row["winlostdate"].ToString());
                    contentBlock.turnover = Formater.DecFormat(row["turnover"], 2);
                    contentBlock.winloss = Formater.DecFormat(row["winlost"], 2);
                    tableBlock.Content.Append(contentBlock);
                }
                tableBlock.NoInfo.Hide();
            }
            else
            {
                tpl.ChartImage.SetValue("");
                tableBlock.NoInfo.msg = Resources.Lang.noSufficientData;
                tableBlock.width = tpl.width = "100%";
            }
        }

        #endregion Render Report Content

        private bool IsEndOfThisMonth(DateTime now)
        {
            return now.Day == this.DayOfMonth(now.Year, now.Month);
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
    }
}