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
using IBCAgent;
using Fanex.Data.DNAide;
using DNAide.Utils;
using Sunplus.Agent.Model.Entities.Permission;
using Sunplus.Agent.Model.Entities;

public partial class EditAgentCommission : TplPage
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

    protected int RoleID
    {
        get
        {
            return CastAide.AsInt32(Request.QueryString["roleId"], -1);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        this.CssGroup = "popup";

        //Template
        string template = TemplateHandler.LoadTemplate("UserInfo/AgentCommission.html");
        string errorTpl = TemplateHandler.LoadTemplate("ErrorMsg.html");
        TemplateHandler.Assign(ref template, "error-msg", errorTpl);

        //Languages
        TemplateHandler.Assign(ref template, "lbleditcomm", Resources.Lang.lbleditcomm + " " + CustName);
        TemplateHandler.Assign(ref template, "lbleditcustcomm", Resources.Lang.lblCommissionForAgent);        
        TemplateHandler.Assign(ref template, "lblgroupa", Resources.Lang.lblGroupa);
        TemplateHandler.Assign(ref template, "lblgroupb", Resources.Lang.lblGroupb);
        TemplateHandler.Assign(ref template, "lblgroupc", Resources.Lang.lblGroupc);
        TemplateHandler.Assign(ref template, "lblgroupd", Resources.Lang.lblGroupd);
        TemplateHandler.Assign(ref template, "lblgroup1x2", Resources.Lang.lbl1x2Comm);
        TemplateHandler.Assign(ref template, "lblgroupother", Resources.Lang.lblOtherComm);
        if (Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission || !UserPermission.IsShowNumberGame)
        {
            TemplateHandler.Assign(ref template, "lblgroupnumber", string.Empty);
        }
        else
        {
            TemplateHandler.Assign(ref template, "lblgroupnumber", Resources.Lang.numbergamecomm);
        }
        TemplateHandler.Assign(ref template, "update", Resources.Lang.update);
        TemplateHandler.Assign(ref template, "reset", Resources.Lang.reset);
        TemplateHandler.Assign(ref template, "loading", Resources.Lang.processing);

        //Handlers
        TemplateHandler.Assign(ref template, "updateinfohandler", String.Format("../Handlers/UpdateAgentCommission.ashx?custid={0}&roleId={1}",this.CustId, this.RoleID));

        //Load commission
        this.LoadUserCommission(ref template);

        this.TplSetContent(template);
    }

    public void LoadUserCommission(ref string template)
    {
        int userId = Sunplus.Agent.Model.Entities.UserSession.Current.CustId;
        DbResult dr = UserInfo.SelectCommisionByCustomer(this.CustId, userId);
        DataRow row = dr.ResultSet.Tables[0].Rows[0];        

        double discountgroupA = CastAide.AsDouble(row["groupa"], 0);
        double discountgroupB = CastAide.AsDouble(row["groupb"], 0);
        double discountgroupC = CastAide.AsDouble(row["groupc"], 0);
        double discountgroupD = CastAide.AsDouble(row["groupd"], 0);
        double discountgroup1x2 = CastAide.AsDouble(row["discount1x2"], 0);
        double discountgroupother = CastAide.AsDouble(row["discountcs"], 0);
        double discountgroupnumber = CastAide.AsDouble(row["discountnumber"], 0);

        double groupAMax = CastAide.AsDouble(row["groupa_max"], 0);
        double groupBMax = CastAide.AsDouble(row["groupb_max"], 0);
        double groupCMax = CastAide.AsDouble(row["groupc_max"], 0);
        double groupDMax = CastAide.AsDouble(row["groupd_max"], 0);
        double group1x2Max = CastAide.AsDouble(row["discount1x2_max"], 0);
        double groupOtherMax = CastAide.AsDouble(row["discountcs_max"], 0);
        double groupnumberMax = CastAide.AsDouble(row["discountnumber_max"], 0);

        TemplateHandler.Assign(ref template, "groupa-comm", GenerateComboBox("groupa", discountgroupA, groupAMax));
        TemplateHandler.Assign(ref template, "groupb-comm", GenerateComboBox("groupb", discountgroupB, groupBMax));
        TemplateHandler.Assign(ref template, "groupc-comm", GenerateComboBox("groupc", discountgroupC, groupCMax));
        TemplateHandler.Assign(ref template, "groupd-comm", GenerateComboBox("groupd", discountgroupD, groupDMax));
        TemplateHandler.Assign(ref template, "group1x2-comm", GenerateComboBox("group1x2", discountgroup1x2, group1x2Max));
        TemplateHandler.Assign(ref template, "groupother-comm", GenerateComboBox("groupother", discountgroupother, groupOtherMax));
        if (Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission || !UserPermission.IsShowNumberGame)
        {
            TemplateHandler.Assign(ref template, "groupnumber-comm", string.Empty);
        }
        else
        {
            TemplateHandler.Assign(ref template, "groupnumber-comm", GenerateComboBox("groupnumber", discountgroupnumber, groupnumberMax));
        }
    }

    protected string GenerateComboBox(string cmbBoxId, double cmbBoxValue, double maximumValue)
    {
        string html = String.Empty;
        double step = 0.0005;

        html = String.Format("<select id='ddl{0}' name='ddl{0}' class='selectcomm' style='width:70px;'>", cmbBoxId);
        do
        {
            html += String.Format("<option value='{0}' {1}>{0}</option>", (float)maximumValue, ((float)maximumValue == (float)cmbBoxValue) ? "selected='true'" : String.Empty);
            maximumValue = maximumValue - step;
        }
        while (maximumValue > 0);

        if (maximumValue > -0.0005 && maximumValue <= 0)
        {
            if(cmbBoxValue == 0)
                html += String.Format("<option value='{0}' selected='true'>{0}</option>", 0);
            else
                html += String.Format("<option value='{0}'>{0}</option>", 0);
        }
        html += "</select>";
        
        return html;
    }
}
