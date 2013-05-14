// --------------------------------------------------------------------------
// <copyright file="BetListMetaModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.05.14">Bet List MetaModel</change>
// </history>

namespace Apollo.Age.BetList.MetaModel
{
    using System;

    public class BetListMetaModel
    {
        public bool IsExportExcel { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public DateTime FromDate { get; set; }

        public string FromDateString
        {
            get
            {
                return FromDate.ToString();
            }
        }

        public DateTime ToDate { get; set; }

        public string ToDateString
        {
            get
            {
                return ToDate.ToString();
            }
        }

        public long TransId { get; set; }

        public string Type { get; set; }

        public BetListMetaModel()
        {
            this.PageIndex = 1;
            this.PageSize = int.MaxValue - 1;
        }
    }
}