// --------------------------------------------------------------------------
// <copyright file="SportCreditService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.7.31">Created</change>
// </history>

namespace Apollo.Age.Reports.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Apollo.Age.Reports.Criterias;
    using Apollo.Age.Reports.Models;

    /// <summary>
    /// Service for SportCredit.
    /// </summary>
    public class SportCreditService : ReportService, ISportCreditService
    {
        /// <summary>
        /// Get Sport Credits list.
        /// </summary>
        /// <param name="criteria">Input SportCreditCriteria.</param>
        /// <returns>List SportCreditModel.</returns>
        public List<SportCreditModel> GetSportCredits(SportCreditCriteria criteria)
        {
            var sportCreditList = this.GetData<SportCreditModel>(criteria).ToList();

            return sportCreditList;
        }
    }
}
