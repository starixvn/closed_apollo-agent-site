// --------------------------------------------------------------------------
// <copyright file="ISportCreditService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.7.31">Created</change>
// </history>

namespace Apollo.Age.Reports.Services
{
    using System.Collections.Generic;
    using Apollo.Age.Reports.Criterias;
    using Apollo.Age.Reports.Models;

    public interface ISportCreditService
    {
        List<SportCreditModel> GetSportCredits(SportCreditCriteria criteria);
    }
}
