using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Age.Utility.NPOI;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Sunplus.Agent.Model.Entities.Permission;
using Sunplus.Agent.Utilities;

namespace Fanex.Age.Excel.Reports.Commission
{
    internal class CommissionByBetTypeReport : TemplateReport, IReportBuilder
    {
        private CommissionByBetTypeReportBuilder _builder;

        public CommissionByBetTypeReport(string filePath, CommissionByBetTypeReportBuilder builder)
            : base(filePath)
        {
            _builder = builder;
        }

        public HSSFWorkbook Build()
        {
            var data = _builder.Data;
            if (data == null) return null;

            ISheet templateSheet = hssfWorkbook.GetSheet("Template");
            ISheet resultSheet = hssfWorkbook.GetSheet("CommissionByBetType");
            // apply Bold on all fonts
            if (_builder.IsBold)
            {
                BoldAllFonts();
            }

            int rowId = 0;
            int colId = 0;
            hssfWorkbook.SetSheetName(0, Resources.Lang.lblCommissionByBettype);

            // header
            short totalColumn = 37;
            short totalRemoveColumn = 0;
            if (!UserPermission.IsShowCasino)
            {
                totalRemoveColumn++;
            }
            if (!UserPermission.IsShowLiveCasino)
            {
                totalRemoveColumn++;
            }
            if (!UserPermission.IsShowNumberGame)
            {
                totalRemoveColumn++;
            }
            if (!(UserPermission.IsShowBingo && !Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledBingoCommission))
            {
                totalRemoveColumn++;
            }
            if (!Sunplus.Agent.Model.BL.Definitions.AppSettings.IsEnableP2P)
            {
                totalRemoveColumn++;
            }
            for (int i = 0; i < totalRemoveColumn; i++)
            {
                resultSheet.RemoveColumn((short)(totalColumn - 1));
            }
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.lblCommissionByBettype + " " +
                Formater.ShortDateFormat(_builder.FromDate) + "-->" + Formater.ShortDateFormat(_builder.ToDate));
            rowId = 1;
            string currentUserType = "";
            if (_builder.RoleId == (int)(Sunplus.Agent.Model.BL.Definitions.UserTypes.Super))
                currentUserType = Resources.Lang.lblCommPTSuper;
            else if (_builder.RoleId == (int)(Sunplus.Agent.Model.BL.Definitions.UserTypes.Master))
                currentUserType = Resources.Lang.lblCommPTMaster;
            else
                currentUserType = Resources.Lang.lblCommPTAgent;
            var values = new List<object>{Resources.Lang.username,
                currentUserType + " 1", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                currentUserType + " 2", "", "", "", "", "", "", "", "", "", "", "",
                Resources.Lang.OneXTwoCommission, "",
                Resources.Lang.OtherBettypes,
                Resources.Lang.games};

            resultSheet.GetRow(rowId).WriteCells(colId, values);

            rowId = 2;
            values = new List<object>{
                Resources.Lang.hdp,
                Resources.Lang.lblOU,
                Resources.Lang.lbl1stHdp,
                Resources.Lang.lbl1stOU,
                Resources.Lang.lblOE,
                Resources.Lang.CSheet,
                Resources.Lang.DChance,
                Resources.Lang.DNB,
                Resources.Lang.HNB,
                Resources.Lang.ANB,
                Resources.Lang.DND,
                Resources.Lang.BON,
                Resources.Lang.TWinTNill,
                Resources.Lang.ThirdWayHdp,
                Resources.Lang.total,
                Resources.Lang.lbl1stCS,
                Resources.Lang.lbl1x2,
                Resources.Lang.lblCS,
                Resources.Lang.lblTG,
                Resources.Lang.lblMP,
                Resources.Lang.lblSP,
                Resources.Lang.lblOutRight,
                Resources.Lang.fglg,
                Resources.Lang.htft,
                Resources.Lang.moneyline,
                Resources.Lang.lbl1stMoneyLine,
                Resources.Lang.total,
                Resources.Lang.lbl1x2_1st,
                Resources.Lang.lbl1x2,
                string.Empty
            };

            if (UserPermission.IsShowCasino)
            {
                values.Add(Resources.Lang.casino);
            }
            if (UserPermission.IsShowLiveCasino)
            {
                values.Add(Resources.Lang.LiveCasino);
            }
            if (UserPermission.IsShowNumberGame)
            {
                values.Add(Resources.Lang.numbergame);
            }
            if (UserPermission.IsShowBingo && !Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledBingoCommission)
            {
                values.Add(Resources.Lang.bingo);
            }
            if (Sunplus.Agent.Model.BL.Definitions.AppSettings.IsEnableP2P)
            {
                values.Add(Resources.Lang.P2P);
            }

