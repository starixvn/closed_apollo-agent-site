<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers.Agent.AutoInsertAccountLiveCSFromExistingMember_Handler" %>

/*
 * Created 20120409@Jeff - auto insert account from exist
 * Revision:
 * ?@? - ...
 *
 */

using System;
using System.Web;
using System.Collections;

using DAL;
using BLL;

using Fanex.Age.UI;
using Fanex.LiveCasinoSetting;
using Fanex.LiveCasinoSetting.Business;
using Fanex.LiveCasinoSetting.Request;
using Fanex.Age.UI.LiveCasino;
using Fanex.LiveCasinoSetting.Model.Log;
using Sunplus.Agent.Model.Entities.Permission;
using Sunplus.Agent.Model.BL.Authorization;
using Sunplus.Agent.Model.BL.AccountInfo;

namespace Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers.Agent
{
    public class AutoInsertAccountLiveCSFromExistingMember_Handler : TplHandler, System.Web.SessionState.IReadOnlySessionState
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
            var gsResult = new Result(errCode, errMsg);
            try
            {
                var service = new GameSettingService();
                var ptService = new SettingService();

                var model = ptService.GetMemberSetting(custIdOld, CurrentUserSession.CustId);
                var settingPT = model.PTSetting as Fanex.LiveCasinoSetting.Model.PTSetting;
                var accBO = new AccountInfoBO(CurrentUserSession.NetSessionId);
                custInfo = accBO.GetCustInfo(custId);
                custInfoOld = accBO.GetCustInfo(custIdOld);

                var ptResult = this.UpdateMemberSetting(settingPT, custId);
                gsResult = this.AutoInsertAccountFromExistLiveCasino();
                
                if (gsResult.ErrCode == 0)
                {
                    this.UpdateMemberStatus();

                    var memberWinLimit = new MemberWinLimitService();
                    var member = new Fanex.LiveCasinoSetting.Model.MemberWinLimit()
                    {
                        MaxWinLimit = model.CustomerInfo.Limit.MaxWinLimit,
                        IsDailyAutoReset = model.CustomerInfo.Limit.IsDailyAutoReset
                    };
                    var context = new RequestContext()
                    {
                        CustId = custId,
                        UserId = CurrentUserSession.UserId,
                        Language = CurrentUserSession.LangKey,
                        SignUpIp = Request.UserHostAddress,
                        SubAccId = CurrentUserSession.SubAccId
                    };

                    memberWinLimit.UpdateMemberWinLimit(member, context);
                }

                errCode = gsResult.ErrCode;
                errMsg = gsResult.ErrMsg;
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

        private Result UpdateMemberSetting(Fanex.LiveCasinoSetting.Model.PTSetting pt, int custId)
        {
            pt.PropertiesChangedList.Add("Comm");
            ((Fanex.LiveCasinoSetting.Model.PTSetting)pt.Upline).PropertiesChangedList.Add("PresetPT");
            ((Fanex.LiveCasinoSetting.Model.PTSetting)pt.Upline).PresetPT = pt.Upline.PresetPT;

            var results = new System.Collections.Generic.Dictionary<int, Fanex.LiveCasinoSetting.Model.Result>();

            Fanex.LiveCasinoSetting.Model.Result result = new SettingService().UpdateMemberPTSetting
            (
                new Fanex.LiveCasinoSetting.Request.RequestContext()
                {
                    CustId = custId,
                    SignUpIp = Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                    Language = CurrentUserSession.LangKey,
                    SubAccId = CurrentUserSession.SubAccId,
                    UplineId = CurrentUserSession.CustId,
                    UserId = CurrentUserSession.UserId,
                    IsSyncLiveCasino = CurrentUserSession.IsLiveCasinoSync
                },
                pt
            );

            return new Result(string.IsNullOrWhiteSpace(result.ErrorCode) ? 0 : -1, result.ErrorMessage);
        }

        private Result AutoInsertAccountFromExistLiveCasino()
        {
            if (custInfo == null || custInfoOld == null)
            {
                return new Result(101, errMsg);
            }

            var service = new GameSettingService();
            if (CurrentUserSession.IsLiveCasinoSync && custInfoOld.LiveCasinoSync)
            {
                var results = service.InsertAccountFromExisting(new InsertAccountRequest()
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
                }
                else
                {
                    errCode = -999;
                }

                errMsg = results.ErrorMessage;
            }

            return new Result(errCode, errMsg);
        }

        private void UpdateMemberStatus()
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