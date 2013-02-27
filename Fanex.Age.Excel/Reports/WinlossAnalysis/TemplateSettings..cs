/*
 * Created 20120220@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.Excel.Reports.WinlossAnalysis
{
    using System.Web;

    internal class TemplateSettings
    {
        private string dirPath;

        public TemplateSettings()
        {
            this.dirPath = HttpContext.Current.Server.MapPath("~") + "\\ExcelTemplates\\_Reports\\WinlossAnalysis";
        }

        public string GetWinlossAnalysisSuperPath()
        {
            return this.dirPath + "\\" + "WinlossAnalysisSuper.xls";
        }

        public string GetWinlossAnalysisMasterPath()
        {
            return this.dirPath + "\\" + "WinlossAnalysisMaster.xls";
        }

        public string GetWinlossAnalysisAgentPath()
        {
            return this.dirPath + "\\" + "WinlossAnalysisAgent.xls";
        }
    }
}