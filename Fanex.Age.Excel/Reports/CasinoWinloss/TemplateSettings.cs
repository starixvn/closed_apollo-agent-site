using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Fanex.Age.Excel.Reports.CasinoWinloss
{
    class TemplateSettings
    {
        private string dirPath;

        public TemplateSettings()
        {
            this.dirPath = HttpContext.Current.Server.MapPath("~") + "\\ExcelTemplates\\_Reports\\CasinoWinloss";
        }

        public string GetCSWinlossDetailSuperPath()
        {
            return this.dirPath + "\\" + "CSWinlossDetailSuper.xls";
        }

        public string GetCSWinlossDetailMasterPath()
        {
            return this.dirPath + "\\" + "CSWinlossDetailMaster.xls";
        }

        public string GetCSWinlossDetailAgentPath()
        {
            return this.dirPath + "\\" + "CSWinlossDetailAgent.xls";
        }

        public string GetCSWinlossSuperPath()
        {
            return this.dirPath + "\\" + "CSWinlossSuper.xls";
        }

        public string GetCSWinlossMasterPath()
        {
            return this.dirPath + "\\" + "CSWinlossMaster.xls";
        }

        public string GetCSWinlossAgentPath()
        {
            return this.dirPath + "\\" + "CSWinlossAgent.xls";
        }
    }
}
