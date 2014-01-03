// --------------------------------------------------------------------------
// <copyright file="CommissionItem.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.1">Created</change>
// </history>

namespace Apollo.CustSetting.ViewData
{
    /// <summary>
    /// Class name.
    /// </summary>
    public class CommissionItem
    {
        public string Group { get; set; }

        public string Name { get; set; }

        public int SportId { get; set; }

        public int BetTypeId { get; set; }

        public decimal Discount { get; set; }

        public decimal DiscountA { get; set; }

        public decimal DiscountB { get; set; }

        public decimal DiscountC { get; set; }

        public decimal DiscountD { get; set; }

        public CommissionItem Upline { get; set; }
    }
}