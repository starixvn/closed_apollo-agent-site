// --------------------------------------------------------------------------
// <copyright file="InfoMember.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2012.01.30">Improve styles</change>
// </history>

namespace Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Master
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
    using Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Components.SelectedUserName;
    using Sunplus.Agent.Utilities;

    public partial class AgentInfo_Page : SearchAndCopy_Page
    {
        #region Variable

        private InfoAgent_Template tpl = new InfoAgent_Template();
        private string isSkip = string.Empty;
        private string credit = string.Empty;

        #endregion Variable

        #region Override

        public override void RenderContent(HtmlTextWriter writer)
        {
            base.RenderContent(writer);

            DataTable infoMemberData = (View as Dictionary<string, DataTable>)["Data"];

            if (infoMemberData.Rows.Count > 0)
            {
                DataRow dr = infoMemberData.Rows[0];
                string newUserName = dr["NewUserName"] as string;
                string no2 = newUserName.Substring(1, 1).ToUpper();
                string no1 = newUserName.Substring(0, 1).ToUpper();

                this.tpl.uname_parent = CurrentUserSession.UserName;
                this.credit = Formater.FormatNumber2(dr["credit_max"], CurrentUserSession.LangKey);
                this.tpl.agent_credit_max = this.credit;
                this.tpl.member_credit_max = "0";
                bool isNextIndex = false;

                this.tpl.number2_val = new UserName_Selection(this.tpl, "Number2", no2, ref isNextIndex);
                this.tpl.number1_val = new UserName_Selection(this.tpl, "Number1", no1, ref isNextIndex);

                this.tpl.back_function = "SearchAndCopy.Agent.Back()";
                this.tpl.submit_function = "SearchAndCopy.Agent.CopyUser()";

                RenderHeaderStep(out this.tpl.Header.header, 3);
            }

            string lblCreditInvalidMember = Resources.Lang.memberMaxCreditInvalid;
            lblCreditInvalidMember = lblCreditInvalidMember.Replace("{0}", "0");
            string lblCreditInvalidAgent = Resources.Lang.agentMaxCreditInvalid;
            lblCreditInvalidAgent = lblCreditInvalidAgent.Replace("{0}", "0");
            lblCreditInvalidAgent = lblCreditInvalidAgent.Replace("{1}", this.credit);

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
            RegisterJavascriptObject("lblCreditInvalidMember", lblCreditInvalidMember);
            RegisterJavascriptObject("lblCreditInvalidAgent", lblCreditInvalidAgent);
            RegisterJavascriptObject("maxCredit", this.credit);

            writer.Write(this.tpl);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            AccountInfoBO bo = new AccountInfoBO(CurrentUserSession.NetSessionId);
            DataTable dtb = new DataTable();
            dtb = bo.Acc_CustomerSelMAgentInfo(CurrentUserSession.CustId);

            viewData.Add("Data", dtb);
            return viewData;
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            userName = GetStringRequest("userName", userName);
            custId = GetIntRequest("custId", custId);
            this.isSkip = GetStringRequest("isSkip", this.isSkip);
        }

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