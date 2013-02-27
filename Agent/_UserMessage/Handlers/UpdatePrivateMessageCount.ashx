<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._UserMessage.UpdatePrivateMessageCount_Handler" %>

using System;
using System.Web;
using System.Web.SessionState;
using Sunplus.Agent.Model.BL.UserMessage;
using Sunplus.Agent.Model.Entities;

using Fanex.Age.UI;

/*
 * Created Don@20100617 - Handler class of user message function: update for status is readed to private messages
 * Revision:
 *
 *
 */

namespace Sunplus.Agent.UI._UserMessage
{

    public class UpdatePrivateMessageCount_Handler : Fanex.Age.UI.TplHandler, IReadOnlySessionState
    {
        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            data.AsResult(View as Result);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            int errCode = (int)ErrorType.Unknown;
            if (!CurrentUserSession.IsInternal)
            {
                UserMessageBO userMessageBO = new UserMessageBO(CurrentUserSession.NetSessionId);
                if (userMessageBO.SetReadPriMessage())
                {
                    errCode = (int)ErrorType.Success;
                }
            }

            return new Result(errCode, null);
        }
    }

}