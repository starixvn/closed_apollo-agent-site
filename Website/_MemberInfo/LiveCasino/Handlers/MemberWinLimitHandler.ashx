<%@ WebHandler Language="C#" Class="Fanex.Age.UI.LiveCasino.MemberWinLimitHandler" %>

using System;
using System.Web;
using System.Collections.Generic;
using Resources;
using Fanex.LiveCasinoSetting.Model;
using Fanex.LiveCasinoSetting.Request;

namespace Fanex.Age.UI.LiveCasino
{
    public class MemberWinLimitHandler : Fanex.Age.UI.LiveCasino.LiveCasinoHandler, System.Web.SessionState.IReadOnlySessionState
    {
        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (!CurrentUserSession.SubPageAccess.UpdateAccount || CurrentUserSession.IsInternal)
            {
                return new Fanex.Age.UI.Result(Fanex.Age.UI.ErrorType.AccessDenied, BeginDivErr + Resources.Lang.lblNotAuthorized + EndDiv);
            }

            return base.ValidateRequest(p);
        }

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            if (Custids.Count > 0)
            {
                var res = View as IList<Fanex.LiveCasinoSetting.Model.Result>;
                ErrCode = "0";

                foreach (var item in res)
                {
                    if (string.IsNullOrWhiteSpace(item.ErrorCode) || Convert.ToInt32(item.ErrorCode) == 0)
                    {
                        ErrCodeStrs += "0$";
                        ErrMsg += string.Join(null, BeginDivSucc, Lang.MemberWinLimit, ": ", Lang.updatesuccessfull, EndDiv, "$");
                    }
                    else
                    {
                        ErrCodeStrs += item.ErrorCode + "$";
                        ErrMsg += string.Join(null, BeginDivErr, Lang.MemberWinLimit, ": ", item.ErrorMessage, EndDiv, "$");
                    }
                }

                data.AsResult(new Fanex.Age.UI.Result(0, this.FormatMsg(ErrCodeStrs, ErrMsg)), true);
            }
            else
            {
                var res = View as Fanex.LiveCasinoSetting.Model.Result;

                if (string.IsNullOrWhiteSpace(res.ErrorCode) || Convert.ToInt32(res.ErrorCode) == 0)
                {
                    ErrMsg = string.Join(null, "0|", BeginDivSucc, Lang.MemberWinLimit, ": ", Lang.updatesuccessfull, EndDiv);
                    ErrCode = "0";
                }
                else
                {
                    ErrMsg = string.Join(null, "1|", BeginDivErr, Lang.MemberWinLimit, ": ", res.ErrorMessage, EndDiv);
                    ErrCode = res.ErrorCode;
                }

                data.AsResult(new Fanex.Age.UI.Result(Convert.ToInt32(ErrCode), ErrMsg), true);
            }
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            var service =
                new Fanex.LiveCasinoSetting.Business.MemberWinLimitService();

            var memberWinLimit = new MemberWinLimit()
            {
                IsDailyAutoReset = Convert.ToBoolean(Request.QueryString["isDailyReset"]),
                MaxWinLimit = Convert.ToDecimal(Request.QueryString["maxWinLimit"])
            };

            var context = new RequestContext()
            {
                CustId = RequestCustId,
                Language = CurrentUserSession.LangKey,
                SignUpIp = Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                SubAccId = CurrentUserSession.SubAccId,
                UserId = CurrentUserSession.UserId
            };

            if (Custids.Count > 0)
            {
                return service.UpdateMultipleMemberWinLimit(memberWinLimit, context, Custids);
            }
            else
            {
                return service.UpdateMemberWinLimit(memberWinLimit, context);
            }
        }
    }
}