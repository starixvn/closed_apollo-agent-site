using System.Collections.Generic;
using System.Web.UI.DataVisualization.Charting;
using System.IO;
using System.Drawing;
using Sunplus.Agent.Model.Entities;

namespace Sunplus.Agent.UI._Chart
{
    /// <summary>
    /// Summary description for MyChartBase
    /// </summary>
    public class MyChartBase
    {
        #region Define variable
        protected List<Series> ChartSeriesData { get; set; }
        protected string ChartTitle { get; set; }
        protected Chart myChart;
        protected ChartArea myChartArea;
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

        // This is the method to get the chart image
        public MemoryStream GetChartImage(int width, int height)
        {
            var chart = InitiateChart(width, height);
            chart.RenderType = RenderType.BinaryStreaming;

            var ms = new MemoryStream();
            chart.SaveImage(ms, ChartImageFormat.Png);

            return ms;
        }

        // This is the method to get the chart image map
        public string GetChartImageMap(int width, int height, string mapName)
        {
            var chart = InitiateChart(width, height);
            chart.RenderType = RenderType.ImageMap;
            chart.SaveImage(Stream.Null);

            return chart.GetHtmlImageMap(mapName);
        }

        // Override this method to add title to the chart
        protected virtual void AddChartTitle()
        {
            ChartTitle = null;
        }

        // Override this method to add data to the chart
        protected virtual void AddChartSeries()
        {
            ChartSeriesData = new List<Series>();
        }

        #region Initiate the chart to be rendered
        private Chart InitiateChart(int width, int height)
        {
            myChart = new Chart();
            myChart.Width = width;
            myChart.Height = height;

            myChart.BackColor = Color.FromArgb(232, 232, 232);
            myChart.BorderlineDashStyle = ChartDashStyle.Solid;
            myChart.BackSecondaryColor = System.Drawing.Color.White;
            myChart.BackGradientStyle = GradientStyle.TopBottom;
            myChart.BorderlineWidth = 1;
            myChart.Palette = ChartColorPalette.BrightPastel;
            myChart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            myChart.AntiAliasing = AntiAliasingStyles.All;
            myChart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;

            //Set Chart Title
            AddChartTitle();
            if (ChartTitle != null)
            {
                myChart.Titles.Add(CreateTitleTop());
                myChart.Titles.Add(WinlossTitleLeft());
                myChart.Titles.Add(TurnoverTitleRight());
                //Legend
                myChart.Legends.Add(CreateLegend());
                myChart.CustomizeLegend += new System.EventHandler<CustomizeLegendEventArgs>(this.ChartID_CustomizeLegend);
            }


            AddChartSeries();
            foreach (var series in ChartSeriesData)
            {
                myChart.Series.Add(series);
            }

            //Create ChartArea
            myChartArea = CreateChartArea();
            myChart.ChartAreas.Add(myChartArea);
            this.CreateTurnoverChart(myChart, myChartArea, myChart.Series["seriesWinloss"]);
            return myChart;
        } 
        #endregion

        #region Create chart title: Top, Left, Right
        private Title CreateTitleTop()
        {
            return new Title
            {
                Text = ChartTitle,
                ShadowColor = Color.FromArgb(32, 0, 0, 0),
                Font = new Font("Trebuchet MS", 14, FontStyle.Bold),
                ShadowOffset = 2,
                ForeColor = Color.Black
            };
        }
        private Title WinlossTitleLeft()
        {
            return new Title
            {
                Text = TitleWinloss,
                Font = new Font("Trebuchet MS", 9, FontStyle.Bold),
                TextOrientation = TextOrientation.Rotated270,
                Alignment = ContentAlignment.MiddleCenter,
                Docking = Docking.Left,
                Position = new ElementPosition(2, 1, 2, 90)
            };
        }
        private Title TurnoverTitleRight()
        {
            return new Title
            {
                Text = Resources.Lang.turnover,
                Font = new Font("Trebuchet MS", 9, FontStyle.Bold),
                TextOrientation = TextOrientation.Rotated90,
                Alignment = ContentAlignment.MiddleCenter,
                Docking = Docking.Right,
                Position = new ElementPosition(96, 1, 2, 95)
            };
        }
        #endregion

        #region Create chart Legend
        private Legend CreateLegend()
        {
            return new Legend()
            {
                Name = "LegendBottom",
                Docking = Docking.Bottom,
                Alignment = StringAlignment.Center,
                BorderColor = Color.Gray,
                Font = new Font(new FontFamily("Trebuchet MS"), 9, FontStyle.Bold),
                LegendStyle = LegendStyle.Row,
                IsTextAutoFit = false
            };
        }

        /// <summary>
        /// Handles the CustomizeLegend event of the ChartID control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.DataVisualization.Charting.CustomizeLegendEventArgs"/> instance containing the event data.</param>
        protected void ChartID_CustomizeLegend(object sender, CustomizeLegendEventArgs e)
        {
            // Loop through all default legend items
            if (e.LegendName == "LegendBottom")
            {
                foreach (LegendItem item in e.LegendItems)
                {
                    item.ShadowColor = Color.FromArgb(128, 64, 64, 64);
                    item.ShadowOffset = 2;
                    item.BorderWidth = 0;
                    // Check item series name
                    if (item.SeriesName == "seriesWinloss")
                    {
                        item.Cells[1].Text = TitleWinloss;
                        item.ImageStyle = LegendImageStyle.Rectangle;
                    }
                    else if (item.SeriesName == "seriesTurnover")
                    {
                        item.Cells[1].Text = Resources.Lang.turnover;
                    }
                }
            }
        }
        #endregion

