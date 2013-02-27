using System;
using System.Data;

using DNAide.Web;
using DNAide.Core;
using IBCAgent;
using Fanex.Data.DNAide;

using Sunplus.Agent.Model.Entities.Permission;

public partial class EditMemCommission : TplPage
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

    protected void Page_Load(object sender, EventArgs e)
    {

        this.CssGroup = "popup";

        //Template
        string template = TemplateHandler.LoadTemplate("UserInfo/MemberCommission.html");
        string errorTpl = TemplateHandler.LoadTemplate("ErrorMsg.html");
        TemplateHandler.Assign(ref template, "error-msg", errorTpl);

        //Languages
        TemplateHandler.Assign(ref template, "lbleditcomm", Resources.Lang.lbleditcomm + " " + CustName);
        TemplateHandler.Assign(ref template, "lbleditcustcomm", Resources.Lang.lbleditcustcomm);
        TemplateHandler.Assign(ref template, "lblCommissionForAgent", Resources.Lang.lblCommissionForAgent);
        TemplateHandler.Assign(ref template, "lblAsianHDC_OU_OE", Resources.Lang.lblAsianHDC_OU_OE);
        TemplateHandler.Assign(ref template, "lbl1x2_CS_TG", Resources.Lang.lblOtherTypes);
        TemplateHandler.Assign(ref template, "lblCommissionForMember", Resources.Lang.lblCommissionForMember);
        TemplateHandler.Assign(ref template, "lbl1x2", Resources.Lang.lbl1x2);
        if (UserPermission.IsShowNumberGame && !Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission)
        {
            TemplateHandler.Assign(ref template, "lblCommissionForAgent_number", Resources.Lang.lblCommissionForAgent);
            TemplateHandler.Assign(ref template, "lblCommissionForMember_number", Resources.Lang.lblCommissionForMember);
            TemplateHandler.Assign(ref template, "lblnumber", Resources.Lang.numbergame);
        }
        else
        {
            TemplateHandler.RemoveBlock(ref template, "none-number");
            TemplateHandler.Assign(ref template, "lblCommissionForAgent_number", string.Empty );
            TemplateHandler.Assign(ref template, "lblCommissionForMember_number", string.Empty);
            TemplateHandler.Assign(ref template, "lblnumber", string.Empty);
        }
        TemplateHandler.Assign(ref template, "update", Resources.Lang.update);
        TemplateHandler.Assign(ref template, "reset", Resources.Lang.reset);
        TemplateHandler.Assign(ref template, "loading", Resources.Lang.processing);

        //Handlers
        TemplateHandler.Assign(ref template, "updateinfohandler", "../Handlers/UpdateMemCommission.ashx?custid=" + this.CustId);        

        //Load commission
        this.LoadUserCommission(ref template);

        this.TplSetContent(template);
    }

    public void LoadUserCommission(ref string template)
    {
        int userId = Sunplus.Agent.Model.Entities.UserSession.Current.CustId;
        DbResult dr = UserInfo.SelectCommisionByCustomer(this.CustId, userId);
        DataRow row = dr.ResultSet.Tables[0].Rows[0];        

        double discountVal = CastAide.AsDouble(row["discount"], 0);
        double discountcsVal = CastAide.AsDouble(row["discountcs"], 0);
        double playerDiscountVal = CastAide.AsDouble(row["playerdiscount"], 0);
        double playerDiscountcsVal = CastAide.AsDouble(row["playerdiscountcs"], 0);
        double discount1x2 = CastAide.AsDouble(row["discount1x2"], 0);
        double discountnumber = CastAide.AsDouble(row["discountnumber"], 0);
        double playerdiscount1x2 = CastAide.AsDouble(row["playerdiscount1x2"], 0);
        double playerdiscountnumber = CastAide.AsDouble(row["playerdiscountnumber"], 0);

        TemplateHandler.Assign(ref template, "discount-val", discountVal.ToString() );
        TemplateHandler.Assign(ref template, "discountcs-val", discountcsVal.ToString());        
        TemplateHandler.Assign(ref template, "CommissionH_OU_OE", GenerateComboBox("CommissionH_OU_OE", playerDiscountVal, discountVal));
        TemplateHandler.Assign(ref template, "CommissionCS_TG", GenerateComboBox("CommissionCS_TG", playerDiscountcsVal, discountcsVal));
        TemplateHandler.Assign(ref template, "comm1x2-val", discount1x2);
        TemplateHandler.Assign(ref template, "Commission1x2", GenerateComboBox("Commission1x2", playerdiscount1x2, discount1x2));

        if (UserPermission.IsShowNumberGame && !Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledNumberGameCommission)
        {
            TemplateHandler.Assign(ref template, "commnumber-val", discountnumber);
            TemplateHandler.Assign(ref template, "Commissionnumber", GenerateComboBox("Commissionnumber", playerdiscountnumber, discountnumber));
        }
    }

    protected string GenerateComboBox(string cmbBoxId, double cmbBoxValue, double maximumValue)
    {
        string html = String.Empty;
        double step = 0.0005;

        html = String.Format("<select id='ddl{0}' name='ddl{0}' class='selectcomm' style='width:70px;'>", cmbBoxId);
        html += String.Format("<option value='{0}' selected='true'>{0}</option>", cmbBoxValue);
        do {            
            html += String.Format("<option value='{0}'>{0}</option>", (float)maximumValue);
            maximumValue = maximumValue - step;
        }
        while (maximumValue > 0);

        if (maximumValue > -0.0005 && maximumValue <= 0)
            html += String.Format("<option value='{0}'>{0}</option>", 0);        

        html += "</select>";
        
        return html;
    }
}
