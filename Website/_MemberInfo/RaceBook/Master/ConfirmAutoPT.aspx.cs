// --------------------------------------------------------------------------
// <copyright file="ConfirmAutoPT.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2012.06.01">Check StypeCop</change>
// </history>

namespace Sunplus.Agent.UI._MemberInfo.RaceBook.Super
{
    using System;
    using Sunplus.Agent.UI.PositionTaking.RaceBook;

    public partial class ConfirmAutoPT_Page : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ConfirmAutoChanged_Template tpl = new ConfirmAutoChanged_Template();
            MasterPage_Template tplMaster = new MasterPage_Template();
            tplMaster.RaceBookJs.extJs = "ConfirmAutoPT.js" + "?" + Sunplus.Agent.Model.BL.Definitions.AppSettings.JsVersion;
            tplMaster.RaceBookJs.ExtJs_More = null;
            tplMaster.title = null;
            tpl.confirmMessage = Resources.Lang.lblConfirmEditAgent;
            tplMaster.content = tpl;
            tplMaster.hidden_PT = string.Empty;
            Response.Write(tplMaster);
        }
    }
}