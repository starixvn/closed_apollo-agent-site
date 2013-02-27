using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Fanex.Age.Excel.Reports.MatchWinloss
{
    class TemplateSettings
    {
        private string _dir;

        public TemplateSettings()
        {
            _dir = HttpContext.Current.Server.MapPath("~") + "\\ExcelTemplates\\_Reports\\MatchWinloss";
        }

        public string GetMatchWinLossDetailPath(bool isSuper, bool isMaster, bool isAgent)
        {
            if (isSuper)
                return _dir + "\\" + "MatchWinLossDetailSuper.xls";
            else if (isMaster)
                return _dir + "\\" + "MatchWinLossDetailMaster.xls";
            else if (isAgent)
                return _dir + "\\" + "MatchWinLossDetailAgent.xls";
            else return _dir;
        }

        public string GetMatchWinLossPath()
        {
            return _dir + "\\" + "MatchWinLoss.xls";
        }
    }
}
