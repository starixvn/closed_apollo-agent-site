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
public partial class MemberInfo_PositionTaking : TplPage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.CssGroup = "main";

        string main = TemplateHandler.LoadTemplate("MemberInfo/PositionTaking_new.html");
        string custid = string.Empty;
        string type = string.Empty;
        type = (Request.QueryString["type"] == null) ? "" : Request.QueryString["type"].ToString();
        custid = (Request.QueryString["custid"] == null) ? Sunplus.Agent.Model.Entities.UserSession.Current.CustId.ToString() : Request.QueryString["custid"].ToString();

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

                    TemplateHandler.Assign(ref main, "creditbalance", Resources.Lang.creditbalance);
                    TemplateHandler.Assign(ref main, "url-credit", "CreditBalance.aspx?act=AgentList&custid=" + custid);
                }
                else
                {
                    TemplateHandler.Assign(ref main, "url-list", "../_MemberInfo/CustomerList/Super/MemberList.aspx?custid=" + custid);
                    TemplateHandler.Assign(ref main, "customerlist", Resources.Lang.customerlist);

                    TemplateHandler.Assign(ref main, "creditbalance", Resources.Lang.creditbalance);
                    TemplateHandler.Assign(ref main, "url-credit", "CreditBalance.aspx?act=MemberList&custid=" + custid);
                }
            }
            else if (AppSettings.IsMasterSite)
            {
                TemplateHandler.Assign(ref main, "url-list", "../_MemberInfo/CustomerList/Master/MemberList.aspx?custid=" + custid);
                TemplateHandler.Assign(ref main, "customerlist", Resources.Lang.customerlist);

                TemplateHandler.Assign(ref main, "creditbalance", Resources.Lang.creditbalance);
                TemplateHandler.Assign(ref main, "url-credit", "CreditBalance.aspx?act=MemberList&custid=" + custid);
            }
            
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
        TemplateHandler.Assign(ref main, "lblSoccer", Resources.Lang.soccer);
        TemplateHandler.Assign(ref main, "lblLive", Resources.Lang.lblLive);
        TemplateHandler.Assign(ref main, "lblBasketball", Resources.Lang.Basketball);
        TemplateHandler.Assign(ref main, "lblFootball", Resources.Lang.Football);
        TemplateHandler.Assign(ref main, "lblTennis", Resources.Lang.Tennis);
        TemplateHandler.Assign(ref main, "lblBaseball", Resources.Lang.Baseball);
        TemplateHandler.Assign(ref main, "lblGolf", Resources.Lang.Golf);
        TemplateHandler.Assign(ref main, "lblMotosports", Resources.Lang.MotorSports);
        TemplateHandler.Assign(ref main, "lblHorse", Resources.Lang.Horse);
        TemplateHandler.Assign(ref main, "lblOtherSports", Resources.Lang.OtherSports);
        TemplateHandler.Assign(ref main, "lblHdp", Resources.Lang.hdp);
        TemplateHandler.Assign(ref main, "lblOU", Resources.Lang.lblOU);
        TemplateHandler.Assign(ref main, "lbl1stHdp", Resources.Lang.lbl1stHdp);
        TemplateHandler.Assign(ref main, "lbl1stOU", Resources.Lang.lbl1stOU);
        TemplateHandler.Assign(ref main, "lblOE", Resources.Lang.lblOE);
        TemplateHandler.Assign(ref main, "lbl1x2", Resources.Lang.lbl1x2);
        TemplateHandler.Assign(ref main, "lblCS", Resources.Lang.lblCS);
        TemplateHandler.Assign(ref main, "lblTG", Resources.Lang.lblTG);
        TemplateHandler.Assign(ref main, "lblML", Resources.Lang.lblML2);   
        TemplateHandler.Assign(ref main, "lblOutRight", Resources.Lang.lblOutRight);

        
        TemplateHandler.Assign(ref main, "lblFOWin", Resources.Lang.winuktote);
        TemplateHandler.Assign(ref main, "lblFOPlace", Resources.Lang.placeuktote);

        if (UserPermission.IsShowMixedSport)
        {
            TemplateHandler.Assign(ref main, "lblMP", Resources.Lang.lblMP);
            TemplateHandler.Assign(ref main, "lblParlayMixedSports", Resources.Lang.mixsports);
        }
        else
        {
            TemplateHandler.RemoveBlock(ref main, "MixSportsParlay");
        }

        if (!Sunplus.Agent.Model.Entities.UserSession.Current.IsAllowHRCurrency)
        {
            TemplateHandler.RemoveBlock(ref main, "HorseT");
            TemplateHandler.Assign(ref main, "hrcols", 2);
        }
        else
        {
            TemplateHandler.Assign(ref main, "lblWin", Resources.Lang.winsatote);
            TemplateHandler.Assign(ref main, "lblPlace", Resources.Lang.placesatote);
            TemplateHandler.Assign(ref main, "hrcols", 4);
        }        

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

        content = MemberInfo_Downline.GetPositionTaking(row, type, custid, ref rowcount);

        TemplateHandler.Assign(ref main, "rowcount", rowcount.ToString());

        TemplateHandler.ReplaceBlock(ref main, "row", content);

        //Load CSS for freeze headline
        string styleSheet = string.Empty;
        string cellspacing = "1";     

        TemplateHandler.Assign(ref main, "StyleSheet", styleSheet);
        TemplateHandler.Assign(ref main, "cellspacing", cellspacing);

        if (!Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing)
        {
            TemplateHandler.RemoveBlock(ref main, "Horse");
        }

        this.TplSetContent(main);

    }
}
