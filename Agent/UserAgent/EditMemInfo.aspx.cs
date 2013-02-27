using System;
using System.Data;
using System.Web;

using DNAide.Web;
using DNAide.Core;
using IBCAgent;
using Fanex.Data.DNAide;

public partial class EditMemInfo : TplPage
{
    protected int CustId
    {
        get
        {
            return CastAide.AsInt32(Request.QueryString["custId"], -1);
        }
    }

    protected string CustName
    {
        get
        {
            return Request.QueryString["username"];
        }
    }

    protected int IsCustSync
    {
        get
        {
            return CastAide.AsInt32(Request.QueryString["isCustSync"], 1);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.CssGroup = "popup";

        //Template
        string template = TemplateHandler.LoadTemplate("UserInfo/MemberInfo.html");
        string errorTpl = TemplateHandler.LoadTemplate("ErrorMsg.html");

        TemplateHandler.Assign(ref template, "error-message", errorTpl);

        //Languages
        TemplateHandler.Assign(ref template, "lbleditcustinfo", Resources.Lang.editcustinfo);
        TemplateHandler.Assign(ref template, "lbleditinfo", Resources.Lang.lblEditInfo + " " + CustName);
        TemplateHandler.Assign(ref template, "fname-text", Resources.Lang.firstname);
        TemplateHandler.Assign(ref template, "lname-text", Resources.Lang.lastname);
        TemplateHandler.Assign(ref template, "phone-text", Resources.Lang.lblPhone);
        TemplateHandler.Assign(ref template, "mobile-text", Resources.Lang.lblMobilePhone);        
        TemplateHandler.Assign(ref template, "email-text", Resources.Lang.email);
        TemplateHandler.Assign(ref template, "fax-text", Resources.Lang.lblFax);
        TemplateHandler.Assign(ref template, "update", Resources.Lang.update);
        TemplateHandler.Assign(ref template, "reset", Resources.Lang.reset);
        TemplateHandler.Assign(ref template, "loading", Resources.Lang.processing);

        DbResult dr = UserInfo.GetCustomerByInfo(this.CustId);
        DataRow row = dr.ResultSet.Tables[0].Rows[0];

        //Handlers
        TemplateHandler.Assign(ref template, "updateinfohandler",
                String.Format("../Handlers/UpdateMemInfo.ashx?custid={0}&s={1}&c={2}&isCustSync={3}", this.CustId, row["suspend"], row["closed"], IsCustSync)
            );
            //"../Handlers/UpdateMemInfo.ashx?custid=" + this.CustId + "&s=" + row["suspend"] + "&c=" + row["closed"]);


        //Load info
        this.LoadUserInfo(ref template, row);

        this.TplSetContent(template);

        AddJsVariable("emailnotvalid", Resources.Lang.emailnotvalid);
    }


    public void LoadUserInfo(ref string template, DataRow row)
    {

        TemplateHandler.Assign(ref template, "fname-val", HttpUtility.HtmlEncode(CastAide.AsString(row["firstname"], String.Empty)));
        TemplateHandler.Assign(ref template, "lname-val", HttpUtility.HtmlAttributeEncode(CastAide.AsString(row["lastname"], String.Empty)));
        TemplateHandler.Assign(ref template, "phone-val", HttpUtility.HtmlAttributeEncode(CastAide.AsString(row["phone"], String.Empty)));
        TemplateHandler.Assign(ref template, "mobile-val", HttpUtility.HtmlAttributeEncode(CastAide.AsString(row["mobilephone"], String.Empty)));
        TemplateHandler.Assign(ref template, "fax-val", HttpUtility.HtmlAttributeEncode(CastAide.AsString(row["fax"], String.Empty)));
        TemplateHandler.Assign(ref template, "email-val", HttpUtility.HtmlAttributeEncode(CastAide.AsString(row["email"], String.Empty)));
    }
}
