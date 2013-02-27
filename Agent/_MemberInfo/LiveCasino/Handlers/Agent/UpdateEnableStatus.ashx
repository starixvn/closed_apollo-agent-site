<%@ WebHandler Language="C#" Class="Fanex.Age.UI.LiveCasino.UpdateEnableStatus_Handler" %>
/*
    Created by: Andy
    Description: Handler for processing agent edits [Enable Live Casino] of member
*/
using System;
using System.Web;
using System.Collections.Generic;

using Fanex.LiveCasinoSetting.Request;
using Fanex.LiveCasinoSetting.Model;

using System.ComponentModel;

namespace Fanex.Age.UI.LiveCasino
{
    public class UpdateEnableStatus_Handler : TplHandler, System.Web.SessionState.IReadOnlySessionState
    {
        public override void RenderResult(ref JSONData data)
        {
            var res = View as Fanex.LiveCasinoSetting.Model.Result;

            data.AsResult(new Result(string.IsNullOrWhiteSpace(res.ErrorCode) ? 0 : -1, res.ErrorMessage), true);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            var service = new Fanex.LiveCasinoSetting.Business.SettingService();

            var req = this.GetInsertAccountRequest(Convert.ToInt32(Request["custid"]));

            return service.UpdateStatusLiveCasino(new UpdateLiveCasinoStatusRequest(req));
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {

        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            var cust = new Fanex.LiveCasinoSetting.Business.SettingService().GetCustomerInfo(Convert.ToInt32(Request["custid"]));

            if (!CurrentUserSession.IsAgent || Convert.ToInt32(Request["CustRoleId"]) != 1 || cust.UplineDisableLiveCasino)
            {
                return new Fanex.Age.UI.Components.ErrorPage_Page(Resources.Lang.strNotAccessPage);
            }

            if (!CurrentUserSession.SubPageAccess.UpdateAccount || CurrentUserSession.IsInternal)
            {
                return new Fanex.Age.UI.Result(Fanex.Age.UI.ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
            }

            return null;
        }

        private InsertAccountRequest GetInsertAccountRequest(int custId)
        {
            var custInfo = new Sunplus.Agent.Model.BL.AccountInfo.AccountInfoBO(CurrentUserSession.NetSessionId).GetCustInfo(custId);

            var result = new Fanex.LiveCasinoSetting.Request.InsertAccountRequest()
            {
                AgentId = custInfo.AIBCCustID,
                CustId = custId,
                CustUserName = custInfo.UserName,
                ExchangeId = CurrentUserSession.CurrencyId,
                SignUpIp = Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                IsDisabledLiveCasino = !Convert.ToBoolean(Request["enable"]),
                IsEnableDownline = false,
                Language = CurrentUserSession.LangKey,
                MasterId = custInfo.MIBCCustID,
                RoleId = custInfo.RoleId,
                SignUpForm = 2, // Agency system
                SubAccId = CurrentUserSession.SubAccId,
                SubAccName = CurrentUserSession.SubUserName,
                SuperId = custInfo.SIBCCustID,
                SyncLiveCasino = CurrentUserSession.IsLiveCasinoSync,
                UserId = CurrentUserSession.UserId,
                UserName = CurrentUserSession.UserName,
                IsAutoInsert = false // Manually insert account
            };

            return result;
        }
    }
}