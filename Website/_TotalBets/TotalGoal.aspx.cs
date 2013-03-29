using System;
using System.Data;
using System.Web.UI;
using Sunplus.Agent.Utilities;

/*
 * Created 20100507@Jacob - Render TotalGoal page
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.UI.TotalBets
{
    public partial class TotalGoal_Page : TotalBets_Page
    {
        #region Override

        public override void RenderContent(HtmlTextWriter writer)
        {
            DataTable totalGoalList = View as DataTable;

            TotalGoal_Template tpl = new TotalGoal_Template();
            tpl.Contents.TotalGoalContent.Clear();

            int counter = 0;
            if (totalGoalList.Rows.Count > 0)
            {
                tpl.Contents.NotFoundContent.Hide();

                TotalGoal_Contents_TotalGoalContent_Block contentBlock = new TotalGoal_Contents_TotalGoalContent_Block();
                for (int i = 0; i < totalGoalList.Rows.Count; i++)
                {
                    DataRow dr = totalGoalList.Rows[i];

                    counter++;
                    if (totalGoalList.Columns.Contains("closed") && Convert.ToInt32(dr["closed"]) != 0)
                    {
                        contentBlock.ColorClass = Common.RowBgClosed;
                    }
                    else
                    {
                        contentBlock.ColorClass = (counter % 2) == 0 ? Common.RowBgEven : Common.RowBgOdd;
                    }

                    contentBlock.ColorOver = Common.RowBgOver;
                    contentBlock.ValueNo = counter.ToString();
                    contentBlock.ValueHome = Sunplus.Agent.Model.BL.Cache.CacheRef.GetTeamName(dr["homeid"]) + ((bool)dr["isNeutral"] ? Resources.Lang.IsNeutral : string.Empty);
                    contentBlock.ValueAway = Sunplus.Agent.Model.BL.Cache.CacheRef.GetTeamName(dr["awayid"]);
                    contentBlock.Value01 = TotalGoalRoundColor(dr["total01"]);
                    contentBlock.Value23 = TotalGoalRoundColor(dr["total23"]);
                    contentBlock.Value46 = TotalGoalRoundColor(dr["total46"]);
                    contentBlock.Value7Over = TotalGoalRoundColor(dr["total7over"]);

                    contentBlock.MatchId = dr["matchid"].ToString();
                    contentBlock.BetType = Common.BetListByBetTypeMatch;

                    tpl.Contents.TotalGoalContent.Append(contentBlock);
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
            return _TotalBetsBO.GetTotalGoal();
        }

        #endregion Override

        #region Utility

        /// <summary>
        /// Round color for total goal
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private static string TotalGoalRoundColor(object number)
        {
            decimal num = Convert.ToDecimal(number);

            return Math.Round(num) > 0 ? "<font color='red'>" + Formater.DecFormat(num, 0) + "</font>" : Formater.DecFormat(num, 0);
        }

        #endregion Utility
    }
}