using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fanex.Common;

public partial class _MemberInfo_CustomerSetting_Pages_EditAgentPositionTaking : System.Web.UI.Page
{
    public int AgentId { get; set; }
    public int RoleId { get; set; }
    public int TargetAgentId { get; set; }
    public int TargetRoleId { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        TargetAgentId = Converter.ToInt(Context.Request.QueryString["custid"]);
        RoleId = (int)Fanex.CustSetting.ViewData.Role.Agent;
        TargetRoleId = (int)Fanex.CustSetting.ViewData.Role.Agent;
    }
}