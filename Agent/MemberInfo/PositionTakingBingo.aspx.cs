using System;
using System.Web.UI;
using BLL;
using DNAide.Core;
using DNAide.Web;
using Microsoft.Practices.Unity;
using Sunplus.Bingo.Core;
using Sunplus.Bingo.Entities;
using Sunplus.Bingo.Services;

public partial class PositionTakingBingo : Page
{
    bool IsMultiple = false;

    protected override void InitializeCulture()
    {
        System.Globalization.CultureInfo def = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Sunplus.Agent.Model.Entities.UserSession.Current.Language);
        System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(Sunplus.Agent.Model.Entities.UserSession.Current.Language);
        System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat = def.DateTimeFormat;
        System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat = def.NumberFormat;

        base.InitializeCulture();
    }

    protected override void OnPreLoad(EventArgs e)
    {
        if (!Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowBingo)
        {
            Server.Transfer("~/AccessDenied.aspx");
        }

        base.OnPreLoad(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // Hot fix for Opera browser (IIS does sniffing user agent)
        Response.ContentType = "text/html";

        IsMultiple = (Request["isMult"] == "1");
        string errmsg = string.Empty;

        int custId = CastAide.AsInt32(Request.QueryString["custid"]);
        if (custId != 0)
        {            
            try
            {
                #region Get Customer Information

                ICustomerService customerService = BingoContainer.Current.Resolve<ICustomerService>();
                IResult result = new Sunplus.Bingo.Entities.Result();
                ICustomer _cust = customerService.GetCustomerSetting(custId, Sunplus.Agent.Model.Entities.UserSession.Current.RoleId, ref result);

                if (_cust.RoleId == 0) // Have no data in Bingo DB.
                {
                    ctrHTML.Text = new Fanex.Age.UI.Components.ErrorPage_Page(Resources.Lang.noInfo);
                    return;
                }

                #endregion Get Customer Information

                if (IsMultiple)
                {
                    _cust.IsEnabledBingo = true; // Default is checked for multiple edit
                }

                string tempBingo = BingoManager.GenBingoPTHTML(_cust, BingoDef.BingoGroup.Group1);

                TemplateHandler.RemoveComment(ref tempBingo);

                if (Request["insert"] == "true" || _cust.SyncBingo)
                {
                    if (Request["issucc"] == "1")
                    {
                        tempBingo += String.Format(" <script>showErrMsg('{0}', 1);</script> ", Resources.Lang.updatesuccessfull);
                    }
                }

                // From AccountInfo tab
                if (!String.IsNullOrEmpty(Request.QueryString["nobackground"]))
                {
                    tempBingo = tempBingo.Replace("header_popup", "header_main");
                }

                ctrHTML.Text = tempBingo;
            }
            catch
            {
                ctrHTML.Text = new Fanex.Age.UI.Components.ErrorPage_Page(Resources.Lang.noInfo);
            }
        }
        else
        {
            ctrHTML.Text = new Fanex.Age.UI.Components.ErrorPage_Page(Resources.Lang.noInfo);
        }

        AddJsVariable("bettingLimitMsg", Resources.Lang.bettingLimitMsg);
        AddJsVariable("resetMaxTransferMsg", Resources.Lang.resetMaxTransferMsg);
    }

    protected override void Render(HtmlTextWriter writer)
    {
        base.Render(writer);
        WriteJsVariables(writer);
    }

    private System.Text.StringBuilder _jsVariables = new System.Text.StringBuilder(5000);

    public void AddJsVariable(string strName, string value)
    {
        _jsVariables.Append(strName + "='" + value.ToString().Replace("'", "\\'") + "';\r\n");
    }

    private void WriteJsVariables(HtmlTextWriter writer)
    {
        if (_jsVariables.Length == 0) return;
        // write javascript variables
        writer.WriteLine("<script type=\"text/javascript\">\r\n");
        writer.WriteLine(_jsVariables.ToString());
        writer.WriteLine("</script>\r\n");
    }
}