// --------------------------------------------------------------------------
// <copyright file="ReportMenuViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.10">Created</change>
// </history>

namespace Apollo.Age.Reports.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Entities;
    using Apollo.Age.Reports.Models;
    using Apollo.Age.PermissionControl.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ReportMenuViewModel
    {
        public List<SingbetSport> SportList { get; set; }

        public List<FunctionPolicy> FunctionPolicyList { get; set; }

        public string ReportName { get; set; }

        public string SportType { get; set; }

        public string SportDropDownControl { get; set; }

        public string SportDropDownClassName { get; set; }
    }
}
