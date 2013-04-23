<%@ WebHandler Language="C#" Class="SetLanguage" %>

using System;
using System.Web;
using System.Web.SessionState;

public class SetLanguage : IHttpHandler, IRequiresSessionState {
    
    public void ProcessRequest (HttpContext context) {
        context = IbcHandler.InitializeCulture(context);
        context.Response.ContentType = "text/html";
        try
        {
            string strLang = context.Request["lang"];
            switch (strLang)
            {
                case "en-US":
                case "ja-JP":
                case "zh-TW":
                case "zh-CN":
                case "th-TH":
                case "ko-KR":
                    break;
                default:
                    strLang = "en-US";
                    break;
            }
            Sunplus.Agent.Model.Entities.UserSession.Current.Language = strLang;
            context.Response.Write("1");
        }
        catch (Exception ex)
        {
            DNAide.Web.DNAideLog.Write(ex.Message);
            context.Response.Write("0");
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}