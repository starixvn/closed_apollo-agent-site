<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.RaceBook.UpdateMinPT_Handler" %>

using System;
using System.Web;
using System.Collections;

using Sunplus.Agent.UI.PositionTaking.RaceBook;
using Fanex.Age.UI.Components;
using Sunplus.Agent.UI._Components.RacingStatus_Control;
using Fanex.Age.UI.Components.PT_Control;
using Sunplus.Agent.UI._MemberInfo.RaceBook.MinMaxBet_Control;
using Fanex.Age.UI;
using Sunplus.Agent.Utilities;
using Sunplus.Agent.Model.BL.Racing;

namespace Sunplus.Agent.UI._MemberInfo.RaceBook
{
    public class UpdateMinPT_Handler : RaceBookUpdateBase_Handler, System.Web.SessionState.IReadOnlySessionState
    {

        public override void RenderResult(ref JSONData data)
        {
            Hashtable view = View as Hashtable;
            string[] ArrCodes = view["errcode"].ToString().Split("\a".ToCharArray());
            string[] ArrMsgs = view["errmsg"].ToString().Split("\a".ToCharArray());

            GetResourceMsg(ref ArrCodes, ref ArrMsgs);

            data.Add("ArrCodes", ArrCodes);
            data.Add("ArrMsgs", ArrMsgs);


        }

        public override object InvokeData(Hashtable p)
        {
            Hashtable outputs = null;
            RacingBO racing = new RacingBO(CurrentUserSession.NetSessionId);

            racing.UpdateMinPTHR(p, out outputs);

            if (outputs["errcode"].Equals(0))
            {
                outputs["errcode"] = -107;
            }

            return outputs;
        }
    }
}