namespace Fanex.Age.Business.Reports.PortalPage
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Age.DTO.Reports.PortalPage;

    public class AnnouncementBusiness
    {
        public IList<AnnouncementItem> ToList(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }

            var result = (from c in dt.AsEnumerable()
                          select new AnnouncementItem
                          {
                              Bettransid = c.Field<string>("Bettransid"),
                              Content_ch = c.Field<string>("Content_ch"),
                              Content_cs = c.Field<string>("Content_cs"),
                              Content_en = c.Field<string>("Content_en"),
                              Content_es = c.Field<string>("Content_es"),
                              Content_it = c.Field<string>("Content_it"),
                              Content_jp = c.Field<string>("Content_jp"),
                              Content_ko = c.Field<string>("Content_ko"),
                              Content_th = c.Field<string>("Content_th"),
                              IsRead = c.Field<bool>("IsRead"),
                              IsSticky = c.Field<bool>("IsSticky"),
                              LastUpdateTime = c.Field<DateTime>("LastUpdateTime"),
                              MessageId = c.Field<int>("MessageId"),
                              MsgType = c.Field<byte>("MsgType"),
                              PostTime = c.Field<DateTime>("PostTime")
                          }).ToList();

            return result;
        }
    }
}