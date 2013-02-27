namespace Sunplus.Agent.Model.BL.AccountInfo
{
    using System;
    using System.Collections;
    using System.Data;
    using Sunplus.Agent.Model.BL.Base;
    using Sunplus.Agent.Model.BL.Casino;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.BL.MemberInfo.CreditBalance;
    using Sunplus.Agent.Model.DL.AccountInfo;
    using Sunplus.Agent.Model.Entities;

    public class AccountInfoBO : BOBase
    {
        #region Variable

        private readonly AccountInfoDAO accountInfoDAO;

        #endregion Variable

        public AccountInfoBO(string netSessionId)
            : base(netSessionId)
        {
            this.accountInfoDAO = new AccountInfoDAO();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="query">UserName, First Name, Last Name</param>
        /// <returns></returns>
        public DataTable SearchCustomer(string query)
        {
            return this.accountInfoDAO.Age_CustomerSelUserSearch(CurrentSession.CustId, query, 0, 0, 0, 1, CurrentSession.IsInternal);
        }

        public DataTable SearchCustomer(int custId, string query, int isSubAccount, int excludeMember, int directDownline, int isAccInfo)
        {
            return this.accountInfoDAO.Age_CustomerSelUserSearch(custId, query, isSubAccount, excludeMember, directDownline, isAccInfo, CurrentSession.IsInternal);
        }

        public Hashtable UpdateUserSearch(string search, string firstName, string lastName)
        {
            AccountInfoDAO dao = new AccountInfoDAO();
            return dao.Acc_CustomerUpdUserSearch(CurrentSession.CustId, search, firstName, lastName);
        }

        public DataTable GetCustomerList(int custId, int pageSize, int pageIndex, ref int totalPages)
        {
            return this.accountInfoDAO.Acc_CustomerSelList(custId, pageSize, pageIndex, ref totalPages);
        }

        public DataSet GetCustomerInfo(string userName)
        {
            return this.accountInfoDAO.Age_CustomerSelAllInfo(userName, CurrentSession.CustId, CurrentSession.IsInternal);
        }

        public DataTable CustomerSelOnline(int custId)
        {
            return this.accountInfoDAO.Acc_CustomerSelOnline(custId);
        }

        public DataTable Acc_CustomerSelAgentInfo(int custId)
        {
            return this.accountInfoDAO.Acc_CustomerSelAgentInfo(custId);
        }

        public DataTable Acc_CustomerSelMAgentInfo(int custId)
        {
            return this.accountInfoDAO.Acc_CustomerSelMAgentInfo(custId);
        }

        public DataTable Acc_CustomerSelSAgentInfo(int custId)
        {
            return this.accountInfoDAO.Acc_CustomerSelSAgentInfo(custId);
        }

        /// <summary>
        /// Update credit balance
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="custId"></param>
        /// <param name="amount"></param>
        /// <param name="ip"></param>
        /// <param name="errMsg"></param>
        /// <returns>
        ///     Error : 1
        ///     Success : 0
        /// </returns>
        public int UpdateCreditBalance(int roleId, int custId, double amount, string ip, ref string errMsg)
        {
            if (CurrentSession.IsAgent || roleId == (int)UserTypes.Member)
            {
                return this.accountInfoDAO.Age_CustomerUpdCredit_Ag(CurrentSession.UserId, CurrentSession.SubAccId, custId, amount, ip, CurrentSession.LangKey, ref errMsg);
            }
            else if (CurrentSession.IsMaster || roleId == (int)UserTypes.Agent)
            {
                return this.accountInfoDAO.Age_CustomerUpdCredit_MAg(CurrentSession.UserId, CurrentSession.SubAccId, custId, amount, ip, CurrentSession.LangKey, ref errMsg);
            }
            else if (CurrentSession.IsSuper)
            {
                return this.accountInfoDAO.Age_CustomerUpdCredit_SAg(CurrentSession.UserId, CurrentSession.SubAccId, custId, amount, ip, CurrentSession.LangKey, ref errMsg);
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Update max credit
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="newMaxCredit"></param>
        /// <param name="ip"></param>
        /// <param name="errMsg"></param>
        /// <returns>Error code: 119,120</returns>
        public int UpdateMaxCredit(int custId, double newMaxCredit, string ip, ref string errMsg)
        {
            return this.accountInfoDAO.Age_CustomerUpdMaxCredit(CurrentSession.CustId, CurrentSession.SubAccId, custId, newMaxCredit, ip, CurrentSession.LangKey, ref errMsg);
        }

        /// <summary>
        /// Update Open/Closed status for customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="isClosed"></param>
        /// <param name="isSynCasino"></param>
        /// <param name="isClearAllCredit"></param>
        /// <param name="ip"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int UpdateClosedStatus(int custId, bool isClosed, bool isSynCasino, bool isClearAllCredit, string ip, ref string errMsg)
        {
            bool disabledCasino = Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledCasino;
            int errorCode = this.accountInfoDAO.Acc_CustomerUpdClosed(custId, CurrentSession.UserId, CurrentSession.SubAccId, isClosed, disabledCasino, ip, CurrentSession.LangKey, ref errMsg);

            if (errorCode == 0 && isClearAllCredit)
            {
                CreditBalanceBO creditBO = new CreditBalanceBO(NetSessionId);
                creditBO.ClearCustomerCredit(custId, ip, out errMsg);
            }

            if (errorCode == 0 && isSynCasino)
            {
                CasinoBO casinoBO = new CasinoBO(CurrentSession.NetSessionId);
                casinoBO.Customer.UpdateCustClosed(custId, isClosed, ip, CurrentSession.UserId, CurrentSession.UserName, CasinoBO.AGENT_SYSTEM);
            }

            return errorCode;
        }

        /// <summary>
        /// Update suspended status for customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="isSuspended"></param>
        /// <param name="isSynCasino"></param>
        /// <param name="ip"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int UpdateSuspendedStatus(int custId, bool isSuspended, bool isSynCasino, string ip, ref string errMsg)
        {
            int errorCode = this.accountInfoDAO.Acc_CustomerUpdSuspended(custId, CurrentSession.UserId, CurrentSession.SubAccId, isSuspended, AppSettings.DisabledCasino, ip, CurrentSession.LangKey, ref errMsg);
            if (errorCode == 0 && isSynCasino)
            {
                CasinoBO casinoBO = new CasinoBO(CurrentSession.NetSessionId);
                casinoBO.Customer.UpdateCustSuspend(custId, isSuspended, ip, CurrentSession.UserId, CurrentSession.UserName, CasinoBO.AGENT_SYSTEM);
            }

            return errorCode;
        }

        /// <summary>
        /// Update disabled outright status for customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="isDisableOutright"></param>
        /// <param name="ip"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int UpdateOutrightStatus(int custId, bool isDisableOutright, string ip, ref string errMsg)
        {
            return this.accountInfoDAO.Acc_CustomerUpdDisableOutright(custId, CurrentSession.UserId, CurrentSession.SubAccId, isDisableOutright, ip, CurrentSession.LangKey, ref errMsg);
        }

        /// <summary>
        /// Update disabled HR status for customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="disabledHR"></param>
        /// <param name="updateDownline"></param>
        /// <param name="ip"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int UpdateHRStatus(int custId, bool disabledHR, bool updateDownline, string ip, ref string errMsg)
        {
            return this.accountInfoDAO.Acc_CustomerUpdDisable151_31(custId, CurrentSession.UserId, CurrentSession.SubAccId, disabledHR, updateDownline, ip, CurrentSession.LangKey, ref errMsg);
        }

        /// <summary>
        /// Update open tote status for customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="disabledOpenTote"></param>
        /// <param name="updateDownline"></param>
        /// <param name="ip"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int UpdateOpenToteStatus(int custId, bool disabledOpenTote, bool updateDownline, string ip, ref string errMsg)
        {
            return this.accountInfoDAO.Acc_CustomerUpdDisable151_41(custId, CurrentSession.UserId, CurrentSession.SubAccId, disabledOpenTote, updateDownline, ip, CurrentSession.LangKey, ref errMsg);
        }

        /// <summary>
        /// Update AuTote status for customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="disabledAuTote"></param>
        /// <param name="updateDownline"></param>
        /// <param name="ip"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int UpdateAuToteStatus(int custId, bool disabledAuTote, bool updateDownline, string ip, ref string errMsg)
        {
            return this.accountInfoDAO.Acc_CustomerUpdDisable151_51(custId, CurrentSession.UserId, CurrentSession.SubAccId, disabledAuTote, updateDownline, ip, CurrentSession.LangKey, ref errMsg);
        }

        /// <summary>
        /// Update GreyHoundUK status for customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="disabledGreyhoundUK"></param>
        /// <param name="updateDownline"></param>
        /// <param name="ip"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int UpdateGreyhoundUKStatus(int custId, bool disabledGreyhoundUK, bool updateDownline, string ip, ref string errMsg)
        {
            return this.accountInfoDAO.Acc_CustomerUpdDisable152_41(custId, CurrentSession.UserId, CurrentSession.SubAccId, disabledGreyhoundUK, updateDownline, ip, CurrentSession.LangKey, ref errMsg);
        }

        /// <summary>
        /// Update GreyHoundAU status for customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="disabledGreyhoundAU"></param>
        /// <param name="updateDownline"></param>
        /// <param name="ip"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int UpdateGreyhoundAUStatus(int custId, bool disabledGreyhoundAU, bool updateDownline, string ip, ref string errMsg)
        {
            return this.accountInfoDAO.Acc_CustomerUpdDisable152_51(custId, CurrentSession.UserId, CurrentSession.SubAccId, disabledGreyhoundAU, updateDownline, ip, CurrentSession.LangKey, ref errMsg);
        }

        /// <summary>
        /// Update HarnessAU status for customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="disabledGreyhoundAU"></param>
        /// <param name="updateDownline"></param>
        /// <param name="ip"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int UpdateHarnessAUStatus(int custId, bool disabledHarnessAU, bool updateDownline, string ip, ref string errMsg)
        {
            return this.accountInfoDAO.Acc_CustomerUpdDisable153_51(custId, CurrentSession.UserId, CurrentSession.SubAccId, disabledHarnessAU, updateDownline, ip, CurrentSession.LangKey, ref errMsg);
        }

        /// <summary>
        /// Update disabled financial status for customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="disabledFinancial"></param>
        /// <param name="updateDownline"></param>
        /// <param name="ip"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int UpdateFinancialStatus(int custId, bool disabledFinancial, bool updateDownline, string ip, ref string errMsg)
        {
            return this.accountInfoDAO.Acc_CustomerUpdDisableFI(custId, CurrentSession.UserId, CurrentSession.SubAccId, disabledFinancial, updateDownline, ip, CurrentSession.LangKey, ref errMsg);
        }

        /// <summary>
        /// Update disabled auto master position taking status for customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="allowedAutoMasterPT"></param>
        /// <param name="ip"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int UpdateAutoMasterPTStatus(int custId, bool allowedAutoMasterPT, string ip, ref string errMsg)
        {
            return this.accountInfoDAO.Acc_CustomerUpdMaster_DisableAutoMPT(custId, CurrentSession.UserId, CurrentSession.SubAccId, allowedAutoMasterPT, ip, CurrentSession.LangKey, ref errMsg);
        }

        /// <summary>
        /// Update disabled Casino status for customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="disabledCasino"></param>
        /// <param name="isSynCasino"></param>
        /// <param name="isUpdateDownline"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public int UpdateCasinoStatus(int custId, bool disabledCasino, bool isSynCasino, bool isUpdateDownline, string ip)
        {
            bool success = this.accountInfoDAO.Acc_UpdCSStatus(custId, CurrentSession.UserId, CurrentSession.SubAccId, isSynCasino, disabledCasino, isUpdateDownline, ip, CurrentSession.LangKey);
            if (success)
            {
                CasinoBO casinoBO = new CasinoBO(CurrentSession.NetSessionId);
                casinoBO.Customer.UpdateCustEnableCasino(custId, !disabledCasino, isUpdateDownline, ip, CurrentSession.UserId, CurrentSession.UserName, CasinoBO.AGENT_SYSTEM);
            }

            return success ? 0 : 1;
        }

        /// <summary>
        /// Update disabled Casino status for customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="disabledCasino"></param>
        /// <param name="isSynCasino"></param>
        /// <param name="isUpdateDownline"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public int UpdateCasinoStatus(int custId, bool disabledCasino, bool isSynCasino, bool isUpdateDownline, string ip, ref string errMsg)
        {
            int errCode = this.accountInfoDAO.Acc_UpdCSStatus(custId, CurrentSession.UserId, CurrentSession.SubAccId, isSynCasino, disabledCasino, isUpdateDownline, ip, CurrentSession.LangKey, ref errMsg);
            if (errCode == 0)
            {
                CasinoBO casinoBO = new CasinoBO(CurrentSession.NetSessionId);
                casinoBO.Customer.UpdateCustEnableCasino(custId, !disabledCasino, isUpdateDownline, ip, CurrentSession.UserId, CurrentSession.UserName, CasinoBO.AGENT_SYSTEM);
            }

            return errCode;
        }

        /// <summary>
        /// Update disabled Bingo status for customer
        /// </summary>vao i
        /// <param name="custId"></param>
        /// <param name="disabledBingo"></param>
        /// <param name="isSynBingo"></param>
        /// <param name="isEnableDownline"></param>
        /// <param name="ip"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int UpdateBingoStatus(int custId, bool disabledBingo, bool isSynBingo, bool isEnableDownline, string ip, ref string errMsg)
        {
            return this.accountInfoDAO.Acc_UpdBingoStatus(custId, CurrentSession.UserId, CurrentSession.SubAccId, isSynBingo, disabledBingo, isEnableDownline, ip, CurrentSession.LangKey, ref errMsg);
        }

        public int UpdateNumberStatus(int custId, bool disableNumber, bool isUpdateDownline, string ip, ref string errMsg)
        {
            return this.accountInfoDAO.Acc_CustomerUpdDisableNumber(custId, CurrentSession.UserId, CurrentSession.SubAccId, disableNumber, isUpdateDownline, ip, CurrentSession.LangKey, ref errMsg);
        }

        public int UpdateVirtualSportsStatus(int custId, bool disabledVS, bool isUpdateDownline, string ip, ref string errMsg)
        {
            return this.accountInfoDAO.Acc_CustomerUpdDisableVirtualSports(custId, CurrentSession.UserId, CurrentSession.SubAccId, disabledVS, isUpdateDownline, ip, CurrentSession.LangKey, ref errMsg);
        }

        public int UpdateDoubleCommStatus(int custId, bool doubleCommLosing, string ip, ref string errMsg)
        {
            return this.accountInfoDAO.Acc_CustomerUpdDoubleCommLosing(custId, CurrentSession.UserId, CurrentSession.SubAccId, doubleCommLosing, ip, CurrentSession.LangKey, ref errMsg);
        }

        public DataTable GetCustomerSelUserInfo(int custId)
        {
            return this.accountInfoDAO.Age_CustomerSelUserInfo(custId);
        }

        public int UpdateCustomerUserInfo(int custId, string firstName, string lastName, string phone, string mobilePhone, string email, string fax, int isSync, string ip, ref int errorCode)
        {
            return this.accountInfoDAO.Age_CustomerUpdUserInfo(custId, firstName, lastName, phone, mobilePhone, email, fax, isSync, ip, CurrentSession.UserId, CurrentSession.SubAccId, ref errorCode);
        }

        public CustomerInfo GetCustInfo(int custId)
        {
            var dtbCustomer = this.GetCustomerSelUserInfo(custId);
            var customerInfo = new CustomerInfo();

            if (dtbCustomer.Rows.Count > 0)
            {
                var row = dtbCustomer.Rows[0];
                customerInfo.UserName = Convert.ToString(row["username"]);
                customerInfo.UserName2 = Convert.ToString(row["username2"]);
                customerInfo.FirstName = Convert.ToString(row["firstname"]);
                customerInfo.LastName = Convert.ToString(row["lastname"]);
                customerInfo.Phone = Convert.ToString(row["phone"]);
                customerInfo.MobilePhone = Convert.ToString(row["mobilephone"]);
                customerInfo.Fax = Convert.ToString(row["fax"]);
                customerInfo.Email = Convert.ToString(row["email"]);
                customerInfo.RoleId = Convert.ToInt32(row["roleid"]);
                customerInfo.Currency = Convert.ToInt32(row["currency"]);
                customerInfo.Closed = Convert.ToBoolean(row["closed"]);
                customerInfo.Suspend = Convert.ToBoolean(row["suspend"]);
                customerInfo.SIBCCustID = Convert.ToInt32(row["superid"]);
                customerInfo.MIBCCustID = Convert.ToInt32(row["masterid"]);
                customerInfo.AIBCCustID = Convert.ToInt32(row["agentid"]);

                customerInfo.GivenCredit = Convert.ToDecimal(row["credit"]);

                customerInfo.CasinoSync = Convert.ToBoolean(row["synccasino"]);
                customerInfo.CasinoDisabled = Convert.ToBoolean(row["disablecasino"]);

                customerInfo.LiveCasinoSync = Convert.ToBoolean(row["synclivecasino"]);
                customerInfo.LiveCasinoDisabled = Convert.ToBoolean(row["disablelivecasino"]);

                //customerInfo.VirtualSportsDisabled = Convert.ToBoolean(row["disablevirtualsports"]);

                customerInfo.BingoSync = Convert.ToBoolean(row["syncbingo"]);
                if (row.Table.Columns.Contains("disablebingo"))
                {
                    customerInfo.BingoDisabled = Convert.ToBoolean(row["disablebingo"]);
                }

                customerInfo.IBCCustID = Convert.ToInt32(row["custid"]);
                customerInfo.UserPwd = Convert.ToString(row["userpwd"]);
                customerInfo.CurrencyName = CurrentSession.Currency;

                customerInfo.UpperCustID = CurrentSession.CustId;

                // Get max transfer information
                if (customerInfo.UpperCustID != 0 && customerInfo.CasinoSync)
                {
                    CasinoLibrary.SBAgent.Customer customer = new CasinoLibrary.SBAgent.Customer();
                    DataTable dt = null;

                    try
                    {
                        dt = customer.GetDefaultMaxTransfer(customerInfo.UpperCustID, 0, false);
                    }
                    catch
                    {
                    }

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        customerInfo.MaxTransferCreditLimit = Convert.ToDouble(dt.Rows[0]["MaxLimit"]);
                        customerInfo.MinTransferCreditLimit = Convert.ToDouble(dt.Rows[0]["MinLimit"]);
                        customerInfo.DefaultTransferCredit = Convert.ToDouble(dt.Rows[0]["DefMaxTransferCredit"]);
                    }
                }

                customerInfo.NoIBCData = false;
            }

            return customerInfo;
        }

        public int ResetPassword(string custId, string newPassword, string ip)
        {
            Hashtable outputs = accountInfoDAO.Age_CustomerUpdPwd(custId, newPassword, ip);

            return (int)outputs["errcode"];
        }
    }
}