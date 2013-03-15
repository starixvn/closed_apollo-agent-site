using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Sunplus.Agent.UI._ViewLog
{
    using Sunplus.Agent.Model.Common;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;
    using Sunplus.Core.Common.IPs;
    using Sunplus.Shared.ViewLogLib;
    using Sunplus.Shared.ViewLogLib.Entities;
    using Sunplus.Shared.ViewLogLib.Services;

    public class XsltLogFormat
    {
        const string RESOURCE_FILE_NAME = "ViewLog";

        public XsltLogFormat()
        {
            if (_visibilities == null)
            {
                _visibilities = new Dictionary<string, bool>();
                _visibilities.Add("151_31", UserPermission.IsShowRacing_151_31);
                _visibilities.Add("151_41", UserPermission.IsShowRacing_151_41);
                _visibilities.Add("151_51", UserPermission.IsShowRacing_151_51);
                _visibilities.Add("152_41", UserPermission.IsShowRacing_152_41);
                _visibilities.Add("152_51", UserPermission.IsShowRacing_152_51);
                _visibilities.Add("153_51", UserPermission.IsShowRacing_153_51);
                _visibilities.Add("161_90", UserPermission.IsShowNumberGroup);
                _visibilities.Add("180", UserPermission.IsShowVirtualSports);
            }
        }

        private Dictionary<string, bool> _visibilities = null;

        public bool IsVisible(string key)
        {
            return _visibilities[key];
        }

        public static IUserSession CurrentUserSession
        {
            get
            {
                return UserSession.Current;
            }
        }

        static object locker = new object();
        static ResourceManager _resourceManager = null;

        public string GetLogResource(string key)
        {
            if (_resourceManager == null)
            {
                lock (locker)
                {
                    if (_resourceManager == null)
                    {
                        _resourceManager = new ResourceManager(typeof(Resources.Viewlog));
                        _resourceManager.IgnoreCase = true;
                    }
                }
            }

            return _resourceManager.GetString(key) ?? string.Empty;
        }

        public string GetSportName(int sporttype)
        {
            return CacheRef.GetSportName(sporttype);
        }

        private string CovertCodeCondition(string code)
        {
            char[] c = code.Trim().ToCharArray();
            string condition = string.Empty;
            string hyphen = " - ";
            if (c.Length > 0)
            {
                if (c[7].ToString() == "0") // daily
                {
                    return Resources.Lang.daily;
                }
                else
                {
                    if (c[0] == '1') condition += Resources.Lang.sun + hyphen;
                    if (c[1] == '1') condition += Resources.Lang.mon + hyphen;
                    if (c[2] == '1') condition += Resources.Lang.tue + hyphen;
                    if (c[3] == '1') condition += Resources.Lang.wed + hyphen;
                    if (c[4] == '1') condition += Resources.Lang.thurs + hyphen;
                    if (c[5] == '1') condition += Resources.Lang.fri + hyphen;
                    if (c[6] == '1') condition += Resources.Lang.sat + hyphen;

                    return Resources.Lang.weekly + " (" + condition.Remove(condition.Length - 3, 3) + ")";
                }
            }
            return condition;
        }

        public string TransferConditionFromCode(string code)
        {
            if (string.IsNullOrEmpty(code) || code == " ") return string.Empty;
            if (code.IndexOf("</div>") != -1)
            {
                string[] arr = code.Split("</div>".ToCharArray());
                string valuechange = arr[23];
                string valueold = arr[4];
                code = code.Replace(valuechange, CovertCodeCondition(valuechange));
                code = code.Replace(valueold, CovertCodeCondition(valueold));
                return code;
            }
            return CovertCodeCondition(code);
        }

        public static string FormatIp(string ip)
        {
            if (string.IsNullOrEmpty(ip)) return string.Empty;

            bool canViewVNIp = CurrentUserSession.IsAdminCanViewVNIP;

            if (IpUtils.IsVnIp(ip) && !canViewVNIp)
            {
                return string.Empty;
            }

            return IpUtils.FormatIp(ip);
        }

        public static string FormatCountryCode(string ip, string country)
        {
            if (string.IsNullOrEmpty(ip)) return string.Empty;

            bool canViewVNIp = CurrentUserSession.IsAdminCanViewVNIP;

            if (IpUtils.IsVnIp(ip) && !canViewVNIp)
            {
                return string.Empty;
            }

            return country;
        }

        public static string FormatBettypeID(string bettypeId)
        {
            return Sunplus.Agent.Model.BL.Cache.CacheRef.GetBettype((object)bettypeId);
        }

        public static string FormatCodeSite(string code)
        {
            string condition = string.Empty;
            if (!string.IsNullOrEmpty(code))
            {
                if (code == "2")
                {
                    condition = Resources.Viewlog.agentSite;
                }
                else if (code == "1")
                {
                    condition = Resources.Viewlog.accountingSite;
                }
            }
            return condition;
        }

        public static string FormatErrCode(string code)
        {
            if (string.IsNullOrEmpty(code)) return "0";

            return code;
        }

        public static string FormatErrMsg(string code)
        {
            if (string.Compare(code, "no information is available") == 0)
            {
                code = "";
            }

            return code;
        }

        public static string FormatMemberWinLimit(string value)
        {
            return value.Replace("x", "X").Replace("unlimited", Resources.Lang.Unlimited);
        }
    }
}