// --------------------------------------------------------------------------
// <copyright file="FC1X2.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.3.5">Created</change>
// </history>

namespace Fanex.Age.UI.Forecast
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web.UI;
    using Sunplus.Agent.Model.BL.Forecast;
    using Sunplus.Agent.Utilities;

    public partial class FC1X2_Page : Fanex.Age.UI.TplPage
    {
        #region Const
        private const string RUNNING_LEAGUES = "RunningLeague";
        private const string FC_DATA = "FCData";
        #endregion

        #region Variable
        private int _matchId = 0;
        private int _leagueId = 0;
        private int _betType = 0;
        private bool _fullMode = true;
        private bool _isPopup = false;
        #endregion

        #region Override
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderContent(HtmlTextWriter writer)
        {
            DataTable fcData = (View as Dictionary<string, DataTable>)[FC_DATA];
            FC1X2_Template tpl = new FC1X2_Template();

            RenderLeagueList(tpl.Content);
            //// Render row content
            FC1X2_Content_RowContent_Block block = new FC1X2_Content_RowContent_Block();
            tpl.Content.RowContent.Clear();
            if (fcData != null && fcData.Rows.Count > 0)
            {
                for (int i = 0; i < fcData.Rows.Count; i++)
                {
                    DataRow dr = fcData.Rows[i];
                    block.Home = Sunplus.Agent.Model.BL.Cache.CacheRef.GetTeamName(dr["homeid"]) + ((bool)dr["isNeutral"] ? Resources.Lang.IsNeutral : string.Empty);
                    block.Away = Sunplus.Agent.Model.BL.Cache.CacheRef.GetTeamName(dr["awayid"]);

                    if (Convert.ToInt16(dr["liveindicator"]) == 1)
                    {
                        block.Time = dr["livehomescore"].ToString() + " - " + dr["liveawayscore"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;<i><font style='color:red;'>" + Resources.Lang.lblLive + "</font></i>";
                    }
                    else
                    {
                        block.Time = dr["showtime"].ToString();
                    }

                    block.MatchId = dr["matchid"].ToString();

                    block.HomeWinLose = Convert.IsDBNull(dr["hl"]) ? string.Empty : RedForPositiveAmount(dr["hl"], 0);
                    block.DrawWinLose = Convert.IsDBNull(dr["dl"]) ? string.Empty : RedForPositiveAmount(dr["dl"], 0);
                    block.AwayWinLose = Convert.IsDBNull(dr["al"]) ? string.Empty : RedForPositiveAmount(dr["al"], 0);
                    block.HTHomeWinLose = Convert.IsDBNull(dr["ht_hl"]) ? string.Empty : RedForPositiveAmount(dr["ht_hl"], 0);
                    block.HTDrawWinLose = Convert.IsDBNull(dr["ht_dl"]) ? string.Empty : RedForPositiveAmount(dr["ht_dl"], 0);
                    block.HTAwayWinLose = Convert.IsDBNull(dr["ht_al"]) ? string.Empty : RedForPositiveAmount(dr["ht_al"], 0);

                    tpl.Content.RowContent.Append(block);
                }

                tpl.Content.NotFoundContent.Hide();
            }

            if (_isPopup)
            {
                tpl.TitleHeader.Hide();
                tpl.TitleFooter.Hide();
            }

            // Table width
            //tpl.Content.TableWidth = _isPopup ? "100%" : "600px";
            //tpl.Content.LimitLeagueList.TableWidth = _isPopup ? "100%" : "577px";
            //// Response
            if (_fullMode)
            {
                writer.Write(tpl);
            }
            else
            {
                writer.Write(tpl.Content);
            }
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            p.AddHttpParameters(
                new string[] { "matchid", "leagueid", "bettype" }, 
                Request);

            _fullMode = string.IsNullOrEmpty(Request["full_mode"]) ? true : Convert.ToInt32(Request["full_mode"]) > 0;
            _isPopup = string.IsNullOrEmpty(Request["is_popup"]) ? false : Convert.ToInt32(Request["is_popup"]) > 0;
            _leagueId = Convert.ToInt32(p["leagueid"]);
            _matchId = Convert.ToInt32(p["matchid"]);
            _betType = Convert.ToInt32(p["bettype"]);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            Dictionary<string, DataTable> view = new Dictionary<string, DataTable>();
            ForecastBO bo = new ForecastBO(CurrentUserSession.NetSessionId);

            DataTable fcData = null;
            DataTable runningLeagues = null;
            //// Get default leagueid
            if (_matchId > 0)
            {
                fcData = bo.GetFC1X2ByMatch(_matchId, _betType);
            }
            else
            {
                runningLeagues = bo.GetFCRunningLeagues(5); // 5: for 1x2
                if (_leagueId == 0 && runningLeagues.Rows.Count > 0)
                {
                    _leagueId = Convert.ToInt32(runningLeagues.Rows[0]["leagueid"]);
                }

                if (_leagueId > 0)
                {
                    fcData = bo.GetFC1X2ByLeague(_leagueId);
                }
            }

            view.Add(RUNNING_LEAGUES, runningLeagues);
            view.Add(FC_DATA, fcData);
            return view;
        }
        #endregion

        #region Render Utility
        /// <summary>
        /// Round color for negative number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private string RedForPositiveAmount(object amount, byte decDigits)
        {
            if ((decimal)amount > 0)
            {
                return "<font color='red'>" + Formater.DecFormat(amount, decDigits) + "</font>";
            }
            else
            {
                return Formater.DecFormat(amount, decDigits);
            }
        }

        private void RenderLeagueList(FC1X2_Content_Block content)
        {
            DataTable leagues = (View as Dictionary<string, DataTable>)[RUNNING_LEAGUES];

            if (leagues != null && leagues.Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (DataRow dr in leagues.Rows)
                {
                    int leagueId = Convert.ToInt32(dr["leagueid"]);
                    string leagueName = Sunplus.Agent.Model.BL.Cache.CacheRef.GetLeagueName(dr["leagueid"]);
                    if (leagueId == _leagueId)
                    {
                        sb.Append(string.Format("<option value='{0}' selected>{1}</option>", leagueId, leagueName));
                    }
                    else
                    {
                        sb.Append(string.Format("<option value='{0}'>{1}</option>", leagueId, leagueName));
                    }
                }

                content.LimitLeagueList.LeagueList = sb.ToString();
            }
            else
            {
                content.LimitLeagueList.Hide();
            }
        }
        #endregion
    }
}
