<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.RaceBook.Master.UpdateMember_Handler" %>

using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Data;

using Sunplus.Agent.UI.PositionTaking.RaceBook;
using Fanex.Age.UI.Components;
using Sunplus.Agent.UI._Components.RacingStatus_Control;
using Fanex.Age.UI.Components.PT_Control;
using Fanex.Age.UI;
using Sunplus.Agent.Utilities;
using Sunplus.Agent.Model.BL.Racing;

namespace Sunplus.Agent.UI._MemberInfo.RaceBook.Master
{
    public class UpdateMember_Handler : RaceBookUpdateBase_Handler, System.Web.SessionState.IReadOnlySessionState
    {
        public override void RenderResult(ref JSONData data)
        {
            Hashtable view = View as Hashtable;
            string[] ArrCodes = view["errcode"].ToString().Split("\a".ToCharArray());
            string[] ArrMsgs = view["errmsg"].ToString().Split("\a".ToCharArray());
            string[] ArrUsernames = view["username"].ToString().Split("\a".ToCharArray());

            GetResourceMsg(ref ArrCodes, ref ArrMsgs);

            Hashtable hash = AppSession.RequestQueryString;
            if (hash["ParentForm"] == null || hash["ParentForm"].ToString() != "RacingPositionTakingList.aspx")
            {
                data.Add("ArrCodes", ArrCodes);
                data.Add("ArrMsgs", ArrMsgs);
            }
            else
            {
                data.Add("ArrCodes", -1);

                string param = String.Empty;
                param += hash["ParentForm"] != null ? hash["ParentForm"] : string.Empty;
                param += "?act=" + (hash["act"] != null ? hash["act"] : string.Empty);
                param += "&custid=" + (hash["custid"] != null ? hash["custid"] : string.Empty);
                param += "&masterid=" + (hash["masterid"] != null ? hash["masterid"] : string.Empty);
                param += "&type=" + (hash["type"] != null ? hash["type"] : string.Empty);
                param += "&USER=" + (hash["USER"] != null ? hash["USER"] : string.Empty);
                data.Add("ArrMsgs", "parent.age.DelayReloadPage('" + param + "',1000)");
            }
            data.Add("ArrUsernames", ArrUsernames);
        }

        public override object InvokeData(Hashtable p)
        {
            Hashtable outputs = null;
            RacingBO racing = new RacingBO(CurrentUserSession.NetSessionId);

            racing.MasterUpdateMemberHR(p, out outputs);

            return outputs;
        }
    }
}