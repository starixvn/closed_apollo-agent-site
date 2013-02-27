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
using Sunplus.Agent.Model.Entities.Permission;
public partial class MemberInfo_PositionTakingRacing : TplPage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.CssGroup = "main";

        string main = TemplateHandler.LoadTemplate("MemberInfo/PositionTakingRacing_new.html");

        #region Remove Block
        int countGreycols = 4;
        if (!UserPermission.IsShowRacing_151_31)
            TemplateHandler.RemoveBlock(ref main, "SAHorse");
        if (!UserPermission.IsShowRacing_151_41)
            TemplateHandler.RemoveBlock(ref main, "UKHorse");
        if (!UserPermission.IsShowRacing_151_51)
            TemplateHandler.RemoveBlock(ref main, "AUHorse");
        if (!UserPermission.IsShowRacing_152_41)
        {
            TemplateHandler.RemoveBlock(ref main, "UKGreyhound");
            countGreycols -= 2;
        }
        if (!UserPermission.IsShowRacing_152_51)
        {
            TemplateHandler.RemoveBlock(ref main, "AUGreyhound");
            countGreycols -= 2;
        }
        TemplateHandler.Assign(ref main, "Greycols", countGreycols.ToString());
        if (countGreycols == 0) TemplateHandler.RemoveBlock(ref main, "Greyhound");
        if (!UserPermission.IsShowRacing_153_51)
            TemplateHandler.RemoveBlock(ref main, "AUHarness");
        if (!UserPermission.IsShowRacing)
            TemplateHandler.RemoveBlock(ref main, "Horse");
        #endregion

        string custid = string.Empty;
        string type = string.Empty;
        type = (Request.QueryString["type"] == null) ? "" : Request.QueryString["type"].ToString();
        custid = (Request.QueryString["custid"] == null) ? Sunplus.Agent.Model.Entities.UserSession.Current.CustId.ToString() : Request.QueryString["custid"].ToString();

        //Hide Financial follow web.config
        if (!Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowFinance)
        {
            TemplateHandler.RemoveBlock(ref main, "Financial");
        }
        //?type=agent&custid=68289&USER=ITibc
        Hashtable hash = new Hashtable();
        hash.Add("type", type);
        hash.Add("custid", custid);
        hash.Add("USER", Request["USER"]);
        hash.Add("ParentForm", "PositionTaking.aspx");
        AppSession.RequestQueryString = hash;

        //Apply language
        if (AppSettings.IsAgentSite)
        {
            TemplateHandler.RemoveBlock(ref main, "limit-header-position");
            TemplateHandler.Assign(ref main, "lblPositionTaking", Resources.Lang.lblPositionTaking);
        }
        else if ((AppSettings.IsSuperSite && type == "") || (AppSettings.IsMasterSite && type == ""))
        {
            TemplateHandler.RemoveBlock(ref main, "limit-header-position");
            TemplateHandler.Assign(ref main, "lblPositionTaking", Resources.Lang.lblPositionTaking);

        }
        else if ((AppSettings.IsSuperSite && type != "") || (AppSettings.IsMasterSite && type != ""))
        {
            TemplateHandler.RemoveBlock(ref main, "limit-header-position-normal");
            TemplateHandler.Assign(ref main, "lblPositionTaking", Resources.Lang.lblPositionTaking);
            
            if (AppSettings.IsSuperSite)
            {
                if (type == "master")
                {
                    TemplateHandler.Assign(ref main, "url-list", "../_MemberInfo/CustomerList/Super/AgentList.aspx?custid=" + custid);
                    TemplateHandler.Assign(ref main, "customerlist", Resources.Lang.lblAgentList);
                }
                else
                {
                    TemplateHandler.Assign(ref main, "url-list", "../_MemberInfo/CustomerList/Super/MemberList.aspx?custid=" + custid);
                    TemplateHandler.Assign(ref main, "customerlist", Resources.Lang.customerlist);
                }
            }
            else if (AppSettings.IsMasterSite)
            {
                TemplateHandler.Assign(ref main, "url-list", "../_MemberInfo/CustomerList/Master/MemberList.aspx?custid=" + custid);
                TemplateHandler.Assign(ref main, "customerlist", Resources.Lang.customerlist);
            }
            TemplateHandler.Assign(ref main, "creditbalance", Resources.Lang.creditbalance);
            TemplateHandler.Assign(ref main, "url-credit", "CreditBalance.aspx?act=MemberList&custid=" + custid);
            TemplateHandler.ReplaceBlock(ref main, "limit-position-taking", Resources.Lang.positiontaking);
            if (AppSettings.IsSuperSite)
            {
                TemplateHandler.Assign(ref main, "agentlist", Resources.Lang.lblMasterList);
                TemplateHandler.Assign(ref main, "url-agentlist", "../_MemberInfo/CustomerList/Super/MasterList.aspx");
            }
            else
            {
                TemplateHandler.Assign(ref main, "agentlist", Resources.Lang.lblAgentList);
                TemplateHandler.Assign(ref main, "url-agentlist", "../_MemberInfo/CustomerList/Master/AgentList.aspx");
            }
            

        }
        //If set enable number commission on webconfig
        if (!UserPermission.IsShowNumberGame)
        {
            TemplateHandler.RemoveBlock(ref main, "number-game");
        }
        else
        {
            TemplateHandler.Assign(ref main, "NumberGame", Resources.Lang.numbergame);
        }
        TemplateHandler.Assign(ref main, "Sportbook", Resources.Lang.sportBook);
        TemplateHandler.Assign(ref main, "Racing", Resources.Lang.Racebook);
        TemplateHandler.Assign(ref main, "lblUserName", Resources.Lang.username);

        TemplateHandler.Assign(ref main, "lblHorse", Resources.Lang.horseRacing);
        TemplateHandler.Assign(ref main, "lblGreyhound", Resources.Lang.greyHound);
        TemplateHandler.Assign(ref main, "lblHarness", Resources.Lang.harness);        

        TemplateHandler.Assign(ref main, "lblUKWin", Resources.Lang.winuktote);
        TemplateHandler.Assign(ref main, "lblUKPlace", Resources.Lang.placeuktote);
        TemplateHandler.Assign(ref main, "lblAUWin", Resources.Lang.winautote);
        TemplateHandler.Assign(ref main, "lblAUPlace", Resources.Lang.placeautote);

        // Horse
        int hrcols = 6;
        if (!UserPermission.IsShowRacing_151_31)
            TemplateHandler.RemoveBlock(ref main, "SAHorse");
        if (!UserPermission.IsShowRacing_151_41)
            TemplateHandler.RemoveBlock(ref main, "UKHorse");
        if (!UserPermission.IsShowRacing_151_51)
            TemplateHandler.RemoveBlock(ref main, "AUHorse");

        if (!Sunplus.Agent.Model.Entities.UserSession.Current.IsAllowHRCurrency || !UserPermission.IsShowRacing_151_31)
        {
            TemplateHandler.RemoveBlock(ref main, "SAHorse");
            hrcols -= 2;
        }
        else
        {
            TemplateHandler.Assign(ref main, "lblWin", Resources.Lang.winsatote);
            TemplateHandler.Assign(ref main, "lblPlace", Resources.Lang.placesatote);
        }

        if (!UserPermission.IsShowRacing_151_41)
            hrcols -= 2;
        if (!UserPermission.IsShowRacing_151_51)
            hrcols -= 2;
        TemplateHandler.Assign(ref main, "hrcols", hrcols.ToString());
        if (hrcols == 0) TemplateHandler.RemoveBlock(ref main, "Horse");

        // Apply header style
        TemplateHandler.Assign(ref main, "colHdr", "colHdr");
        TemplateHandler.Assign(ref main, "altHdr", "altHdr");

        TemplateHandler.Assign(ref main, "firstCol", "firstCol");

        TemplateHandler.Assign(ref main, "normalCol", "#424242"); //006666
        TemplateHandler.Assign(ref main, "altCol", "#2222dd");
        TemplateHandler.Assign(ref main, "BgCol", "#F9EEFB");


        if (AppSettings.IsAgentSite || (AppSettings.IsSuperSite && type == ""))
        {
            TemplateHandler.RemoveBlock(ref main, "AdditionalRow");
            TemplateHandler.Assign(ref main, "colNum", "2");
        }
        else
        {
            TemplateHandler.Assign(ref main, "colNum", "3");
        }


        string row = TemplateHandler.CopyBlock(main, "row");
        TemplateHandler.Assign(ref row, "overcolor", AppSettings.RowBgOver);
        string content = string.Empty;

        int rowcount = 0;

        content = MemberInfo_Downline.GetPositionTakingRacing(row, type, custid, ref rowcount);

        TemplateHandler.Assign(ref main, "rowcount", rowcount.ToString());

        TemplateHandler.ReplaceBlock(ref main, "row", content);

        //Load CSS for freeze headline
        string styleSheet = string.Empty;
        string cellspacing = "1";     

        TemplateHandler.Assign(ref main, "StyleSheet", styleSheet);
        TemplateHandler.Assign(ref main, "cellspacing", cellspacing);    

        this.TplSetContent(main);

    }
}
