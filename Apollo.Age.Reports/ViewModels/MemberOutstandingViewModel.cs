// --------------------------------------------------------------------------
// <copyright file="MemberOutstandingViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.7.12">Created</change>
// </history>

namespace Apollo.Age.Reports.ViewModels
{
    using System.Collections.Generic;
    using Apollo.Age.Reports.Models;

    /// <summary>
    /// Member outstanding view model class.
    /// </summary>
    public class MemberOutstandingViewModel
    {
        public IEnumerable<OutstandingModel> ListOutstanding { get; set; }

        public OutstandingModel Total { get; set; }

        public int MainShareHolderId { get; set; }

        public string MainShareHolderUsername { get; set; }

        public bool HasPermissionDownlineDetail { get; set; }
    }
}