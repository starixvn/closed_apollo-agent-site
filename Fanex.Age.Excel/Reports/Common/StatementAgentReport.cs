using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;

/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Fanex.Age.Excel.Reports.Common
{
    internal class StatementAgentReport : StatementSuperReport
    {
        public StatementAgentReport(string filePath, StatementReportBuilder builder)
            : base(filePath, builder)
        {
        }
    }
}
