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
public partial class MemberInfo_MemberCommission : TplPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.CssGroup = "main";
        
        string idmaster = (Request["idmaster"] == null) ? "" : Request["idmaster"];
        string idgent = (Request["idagent"] == null) ? "" : Request["idagent"];
        string content = MemberInfo_Downline.memberCommission(idmaster, idgent);
        if (!Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing)
        {
            TemplateHandler.RemoveBlock(ref content, "hr-comm");
        }
        //Load CSS for freeze headline
        string styleSheet = string.Empty;
        string cellspacing = "1";              
        TemplateHandler.Assign(ref content, "StyleSheet", styleSheet);
        TemplateHandler.Assign(ref content, "cellspacing", cellspacing);
       

        this.TplSetContent(content);
    }
}
