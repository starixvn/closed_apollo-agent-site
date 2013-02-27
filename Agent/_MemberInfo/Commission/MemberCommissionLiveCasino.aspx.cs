namespace Sunplus.Agent.UI._MemberInfo.Commission
{
    using System;
    using System.Data;
    using System.Text;
    using Fanex.Age.UI;
    using Resources;
    using Sunplus.Agent.Model.BL.MemberInfo.MemberCommission;
    using Sunplus.Agent.Model.Entities.Permission;
    using Sunplus.Agent.UI._MemberCommision;
    using Sunplus.Agent.UI._MemberInfo.Commission;
    using Sunplus.Agent.Utilities;

    public partial class MemberCommissionLiveCasino_Page : MemberCommision_Page
    {
        private int idMaster = 0, idAgent = 0, idFirstAgent = 0, idFirstMaster = 0;
        private MemberCommissionBO bo = null;
        private MemberCommissionLiveCasino_Template tpl = new MemberCommissionLiveCasino_Template();

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            this.tpl.CommonGroup = GroupOptionMemberCommission("LC");

            if (CurrentUserSession.RoleId == 2)
            {
                this.tpl.colspancomm = "2";

                this.tpl.masterColumn.Clear();

                this.tpl.NoRowInfo.NoInfoColspan = "8";

                this.tpl.CommToAllTypes = Lang.lblA + ", " + Lang.lblMe;
            }
            else
            {
                this.tpl.colspancomm = "3";
                this.tpl.NoRowInfo.NoInfoColspan = "9";

                this.tpl.CommToAllTypes = Lang.lblM + ", " + Lang.lblA + ", " + Lang.lblMe;
            }

            if (View == null || ((DataTable)View).Rows.Count == 0)
            {
                this.tpl.Rows.Clear();
                this.tpl.NoRowInfo.NoInfo = Lang.noInfo;
            }
            else
            {
                this.tpl.NoRowInfo.Clear();
                var rows = new MemberCommissionLiveCasino_Rows_Block();
                if (CurrentUserSession.RoleId == 2)
                {
                    rows.ADiscount.Clear();
                }

                var dt = View as DataTable;
                StringBuilder result = new StringBuilder();
                var index = 0;
                string closedBg = Fanex.Age.UI.Common.RowCssClosed;
                string suspendedBg = Fanex.Age.UI.Common.RowCssSuspended;
                string disabledBg = Fanex.Age.UI.Common.RowCssDisabled;
                string evenBg = Fanex.Age.UI.Common.RowCssEven;
                string oddBg = Fanex.Age.UI.Common.RowCssOdd;

                DataRowCollection dtc = dt.Rows;
                foreach (DataRow dr in dtc)
                {
                    index++;

                    if (Convert.ToInt32(dr["enabled"]) != 1)
                    {
                        rows.BgRow = disabledBg;
                    }
                    else if (Convert.ToInt32(dr["closed"]) != 0)
                    {
                        rows.BgRow = closedBg;
                    }
                    else if (Convert.ToInt32(dr["suspended"]) != 0)
                    {
                        rows.BgRow = suspendedBg;
                    }
                    else if (index % 2 != 0)
                    {
                        rows.BgRow = oddBg;
                    }
                    else
                    {
                        rows.BgRow = evenBg;
                    }

                    rows.No = index.ToString();
                    rows.UserName = dr["username"].ToString();
                    rows.FirstName = dr["firstname"].ToString();
                    rows.LastName = dr["lastname"].ToString();

                    rows.Discount = Formater.DecFormat(dr["discount"], 2);
                    rows.PlayerDiscount = Formater.DecFormat(dr["playerdiscount"], 2);

                    if (CurrentUserSession.RoleId != 2)
                    {
                        rows.ADiscount.ADiscount = Formater.DecFormat(dr["adiscount"], 2);
                    }

                    rows.credit_value = Formater.DecFormat(dr["credit"], 2);

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
                return this.bo.GetLiveCasinoCommAgent(CurrentUserSession.CustId);
            }
            else if (CurrentUserSession.RoleId == 3)
            {
                this.tpl.GroupAgent.GroupMaster.Clear();
                this.tpl.GroupAgent.ComboAgent = GetComboCustomer(false, this.bo.GetAgentList(CurrentUserSession.CustId), CurrentUserSession.CustId, this.idAgent, ref this.idFirstAgent);

                if (this.idAgent == 0)
                {
                    return this.bo.GetLiveCasinoCommAgent(this.idFirstAgent);
                }
                else
                {
                    return this.bo.GetLiveCasinoCommAgent(this.idAgent);
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
                        return this.bo.GetLiveCasinoCommAgent(this.idFirstAgent);
                    }
                    else
                    {
                        return this.bo.GetLiveCasinoCommAgent(this.idAgent);
                    }
                }
                else
                {
                    if (this.idAgent == 0)
                    {
                        return this.bo.GetLiveCasinoCommMaster(this.idFirstMaster);
                    }
                    else
                    {
                        return this.bo.GetLiveCasinoCommMaster(this.idAgent);
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