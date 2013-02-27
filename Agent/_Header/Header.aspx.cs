using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using Fanex.Age.UI;
using Fanex.Age.UI.Components;
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
            RegisterJavascriptObject("CookieCurrentURLKey", CookieCurrentURLKey);

            #endregion Register javascript

            Header_Template tpl = new Header_Template();

            #region Css filter for Super, Master, Agent

            if (session.RoleId == (int)(UserTypes.Agent))
            {
                tpl.CssMaster = null;
                tpl.CssSuper = null;
            }
            else if (session.RoleId == (int)(UserTypes.Master))
            {
                tpl.CssAgent = null;
                tpl.CssSuper = null;
            }
            else if (session.RoleId == (int)(UserTypes.Super))
            {
                tpl.CssAgent = null;
                tpl.CssMaster = null;
            }

            #endregion Css filter for Super, Master, Agent

            #region Transfer filter for Super, Master, Agent

            //tpl.Transfer.TransferURL = "Transfer/MultipleTransfer.aspx";

            if (session.RoleId == (int)(UserTypes.Agent))
            {
                tpl.Transfer.TransferURL = "_Transfer/Agent/MemberList.aspx";
            }
            else if (session.RoleId == (int)(UserTypes.Master))
            {
                tpl.Transfer.TransferURL = "_Transfer/Master/AgentList.aspx";
            }
            else if (session.RoleId == (int)(UserTypes.Super))
            {
                tpl.Transfer.TransferURL = "_Transfer/Super/MasterList.aspx";
            }

            #endregion Transfer filter for Super, Master, Agent

            #region Tab filter for sub-account

            if (session.IsSubAcc)
            {
                PageAccess pageAccess = session.SubPageAccess;

                // Transfer tab
                if (!pageAccess.Transfer)
                {
                    tpl.Transfer = null;
                }

                // View log tab
                if (!pageAccess.ViewLog)
                {
                    tpl.Viewlog = null;
                }
            }

            #endregion Tab filter for sub-account

            if (!CurrentUserSession.IsLogged)
            {
                tpl.Transfer = null;
                tpl.Viewlog = null;
            }

            if (!Sunplus.Agent.Model.BL.Definitions.AppSettings.ShowViewLog)
            {
                tpl.Viewlog = null;
            }

            tpl.langFlag = "langFlag_" + session.LangKey.ToLower();

            tpl.popupTopDisplay = "none";

            tpl.roleid = session.RoleId.ToString();
            tpl.userid = session.UserId.ToString();
            tpl.subaccid = session.SubAccId.ToString();

            // Show introduction flash here
            tpl.flash = null;

            tpl.cdate = DateTime.Now.AddHours(12).ToString("MMM dd,yyyy 'GMT'+8");
            tpl.ctime = DateTime.Now.AddHours(12).ToString("H-m-s-tt");

            string space = " ";
            if (session.IsSubAcc)
            {
                tpl.SetUserName.SetValue(HttpUtility.HtmlEncode(session.UserName));
                tpl.addAccName = Resources.Lang.subacc + space + HttpUtility.HtmlEncode(session.SubUserName);
                tpl.Tooltip = null;
                tpl.SetNickName = null;
            }
            else if (session.IsInternal)
            {
                tpl.SetUserName.SetValue(HttpUtility.HtmlEncode(session.UserName));
                tpl.addAccName = Resources.Lang.admin + space + HttpUtility.HtmlEncode(session.AdminName);
                tpl.Tooltip = null;
                tpl.SetNickName = null;
            }
            else
            {
                tpl.addAccName = null;

                #region Check counter display Tooltip

                if (session.NickName != null && session.NickName != "")
                {
                    tpl.SetNickName.nickname = session.NickName;
                    tpl.Tooltip = null;
                    tpl.SetNickName.IconSetNickName.IsShowIcon = null;
                    tpl.SetNickName.LabelNickName = null;
                    tpl.SetNickName.SetLinkConfigNow = "linkNickName";
                }
                else
                {
                    if (CountNickNameToolTip > 3)
                        tpl.Tooltip = null;
                    else
                        CountNickNameToolTip++;
                    tpl.SetNickName.nickname = Resources.Lang.configNow;
                    tpl.SetNickName.IconSetNickName.IsShowIcon = "none";
                    tpl.SetNickName.SetLinkConfigNow = "linkConfigNow";
                }

                #endregion Check counter display Tooltip

                tpl.SetUserName.mainAccName = session.UserName;
            }

            #region User Message

            SetHotMessage(ref tpl);
            tpl.issticky = string.Empty;
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

                if (string.Compare(CurrentUserSession.Site, "789y", true) != 0) // i b c
                {

                    if (CurrentUserSession.IsLiveCasinoSync)
                    {
                        string msgUS = "Dear valued agents,<br />We added a new feature ‘Member Win Limit’ for you to better handling your member winning in Live Casino. You may now go to your downline list and click on ‘Edit Live Casino’ for configuration. Click <a href='../_HowToUse/Index.aspx?lang=ja-JP' target='_blank' style='color:Blue'>here</a> for more information.";
                        //Chinese Simplifier
                        string msgCN = "敬爱的代理们,<br />我们新增了一项新功能--会员的赢额上限，让您能更轻易的控制会员在娱乐厅里的赢额上限。您可到下线列表中的修改栏中点击娱乐厅以做出调整。点击<a href='../_HowToUse/Index.aspx?lang=zh-CN' target='_blank' style='color:Blue'>这</a>以了解更多关于此功能的详情。";
                        //Chinese Traditional
                        string msgTW = "敬愛的代理們,<br />我們新增了一項新功能--會員的贏額上限，讓您能更輕易的控制會員在娛樂廳裡的贏額上限。您可到下線列表中的修改欄中點擊娛樂廳以做出調整。點擊<a href='../_HowToUse/Index.aspx?lang=zh-TW' target='_blank' style='color:Blue'>這</a>以了解更多關於此功能的詳情。";
                        string msgTH = "เรียนลูกค้าที่เคารพ,<br />เราได้เพิ่มฟังก์ชันใหม่คือ 'วงเงินชนะสูงสุดของสมาชิก' เพื่อให้คุณสามารถกำหนดวงเงินชนะในคาสิโนเกมสดของลูกค้าได้. คุณสามารถทำการตั้งค่าได้ที่รายชื่อคลิกที่แก้ไขคาสิโนเกมสด. คลิก<a href='../_HowToUse/Index.aspx?lang=th-TH' target='_blank' style='color:Blue'>ที่นี่</a>เพื่อตรวจสอบข้อมูลเพิ่มเติม";
                        string msgKR = "친애하는 고객님께,<br />회원의 라이브 카지노 지급금액 한도에 대한 새로운 기능이 추가 되었습니다.이는 고객님이 회원관리를 좀 더 수월하게 관리할 수 있는 기능입니다.이 기능을 보실려면, 하부 라인목록에서 '라이브 카지노 수정하기'를 선택하여 주십시요.이 기능을 보실려면, 하부 라인목록에서 '라이브 카지노 수정하기'를 선택하여 주십시요. 좀더 자세한 내용은 <a href='../_HowToUse/Index.aspx?lang=ko-KR' target='_blank' style='color:Blue'>여기</a>를 클릭하여 주십시요.";

                        msgListUS.Add(msgUS);
                        msgListTW.Add(msgTW);
                        msgListCN.Add(msgCN);
                        msgListJP.Add(msgUS);
                        msgListTH.Add(msgTH);
                        msgListKR.Add(msgKR);
                    }
                }
                
                if (string.Compare(CurrentUserSession.Site, "789y", true) == 0) // hardcode for 789y message
                {
                    string msgUS = "Dear valued agents,<br />We added a new feature ‘Member Win Limit’ for you to better handling your member winning in Live Casino. You may now go to your downline list and click on ‘Edit Live Casino’ for configuration.";
                    //Chinese Simplifier
                    string msgCN = "敬爱的代理们,<br />我们新增了一项新功能--会员的赢额上限，让您能更轻易的控制会员在娱乐厅里的赢额上限。您可到下线列表中的修改栏中点击娱乐厅以做出调整。";
                    //Chinese Traditional
                    string msgTW = "敬愛的代理們,<br />我們新增了一項新功能--會員的贏額上限，讓您能更輕易的控制會員在娛樂廳裡的贏額上限。您可到下線列表中的修改欄中點擊娛樂廳以做出調整。";
                    string msgTH = "เรียนลูกค้าที่เคารพ,<br />เราได้เพิ่มฟังก์ชันใหม่คือ 'วงเงินชนะสูงสุดของสมาชิก' เพื่อให้คุณสามารถกำหนดวงเงินชนะในคาสิโนเกมสดของลูกค้าได้. คุณสามารถทำการตั้งค่าได้ที่รายชื่อคลิกที่แก้ไขคาสิโนเกมสด. ";
                    string msgKR = "친애하는 고객님께,<br />회원의 라이브 카지노 지급금액 한도에 대한 새로운 기능이 추가 되었습니다.이는 고객님이 회원관리를 좀 더 수월하게 관리할 수 있는 기능입니다.이 기능을 보실려면, 하부 라인목록에서 '라이브 카지노 수정하기'를 선택하여 주십시요.이 기능을 보실려면, 하부 라인목록에서 '라이브 카지노 수정하기'를 선택하여 주십시요. ";

                    if (CurrentUserSession.IsLiveCasinoSync)
                    {
                        msgListUS.Add(msgUS);
                        msgListTW.Add(msgTW);
                        msgListCN.Add(msgCN);
                        msgListJP.Add(msgUS);
                        msgListTH.Add(msgTH);
                        msgListKR.Add(msgKR);

                        msgUS = "Quý khách hàng lưu ý: Trang web ‘www.3399933.com’, ‘www.9966699.com’, ‘www.9911199.com’, ‘www.3311133.com’ và ‘www.5511155.com’ Không thuộc ibet888.net của chúng tôi. Đăng nhập những trang web này, Quý khách sẽ không thể truy cập được “Live Casino” và “Number Game”, từ đây chứng minh được những trang web này là giả mạo. Để tránh có sự nhầm lẫn hoặc bất cứ hành vi lừa đảo xảy ra, chúng tôi kiến nghị Quý khách liên hệ với Đại Lý để được tư vấn trực tiếp.";
                        //Chinese Simplifier
                        msgCN = "各位尊貴的會員請注意: 網址‘www.3399933.com’, ‘www.9966699.com’, ‘www.9911199.com’, ‘www.3311133.com’ 及 ‘www.5511155.com’ 並不屬於ibet888.net. 登入該網站,您將無法訪問“娛樂廳”及“百練賽”的部分,這證明了該網站的冒充行為. 我們強烈建議您聯絡您的上线以便直接查詢, 這可避免任何誤會或欺詐行為的產生.";
                        //Chinese Traditional
                        msgTW = "各位尊貴的會員請注意: 網址‘www.3399933.com’, ‘www.9966699.com’, ‘www.9911199.com’, ‘www.3311133.com’ 及 ‘www.5511155.com’ 並不屬於ibet888.net. 登入該網站,您將無法訪問“娛樂廳”及“百練賽”的部分,這證明了該網站的冒充行為. 我們強烈建議您聯絡您的上线以便直接查詢, 這可避免任何誤會或欺詐行為的產生.";
                        msgTH = "Quý khách hàng lưu ý: Trang web ‘www.3399933.com’, ‘www.9966699.com’, ‘www.9911199.com’, ‘www.3311133.com’ và ‘www.5511155.com’ Không thuộc ibet888.net của chúng tôi. Đăng nhập những trang web này, Quý khách sẽ không thể truy cập được “Live Casino” và “Number Game”, từ đây chứng minh được những trang web này là giả mạo. Để tránh có sự nhầm lẫn hoặc bất cứ hành vi lừa đảo xảy ra, chúng tôi kiến nghị Quý khách liên hệ với Đại Lý để được tư vấn trực tiếp.";
                        msgKR = "Quý khách hàng lưu ý: Trang web ‘www.3399933.com’, ‘www.9966699.com’, ‘www.9911199.com’, ‘www.3311133.com’ và ‘www.5511155.com’ Không thuộc ibet888.net của chúng tôi. Đăng nhập những trang web này, Quý khách sẽ không thể truy cập được “Live Casino” và “Number Game”, từ đây chứng minh được những trang web này là giả mạo. Để tránh có sự nhầm lẫn hoặc bất cứ hành vi lừa đảo xảy ra, chúng tôi kiến nghị Quý khách liên hệ với Đại Lý để được tư vấn trực tiếp.";

                        msgListUS.Add(msgUS);
                        msgListTW.Add(msgTW);
                        msgListCN.Add(msgCN);
                        msgListJP.Add(msgUS);
                        msgListTH.Add(msgTH);
                        msgListKR.Add(msgKR);
                    }
                    else
                    {
                        msgUS = "Quý khách hàng lưu ý: Trang web ‘www.3399933.com’, ‘www.9966699.com’, ‘www.9911199.com’, ‘www.3311133.com’ và ‘www.5511155.com’ Không thuộc ibet888.net của chúng tôi. Đăng nhập những trang web này, Quý khách sẽ không thể truy cập được “Live Casino” và “Number Game”, từ đây chứng minh được những trang web này là giả mạo. Để tránh có sự nhầm lẫn hoặc bất cứ hành vi lừa đảo xảy ra, chúng tôi kiến nghị Quý khách liên hệ với Đại Lý để được tư vấn trực tiếp.";
                        //Chinese Simplifier
                        msgCN = "各位尊貴的會員請注意: 網址‘www.3399933.com’, ‘www.9966699.com’, ‘www.9911199.com’, ‘www.3311133.com’ 及 ‘www.5511155.com’ 並不屬於ibet888.net. 登入該網站,您將無法訪問“娛樂廳”及“百練賽”的部分,這證明了該網站的冒充行為. 我們強烈建議您聯絡您的上线以便直接查詢, 這可避免任何誤會或欺詐行為的產生.";
                        //Chinese Traditional
                        msgTW = "各位尊貴的會員請注意: 網址‘www.3399933.com’, ‘www.9966699.com’, ‘www.9911199.com’, ‘www.3311133.com’ 及 ‘www.5511155.com’ 並不屬於ibet888.net. 登入該網站,您將無法訪問“娛樂廳”及“百練賽”的部分,這證明了該網站的冒充行為. 我們強烈建議您聯絡您的上线以便直接查詢, 這可避免任何誤會或欺詐行為的產生.";
                        msgTH = "Quý khách hàng lưu ý: Trang web ‘www.3399933.com’, ‘www.9966699.com’, ‘www.9911199.com’, ‘www.3311133.com’ và ‘www.5511155.com’ Không thuộc ibet888.net của chúng tôi. Đăng nhập những trang web này, Quý khách sẽ không thể truy cập được “Live Casino” và “Number Game”, từ đây chứng minh được những trang web này là giả mạo. Để tránh có sự nhầm lẫn hoặc bất cứ hành vi lừa đảo xảy ra, chúng tôi kiến nghị Quý khách liên hệ với Đại Lý để được tư vấn trực tiếp.";
                        msgKR = "Quý khách hàng lưu ý: Trang web ‘www.3399933.com’, ‘www.9966699.com’, ‘www.9911199.com’, ‘www.3311133.com’ và ‘www.5511155.com’ Không thuộc ibet888.net của chúng tôi. Đăng nhập những trang web này, Quý khách sẽ không thể truy cập được “Live Casino” và “Number Game”, từ đây chứng minh được những trang web này là giả mạo. Để tránh có sự nhầm lẫn hoặc bất cứ hành vi lừa đảo xảy ra, chúng tôi kiến nghị Quý khách liên hệ với Đại Lý để được tư vấn trực tiếp.";

                        msgListUS.Add(msgUS);
                        msgListTW.Add(msgTW);
                        msgListCN.Add(msgCN);
                        msgListJP.Add(msgUS);
                        msgListTH.Add(msgTH);
                        msgListKR.Add(msgKR);

                        msgUS = "Hãy đến và tham gia bốc thăm may mắn hàng ngày của chúng tôi! Quý khách có cơ hội để giành chiến thắng lên đến 10.000 USD tiền thưởng! Chỉ cần Quý khách cá cược thông thường, Quý khách sẽ có thể trở thành Người may mắn của chúng tôi!";
                        //Chinese Simplifier
                        msgCN = "快來參加我們的每日幸運大抽獎! 您有機會贏取高達10,000美元的獎金! 只要您像往常一樣的投注, 您將有可能成為我們的幸運兒!";
                        //Chinese Traditional
                        msgTW = "快來參加我們的每日幸運大抽獎! 您有機會贏取高達10,000美元的獎金! 只要您像往常一樣的投注, 您將有可能成為我們的幸運兒!";
                        msgTH = "Hãy đến và tham gia bốc thăm may mắn hàng ngày của chúng tôi! Quý khách có cơ hội để giành chiến thắng lên đến 10.000 USD tiền thưởng! Chỉ cần Quý khách cá cược thông thường, Quý khách sẽ có thể trở thành Người may mắn của chúng tôi!";
                        msgKR = "Hãy đến và tham gia bốc thăm may mắn hàng ngày của chúng tôi! Quý khách có cơ hội để giành chiến thắng lên đến 10.000 USD tiền thưởng! Chỉ cần Quý khách cá cược thông thường, Quý khách sẽ có thể trở thành Người may mắn của chúng tôi!";

                        msgListUS.Add(msgUS);
                        msgListTW.Add(msgTW);
                        msgListCN.Add(msgCN);
                        msgListJP.Add(msgUS);
                        msgListTH.Add(msgTH);
                        msgListKR.Add(msgKR);
                    }


                }

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