        #region Configure the chart area - the chart frame x/y axes
        private ChartArea CreateChartArea()
        {
            var area = new ChartArea()
            {
                Name = "chartAreaID",
                BorderColor = Color.FromArgb(64, 64, 64, 64),
                BackColor = Color.Transparent,
                ShadowColor = Color.Transparent
            };

            area.AxisX.LineColor = Color.FromArgb(64, 64, 64, 64);
            area.AxisX.IsLabelAutoFit = false;
            area.AxisX.IsStartedFromZero = true;
            area.AxisX.LabelStyle.Font = new Font("Trebuchet MS", 8F);
            area.AxisX.LabelStyle.Angle = -50;
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisX.MajorTickMark.Size = float.Parse("0.5");


            area.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64);
            area.AxisY.IsLabelAutoFit = false;
            area.AxisY.IsStartedFromZero = true;
            area.AxisY.LabelStyle.Font = new Font("Trebuchet MS", 8F, FontStyle.Bold);
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            area.AxisY.MajorTickMark.Size = float.Parse("0");

            area.AxisY2.IsLabelAutoFit = false;
            area.AxisY2.IsStartedFromZero = true;
            area.AxisY2.LabelStyle.Font = new Font("Trebuchet MS", 8F, FontStyle.Bold);
            area.AxisY2.LabelStyle.Format = "N0";
            area.AxisY2.MajorGrid.Enabled = false;
            area.AxisY2.MajorTickMark.Size = float.Parse("0.5");
            return area;
        } 
        #endregion

        #region Copy and Override Turnover series
        private void CreateTurnoverChart(Chart chart, ChartArea area, Series seriesWinloss)
        {
            // Enable Secondary Y axis
            area.AxisX.Interval = 1;
            area.AxisY2.Enabled = AxisEnabled.True;
            area.AxisY.Enabled = AxisEnabled.False;

            // Set custom Chart Area position
            area.Position = new ElementPosition(10, 12, 98, 77);
            area.InnerPlotPosition = new ElementPosition(3, 0, 74, 87);
            // Create extra Y axis for second and third series
            CreateYAxis(chart, area, seriesWinloss, 8, 8);
        }
        /// <summary>
        /// Creates Y axis for the specified series.
        /// </summary>
        /// <param name="chart">Chart control.</param>
        /// <param name="area">Original chart area.</param>
        /// <param name="series">Series.</param>
        /// <param name="axisOffset">New Y axis offset in relative coordinates.</param>
        /// <param name="labelsSize">Extar space for new Y axis labels in relative coordinates.</param>
        public void CreateYAxis(Chart chart, ChartArea area, Series series, float axisOffset, float labelsSize)
        {
            // Create new chart area for original series
            ChartArea areaSeries = chart.ChartAreas.Add("ChartArea_" + series.Name);
            areaSeries.BackColor = Color.Transparent;
            areaSeries.BorderColor = Color.Transparent;
            areaSeries.Position.FromRectangleF(area.Position.ToRectangleF());
            areaSeries.InnerPlotPosition.FromRectangleF(area.InnerPlotPosition.ToRectangleF());
            areaSeries.AxisX.MajorGrid.Enabled = false;
            areaSeries.AxisX.MajorTickMark.Enabled = false;
            areaSeries.AxisX.LabelStyle.Enabled = false;

            areaSeries.AxisY.MajorGrid.Enabled = true;
            areaSeries.AxisY.MajorTickMark.Enabled = true;
            areaSeries.AxisY.MajorGrid.LineColor = Color.LightGray;
            areaSeries.AxisY.LabelStyle.Enabled = false;
            areaSeries.AxisY.IsStartedFromZero = area.AxisY.IsStartedFromZero;
            areaSeries.AxisY.LineColor = Color.Transparent;
            areaSeries.AxisY.MajorTickMark.Size = float.Parse("0.4");
            series.ChartArea = areaSeries.Name;

            // Create new chart area for axis
            ChartArea areaAxis = chart.ChartAreas.Add("AxisY_" + series.ChartArea);
            areaAxis.BackColor = Color.Transparent;
            areaAxis.BorderColor = Color.Transparent;
            areaAxis.Position.FromRectangleF(chart.ChartAreas[series.ChartArea].Position.ToRectangleF());
            areaAxis.InnerPlotPosition.FromRectangleF(chart.ChartAreas[series.ChartArea].InnerPlotPosition.ToRectangleF());

            // Create a copy of specified series
            Series seriesCopy = chart.Series.Add(series.Name + "_Copy");
            seriesCopy.ChartType = series.ChartType;
            foreach (DataPoint point in series.Points)
            {
                seriesCopy.Points.AddXY(point.XValue, point.YValues[0]);
            }

            // Hide copied series
            seriesCopy.IsVisibleInLegend = false;
            seriesCopy.Color = Color.Transparent;
            seriesCopy.BorderColor = Color.Transparent;
            seriesCopy.ChartArea = areaAxis.Name;

            // Disable drid lines & tickmarks
            areaAxis.AxisX.LineWidth = 0;
            areaAxis.AxisX.MajorGrid.Enabled = false;
            areaAxis.AxisX.MajorTickMark.Enabled = false;
            areaAxis.AxisX.LabelStyle.Enabled = false;

            areaAxis.AxisY.MajorGrid.Enabled = false;
            areaAxis.AxisY.IsStartedFromZero = area.AxisY.IsStartedFromZero;
            areaAxis.AxisY.LabelStyle.Font = area.AxisY.LabelStyle.Font;
            areaAxis.AxisY.LabelStyle.Format = "N0";
            areaAxis.AxisY.MajorTickMark.Enabled = false;

            // Adjust area position
            areaAxis.Position.X -= axisOffset;
            areaAxis.InnerPlotPosition.X += labelsSize;
        } 
        #endregion
    }
}