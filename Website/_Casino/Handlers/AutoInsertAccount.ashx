<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._Casino.AutoInsertAccount_Handler" %>

using System;
using System.Web;

using CasinoLibrary;
using Sunplus.Agent.Model.BL.Casino;

using DAL;
using BLL;

using Fanex.Age.UI;
using Sunplus.Agent.Utilities;
using Sunplus.Agent.Model.BL.AccountInfo;
using Sunplus.Agent.Model.BL.Authorization;
using System.Collections;

namespace Sunplus.Agent.UI._Casino
{
    public class AutoInsertAccount_Handler : Fanex.Age.UI.TplHandler, System.Web.SessionState.IReadOnlySessionState
    {
        string CasinoNo = Globals.CASINO_GROUP1;
        int custId;
        int FromSystem = 2; // Agent

        CSCustomerDetail custInfo;

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            Result result = View as Result;
            data.AsResult(result);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            Result result = new Result(ErrorType.Unknown, string.Empty);

            if (!CurrentUserSession.IsCasinoSync) // Don't call auto insert casino account
            {
                return result;
            }

            custInfo = CasinoManager.GetCustInfoIBC(custId);

            if (custInfo == null || custInfo.CasinoSync) // Haven't been replicated yet or had been synchronized already
            {
                return result;
            }

            bool isSuccess = false;

            CasinoLibrary.SBAgent.Customer customer = new CasinoLibrary.SBAgent.Customer();

            try
            {
                isSuccess = customer.AutoInsertAccount(
                    CasinoNo,
                    custInfo.IBCCustID,
                    custInfo.UserName,
                    custInfo.UserName2,
                    custInfo.FirstName,
                    custInfo.LastName,
                    custInfo.UserPwd,
                    custInfo.Phone,
                    custInfo.MobilePhone,
                    custInfo.Currency,
                    custInfo.Fax,
                    custInfo.Closed,
                    custInfo.Suspend,
                    custInfo.IsEnablePT,
                    custInfo.SIBCCustID,
                    custInfo.MIBCCustID,
                    custInfo.AIBCCustID,
                    custInfo.RoleId.ToString(),
                    DateTime.Now,
                    Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                    CurrentUserSession.CustId,
                    CurrentUserSession.UserName,
                    FromSystem
                    );
            }
            catch (Exception e)
            {

            }

            if (isSuccess) // Auto insert casino account successful
            {
                result.ErrCode = (int)(ErrorType.Success);

                // Get Status Current user. Check data concurrent update
                Hashtable outputs = new Hashtable();
                AuthorizationBO authorizationBO = new AuthorizationBO(CurrentUserSession.NetSessionId);
                authorizationBO.GetCustomerStatus(CurrentUserSession.CustId, 0, ref outputs);

                if (outputs.Count > 0)
                {
                    // Update syncs status
                    AccountInfoBO accountInfo = new AccountInfoBO(CurrentUserSession.NetSessionId);
                    accountInfo.UpdateCasinoStatus(custId, (bool)(outputs["disablecasino"]), true, true, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
                }

            }
            else
            {
                customer.RemoveAccount(custInfo.UserName, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), CurrentUserSession.CustId, CurrentUserSession.UserName, FromSystem);
            }

            return result;
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            int.TryParse(Request["custid"], out custId);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (custId <= 0)
            {
                return new Result(ErrorType.Unknown, string.Empty);
            }

            return null;
        }
    }
}