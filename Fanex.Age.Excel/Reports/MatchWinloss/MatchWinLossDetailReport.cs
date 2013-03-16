namespace Fanex.Age.Excel.Reports.MatchWinloss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Age.DTO.Reports.MatchWinloss;
    using Fanex.Age.Utility.NPOI;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using Fanex.Age.Utility;

    internal class MatchWinLossDetailReport : TemplateReport, IReportBuilder
    {
        private MatchWinLossDetailReportBuilder _builder;

        public MatchWinLossDetailReport(string filePath, MatchWinLossDetailReportBuilder builder)
            : base(filePath)
        {
            _builder = builder;
        }

        public HSSFWorkbook Build()
        {
            var data = _builder.Data;
            if (data == null) return null;

            ISheet templateSheet = hssfWorkbook.GetSheet("Template");
            ISheet resultSheet = hssfWorkbook.GetSheet("MatchWinLossDetail");

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
            bool checkSuper = (_builder.IsSuper == true);
            bool checkMaster = (_builder.IsSuper == true || _builder.IsMaster == true);
            bool checkAgent = (_builder.IsSuper == true || _builder.IsMaster == true || _builder.IsAgent == true);

            hssfWorkbook.SetSheetName(0, Resources.Lang.lblMatchWinLostDetail);
            // header
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.lblMatchWinLostDetail + " " + Formater.ShortDateFormat(_builder.FromDate) + "-->" + Formater.ShortDateFormat(_builder.ToDate));
            rowId = 1;
            var values = new List<object>{Resources.Lang.bettype,
                Resources.Lang.turnover,
                Resources.Lang.grosscomm,
                Resources.Lang.member, "", ""};
            if (checkAgent)
                values.AddRange(new List<string> { Resources.Lang.agent, "", "" });
            if (checkMaster)
                values.AddRange(new List<string> { Resources.Lang.master, "", "" });
            if (checkSuper)
                values.AddRange(new List<string> { Resources.Lang.super, "", "" });
            values.Add(Resources.Lang.company);
            resultSheet.GetRow(rowId).WriteCells(colId, values);
            rowId = 2;
            values = new List<object>{Resources.Lang.win,
                Resources.Lang.com,
                Resources.Lang.total};
            if (checkAgent)
                values.AddRange(new List<string> {Resources.Lang.win,
                Resources.Lang.com,
                Resources.Lang.total});
            if (checkMaster)
                values.AddRange(new List<string> {Resources.Lang.win,
                Resources.Lang.com,
                Resources.Lang.total});
            if (checkSuper)
                values.AddRange(new List<string> {Resources.Lang.win,
                Resources.Lang.com,
                Resources.Lang.total});
            values.AddRange(new List<string> {Resources.Lang.com,
                Resources.Lang.total});
            resultSheet.GetRow(rowId).WriteCells(colId + 3, values);

            // all content
            rowId = 3;
            if (data.Count > 0)
            {
                int id = 1;
                MatchWinLossDetailItem parlayItem = null;
                MatchWinLossDetailItem combineMixParlayItem = null;
                int rowCount = data.Count();
                var matchIds = data.Select(p => p.MatchId).Distinct();
                foreach (var matchId in matchIds)
                {
                    string matchName = "";

                    // matchId = 0 is used for special betgroups (2 = Outright, 3 = Parlay)
                    var items = data.Where(p => p.MatchId == matchId);
                    if (items.Count() > 0)
                    {
                        var firstItem = items.First();

                        // show League Name & Match Name
                        string betTypeName = Sunplus.Agent.Model.BL.Cache.CacheRef.GetBettype(firstItem.BetType);
                        string leagueName = Sunplus.Agent.Model.BL.Cache.CacheRef.GetLeagueName(firstItem.LeagueId);
                        switch (firstItem.BetType)
                        {
                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Outright: // 10
                                {
                                    leagueName = betTypeName;
                                }
                                break;

                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Win: // 31
                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Place: // 32
                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Win_Place: // 33
                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Win_UK_Tote: // 41
                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Place_UK_Tote: // 42
                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Win_Place_UK_Tote: // 43
                                {
                                    matchName = Resources.Lang.lblRace + firstItem.Race;
                                    leagueName = Resources.Lang.hr + " - " + leagueName;
                                }
                                break;

                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType._5_min_O_U: // financial 51
                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType._15_min_O_U: // financial 52
                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType._30_min_O_U: // financial 53
                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType._45_min_O_U: // financial 54
                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType._60_min_O_U: // financial 55
                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType.End_of_day_O_U: // financial 56
                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType._5_min_OE: // financial 61
                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType._15_min_OE: // financial 62
                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType._30_min_OE: // financial 63
                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType._45_min_OE: // financial 64
                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType._60_min_OE: // financial 65
                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType.End_of_day_OE: // financial 66
                                {
                                    string leagueNameTitle = Resources.Lang.Financial;
                                    if (!String.IsNullOrEmpty(leagueName)) leagueNameTitle += "-" + leagueName;

                                    string indexName = Sunplus.Agent.Model.BL.Cache.CacheRef.GetFncIndexName(firstItem.HomeId);
                                    String title = leagueName + indexName;

                                    matchName = indexName;
                                    leagueName = leagueNameTitle;
                                }
                                break;

                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType._1st_Ball_OU: // 81
                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Last_Ball_OU: // 82
                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType._1st_Ball_OE: // 83
                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Last_Ball_OE: // 84
                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType.N_Over_Under: //85
                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType.N_Odd_Even: // 86
                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Next_High_Low: // 87
                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Warrior: // 88
                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Next_Combo: // 89
                                {
                                    string leagueNameTitle = Resources.Lang.numbergame;
                                    if (!string.IsNullOrEmpty(leagueName)) leagueNameTitle += "-" + leagueName;
                                    leagueName = leagueNameTitle;

                                    string gameId = Resources.Lang.numbergameno + " " + firstItem.MatchCode;
                                    String title = leagueName + gameId;

                                    matchName = gameId;
                                    leagueName = leagueNameTitle;
                                }
                                break;

                            // For Live Casino
                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Baccarat: // 1101
                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Roulette: // 1102
                            case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Sicbo: // 1103
                                {
                                    string leagueNameTitle = Resources.Lang.LiveCasino;
                                    String title = betTypeName + " " + firstItem.ShowTime;

                                    matchName = title;
                                    leagueName = leagueNameTitle;
                                }
                                break;
                            default:
                                {
                                    String homeName = Sunplus.Agent.Model.BL.Cache.CacheRef.GetTeamName(firstItem.HomeId) + (firstItem.IsNeutral ? Resources.Lang.IsNeutral : string.Empty);
                                    String awayName = Sunplus.Agent.Model.BL.Cache.CacheRef.GetTeamName(firstItem.AwayId);
                                    matchName = homeName + "-vs-" + awayName;
                                }
                                break;
                        }
                        if (matchId != 0)
                        {
                            resultSheet.CopyRow(templateSheet, 1, rowId);
                            string text = leagueName;
                            text = leagueName + " >>> " + matchName;
                            resultSheet.GetRow(rowId).WriteCells(colId, text.ToUpper());
                            rowId++;
                        }

                        // show content
                        foreach (var item in items)
                        {
                            if (item.BetGroup == 3)
                            {
                                if (item.BetType == (int)Sunplus.Agent.Model.BL.Definitions.BetType.Mix_Parlay)
                                {
                                    parlayItem = item;
                                    continue;
                                }
                                else if (item.BetType == (int)Sunplus.Agent.Model.BL.Definitions.BetType.Combine_Mix_Parlay)
                                {
                                    combineMixParlayItem = item;
                                    continue;
                                }
                            }

                            if (id % 2 == 0)
                            {
                                resultSheet.CopyRow(templateSheet, 3, rowId);
                            }
                            else
                            {
                                resultSheet.CopyRow(templateSheet, 5, rowId);
                            }

                            betTypeName = Sunplus.Agent.Model.BL.Cache.CacheRef.GetBettype(item.BetType);
                            string strTypeNameNG = "";
                            switch (item.BetType)
                            {
                                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.N_Over_Under: //85
                                    strTypeNameNG = Resources.Lang.next + " ";
                                    break;

                                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.N_Odd_Even: // 86
                                    if (item.LiveIndicator == 1)
                                        strTypeNameNG = Resources.Lang.next + " ";
                                    else
                                        strTypeNameNG = Resources.Lang.ft + ". ";
                                    break;
                            }
                            values = new List<object>{strTypeNameNG + betTypeName, item.Turnover, item.GrossComm,
                                item.MemberWinloss, item.MemberComm, item.MemberTotal};
                            if (checkAgent)
                                values.AddRange(new List<object> { item.AgentWinloss, item.AgentComm, item.AgentTotal });
                            if (checkMaster)
                                values.AddRange(new List<object> { item.MasterWinloss, item.MasterComm, item.MasterTotal });
                            if (checkSuper)
                                values.AddRange(new List<object> { item.SuperWinloss, item.SuperComm, item.SuperTotal });
                            values.AddRange(new List<object> { item.CompanyComm, item.CompanyTotal });
                            resultSheet.GetRow(rowId).WriteCells(colId, values);

                            id++;
                            rowId++;
                        }

                        // show total
                        if (matchId != 0)
                        {
                            resultSheet.CopyRow(templateSheet, 7, rowId);
                            values = new List<object>{Resources.Lang.subTotal,
                            items.Sum(p => p.Turnover),
                            items.Sum(p => p.GrossComm),
                            items.Sum(p => p.MemberWinloss),
                            items.Sum(p => p.MemberComm),
                            items.Sum(p => p.MemberTotal)};
                            if (checkAgent)
                                values.AddRange(new List<object> { items.Sum(p => p.AgentWinloss),
                            items.Sum(p => p.AgentComm),
                            items.Sum(p => p.AgentTotal) });
                            if (checkMaster)
                                values.AddRange(new List<object> { items.Sum(p => p.MasterWinloss),
                            items.Sum(p => p.MasterComm),
                            items.Sum(p => p.MasterTotal) });
                            if (checkSuper)
                                values.AddRange(new List<object> { items.Sum(p => p.SuperWinloss),
                            items.Sum(p => p.SuperComm),
                            items.Sum(p => p.SuperTotal) });
                            values.AddRange(new List<object> { items.Sum(p => p.CompanyComm),
                            items.Sum(p => p.CompanyTotal)});
                            resultSheet.GetRow(rowId).WriteCells(colId, values);
                            rowId++;
                        }
                    }
                }

                // total values wihout parlay
                resultSheet.CopyRow(templateSheet, 9, rowId);
                var newItems = data.Where(p => p.BetGroup != 3);
                values = new List<object> {Resources.Lang.total,
                    newItems.Sum(p => p.Turnover),
                    newItems.Sum(p => p.GrossComm),
                    newItems.Sum(p => p.MemberWinloss),
                    newItems.Sum(p => p.MemberComm),
                    newItems.Sum(p => p.MemberTotal) };
                if (checkAgent)
                    values.AddRange(new List<object> { newItems.Sum(p => p.AgentWinloss),
                            newItems.Sum(p => p.AgentComm),
                            newItems.Sum(p => p.AgentTotal) });
                if (checkMaster)
                    values.AddRange(new List<object> { newItems.Sum(p => p.MasterWinloss),
                            newItems.Sum(p => p.MasterComm),
                            newItems.Sum(p => p.MasterTotal) });
                if (checkSuper)
                    values.AddRange(new List<object> { newItems.Sum(p => p.SuperWinloss),
                            newItems.Sum(p => p.SuperComm),
                            newItems.Sum(p => p.SuperTotal) });
                values.AddRange(new List<object> { newItems.Sum(p => p.CompanyComm),
                            newItems.Sum(p => p.CompanyTotal)});
                resultSheet.GetRow(rowId).WriteCells(colId, values);
                rowId++;

                // show parlay
                if (parlayItem != null || combineMixParlayItem != null)
                {
                    var parlayItems = new List<MatchWinLossDetailItem>();
                    if (parlayItem != null) parlayItems.Add(parlayItem);
                    if (combineMixParlayItem != null) parlayItems.Add(combineMixParlayItem);
                    foreach (var item in parlayItems)
                    {
                        if (item != null)
                        {
                            if (id % 2 == 0)
                            {
                                resultSheet.CopyRow(templateSheet, 3, rowId);
                            }
                            else
                            {
                                resultSheet.CopyRow(templateSheet, 5, rowId);
                            }

                            string betTypeName = "";
                            switch (item.BetType)
                            {
                                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Mix_Parlay: // 9
                                    betTypeName = Resources.Lang.lblMixParlay;
                                    break;

                                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Combine_Mix_Parlay: // 29
                                    betTypeName = Resources.Lang.lblSystemParlay;
                                    break;
                            }

                            values = new List<object>{betTypeName, item.Turnover, item.GrossComm,
                                item.MemberWinloss, item.MemberComm, item.MemberTotal};
                            if (checkAgent)
                                values.AddRange(new List<object> { item.AgentWinloss, item.AgentComm, item.AgentTotal });
                            if (checkMaster)
                                values.AddRange(new List<object> { item.MasterWinloss, item.MasterComm, item.MasterTotal });
                            if (checkSuper)
                                values.AddRange(new List<object> { item.SuperWinloss, item.SuperComm, item.SuperTotal });
                            values.AddRange(new List<object> { item.CompanyComm, item.CompanyTotal });
                            resultSheet.GetRow(rowId).WriteCells(colId, values);

                            rowId++;
                            id++;
                        }
                    }

                    // show total with parlay
                    resultSheet.CopyRow(templateSheet, 9, rowId);
                    newItems = newItems.Union(parlayItems);
                    values = new List<object> {Resources.Lang.grandTotal,
                    newItems.Sum(p => p.Turnover),
                    newItems.Sum(p => p.GrossComm),
                    newItems.Sum(p => p.MemberWinloss),
                    newItems.Sum(p => p.MemberComm),
                    newItems.Sum(p => p.MemberTotal) };
                    if (checkAgent)
                        values.AddRange(new List<object> { newItems.Sum(p => p.AgentWinloss),
                            newItems.Sum(p => p.AgentComm),
                            newItems.Sum(p => p.AgentTotal) });
                    if (checkMaster)
                        values.AddRange(new List<object> { newItems.Sum(p => p.MasterWinloss),
                            newItems.Sum(p => p.MasterComm),
                            newItems.Sum(p => p.MasterTotal) });
                    if (checkSuper)
                        values.AddRange(new List<object> { newItems.Sum(p => p.SuperWinloss),
                            newItems.Sum(p => p.SuperComm),
                            newItems.Sum(p => p.SuperTotal) });
                    values.AddRange(new List<object> { newItems.Sum(p => p.CompanyComm),
                            newItems.Sum(p => p.CompanyTotal)});
                    resultSheet.GetRow(rowId).WriteCells(colId, values);

                    rowId++;
                }
            }

            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return hssfWorkbook;
        }
    }
}