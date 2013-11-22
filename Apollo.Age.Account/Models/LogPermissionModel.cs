// --------------------------------------------------------------------------
// <copyright file="LogPermissionModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.7">Created</change>
// </history>

namespace Apollo.Age.Account.Models
{
    using Apollo.Age.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LogPermissionModel
    {
        public Function Function { get; set; }

        public byte? Policy { get; set; }

        public byte OldPolicy { get; set; }
    }
}
