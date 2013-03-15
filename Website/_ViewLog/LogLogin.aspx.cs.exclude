using System;
using System.Collections;
using System.Collections.Generic;
using Sunplus.Agent.Model.Entities;
using Sunplus.Shared.ViewLogLib.Entities;
using Sunplus.Shared.ViewLogLib.Services;
using Sunplus.Agent.Model.BL.Authorization;
using Sunplus.Agent.Model.Entities;

namespace Sunplus.Agent.UI._ViewLog
{
    public partial class LogLogin_Page : ViewLogPageBase
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            DataControl = rptDataLog;
            NoInfoLabel = ltrNoInfo;
            CurrentUrl = "LogLogin.aspx";
        }

        public override IRequestViewLogContext CreateRequestViewLogContext()
        {
            if (Sunplus.Agent.Model.Entities.UserSession.Current.RoleId == 2) // For Agent level
            {
                var isSubAccount = false;
                var customerId = -1;

                AuthorizationBO bo = new AuthorizationBO(Session.SessionID);

                bool isValidDownline = bo.IsUsernameBelong(CurrentUserSession.UserName,
                    UserSession.Current.CustId,
                    out customerId, out isSubAccount);

                if (!isValidDownline)
                {
                    return null;
                }

                IRequestViewLogContext requestViewLogContext = new RequestViewLogContext
                {
                    IsInternal = CurrentUserSession.IsInternal,
                    CustomerName = CurrentUserSession.UserName,
                    FromDate = this.FromDate,
                    ToDate = this.ToDate,
                    CustomerId = customerId,
                    IsSubCust = isSubAccount,
                    LanguageCode = ToLanguageCode(CurrentUserSession.LangKey)
                };

                return requestViewLogContext;
            }

            return base.CreateRequestViewLogContext();
        }

        protected override IList GetLogData(out int totalRecords)
        {
            totalRecords = 0;
            var requestViewLogContext = this.RequestViewLogContext;

            List<LogLogin> entityList = new List<LogLogin>();

            if (requestViewLogContext != null)
            {
                entityList = (new LogService<LogLogin>()).GetPaged(requestViewLogContext, this.PageIndex, this.PageSize, out totalRecords, null);
            }

            return entityList;
        }

    }
}
