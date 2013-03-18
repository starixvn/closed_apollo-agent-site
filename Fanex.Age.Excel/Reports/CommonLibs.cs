using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using Fanex.Age.DTO;
using Fanex.Age.Utility;

/*
 * Created 20120220@Leo
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Fanex.Age.Excel.Reports
{
    public class CommonLibs
    {
        public static string GetHeaderTitle(eReportType reportType)
        {
            string title = string.Empty;
            switch (reportType)
            {
                case eReportType.Daily:
                    title = Resources.Lang.dailywinlossanalysisreport;
                    break;
                case eReportType.Monthly:
                    title = Resources.Lang.monthlywinlossanalysisreport;
                    break;
                case eReportType.Sport:
                    title = Resources.Lang.sportwinlossanalysisreport;
                    break;
                default:
                    title = "Unknown";
                    break;
            }
            return title;
        }

        public static string GetBreakDownTitle(eReportType reportType)
        {
            string title = string.Empty;
            switch (reportType)
            {
                case eReportType.Daily:
                    title = Resources.Lang.daily;
                    break;
                case eReportType.Monthly:
                    title = Resources.Lang.monthly;
                    break;
                case eReportType.Sport:
                    title = Resources.Lang.sport;
                    break;
                default:
                    title = "Unknown";
                    break;
            }
            return title;
        }

        public static string GetBreakDownText(int breakDown, eReportType reportType)
        {
            string text = string.Empty;
            switch (reportType)
            {
                case eReportType.Daily:
                    DateTime datetime = breakDown.ToDateTime(eConvertType.YYYYMMDD);
                    text = datetime.ToString("M/d/yyyy");
                    break;
                case eReportType.Monthly:
                    datetime = breakDown.ToDateTime(eConvertType.YYYYMM);
                    text = datetime.ToString("M/yyyy");
                    break;
                case eReportType.Sport:
                    string resourceName = GetResourceNameBySportId(breakDown);
                    if (!string.IsNullOrEmpty(resourceName))
                        text = Resources.Lang.ResourceManager.GetString(resourceName);
                    break;
                default:
                    text = "Unknown";
                    break;
            }
            return text;
        }

        public static string GetResourceNameBySportId(int sportId)
        {
            string resourceName = string.Empty;
            switch (sportId)
            {
                case 0:
                    resourceName = "sportBook";
                    break;
                case 251:
                    resourceName = "casino";
                    break;
                case 151:
                    resourceName = "Racebook";
                    break;
                case 161:
                    resourceName = "numbergame";
                    break;
                case 252:
                    resourceName = "bingo";
                    break;
                case 253:
                    resourceName = "P2P";
                    break;
                case 162:
                    resourceName = "LiveCasino";
                    break;
            }
            return resourceName;
        }
    }
}
