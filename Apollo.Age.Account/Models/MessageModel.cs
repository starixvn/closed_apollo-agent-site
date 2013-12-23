// --------------------------------------------------------------------------
// <copyright file="MessageModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.4.9">Created</change>
// </history>

namespace Apollo.Age.Account.Models
{
    using System;
    using Apollo.Age.Account.Core;

    /// <summary>
    /// Message model.
    /// </summary>
    public class MessageModel
    {
        public int MessageId { get; set; }

        public string ContentEN { get; set; }

        public string ContentCS { get; set; }

        public string ContentCH { get; set; }

        public string ContentJP { get; set; }

        public string ContentTH { get; set; }

        public string ContentIT { get; set; }

        public string ContentKO { get; set; }

        public string ContentES { get; set; }

        public string ContentVN { get; set; }

        public string ContentID { get; set; }

        public string ContentEL { get; set; }

        public string ContentMX { get; set; }

        public DateTime PostTime { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public byte MsgType { get; set; }

        public bool IsSticky { get; set; }

        public bool IsTest { get; set; }

        public int MatchId { get; set; }

        public bool IsBatch { get; set; }

        public int Creator { get; set; }

        public int LastUpdator { get; set; }

        public string CreatorName { get; set; }

        public string LastUpdateName { get; set; }

        public string UserName { get; set; }

        public string BettransID { get; set; }

        public int TargetId { get; set; }

        /// <summary>
        /// Get content.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns>Language content.</returns>
        public string GetContent(string language = "")
        {
            string content = string.Empty;
            string betTransId = this.BettransID;

            switch (language)
            {
                case "en-US":
                    content = this.ContentEN;
                    break;

                case "th-TH":
                    content = this.ContentTH;
                    break;

                case "zh-CN":
                    content = this.ContentCS;
                    break;

                case "zh-TW":
                    content = this.ContentCH;
                    if (string.IsNullOrEmpty(content))
                    {
                        content = this.ContentCS;
                    }

                    break;

                case "ja-JP":
                    content = this.ContentJP;
                    break;

                case "ko-KR":
                    content = this.ContentKO;
                    break;
            }

            if (string.IsNullOrEmpty(content))
            {
                content = this.ContentEN;
            }

            content = content != null ? content : string.Empty;

            if (this.MsgType == 1) // Private message.
            {
                content = content.Replace("@@", betTransId == null ? string.Empty : betTransId);
            }

            return content
                .Replace("\n", string.Empty)
                .Replace("\r", string.Empty)
                .Replace("'", "\'") + string.Empty;
        }
    }
}