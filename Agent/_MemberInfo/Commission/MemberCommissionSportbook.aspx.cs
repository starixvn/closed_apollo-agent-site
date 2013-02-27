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
    using Sunplus.Agent.Model.Entities.Permission;

    public partial class MemberCommissionSportbook_Page : MemberCommision_Page
    {
        private int idMaster = 0, idAgent = 0, idFirstAgent = 0, idFirstMaster = 0;
        private MemberCommissionBO bo = null;
        private MemberCommissionSportbook_Template tpl = new MemberCommissionSportbook_Template();

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            this.tpl.CommonGroup = GroupOptionMemberCommission("SB");
            int colspanNoRowInfo = 16;

            if (CurrentUserSession.RoleId == 2)
            {
                this.tpl.colspancomm = "2";
                this.tpl.NumberComm.colspancomm = "2";                

                this.tpl.MasterCom1.Clear();
                this.tpl.MasterCom2.Clear();
                this.tpl.MasterCom3.Clear();
                this.tpl.NumberComm1.MasterCom.Clear();

                this.tpl.CommToAllTypes = Lang.appliesallbettype + "<li>" + Lang.lblA + ", " + Lang.lblMe + "</li>";
            }
            else
            {
                this.tpl.colspancomm = "3";
                this.tpl.NumberComm.colspancomm = "3";

                colspanNoRowInfo += 4;                

                this.tpl.CommToAllTypes = Lang.appliesallbettype + "<li>" + Lang.lblM + ", " + Lang.lblA + ", " + Lang.lblMe + "</li>";
            }

            if (Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGame || Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission)
            {
                this.tpl.NumberComm.Clear();
                this.tpl.NumberComm1.Clear();
            }            

            if (View == null || ((DataTable)View).Rows.Count == 0)
            {
                if (!UserPermission.IsShowDoubleCommission)
                {
                    this.tpl.DCom.Clear();
                    colspanNoRowInfo --;     
                }

                this.tpl.Rows.Clear();
                this.tpl.NoRowInfo.NoInfoColspan = colspanNoRowInfo.ToString();
                this.tpl.NoRowInfo.NoInfo = Lang.noInfo;
            }
            else
            {
                this.tpl.NoRowInfo.Clear();
                var rows = new MemberCommissionSportbook_Rows_Block();
                if (!UserPermission.IsShowDoubleCommission)
                {
                    this.tpl.DCom.Clear();
                    rows.DCom.Clear();
                }

                if (CurrentUserSession.RoleId == 2)
                {
                    rows.ADiscount.Clear();
                    rows.ADiscount1x2.Clear();
                    rows.ADiscountCs.Clear();
                    rows.NumberComm.ADiscountNumberGame.Clear();
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
                    rows.Group = dr["ugroup"].ToString();
                    rows.DCom.DoubleCommLosing = Convert.ToBoolean(dr["doublecommlosing"].ToString()) ? "class='tick " + rows.BgRow + "'" : string.Empty;

                    rows.Discount = Formater.DecFormat(dr["discount"], 2);
                    rows.Discount1x2 = Formater.DecFormat(dr["discount1x2"], 2);
                    rows.DiscountCs = Formater.DecFormat(dr["discountcs"], 2);
                    rows.NumberComm.DiscountNumberGame = Formater.DecFormat(dr["discount_numbergame"], 2);

                    rows.PlayerDiscount = Formater.DecFormat(dr["playerdiscount"], 2);
                    rows.PlayerDiscount1x2 = Formater.DecFormat(dr["playerdiscount1x2"], 2);
                    rows.PlayerDiscountCs = Formater.DecFormat(dr["playerdiscountcs"], 2);
                    rows.NumberComm.PlayerDiscountNumberGame = Formater.DecFormat(dr["playerdiscount_numbergame"], 2);

                    if (CurrentUserSession.RoleId != 2)
                    {
                        rows.ADiscount.ADiscount = Formater.DecFormat(dr["adiscount"], 2);
                        rows.ADiscount1x2.ADiscount1x2 = Formater.DecFormat(dr["adiscount1x2"], 2);
                        rows.ADiscountCs.ADiscountCs = Formater.DecFormat(dr["adiscountcs"], 2);
                        rows.NumberComm.ADiscountNumberGame.ADiscountNumberGame = Formater.DecFormat(dr["adiscount_numbergame"], 2);
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
                return this.bo.GetSportbookCommAgent(CurrentUserSession.CustId);
            }
            else if (CurrentUserSession.RoleId == 3)
            {
                this.tpl.GroupAgent.GroupMaster.Clear();
                this.tpl.GroupAgent.ComboAgent = this.GetComboCustomer(false, this.bo.GetAgentList(CurrentUserSession.CustId), CurrentUserSession.CustId, this.idAgent, ref this.idFirstAgent);

                if (this.idAgent == 0)
                {
                    return this.bo.GetSportbookCommAgent(this.idFirstAgent);
                }
                else
                {
                    return this.bo.GetSportbookCommAgent(this.idAgent);
                }
            }
            else
            {
                this.tpl.GroupAgent.GroupMaster.ComboMaster = this.GetComboCustomer(false, this.bo.GetMasterList(CurrentUserSession.CustId), CurrentUserSession.CustId, this.idMaster, ref this.idFirstMaster);

                if (this.idMaster != 0)
                {
                    this.tpl.GroupAgent.ComboAgent = this.GetComboCustomer(true, this.bo.GetAgentList(this.idMaster), this.idMaster, this.idAgent, ref this.idFirstAgent);
                }
                else
                {
                    this.tpl.GroupAgent.ComboAgent = this.GetComboCustomer(true, this.bo.GetAgentList(this.idFirstMaster), this.idFirstMaster, this.idAgent, ref this.idFirstAgent);
                }

                if (this.idMaster != this.idAgent)
                {
                    if (this.idAgent == 0)
                    {
                        return this.bo.GetSportbookCommAgent(this.idFirstAgent);
                    }
                    else
                    {
                        return this.bo.GetSportbookCommAgent(this.idAgent);
                    }
                }
                else
                {
                    if (this.idAgent == 0)
                    {
                        return this.bo.GetSportbookCommMaster(this.idFirstMaster);
                    }
                    else
                    {
                        return this.bo.GetSportbookCommMaster(this.idAgent);
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