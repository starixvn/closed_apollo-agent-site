// --------------------------------------------------------------------------
// <copyright file="InfoMember.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2012.05.30">Improve performance, check StypeCop</change>
// </history>

namespace Sunplus.Agent.UI._Transfer.Super
{
    using System;
    using System.Web.UI;

    public partial class Popup_MasterTransfer : Transfer_Page
    {
        #region Variable

        private MasterTransfer_Template tpl = new MasterTransfer_Template();
        private string isClose = string.Empty;
        private string custId = string.Empty;
        private double amt = 0;

        #endregion

        #region Override
        public override void RenderContent(HtmlTextWriter writer)
        {
            RegisterJavascriptObject("wrnTransferSuccessful", Resources.Lang.transfersuccessful);
            RegisterJavascriptObject("invalidValue", Resources.Lang.invalidvalue);
            RegisterJavascriptObject("lblconfirmclosesubacc", Resources.Lang.lblconfirmclosesubacc);
            RegisterJavascriptObject("lblConfirmClosed", Resources.Lang.lblConfirmClosed);
            RegisterJavascriptObject("wrnTransfer", Resources.Lang.confirmtransfer + "?");
            RegisterJavascriptObject("custid", this.custId);
            RegisterJavascriptObject("isclose", this.isClose);
            RegisterJavascriptObject("amt", this.amt);
            RegisterJavascriptObject("CallBack", Request["CallBack"] == null ? "parent.OnPopupComplete(true)" : Request["CallBack"]);

            if (Request["CallBack"] != null)
            {
                this.tpl.CancelBtn = null;
            }
            else
            {
                this.tpl.Header = null;
            }

            this.RenderContentBlock();
            writer.Write(this.tpl);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            return null;
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            this.custId = GetStringRequest("custid", CurrentUserSession.CustId.ToString());
            this.isClose = (Request["status"] == null) ? "1" : Convert.ToString(Request["status"]);
            this.amt = (Request["amt"] == null) ? 0 : Convert.ToDouble(Request["amt"]);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }
        #endregion

        #region Utility

        public void RenderContentBlock()
        {
            this.tpl.amtvalue = Convert.ToString(this.amt * (-1));
        }
        #endregion
    }
}