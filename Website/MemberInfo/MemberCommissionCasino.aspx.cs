using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using DNAide.Web;
using DNAide.Core;
using CasinoLibrary;
using CasinoLibrary.SBAgent;
using CasinoLibrary.Common;
using CasinoLibrary.Model;
using System.Text;

public partial class MemberInfo_MemberCommissionCasino : TplPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.CssGroup = "main";
        if (!Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowCasino) return;

        string idmaster = (Request["idmaster"] == null) ? "" : Request["idmaster"];
        string idgent = (Request["idagent"] == null) ? "" : Request["idagent"];
        string content = MemberInfo_Downline.memberCommissionCasino(idmaster, idgent);
        //Load CSS for freeze headline
        string styleSheet = string.Empty;
        string cellspacing = "1";
        if (Request.Browser.Browser == "IE")
        {
            styleSheet = TemplateHandler.LoadTemplate("IEReportCSS.html");
            this.IsXHtml = false;
            cellspacing = "0";
        }
        TemplateHandler.Assign(ref content, "StyleSheet", styleSheet);
        TemplateHandler.Assign(ref content, "cellspacing", cellspacing);

        this.TplSetContent(content);
    }
}
