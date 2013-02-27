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
using DNAide.Core;

public partial class MemberInfo_downlineList : TplPage
{
    private string HtmlContent = String.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.CssGroup = "main";

        string temp_main = "";
        string act_list = "";
        string custid = "", masterid = "";
        act_list = (Request["act"] == null) ? Sunplus.Agent.Model.Entities.UserSession.Current.SiteName : Request["act"];
        custid = (Request["custid"] == null) ? Sunplus.Agent.Model.Entities.UserSession.Current.CustId.ToString() : Request["custid"];
        masterid = (Request["masterid"] == null) ? "0" : Request["masterid"];
        string username = (Request["username"] == null || Request["username"].ToString().ToLower() == "all" || Request["username"].ToString() == Resources.Lang.enterusername) ? "" : Request["username"].ToString().Trim();
        int pageindex = CastAide.AsInt32(Request["page"], 1);
        int status = CastAide.AsInt32(Request["status"], 0);
        string search = (Request["search"] == null) ? string.Empty : Request["search"].ToString();
        int totalpages = (Request["total"] == null) ? 0 : CastAide.AsInt32(Request["total"]);
        Hashtable hash = new Hashtable();
        hash.Add("act", act_list);
        hash.Add("custid", custid);
        hash.Add("masterid", masterid);
        hash.Add("ParentForm", "DownlineList.aspx");
        AppSession.RequestQueryString = hash;

        if (AppSettings.IsAgentSite)
        {
            this.HtmlContent = MemberInfo_Downline.MemberList(Sunplus.Agent.Model.Entities.UserSession.Current.CustId, masterid, 1, username, pageindex, status, act_list, search, totalpages);
            AddJsRawVariable("droleid", ((int)(Sunplus.Agent.Model.BL.Definitions.UserTypes.Member)).ToString());
        }
        else if (AppSettings.IsMasterSite) // for master site
        {
            if (act_list == "MemberList")
            {
                temp_main = MemberInfo_Downline.MemberList(CastAide.AsInt32(custid), masterid, 1, username, pageindex, status, act_list, search, totalpages);
                AddJsRawVariable("droleid", ((int)(Sunplus.Agent.Model.BL.Definitions.UserTypes.Member)).ToString());
            }
            else
            {
                temp_main = MemberInfo_Downline.DirectDownlineList(Sunplus.Agent.Model.Entities.UserSession.Current.CustId, masterid, 2, username, pageindex, status, act_list, search, totalpages);
                AddJsRawVariable("droleid", ((int)(Sunplus.Agent.Model.BL.Definitions.UserTypes.Agent)).ToString());
            }
            this.HtmlContent = temp_main;
        }
        else if (AppSettings.IsSuperSite) // for super site
        {
            if (act_list == "AgentList")
            {
                temp_main = MemberInfo_Downline.MasterAgentList(custid, masterid, false, act_list, username, pageindex, status, search, totalpages);
                TemplateHandler.RemoveBlock(ref temp_main, "limit-edit");

                AddJsRawVariable("droleid", ((int)(Sunplus.Agent.Model.BL.Definitions.UserTypes.Agent)).ToString());

                int numcol = 15;

                if (Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowCasino)
                {
                    numcol++;
                }

                numcol = 16;

                TemplateHandler.Assign(ref temp_main, "horseracing", Resources.Lang.hr);

                TemplateHandler.Assign(ref temp_main, "numcol", numcol.ToString());
            }
            else if (act_list == "MemberList")
            {
                temp_main = MemberInfo_Downline.MemberList(CastAide.AsInt32(custid), masterid, 1, username, pageindex, status, act_list, search, totalpages);

                AddJsRawVariable("droleid", ((int)(Sunplus.Agent.Model.BL.Definitions.UserTypes.Member)).ToString());

            }
            else
            {
                temp_main = MemberInfo_Downline.DirectDownlineList(Sunplus.Agent.Model.Entities.UserSession.Current.CustId, masterid, 3, username, pageindex, status, act_list, search, totalpages);
                TemplateHandler.Assign(ref temp_main, "level", "Super");

                AddJsRawVariable("droleid", ((int)(Sunplus.Agent.Model.BL.Definitions.UserTypes.Master)).ToString());

            }
            this.HtmlContent = temp_main;
        }

