using DNAide.Web;
using DNAide.Core;
using CasinoLibrary;
using CasinoLibrary.SBAgent;
using CasinoLibrary.Common;
using CasinoLibrary.Model;
using System.Text;
using System;
using Sunplus.Agent.Model.Entities.Permission;

public partial class MemberInfo_MemberCommissionBingo : TplPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.CssGroup = "main";
        if (!UserPermission.IsShowBingo) return;

        string idmaster = (Request["idmaster"] == null) ? "" : Request["idmaster"];
        string idgent = (Request["idagent"] == null) ? "" : Request["idagent"];
        string content = MemberInfo_Downline.memberCommissionBingo(idmaster, idgent);

        string styleSheet = string.Empty;
        string cellspacing = "1";


        TemplateHandler.Assign(ref content, "StyleSheet", styleSheet);
        TemplateHandler.Assign(ref content, "cellspacing", cellspacing);

        this.TplSetContent(content);
    }
}
