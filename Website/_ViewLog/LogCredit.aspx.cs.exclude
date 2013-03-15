using System;
using System.Collections;
using System.Collections.Generic;
using Sunplus.Shared.ViewLogLib.Services;
using Sunplus.Shared.ViewLogLib.Entities;

namespace Sunplus.Agent.UI._ViewLog
{
    public partial class LogCredit_Page : ViewLogPageBase
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            DataControl = rptDataLog;
            NoInfoLabel = ltrNoInfo;
            CurrentUrl = "LogCredit.aspx";
        }

        protected override IList GetLogData(out int totalRecords)
        {
            totalRecords = 0;

            List<LogCredit> entityList = new List<LogCredit>();

            var requestViewLogContext = this.RequestViewLogContext;

            if (requestViewLogContext != null)
            {
                entityList = (new LogService<LogCredit>()).GetPaged(requestViewLogContext, this.PageIndex, this.PageSize, out totalRecords, null);
            }

            return entityList;
        }
    }
}