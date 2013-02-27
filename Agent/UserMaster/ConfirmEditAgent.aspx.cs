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

public partial class Super_ConfirmEditAgent : TplPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string temp = TemplateHandler.LoadTemplate("MemberInfo/ConfirmEditAutoPT.html");
        TemplateHandler.Assign(ref temp, "information", Resources.Lang.lblConfirmEditMaster);
        this.TplSetContent(temp); 
    }
}
