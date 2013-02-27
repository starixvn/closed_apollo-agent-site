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
using Fanex.Data.DNAide;
using DNAide.Core;
using DNAide.Utils;
public partial class Popup_TransYesBalance : TplPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.CssGroup = "popup"; 

        string _main = TemplateHandler.LoadTemplate("Popup/TransYesBalance.html");        
        string errorTpl = TemplateHandler.LoadTemplate("ErrorMsg.html");
        TemplateHandler.Assign(ref _main, "error-message", errorTpl);

        TemplateHandler.Assign(ref _main, "transfer", Resources.Lang.transfer);
        TemplateHandler.Assign(ref _main, "amount", Resources.Lang.amount);
        TemplateHandler.Assign(ref _main, "submit", Resources.Lang.submit);
        TemplateHandler.Assign(ref _main, "cancel", Resources.Lang.cancel);
        Double amt = (Request["amt"] == null) ? 0 : CastAide.AsDouble(Request["amt"]);
        string custid = CastAide.AsString(Request["custid"]);
        string username = CastAide.AsString(Request["usn"]);
        string isclose = (Request["status"] == null) ? "1" : CastAide.AsString(Request["status"]);
        TemplateHandler.Assign(ref _main, "amtvalue", amt * (-1));
        TemplateHandler.Assign(ref _main, "custid", custid);
        TemplateHandler.Assign(ref _main, "usn_value", username);
        TemplateHandler.Assign(ref _main, "isclose_value", isclose);

        if (!String.IsNullOrEmpty(Request.QueryString["nobackground"])) //come from AccountInfo tab
        {
            TemplateHandler.RemoveBlock(ref _main, "cancelbtn");
            TemplateHandler.Assign(ref _main, "tablealign", "left");
        }
        else
        {
            TemplateHandler.RemoveBlock(ref _main, "nobackground");
            TemplateHandler.Assign(ref _main, "tablealign", "center");
        }

        this.TplSetContent(_main);

      
        AddJsVariable("wrnconfirmtranfer", Resources.Lang.confirmtransfer + "?");
        AddJsVariable("transfersuccessful", Resources.Lang.transfersuccessful);
        AddJsVariable("lblconfirmclosesubacc", Resources.Lang.lblconfirmclosesubacc);
        AddJsVariable("lblConfirmClosed", Resources.Lang.lblConfirmClosed);
    }
}
