// <copyright file="BetType1000.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// <summary>
//  P2P
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.01.30">Created</change>
// </history>

namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web;

    public class BetType1000 : BetType1
    {
        public override void Assign(int index, System.Data.DataRow row)
        {
            base.Assign(index, row);

            // modify - using sessionId instead of refNo to call API service
            this.Status.StatusResult.refNo = GetValueFromDataRow(row, "betcheck").ToString();

            TransTime.Show();
            TransTimeHyperLink.Hide();
        }

        public override void AssignEvent(DataRow row)
        {
            this.Event.betTeam = Resources.Lang.p2pGame;
            this.Event.betTeamClassName = BL_FAVORITE;

            this.Event.betTypeName = null;
            this.Event.VS = null;
            this.Event.sportTypeName = null;
            this.Event.Score = null;
            this.Event.Handicap = null;

            this.Event.homeTeam = null;
            this.Event.home_firstGoal_lastGoal = null;
            this.Event.awayTeam = null;
            this.Event.away_firstGoal_lastGoal = null;
            this.Event.LeagueName.leagueName = null;

            if (this.Super != null)
            {
                this.Super.SuperPT.Visible = false;
                this.Super.SuperComm.Visible = false;
                this.Super.SuperPTValue.superPT_value = "0";
            }

            if (this.Master != null)
            {
                this.Master.MasterPT.Visible = false;
                this.Master.MasterComm.Visible = false;
                this.Master.MasterPTValue.masterPT_value = "0";
            }

            if (this.Agent != null)
            {
                this.Agent.AgentPT.Visible = false;
                this.Agent.AgentComm.Visible = false;
                this.Agent.AgentPTValue.agentPT_value = "0";
            }
        }
    }
}