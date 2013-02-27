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

    public partial class MemberCommissionRacing_Page : MemberCommision_Page
    {
        private int idMaster = 0, idAgent = 0, idFirstAgent = 0, idFirstMaster = 0;
        private MemberCommissionBO bo = null;
        private MemberCommissionRacing_Template tpl = new MemberCommissionRacing_Template();

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            this.tpl.CommonGroup = GroupOptionMemberCommission("RC");

            if (CurrentUserSession.RoleId == 2)
            {
                this.tpl.Horse.SAHorse.colspancomm = "2";
                this.tpl.Horse.UKHorse.colspancomm = "2";
                this.tpl.Horse.AUHorse.colspancomm = "2";
                this.tpl.UKGreyhound.colspancomm = "2";
                this.tpl.AUGreyhound.colspancomm = "2";
                this.tpl.AUHarness.colspancomm = "2";

                this.tpl.Horse1.SAHorse.IsAgentSite.Clear();
                this.tpl.Horse1.UKHorse.IsAgentSite.Clear();
                this.tpl.Horse1.AUHorse.IsAgentSite.Clear();
                this.tpl.UKGreyhound1.IsAgentSite.Clear();
                this.tpl.AUGreyhound1.IsAgentSite.Clear();
                this.tpl.AUHarness1.IsAgentSite.Clear();

                this.tpl.NoRowInfo.NoInfoColspan = "18";

                this.tpl.CommToAllTypes = Lang.lblA + ", " + Lang.lblMe;
            }
            else
            {
                this.tpl.Horse.SAHorse.colspancomm = "3";
                this.tpl.Horse.UKHorse.colspancomm = "3";
                this.tpl.Horse.AUHorse.colspancomm = "3";
                this.tpl.UKGreyhound.colspancomm = "3";
                this.tpl.AUGreyhound.colspancomm = "3";
                this.tpl.AUHarness.colspancomm = "3";
                this.tpl.NoRowInfo.NoInfoColspan = "24";

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
                var rows = new MemberCommissionRacing_Rows_Block();
                if (CurrentUserSession.RoleId == 2)
                {
                    rows.Horse.SAHorse.IsAgentSite.Clear();
                    rows.Horse.UKHorse.IsAgentSite.Clear();
                    rows.Horse.AUHorse.IsAgentSite.Clear();
                    rows.UKGreyhound.IsAgentSite.Clear();
                    rows.AUGreyhound.IsAgentSite.Clear();
                    rows.AUHarness.IsAgentSite.Clear();
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

                    rows.Horse.SAHorse.v151discount_31 = Formater.DecFormat(dr["v151discount_31"], 2);
                    rows.Horse.UKHorse.v151discount_41 = Formater.DecFormat(dr["v151discount_41"], 2);
                    rows.Horse.AUHorse.v151discount_51 = Formater.DecFormat(dr["v151discount_51"], 2);
                    rows.UKGreyhound.v152discount_41 = Formater.DecFormat(dr["v152discount_41"], 2);
                    rows.AUGreyhound.v152discount_51 = Formater.DecFormat(dr["v152discount_51"], 2);
                    rows.AUHarness.v153discount_51 = Formater.DecFormat(dr["v153discount_51"], 2);

                    rows.Horse.SAHorse.v151playerdiscount_31 = Formater.DecFormat(dr["v151playerdiscount_31"], 2);
                    rows.Horse.UKHorse.v151playerdiscount_41 = Formater.DecFormat(dr["v151playerdiscount_41"], 2);
                    rows.Horse.AUHorse.v151playerdiscount_51 = Formater.DecFormat(dr["v151playerdiscount_51"], 2);
                    rows.UKGreyhound.v152playerdiscount_41 = Formater.DecFormat(dr["v152playerdiscount_41"], 2);
                    rows.AUGreyhound.v152playerdiscount_51 = Formater.DecFormat(dr["v152playerdiscount_51"], 2);
                    rows.AUHarness.v153playerdiscount_51 = Formater.DecFormat(dr["v153playerdiscount_51"], 2);

                    if (CurrentUserSession.RoleId != 2)
                    {
                        rows.Horse.SAHorse.IsAgentSite.v151adiscount_31 = Formater.DecFormat(dr["v151adiscount_31"], 2);
                        rows.Horse.UKHorse.IsAgentSite.v151adiscount_41 = Formater.DecFormat(dr["v151adiscount_41"], 2);
                        rows.Horse.AUHorse.IsAgentSite.v151adiscount_51 = Formater.DecFormat(dr["v151adiscount_51"], 2);
                        rows.UKGreyhound.IsAgentSite.v152adiscount_41 = Formater.DecFormat(dr["v152adiscount_41"], 2);
                        rows.AUGreyhound.IsAgentSite.v152adiscount_51 = Formater.DecFormat(dr["v152adiscount_51"], 2);
                        rows.AUHarness.IsAgentSite.v153adiscount_51 = Formater.DecFormat(dr["v153adiscount_51"], 2);
                    }

                    rows.Credit = Formater.DecFormat(dr["credit"], 2);

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
                return this.bo.GetRacingCommAgent(CurrentUserSession.CustId);
            }
            else if (CurrentUserSession.RoleId == 3)
            {
                this.tpl.GroupAgent.GroupMaster.Clear();
                this.tpl.GroupAgent.ComboAgent = GetComboCustomer(false, this.bo.GetAgentList(CurrentUserSession.CustId), CurrentUserSession.CustId, this.idAgent, ref this.idFirstAgent);

                if (this.idAgent == 0)
                {
                    return this.bo.GetRacingCommAgent(this.idFirstAgent);
                }
                else
                {
                    return this.bo.GetRacingCommAgent(this.idAgent);
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
                        return this.bo.GetRacingCommAgent(this.idFirstAgent);
                    }
                    else
                    {
                        return this.bo.GetRacingCommAgent(this.idAgent);
                    }
                }
                else
                {
                    if (this.idAgent == 0)
                    {
                        return this.bo.GetRacingCommMaster(this.idFirstMaster);
                    }
                    else
                    {
                        return this.bo.GetRacingCommMaster(this.idAgent);
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