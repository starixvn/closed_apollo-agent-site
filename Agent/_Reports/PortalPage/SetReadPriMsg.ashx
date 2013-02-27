<%@ WebHandler Language="C#" Class="Faxex.Age.UI.Reports.PortalPage.SetReadPriMsg" %>

using System;
using System.Web;
using Fanex.Age.UI;
using Sunplus.Agent.Model.BL.UserMessage;
using Sunplus.Agent.Model.Entities;

namespace Faxex.Age.UI.Reports.PortalPage
{
    public class SetReadPriMsg : TplHandler
    {
        private int _messageId = -1;
        private ErrorType errCode = ErrorType.Unknown;
        private string errMsg = "Unknown Message";

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            _messageId = Convert.ToInt32(Request.Form["messageId"]);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (_messageId == -1 || CurrentUserSession.IsInternal)
            {
                return new Result(errCode, errMsg);
            }

            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            var business = new UserMessageBO(UserSession.Current.NetSessionId);
            business.SetReadPriMsg(CurrentUserSession.CustId, _messageId.ToString());
            errCode = ErrorType.Success;
            errMsg = "";
            return new Result(errCode, errMsg);
        }

        public override void RenderResult(ref JSONData data)
        {
            Result r = View as Result;
            data.AsResult(r);
        }
    }
}