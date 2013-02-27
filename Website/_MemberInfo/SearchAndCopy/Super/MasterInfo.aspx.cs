// --------------------------------------------------------------------------
// <copyright file="InfoMember.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2012.01.30">Improve styles</change>
// </history>

namespace Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Super
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

    public partial class MasterInfo_Page : SearchAndCopy_Page
    {
        #region Variable

        private MasterInfo_Template tpl = new MasterInfo_Template();
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
                string no1 = "0";
                string no2 = "1";
                string numbers = dr["NewUserName"].ToString();
                if (numbers.Length == 2)
                {
                    no1 = numbers.Substring(0, 1);
                    no2 = numbers.Substring(1, 1);
                }

                this.tpl.uname_parent = CurrentUserSession.UserName;
                this.credit = Formater.FormatNumber2(dr["credit_max"], CurrentUserSession.LangKey);
                this.tpl.credit_max = this.credit;
                bool isNextIndex = false;
                this.tpl.number2_val = new UserName_Selection(this.tpl, "Number2", no2, ref isNextIndex);
                this.tpl.number1_val = new UserName_Selection(this.tpl, "Number1", no1, ref isNextIndex);

                this.tpl.back_function = "SearchAndCopy.Master.Back()";
                this.tpl.submit_function = "SearchAndCopy.Master.CopyUser()";

                RenderHeaderStep(out this.tpl.Header.header, 3);
            }

            string lblCreditInvalid = string.Format(Resources.Lang.masterMaxCreditInvalid, 0, this.credit);

            RegisterJavascriptObject("userName", userName);
            RegisterJavascriptObject("custId", custId);
            RegisterJavascriptObject("isSkip", this.isSkip);
            RegisterJavascriptObject("isShowFI", "1");
            RegisterJavascriptObject("isShowCS", UserPermission.IsShowCasino ? "0" : "1");
            RegisterJavascriptObject("isShowHR", UserPermission.IsShowRacing ? "0" : "1");
            RegisterJavascriptObject("isShowBG", UserPermission.IsShowBingo ? "0" : "1");
            RegisterJavascriptObject("isLiveCS", UserPermission.IsShowLiveCasino ? "0" : "1");
            RegisterJavascriptObject("alertConfirmPasswordEmpty", Resources.Lang.alertPasswordEmpty);
            RegisterJavascriptObject("lblPasswordRequire", Resources.Lang.lblPasswordRequire);
            RegisterJavascriptObject("alertMasterCredit", Resources.Lang.alertMasterCredit);
            RegisterJavascriptObject("lblCreditInvalid", lblCreditInvalid);
            RegisterJavascriptObject("maxCredit", this.credit);

            writer.Write(this.tpl);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            AccountInfoBO bo = new AccountInfoBO(CurrentUserSession.NetSessionId);
            DataTable dtb = new DataTable();
            dtb = bo.Acc_CustomerSelSAgentInfo(CurrentUserSession.CustId);

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