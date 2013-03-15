using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using Sunplus.Agent.Model.Entities;
using Sunplus.Agent.Utilities;

namespace Sunplus.Agent.UI._Chart
{
    public class WinlossInfo
    {
        #region Define Properties
        public double Turnover { get; set; }
        public double Winlost { get; set; }
        public string Date { get; set; }
        public string ToolTipWinloss
        {
            get
            {
                return string.Format("{0}, {1} = {2} {3}", Date, TitleWinloss, Formater.DecFormat(Winlost, 2), UserSession.Current.Currency);
            }
        }

        public string ToolTipTurnover
        {
            get
            {
                return string.Format("{0}, {1} = {2} {3}", Date, Resources.Lang.turnover, Formater.DecFormat(Turnover, 2), UserSession.Current.Currency);
            }
        }

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
        #endregion
    }

    public class ShareChartData
    {
        public string Title { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<WinlossInfo> ShareData { get; set; }

        public MemoryStream ChartImageStream()
        {

            var chart = new ShareChart(this);
            return chart.GetChartImage(Width, Height);
        }

        public string ChartImageMap(string name)
        {
            var chart = new ShareChart(this);
            return chart.GetChartImageMap(Width, Height, name);
        }
    }

    public class ChartRepository
    {
        public static ShareChartData GetBrowserShares(DataTable reportData, string chartTitle)
        {
            var chartData = new ShareChartData()
                                {
                                    Title = chartTitle,
                                    Width = 780,
                                    Height = 500,
                                    ShareData = new List<WinlossInfo>()
                                };
            if (reportData.Rows.Count > 1)
            {
                foreach (DataRow dr in reportData.Rows)
                {
                    chartData.ShareData.Add(new WinlossInfo()
                    {
                        Date = FormatDateTime(dr["winlostdate"].ToString()),
                        Turnover = Convert.ToDouble(dr["turnover"]),
                        Winlost = Convert.ToDouble(dr["winlost"]),
                    });
                }
            }
             return chartData;
        }
        private static string FormatDateTime(string DateTimeString)
        {
            int year = Convert.ToInt32(DateTimeString.Substring(0, 4));
            int month = Convert.ToInt32(DateTimeString.Substring(4, 2));
            DateTime dt = new DateTime(year, month, 1);
            return String.Format("{0:MMM-yy}", dt);
        } 
    }
}