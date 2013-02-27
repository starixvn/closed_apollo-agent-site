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
using Sunplus.Agent.Model.Entities;
using Sunplus.Agent.Model.Entities.Permission;

public partial class MemberInfo_TabPT : TplPage
{
    protected bool isSettingMinPT = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        string[] ArrText = null;
        string[] ArrURL = null;
        string[] ArrIsDisable = null;
        string[] ArrHd = null;
        string urlRacing = string.Empty, urlSPMinPT = string.Empty, urlRacingMinPT = string.Empty, urlLiveCasino = string.Empty;

        if (Sunplus.Agent.Model.Entities.Permission.UserPermission.IsNeutral) // hide Bingo
            ArrText = new string[] { Resources.Lang.sbminpt, Resources.Lang.hr, Resources.Lang.hrminpt, Resources.Lang.casino, Resources.Lang.P2P, Resources.Lang.LiveCasino, Resources.Lang.VirtualSports };
        else
            ArrText = new string[] { Resources.Lang.sbminpt, Resources.Lang.hr, Resources.Lang.hrminpt, Resources.Lang.casino, Resources.Bingo.bingo, Resources.Lang.P2P, Resources.Lang.LiveCasino, Resources.Lang.VirtualSports };

        if (UserSession.Current.IsSuper)
        {
            urlRacing = "../_MemberInfo/RaceBook/Super/AddNewMaster.aspx";
            urlRacingMinPT = "../_MemberInfo/RaceBook/Super/EditMinPT.aspx";
            urlSPMinPT = "../_MemberInfo/PositionTaking/Super/EditMinPTMaster.aspx";
            urlLiveCasino = "../_MemberInfo/LiveCasino/Super/EditMaster.aspx";
        }
        else if (UserSession.Current.IsMaster)
        {
            urlRacing = "../_MemberInfo/RaceBook/Master/AddNewAgent.aspx";
            urlRacingMinPT = "../_MemberInfo/RaceBook/Master/EditMinPT.aspx";
            urlSPMinPT = "../_MemberInfo/PositionTaking/Master/EditMinPTAgent.aspx";
            urlLiveCasino = "../_MemberInfo/LiveCasino/Master/EditAgent.aspx";
        }
        else
        {
            urlRacing = "../_MemberInfo/RaceBook/Agent/AddNewMember.aspx";
            urlLiveCasino = "../_MemberInfo/LiveCasino/Agent/EditMember.aspx";
        }

        ArrURL = new string[] 
        {   urlSPMinPT + "?custid=" + Request["custid"] +"&user="+ Request["username"] ,            
            urlRacing + "?custid=" + Request["custid"], 
            urlRacingMinPT + "?custid=" + Request["custid"] + "&username=" + Request["username"], 
            "PositionTakingCS.aspx" + "?cscustid=" + Request["custid"] + "&insert=" + Request["insert"], 
            "PositionTakingBingo.aspx" + "?custid=" + Request["custid"] + "&insert=" + Request["insert"], 
            "../_MemberInfo/P2P/P2P_Page.aspx?custids=" + Request["custid"] + "&roleid=" + Convert.ToString(UserSession.Current.RoleId - 1) + "&username=",
            urlLiveCasino + "?custids=" + Request["custid"] + "&roleid=" + Convert.ToString(UserSession.Current.RoleId - 1) + "&isMult=0" + "&disable=0" + "&username=" + Request["username"],
            "../_MemberInfo/VirtualSports/EditSingle.aspx?custid=" + Request["custid"] + "&roleid=" + Convert.ToString(UserSession.Current.RoleId - 1) + "&username=" + Request["username"],
        };
        ArrHd = new string[] { "SPMinPT", "Hr", "RacingMinPT", "Cs", "Bg", "P2P", "LiveCS", "VirtualSports" };

        isSettingMinPT = (UserSession.Current.IsSuper || UserSession.Current.IsMaster);
        ArrIsDisable = new string[] 
        {   
            isSettingMinPT ? "0" : "1",
            Request["ishr"].ToString(), 
            isSettingMinPT ? "0" : "1",
            Request["isCS"].ToString(), 
            UserPermission.IsShowBingo ? "0" : "1", 
            UserPermission.IsShowP2P ? "0" : "1",
            UserPermission.IsShowLiveCasino ? "0" : "1",
            UserPermission.IsShowVirtualSports ? "0" : "1"
        };
        string submenu = StatusSetting.GenSubMenu(ArrURL, ArrText, ArrIsDisable, ArrHd);
        this.TplSetContent(submenu);
    }
}
