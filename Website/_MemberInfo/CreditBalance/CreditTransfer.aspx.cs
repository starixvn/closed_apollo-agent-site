// --------------------------------------------------------------------------
// <copyright file="CreditTransfer.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2012.05.30">Refactor code and check StyleCop</change>
// </history>

namespace Sunplus.Agent.UI._MemberInfo.CreditBalance
{
    using System;
    using System.Web.UI;
    using Resources;

    public partial class CreditTransfer_Page : ContextMenuColumnsBase
    {
        private CreditTransfer_Template template = new CreditTransfer_Template();
        private int custId = 0;
        private string userName = string.Empty, roleId = string.Empty, amt = string.Empty;

        public override void RenderContent(HtmlTextWriter writer)
        {
            this.template.UserName = this.userName;
            this.template.uus = Sunplus.Agent.Model.Entities.UserSession.Current.Currency;
            this.template.submit = Resources.Lang.submit;
            this.template.cancel = Resources.Lang.cancel;
            this.template.amtvalue = this.amt.Replace(",", string.Empty);
            this.template.custid = this.custId.ToString();
            this.template.roleId = this.roleId;

            this.RegisterJavascriptObject("lblConfirmClosed", Resources.Lang.lblConfirmClosed);
            this.RegisterJavascriptObject("lblconfirmclosesubacc", Resources.Lang.lblconfirmclosesubacc);
            writer.Write(this.template);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            this.userName = (this.Request["username"] == null || this.Request["username"] == "All" || this.Request["username"] == Lang.enterusername) ? string.Empty : this.Request["username"];
            this.userName = this.userName.Length == 0 ? string.Empty : this.userName;

            this.roleId = (this.Request["roleId"] == null) ? CurrentUserSession.SiteName : this.Request["roleId"];
            this.custId = (this.Request["custid"] == null) ? CurrentUserSession.CustId : Convert.ToInt32(this.Request["custid"]);
            this.amt = (this.Request["amt"] == null) ? "0" : this.Request["amt"];
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            return null;
        }
    }
}