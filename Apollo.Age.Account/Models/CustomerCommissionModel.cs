// --------------------------------------------------------------------------
// <copyright file="CustomerCommissionModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2013.4.18">Created</change>
// </history>

namespace Apollo.Age.Account.Models
{
    /// <summary>
    /// Customer commission model.
    /// </summary>
    public class CustomerCommissionModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public decimal DiscountA { get; set; }

        public decimal DiscountB { get; set; }

        public decimal DiscountC { get; set; }

        public decimal DiscountD { get; set; }

        public decimal Discount1x2 { get; set; }

        public decimal DiscountOthers { get; set; }
    }
}