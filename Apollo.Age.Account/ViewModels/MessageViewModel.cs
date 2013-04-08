using System;

namespace Apollo.Age.Account.ViewModels
{
    public class MessageViewModel
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
    }
}