namespace Sunplus.Agent.UI.BetList.BetTypes
{
    using System.Data;

    /// <summary>
    /// P2P Transfer
    /// </summary>
    public class BetType23101 : BetType1
    {
        public override void Assign(int index, System.Data.DataRow row)
        {
            base.Assign(index, row);

            this.TransTime.refNo = GetValueFromDataRow(row, "refno").ToString();
            this.Status.StatusResult.refNo = this.TransTime.refNo;

            this.TransTime.Show();
            this.TransTimeHyperLink.Hide();
        }

        public override void AssignEvent(DataRow row)
        {
            base.AssignEvent(row);

            this.Event.betTeam = Resources.Lang.p2pGame + " - " + Resources.BetList.transfer;
            this.Event.betTeamClassName = BetType1.BL_FAVORITE;

            this.Event.betTypeName = null;
            this.Event.VS = null;
            this.Event.sportTypeName = null;
            this.Event.Score = null;
            this.Event.Handicap = null;

            if (this.Super != null)
            {
                this.Super.SuperPT.Visible = false;
                this.Super.SuperComm.Visible = false;
            }

            if (this.Master != null)
            {
                this.Master.MasterPT.Visible = false;
                this.Master.MasterComm.Visible = false;
            }

            if (this.Agent != null)
            {
                this.Agent.AgentPT.Visible = false;
                this.Agent.AgentComm.Visible = false;
            }
        }
    }
}
