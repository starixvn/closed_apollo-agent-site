// --------------------------------------------------------------------------
// <copyright file="InfoMember.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2012.05.30">Improve performance, check StypeCop</change>
// </history>

namespace Sunplus.Agent.UI._Transfer.Agent
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web.UI;
    using Fanex.Age.UI;
    using Fanex.Age.UI.Components;

    using Sunplus.Agent.Model.BL.Transfer;
    using Sunplus.Agent.Utilities;

    public partial class Popup_MemberTransfer : Transfer_Page
    {
        #region Variable

        private MemberTransfer_Template tpl = new MemberTransfer_Template();
        private string custId = string.Empty;
        private double amt = 0;
        private bool isDisableCasino = false;
        private bool isSyncCasino = false;
        private Dictionary<string, Hashtable> viewData = new Dictionary<string, Hashtable>();
        private StringBuilder wrntransfer = new StringBuilder();
        #endregion

        #region Override
        public override void RenderContent(HtmlTextWriter writer)
        {
            Hashtable transferData = (View as Dictionary<string, Hashtable>)["TransferData"];
            this.RenderContentBlock(transferData);
            RegisterJavascriptObject("wrntransfersuccessful", Resources.Lang.transfersuccessful);
            RegisterJavascriptObject("lblconfirmclosesubacc", Resources.Lang.lblconfirmclosesubacc);
            RegisterJavascriptObject("lblConfirmClosed", Resources.Lang.lblConfirmClosed);
            RegisterJavascriptObject("invalidValue", Resources.Lang.invalidvalue);
            RegisterJavascriptObject("wrnResetMaxTransfer", Resources.Lang.resetmaxtranfer + " " + Resources.Lang.confirmtransfer + "?");
            RegisterJavascriptObject("wrnTransfer", Resources.Lang.confirmtransfer + "?");
            
            RegisterJavascriptObject("custId", this.custId);
            RegisterJavascriptObject("amt", this.amt * (-1));
            RegisterJavascriptObject("isSyncCasino", this.isSyncCasino);
            RegisterJavascriptObject("disableCasino", this.isDisableCasino);
            RegisterJavascriptObject("CallBack", Request["CallBack"] == null ? "parent.OnPopupComplete(true)" : Request["CallBack"]);

            this.tpl.accinfo = Resources.Lang.lblAccountInformation;
            this.tpl.warning_message = null;

            writer.Write(this.tpl);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            TransferBO bo = new TransferBO(CurrentUserSession.NetSessionId);

            Hashtable htb = bo.TransferSelMemberInfoYesterday(this.custId);

            this.viewData.Add("TransferData", htb);
            return this.viewData;
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            this.custId = Request["custid"];
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (string.IsNullOrEmpty(this.custId))
            {
                return new ErrorPage_Page(Resources.Lang.strNotAccessPage);
            }

            return null;
        }
        #endregion

        #region Utility

        private void RenderContentBlock(Hashtable htb)
        {
            this.tpl.customer_value = htb["username"].ToString();
            this.tpl.usernameto = CurrentUserSession.UserName;
            this.tpl.cashbalance_value = Formater.FormatNumber2(htb["cashbalance"], langKey);
            this.tpl.outtxn_value = Formater.FormatNumber2(htb["oustanding"], langKey);
            this.tpl.given_credit_value = Formater.FormatNumber2(htb["givencredit"], langKey);
            this.tpl.bet_credit_value = Formater.FormatNumber2(htb["betcredit"], langKey);
            this.tpl.transferamount_value = Formater.FormatNumber2(htb["amttransferred"], langKey);
            this.tpl.lastlogin_value = htb["lastlogin"].ToString();
            this.tpl.lasttxn_value = htb["maxtransdate"].ToString();
            this.tpl.amtvalue = (Convert.ToDouble(htb["balance"]) * (-1)).ToString();
            this.tpl.transfermaximum = Resources.Lang.youcantransfer + " " + (Convert.ToDouble(htb["balance"]) * (-1)).ToString();
            bool.TryParse(htb["synccasino"].ToString(), out this.isSyncCasino);
            bool.TryParse(htb["disablecasino"].ToString(), out this.isDisableCasino);
            double.TryParse(htb["balance"].ToString(), out this.amt);
        }
        #endregion
    }
}