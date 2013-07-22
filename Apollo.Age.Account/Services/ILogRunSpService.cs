// --------------------------------------------------------------------------
// <copyright file="ILogRunSpService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.5.16">Created</change>
// </history>

namespace Apollo.Age.Account.Services
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Apollo.Age.Account.Criterias;
    using Apollo.Age.Account.Models;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface ILogRunSpService
    {
        IEnumerable<LogRunSpModel> Search(ref GetLogCustSettingCriteria criteria);

        string RevertCommGroupName(string group);
    }
}
