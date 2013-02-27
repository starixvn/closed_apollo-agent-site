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
using Sunplus.Agent.Model.Entities;
public partial class Popup_TransferCondition : TplPage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.CssGroup = "popup";

        string _main = TemplateHandler.LoadTemplate("Popup/TransferCondition.html");
        string errorTpl = TemplateHandler.LoadTemplate("ErrorMsg.html");
        TemplateHandler.Assign(ref _main, "error-message", errorTpl);

        TemplateHandler.Assign(ref _main, "daily", Resources.Lang.daily);
        TemplateHandler.Assign(ref _main, "weekly", Resources.Lang.weekly);
        TemplateHandler.Assign(ref _main, "sun", Resources.Lang.sun);
        TemplateHandler.Assign(ref _main, "mon", Resources.Lang.mon);
        TemplateHandler.Assign(ref _main, "tue", Resources.Lang.tue);
        TemplateHandler.Assign(ref _main, "thur", Resources.Lang.thurs);
        TemplateHandler.Assign(ref _main, "wed", Resources.Lang.wed);
        TemplateHandler.Assign(ref _main, "fri", Resources.Lang.fri);
        TemplateHandler.Assign(ref _main, "sat", Resources.Lang.sat);
        TemplateHandler.Assign(ref _main, "TransferCondition", Resources.Lang.accbalancetrancondition);
        TemplateHandler.Assign(ref _main, "choosedaily", Resources.Lang.choonsedaily);
        TemplateHandler.Assign(ref _main, "chooseoneday", Resources.Lang.chooseoneday);

        if (!String.IsNullOrEmpty(Request.QueryString["nobackground"])) //come from AccountInfo tab
        {
            TemplateHandler.Assign(ref _main, "tblalign", "left");
        }
        else
        {
            TemplateHandler.RemoveBlock(ref _main, "nobackground");
            TemplateHandler.Assign(ref _main, "tblalign", "center");
        }

        if (AppSettings.IsSuperSite)
        {
            TemplateHandler.Assign(ref _main, "confirm", Resources.Lang.confirmtranscondition);
        }
        else
        {
            TemplateHandler.Assign(ref _main, "confirm", Resources.Lang.confrmtransnotdownline);
        }
        TemplateHandler.Assign(ref _main, "submit", Resources.Lang.submit); 

        string custid = (Request["custid"] == null) ? "" : Request["custid"];
        TemplateHandler.Assign(ref _main, "custid", custid); 
        string conditiontime = MemberInfo.GetDataTransferCondition(custid);
        char[] ArrTime = conditiontime.ToCharArray();
        string[] strArr ={ "chksunchecked", "chkmonchecked", "chktuechecked", "chkwedchecked", "chkthurchecked", "chkfrichecked", "chksatchecked" };
        if (ArrTime[7].ToString() == "1")
        {

            TemplateHandler.Assign(ref _main, "rdbweeklychecked", "checked");
            TemplateHandler.Assign(ref _main, "rdbdailychecked", string.Empty);
            for (int i = 0; i < ArrTime.Length - 1; i++)
            {
                if (ArrTime[i].ToString() == "1")
                {
                    TemplateHandler.Assign(ref _main, strArr[i], "checked");
                    TemplateHandler.Assign(ref _main, strArr[i] + "_value", "1");
                }
                else
                {
                    TemplateHandler.Assign(ref _main, strArr[i], string.Empty);
                    TemplateHandler.Assign(ref _main, strArr[i] + "_value", "0");
                }
            }
        
        }
        else
        {

            TemplateHandler.Assign(ref _main, "rdbweeklychecked", string.Empty);
            TemplateHandler.Assign(ref _main, "rdbdailychecked", "checked");
            for (int i = 0; i < strArr.Length; i++)
            {
                TemplateHandler.Assign(ref _main, strArr[i], "disabled");
                TemplateHandler.Assign(ref _main, strArr[i] + "_value", "0");
            }
        }        

        this.TplSetContent(_main);

        AddJsVariable("transfersuccessful", Resources.Lang.transfersuccessful);
        AddJsVariable("lblconfirmclosesubacc", Resources.Lang.lblconfirmclosesubacc);
        AddJsVariable("lblConfirmClosed", Resources.Lang.lblConfirmClosed);
    }
}
