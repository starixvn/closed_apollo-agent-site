using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

/*
 * Created 20120221@Leo
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Fanex.Age.Excel.Reports.Consolidated
{
    class TemplateSettings
    {
        private string _dir;

        public TemplateSettings()
        {
            _dir = HttpContext.Current.Server.MapPath("~") + "\\ExcelTemplates\\_Reports\\Consolidated";
        }

        public string GetConsolidatedPath()
        {
            return _dir + "\\" + "Consolidated.xls";
        }
    }
}
