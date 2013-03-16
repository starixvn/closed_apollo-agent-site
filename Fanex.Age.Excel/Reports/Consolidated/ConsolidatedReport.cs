using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Fanex.Age.Utility;
using Fanex.Age.Utility.NPOI;

/*
 * Created 20120221@Leo
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Fanex.Age.Excel.Reports.Consolidated
{
    internal class ConsolidatedReport : TemplateReport, IReportBuilder
    {
        private ConsolidatedReportBuilder _builder;

        public ConsolidatedReport(string filePath, ConsolidatedReportBuilder builder)
            : base(filePath)
        {
            _builder = builder;
        }

        public HSSFWorkbook Build()
        {
            var data = _builder.Data;
            if (data == null) return null;

            ISheet templateSheet = hssfWorkbook.GetSheet("Template");
            ISheet resultSheet = hssfWorkbook.GetSheet("Consolidated");
            // apply Bold on all fonts
            if (_builder.IsBold)
            {
                for (short i = 0; i < hssfWorkbook.NumberOfFonts; i++)
                {
                    var font = hssfWorkbook.GetFontAt(i);
                    font.Boldweight = (short)FontBoldWeight.BOLD;
                }
            }

            int rowId = 0;
            int colId = 0;
            hssfWorkbook.SetSheetName(0, CommonLibs.GetHeaderTitle(_builder.ReportType));

            // header
            string title = CommonLibs.GetHeaderTitle(_builder.ReportType);
            string headerCurrency = CommonLibs.GetHeaderCurrency(_builder.CurrencyId);
            resultSheet.GetCell(rowId, colId).SetCellValue(title + " " + Formater.ShortDateFormat(_builder.FromDate) + "-->" + Formater.ShortDateFormat(_builder.ToDate) + " - " + headerCurrency);
            rowId++;
            string breakdownTitle = Reports.CommonLibs.GetBreakDownTitle(_builder.ReportType);
            resultSheet.GetCell(rowId, colId).SetCellValue(breakdownTitle);
            resultSheet.GetRow(rowId).WriteCells(colId + 1,
                Resources.Lang.username,
                Resources.Lang.currency,
                Resources.Lang.turnover);
            resultSheet.GetCell(rowId, colId + 4).SetCellValue(Resources.Lang.member);
            resultSheet.GetCell(rowId, colId + 7).SetCellValue(Resources.Lang.owner);
            resultSheet.GetCell(rowId, colId + 10).SetCellValue(Resources.Lang.company);
            resultSheet.GetCell(rowId, colId + 11).SetCellValue(Resources.Lang.betcount);
            rowId++;
            resultSheet.GetRow(rowId).WriteCells(colId + 4,
                Resources.Lang.lblWinLoss,
                Resources.Lang.lblComm,
                Resources.Lang.total,
                Resources.Lang.lblWinLoss,
                Resources.Lang.lblComm,
                Resources.Lang.total);

            if (data.Count > 0)
            {
                rowId = 3;
                int id = 1;

                // group by BreakDown
                var breakdownList = data.Select(p => p.BreakDown).Distinct().OrderBy(p => p);
                foreach (var breakdown in breakdownList)
                {
                    var records = data.Where(p => p.BreakDown == breakdown);
                    foreach (var item in records)
                    {
                        if (id % 2 == 0)
                        {
                            resultSheet.CopyRow(templateSheet, 1, rowId);
                        }
                        else
                        {
                            resultSheet.CopyRow(templateSheet, 3, rowId);
                        }

                        string breakdownText = Reports.CommonLibs.GetBreakDownText(item.BreakDown, _builder.ReportType);
                        switch (_builder.CurrencyId)
                        {
                            case -2:
                                resultSheet.GetRow(rowId).WriteCells(colId, breakdownText, item.Username, RemotingRef.RefData.GetExchangeData(item.Currency).Currency, item.RM_Turnover,
                                    item.RM_MemberWinloss, item.RM_MemberComm, item.RM_MemberTotal,
                                    item.RM_OwnerWinloss, item.RM_OwnerComm, item.RM_OwnerTotal,
                                    item.RM_Company, item.BetCount);
                                break;
                            case -3:
                                resultSheet.GetRow(rowId).WriteCells(colId, breakdownText, item.Username, RemotingRef.RefData.GetExchangeData(item.Currency).Currency, item.USD_Turnover,
                                    item.USD_MemberWinloss, item.USD_MemberComm, item.USD_MemberTotal,
                                    item.USD_OwnerWinloss, item.USD_OwnerComm, item.USD_OwnerTotal,
                                    item.USD_Company, item.BetCount);
                                break;
                            default:
                                resultSheet.GetRow(rowId).WriteCells(colId, breakdownText, item.Username, RemotingRef.RefData.GetExchangeData(item.Currency).Currency, item.Turnover,
                                    item.MemberWinloss, item.MemberComm, item.MemberTotal,
                                    item.OwnerWinloss, item.OwnerComm, item.OwnerTotal,
                                    item.Company, item.BetCount);
                                break;
                        }

                        rowId++;
                    }
                    if (records.Count() > 1)
                    {
                        resultSheet.MergeCells(rowId - records.Count(), colId, rowId - 1, colId);
                    }

                    id++;
                }

                // total
                IEnumerable<ConsolidatedTotalEntity> query = null;
                switch (_builder.CurrencyId)
                {
                    case -2:
                        {
                            query = from item in _builder.Data
                                    group item by item.Currency into grp
                                    orderby grp.Key
                                    select new ConsolidatedTotalEntity()
                                    {
                                        Currency = grp.Key,
                                        TotalTurnover = Convert.ToDecimal(grp.Sum(p => p.RM_Turnover)),
                                        TotalMemberWinloss = Convert.ToDecimal(grp.Sum(p => p.RM_MemberWinloss)),
                                        TotalMemberComm = Convert.ToDecimal(grp.Sum(p => p.RM_MemberComm)),
                                        TotalMemberTotal = Convert.ToDecimal(grp.Sum(p => p.RM_MemberTotal)),
                                        TotalOwnerWinloss = Convert.ToDecimal(grp.Sum(p => p.RM_OwnerWinloss)),
                                        TotalOwnerComm = Convert.ToDecimal(grp.Sum(p => p.RM_OwnerComm)),
                                        TotalOwnerTotal = Convert.ToDecimal(grp.Sum(p => p.RM_OwnerTotal)),
                                        TotalCompany = Convert.ToDecimal(grp.Sum(p => p.RM_Company)),
                                        TotalBetCount = grp.Sum(p => p.BetCount)
                                    };
                        }
                        break;
                    case -3:
                        {
                            query = from item in _builder.Data
                                    group item by item.Currency into grp
                                    orderby grp.Key
                                    select new ConsolidatedTotalEntity()
                                    {
                                        Currency = grp.Key,
                                        TotalTurnover = Convert.ToDecimal(grp.Sum(p => p.USD_Turnover)),
                                        TotalMemberWinloss = Convert.ToDecimal(grp.Sum(p => p.USD_MemberWinloss)),
                                        TotalMemberComm = Convert.ToDecimal(grp.Sum(p => p.USD_MemberComm)),
                                        TotalMemberTotal = Convert.ToDecimal(grp.Sum(p => p.USD_MemberTotal)),
                                        TotalOwnerWinloss = Convert.ToDecimal(grp.Sum(p => p.USD_OwnerWinloss)),
                                        TotalOwnerComm = Convert.ToDecimal(grp.Sum(p => p.USD_OwnerComm)),
                                        TotalOwnerTotal = Convert.ToDecimal(grp.Sum(p => p.USD_OwnerTotal)),
                                        TotalCompany = Convert.ToDecimal(grp.Sum(p => p.USD_Company)),
                                        TotalBetCount = grp.Sum(p => p.BetCount)
                                    };
                        }
                        break;
                    default:
                        {
                            query = from item in _builder.Data
                                    group item by item.Currency into grp
                                    orderby grp.Key
                                    select new ConsolidatedTotalEntity()
                                    {
                                        Currency = grp.Key,
                                        TotalTurnover = grp.Sum(p => p.Turnover),
                                        TotalMemberWinloss = grp.Sum(p => p.MemberWinloss),
                                        TotalMemberComm = grp.Sum(p => p.MemberComm),
                                        TotalMemberTotal = grp.Sum(p => p.MemberTotal),
                                        TotalOwnerWinloss = grp.Sum(p => p.OwnerWinloss),
                                        TotalOwnerComm = grp.Sum(p => p.OwnerComm),
                                        TotalOwnerTotal = grp.Sum(p => p.OwnerTotal),
                                        TotalCompany = grp.Sum(p => p.Company),
                                        TotalBetCount = grp.Sum(p => p.BetCount)
                                    };

                        }
                        break;
                }
                if (query != null)
                {
                    foreach (var item in query)
                    {
                        resultSheet.CopyRow(templateSheet, 5, rowId);
                        resultSheet.GetRow(rowId).WriteCells(colId, Resources.Lang.total, "",
                            RemotingRef.RefData.GetExchangeData(item.Currency).Currency, item.TotalTurnover,
                            item.TotalMemberWinloss, item.TotalMemberComm, item.TotalMemberTotal,
                            item.TotalOwnerWinloss, item.TotalOwnerComm, item.TotalOwnerTotal,
                            item.TotalCompany, item.TotalBetCount);
                        rowId++;
                    }
                    if (query.Count() > 1)
                    {
                        resultSheet.RemoveMergedRegions(rowId - query.Count(), colId, rowId - 1, colId + 1);
                        resultSheet.MergeCells(rowId - query.Count(), colId, rowId - 1, colId + 1);
                    }
                }

                // base currency
                resultSheet.CopyRow(templateSheet, 7, rowId);
                resultSheet.GetRow(rowId).WriteCells(colId,
                    Resources.Lang.basecurrency + " " + RemotingRef.RefData.GetExchangeData(2).Currency, "", "",
                        _builder.Data.Sum(p => p.RM_Turnover),
                        _builder.Data.Sum(p => p.RM_MemberWinloss),
                        _builder.Data.Sum(p => p.RM_MemberComm),
                        _builder.Data.Sum(p => p.RM_MemberTotal),
                        _builder.Data.Sum(p => p.RM_OwnerWinloss),
                        _builder.Data.Sum(p => p.RM_OwnerComm),
                        _builder.Data.Sum(p => p.RM_OwnerTotal),
                        _builder.Data.Sum(p => p.RM_Company),
                        _builder.Data.Sum(p => p.BetCount)
                        );
                rowId++;

                resultSheet.CopyRow(templateSheet, 7, rowId);
                resultSheet.GetRow(rowId).WriteCells(colId,
                    Resources.Lang.basecurrency + " " + RemotingRef.RefData.GetExchangeData(3).Currency, "", "",
                        _builder.Data.Sum(p => p.USD_Turnover),
                        _builder.Data.Sum(p => p.USD_MemberWinloss),
                        _builder.Data.Sum(p => p.USD_MemberComm),
                        _builder.Data.Sum(p => p.USD_MemberTotal),
                        _builder.Data.Sum(p => p.USD_OwnerWinloss),
                        _builder.Data.Sum(p => p.USD_OwnerComm),
                        _builder.Data.Sum(p => p.USD_OwnerTotal),
                        _builder.Data.Sum(p => p.USD_Company),
                        _builder.Data.Sum(p => p.BetCount)
                        );
            }

            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return hssfWorkbook;
        }
    }
}
