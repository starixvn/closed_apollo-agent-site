using System;
using System.Web.UI;
using Fanex.Agent.UI._IPInfo;
using Sunplus.Core.Common.IPs;
using Sunplus.Core.Common.IPs.DAL;

/*
 * Created 20100212@Ryan - Render IP information page
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.UI._IPInfo
{
    public partial class IPInfo_Page : Fanex.Age.UI.TplPage
    {
        public string IP
        {
            get
            {
                string ip = Request.QueryString["ip"];
                if (String.IsNullOrEmpty(ip)) ip = Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP();
                return ip;
            }
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            return null;
        }

        public override void RenderContent(HtmlTextWriter writer)
        {
            IpInfo_Template template = new IpInfo_Template();
            template.lblip = Resources.Lang.ip;
            template.lblcountry = Resources.Lang.country;
            template.lblcity = Resources.Lang.city;
            template.ipnote = string.Format(Resources.Lang.ipnote, "Whois.ashx?ip=" + this.IP);

            string currentIP = this.IP;
            IPTable ipInfo = IpUtils.LocationFromIP(currentIP);
            template.ip = currentIP;
            template.countryname = ipInfo.CountryLong;
            template.cityname = ipInfo.City;
            template.flagpath = "../Flags/" + ipInfo.CountryShort + ".gif";
            writer.Write(template);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }
    }
}