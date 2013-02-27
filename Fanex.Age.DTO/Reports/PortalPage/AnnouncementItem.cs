namespace Fanex.Age.DTO.Reports.PortalPage
{
    using System;
    using System.Collections.Generic;

    public class AnnouncementItem
    {
        public string Bettransid { get; set; }

        public string Content_ch { get; set; }

        public string Content_cs { get; set; }

        public string Content_en { get; set; }

        public string Content_es { get; set; }

        public string Content_it { get; set; }

        public string Content_jp { get; set; }

        public string Content_ko { get; set; }

        public string Content_th { get; set; }

        public bool IsRead { get; set; }

        public bool IsSticky { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public int MessageId { get; set; }

        public byte MsgType { get; set; }

        public DateTime PostTime { get; set; }
    }
}