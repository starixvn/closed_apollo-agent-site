using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Fanex.Age.Excel.Reports.Common
{
    internal class TemplateSettings
    {
        private string _dir;

        public TemplateSettings()
        {
            _dir = HttpContext.Current.Server.MapPath("~") + "\\ExcelTemplates\\_Reports\\Common";
        }

        public string GetStatementPath(bool isAgentSite)
        {
            return _dir + "\\" + "Statement.xls";
        }

        public string GetMatchResultEntered()
        {
            return _dir + "\\" + "MatchResultEntered.xls";
        }
    }
}
