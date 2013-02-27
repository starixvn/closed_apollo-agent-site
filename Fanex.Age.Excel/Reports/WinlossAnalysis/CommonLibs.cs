namespace Fanex.Age.Excel.Reports.WinlossAnalysis
{
    using Fanex.Age.DTO;

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
    }
}