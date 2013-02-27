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
using DNAide.Core;
public partial class MemberInfo_creditbalance : TplPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        this.CssGroup = "main";
        string site_list = "";

        if (AppSettings.IsSuperSite)
        {           
            site_list = (Request["act"] == null) ? Common.GetLevel(Sunplus.Agent.Model.Entities.UserSession.Current.RoleId) : Request["act"];
        }
        else if (AppSettings.IsMasterSite)
        {
            site_list = (Request["act"] == null) ? "AgentList" : Request["act"];
        }
        else
        {
            site_list = (Request["act"] == null) ? "" : Request["act"];
        }
        int pageId = (Request["page"] == null) ? 1 : CastAide.AsInt32(Request["page"]);
        string custid = (Request["custid"] == null) ? Sunplus.Agent.Model.Entities.UserSession.Current.CustId.ToString() : Request["custid"];
        string search = (Request["search"] == null) ? string.Empty : Request["search"];
        string username = (Request["username"] == null || Request["username"] == "All" || Request["username"] == Resources.Lang.enterusername) ? "" : Request["username"];
        username = (username.Length == 0 ? "" : username);
        int status = (Request["status"] == null) ? 1 : CastAide.AsInt32(Request["status"]);
        int totalpages = CastAide.AsInt32(Request["total"], 0);
        string content = MemberInfo_Downline.CreditBalance(custid, site_list, pageId, search, username, status, totalpages);

        //Load CSS for freeze headline
        string styleSheet = string.Empty;
        string cellspacing = "1";       
        
        TemplateHandler.Assign(ref content, "StyleSheet", styleSheet);
        TemplateHandler.Assign(ref content, "cellspacing", cellspacing);
        TemplateHandler.Assign(ref content, "lblprint", Resources.Lang.print);

        //print function
        string printerHtml = TemplateHandler.LoadTemplate("Print.html");
        string printer = TemplateHandler.CopyBlock(printerHtml, "printer");
        string printcss = TemplateHandler.CopyBlock(printerHtml, "printcss");
        TemplateHandler.Assign(ref printer, "lblprint", Resources.Lang.print);
        TemplateHandler.Assign(ref content, "printer", printer);
        TemplateHandler.Assign(ref content, "printcss", printcss);
        //--------------------------

        content = Sunplus.Agent.Model.Common.AgentIPFormat.FormatIPs(content);


        content += "<div class=\"shadow\" id=\"shadow\" /><div class=\"output\" id=\"output\" />";

        this.TplSetContent(content);
        
        //CreditBalance
    }
}
