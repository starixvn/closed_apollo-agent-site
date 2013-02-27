// --------------------------------------------------------------------------
// <copyright file="CreditBalance.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lee" date="2010.02.19">Render credit balance page</change>
//   <change who="Kaden.Trinh" date="2012.05.30">Refactor code and check StyleCop</change>
// </history>

namespace Sunplus.Agent.UI._MemberInfo.CreditBalance
{
    using System.Collections;
    using System.Data;
    using System.Web.UI;
    using Fanex.Age.UI.Components;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.BL.MemberInfo.CreditBalance;
    using Sunplus.Agent.Model.BL.UserMessage;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;
    using Sunplus.Agent.Utilities;

    public partial class CreditBalance_Page : Fanex.Age.UI.TplPage
    {
        #region Define Variables

        private string positive = "positive";
        private string negative = "negative";
        private string currency = null;

        #endregion Define Variables

        public override void RenderContent(HtmlTextWriter writer)
        {
            this.RegisterJavascriptObject("userid", CurrentUserSession.UserId);
            this.RegisterJavascriptObject("subid", CurrentUserSession.SubAccId);
            this.RegisterJavascriptObject("viewlog", Sunplus.Agent.Model.BL.Definitions.AppSettings.ShowViewLog && !CurrentUserSession.IsSubAcc);
            this.currency = CurrentUserSession.Currency + " ";

            CreditBalance_Template tpl = new CreditBalance_Template();

            // Display Popup Introduction
            if (!UserPermission.IsNeutral || string.Compare(CurrentUserSession.Site, "789y", true) == 0)
            {
                Fanex.Age.UI.Components.PopupAd.PopupAd_Template tplPopup = new Fanex.Age.UI.Components.PopupAd.PopupAd_Template();

                if (this.IsOnlyAustTote())
                {
                    if (CurrentUserSession.Language == "th-TH")
                    {
                        tplPopup.Content = new Sunplus.Agent.UI._MemberInfo.CreditBalance.Announcement.th_TH.AustTote_Template();
                    }
                    else if (CurrentUserSession.Language == "zh-CN")
                    {
                        tplPopup.Content = new Sunplus.Agent.UI._MemberInfo.CreditBalance.Announcement.zh_CN.AustTote_Template();
                    }
                    else if (CurrentUserSession.Language == "zh-TW")
                    {
                        tplPopup.Content = new Sunplus.Agent.UI._MemberInfo.CreditBalance.Announcement.zh_TW.AustTote_Template();
                    }
                    else
                    {
                        tplPopup.Content = new Sunplus.Agent.UI._MemberInfo.CreditBalance.Announcement.en_US.AustTote_Template();
                    }
                }
                else
                {
                    tplPopup.Content = new Sunplus.Agent.UI._MemberInfo.CreditBalance.Announcement.en_US.AustSATote_Template();
                }

                tpl.AdditionalContent.Append(tplPopup);
            }

            object[] dts = View as object[];
            Hashtable creditBalance = dts[0] as Hashtable;
            bool isPTChanged = (bool)dts[1];
            bool isBettingLimitChanged = (bool)dts[2];

            this.RenderPopupMessage(tpl, isPTChanged, isBettingLimitChanged);

            this.RenderCasino(tpl, creditBalance);

            this.RenderBingo(tpl, creditBalance);

            tpl.username = CurrentUserSession.DisplayNickName;

            decimal cashbalance = (decimal)creditBalance["cashbalance"];
            tpl.cashBalance = this.CurrencyString(cashbalance);
            tpl.classCashBalance = this.CssClassName(cashbalance);

            decimal yesterday_cashbalance = (decimal)creditBalance["yesterday_cashbalance"];
            tpl.yesterdayCashBalance = this.CurrencyString(yesterday_cashbalance);

            decimal totalbalance = (decimal)creditBalance["totalbalance"];
            tpl.totalBalance = this.CurrencyString(totalbalance);
            tpl.classTotalBalance = this.CssClassName(totalbalance);

            decimal yesterday_totalbalance = (decimal)creditBalance["yesterday_totalbalance"];
            tpl.yesterdayTotalBalance = this.CurrencyString(yesterday_totalbalance);

            decimal totaloutstanding = (decimal)creditBalance["totaloutstanding"];
            tpl.totalOutstanding = this.CurrencyString(totaloutstanding);
            tpl.classTotalOutstanding = this.CssClassName(totaloutstanding);

            int roleId = CurrentUserSession.RoleId;
            if (roleId == (int)UserTypes.Super)
            {
                this.RenderSuper(tpl, creditBalance);
            }
            else if (roleId == (int)UserTypes.Master)
            {
                this.RenderMaster(tpl, creditBalance);
            }
            else if (roleId == (int)UserTypes.Agent)
            {
                this.RenderAgent(tpl, creditBalance);
            }

            writer.Write(tpl);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            // Get credit information
            CreditBalanceBO creditBalanceBO = new CreditBalanceBO(CurrentUserSession.NetSessionId);
            Hashtable creditBalance = creditBalanceBO.GetCreditBalance();

            // Get PT message status
            UserMessageBO userMessageBO = new UserMessageBO(CurrentUserSession.NetSessionId);
            DataTable dataTable = userMessageBO.GetPopupMessageStatus();

            bool isPTChanged = false, isBettingLimitChanged = false;
            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    if (!dataTable.Columns.Contains("typeid"))
                    {
                        break;
                    }

                    int typeId = 0;

                    int.TryParse(row["typeid"] + string.Empty, out typeId);

                    // For PT Change
                    if (typeId == 1)
                    {
                        isPTChanged = true;
                    }
                    else if (typeId == 2)
                    {
                        // For Betting Limit Change
                        isBettingLimitChanged = true;
                    }
                }
            }

