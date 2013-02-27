<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers.AutoInsertAccountLiveCSFromExisting_Handler" %>

/*
 * Created 20120409@Jeff - auto insert account from exist
 * Revision:
 * ?@? - ...
 *
 */

using System;
using System.Web;

using DAL;
using BLL;

using Fanex.Age.UI;
using Fanex.LiveCasinoSetting;
using Fanex.LiveCasinoSetting.Model.Log;
using Fanex.LiveCasinoSetting.Request;
using Fanex.LiveCasinoSetting.Business;
using Sunplus.Agent.Model.Entities.Permission;
using Sunplus.Agent.Model.BL.Authorization;
using Sunplus.Agent.Model.BL.AccountInfo;

namespace Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers
{
    public class AutoInsertAccountLiveCSFromExisting_Handler : TplHandler, System.Web.SessionState.IReadOnlySessionState
    {
        int custId;
        int custIdOld;
        int FromSystem = 2; // Agent
        string errMsg = string.Empty;
        int errCode = (int)ErrorType.Unknown;

        Sunplus.Agent.Model.Entities.CustomerInfo custInfo;
        Sunplus.Agent.Model.Entities.CustomerInfo custInfoOld;

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            Result r = View as Result;
            data.AsResult(r);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            try
            {
                var accBO = new AccountInfoBO(CurrentUserSession.NetSessionId);
                custInfo = accBO.GetCustInfo(custId);
                custInfoOld = accBO.GetCustInfo(custIdOld);

                if (custInfo == null || custInfoOld == null)
                {
                    return new Result(101, errMsg);
                }

                var service = new Fanex.LiveCasinoSetting.Business.GameSettingService();
                if (CurrentUserSession.IsLiveCasinoSync && custInfoOld.LiveCasinoSync)
                {
                    var results = service.InsertAccountFromExisting(new Fanex.LiveCasinoSetting.Request.InsertAccountRequest()
                    {
                        AgentId = custInfo.AIBCCustID,
                        CustId = custId,
                        CustUserName = custInfo.UserName,
                        ExchangeId = CurrentUserSession.CurrencyId,
                        SignUpIp = Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                        Language = CurrentUserSession.LangKey,
                        MasterId = custInfo.MIBCCustID,
                        OldCustId = custIdOld,
                        RoleId = custInfo.RoleId,
                        SignUpForm = FromSystem,
                        SubAccId = CurrentUserSession.SubAccId,
                        SuperId = custInfo.SIBCCustID,
                        UserId = CurrentUserSession.UserId,
                        UserName = CurrentUserSession.UserName,
                        CopiedFrom = custInfoOld.UserName
                    });

                    if (string.IsNullOrWhiteSpace(results.ErrorCode))
                    {
                        errCode = (int)(ErrorType.Success);

                        this.UpdateCustomerStatus();
                    }
                    else
                    {
                        errCode = -999;
                    }

                    errMsg = results.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                errCode = 102;
                errMsg = ex.Message;
            }

            return new Result(errCode, errMsg);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            int.TryParse(Request["custid"], out custId);
            int.TryParse(Request["custidOld"], out custIdOld);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (custId <= 0 || custIdOld <= 0 ||
                !CurrentUserSession.IsLiveCasinoSync)
            {
                return new Result(errCode, errMsg);
            }

            return null;
        }

        private void UpdateCustomerStatus()
        {
            var req = new InsertAccountRequest()
            {
                AgentId = custInfo.AIBCCustID,
                CustId = custId,
                CustUserName = custInfo.UserName,
                ExchangeId = CurrentUserSession.CurrencyId,
                SignUpIp = Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                IsDisabledLiveCasino = custInfoOld.LiveCasinoDisabled,
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

            var service = new SettingService();
            service.UpdateStatusLiveCasino(new UpdateLiveCasinoStatusRequest(req));
        }
    }
}