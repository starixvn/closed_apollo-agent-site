<%@ WebHandler Language="C#" Class="Fanex.Age.UI.LiveCasino.CurrentMemberWinLimitHandler" %>

using System;
using System.Web;
using System.Collections.Generic;
using Resources;
using Fanex.LiveCasinoSetting.Model;
using Fanex.LiveCasinoSetting.Request;

namespace Fanex.Age.UI.LiveCasino
{
    public class CurrentMemberWinLimitHandler : Fanex.Age.UI.LiveCasino.LiveCasinoHandler, System.Web.SessionState.IReadOnlySessionState
    {
        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            //if (!CurrentUserSession.SubPageAccess.UpdateAccount || CurrentUserSession.IsInternal)
            //{
            //    return new Fanex.Age.UI.Result(Fanex.Age.UI.ErrorType.AccessDenied, BeginDivErr + Resources.Lang.lblNotAuthorized + EndDiv);
            //}

            //return base.ValidateRequest(p);
            return null;
        }

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            var memberWinLimit = View as IMemberWinLimit;
            var ofTotal = string.Empty;

            if (memberWinLimit.MaxWinLimit == 0m && memberWinLimit.TotalWinLimit == 0m && memberWinLimit.CurrentWinLimit == 0m)
            {
                ofTotal = "&nbsp;&nbsp;" + Lang.Unlimited;
            }
            else
            {
                ofTotal = string.Join(null, "<div style='width: 68px; float: left;'>", "&nbsp;&nbsp;", this.FormatPercent(memberWinLimit.CurrentWinLimit), "</div>", "&nbsp;&nbsp;&nbsp;", Lang.of, "&nbsp;", memberWinLimit.TotalWinLimit.ToString("#,##0.##"));
            }

            data.AsResult(new Result(0, ofTotal), true);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            var service =
                new Fanex.LiveCasinoSetting.Business.MemberWinLimitService();

            return service.GetMemberWinLimit(RequestCustId);
        }

        private string FormatPercent(decimal value)
        {
            if (value >= 100m)
            {
                return "<font color='Red'>" + value.ToString("#,##0.##") + "%</font>";
            }

            return value.ToString("#,##0.##") + "%";
        }
    }
}