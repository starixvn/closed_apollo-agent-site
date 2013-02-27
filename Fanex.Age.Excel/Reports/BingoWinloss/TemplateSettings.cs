using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Fanex.Age.Excel.Reports.BingoWinloss
{
    class TemplateSettings
    {
        private string dirPath;

        public TemplateSettings()
        {
            this.dirPath = HttpContext.Current.Server.MapPath("~") + "\\ExcelTemplates\\_Reports\\BingoWinloss";
        }

        public string GetBingoWinlossDetailSuperPath()
        {
            return this.dirPath + "\\" + "BingoWinlossDetailSuper.xls";
        }

        public string GetBingoWinlossDetailMasterPath()
        {
            return this.dirPath + "\\" + "BingoWinlossDetailMaster.xls";
        }

        public string GetBingoWinlossDetailAgentPath()
        {
            return this.dirPath + "\\" + "BingoWinlossDetailAgent.xls";
        }

        public string GetBingoWinlossSuperPath()
        {
            return this.dirPath + "\\" + "BingoWinlossSuper.xls";
        }

        public string GetBingoWinlossMasterPath()
        {
            return this.dirPath + "\\" + "BingoWinlossMaster.xls";
        }

        public string GetBingoWinlossAgentPath()
        {
            return this.dirPath + "\\" + "BingoWinlossAgent.xls";
        }
    }
}
