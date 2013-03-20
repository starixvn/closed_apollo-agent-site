/*
 * Created 20110707@Simon - Render popup announcement.
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.UI._MemberInfo.Announcement
{
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Resources;
    using System.Threading;
    using System.Web.UI;
    using System.Xml;
    using Fanex.Age.UI.Components;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.BL.UserMessage;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.UI._Announcement;
    using Sunplus.Agent.Utilities;

    public partial class Announcement_Page : Fanex.Age.UI.TplPage
    {
        private IList<long> showedMessageIds = new List<long>();

        public override void RenderContent(HtmlTextWriter writer)
        {
            string src = string.Empty;
            long id = 0;
            int top = 110, left = 400, width = 400, height = 200;

            string msg = this.GetSettingChangedMsg(out id);

            if (0 == id)
            {
                msg = this.GetAnnouncementMsg(out src, out id, out top, out left, out width, out height);
            }
            ////Other if here...

            if (0 != id)
            {
                writer.Write(this.CreatePopup(id, msg, top, left, width, height, src));
            }
            else
            {
                writer.Write("-");
            }
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            string id = Request["id"];

            if (!string.IsNullOrEmpty(id))
            {
                string[] ids = id.Split(new char[] { ',', ';', ':' }, System.StringSplitOptions.RemoveEmptyEntries);
                foreach (string element in ids)
                {
                    int i = 0;
                    int.TryParse(element, out i);
                    if (0 == i)
                    {
                        continue;
                    }

                    this.showedMessageIds.Add(i);
                }
            }
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            UserMessageBO userMessageBO = new UserMessageBO(CurrentUserSession.NetSessionId);
            DataTable dt = userMessageBO.GetPopupMessageStatus();

            bool isPositionTakingChanged = false, isBettingLimitChanged = false;
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (!dt.Columns.Contains("typeid"))
                    {
                        break;
                    }

                    int typeId = 0;

                    int.TryParse(row["typeid"] + string.Empty, out typeId);
                    ////For Position Taking Changed
                    if (typeId == 1)
                    {
                        isPositionTakingChanged = true;
                    }
                    else if (typeId == 2)
                    {
                        ////For Betting Limit Changed
                        isBettingLimitChanged = true;
                    }
                }
            }

            return new object[] { isPositionTakingChanged, isBettingLimitChanged };
        }

        ////Setting Changed Message
        public long InsertPersonalMsg()
        {
            string currentCulture = CurrentUserSession.Language;
            string[] msgs =
            {
                this.GetMsgByCulture("en-US"),
                this.GetMsgByCulture("zh-TW"),
                this.GetMsgByCulture("zh-CN"),
                this.GetMsgByCulture("ja-JP"),
                string.Empty,
                this.GetMsgByCulture("th-TH"),
                this.GetMsgByCulture("ko-KR"),
                string.Empty
            };

            CurrentUserSession.Language = currentCulture;

            UserMessageBO userMessageBO = new UserMessageBO(CurrentUserSession.NetSessionId);
            DataTable dt = userMessageBO.InsPersonalMessage(msgs[0], msgs[1], msgs[2], msgs[3], msgs[4], msgs[5], msgs[6], msgs[7], 1, false, 0, true);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    userMessageBO.InsPersonalMsgTarget((decimal)dt.Rows[0]["messageId"], CurrentUserSession.UserId, CurrentUserSession.Username, false, "0");
                    userMessageBO.UpdateViewedPTMessage(true);
                }
            }

            return (long)(decimal)dt.Rows[0]["messageId"];
        }

        public long InsertPositionTakingChangedMsg()
        {
            string curCulture = CurrentUserSession.Language;
            string[] arrLangMsg =
            {
                this.GetPositionTakingChangedMsgByCulture("en-US"),
                this.GetPositionTakingChangedMsgByCulture("zh-TW"),
                this.GetPositionTakingChangedMsgByCulture("zh-CN"),
                this.GetPositionTakingChangedMsgByCulture("ja-JP"),
                string.Empty,
                this.GetPositionTakingChangedMsgByCulture("th-TH"),
                this.GetPositionTakingChangedMsgByCulture("ko-KR"),
                string.Empty
            };
            CurrentUserSession.Language = curCulture;

            UserMessageBO userMessageBo = new UserMessageBO(CurrentUserSession.NetSessionId);
            DataTable dt = userMessageBo.InsPersonalMessage(arrLangMsg[0], arrLangMsg[1], arrLangMsg[2], arrLangMsg[3], arrLangMsg[4], arrLangMsg[5], arrLangMsg[6], arrLangMsg[7], 1, false, 0, true);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    userMessageBo.InsPersonalMsgTarget((decimal)dt.Rows[0]["messageId"], CurrentUserSession.UserId, CurrentUserSession.Username, false, "0");
                    userMessageBo.UpdateViewedPTMessage(true);
                }
            }

            return (long)(decimal)dt.Rows[0]["messageId"];
        }

        public long InsertBettingLimitChangedMsg()
        {
            string curCulture = CurrentUserSession.Language;
            string[] arrLangMsg =
            {
                this.GetBettingLimitChangedMsgByCulture("en-US"),
                this.GetBettingLimitChangedMsgByCulture("zh-TW"),
                this.GetBettingLimitChangedMsgByCulture("zh-CN"),
                this.GetBettingLimitChangedMsgByCulture("ja-JP"),
                string.Empty,
                this.GetBettingLimitChangedMsgByCulture("th-TH"),
                this.GetBettingLimitChangedMsgByCulture("ko-KR"),
                string.Empty
            };
            CurrentUserSession.Language = curCulture;

            UserMessageBO userMessageBo = new UserMessageBO(CurrentUserSession.NetSessionId);
            DataTable dt = userMessageBo.InsPersonalMessage(arrLangMsg[0], arrLangMsg[1], arrLangMsg[2], arrLangMsg[3], arrLangMsg[4], arrLangMsg[5], arrLangMsg[6], arrLangMsg[7], 1, false, 0, true);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    userMessageBo.InsPersonalMsgTarget((decimal)dt.Rows[0]["messageId"], CurrentUserSession.UserId, CurrentUserSession.Username, false, "0");
                    userMessageBo.UpdateViewedPTMessage(true);
                }
            }

            return (long)(decimal)dt.Rows[0]["messageId"];
        }

        private string GetMsgByCulture(string culture)
        {
            ResourceManager resourceManager = new ResourceManager(typeof(Resources.Lang));
            return resourceManager.GetString("modifiedMinPTBettingLimitValue", new CultureInfo(culture)).Replace("<br>", " ");
        }

        private string GetPositionTakingChangedMsgByCulture(string culture)
        {
            ResourceManager resourceManager = new ResourceManager(typeof(Resources.Lang));
            return resourceManager.GetString("modifiedMinPTValueText", new CultureInfo(culture)).Replace("<br>", " ");
        }

        private string GetBettingLimitChangedMsgByCulture(string culture)
        {
            ResourceManager resourceManager = new ResourceManager(typeof(Resources.Lang));
            return resourceManager.GetString("modifiedBettingLimitValue", new CultureInfo(culture)).Replace("<br>", " ");
        }

        private string GetSettingChangedMsg(out long id)
        {
            object[] dts = View as object[];
            bool isPositionTakingChanged = (bool)dts[0];
            bool isBettingLimitChanged = (bool)dts[1];
            string message = string.Empty;
            id = 0;

            ////Show PT and Betting Limit modifying information for Agent customer
            if (isPositionTakingChanged && isBettingLimitChanged)
            {
                message = Resources.Lang.modifiedMinPTBettingLimitValue;
                id = this.InsertPersonalMsg();
            }
            else
            {
                ////Show PT modifying information for Agent customer
                if (isPositionTakingChanged)
                {
                    message = Resources.Lang.modifiedMinPTValueText;
                    id = this.InsertPositionTakingChangedMsg();
                }
                ////Show Betting Limit modifying information for Agent customer
                if (isBettingLimitChanged)
                {
                    message = Resources.Lang.modifiedBettingLimitValue;
                    id = this.InsertBettingLimitChangedMsg();
                }
            }

            return message;
        }

        ////End Setting Changed Message

        private string CreatePopup(long id, string content, int top, int left, int width, int height, string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                AnnouncementText_Template tpl = new AnnouncementText_Template();
                tpl.announcementCookiesId = Encryption.PearsonEncode(CurrentUserSession.UserId.ToString() + CurrentUserSession.SubAccId.ToString());
                tpl.popUpId = id.ToString();
                tpl.top = top.ToString();
                tpl.left = left.ToString();
                tpl.content = content;
                if (UserSession.Current.Language == "th-TH")
                {
                    tpl.toptitlepopup = "toptitlepopup_TH";
                }
                else if (UserSession.Current.Language == "zh-TW" || UserSession.Current.Language == "zh-CN")
                {
                    tpl.toptitlepopup = "toptitlepopup_TW";
                }
                else if (UserSession.Current.Language == "ko-KR")
                {
                    tpl.toptitlepopup = "toptitlepopup_KO";
                }
                else
                {
                    tpl.toptitlepopup = "toptitlepopup";
                }

                return tpl;
            }
            else
            {
                Announcement_Template tpl = new Announcement_Template();

                // Hide Do show not again
                // tpl.HiddenCheckBox = null;
                tpl.announcementCookiesId = Encryption.PearsonEncode(CurrentUserSession.UserId.ToString() + CurrentUserSession.SubAccId.ToString());
                tpl.popUpId = id.ToString();
                tpl.width = width.ToString();
                tpl.height = height.ToString();
                tpl.top = top.ToString();
                tpl.left = left.ToString();
                tpl.src = source;

                return tpl;
            }
        }

        // Announcement Message
        private string GetAnnouncementMsg(out string source, out long id, out int top, out int left, out int width, out int height)
        {
            string content = string.Empty;
            string path = string.Empty;
            source = string.Empty;
            id = top = left = width = height = 0;

            //if (Sunplus.Agent.Model.Entities.Permission.UserPermission.IsNeutral)
            //{
            //    if (CurrentUserSession.IsSuper)
            //    {
            //        path = Server.MapPath("Resources/PopupAdSuperNeutral.xml");
            //    }
            //    else if (CurrentUserSession.IsMaster)
            //    {
            //        path = Server.MapPath("Resources/PopupAdMasterNeutral.xml");
            //    }
            //    else
            //    {
            //        path = Server.MapPath("Resources/PopupAdAgentNeutral.xml");
            //    }
            //}
            //else
            //{
            //    if (CurrentUserSession.IsSuper)
            //    {
            //        path = Server.MapPath("Resources/PopupAdSuper.xml");
            //    }
            //    else if (CurrentUserSession.IsMaster)
            //    {
            //        path = Server.MapPath("Resources/PopupAdMaster.xml");
            //    }
            //    else
            //    {
            //        path = Server.MapPath("Resources/PopupAdAgent.xml");
            //    }
            //}

            //XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.Load(path);

            //string language = CurrentUserSession.Language;

            //XmlNodeList root = xmlDoc.SelectNodes("FileDB/PopupAnnouncement");
            //foreach (XmlNode node in root)
            //{
            //    string nodeIds = node.Attributes["id"].Value;
            //    long nodeId = 0;
            //    long.TryParse(nodeIds, out nodeId);
            //    nodeId = -nodeId;

            //    // Skip old nodes
            //    if (0 == nodeId || this.showedMessageIds.Contains(nodeId))
            //    {
            //        continue;
            //    }

            //    id = nodeId;

            //    int.TryParse(node.Attributes["top"].Value, out top);
            //    int.TryParse(node.Attributes["left"].Value, out left);
            //    int.TryParse(node.Attributes["width"].Value, out width);
            //    int.TryParse(node.Attributes["height"].Value, out height);

            //    // Choose one new node
            //    XmlNodeList choosedNodes = node.SelectNodes(language);
            //    if (choosedNodes.Count == 0)
            //    {
            //        choosedNodes = node.SelectNodes("en-US");
            //    }

            //    if (choosedNodes.Count != 0)
            //    {
            //        source = choosedNodes[0].Attributes["src"].Value;
            //        content = choosedNodes[0].InnerXml;
            //        break;
            //    }
            //}

            return content;
        }

        ////End Announcement Message
    }
}