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
using Fanex.Data.DNAide;
using DNAide.Web;
using Sunplus.Agent.Model.Entities.Permission;
public partial class MemberInfo_MemberCommissionRacing : TplPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.CssGroup = "main";
        
        string idmaster = (Request["idmaster"] == null) ? "" : Request["idmaster"];
        string idgent = (Request["idagent"] == null) ? "" : Request["idagent"];
        string content = MemberInfo_Downline.MemberCommissionForRacing(idmaster, idgent);

        if (!UserPermission.IsShowRacing_151_31)
            TemplateHandler.RemoveBlock(ref content, "SAHorse");
        if (!UserPermission.IsShowRacing_151_41)
            TemplateHandler.RemoveBlock(ref content, "UKHorse");
        if (!UserPermission.IsShowRacing_151_51)
            TemplateHandler.RemoveBlock(ref content, "AUHorse");
        if (!UserPermission.IsShowRacing_152_41)
            TemplateHandler.RemoveBlock(ref content, "UKGreyhound");
        if (!UserPermission.IsShowRacing_152_51)
            TemplateHandler.RemoveBlock(ref content, "AUGreyhound");
        if (!UserPermission.IsShowRacing_153_51)
            TemplateHandler.RemoveBlock(ref content, "AUHarness");

        //Load CSS for freeze headline
        string styleSheet = string.Empty;
        string cellspacing = "1";              
        TemplateHandler.Assign(ref content, "StyleSheet", styleSheet);
        TemplateHandler.Assign(ref content, "cellspacing", cellspacing);
       

        this.TplSetContent(content);
    }
}
