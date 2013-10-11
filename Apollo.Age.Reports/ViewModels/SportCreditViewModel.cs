// --------------------------------------------------------------------------
// <copyright file="SportCreditViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.7.31">Created</change>
// </history>

namespace Apollo.Age.Reports.ViewModels
{
    using System.Collections.Generic;
    using Apollo.Age.Reports.Models;

    /// <summary>
    /// Sport credit view model class.
    /// </summary>
    public class SportCreditViewModel
    {
        public string Username { get; set; }

        public List<SportCreditModel> SportCreditModelList { get; set; }
    }
}
