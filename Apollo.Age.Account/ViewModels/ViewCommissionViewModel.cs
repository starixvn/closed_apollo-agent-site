// --------------------------------------------------------------------------
// <copyright file="ViewCommissionViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2013.4.18">Created</change>
// </history>

namespace Apollo.Age.Account.ViewModels
{
    using System.Collections.Generic;
    using Apollo.Age.Account.Models;

    /// <summary>
    /// View commission view model.
    /// </summary>
    public class ViewCommissionViewModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Group { get; set; }

        public bool IsMember { get; set; }

        public IEnumerable<CustomerCommissionModel> CommissionList { get; set; }
    }
}