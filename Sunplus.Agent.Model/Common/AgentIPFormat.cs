/*
 * Created 20100305@Ryan - IP format for Agent site
 * Revision:
 *
 */

namespace Sunplus.Agent.Model.Common
{
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Core.Common.IPs;

    /// <summary>
    /// IP Handle for Agent site
    /// </summary>
    public class AgentIPFormat : IPFormat
    {
        public AgentIPFormat(string ip)
            : base(ip)
        {
        }

        protected override IContextIPInfo Context
        {
            get
            {
                IUserSession sessionInfo = UserSession.Current;
                return new ContextIPInfo
                {
                    AdminId = sessionInfo.AdminId,
                    IsAdminCanViewVNIP = sessionInfo.IsAdminCanViewVNIP
                };
            }
        }

        public static string FormatIP(string ip)
        {
            return new AgentIPFormat(ip).Format();
        }

        public static string FormatIPs(string content)
        {
            return new AgentIPFormat(string.Empty).FormatAll(content);
        }

        public static string[] GetClientIPs()
        {
            string IP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            string[] IPs = string.IsNullOrEmpty(IP) ? new string[] { "" } : IP.Split(',');
            return IPs;
        }

        public static string GetClientIP()
        {
            string[] IPs = GetClientIPs();
            return IPs[0];
        }
    }
}