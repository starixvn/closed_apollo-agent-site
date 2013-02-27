<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._IPInfo.Whois_Handler" %>

using System;
using System.Web;
using System.Collections;

using Fanex.Age.UI;
using Sunplus.Agent.Model.Entities;


namespace Sunplus.Agent.UI._IPInfo
{
    public class Whois_Handler : TplHandler
    {

        public override void RenderResult(ref JSONData data)
        {
            string url = "http://cloakedlink.com/legacytools.dnsstuff.com/tools/ipall.ch?tool_id=67&token=&toolhandler_redirect=0&ip=" + Request["ip"] + "&j=1";
            Response.Redirect(url, true);
        }

        public override object InvokeData(Hashtable p)
        {
            return null;
        }

        public override void ProcessRequest(ref Hashtable p)
        {

        }

        public override object ValidateRequest(Hashtable p)
        {
            return null;
        }
    }
}