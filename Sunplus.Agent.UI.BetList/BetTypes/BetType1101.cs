// <copyright file="BetType1101.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved. 
// </copyright>
//
// <summary>
//  LiveCasino Baccarat
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.01.18">Created</change>
// </history>

namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System.Data;
    using Sunplus.Agent.UI.BetList.Utils;

    public class BetType1101 : BetType1
    {
        public override void Assign(int index, System.Data.DataRow row)
        {
            base.Assign(index, row);
            TransTime.Show();
            TransTimeHyperLink.Hide();
            odds = "-";
        }

        public override void RenderBetTeam(Excel.BetTypeExcelData excelData)
        {
            if (OutDataTable != null && OutDataTable.Rows.Count > 0)
            {
                DataRow[] foundRows = OutDataTable.Select("refno=" + excelData.DataRow["refno"].ToString());
                if (foundRows != null && foundRows.Length > 0)
                {
                    Event.betTeam = string.Empty;
                    for (int i = 0; i < foundRows.Length; i++)
                    {
                        string betTeam = Sunplus.Agent.Model.BL.Cache.CacheRef.GetCasinoBetItem(GetValueFromDataRow(foundRows[i], "betteam"));
                        string odds = ConvertByBetType.Odds(GetValueFromDataRow(foundRows[i], "odds", 0), BetTypeID, GetValueFromDataRow(excelData.DataRow, "oddstype", 0));
                        string stake = ConvertByBetType.Stake(GetValueFromDataRow(foundRows[i], "stake"));
                        excelData.RtfTextRender.AddText(betTeam, excelData.PosFont);
                        excelData.RtfTextRender.AddText(string.Format(" {0} @ {1}\n", stake, odds));
                    }
                }
            }
        }

        public override void AssignEvent(System.Data.DataRow row)
        {
            base.AssignEvent(row);
            Event.LeagueName.leagueName = string.Format("{0} {1}", Resources.Lang.baccarat, row["showtime"].ToString());

            if (OutDataTable != null && OutDataTable.Rows.Count > 0)
            {
                DataRow[] foundRows = OutDataTable.Select("refno=" + row["refno"].ToString());

                if (foundRows != null && foundRows.Length > 0)
                {
                    for (int i = 0; i < foundRows.Length; i++)
                    {
                        string betTeam = Sunplus.Agent.Model.BL.Cache.CacheRef.GetCasinoBetItem(GetValueFromDataRow(foundRows[i], "betteam"));
                        string odds = ConvertByBetType.Odds(GetValueFromDataRow(foundRows[i], "odds", 0), BetTypeID, GetValueFromDataRow(row, "oddstype", 0));
                        Event.betTeam += string.Format("<div><span class='bl_favorite'>{0}</span> {1} @ <span class='colorHandicap b'>{2}</span></div>", betTeam, ConvertByBetType.Stake(GetValueFromDataRow(foundRows[i], "stake")), odds);
                        Event.Handicap.Hide();
                        Event.betTypeName = string.Empty;
                        Event.betTeamClassName = string.Empty;
                        Event.VS.Hide();
                    }
                }
            }
            else
            {
                Event.betTeam = string.Empty;
                Event.Handicap.Hide();
                Event.betTypeName = Resources.Lang.baccarat;
                Event.betTeamClassName = string.Empty;
                Event.VS.Hide();
            }
        }
    }
}