            values.Add(Resources.Lang.total);
            resultSheet.GetRow(rowId).WriteCells(colId + 1, values);

            // content
            rowId = 3;
            int id = 1;
            var totalGame = 0m;

            if (data.Count > 0)
            {
                int lastColumnIndex = colId + totalColumn - totalRemoveColumn - 1;

                foreach (var item in data)
                {
                    var sumGame = 0m;

                    if (id % 2 == 0)
                    {
                        resultSheet.CopyRow(templateSheet, 1, rowId, colId, lastColumnIndex);
                    }
                    else
                    {
                        resultSheet.CopyRow(templateSheet, 3, rowId, colId, lastColumnIndex);
                    }
                    values = new List<object> { item.Username, item.Handicap, item.OverUnder, item.FirstHalfHandicap, item.FirstHalfOverUnder,
                        item.OddEven, item.CSheet, item.DChance, item.DNB, item.HNB, item.ANB, item.DND, item.BON, item.TWinTNill, item.ThirdWayHdp, item.Total1,
                        item.FirstCS, item.OneXTwo, item.CorrectScore, item.TotalGoal, item.MixParlay, item.CombinationMixParlay, item.Outright,
                        item.FGLG, item.HTFT, item.MoneyLineFullTime, item.MoneyLineHalfTime, item.Total2, item.First1X2Comm, item.OneXTwoComm, item.Others};
                    if (UserPermission.IsShowCasino)
                    {
                        values.Add(item.Casino);
                        sumGame += item.Casino;
                    }
                    if (UserPermission.IsShowLiveCasino)
                    {
                        values.Add(item.LiveCasino);
                        sumGame += item.LiveCasino;
                    }
                    if (UserPermission.IsShowNumberGame)
                    {
                        values.Add(item.NumberGame);
                        sumGame += item.NumberGame;
                    }
                    if (UserPermission.IsShowBingo && !Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledBingoCommission)
                    {
                        values.Add(item.Bingo);
                        sumGame += item.Bingo;
                    }
                    if (Sunplus.Agent.Model.BL.Definitions.AppSettings.IsEnableP2P)
                    {
                        values.Add(item.P2P);
                        sumGame += item.P2P;
                    }

                    totalGame += sumGame;

                    values.Add(sumGame);

                    resultSheet.GetRow(rowId).WriteCells(colId, values);

                    id++;
                    rowId++;
                }

                // total
                resultSheet.CopyRow(templateSheet, 5, rowId, colId, lastColumnIndex);
                values = new List<object> {Resources.Lang.total,
                    data.Sum(p => p.Handicap), data.Sum(p => p.OverUnder), data.Sum(p => p.FirstHalfHandicap), data.Sum(p => p.FirstHalfOverUnder),
                    data.Sum(p => p.OddEven), data.Sum(p => p.CSheet), data.Sum(p => p.DChance), data.Sum(p => p.DNB),
                    data.Sum(p => p.HNB), data.Sum(p => p.ANB), data.Sum(p => p.DND), data.Sum(p => p.BON),
                    data.Sum(p => p.TWinTNill), data.Sum(p => p.ThirdWayHdp), data.Sum(p => p.Total1),
                    data.Sum(p => p.FirstCS), data.Sum(p => p.OneXTwo), data.Sum(p => p.CorrectScore), data.Sum(p => p.TotalGoal),
                    data.Sum(p => p.MixParlay), data.Sum(p => p.CombinationMixParlay), data.Sum(p => p.Outright),
                    data.Sum(p => p.FGLG), data.Sum(p => p.HTFT), data.Sum(p => p.MoneyLineFullTime),
                    data.Sum(p => p.MoneyLineHalfTime), data.Sum(p => p.Total2) };

                values.Add(data.Sum(p => p.OneXTwoComm));
                values.Add(data.Sum(p => p.First1X2Comm));
                values.Add(data.Sum(p => p.Others));

                if (UserPermission.IsShowCasino)
                {
                    values.Add(data.Sum(p => p.Casino));
                }
                if (UserPermission.IsShowLiveCasino)
                {
                    values.Add(data.Sum(p => p.LiveCasino));
                }
                if (UserPermission.IsShowNumberGame)
                {
                    values.Add(data.Sum(p => p.NumberGame));
                }
                if (UserPermission.IsShowBingo && !Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledBingoCommission)
                {
                    values.Add(data.Sum(p => p.Bingo));
                }
                if (Sunplus.Agent.Model.BL.Definitions.AppSettings.IsEnableP2P)
                {
                    values.Add(data.Sum(p => p.P2P));
                }

                values.Add(totalGame);

                resultSheet.GetRow(rowId).WriteCells(colId, values);
            }

            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return hssfWorkbook;
        }
    }
}