<%@ WebHandler Language="C#" Class="GetDownlineList" %>

using System;
using System.Web;
using System.Collections.Specialized;
using System.Data;
using DNAide.Core;
using System.Web.SessionState;
using DNAide.Web;
using Fanex.Data.DNAide;

public class GetDownlineList : IHttpHandler, IRequiresSessionState
{        
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        Common.UpdateCulture();
        string result = String.Empty;

        const int PAGESIZE = 500;
        int custid = CastAide.AsInt32(context.Request.QueryString["custid"], -1);        
        int totalPages = CastAide.AsInt32(context.Request.QueryString["totalpages"], 0);
        int pageIndex = CastAide.AsInt32(context.Request.QueryString["pageidx"], 1);        

        DbResult dbResult = AccountInfo.GetCustList(custid, PAGESIZE, pageIndex, ref totalPages);        
        DataTable tblDownline = dbResult.ResultSet.Tables[0];
        
        result = TemplateHandler.LoadTemplate("UserInfo/ShortDownlineList.html");
        string maxHeight = "height";
        int versionIE = getVersionIE();
        if (versionIE < 0 || versionIE > 6) maxHeight = "max-height";
        TemplateHandler.Assign(ref result, "maxheight", maxHeight);
        TemplateHandler.Assign(ref result, "custid", custid);
        TemplateHandler.Assign(ref result, "curpage", pageIndex);
        TemplateHandler.Assign(ref result, "totalpages", totalPages);
        result = GenPageButton(result, pageIndex, totalPages);
        
        string rowTpl = TemplateHandler.CopyBlock(result, "userline");
        
        string rowCollection = "";
        int userCount = tblDownline.Rows.Count;
        if (userCount == 0)
        {
            result = String.Format("<center>{0}</center>", Resources.Lang.noInfo);
        }
        else
        {
            //If exist member has length too long, then change the UI
            bool isExistLongText = IsExistLongText(tblDownline);
            string widthLeft = "50%", textAlign = "left";
            if (isExistLongText) {
                widthLeft = "52%";
                textAlign = "";                
            }
            TemplateHandler.Assign(ref rowTpl, "widthLeft", widthLeft);
            TemplateHandler.Assign(ref rowTpl, "textAlign", textAlign);
            
            for (int i = 0; i < userCount; i = i + 2)
            {
                string rowHtml = rowTpl;
                DataRow row = tblDownline.Rows[i];
                TemplateHandler.Assign(ref rowHtml, "usercol1", CastAide.AsString(row["username"]));
                if (i + 1 < userCount)
                {
                    row = tblDownline.Rows[i + 1];
                    TemplateHandler.Assign(ref rowHtml, "usercol2", CastAide.AsString(row["username"]));
                }
                else
                {
                    TemplateHandler.Assign(ref rowHtml, "usercol2", String.Empty);
                }
                rowCollection += rowHtml;
            }
            TemplateHandler.ReplaceBlock(ref result, "userline", rowCollection);
        }       

        context.Response.Write(String.Format("<script type='text/javascript'>parent.loadCustDownlineList('{0}');</script>", result));
    }

    private static string GenPageButton(string result, int pageIndex, int totalPages)
    {
        if (totalPages > 1)
        {
            int pageIndexNext = pageIndex + 1;
            int pageIndexPrev = pageIndex - 1;

            if (pageIndexNext > totalPages)
                TemplateHandler.RemoveBlock(ref result, "PNext");
            else
                TemplateHandler.Assign(ref result, "pageidxNext", pageIndexNext);

            if (pageIndexPrev < 1)
                TemplateHandler.RemoveBlock(ref result, "PPrev");
            else
                TemplateHandler.Assign(ref result, "pageidxPrev", pageIndexPrev);
        }
        else
        {
            TemplateHandler.RemoveBlock(ref result, "Paging");
        }             
        return result;
    }

    public bool IsExistLongText(DataTable tblDownline)
    {
        foreach (DataRow row in tblDownline.Rows)
        {
            string userName = CastAide.AsString(row["username"]);
            if (userName.Length > 12)
                return true;
        }
        return false;
    }

    public int getVersionIE()
    {        
        if (HttpContext.Current.Request.Browser.Browser == "IE")
        {
            int versionIE = 6;     
            string version = HttpContext.Current.Request.Browser.Version.Substring(0, 1);
            if (!String.IsNullOrEmpty(version))
            {
                versionIE = DNAide.Core.CastAide.AsInt32(version);
            }
            return versionIE;
        }
        return -1; //not IE
    }   

    public bool IsReusable {
        get {
            return false;
        }
    }

}