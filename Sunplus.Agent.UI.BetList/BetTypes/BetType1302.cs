/// Match Correct Score inherit from Handicap
namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System;
    using System.Data;
    using Sunplus.Agent.Model.BL.Cache;

    /// <summary>
    /// Match Correct Score
    /// </summary>
    public class BetType1302 : BetType1
    {
        public override void Assign(int index, System.Data.DataRow row)
        {
            base.Assign(index, row);

            // modify            
            TransTime.Show();
            TransTimeHyperLink.Hide();
        }

        public override void AssignEvent(System.Data.DataRow row)
        {
            base.AssignEvent(row);
            
            string choice = row["betteam"] as string;
            var betTeam = string.Empty;
            if (choice.Length == 2)
            {
                betTeam = choice.Substring(0, 1) + ":" + choice.Substring(1, 1);
            }

            // "&nbsp" because export format
            this.Event.betTeam = "&nbsp;" + betTeam;
            this.Event.Handicap = null;
            this.Event.betTypeName = CacheRef.GetBettype(Convert.ToInt32(this.BetTypeID), row["betid"]);
        }
    }
}
