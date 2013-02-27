using System;
using DNAide.Web;

public partial class MemberInfo_BingoPositionTaking : TplPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.CssGroup = "main";
        string custid = string.Empty;
        string type = string.Empty;
        string main = string.Empty;
        type = (Request.QueryString["type"] == null) ? "" : Request.QueryString["type"].ToString();
        custid = (Request.QueryString["custid"] == null) ? Sunplus.Agent.Model.Entities.UserSession.Current.CustId.ToString() : Request.QueryString["custid"].ToString();
        main = MemberInfo_Downline.GetBingoPositionTaking(type, custid);

        //Load CSS for freeze headline
        string styleSheet = string.Empty;
        string cellspacing = "1";

        TemplateHandler.Assign(ref main, "StyleSheet", styleSheet);
        TemplateHandler.Assign(ref main, "cellspacing", cellspacing);

        this.TplSetContent(main);
    }
}
