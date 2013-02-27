using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;

using Sunplus.Agent.Model.BL.TotalBets;
using Sunplus.Agent.Model.BL.Definitions;

using Fanex.Age.UI;

using Fanex.Age.UI.Components;

using Sunplus.Agent.Utilities;



/*
 * Created 20100507@Jacob - Render FGLG page
 * Revision:
 * ?@? - ... 
 * 
 */
namespace Fanex.Age.UI.TotalBets
{

    public partial class FGLG_Page : TotalBets_Page
    {
        #region Override
        public override void RenderContent(HtmlTextWriter writer)
        {
            DataTable FGLGList = View as DataTable;

            FGLG_Template tpl = new FGLG_Template();

            tpl.Contents.LeagueContents.Clear();
            if (FGLGList.Rows.Count > 0)
            {
                tpl.Contents.NotFoundContent.Hide();

                // Render league content
                int currentLeagueId = 0;
                int matchIndex = 0;
                FGLG_Contents_LeagueContents_Block leagueBlock = new FGLG_Contents_LeagueContents_Block();
                FGLG_Contents_LeagueContents_MatchContents_Block matchBlock = new FGLG_Contents_LeagueContents_MatchContents_Block();

                for (int i = 0; i < FGLGList.Rows.Count; i++)
                {
                    DataRow dr = FGLGList.Rows[i];

                    int leagueId = Convert.ToInt32(dr["leagueid"]);

                    if (i == 0 || currentLeagueId != leagueId)
                    {
                        // New league, ignore first row
                        if (i != 0) tpl.Contents.LeagueContents.Append(leagueBlock);

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

                if (leagueBlock != null) tpl.Contents.LeagueContents.Append(leagueBlock);
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
            return _TotalBetsBO.GetFGLG();
        }
        #endregion

        #region Utility

        /// <summary>
        /// Render match row
        /// </summary>
        /// <param name="matchBlock"></param>
        /// <param name="dr"></param>
        /// <param name="matchIndex"></param>
        /// <param name="rowIndex"></param>
        private void RenderMatchRow(FGLG_Contents_LeagueContents_MatchContents_Block matchBlock, DataRow dr, int matchIndex, int rowIndex)
        {
            matchBlock.ColorOver = Fanex.Age.UI.Common.RowBgOver;
            matchBlock.ColorClass = (matchIndex % 2) == 0 ? Fanex.Age.UI.Common.RowBgEven : Fanex.Age.UI.Common.RowBgOdd;
            matchBlock.NoValue = rowIndex.ToString();
            matchBlock.TimeValue = dr["showtime"].ToString();
            matchBlock.HomeValue = Sunplus.Agent.Model.BL.Cache.CacheRef.GetTeamName(dr["homeid"]) + ((bool)dr["isNeutral"] ? Resources.Lang.IsNeutral : string.Empty);
            matchBlock.AwayValue = Sunplus.Agent.Model.BL.Cache.CacheRef.GetTeamName(dr["awayid"]);
            matchBlock.FirstHomeValue = PositiveNumberRoundColor(dr["winlost_homeFG"]);
            matchBlock.LastHomeValue = PositiveNumberRoundColor(dr["winlost_homeLG"]);
            matchBlock.FirstAwayValue = PositiveNumberRoundColor(dr["winlost_awayFG"]);
            matchBlock.LastAwayValue = PositiveNumberRoundColor(dr["winlost_awayLG"]);
            matchBlock.NoGoalValue = PositiveNumberRoundColor(dr["winlost_NoGoal"]);
            matchBlock.MatchId = dr["matchid"].ToString();
            matchBlock.BetType = Fanex.Age.UI.Common.BetListByBetTypeMatch;
        }
        /// <summary>
        /// Round color for positive number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private string PositiveNumberRoundColor(object number)
        {
            decimal num = Convert.ToDecimal(number);
            return (num > 0) ? "<font color='red'>" + Formater.FormatNumber2(num, CurrentUserSession.Language) + "</font>" : Formater.FormatNumber2(num, CurrentUserSession.Language);
        }
        #endregion
    }

}
