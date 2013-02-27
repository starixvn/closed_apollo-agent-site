<%@ WebHandler Language="C#" Class="Fanex.Age.UI.LiveCasino.AutoInsertAccount_Handler" %>
/*
    Created by: Andy
    Description: Handler for processing auto insert account
*/
using System;
using System.Web;

using DAL;
using BLL;

using Fanex.Age.UI;
using Fanex.LiveCasinoSetting.Model.Log;
using Sunplus.Agent.Model.Entities.Permission;
using Sunplus.Agent.Model.BL.Authorization;
using Sunplus.Agent.Model.BL.AccountInfo;

namespace Fanex.Age.UI.LiveCasino
{
    public class AutoInsertAccount_Handler : Fanex.Age.UI.TplHandler, System.Web.SessionState.IReadOnlySessionState
    {
        int custId;
        int FromSystem = 2; // Agent

        Sunplus.Agent.Model.Entities.CustomerInfo custInfo;

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            Result result = View as Result;
            data.Add("errCode", result.ErrCode);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            Result result = new Result(ErrorType.Unknown, string.Empty);

            custInfo = new AccountInfoBO(CurrentUserSession.NetSessionId).GetCustInfo(custId);

            if (custInfo == null)
            {
                return result;
            }

            result.ErrCode = (int)(ErrorType.Success);

            var service = new LiveCasinoSetting.Business.GameSettingService();
            if (CurrentUserSession.IsLiveCasinoSync && !custInfo.LiveCasinoSync)
            {
                var tblResult = new System.Data.DataTable();
                var results = service.InsertAccount(new Fanex.LiveCasinoSetting.Request.InsertAccountRequest()
                {
                    AgentId = custInfo.AIBCCustID,
                    CustId = custId,
                    CustUserName = custInfo.UserName,
                    ExchangeId = CurrentUserSession.CurrencyId,
                    SignUpIp = Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                    IsDisabledLiveCasino = CurrentUserSession.IsLiveCasinoDisabled,
                    IsEnableDownline = false,
                    Language = CurrentUserSession.Language,
                    MasterId = custInfo.MIBCCustID,
                    RoleId = custInfo.RoleId,
                    SignUpForm = FromSystem,
                    SubAccId = CurrentUserSession.SubAccId,
                    SuperId = custInfo.SIBCCustID,
                    SyncLiveCasino = CurrentUserSession.IsLiveCasinoSync,
                    UserId = CurrentUserSession.UserId,
                    UserName = CurrentUserSession.UserName,
                    IsAutoInsert = true
                }, ref tblResult);

                if (string.IsNullOrWhiteSpace(results.ErrorCode))
                {
                    result.ErrCode = 0;

                    new Fanex.LiveCasinoSetting.Business.LogWriterService().
                    WriteLog
                    (
                        new Fanex.LiveCasinoSetting.Model.Log.LogAutoInsertAccount()
                        {
                            Action = Level,
                            AgentId = custInfo.AIBCCustID,
                            CustId = custId,
                            CustUserName = custInfo.UserName,
                            ExchangeId = CurrentUserSession.CurrencyId,
                            SignUpIp = Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                            Language = CurrentUserSession.LangKey,
                            MasterId = custInfo.MIBCCustID,
                            RoleId = custInfo.RoleId,
                            SignUpForm = FromSystem,
                            SubAccId = CurrentUserSession.SubAccId,
                            SubAccName = CurrentUserSession.SubUserName,
                            SuperId = custInfo.SIBCCustID,
                            UserId = CurrentUserSession.UserId,
                            UserName = CurrentUserSession.UserName,
                            ErrorCode = result.ErrCode.ToString(),
                            ErrorMessage = result.ErrMsg,
                            dataResult = tblResult
                        }
                    );
                }
                else
                    result.ErrCode = -999;

                result.ErrMsg = results.ErrorMessage;
            }

            return result;
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            int.TryParse(Request["custid"], out custId);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (custId <= 0 ||
                !CurrentUserSession.IsLiveCasinoSync)
            {
                return new Result(ErrorType.Unknown, string.Empty);
            }

            return null;
        }

        private LogActions Level
        {
            get
            {
                switch (custInfo.RoleId)
                {
                    case 3:
                        return LogActions.AutoInsert_Master;
                    case 2:
                        return LogActions.AutoInsert_Agent;
                    case 1:
                        return LogActions.AutoInsert_Member;
                }

                return LogActions.AutoInsert_Master;
            }
        }
    }
}