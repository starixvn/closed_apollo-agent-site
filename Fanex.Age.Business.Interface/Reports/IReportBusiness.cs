// <copyright file="IWinlossBusiness.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved. 
// </copyright>
//
// <summary>
//  Winloss Besiness Interface
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.01.10">Created</change>
// </history>

namespace Fanex.Age.Business.Interface.Reports
{
    using System;
    using System.Collections.Generic;
    using Fanex.Age.DTO.Reports.Winloss;

    public interface IReportBusiness
    {
        #region Charting
        IEnumerable<WinlossReport> GetChartWinlossAnalysis(int custId, DateTime fromDate, DateTime toDate, 
            ICollection<KeyValuePair<string, bool>> filterParams);
        #endregion

        #region Utilities
        DateTime BalanceUpDay { get; }
        DateTime MoveBetsDay { get; }
        #endregion
    }
}
