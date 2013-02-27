using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DNAide.Web;
using DNAide.Utils;
using DNAide.Core;
public partial class Popup_CreditTransfer : TplPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.CssGroup = "popup";

        string sitetype = (Request["sitetype"] == null) ? Sunplus.Agent.Model.Entities.UserSession.Current.SiteName : Request["sitetype"];
        string custid = (Request["custid"] == null) ? Sunplus.Agent.Model.Entities.UserSession.Current.CustId.ToString() : Request["custid"];
        string username = (Request["usn"] == null) ? "" : Request["usn"];
        string amt = (Request["amt"] == null) ? "0" : Request["amt"];
        string main = TemplateHandler.LoadTemplate("Popup/TransferCreditBalance.html");
        TemplateHandler.Assign(ref main, "transfer", username);
        TemplateHandler.Assign(ref main, "uus", Sunplus.Agent.Model.Entities.UserSession.Current.Currency);
        TemplateHandler.Assign(ref main, "submit", Resources.Lang.submit);
        TemplateHandler.Assign(ref main, "cancel", Resources.Lang.cancel);               
        TemplateHandler.Assign(ref main, "transfer", username);
        TemplateHandler.Assign(ref main, "amtvalue", amt.Replace(",",""));
        TemplateHandler.Assign(ref main, "custid", custid);
        TemplateHandler.Assign(ref main, "sitetype", sitetype);
        this.TplSetContent(main);

        AddJsVariable("lblConfirmClosed", Resources.Lang.lblConfirmClosed);
        AddJsVariable("lblconfirmclosesubacc", Resources.Lang.lblconfirmclosesubacc);
    }
}
