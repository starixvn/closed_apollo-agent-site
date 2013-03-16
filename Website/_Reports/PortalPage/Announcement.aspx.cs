namespace Fanex.Age.UI.Reports.PortalPage
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using Fanex.Age.Business.Reports.PortalPage;
    using Fanex.Age.DTO.Reports.PortalPage;
    using Fanex.Age.Utility;
    using Resources;
    using Sunplus.Agent.Model.BL.UserMessage;
    using Sunplus.Agent.Model.Entities;
  

    public partial class Announcement : TplPage
    {
        private readonly DateTime MaxDateTime = new DateTime(9999, 12, 31);
        private const int TotalMessage = 10;
        private DateTime _lastPostTime = DateTime.Now;
        private bool _moreIsVisible = false;
        private UserMessageBO message = null;
        private int uid = 0;
        private string currency = null;

        public override void RenderContent(HtmlTextWriter writer)
        {
            var tpl = new Announcement_Template();
            var dt = View as DataTable;

            if (IsFirstRun())
            {
                tpl.Title.Announcement = Lang.announcement.ToUpper();
            }
            else
            {
                tpl.Title.Hide();
            }

            var business = new AnnouncementBusiness();
            var items = business.ToList(dt);
            DateTime newLastPostTime = new DateTime(1900, 1, 1);

            if (items.Count > 0)
            {
                tpl.NoRow.Clear();

                var index = 0;
                var ids = string.Empty;
                var msgIds = string.Empty;

                var datesQuery = items.Select(p => p.PostTime.Date).Distinct().OrderByDescending(p => p);
                tpl.GroupDate.Clear();
                foreach (var date in datesQuery)
                {
                    var groupDateBlock = new Announcement_GroupDate_Block();
                    if (date == _lastPostTime.Date)
                    {
                        groupDateBlock.GroupTitle.Hide();
                    }
                    else
                    {
                        groupDateBlock.GroupTitle.GroupDate = date.ToShortDateString();
                    }
                    groupDateBlock.DataRow.Clear();

                    var dateItemsQuery = items.Where(p => p.PostTime.Date == date).OrderByDescending(p => p.PostTime);
                    foreach (var item in dateItemsQuery)
                    {
                        index++;
                        var row = new Announcement_GroupDate_DataRow_Block();
                        row.oddEvenRow = index % 2 == 0 ? "evenRow" : "oddRow";
                        row.Announment_Id = item.MessageId.ToString();
                        row.Date = item.PostTime.ToLongTimeString();
                        var typeName = this.GetMessageType(item.MsgType, item.IsSticky);

                        if (typeName == "private" && !CurrentUserSession.IsInternal && !item.IsRead)
                        {
                            ids += item.MessageId.ToString() + ",";
                        }

                        row.msgType = typeName;
                        row.IsRead = "1";
                        int maxLength = 100;
                        if (item.IsRead == false)
                        {
                            maxLength = 80;
                            row.IsRead = "0";
                            row.msgType = "private_unread";
                        }

                        var lang = this.GetLang("Content");
                        string content = GetContent(item, lang);
                        if (typeName == "private")
                        {
                            if (item.Bettransid != null)
                            {
                                content = content.Replace("@@", item.Bettransid);
                            }
                        }
                        row.FullContent = content;

                        string smallContent = GetSmallContent(content, maxLength);
                        row.SmallContent = smallContent;

                        groupDateBlock.DataRow.Append(row);

                        newLastPostTime = item.PostTime;
                    }

                    tpl.GroupDate.Append(groupDateBlock);
                }
            }
            else
            {
                _lastPostTime = new DateTime(1900, 1, 1);
                tpl.GroupDate.Hide();
                tpl.NoRow.NoRowInfo = Lang.noInfo;
            }

            if (items.Count == TotalMessage)
            {
                _moreIsVisible = true;
                if (!IsFirstRun())
                {
                    tpl.MoreFooter.Hide();
                }
            }
            else
            {
                tpl.MoreFooter.Hide();
            }

            _lastPostTime = newLastPostTime;

            Response.SetCookie(new System.Web.HttpCookie("lastPostTime", _lastPostTime.ToString()));
            Response.SetCookie(new System.Web.HttpCookie("moreIsVisible", _moreIsVisible.ToString()));

            writer.Write(tpl);
        }

        private string GetSmallContent(string fullContent, int maxLength)
        {
            int length = Math.Min(maxLength, fullContent.Length);
            return fullContent.Substring(0, length) + " ...";
        }

        private bool IsFirstRun()
        {
            if (_lastPostTime == MaxDateTime)
                return true;
            return false;
        }

        private string GetContent(AnnouncementItem item, string langName)
        {
            string result = "";
            object value = ReflectionUtil.GetValue(item, langName);
            if ((value == null || value == DBNull.Value || string.IsNullOrEmpty(value.ToString())) && !CurrentUserSession.Language.Contains("zh"))
            {
                langName = "Content_en";
            }

            if (CurrentUserSession.Language.Contains("zh"))
            {
                var Content_cs = ReflectionUtil.GetValue(item, "Content_cs");
                var Content_ch = ReflectionUtil.GetValue(item, "Content_ch");
                if ((Content_cs == null || Content_cs == DBNull.Value || string.IsNullOrEmpty(Content_cs.ToString())) &&
                    (Content_ch == null || Content_ch == DBNull.Value || string.IsNullOrEmpty(Content_ch.ToString())))
                {
                    langName = "Content_en";
                }
                else
                {
                    if (CurrentUserSession.Language.Equals("zh-CN"))
                    {
                        value = ReflectionUtil.GetValue(item, langName);
                        if (value == null || value == DBNull.Value || string.IsNullOrEmpty(value.ToString()))
                        {
                            langName = "Content_ch";
                        }
                    }
                    else
                    {
                        value = ReflectionUtil.GetValue(item, langName);
                        if (value == null || value == DBNull.Value || string.IsNullOrEmpty(value.ToString()))
                        {
                            langName = "Content_cs";
                        }
                    }
                }
            }

            value = ReflectionUtil.GetValue(item, langName);
            if (value != null || value != DBNull.Value)
            {
                result = value.ToString();
            }
            return result;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            try
            {
                DataTable dt = message.GetMessages(uid, this.GetParentSites1(this.GetTargetID().ToString()), _lastPostTime, TotalMessage, false);
                return dt;
            }
            catch (Exception ex)
            {
                Fanex.Age.Utility.Logs.Error("Can't get data", ex);
                return null;
            }
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            _lastPostTime = Convert.ToDateTime(GetStringRequest("lastPostTime", MaxDateTime.ToShortDateString()));

            if (this.message == null)
                this.message = new UserMessageBO(UserSession.Current.NetSessionId);

            if (this.uid == 0)
                this.uid = UserSession.Current.CustId;

            if (this.currency == null)
                this.currency = UserSession.Current.Currency + " ";
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        private string GetMessageType(int msgType, bool IsSticky)
        {
            var typeName = string.Empty;

            if (msgType == 1 && IsSticky != true)
            {
                typeName = "private";
            }
            else if ((msgType == 1 || msgType == 3) && IsSticky == true)
            {
                typeName = "special";
            }
            else if (msgType == 3 && IsSticky == false)
            {
                typeName = "public";
            }
            else
            {
                typeName = "--";
            }

            return typeName;
        }

        private string GetLang(string original)
        {
            var tmp = UserSession.Current.Language;
            var ret = "_";

            switch (tmp)
            {
                case "zh-CN":
                    ret += "cs";
                    break;
                case "zh-TW":
                    ret += "ch";
                    break;
                case "th-TH":
                    ret += "th";
                    break;
                case "ja-JP":
                    ret += "jp";
                    break;
                case "ko-KR":
                    ret += "ko";
                    break;
                default:
                    ret += "en";
                    break;
            }

            return original + ret;
        }

        private int GetTargetID()
        {
            int siteid = 3000;
            if (Sunplus.Agent.Model.Entities.UserSession.Current.RoleId == 4)
            {
                siteid = 3100;
            }
            else if (Sunplus.Agent.Model.Entities.UserSession.Current.RoleId == 3)
            {
                siteid = 3200;
            }
            else
            {
                siteid = 3300;
            }

            return siteid;
        }

        private string GetParentSites1(string SiteId)
        {
            ArrayList list = new ArrayList();
            int num = 0;
            for (int i = SiteId.Length - 1; i >= 0; i--)
            {
                if (SiteId[i] != '0')
                {
                    num = i;
                    break;
                }
            }
            for (int j = 1; j <= num; j++)
            {
                string str = "";
                for (int m = j; m < SiteId.Length; m++)
                {
                    str = str + "0";
                }

                list.Add(SiteId.Remove(j) + str);
            }
            string str2 = "";
            for (int k = 0; k < list.Count; k++)
            {
                str2 = str2 + list[k] + ",";
            }

            return (str2 + SiteId);
        }

        private bool HasPmAt1stPage(IEnumerable<DataRow> rows, out string msgIds)
        {
            msgIds = string.Empty;
            var ret = false;

            foreach (DataRow dr in rows)
            {
                var typeName = this.GetMessageType(Convert.ToInt32(dr["MsgType"]), Convert.ToBoolean(dr["IsSticky"]));
                var isRead = Convert.ToBoolean(dr["isRead"]);

                if (typeName == "private" && !isRead)
                {
                    msgIds += dr["MessageId"].ToString() + ",";
                    ret = true;
                }
            }

            msgIds = msgIds != string.Empty ? msgIds.Substring(0, msgIds.Length - 1) : msgIds;

            return ret;
        }
    }
}