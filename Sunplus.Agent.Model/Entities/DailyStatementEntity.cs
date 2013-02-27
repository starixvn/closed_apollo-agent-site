namespace Sunplus.Agent.Model.Entities
{
    using System;
    using System.Data;
    using Sunplus.Agent.Model.Entities.Base;

    public class DailyStatementEntity : IEntity    
    {
        public DailyStatementEntity() 
        {
        }

        public DailyStatementEntity(DataRow dataRow)            
        {
            this.WinlostDate = (DateTime)dataRow["winlostdate"];
            this.TransType = dataRow["transtype"] as string;
            this.Transdesc = dataRow["transdesc"] as string;
            this.Turnover = (decimal)dataRow["turnover"];
            this.TotalAmt = (decimal)dataRow["totalAmt"];
            this.Commission = (decimal)dataRow["commission"];
        }

        public DateTime WinlostDate { get; set; }

        public string TransType { get; set; }

        public string Transdesc { get; set; }

        public decimal Turnover { get; set; }

        public decimal TotalAmt { get; set; }

        public decimal Commission { get; set; }
    }
}
