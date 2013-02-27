<%@ WebHandler Language="C#" Class="RenameDBNAME" %>

using System;
using System.Web;
using System.IO;
using System.Collections;

public class RenameDBNAME : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {

        context.Response.ContentType = "text/xml";

        // Read all xtest from a file xtest.txt and put into a collection
        string[] sps = File.ReadAllLines(HttpContext.Current.Server.MapPath("~/dbname.txt"));

        string[] spsdbs = File.ReadAllLines(HttpContext.Current.Server.MapPath("~/SpNames.xml"));

        System.Text.StringBuilder text = new System.Text.StringBuilder(spsdbs.Length * 100);

        foreach (string spdb in spsdbs)
        {
            string line = spdb;
            foreach (string sp in sps)
            {
                string[] keys = sp.Trim().Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (keys.Length > 3) throw new Exception("Invalid format: \n" + sp);
                if (keys.Length == 0) continue;

                string dbAlias = keys[0].Trim();
                string dbCatalog = keys[1].Trim();
                string spName = keys[2].Trim();

                if (spdb.Contains("SpName=\"" + spName + "\""))
                {
                    int i = spdb.IndexOf("DbName=\"");
                    int j = spdb.IndexOf("\"", i + 8);

                    string dbname = spdb.Substring(i + 8, j - i - 8);
                    line = spdb.Replace("DbName=\"" + dbname + "\"", "DbName=\"" + dbAlias + "." + dbCatalog + "\"");
                    break;
                }
            }

            text.AppendLine(line);
        }

        context.Response.Write(text);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}