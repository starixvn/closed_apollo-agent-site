using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Fanex.Age.UI;
using Fanex.Age.UI.Components;
using Fanex.Agent.UI._Header;
using Sunplus.Agent.Model.BL;
using Sunplus.Agent.Model.BL.Definitions;
using Sunplus.Agent.Model.Entities;
using Sunplus.Agent.Utilities;

/*
 * Created 20100126@Lee - Render header page
 * Revision:
 * Don@20100617 -  Refactor user message function: remove codes of showing message when the first request is reached.
 *
 */

namespace Sunplus.Agent.UI._Header
{
    public partial class Header_Page : Fanex.Age.UI.TplPage
    {
        public string CookieCurrentURLKey
        {
            get
            {
                string name = string.Join("_", new string[] {
               UserSession.Current.CustId.ToString(),
               UserSession.Current.SubAccId.ToString(),
               "URL"
            });

                return Encryption.PearsonEncode(name);
            }
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            //TplCore.TplSettings.CurrentTheme = "xmas";
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            return null;
        }

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            IUserSession session = CurrentUserSession;

            #region Register javascript

            RegisterJavascriptObject("Language", session.Language);
            RegisterJavascriptObject("OnUserID", session.OnUserId);
            RegisterJavascriptObject("ShowLetter", false);
            RegisterJavascriptObject("Months", "['" + Resources.Lang.jan + "', '" + Resources.Lang.feb + "', '" + Resources.Lang.mar + "', '" + Resources.Lang.apr + "', '" + Resources.Lang.may2 + "', '" + Resources.Lang.jun + "', '" + Resources.Lang.jul + "', '" + Resources.Lang.aug + "', '" + Resources.Lang.sept + "', '" + Resources.Lang.oct + "', '" + Resources.Lang.nov + "', '" + Resources.Lang.dec + "']");
            RegisterJavascriptObject("year", DateTime.Now.Year.ToString());
            RegisterJavascriptObject("month", DateTime.Now.Month.ToString());
            RegisterJavascriptObject("day", DateTime.Now.Day.ToString());
            RegisterJavascriptObject("hrs", DateTime.Now.Hour.ToString());
            RegisterJavascriptObject("min", DateTime.Now.Minute.ToString());
            RegisterJavascriptObject("sec", DateTime.Now.Second.ToString());
            RegisterJavascriptObject("PM", Resources.Lang.PM);
            RegisterJavascriptObject("AM", Resources.Lang.AM);
            string urlPath = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath;
            RegisterJavascriptObject("getBaseURL", urlPath.LastIndexOf("/", urlPath.Length - 1, 1) != -1 ? urlPath : urlPath + "/");

            // RegisterJavascriptObject("CookieCurrentURLKey", CookieCurrentURLKey);

            #endregion Register javascript

            Header_Template tpl = new Header_Template();

            tpl.roleid = session.RoleId.ToString();
            tpl.userid = session.UserId.ToString();
            tpl.subaccid = session.SubAccId.ToString();

            // Show introduction flash here
            tpl.flash = null;

            tpl.cdate = DateTime.Now.AddHours(12).ToString("MMM dd,yyyy 'GMT'+8");
            tpl.ctime = DateTime.Now.AddHours(12).ToString("H-m-s-tt");

            

            tpl.SetUserName.mainAccName = HttpUtility.HtmlEncode(session.Username);
            tpl.SetUserName.AgentUsername = HttpUtility.HtmlEncode(session.AgentName);

            #region User Message

            SetHotMessage(ref tpl);
            //tpl.issticky = string.Empty;
            tpl.scrollMessage = string.Empty;
            tpl.priMsgCount = InitUnreadPrivateMsgCountText();

            #endregion User Message

            writer.Write(tpl);
        }

        #region Set and get Cookie for Tooltip introduction new function

        private string CookiesNameToolTip
        {
            get
            {
                return Encryption.CfsEnCode("ToolTip" + UserSession.Current.CustId.ToString(), 6);
            }
        }

        private int CountNickNameToolTip
        {
            get
            {
                if (Request.Cookies[CookiesNameToolTip] == null) return 0;
                return int.Parse(Request.Cookies[CookiesNameToolTip].Value);
            }
            set
            {
                HttpCookie objCookie = new HttpCookie(CookiesNameToolTip);
                Response.Cookies.Clear();
                Response.Cookies.Add(objCookie);
                objCookie.Value = value.ToString();
                DateTime dtExpiry = DateTime.Now.AddDays(30);
                Response.Cookies[CookiesNameToolTip].Expires = dtExpiry;
            }
        }

        #endregion Set and get Cookie for Tooltip introduction new function

        private void SetHotMessage(ref Header_Template htmlContent)
        {
            List<string> msgListUS = new List<string>();
            List<string> msgListTW = new List<string>();
            List<string> msgListCN = new List<string>();
            List<string> msgListJP = new List<string>();
            List<string> msgListTH = new List<string>();
            List<string> msgListKR = new List<string>();

            bool isShowed = false;
            if (!isShowed && !true.Equals(Session["MessageIsShowed"]))
            {
                isShowed = true;             

                Session["MessageIsShowed"] = isShowed;
            }

            string[] msgList = null;

            // Register correct message language
            if (string.Compare(CurrentUserSession.Language, "zh-TW") == 0)
            {
                msgList = msgListTW.ToArray();
            }
            else if (string.Compare(CurrentUserSession.Language, "zh-CN") == 0)
            {
                msgList = msgListCN.ToArray();
            }
            else if (string.Compare(CurrentUserSession.Language, "zh-JP") == 0)
            {
                msgList = msgListJP.ToArray();
            }
            else if (string.Compare(CurrentUserSession.Language, "zh-TH") == 0)
            {
                msgList = msgListTH.ToArray();
            }
            else if (string.Compare(CurrentUserSession.Language, "zh-KR") == 0)
            {
                msgList = msgListKR.ToArray();
            }
            else
            {
                msgList = msgListUS.ToArray();
            }

            RegisterJavascriptObject("msgList", msgList);
        }

        private string InitUnreadPrivateMsgCountText()
        {
            string result = "{0}({1})", firstWord = "Personal";
            string displayText = Resources.Lang.personalmessage;
            if (displayText.IndexOf(firstWord) >= 0) //check if Language is en-US
            {
                displayText = firstWord + " Msg";
            }
            string count = "0";

            return string.Format(result, displayText, count);
        }
    }
}