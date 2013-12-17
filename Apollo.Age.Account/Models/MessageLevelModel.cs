// --------------------------------------------------------------------------
// <copyright file="MessageLevelModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.12.16">Created</change>
// </history>

namespace Apollo.Age.Account.Models
{
    using System;

    /// <summary>
    /// Message model.
    /// </summary>
    public class MessageLevelModel
    {
        public string Content_en { get; set; }

        public string Content_cs { get; set; }

        public string Content_ch { get; set; }

        public string Content_jp { get; set; }

        public string Content_th { get; set; }

        public string Content_it { get; set; }

        public string Content_ko { get; set; }

        public string Content_es { get; set; }

        public string Content_vn { get; set; }

        public string Content_id { get; set; }

        public string Content_el { get; set; }

        public string Content_mx { get; set; }

        public int MessageId { get; set; }

        public DateTime? PostTime { get; set; }

        /// <summary>
        /// Get content.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns>Language content.</returns>
        public string GetContent(string language = "")
        {
            string content = string.Empty;

            switch (language)
            {
                case "en-US":
                    content = this.Content_en;
                    break;

                case "th-TH":
                    content = this.Content_th;
                    break;

                case "zh-CN":
                    content = this.Content_cs;
                    break;

                case "zh-TW":
                    content = this.Content_ch;
                    if (string.IsNullOrEmpty(content))
                    {
                        content = this.Content_cs;
                    }

                    break;

                case "ja-JP":
                    content = this.Content_jp;
                    break;

                case "ko-KR":
                    content = this.Content_ko;
                    break;
            }

            if (string.IsNullOrEmpty(content))
            {
                content = this.Content_en;
            }

            content = content != null ? content : string.Empty;

            return content
                .Replace("\n", string.Empty)
                .Replace("\r", string.Empty)
                .Replace("'", "\'") + string.Empty;
        }
    }
}