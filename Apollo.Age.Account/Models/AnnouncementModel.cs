// --------------------------------------------------------------------------
// <copyright file="AnnouncementModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.4">Created</change>
//   <change who="Anders.Le" date="2013.4.25">Move all function of ReflectionUtil</change>
// </history>

namespace Apollo.Age.Account.Models
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Announcement model.
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

        public string ContentVN { get; set; }

        public string ContentJP { get; set; }

        public string ContentKO { get; set; }

        public string ContentTH { get; set; }

        public string GetMessageType
        {
            get
            {
                var typeName = string.Empty;

                if (MsgType == 1 && !IsSticky)
                {
                    typeName = "private";
                }
                else if ((MsgType == 1 || MsgType == 3) && IsSticky)
                {
                    typeName = "special";
                }
                else if (MsgType == 3 && !IsSticky)
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

        /// <summary>
        /// Get full content.
        /// </summary>
        /// <param name="userLang">The user language.</param>
        /// <returns>Full content string.</returns>
        public string GetFullContent(string userLang)
        {
            string lang = GetLang(userLang, "Content").Replace("_", string.Empty);
            string content = GetContent(userLang, lang);

            if (GetMessageType == "private")
            {
                content = content.Replace("@@", this.BetTransId);
            }

            return content;
        }

        /// <summary>
        /// Get small content.
        /// </summary>
        /// <param name="userLang">The user language.</param>
        /// <returns>Small content string.</returns>
        public string GetSmallContent(string userLang)
        {
            int maxLength = this.IsRead ? 100 : 80;

            return GetSmallContent(GetFullContent(userLang), maxLength);
        }

        /// <summary>
        /// Get content.
        /// </summary>
        /// <param name="userLanguage">The user language.</param>
        /// <param name="langName">The language name.</param>
        /// <returns>Content string.</returns>
        private string GetContent(string userLanguage, string langName)
        {
            string content = string.Empty;
            object value = GetValue(this, langName);

            if ((value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString()))
                && !userLanguage.Contains("zh"))
            {
                langName = "Contenten";
            }

            if (userLanguage.Contains("zh"))
            {
                var contentCS = this.ContentCS;
                var contentCH = this.ContentCH;

                if (string.IsNullOrWhiteSpace(this.ContentCS) && string.IsNullOrWhiteSpace(this.ContentCH))
                {
                    langName = "Contenten";
                }
                else
                {
                    if (userLanguage.Equals("zh-CN"))
                    {
                        value = GetValue(this, langName);
                        if (value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString()))
                        {
                            langName = "Contentch";
                        }
                    }
                    else
                    {
                        value = GetValue(this, langName);
                        if (value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString()))
                        {
                            langName = "Contentcs";
                        }
                    }
                }
            }

            value = GetValue(this, langName);
            if (value != null || value != DBNull.Value)
            {
                content = value.ToString();
            }

            return content;
        }

        /// <summary>
        /// Get language.
        /// </summary>
        /// <param name="userLang">The user language.</param>
        /// <param name="original">The original string.</param>
        /// <returns>Language string.</returns>
        private string GetLang(string userLang, string original)
        {
            var ret = "_";
            switch (userLang)
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

        /// <summary>
        /// Get small content.
        /// </summary>
        /// <param name="fullContent">The full content.</param>
        /// <param name="maxLength">The max length.</param>
        /// <returns>Small content string.</returns>
        private string GetSmallContent(string fullContent, int maxLength)
        {
            int length = Math.Min(maxLength, fullContent.Length);

            return fullContent.Substring(0, length) + " ...";
        }

        /// <summary>
        /// Get property value.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>Property value.</returns>
        private object GetValue(object obj, string propertyName)
        {
            var propertyInfo = GetPropertyInfo(obj, propertyName);
            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(obj, null);
            }

            return null;
        }

        /// <summary>
        /// Get property information.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>Property information.</returns>
        private PropertyInfo GetPropertyInfo(object obj, string propertyName)
        {
            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
            {
                if (string.Compare(propertyInfo.Name, propertyName, true) == 0)
                {
                    return propertyInfo;
                }
            }

            return null;
        }

        /// <summary>
        /// Check if has property information.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>True : has, false : not have.</returns>
        private bool HasPropertyInfo(object obj, string propertyName)
        {
            var propertyInfo = GetPropertyInfo(obj, propertyName);

            return propertyInfo != null;
        }
    }
}