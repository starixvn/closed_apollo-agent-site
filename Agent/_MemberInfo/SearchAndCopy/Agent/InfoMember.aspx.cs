// --------------------------------------------------------------------------
// <copyright file="InfoMember.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2012.01.30">Improve styles</change>
// </history>

namespace Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Agent
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Fanex.Age.UI;
    using Fanex.Age.UI.Components;
    using Sunplus.Agent.Model;
    using Sunplus.Agent.Model.BL.AccountInfo;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.BL.MemberInfo.SearchAndCopy;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;
    using Sunplus.Agent.Model.Security;
    using Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Components.SelectedUserName;
    using Sunplus.Agent.Utilities;

    /// <summary>
    /// Enter infomation member
    /// </summary>
    public partial class MemberInfo_Page : SearchAndCopy_Page
    {
        #region Variable

        /// <summary>
        /// Template page infomation member
        /// </summary>
        private InfoMember_Template tpl = new InfoMember_Template();

        /// <summary>
        /// Check if skip review setting
        /// </summary>
        private string isSkip = string.Empty;

        /// <summary>
        /// Credit of upline
        /// </summary>
        private string credit = string.Empty;

        #endregion Variable

        #region Override

        /// <summary>
        ///  Render UI html
        /// </summary>
        /// <param name="writer">Html text writer</param>
        public override void RenderContent(HtmlTextWriter writer)
        {
            base.RenderContent(writer);

            DataTable infoMemberData = (View as Dictionary<string, DataTable>)["Data"];

            if (infoMemberData.Rows.Count > 0)
            {
                DataRow dr = infoMemberData.Rows[0];
                string newUserName = dr["NewUserName"] as string;
                string no3 = newUserName.Substring(2, 1).ToUpper();
                string no2 = newUserName.Substring(1, 1).ToUpper();
                string no1 = newUserName.Substring(0, 1).ToUpper();

                this.tpl.uname_parent = CurrentUserSession.UserName;
                this.credit = Formater.FormatNumber2(dr["credit_max"], CurrentUserSession.LangKey);
                this.tpl.credit_max = this.credit;
                bool isNextIndex = false;
                this.tpl.number3_val = new UserName_Selection(this.tpl, "Number3", no3, ref isNextIndex);
                this.tpl.number2_val = new UserName_Selection(this.tpl, "Number2", no2, ref isNextIndex);
                this.tpl.number1_val = new UserName_Selection(this.tpl, "Number1", no1, ref isNextIndex);

                this.tpl.back_function = "SearchAndCopy.Agent.Back()";
                this.tpl.submit_function = "SearchAndCopy.Agent.CopyUser()";

                RenderHeaderStep(out this.tpl.Header.header, 3);
            }

            HttpFrequenceRule rule1 = new HttpFrequenceRule("CopyMember.ashx", 20, TimeSpan.FromDays(2));
            if (!rule1.IsViolated)
            {
                this.tpl.Captchar = null;
            }

            string lblCreditInvalid = Resources.Lang.lblCreditInvalid.Replace("{0}", Resources.Lang.givencredit);
            lblCreditInvalid = lblCreditInvalid.Replace("{1}", "0");
            lblCreditInvalid = lblCreditInvalid.Replace("{2}", this.credit);

            RegisterJavascriptObject("userName", userName);
            RegisterJavascriptObject("custId", custId);
            RegisterJavascriptObject("isSkip", this.isSkip);
            RegisterJavascriptObject("isShowFI", "1");
            RegisterJavascriptObject("isShowCS", UserPermission.IsShowCasino ? "0" : "1");
            RegisterJavascriptObject("isShowHR", UserPermission.IsShowRacing ? "0" : "1");
            RegisterJavascriptObject("isShowBG", UserPermission.IsShowBingo ? "0" : "1");
            RegisterJavascriptObject("isLiveCS", UserPermission.IsShowLiveCasino ? "0" : "1");
            RegisterJavascriptObject("alertConfirmPasswordEmpty", Resources.Lang.alertPasswordEmpty);
            RegisterJavascriptObject("alertMemberCredit", Resources.Lang.alertMemberCredit);
            RegisterJavascriptObject("lblPasswordRequire", Resources.Lang.lblPasswordRequire);
            RegisterJavascriptObject("lblCreditInvalid", lblCreditInvalid);
            RegisterJavascriptObject("errRequireValidationCode", Resources.Lang.errRequireValidationCode);
            RegisterJavascriptObject("maxCredit", this.credit);

            writer.Write(this.tpl);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            AccountInfoBO bo = new AccountInfoBO(CurrentUserSession.NetSessionId);
            DataTable dtb = new DataTable();

            dtb = bo.Acc_CustomerSelAgentInfo(CurrentUserSession.CustId);
            viewData.Add("Data", dtb);

            return viewData;
        }

        /// <summary>
        /// Get para from request
        /// </summary>
        /// <param name="p">Hashtable</param>
        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            userName = GetStringRequest("userName", userName);
            custId = GetIntRequest("custId", custId);
            this.isSkip = GetStringRequest("isSkip", this.isSkip);
        }

        /// <summary>
        /// Validate para from request
        /// </summary>
        /// <param name="p">Hashtable</param>
        /// <returns>Null</returns>
        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (string.IsNullOrEmpty(userName))
            {
                userName = CurrentUserSession.UserName;
            }

            return null;
        }

        #endregion Override
    }
}