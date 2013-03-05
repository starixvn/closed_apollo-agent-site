// --------------------------------------------------------------------------
// <copyright file="MoneyLine.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.3.5">Created</change>
// </history>

namespace Fanex.Age.UI.TotalBets
{
    using System;
    using System.Data;
    using System.Web.UI;
    using Sunplus.Agent.Utilities;

    public partial class MoneyLine_Page : TotalBets_Page
    {
        #region Override

        public override void RenderContent(HtmlTextWriter writer)
        {
            DataTable moneyLineList = View as DataTable;

            MoneyLine_Template tpl = new MoneyLine_Template();
            tpl.Contents.LeagueContents.Clear();

            if (moneyLineList.Rows.Count > 0)
            {
                tpl.Contents.MoneyLineNotFoundContent.Clear();

                // Render league content
                int currentLeagueId = 0;
                int matchIndex = 0;
                MoneyLine_Contents_LeagueContents_Block leagueBlock = new MoneyLine_Contents_LeagueContents_Block();
                MoneyLine_Contents_LeagueContents_MatchContents_Block matchBlock = new MoneyLine_Contents_LeagueContents_MatchContents_Block();

                for (int i = 0; i < moneyLineList.Rows.Count; i++)
                {
                    DataRow dr = moneyLineList.Rows[i];

                    int leagueId = Convert.ToInt32(dr["leagueid"]);

                    if (i == 0 || currentLeagueId != leagueId)
                    {
                        // New league, ignore first row
                        if (i != 0)
                        {
                            tpl.Contents.LeagueContents.Append(leagueBlock);
                        }

                        matchIndex = 1;
                        currentLeagueId = leagueId;

                        leagueBlock.MatchContents.Clear();
                        leagueBlock.LeagueName = Sunplus.Agent.Model.BL.Cache.CacheRef.GetLeagueName(leagueId);
                    }
                    else
                    {
                        matchIndex++;
                    }

                    RenderMatchRow(matchBlock, dr, matchIndex, i + 1);

                    leagueBlock.MatchContents.Append(matchBlock);
                }

                if (leagueBlock != null)
                {
                    tpl.Contents.LeagueContents.Append(leagueBlock);
                }
            }

            if (_fullMode)
            {
                writer.Write(tpl);
            }
            else
            {
                writer.Write(tpl.Contents);
            }
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            return _TotalBetsBO.GetMoneyLine();
        }
        #endregion

        #region Utility
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string RoundPositiveColor(object number)
        {
            decimal num = Convert.ToDecimal(number);
            return (num > 0) ? "<font color='red'>" + Formater.FormatNumber2(num, CurrentUserSession.Language) + "</font>" : Formater.FormatNumber2(num, CurrentUserSession.Language);
        }

        /// <summary>
        /// Render match row
        /// </summary>
        /// <param name="matchBlock"></param>
        /// <param name="dr"></param>
        /// <param name="matchIndex"></param>
        /// <param name="rowIndex"></param>
        private void RenderMatchRow(MoneyLine_Contents_LeagueContents_MatchContents_Block matchBlock, DataRow dr, int matchIndex, int rowIndex)
        {
            //// Render match content

            if (dr.Table.Columns.Contains("closed") && Convert.ToInt32(dr["closed"]) != 0)
            {
                matchBlock.ColorClass = Fanex.Age.UI.Common.RowBgClosed;
            }
            else if (matchIndex % 2 == 0)
            {
                matchBlock.ColorClass = Fanex.Age.UI.Common.RowBgEven;
            }
            else
            {
                matchBlock.ColorClass = Fanex.Age.UI.Common.RowBgOdd;
            }

            matchBlock.ColorOver = Fanex.Age.UI.Common.RowBgOver;
            matchBlock.NoValue = rowIndex.ToString();
            matchBlock.TimeValue = dr["showtime"].ToString();
            matchBlock.Home = Sunplus.Agent.Model.BL.Cache.CacheRef.GetTeamName(dr["homeid"]) + ((bool)dr["isneutral"] ? Resources.Lang.IsNeutral : string.Empty);
            matchBlock.Away = Sunplus.Agent.Model.BL.Cache.CacheRef.GetTeamName(dr["awayid"]);
            matchBlock.TotalHomeFullTime = Formater.FormatNumber2(dr["home_odds"], CurrentUserSession.Language);
            matchBlock.TotalAwayFullTime = Formater.FormatNumber2(dr["away_odds"], CurrentUserSession.Language);
            matchBlock.ForecastHomeFullTime = RoundPositiveColor(dr["winlost_home"]);
            matchBlock.ForecastAwayFullTime = RoundPositiveColor(dr["winlost_away"]);
            matchBlock.TotalHomeHalfTime = Formater.FormatNumber2(dr["home_odds_HT"], CurrentUserSession.Language);
            matchBlock.TotalAwayHalfTime = Formater.FormatNumber2(dr["away_odds_HT"], CurrentUserSession.Language);
            matchBlock.ForecastHomeHalfTime = RoundPositiveColor(dr["winlost_home_HT"]);
            matchBlock.ForecastAwayHalfTime = RoundPositiveColor(dr["winlost_away_HT"]);
            matchBlock.MatchId = dr["matchid"].ToString();
            matchBlock.BetType = Fanex.Age.UI.Common.BetListByBetTypeMatch;
        }
        #endregion
    }
}
