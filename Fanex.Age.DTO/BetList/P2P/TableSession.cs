/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.DTO.BetList.P2P
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class TableSession
    {
        public TableSession()
        {
            this.SessionItems = new List<SessionItem>();
        }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public IList<SessionItem> SessionItems { get; set; }
    }
}