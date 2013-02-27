using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Fanex.Age.Excel.Reports.Commission
{
    class TemplateSettings
    {
        private string _dir;

        public TemplateSettings()
        {
            _dir = HttpContext.Current.Server.MapPath("~") + "\\ExcelTemplates\\_Reports\\Commission";
        }

        public string GetHRCommissionPath()
        {
            return _dir + "\\" + "HRCommission.xls";
        }

        public string CommissionByBetTypePath()
        {
            return _dir + "\\" + "CommissionByBetType.xls";
        }
    }
}
