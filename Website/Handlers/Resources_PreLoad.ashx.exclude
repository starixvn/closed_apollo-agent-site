<%@ WebHandler Language="C#" Class="Resources_PreLoad" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.IO;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Author: Lee
/// Read all resources (javascripts, images and css) indicated in Web.res and render ajax script to pre-load it in browser cache
/// </summary>
public class Resources_PreLoad : IHttpHandler, IRequiresSessionState
{
    private List<string> staticResources;

    private string JsResourcesArray
    {
        get
        {
            if (HttpContext.Current.Application["JsResourcesArray"] != null)
                return (string)(HttpContext.Current.Application["JsResourcesArray"]);
            else
                return null;
        }
        set
        {
            HttpContext.Current.Application["JsResourcesArray"] = value;
        }
    }

    private void LoadStaticResources()
    {
        if (JsResourcesArray == null)
        {
            staticResources = new List<string>();
            // Load list of static resources
            string[] paths = File.ReadAllLines(HttpContext.Current.Server.MapPath("../Web.res"));
            int c = paths.Length;
            for (int i = 0; i < c; i++)
            {
                string[] pd = paths[i].Split(new char[] { '?' });
                string p = pd[0];
                string d = pd[1];
                
                GetUrlFiles(p, d.Split(new char[]{','}), true);
            }

            c = staticResources.Count;
            string basePath = HttpContext.Current.Server.MapPath("../");
            StringBuilder jsArray = new StringBuilder();
            jsArray.Append("[");
            if (c >= 1)
            {
                staticResources[0] = RevertPath(basePath, staticResources[0]);
                jsArray.Append("'" + staticResources[0] + "'");
            }
            for (int i = 1; i < c; i++)
            {
                staticResources[i] = RevertPath(basePath, staticResources[i]);
                if(staticResources[i].EndsWith(".js")) staticResources[i] = staticResources[i] + "?v=" + AppSettings.JsVersion;
                jsArray.Append(",'" + staticResources[i] + "'");
            }
            jsArray.Append("]");

            JsResourcesArray = jsArray.ToString();        
        }
    }

    private void GetUrlFiles(string path, string[] exts, bool searchSubDir)
    {
        HttpServerUtility server = HttpContext.Current.Server;
        int c = exts.Length;
        string[] files;
        for(int i=0; i<c; i++)
        {
            files = Directory.GetFiles(server.MapPath("../" + path), exts[i], SearchOption.AllDirectories);
            staticResources.AddRange(files);
        }
    }

    private string RevertPath(string basePath, string path)
    {
        int i = path.IndexOf(basePath);
        if (-1 == i) return null;

        string vPath = path.Substring(i + basePath.Length);
        vPath = vPath.Replace('\\', '/');

        return vPath;
    }

    /// <summary>
    /// Return array of static files url (excluding domain url)
    /// </summary>
    /// <param name="context"></param>
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        context.Response.Cache.SetNoStore();

        LoadStaticResources();
        context.Response.Write(JsResourcesArray);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}