            if (isPTChanged || isBettingLimitChanged)
            {
                userMessageBO.UpdateViewedPTMessage(true);
            }

            return new object[] { creditBalance, isPTChanged, isBettingLimitChanged };
        }

        /// <summary>
        /// Render Popup Message
        /// </summary>
        /// <param name="tpl">template</param>
        /// <param name="isPTChanged"></param>
        /// <param name="isBettingLimitChanged"></param>
        private void RenderPopupMessage(CreditBalance_Template tpl, bool isPTChanged, bool isBettingLimitChanged)
        {
            // Show PT and Betting Limit modifying information for Agent customer
            if (isPTChanged && isBettingLimitChanged)
            {
                string message = Resources.Lang.modifiedMinPTBettingLimitValue;
                PopupMessage_Page popup = new PopupMessage_Page(message);
                tpl.AdditionalContent.Append(popup);
            }
            else
            {
                // Show PT modifying information for Agent customer
                if (isPTChanged)
                {
                    string message = Resources.Lang.modifiedMinPTValueText;
                    PopupMessage_Page popup = new PopupMessage_Page(message);
                    tpl.AdditionalContent.Append(popup);
                }

                // Show Betting Limit modifying information for Agent customer
                if (isBettingLimitChanged)
                {
                    string message = Resources.Lang.modifiedBettingLimitValue;
                    PopupMessage_Page popup = new PopupMessage_Page(message);
                    tpl.AdditionalContent.Append(popup);
                }
            }
        }

        private string CssClassName(decimal number)
        {
            return number >= 0 ? this.positive : this.negative;
        }

        private string CurrencyString(decimal number)
        {
            return this.currency + Formater.DecFormat(number, 2);
        }

        private bool IsOnlyAustTote()
        {
            switch (UserSession.Current.Currency)
            {
                case "SD$": return true;
                case "RM": return true;
                case "US$": return true;
                case "TB": return true;
                case "HK$": return true;
                case "EURO": return true;
                case "RMB": return true;
                case "IN": return true;
                case "Yen": return true;
                case "NTD": return true;
                case "UUS": return true;
                case "IND": return true;
                case "IDD": return true;
                case "IDR": return true;
                case "$US": return true;
                case "Y$US": return true;
                case "IRR": return true;
                case "THB": return true;
                case "DDD": return true;
                default: return false;
            }
        }

        #region Casino

        private void RenderCasino(CreditBalance_Template tpl, Hashtable hash)
        {
            if (!UserPermission.IsShowCasino)
            {
                tpl.Casino = null;
            }
            else
            {
                decimal totalcstransfer = (decimal)hash["totalcstransfer"];
                tpl.Casino.totalCasinoTransfer = this.CurrencyString(totalcstransfer);
                tpl.Casino.classTotalCasinoTransfer = this.CssClassName(totalcstransfer);

                decimal totalcsoutstanding = (decimal)hash["totalcsoutstanding"];
                tpl.Casino.totalCasinoOutstanding = this.CurrencyString(totalcsoutstanding);
                tpl.Casino.classTotalCasinoOutstanding = this.CssClassName(totalcsoutstanding);
            }
        }

        #endregion Casino

        #region Bingo

        private void RenderBingo(CreditBalance_Template tpl, Hashtable hash)
        {
            if (!UserPermission.IsShowBingo)
            {
                tpl.Bingo = null;
            }
            else
            {
                decimal totalbgtransfer = (decimal)hash["totalbitransfer"];
                tpl.Bingo.totalBingoTransfer = this.CurrencyString(totalbgtransfer);
                tpl.Bingo.classTotalBingoTransfer = this.CssClassName(totalbgtransfer);

                decimal totalbgoutstanding = (decimal)hash["totalbioutstanding"];
                tpl.Bingo.totalBingoOutstanding = this.CurrencyString(totalbgoutstanding);
                tpl.Bingo.classTotalBingoOutstanding = this.CssClassName(totalbgoutstanding);
            }
        }

        #endregion Bingo

        #region Super

        private void RenderSuper(CreditBalance_Template tpl, Hashtable hash)
        {
            tpl.reportBalance = Resources.Lang.lblSABInfo;

            tpl.outstanding = Resources.Lang.lblSAOutstanding;
            decimal superoutstanding = (decimal)hash["superoutstanding"];
            tpl.outstanding_value = this.CurrencyString(superoutstanding);
            tpl.classOutstanding = this.CssClassName(superoutstanding);

            tpl.credit = Resources.Lang.lblSACredit;
            decimal supercredit = (decimal)hash["supercredit"];
            tpl.credit_value = this.CurrencyString(supercredit);
            tpl.classCredit = this.CssClassName(supercredit);

            decimal totalmagentcredit = (decimal)hash["totalmagentcredit"];
            tpl.TotalMasterCredit.value = this.CurrencyString(totalmagentcredit);
            tpl.TotalMasterCredit.className = this.CssClassName(totalmagentcredit);

            decimal totalagentcredit = (decimal)hash["totalagentcredit"];
            tpl.TotalAgentCredit.value = this.CurrencyString(totalagentcredit);
            tpl.TotalAgentCredit.className = this.CssClassName(totalagentcredit);

            decimal totalmembercredit = (decimal)hash["totalmembercredit"];
            tpl.TotalMemberCredit.value = this.CurrencyString(totalmembercredit);
            tpl.TotalMemberCredit.className = this.CssClassName(totalmembercredit);

            tpl.TotalMasterActiveClosed.active = hash["totalmagentactive"].ToString();
            tpl.TotalMasterActiveClosed.closed = hash["totalmagentclosed"].ToString();
            tpl.TotalMasterActiveClosed.suspended = hash["totalmagentsuspended"].ToString();

            tpl.TotalAgentActiveClosed.active = hash["totalagentactive"].ToString();
            tpl.TotalAgentActiveClosed.closed = hash["totalagentclosed"].ToString();
            tpl.TotalAgentActiveClosed.suspended = hash["totalagentsuspended"].ToString();

            tpl.TotalMemberActiveClosed.active = hash["totalmemberactive"].ToString();
            tpl.TotalMemberActiveClosed.closed = hash["totalmemberclosed"].ToString();
            tpl.TotalMemberActiveClosed.suspended = hash["totalmembersuspended"].ToString();
        }

        #endregion Super

        #region Master

        private void RenderMaster(CreditBalance_Template tpl, Hashtable hash)
        {
            tpl.reportBalance = Resources.Lang.lblMBalInfo;

            tpl.outstanding = Resources.Lang.lblMasterOutstanding;
            decimal masteroutstanding = (decimal)hash["masteroutstanding"];
            tpl.outstanding_value = this.CurrencyString(masteroutstanding);
            tpl.classOutstanding = this.CssClassName(masteroutstanding);

            tpl.credit = Resources.Lang.lblMasterCredit;
            decimal mastercredit = (decimal)hash["mastercredit"];
            tpl.credit_value = this.CurrencyString(mastercredit);
            tpl.classCredit = this.CssClassName(mastercredit);

            tpl.TotalMasterCredit = null;

            decimal totalagentcredit = (decimal)hash["totalagentcredit"];
            tpl.TotalAgentCredit.value = this.CurrencyString(totalagentcredit);
            tpl.TotalAgentCredit.className = this.CssClassName(totalagentcredit);

            decimal totalmembercredit = (decimal)hash["totalmembercredit"];
            tpl.TotalMemberCredit.value = this.CurrencyString(totalmembercredit);
            tpl.TotalMemberCredit.className = this.CssClassName(totalmembercredit);

            tpl.TotalMasterActiveClosed = null;

            tpl.TotalAgentActiveClosed.active = hash["totalagentactive"].ToString();
            tpl.TotalAgentActiveClosed.closed = hash["totalagentclosed"].ToString();
            tpl.TotalAgentActiveClosed.suspended = hash["totalagentsuspended"].ToString();

            tpl.TotalMemberActiveClosed.active = hash["totalmemberactive"].ToString();
            tpl.TotalMemberActiveClosed.closed = hash["totalmemberclosed"].ToString();
            tpl.TotalMemberActiveClosed.suspended = hash["totalmembersuspended"].ToString();
        }

        #endregion Master

        #region Agent

        private void RenderAgent(CreditBalance_Template tpl, Hashtable hash)
        {
            tpl.reportBalance = Resources.Lang.lblABalInfo;

            tpl.outstanding = Resources.Lang.lblAgentOutstanding;
            decimal agentoutstanding = (decimal)hash["agentoutstanding"];
            tpl.outstanding_value = this.CurrencyString(agentoutstanding);
            tpl.classOutstanding = this.CssClassName(agentoutstanding);

            tpl.credit = Resources.Lang.lblAgentCredit;
            decimal agentcredit = (decimal)hash["agentcredit"];
            tpl.credit_value = this.CurrencyString(agentcredit);
            tpl.classCredit = this.CssClassName(agentcredit);

            tpl.TotalMasterCredit = null;
            tpl.TotalAgentCredit = null;

            decimal totalmembercredit = (decimal)hash["totalmembercredit"];
            tpl.TotalMemberCredit.value = this.CurrencyString(totalmembercredit);
            tpl.TotalMemberCredit.className = this.CssClassName(totalmembercredit);

            tpl.TotalMasterActiveClosed = null;
            tpl.TotalAgentActiveClosed = null;

            tpl.TotalMemberActiveClosed.active = hash["totalmemberactive"].ToString();
            tpl.TotalMemberActiveClosed.closed = hash["totalmemberclosed"].ToString();
            tpl.TotalMemberActiveClosed.suspended = hash["totalmembersuspended"].ToString();
        }

        #endregion Agent
    }
}