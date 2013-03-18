namespace Sunplus.Agent.UI._MemberInfo.Commission
{
    using System;
    using System.Data;
    using System.Text;
    using Fanex.Age.UI;
    using Resources;
    using Sunplus.Agent.Model.BL.MemberInfo.MemberCommission;
    using Sunplus.Agent.UI._MemberCommision;
    using Sunplus.Agent.UI._MemberInfo.Commission;
    using Sunplus.Agent.Utilities;
    using Fanex.Agent.UI._MemberInfo.Commission;

    public partial class MemberCommissionP2P_Page : MemberCommision_Page
    {
        private int idMaster = 0, idAgent = 0, idFirstAgent = 0, idFirstMaster = 0;
        private MemberCommissionBO bo = null;
        private MemberCommissionP2P_Template tpl = new MemberCommissionP2P_Template();

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            this.tpl.CommonGroup = GroupOptionMemberCommission("P2P");

            if (CurrentUserSession.RoleId == 2)
            {
                this.tpl.gamesColspan = "2";
                this.tpl.privateGamesColspan = "2";

                this.tpl.MasterColumn.Clear();
                this.tpl.MasterColumn2.Clear();

                this.tpl.NoRowInfo.NoInfoColspan = "8";

                this.tpl.CommToAllTypes = Resources.Lang.lblA + ", " + Resources.Lang.lblMe;
            }
            else
            {
                this.tpl.gamesColspan = "3";
                this.tpl.privateGamesColspan = "3";

                this.tpl.NoRowInfo.NoInfoColspan = "10";

                this.tpl.CommToAllTypes = Resources.Lang.lblM + ", " + Resources.Lang.lblA + ", " + Resources.Lang.lblMe;
            }

            if (View == null || ((DataTable)View).Rows.Count == 0)
            {
                this.tpl.Rows.Clear();
                this.tpl.NoRowInfo.NoInfo = Lang.noInfo;
            }
            else
            {
                this.tpl.NoRowInfo.Clear();
                var rows = new MemberCommissionP2P_Rows_Block();
                if (CurrentUserSession.RoleId == 2)
                {
                    rows.MDiscount1.Clear();
                    rows.MDiscount2.Clear();
                }

                var dt = View as DataTable;
                StringBuilder result = new StringBuilder();
                var index = 0;

                for (var i = 0; i < dt.Rows.Count - 1; i += 2)
                {
                    var dr1 = dt.Rows[i];
                    var dr2 = dt.Rows[i + 1];

                    index++;

                    rows.No = index.ToString();
                    rows.UserName = dr2["username"].ToString();

                    rows.ADiscount1 = Formater.DecFormat(dr1["adiscount"], 2);
                    rows.Discount1 = Formater.DecFormat(dr1["discount"], 2);

                    rows.ADiscount2 = Formater.DecFormat(dr2["adiscount"], 2);
                    rows.Discount2 = Formater.DecFormat(dr2["discount"], 2);

                    if (CurrentUserSession.RoleId != 2)
                    {
                        rows.MDiscount1.MDiscount1 = Formater.DecFormat(dr1["mdiscount"], 2);
                        rows.MDiscount2.MDiscount2 = Formater.DecFormat(dr2["mdiscount"], 2);
                    }

                    result.Append(rows);
                }

                this.tpl.Rows.SetValue(result.ToString());
            }

            writer.Write(this.tpl);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            if (CurrentUserSession.RoleId == 2)
            {
                this.tpl.GroupAgent.Clear();
                return this.bo.GetP2PCommAgent(CurrentUserSession.CustId);
            }
            else if (CurrentUserSession.RoleId == 3)
            {
                this.tpl.GroupAgent.GroupMaster.Clear();
                this.tpl.GroupAgent.ComboAgent = GetComboCustomer(false, this.bo.GetAgentList(CurrentUserSession.CustId), CurrentUserSession.CustId, this.idAgent, ref this.idFirstAgent);

                if (this.idAgent == 0)
                {
                    return this.bo.GetP2PCommAgent(this.idFirstAgent);
                }
                else
                {
                    return this.bo.GetP2PCommAgent(this.idAgent);
                }
            }
            else
            {
                this.tpl.GroupAgent.GroupMaster.ComboMaster = GetComboCustomer(false, this.bo.GetMasterList(CurrentUserSession.CustId), CurrentUserSession.CustId, this.idMaster, ref this.idFirstMaster);

                if (this.idMaster != 0)
                {
                    this.tpl.GroupAgent.ComboAgent = GetComboCustomer(true, this.bo.GetAgentList(this.idMaster), this.idMaster, this.idAgent, ref this.idFirstAgent);
                }
                else
                {
                    this.tpl.GroupAgent.ComboAgent = GetComboCustomer(true, this.bo.GetAgentList(this.idFirstMaster), this.idFirstMaster, this.idAgent, ref this.idFirstAgent);
                }

                if (this.idMaster != this.idAgent)
                {
                    if (this.idAgent == 0)
                    {
                        return this.bo.GetP2PCommAgent(this.idFirstAgent);
                    }
                    else
                    {
                        return this.bo.GetP2PCommAgent(this.idAgent);
                    }
                }
                else
                {
                    if (this.idAgent == 0)
                    {
                        return this.bo.GetP2PCommMaster(this.idFirstMaster);
                    }
                    else
                    {
                        return this.bo.GetP2PCommMaster(this.idAgent);
                    }
                }
            }
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            this.idMaster = GetIntRequest("idMaster", 0);
            this.idAgent = GetIntRequest("idAgent", 0);

            this.bo = this.bo == null ? new MemberCommissionBO(CurrentUserSession.NetSessionId) : this.bo;
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }
    }
}