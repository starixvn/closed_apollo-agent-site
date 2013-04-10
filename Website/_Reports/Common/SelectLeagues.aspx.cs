using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Text;
using Sunplus.Agent.Model.BL.Reports;
using Fanex.Age.UI.Reports.Common;

/*
 * Created 20101110@Simon - Render Monitor page
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Fanex.Age.UI.Reports.Monitor
{

    public partial class SelectLeagues_Page : WinlossReports_Page
    {
        ICollection<KeyValuePair<string, bool>> _arrSportIds = null;        
        
        public override void RenderContent(HtmlTextWriter writer)
        {            
            DataTable LeagueByGroup = View as DataTable;
            SelLeague_ContentLeague_Block tpl = new SelLeague_ContentLeague_Block();

            #region LeagueModel By Group
            if (LeagueByGroup.Rows.Count > 0)
            {
                StringBuilder strGenLeague = new StringBuilder();
                StringBuilder strLeagueSel = new StringBuilder();

                if (LeagueByGroup.Rows.Count > 1)
                {
                    SelLeague_ContentLeague_ColLeague_Block ColLeague = new SelLeague_ContentLeague_ColLeague_Block();
                    ColLeague.League = "All";
                    ColLeague.LeagueName = "<b>" + Resources.Lang.all + "</b>";
                    ColLeague.isChecked = "checked";
                    ColLeague.checkAll = "onclick='CheckSelAllLeague()'";
                    strLeagueSel.Append(ColLeague);
                }

                foreach (DataRow dr in LeagueByGroup.Rows)
                {
                    SelLeague_ContentLeague_ColLeague_Block ColLeague = new SelLeague_ContentLeague_ColLeague_Block();
                    ColLeague.League = dr["leagueid"].ToString();
                    ColLeague.LeagueName = dr["leaguename"].ToString();
                    ColLeague.isChecked = "checked";
                    ColLeague.checkAll = "onclick=isAllLeague('chL" + dr["leagueid"].ToString() + "')";
                    strLeagueSel.Append(ColLeague);
                }

                tpl.ColLeague.SetValue(strLeagueSel.ToString());
            }
            else
            {
                tpl.ColLeague.SetValue("<div class='no_info'>" + Resources.Lang.noInfo + "</div>");
            }
            #endregion

            writer.Write(tpl);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            base.ProcessRequest(ref p);

            string SHOWSB = "showsb";
            string SHOWRB = "showrb";
            string SHOWNG = "showng";
            string SHOWP2P = "showp2p";
            string SHOWLIVECASINO = "showlivecasino";

            List<KeyValuePair<string, bool>> list = new List<KeyValuePair<string, bool>>();
            if (!String.IsNullOrEmpty(Request[SHOWSB]))
                list.Add(new KeyValuePair<string, bool>(SHOWSB, Request[SHOWSB] == "1" ? true : false));
            if (!String.IsNullOrEmpty(Request[SHOWRB]))
                list.Add(new KeyValuePair<string, bool>(SHOWRB, Request[SHOWRB] == "1" ? true : false));
            if (!String.IsNullOrEmpty(Request[SHOWNG]))
                list.Add(new KeyValuePair<string, bool>(SHOWNG, Request[SHOWNG] == "1" ? true : false));
            if (!String.IsNullOrEmpty(Request[SHOWP2P]))
                list.Add(new KeyValuePair<string, bool>(SHOWP2P, Request[SHOWP2P] == "1" ? true : false));
            if (!String.IsNullOrEmpty(Request[SHOWLIVECASINO]))
                list.Add(new KeyValuePair<string, bool>(SHOWLIVECASINO, Request[SHOWLIVECASINO] == "1" ? true : false));

            _arrSportIds = list;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            ReportsBO report = new ReportsBO(CurrentUserSession.NetSessionId);
            return report.GetLeagueBySport(_dateFrom, _dateTo, _arrSportIds);
        }
    }
}