using System;
using System.Collections;
using Sunplus.Agent.Model.Entities;



public partial class MemberInfo_RedirectPT : System.Web.UI.Page
{
    public static string UrlMenu = "";
    public static string UrlMainPage = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Hashtable htburl = new Hashtable();
        htburl = AppSession.RequestQueryString;
        string urlParam = "";        

        urlParam += "?custid=" + ((Request["custid"] != null) ? Request["custid"].ToString() : "");
        urlParam += "&isfi=" + ((Request["isfi"] != null) ? Request["isfi"].ToString() : "1");
        urlParam += "&ishr=" + Convert.ToInt16(!Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing); 
        urlParam += "&isCS=" + ((Request["isCS"] != null) ? Request["isCS"].ToString() : "1");
        urlParam += "&isBG=" + ((Request["isBG"] != null) ? Request["isBG"].ToString() : "1");
        urlParam += "&isLiveCS=" + ((Request["isLiveCS"] != null) ? Request["isLiveCS"].ToString() : "1");
        urlParam += "&isedit=" + ((Request["isedit"] != null) ? Request["isedit"].ToString() : "");
        urlParam += "&insert=" + ((Request["insert"] != null) ? Request["insert"].ToString() : "");
        urlParam += "&username=" + ((Request["username"] != null) ? Request["username"].ToString() : "");
        UrlMenu = "TabPT.aspx" + urlParam;

        if (UserSession.Current.IsSuper)
        {
            UrlMainPage = "../_MemberInfo/PositionTaking/Super/EditMinPTMaster.aspx?custid=" + Request["custid"] + "&user=" + ((Request["username"] != null) ? Request["username"].ToString() : "");
            return;
        }
        else if (UserSession.Current.IsMaster)
        {
            UrlMainPage = "../_MemberInfo/PositionTaking/Master/EditMinPTAgent.aspx?custid=" + Request["custid"] + "&user=" + ((Request["username"] != null) ? Request["username"].ToString() : "");
            return;
        }
        else
        {
            if (Request["ishr"] != null && Request["ishr"].ToString() == "0")
            {
                string urlHR = "?custid=" + ((Request["custid"] != null) ? Request["custid"].ToString() : "");

                if (UserSession.Current.IsSuper)
                    UrlMainPage = "../_MemberInfo/RaceBook/Super/AddNewMaster.aspx" + urlHR;
                else if (UserSession.Current.IsMaster)
                    UrlMainPage = "../_MemberInfo/RaceBook/Master/AddNewAgent.aspx" + urlHR;
                else
                    UrlMainPage = "../_MemberInfo/RaceBook/Agent/AddNewMember.aspx" + urlHR;
                return;
            }
            if (Request["isCS"] != null && Request["isCS"].ToString() == "0")
            {
                string urlCS = "?cscustid=" + ((Request["custid"] != null) ? Request["custid"].ToString() : "");
                urlCS += "&insert=" + ((Request["insert"] != null) ? Request["insert"].ToString() : "");
                UrlMainPage = "PositionTakingCS.aspx" + urlCS;
                return;
            }
            if (Request["isLiveCS"] != null && Request["isLiveCS"].ToString() == "0")
            {
                string urlLiveCS = "?custids=" + ((Request["custid"] != null) ? Request["custid"].ToString() : "");
                urlLiveCS += "&roleid=" + Convert.ToString(Sunplus.Agent.Model.Entities.UserSession.Current.RoleId - 1);
                urlLiveCS += "&isMult=0";
                urlLiveCS += "&disable=0";
                urlLiveCS += "&username=" + Request["username"].ToString();

                if (UserSession.Current.IsSuper)
                    UrlMainPage = "../_MemberInfo/LiveCasino/Super/EditMaster.aspx" + urlLiveCS;
                else if (UserSession.Current.IsMaster)
                    UrlMainPage = "../_MemberInfo/LiveCasino/Master/EditAgent.aspx" + urlLiveCS;
                else
                    UrlMainPage = "../_MemberInfo/LiveCasino/Agent/EditMember.aspx" + urlLiveCS;

                return;
            }
        }
    }
}
