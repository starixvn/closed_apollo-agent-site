<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.P2P.GetScreenName_Handler" %>

using System;
using System.Web;

namespace Sunplus.Agent.UI._MemberInfo.P2P
{
    public class GetScreenName_Handler : Fanex.Age.UI.TplHandler, System.Web.SessionState.IReadOnlySessionState
    {
        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            data.AsResult(View as Fanex.Age.UI.Result, true);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            try
            {
                using (var service = new Fanex.Age.Business.BetList.P2P.WebServiceHandle())
                {
                    var info = service.getP2PCustInfo(Sunplus.Agent.Model.BL.Definitions.AppSettings.P2PSiteId, Request.QueryString["custids"]);

                    return new Fanex.Age.UI.Result(info.Error_code, string.IsNullOrWhiteSpace(info.Nickname) ? string.Empty : info.Nickname);
                }
            }
            catch (Exception ex)
            {
                Fanex.Logging.LogService.Error(ex.Message, ex);

                return new Fanex.Age.UI.Result(-1, string.Empty);
            }
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {

        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }
    }
}