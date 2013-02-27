using System;
using System.Web.UI;
using BLL;
using CasinoLibrary.SBAgent;
using DAL;
using DNAide.Core;
using DNAide.Web;
using Sunplus.Agent.Model.Entities;

public partial class PositionTakingCS : Page
{
    public bool isInserting = false;
    private bool isMultiple = false;    
    private System.Text.StringBuilder javascriptVariables = new System.Text.StringBuilder(5000);

    protected override void InitializeCulture()
    {
        System.Globalization.CultureInfo defaultCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(UserSession.Current.Language);
        System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(UserSession.Current.Language);
        System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat = defaultCulture.DateTimeFormat;
        System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat = defaultCulture.NumberFormat;

        base.InitializeCulture();
    }

    protected override void OnLoad(EventArgs e)
    {
        this.isMultiple = this.Request["isMult"] == "1";

        // Hot fix for Opera browser (IIS does sniffing user agent)
        Response.ContentType = "text/html";

        string errMsg = string.Empty;

        int custId = (Request["insert"] == "true") ? CastAide.AsInt32(Request.QueryString["cscustid"], 0) : CastAide.AsInt32(Request.QueryString["custid"], 0);
        if (custId != 0)
        {
            Customer customer = new Customer();
            this.isInserting = this.Request["insert"] == "true";

            // Upline information
            CSCustomerDetail customerUplineDetail = new CSCustomerDetail();
            customerUplineDetail.IBCCustID = UserSession.Current.CustId;
            CasinoManager.GetCustSettingCS(ref customerUplineDetail);

            CSCustomerDetail customerDetail = CasinoManager.GetCustInfoIBC(custId);

            if (customerDetail.NoIBCData)
            {
                ctrHTML.Text = new Fanex.Age.UI.Components.ErrorPage_Page(Resources.Lang.noInfo);
                return;
            }

            if (customerDetail.CasinoSync)
            {
                CasinoManager.GetCustSettingCS(ref customerDetail); // Get more information from casino
            }

            // Get is game type from upline
            customerDetail.PTByGameType = customerDetail.PTByGameTypeUpline = customerUplineDetail.PTByGameType;

            if (this.isMultiple)
            {
                customerDetail.CasinoDisabled = UserSession.Current.IsCasinoDisabled;
                customerDetail.IsMultipleEdit = true;
            }

            string casinoHtml = CasinoManager.GenCasinoPTHTML(customerDetail, Globals.CasinoGroup.Group1);

            TemplateHandler.RemoveComment(ref casinoHtml);

            if (this.Request["insert"] == "true" || customerDetail.CasinoSync)
            {
                if (this.Request["issucc"] == "1")
                {
                    // From AccountInfo tab
                    if (!string.IsNullOrEmpty(Request.QueryString["nobackground"]))
                    {
                        casinoHtml += string.Format(" <script>showErrMsg('{0}', 1); top.menu.reloadMenuUsrInfo();</script> ", Resources.Lang.updatesuccessfull);
                    }
                    else
                    {
                        casinoHtml += string.Format(" <script>showErrMsg('{0}', 1);</script> ", Resources.Lang.updatesuccessfull);
                    }
                }
            }

            // From AccountInfo tab
            if (!string.IsNullOrEmpty(Request.QueryString["nobackground"]))
            {
                casinoHtml = casinoHtml.Replace("header_popup", "header_main");
            }

            ctrHTML.Text = casinoHtml;
        }
        else
        {
            ctrHTML.Text = new Fanex.Age.UI.Components.ErrorPage_Page(Resources.Lang.noInfo);
        }

        this.AddJsVariable("bettingLimitMsg", Resources.Lang.bettingLimitMsg);
        this.AddJsVariable("resetMaxTransferMsg", Resources.Lang.resetMaxTransferMsg);
    }

    protected void AddJsRawVariable(string name, string value)
    {
        this.javascriptVariables.Append(name + "=" + value.ToString() + ";\r\n");
    }

    protected void AddJsVariable(string name, string value)
    {
        this.javascriptVariables.Append(name + "='" + value.ToString().Replace("'", "\\'") + "';\r\n");
    }

    protected override void Render(HtmlTextWriter writer)
    {
        base.Render(writer);
        this.WriteJsVariables(writer);
    }

    private void WriteJsVariables(HtmlTextWriter writer)
    {
        if (this.javascriptVariables.Length == 0)
        {
            return;
        }

        // Write javascript variables
        writer.WriteLine("<script type=\"text/javascript\">\r\n");
        writer.WriteLine(this.javascriptVariables.ToString());
        writer.WriteLine("</script>\r\n");
    }
}