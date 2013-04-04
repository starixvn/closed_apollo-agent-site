// --------------------------------------------------------------------------
// <copyright file="Announcement.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.4">Created</change>
// </history>


namespace Apollo.Age.Account.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Age.Utility;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AnnouncementViewModel
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

        public string GetContent(string userLanguage, string langName)
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
    }
}
