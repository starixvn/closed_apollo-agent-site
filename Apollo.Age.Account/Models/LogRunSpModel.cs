// --------------------------------------------------------------------------
// <copyright file="LogRunSpModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.5.16">Created</change>
// </history>

namespace Apollo.Age.Account.Models
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class LogRunSpModel
    {
        public long LogId { get; set; }

        public DateTime LogDate { get; set; }

        public string Username { get; set; }

        public string AdminUsername { get; set; }

        public int ActionTypeId { get; set; }

        public string ActionName { get; set; }

        public string OldData { get; set; }

        public string Data { get; set; }
    }
}