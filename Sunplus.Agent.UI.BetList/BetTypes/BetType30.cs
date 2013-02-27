using System;
using System.Collections.Generic;

namespace Sunplus.Agent.UI.BetList.BetTypes
{
    /// <summary>
    /// 1st Correct score
    /// </summary>
    public class BetType30 : BetType4
    {
        public override void AssignEvent(System.Data.DataRow row)
        {
            base.AssignEvent(row);

            string choice = row["betteam"] as string;
            choice = (choice == "0:4" || choice == "4:0") ? choice + " UP" : choice;

            // "&nbsp" because export format
            this.Event.betTeam = "&nbsp;" + choice;
        }
    }
}
