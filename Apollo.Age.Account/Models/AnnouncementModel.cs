// --------------------------------------------------------------------------
// <copyright file="Announcement.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.4">Created</change>
// </history>


namespace Apollo.Age.Account.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Age.Utility;
    using Sunplus.Agent.Model.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AnnouncementModel
    {
        public string BetTransId { get; set; }

        public bool IsRead { get; set; }

        public bool IsSticky { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int MessageId { get; set; }

        public byte MsgType { get; set; }

        public DateTime PostTime { get; set; }

        public string ContentCH { get; set; }

        public string ContentCS { get; set; }

        public string ContentEN { get; set; }

        public string ContentES { get; set; }

        public string ContentIT { get; set; }

        public string ContentJP { get; set; }

        public string ContentKO { get; set; }

        public string ContentTH { get; set; }

        public string GetMessageType
        {
            get
            {
                var typeName = string.Empty;

                if (MsgType == 1 && IsSticky != true)
                {
                    typeName = "private";
                }
                else if ((MsgType == 1 || MsgType == 3) && IsSticky == true)
                {
                    typeName = "special";
                }
                else if (MsgType == 3 && IsSticky == false)
                {
                    typeName = "public";
                }
                else
                {
                    typeName = "--";
                }

                return typeName;
            }
        }

        public string FullContent
        {
            get
            {
                string lang = GetLang("Content").Replace("_", string.Empty);
                string content = GetContent(UserSession.Current.Language, lang);

                if (GetMessageType == "private")
                {
                    if (this.BetTransId != null)
                    {
                        content = content.Replace("@@", this.BetTransId);
                    }
                }

                return content;
            }
        }

        public string SmallContent
        {
            get
            {
                int maxLength = this.IsRead ? 100 : 80;
                return GetSmallContent(FullContent, maxLength);
            }
        }

        private string GetContent(string userLanguage, string langName)
        {
            string content = string.Empty;
            object value = ReflectionUtil.GetValue(this, langName);
            if ((value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString())) && !userLanguage.Contains("zh"))
            {
                langName = "Content_en";
            }

            if (userLanguage.Contains("zh"))
            {
                var Content_cs = this.ContentCS;
                var Content_ch = this.ContentCH;

                if (string.IsNullOrWhiteSpace(this.ContentCS) && string.IsNullOrWhiteSpace(this.ContentCH))
                {
                    langName = "Content_en";
                }
                else
                {
                    if (userLanguage.Equals("zh-CN"))
                    {
                        value = ReflectionUtil.GetValue(this, langName);
                        if (value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString()))
                        {
                            langName = "Content_ch";
                        }
                    }
                    else
                    {
                        value = ReflectionUtil.GetValue(this, langName);
                        if (value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString()))
                        {
                            langName = "Content_cs";
                        }
                    }
                }
            }

            value = ReflectionUtil.GetValue(this, langName);
            if (value != null || value != DBNull.Value)
            {
                content = value.ToString();
            }


            return content;
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

        private string GetSmallContent(string fullContent, int maxLength)
        {
            int length = Math.Min(maxLength, fullContent.Length);
            return fullContent.Substring(0, length) + " ...";
        }
    }
}
