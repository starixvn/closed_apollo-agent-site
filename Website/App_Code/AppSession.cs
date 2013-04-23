using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;

[Obsolete]
public static class AppSession
{
    public static string[] ValidLangs = new string[] {"en-US","th-TH","zh-CN","zh-TW","ja-JP", "ko-KR"};

    public static string TplTheme
    {
        get
        {
            return (HttpContext.Current.Session["Tpl_Theme"]) as string;
        }
        set
        {
            HttpContext.Current.Session["Tpl_Theme"] = value;
        }
    }

    //public static bool IsCasinoInDBDisabled
    //{
    //    get { return (bool)(AccountInfo.Age_CustomerSelStatus(Sunplus.Agent.Model.Entities.UserSession.Current.CustId).ResultSet.Tables[0].Rows[0]["disablecasino"]); }
    //}
    

    public static Hashtable RequestQueryString
    {
        get { return HttpContext.Current.Session["RequestQueryString"] == null ? new Hashtable() : (Hashtable)HttpContext.Current.Session["RequestQueryString"]; }
        set { HttpContext.Current.Session["RequestQueryString"] = value; }
    }

    public static bool IsEnglish
    {
        get
        {
            return (string.Compare(Sunplus.Agent.Model.Entities.UserSession.Current.Language, "en-US") == 0);
        }
    }

    public static bool IsJapanese
    {
        get
        {
            return (string.Compare(Sunplus.Agent.Model.Entities.UserSession.Current.Language, "ja-JP") == 0);
        }
    }

    public static bool IsKorean
    {
        get
        {
            return (string.Compare(Sunplus.Agent.Model.Entities.UserSession.Current.Language, "ko-KR") == 0);
        }
    }

    public static bool IsThai
    {
        get { return (string.Compare(Sunplus.Agent.Model.Entities.UserSession.Current.Language, "th-TH") == 0); }
    }

    public static bool IsChineseTraditional
    {
        get { return (string.Compare(Sunplus.Agent.Model.Entities.UserSession.Current.Language, "zh-TW") == 0); }
    }

    public static bool IsChineseSimplified
    {
        get { return (string.Compare(Sunplus.Agent.Model.Entities.UserSession.Current.Language, "zh-CN") == 0); }
    }
}
