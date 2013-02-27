<%@ WebHandler Language="C#" Class="UsrInfo_OnlineList" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Data;
using DNAide.Web;
using DNAide.Core;
using DNAide.Utils;
using IBCAgent;

public class UsrInfo_OnlineList : IHttpHandler, IRequiresSessionState{
    
    public void ProcessRequest (HttpContext context) {
        
        context.Response.Cache.SetNoStore();
        context.Response.ContentType = "text/html";
        Common.UpdateCulture();

        //Load template
        string template = TemplateHandler.LoadTemplate("OnlineList.html");

        //Load languages
        TemplateHandler.Assign(ref template, "lblno", Resources.Lang.no);
        TemplateHandler.Assign(ref template, "lblip", Resources.Lang.ip);
        TemplateHandler.Assign(ref template, "lbllastcheckin", Resources.Lang.lastcheckin);        
        
        if (!String.IsNullOrEmpty(context.Request.QueryString["custid"]))
        {
            string rowhtmlList = String.Empty;           
            
            int custId = CastAide.AsInt32(context.Request.QueryString["custid"], -1);
            if (custId > 0)            
            {
                string rowtpl = TemplateHandler.CheckOutBlock(ref template, "rowlastlogin");
                DataRowCollection rows = UserInfo.CustomerSelOnline(custId);                
                
                if (rows != null && rows.Count > 0)
                {       
                    for (int i = 0; i < rows.Count; ++i)
                    {
                        DataRow row = rows[i];
                        string rowhtml = (string)rowtpl.Clone();
                        TemplateHandler.Assign(ref rowhtml, "{no}", (i + 1) + "");
                        TemplateHandler.Assign(ref rowhtml, "{ip}", Sunplus.Agent.Model.Common.AgentIPFormat.FormatIP(row["IP"].ToString()).Replace("'","\\'"));
                        TemplateHandler.Assign(ref rowhtml, "{date}", FormatHandler.FormatDateTimeEn12(row["reload_time"]));                       
                        rowhtmlList += rowhtml;
                    }
                    TemplateHandler.CheckInBlock(ref template, "rowlastlogin", rowhtmlList);
                }
                else
                {
                    template = Resources.Lang.userisnotonline;
                }
            }
            else
            {
                template = Resources.Lang.internalerror;
            }            
        }        
                
        context.Response.Write(String.Format("<script type='text/javascript'>parent.loadCustOnlineList('{0}');</script>", template));                       
    }
   
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}