using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

namespace Sunplus.Agent.UI._Chart
{
    /// <summary>
    /// Summary description for BrowserShareChart
    /// </summary>
    public class ShareChart : MyChartBase
    {
        private ShareChartData chartData;

        public ShareChart(ShareChartData chartData)
        {
            this.chartData = chartData;
        }

        //Chart Title
        protected override void AddChartTitle()
        {
            ChartTitle = chartData.Title;
        }

        // Override the AddChartSeries method to provide the chart data
        protected override void AddChartSeries()
        {
            ChartSeriesData = new List<Series>();
            var seriesWinloss = new Series()
            {
                Name = "seriesWinloss",
                ChartType = SeriesChartType.Line,
                BorderWidth = 3,
                IsValueShownAsLabel = false,
                MarkerStyle = MarkerStyle.Circle,
                BorderColor = Color.FromArgb(64, 0, 0, 0),
                Color = Color.FromArgb(252, 180, 65),
                MarkerSize =4,
                MarkerColor = Color.White,
                MarkerBorderColor = Color.Black,
                MarkerBorderWidth = 1
            };

            var seriesTurnover = new Series()
            {
                Name = "seriesTurnover",
                ChartType = SeriesChartType.Column,
                BorderWidth = 1,
                IsValueShownAsLabel = false,
                BorderColor = Color.FromArgb(180, 26, 59, 105),
                Color = Color.FromArgb(65, 140, 240),
                ShadowOffset = 2,
                IsVisibleInLegend = true,
                CustomProperties = "MaxPixelPointWidth=14",

            };

            var shares = chartData.ShareData;
            foreach (var share in shares)
            {
                var pointWinloss = new DataPoint();
                pointWinloss.YValues = new double[] { share.Winlost };
                pointWinloss.MapAreaAttributes = "onmouseover=\"DisplayTooltip('" + share.ToolTipWinloss + "','toolTipWinloss');\" onmouseout=\"DisplayTooltip('','');\"";
                seriesWinloss.Points.Add(pointWinloss);

                var pointTurnover = new DataPoint();
                pointTurnover.MapAreaAttributes = "onmouseover=\"DisplayTooltip('" + share.ToolTipTurnover + "','toolTipTurnover');\" onmouseout=\"DisplayTooltip('','');\"";
                pointTurnover.AxisLabel = share.Date;
                pointTurnover.YValues = new double[] { share.Turnover };
                seriesTurnover.Points.Add(pointTurnover);
            }
            ChartSeriesData.Add(seriesWinloss);
            ChartSeriesData.Add(seriesTurnover);
        }
    }
}