using System;
using System.Text.RegularExpressions;

/*
 * Created 20100305@Ryan - Handle IP format rule for IBC
 */

namespace Sunplus.Core.Common.IPs
{
    public interface IContextIPInfo
    {
        int AdminId { get; set; }
        bool IsAdminCanViewVNIP { get; set; }
        string DefaultNoIPVN { get; set; }
    }

    public class ContextIPInfo : IContextIPInfo
    {
        #region IContextIPInfo Members

        public int AdminId { get; set; }
        public bool IsAdminCanViewVNIP { get; set; }
        public string DefaultNoIPVN { get; set; }

        #endregion
    }

    /// <summary>
    /// IBC IP Format Functions
    /// </summary>
    public class IPFormat
    {
        public string IP { get; set; }

        protected IPFormat() { }
        public IPFormat(string ip)
        {
            IP = ip;
        }
        
        protected virtual IContextIPInfo Context
        {
            get
            {
                return new ContextIPInfo
                {
                    AdminId = 0,
                    IsAdminCanViewVNIP = true,
                    DefaultNoIPVN = String.Empty
                };
            }
        }

        protected virtual string LinkFormat
        {
            get { return "<a href=\"javascript:OpenIPInfo('{0}');\" class=\"iplink\">{0}</a>"; }
        }

        protected virtual string LinkNoFormat
        {
            get { return "<span class='ipnolink'>{0}</span>"; }
        }

        /// <summary>
        /// Format IP string with IBC business and link to popup ip info
        /// </summary>        
        public string Format()
        {
            if (String.IsNullOrEmpty(this.IP)) return String.Empty;
            string result = String.Format(LinkFormat, this.IP);
            IContextIPInfo context = Context;
            bool isAdminLogin = (context.AdminId > 0);
            bool isAdminCanViewVNIP = context.IsAdminCanViewVNIP;

            if (isAdminLogin)
            {
                bool isVNIP = Sunplus.Core.Common.IPs.IpUtils.IsVnIp(IP);
                if (isVNIP && !isAdminCanViewVNIP)
                {
                    result = String.Format(LinkNoFormat, context.DefaultNoIPVN);
                }
            }
            return result;
        }

        /// <summary>
        /// Format all IPs detected inside content
        /// </summary>
        /// <param name="content">input text</param>
        /// <returns>output text that contains all IPs formated inside</returns>
        public string FormatAll(string content)
        {
            Regex regex = new Regex(Sunplus.Core.Common.IPs.IpUtils.IpPattern, RegexOptions.Compiled);
            return regex.Replace(content,
                    match =>
                    {
                        this.IP = match.Value;
                        return this.Format();
                    }
            );
        }
    }
}
