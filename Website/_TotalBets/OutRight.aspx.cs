using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using Sunplus.Agent.Utilities;

/*
 * Created 20100507@Jacob - Render OutRight page
 * Revision:
 * ?@? - ... 
 * 
 */
namespace Fanex.Age.UI.TotalBets
{
    public partial class OutRight_Page : TotalBets_Page
    {
        #region Override

        public override void RenderContent(HtmlTextWriter writer)
        {
            DataTable outRightList = View as DataTable;

            OutRight_Template tpl = new OutRight_Template();

            tpl.Contents.LeagueContents.Clear();
            if (outRightList.Rows.Count > 0)
            {
                tpl.Contents.NotFoundContent.Hide();

                // Render league content
                int currentLeagueId = 0;
                int matchIndex = 0;
                OutRight_Contents_LeagueContents_Block leagueBlock = new OutRight_Contents_LeagueContents_Block();
                OutRight_Contents_LeagueContents_MatchContents_Block matchBlock = new OutRight_Contents_LeagueContents_MatchContents_Block();

                for (int i = 0; i < outRightList.Rows.Count; i++)
                {
                    DataRow dr = outRightList.Rows[i];

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

        
        public override object ValidateRequest(Hashtable p)
        {
            return null;
        }

        public override object InvokeData(Hashtable p)
        {
            return _TotalBetsBO.GetOutRight();
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
        private void RenderMatchRow(OutRight_Contents_LeagueContents_MatchContents_Block matchBlock, DataRow dr, int matchIndex, int rowIndex)
        {
            // Render match content
            matchBlock.ColorClass = (matchIndex % 2) == 0 ? Fanex.Age.UI.Common.RowBgEven : Fanex.Age.UI.Common.RowBgOdd;
            matchBlock.ColorOver = Common.RowBgOver;

            matchBlock.NoValue = rowIndex.ToString();
            matchBlock.TeamsValue = Sunplus.Agent.Model.BL.Cache.CacheRef.GetTeamName(dr["teamid"]);

            matchBlock.ForecastValue = PositiveNumberRoundColor(dr["totalwinlost"]);
            matchBlock.BalanceValue = Formater.FormatNumber2(dr["balance"], CurrentUserSession.Language);
            matchBlock.MatchId = dr["orid"].ToString();
            matchBlock.BetType = Common.BetListByBetTypeMatch;

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
