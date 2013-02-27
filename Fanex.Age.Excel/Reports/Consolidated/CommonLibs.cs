using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Age.DTO;
using System.Resources;

namespace Fanex.Age.Excel.Reports.Consolidated
{
    public class CommonLibs
    {
        public static string GetHeaderTitle(eReportType reportType)
        {
            string title = "";
            switch (reportType)
            {
                case eReportType.Daily:
                    title = Resources.Lang.dailyconsolidatedreport;
                    break;
                case eReportType.Monthly:
                    title = Resources.Lang.monthlyconsolidatedreport;
                    break;
                case eReportType.Sport:
                    title = Resources.Lang.sportconsolidatedreport;
                    break;
                default:
                    title = "Unknown";
                    break;
            }
            return title;
        }

        public static string GetHeaderCurrency(int currencyId)
        {
            string result = "";
            switch (currencyId)
            {
                case 0:
                    result = Resources.Lang.allcurrencies;
                    break;
                case -2:
                case -3:
                    result = Resources.Lang.baseon + " " + RemotingRef.RefData.GetExchangeData(currencyId * -1).Currency;
                    break;
                default:
                    result = Resources.Lang.viewunder + " " + RemotingRef.RefData.GetExchangeData(currencyId).Currency;
                    break;
            }
            return result;
        }
    }
}
