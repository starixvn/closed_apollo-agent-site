<%@ WebHandler Language="C#" Class="Flash" %>

using System;
using System.Web;
using Sunplus.Agent.Model.Entities;
using Sunplus.Agent.Model.BL;

public class Flash : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/x-shockwave-flash";
        context.Response.Cache.SetMaxAge(TimeSpan.FromDays(7));
        context.Response.CacheControl = "public";

        string parameter = context.Request["params"] + string.Empty;
        parameter = parameter.Replace(' ', '+');
        parameter = Sunplus.Agent.Utilities.Encryption.CfsDeCrypt(parameter);

        string[] ps = parameter.Split('&');
        string flashName, roleID, Lang;
        if (ps.Length == 3)
        {
            flashName = ps[0] + "/" + ps[0];
            roleID = ps[1];
            Lang = ps[2];
        }
        else
            return;

        string url = "~/_HowToUse/Resources/Flash/" + flashName + roleID + "_" + Lang + ".swf";
        context.Server.Transfer(url);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}