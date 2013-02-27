<%@ WebHandler Language="C#" Class="RenameXTEST" %>

using System;
using System.Web;
using System.IO;
using System.Collections;

public class RenameXTEST : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {

        context.Response.ContentType = "text/plain";
        
        // Read all xtest from a file xtest.txt and put into a collection
        string[] sps = File.ReadAllLines(HttpContext.Current.Server.MapPath("~/xtest.txt"));

        string text = File.ReadAllText(HttpContext.Current.Server.MapPath("~/SpNames.xml"));

        foreach (string sp in sps)
        {            
            string key = sp.Trim();
            if (string.IsNullOrEmpty(key)) continue;

            if (text.Contains("SpName=\"" + key + "\""))
            {
                text = text.Replace("SpName=\"" + key + "\"", "SpName=\"" + key + "_xtest\"");
            }
        }

        context.Response.Write(text);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}