        if (!Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowCasino)
        {
            TemplateHandler.RemoveBlock(ref this.HtmlContent, "casino");
            TemplateHandler.RemoveBlock(ref this.HtmlContent, "no-casino");
            TemplateHandler.RemoveBlock(ref this.HtmlContent, "edit-CS");
        }

        if (!Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowBingo)
        {
            TemplateHandler.RemoveBlock(ref this.HtmlContent, "bingo");
            TemplateHandler.RemoveBlock(ref this.HtmlContent, "no-bingo");
            TemplateHandler.RemoveBlock(ref this.HtmlContent, "edit-BG");
        }

        if (AppSettings.IsAgentSite || act_list == "MemberList")
        {
            AddJsVariable("lblConfirmClearCredit", Resources.Lang.lblConfirmClearCreditMember);
            AddJsVariable("confirmDisableNumberGameDownline", Resources.Lang.confirmDisableNumberGame);
            AddJsVariable("confirmSuspendMem", Resources.Lang.confirmUnSuspendMem);
            AddJsVariable("confirmSuspendDownline", Resources.Lang.confirmSuspendMem);

            AddJsVariable("confirmCloseMem", Resources.Lang.confirmCloseMem);
            AddJsVariable("confirmOpenMem", Resources.Lang.confirmCloseMem);
            
        }
        else
        {
            AddJsVariable("lblConfirmClearCredit", Resources.Lang.lblConfirmClearCredit);
            AddJsVariable("confirmDisableNumberGameDownline", Resources.Lang.confirmDisableNumberGameDownline);
            AddJsVariable("confirmSuspendMem", Resources.Lang.confirmUnSuspended );
            AddJsVariable("confirmSuspendDownline", Resources.Lang.confirmSuspendDownline);
            AddJsVariable("lblConfirmAllowDownline", Resources.Lang.lblConfirmAllowDownline);

            AddJsVariable("confirmCloseMem", Resources.Lang.confirmCloseDownline);
            AddJsVariable("confirmOpenMem", Resources.Lang.confirmCloseMem);
        }
        AddJsVariable("lblClearAllCredit", Resources.Lang.clearallcredit);
        AddJsVariable("lblClearAllDownlines", Resources.Lang.downlines);



        AddJsVariable("masterid", string.IsNullOrEmpty(Request["masterid"]) ? "0" : Request["masterid"]);
        
        

        //Load CSS for freeze headline
        string styleSheet = string.Empty;
        string cellspacing = "1";

        if (Request.Browser.Browser == "IE")
        {
            int versionIE = 6;
            string version = Request.Browser.Version.Substring(0, 1);
            if (!String.IsNullOrEmpty(version))
            {
                versionIE = DNAide.Core.CastAide.AsInt32(version);
            }
            if (versionIE < 7)
            {
                this.IsXHtml = false;
            }
        }

        
        TemplateHandler.Assign(ref HtmlContent, "StyleSheet", styleSheet);
        TemplateHandler.Assign(ref HtmlContent, "cellspacing", cellspacing);

        TemplateHandler.Assign(ref HtmlContent, "open", Resources.Lang.open);
        TemplateHandler.Assign(ref HtmlContent, "closed", Resources.Lang.closed);
        TemplateHandler.Assign(ref HtmlContent, "suspended", Resources.Lang.suspended);

        TemplateHandler.Assign(ref HtmlContent, "outright", Resources.Lang.lblenableOutRight);
        TemplateHandler.Assign(ref HtmlContent, "allowAutoPT", Resources.Lang.allowautopt);
        TemplateHandler.Assign(ref HtmlContent, "allowNumberGame", Resources.Lang.lblEnableNumber);

