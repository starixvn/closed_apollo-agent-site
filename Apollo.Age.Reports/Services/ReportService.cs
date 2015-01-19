// --------------------------------------------------------------------------
// <copyright file="ReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.4.9">Add movebetday and balanceupday</change>
// </history>

namespace Apollo.Age.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Apollo.Age.Entities;
    using Apollo.Age.Reports.Core;
    using Apollo.Age.Reports.Criterias;
    using Apollo.Age.Resources;
    using Apollo.Age.Services.Impl;
    using Fanex.Caching;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Report service class.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
    public class ReportService : QueryService
    {
        private const string _BalanceUpdayKey = "Apollo.Age.Reports.Services.SpecialDays";

        /// <summary>
        /// Gets the cache manager.
        /// </summary>
        /// <value>
        /// The cache manager.
        /// </value>
        protected ICache CacheManager
        {
            get
            {
                return CacheService.CreateInstance(CacheType.MemoryCache);
            }
        }

        /// <summary>
        /// Gets the special days.
        /// </summary>
        /// <value>
        /// The special days.
        /// </value>
        private dynamic SpecialDays
        {
            get
            {
                dynamic specialDays = CacheManager.Get<dynamic>(_BalanceUpdayKey);
                if (specialDays == null)
                {
                    specialDays = this.GetData<dynamic>(new GetSpecialDaysCriteria()).Single();
                    CacheManager.Set<dynamic>(_BalanceUpdayKey, specialDays, DateTime.Now.AddMinutes(5));
                }

                return specialDays;
            }
        }

        /// <summary>
        /// Gets the balance up day.
        /// </summary>
        /// <returns>Balance Up day.</returns>
        public DateTime GetBalanceUpDay()
        {
            return SpecialDays.BalanceUpDay;
        }

        /// <summary>
        /// Moves the bets day.
        /// </summary>
        /// <returns>Move bet day.</returns>
        public DateTime MoveBetsDay()
        {
            return SpecialDays.MoveBetsDay;
        }

        /// <summary>
        /// Gets the parlay bet type.
        /// </summary>
        /// <returns>List of bet types.</returns>
        public List<SingbetBettype> GetParlayBettype()
        {
            var bettypeList = new List<SingbetBettype>
                {
                    new SingbetBettype
                    {
                        Id = BettypeGroup.MixParlay + "," + BettypeGroup.SystemParlay,
                        Name = Lang.all
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.MixParlay,
                        Name = Lang.lblMixParlay
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.SystemParlay,
                        Name = Lang.lblSystemParlay
                    },
                };

            return bettypeList;
        }

        /// <summary>
        /// Gets the bet type.
        /// </summary>
        /// <param name="isIncludeParlay">Include parlay bet type or not.</param>
        /// <returns>List of bet types.</returns>
        public List<SingbetBettype> GetBettype(bool isIncludeParlay = true)
        {
            var bettypeList = new List<SingbetBettype>
                {
                    new SingbetBettype
                    {
                        Id = BettypeGroup.All,
                        Name = Lang.all
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.Handicap,
                        Name = Lang.lblHandicapFC
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.OddEven,
                        Name = Lang.lblOddEven
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.OverUnder,
                        Name = Lang.lblOverUnder
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.CorrectScore,
                        Name = Lang.correctscore
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.FT1X2,
                        Name = Lang.lblFT1X2
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.TotalGoal,
                        Name = Lang.totalgoal
                    },
                     new SingbetBettype
                     {
                        Id = BettypeGroup.FirstGoalLastGoal,
                        Name = Lang.firstgoal + "/" + Lang.lastgoal
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.HTFT,
                        Name = Lang.htft
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.Outright,
                        Name = Lang.lblOutRight
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.FirstHandicap,
                        Name = Lang.lbl1stHdp
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.FirstOddEven,
                        Name = Lang.lbl1stOddEven
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.FirstOverUnder,
                        Name = Lang.lbl1stOU
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.FirstHalf1X2,
                        Name = Lang.lbl1x2_1st
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.FirstHalfTotalGoal,
                        Name = Lang.lbl1sttotalgoal
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.FirstHalfFirstGoalLastGoal,
                        Name = Lang.lblfirstgoallastgoal
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.OtherBettype,
                        Name = Lang.Othertypes
                    },
                };

            if (isIncludeParlay)
            {
                bettypeList.Add(new SingbetBettype
                    {
                        Id = BettypeGroup.MixParlay,
                        Name = Lang.lblMixParlay
                    });

                bettypeList.Add(
                    new SingbetBettype
                    {
                        Id = BettypeGroup.SystemParlay,
                        Name = Lang.lblSystemParlay
                    });
            }

            return bettypeList;
        }

        /// <summary>
        /// Agents the down line path.
        /// </summary>
        /// <param name="parentId">The parent identifier.</param>
        /// <param name="agentId">The agent identifier.</param>
        /// <returns>List of agent.</returns>
        public IEnumerable<Agent> AgentDownlinePath(int parentId, int agentId)
        {
            IEnumerable<Agent> models = GetData<Agent>(new AgentDownlinePathCriteria
            {
                ParentId = parentId,
                AgentId = agentId
            });

            return models;
        }

        /// <summary>
        /// Create new Font.
        /// </summary>
        /// <param name="hssfworkbook">HSSF workbook.</param>
        /// <param name="fontColor">Color of font.</param>
        /// <param name="fontSize">Font size.</param>
        /// <param name="isBold">Is Bold or no.</param>
        /// <param name="isItalic">Is Italic or no.</param>
        /// <returns>Return New font.</returns>
        public IFont CreateFont(HSSFWorkbook hssfworkbook, short fontColor = -1, short fontSize = -1, bool isBold = false, bool isItalic = false)
        {
            var newfont = hssfworkbook.CreateFont();
            if (fontColor != -1)
            {
                newfont.Color = fontColor;
            }

            if (fontSize != -1)
            {
                newfont.FontHeightInPoints = fontSize;
            }

            if (isBold)
            {
                newfont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            }

            newfont.IsItalic = isItalic;

            return newfont;
        }

        /// <summary>
        /// Create new style.
        /// </summary>
        /// <param name="hssfworkbook">HSSF workbook.</param>
        /// <param name="backgroundColor">Back ground color.</param>
        /// <param name="align">Horizontal Alignment.</param>
        /// <param name="font">Font of style.</param>
        /// <param name="isBorder">Border or no.</param>
        /// <returns>New style.</returns>
        public ICellStyle CreateStyle(
            HSSFWorkbook hssfworkbook,
            short backgroundColor = -1,
            HorizontalAlignment align = HorizontalAlignment.Left,
            IFont font = null,
            bool isBorder = true)
        {
            ICellStyle style = hssfworkbook.CreateCellStyle();
            if (backgroundColor != -1)
            {
                style.FillForegroundColor = backgroundColor;
                style.FillPattern = FillPattern.SolidForeground;
                style.FillBackgroundColor = backgroundColor;
            }

            style.Alignment = align;
            style.VerticalAlignment = VerticalAlignment.Center;
            if (font != null)
            {
                style.SetFont(font);
            }

            if (isBorder)
            {
                style.BorderTop = BorderStyle.Thin;
                style.BorderLeft = BorderStyle.Thin;
                style.BorderRight = BorderStyle.Thin;
                style.BorderBottom = BorderStyle.Thin;
            }

            return style;
        }

        /// <summary>
        /// Create new cell with format is number.
        /// </summary>
        /// <param name="row">Row need create cell.</param>
        /// <param name="columIndex">Index of column need create cell.</param>
        /// <param name="style">Style of cell.</param>
        /// <param name="numberFormat">Format of number.</param>
        /// <param name="value">Value in cell.</param>
        public void CreateNumberCell(IRow row, int columIndex, ICellStyle style, short numberFormat, double value)
        {
            var cell = row.CreateCell(columIndex);

            cell.CellStyle = style;
            cell.SetCellType(CellType.Numeric);
            cell.CellStyle.DataFormat = numberFormat;
            cell.SetCellValue(value);
        }

        /// <summary>
        /// Create new cell with format is string.
        /// </summary>
        /// <param name="row">Row need create cell.</param>
        /// <param name="columIndex">Index of Column need create cell.</param>
        /// <param name="style">Style of cell.</param>
        /// <param name="value">Value in cell.</param>
        public void CreateStringCell(IRow row, int columIndex, ICellStyle style, string value)
        {
            var cell = row.CreateCell(columIndex);

            cell.CellStyle = style;
            cell.SetCellValue(value);
        }

        /// <summary>
        /// Customize Color of workbook.
        /// </summary>
        /// <param name="hssfworkbook">Workbook need Customize.</param>
        public void CustomizeColor(HSSFWorkbook hssfworkbook)
        {
            var palette = hssfworkbook.GetCustomPalette();

            palette.SetColorAtIndex(HSSFColor.BlueGrey.Index, (byte)141, (byte)200, (byte)245);
            palette.SetColorAtIndex(HSSFColor.Brown.Index, (byte)104, (byte)119, (byte)128);
            palette.SetColorAtIndex(HSSFColor.Red.Index, (byte)153, (byte)0, (byte)0);

            palette.SetColorAtIndex(HSSFColor.DarkRed.Index, (byte)193, (byte)0, (byte)0);
            palette.SetColorAtIndex(HSSFColor.Grey25Percent.Index, (byte)219, (byte)229, (byte)238);
            palette.SetColorAtIndex(HSSFColor.Grey50Percent.Index, (byte)196, (byte)217, (byte)238);
            palette.SetColorAtIndex(HSSFColor.Green.Index, (byte)0, (byte)128, (byte)129);
        }

        /// <summary>
        /// Get Date Format (year-month-day).
        /// </summary>
        /// <param name="dt">Object date need format.</param>
        /// <returns>Return string date.</returns>
        public string ShortDateFormat(object dt)
        {
            if (Convert.IsDBNull(dt) || dt == null)
            {
                return string.Empty;
            }

            return string.Format(new System.Globalization.CultureInfo("en-US"), "{0:yyyy-MM-dd}", dt);
        }

        /// <summary>
        /// Get Date Format (year-month-day).
        /// </summary>
        /// <param name="dt">Object date need format.</param>
        /// <returns>Return string date.</returns>
        public string LongDateFormat(object dt)
        {
            if (Convert.IsDBNull(dt) || dt == null)
            {
                return string.Empty;
            }

            return string.Format(new System.Globalization.CultureInfo("en-US"), "{0:yyyy-MM-dd h:mm:ss tt}", dt);
        }
    }
}