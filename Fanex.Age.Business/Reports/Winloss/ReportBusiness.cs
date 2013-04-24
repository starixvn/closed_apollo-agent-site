// <copyright file="BusinessBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// <summary>
//  Base Besiness
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.01.10">Created</change>
// </history>
namespace Fanex.Age.Business.Report
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Age.Business;
    
    using Fanex.Age.DTO.Reports.Winloss;
    using Fanex.Caching;
    using Fanex.Data.DNAide;
    using Sunplus.Agent.Model.Entities;

    /// <summary>
    ///
    /// </summary>
    public class ReportBusiness : BusinessBase
    {
        private readonly ICache cache;

        public ReportBusiness()
            : this(null)
        {
        }

        public ReportBusiness(ICache cache)
        {
            this.cache = cache ?? CacheService.CreateInstance();
        }

        public DateTime BalanceUpDay
        {
            get
            {
                this.GetSpecialDays();
                return this.cache.Get<DateTime>("FANEX.AGE.BUSINESS.BALANCEUPDAY");
            }
        }

        public DateTime MoveBetsDay
        {
            get
            {
                this.GetSpecialDays();
                return this.cache.Get<DateTime>("FANEX.AGE.BUSINESS.MOVEBETSDAY");
            }
        }

        public IEnumerable<WinlossReport> GetChartWinlossAnalysis(int custId, DateTime fromDate, DateTime toDate, ICollection<KeyValuePair<string, bool>> filterParams)
        {
            IEnumerable<WinlossReport> source = this.cache.Get<IEnumerable<WinlossReport>>("FANEX.AGE.BUSINESS.CHARTS.WINLOSSANALYSIS" + custId.ToString());
            if (source == null)
            {
                try
                {
                    string settingKey = string.Empty;
                    Hashtable parameters = new Hashtable();
                    parameters.Add("agentid", custId);
                    parameters.Add("fromdate", fromDate);
                    parameters.Add("todate", toDate);
                    foreach (KeyValuePair<string, bool> pair in filterParams)
                    {
                        parameters.Add(pair.Key, pair.Value);
                    }

                    if (UserSession.Current.IsAgent)
                    {
                        settingKey = "Age_Chart_WinlossAnalysis_Ag";
                    }
                    else if (UserSession.Current.IsMaster)
                    {
                        settingKey = "Age_Chart_WinlossAnalysis_MAg";
                    }
                    else
                    {
                        settingKey = "Age_Chart_WinlossAnalysis_SAg";
                    }

                    source = DbHelper.Query<WinlossReport>(settingKey, parameters);
                    if (source.Count<WinlossReport>() > 1)
                    {
                        this.cache.Set<IEnumerable<WinlossReport>>("FANEX.AGE.BUSINESS.CHARTS.WINLOSSANALYSIS" + custId.ToString(), source, TimeSpan.FromMinutes(3.0));
                    }
                }
                catch (Exception exception)
                {
                    ProcessException(exception.ToString());
                }
            }

            return source;
        }

        private void GetSpecialDays()
        {
            if (!this.CheckSpecialDays())
            {
                DataTable table = DbHelper.ExecuteDataTable("Age_SelBalanceUpDay");
                this.cache.Set<DateTime>("FANEX.AGE.BUSINESS.BALANCEUPDAY", Convert.ToDateTime(table.Rows[0]["BalanceUpDay"]), DateTime.Now.AddHours(2.0));
                this.cache.Set<DateTime>("FANEX.AGE.BUSINESS.MOVEBETSDAY", Convert.ToDateTime(table.Rows[0]["MoveBetsDay"]), DateTime.Now.AddHours(2.0));
            }
        }

        private bool CheckSpecialDays()
        {
            if (!(this.cache.Contains("FANEX.AGE.BUSINESS.BALANCEUPDAY") && this.cache.Contains("FANEX.AGE.BUSINESS.MOVEBETSDAY")))
            {
                return false;
            }

            return true;
        }
    }
}