        TemplateHandler.Assign(ref HtmlContent, "close", Resources.Lang.close);
        TemplateHandler.Assign(ref HtmlContent, "suspend", Resources.Lang.suspend);
        TemplateHandler.Assign(ref HtmlContent, "status", Resources.Lang.status);
        TemplateHandler.Assign(ref HtmlContent, "minposition", Resources.Lang.sbminpt);
        TemplateHandler.Assign(ref HtmlContent, "transfer", Resources.Lang.transfer);
        TemplateHandler.Assign(ref HtmlContent, "password", Resources.Lang.password);

        TemplateHandler.Assign(ref HtmlContent, "fi_mult", Resources.Lang.editmultfinance);
        TemplateHandler.Assign(ref HtmlContent, "spb_mult", Resources.Lang.editmultsportbook);
        TemplateHandler.Assign(ref HtmlContent, "hr_mult", Resources.Lang.editmulthr);
        TemplateHandler.Assign(ref HtmlContent, "cs_multEdit", Resources.Lang.editmultcs);
        TemplateHandler.Assign(ref HtmlContent, "bg_multEdit", Resources.Lang.editmultbingo);
        TemplateHandler.Assign(ref HtmlContent, "p2p_multEdit", Resources.Lang.editmultp2p);

        TemplateHandler.Assign(ref HtmlContent, "fi_minpt", Resources.Lang.fiminpt);
        TemplateHandler.Assign(ref HtmlContent, "hr_minpt", Resources.Lang.hrminpt); 

        TemplateHandler.Assign(ref HtmlContent, "isdisablecs", Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowCasino ? "0" : "1");
        TemplateHandler.Assign(ref HtmlContent, "isdisablefi", Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowFinance ? "0" : "1");
        TemplateHandler.Assign(ref HtmlContent, "isdisablehr", Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing ? "0" : "1");
        TemplateHandler.Assign(ref HtmlContent, "isdisablebg", Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowBingo ? "0" : "1");
        //TemplateHandler.Assign(ref HtmlContent, "isdisablep2p", Sunplus.Agent.Model.BL.Definitions.AppSettings.IsEnableP2P ? "0" : "1");

        TemplateHandler.Assign(ref HtmlContent, "roleid_val", Sunplus.Agent.Model.Entities.UserSession.Current.RoleId.ToString());
        //Check Status NumberGame on WebConfig
        TemplateHandler.Assign(ref HtmlContent, "isDisableNumberGameStatus", Sunplus.Agent.Model.BL.Definitions.AppSettings.DisableNumberGameStatus.ToString().ToLower());
        
        //nqminh changed
        bool isSuspended, isDisableOutright;

        int customerToView = CastAide.AsInt32(custid, -1);
        DataRow row = AccountInfo.Age_CustomerSelStatus(customerToView).ResultSet.Tables[0].Rows[0];
        isSuspended = CastAide.AsBoolean(row["Suspend"], false);
        isDisableOutright = CastAide.AsBoolean(row["DisableOutright"], false);
        TemplateHandler.Assign(ref HtmlContent, "callerIsSuspended", isSuspended.ToString().ToLower());
        TemplateHandler.Assign(ref HtmlContent, "callerIsAllowOutright", isDisableOutright.ToString().ToLower());

        

        //print function
        string printerHtml = TemplateHandler.LoadTemplate("Print.html");
        string printer = TemplateHandler.CopyBlock(printerHtml, "printer");
        string printcss = TemplateHandler.CopyBlock(printerHtml, "printcss");
        TemplateHandler.Assign(ref printer, "lblprint", Resources.Lang.print);
        TemplateHandler.Assign(ref HtmlContent, "printer", printer);
        TemplateHandler.Assign(ref HtmlContent, "printcss", printcss);
        //--------------------------

        //HtmlContent = Sunplus.Agent.Model.Common.AgentIPFormat.FormatIPs(HtmlContent);
        
        HtmlContent += "<div class=\"shadow\" id=\"shadow\" /><div class=\"output\" id=\"output\" />";

        this.TplSetContent(this.HtmlContent);
    }
}
