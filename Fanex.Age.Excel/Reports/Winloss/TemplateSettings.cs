/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.Excel.Reports.Winloss
{
    using System.Web;

    internal class TemplateSettings
    {
        private string dirPath;

        public TemplateSettings()
        {
            this.dirPath = HttpContext.Current.Server.MapPath("~") + "\\ExcelTemplates\\_Reports\\Winloss";
        }

        public string GetWinlossSuperPath()
        {
            return this.dirPath + "\\" + "WinlossSuper.xls";
        }

        public string GetWinlossMasterPath()
        {
            return this.dirPath + "\\" + "WinlossMaster.xls";
        }

        public string GetWinlossAgentPath()
        {
            return this.dirPath + "\\" + "WinlossAgent.xls";
        }

        public string GetWinlossDetailSuperPath()
        {
            return this.dirPath + "\\" + "WinlossDetailSuper.xls";
        }

        public string GetWinlossDetailMasterPath()
        {
            return this.dirPath + "\\" + "WinlossDetailMaster.xls";
        }

        public string GetWinlossDetailAgentPath()
        {
            return this.dirPath + "\\" + "WinlossDetailAgent.xls";
        }

        public string GetWinlossByBetTypeAgentPath()
        {
            return this.dirPath + "\\" + "WinlossByBetTypeAgent.xls";
        }

        public string GetWinlossByBetTypeMasterPath()
        {
            return this.dirPath + "\\" + "WinlossByBetTypeMaster.xls";
        }

        public string GetWinlossByBetTypeSuperPath()
        {
            return this.dirPath + "\\" + "WinlossByBetTypeSuper.xls";
        }
    }
}