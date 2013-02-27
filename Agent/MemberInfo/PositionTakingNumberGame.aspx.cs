using System;
using System.Collections;
using DNAide.Web;

public partial class MemberInfo_PositionTakingNumberGame : TplPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.CssGroup = "main";

        string main = TemplateHandler.LoadTemplate("MemberInfo/PositionTakingNumberGame_new.html");
        if (!Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowNumberGroup)
        {
            TemplateHandler.RemoveBlock(ref main, "NumberGroup");
        }

        string custid = string.Empty;
        string type = string.Empty;
        type = (Request.QueryString["type"] == null) ? "" : Request.QueryString["type"].ToString();
        custid = (Request.QueryString["custid"] == null) ? Sunplus.Agent.Model.Entities.UserSession.Current.CustId.ToString() : Request.QueryString["custid"].ToString();

        Hashtable hash = new Hashtable();
        hash.Add("type", type);
        hash.Add("custid", custid);
        hash.Add("USER", Request["USER"]);
        hash.Add("ParentForm", "PositionTakingNumberGame.aspx");
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

        TemplateHandler.Assign(ref main, "Sportbook", Resources.Lang.sportBook);
        TemplateHandler.Assign(ref main, "Racing", Resources.Lang.Racebook);
        TemplateHandler.Assign(ref main, "NumberGame", Resources.Lang.numbergame);
        TemplateHandler.Assign(ref main, "lblUserName", Resources.Lang.username);
        TemplateHandler.Assign(ref main, "lblDeadBall", Resources.Lang.lblDeadball);
        TemplateHandler.Assign(ref main, "lblLiveBall", Resources.Lang.lblLiveBall);        
        TemplateHandler.Assign(ref main, "lblWarrior", Resources.Lang.warrior);
        TemplateHandler.Assign(ref main, "lblOE", Resources.Lang.lblOddEven);
        TemplateHandler.Assign(ref main, "lblOU", Resources.Lang.lblOverUnder);
        TemplateHandler.Assign(ref main, "lblHL", Resources.Lang.highlow);
        TemplateHandler.Assign(ref main, "lblNextCombo", Resources.Lang.nextcombo);
        TemplateHandler.Assign(ref main, "lblNumberGroup", Resources.Lang.numberGroup);        
        TemplateHandler.Assign(ref main, "lbl1stLastOU", Resources.Lang.firstlastoverunder);
        TemplateHandler.Assign(ref main, "lbl1stLastOE", Resources.Lang.firstlastoddeven);
        
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
        content = MemberInfo_Downline.GetPositionTakingNumberGame(row, type, custid, ref rowcount);
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
