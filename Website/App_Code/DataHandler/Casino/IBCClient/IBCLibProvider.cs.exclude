/// <summary>
/// Summary description for IBCLibProvider
/// </summary>
namespace DAL.IBCClient
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Web;
    using DNAide.Core;
    using Fanex.Data.DNAide;

    public class IBCLibProvider : IBCCustomerProvider
    {
        /// <summary>
        /// Get Customer Information from AGE system
        /// Username, username2, lastname, firstname,...
        /// </summary>
        /// <param name="ibcCustId"></param>
        /// <returns></returns>
        public override CSCustomerDetail GetCustInfoIBC(int ibcCustId)
        {
            Hashtable param = new Hashtable();
            param.Add("custid", ibcCustId);

            DbResult drCustomer = DNAide.Data.DbHelper.Execute("Age_CustomerSelUserInfo", param, 300);
            DataRow row = null;
            CSCustomerDetail customerDetail = new CSCustomerDetail();
            if (drCustomer.ResultSet.Tables[0].Rows.Count > 0)
            {
                row = drCustomer.ResultSet.Tables[0].Rows[0];
                customerDetail.UserName = CastAide.AsString(row["username"]);
                customerDetail.UserName2 = CastAide.AsString(row["username2"]);
                customerDetail.FirstName = CastAide.AsString(row["firstname"]);
                customerDetail.LastName = CastAide.AsString(row["lastname"]);
                customerDetail.Phone = CastAide.AsString(row["phone"]);
                customerDetail.MobilePhone = CastAide.AsString(row["mobilephone"]);
                customerDetail.Fax = CastAide.AsString(row["fax"]);
                customerDetail.Email = CastAide.AsString(row["email"]);
                customerDetail.RoleId = CastAide.AsInt32(row["roleid"]);
                customerDetail.Currency = CastAide.AsInt32(row["currency"]);
                customerDetail.Closed = CastAide.AsBoolean(row["closed"]);
                customerDetail.Suspend = CastAide.AsBoolean(row["suspend"]);
                customerDetail.SIBCCustID = CastAide.AsInt32(row["superid"]);
                customerDetail.MIBCCustID = CastAide.AsInt32(row["masterid"]);
                customerDetail.AIBCCustID = CastAide.AsInt32(row["agentid"]);

                customerDetail.GivenCredit = CastAide.AsInt32(row["credit"]);

                customerDetail.CasinoSync = CastAide.AsBoolean(row["synccasino"]);
                customerDetail.CasinoDisabled = CastAide.AsBoolean(row["disablecasino"]);

                customerDetail.LiveCasinoSync = CastAide.AsBoolean(row["synclivecasino"]);
                customerDetail.LiveCasinoDisabled = CastAide.AsBoolean(row["disablelivecasino"]);

                customerDetail.IBCCustID = CastAide.AsInt32(row["custid"]);
                customerDetail.UserPwd = CastAide.AsString(row["userpwd"]);
                customerDetail.CurrencyName = Sunplus.Agent.Model.Entities.UserSession.Current.Currency;

                customerDetail.UpperCustID = Sunplus.Agent.Model.Entities.UserSession.Current.CustId;

                // Get max transfer information
                CasinoLibrary.SBAgent.Customer customer = new CasinoLibrary.SBAgent.Customer();
                DataTable dt = customer.GetDefaultMaxTransfer(customerDetail.UpperCustID, 0, false);

                if (dt != null && dt.Rows.Count > 0)
                {
                    customerDetail.MaxTransferCreditLimit = CastAide.AsDouble((dt.Rows[0]["MaxLimit"]));
                    customerDetail.MinTransferCreditLimit = CastAide.AsDouble((dt.Rows[0]["MinLimit"]));
                    customerDetail.DefaultTransferCredit = CastAide.AsDouble((dt.Rows[0]["DefMaxTransferCredit"]));
                }

                customerDetail.NoIBCData = false;
            }

            Globals.IBCSetting.Casino.UseWebConfigProviderType = true;
            return customerDetail;
        }

        public bool GetCasinoStatus(int custid)
        {
            Hashtable param = new Hashtable();
            param.Add("custid", custid);

            DbResult drCustomer = DNAide.Data.DbHelper.Execute("Age_GetCSStatus", param, 300);
            return CastAide.AsBoolean(drCustomer.Outputs["status"]);
        }

        /// <summary>
        /// Initialize basics property of an object from posted values of a form. Be used in new customer function.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override CSCustomerDetail GetCustInfoIBC(string custid, Hashtable r)
        {
            CSCustomerDetail customerDetail = new CSCustomerDetail();

            customerDetail.UserName = CastAide.AsString(r["username"]);
            customerDetail.UserPwd = CastAide.AsString(r["userpwd"]);
            customerDetail.FirstName = CastAide.AsString(r["firstname"]);
            customerDetail.LastName = CastAide.AsString(r["lastname"]);
            customerDetail.Phone = CastAide.AsString(r["phone"]);
            customerDetail.MobilePhone = CastAide.AsString(r["mobilephone"]);
            customerDetail.Fax = CastAide.AsString(r["fax"]);
            customerDetail.Email = CastAide.AsString(r["email"]);
            customerDetail.RoleId = Sunplus.Agent.Model.Entities.UserSession.Current.RoleId - 1;
            customerDetail.Currency = Sunplus.Agent.Model.Entities.UserSession.Current.CurrencyId;
            customerDetail.CurrencyName = Sunplus.Agent.Model.Entities.UserSession.Current.Currency;
            customerDetail.Closed = CastAide.AsBoolean(r["closed"]);
            customerDetail.Suspend = CastAide.AsBoolean(r["suspend"]);

            customerDetail.SIBCCustID = Sunplus.Agent.Model.Entities.UserSession.Current.RoleId == 4 ? Sunplus.Agent.Model.Entities.UserSession.Current.CustId : 0;
            customerDetail.MIBCCustID = Sunplus.Agent.Model.Entities.UserSession.Current.RoleId == 3 ? Sunplus.Agent.Model.Entities.UserSession.Current.CustId : 0;
            customerDetail.AIBCCustID = Sunplus.Agent.Model.Entities.UserSession.Current.RoleId == 2 ? Sunplus.Agent.Model.Entities.UserSession.Current.CustId : 0;

            customerDetail.CasinoSync = false;
            customerDetail.CasinoDisabled = false;
            customerDetail.IBCCustID = CastAide.AsInt32(custid);

            return customerDetail;
        }

        public override bool UpdateCSStatus(int custid, bool bSyncCS, bool bDisabledCS)
        {
            Hashtable param = new Hashtable();
            param.Add("custid", custid);
            param.Add("disabledcs", bDisabledCS);
            param.Add("synccs", bSyncCS);
            param.Add("enabledownline", false);

            param.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);
            param.Add("subaccid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId);
            param.Add("loginip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
            param.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);

            DbResult drReturn = DNAide.Data.DbHelper.Execute("Acc_UpdCSStatus", param, 300);
            bool status = CastAide.AsBoolean(drReturn.Outputs["status"]);
            if (!status) throw new Exception(drReturn.Outputs["errmsg"].ToString());

            CasinoLibrary.SBAgent.Customer cust = new CasinoLibrary.SBAgent.Customer();
            return cust.UpdateCustEnableCasino(custid, !bDisabledCS, true, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), Sunplus.Agent.Model.Entities.UserSession.Current.CustId, Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId == 0 ? Sunplus.Agent.Model.Entities.UserSession.Current.Username : Sunplus.Agent.Model.Entities.UserSession.Current.SubUserName, 2);
        }

        public override bool UpdateCSStatus(int custid, bool bSyncCS, bool bDisabledCS, bool isUpdDownline)
        {
            Hashtable param = new Hashtable();
            param.Add("custid", custid);
            param.Add("disabledcs", bDisabledCS);
            param.Add("synccs", bSyncCS);
            param.Add("enabledownline", isUpdDownline);

            param.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);
            param.Add("subaccid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId);
            param.Add("loginip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
            param.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);

            DbResult drReturn = DNAide.Data.DbHelper.Execute("Acc_UpdCSStatus", param, 300);
            bool status = CastAide.AsBoolean(drReturn.Outputs["status"]);
            if (!status) throw new Exception(drReturn.Outputs["errmsg"].ToString());

            CasinoLibrary.SBAgent.Customer cust = new CasinoLibrary.SBAgent.Customer();
            return cust.UpdateCustEnableCasino(custid, !bDisabledCS, isUpdDownline, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), Sunplus.Agent.Model.Entities.UserSession.Current.CustId, Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId == 0 ? Sunplus.Agent.Model.Entities.UserSession.Current.Username : Sunplus.Agent.Model.Entities.UserSession.Current.SubUserName, 2);
        }
    }
}