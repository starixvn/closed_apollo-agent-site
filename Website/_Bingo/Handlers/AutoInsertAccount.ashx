<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._Bingo.AutoInsertAccount_Handler" %>

using System;
using System.Web;

using LiveCasinoLibrary;
using Sunplus.Agent.Model.BL.Casino;

using Fanex.Age.UI;
using Sunplus.Agent.Utilities;
using Sunplus.Agent.Model.BL.AccountInfo;
using Sunplus.Agent.Model.BL.Authorization;
using System.Collections;

namespace Sunplus.Agent.UI._Bingo
{
    public class AutoInsertAccount_Handler : Fanex.Age.UI.TplHandler, System.Web.SessionState.IReadOnlySessionState
    {
        string SiteNo = Sunplus.Bingo.Core.BingoSettings.BingoGame.CodeGameNo;
        int custId;
        int FromSystem = 2; // Agent

        Model.Entities.CustomerInfo custInfo;

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            Result result = View as Result;
            data.AsResult(result);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            Result result = new Result(ErrorType.Unknown, "Unknown");

            if (!CurrentUserSession.IsBingoSync) // Don't call auto insert casino account
            {
                result.ErrMsg = "Upline is not a Bingo user!";
                return result;
            }

            AccountInfoBO accountInfo = new AccountInfoBO(CurrentUserSession.NetSessionId);

            custInfo = accountInfo.GetCustInfo(custId);

            if (custInfo == null) // Haven't been replicated yet
            {
                result.ErrMsg = "Data is not ready yet!";
                return result;
            }

            bool isSuccess = false;

            LiveCasinoLibrary.SBAgent.Customer customer = new LiveCasinoLibrary.SBAgent.Customer();

            try
            {
                isSuccess = customer.AutoInsertAccount(
                    SiteNo,
                    custInfo.IBCCustID,
                    custInfo.UserName,
                    custInfo.SIBCCustID,
                    custInfo.MIBCCustID,
                    custInfo.AIBCCustID,
                    custInfo.RoleId.ToString(),
                    custInfo.Currency,
                    DateTime.Now,
                    Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                    CurrentUserSession.CustId,
                    CurrentUserSession.UserName,
                    FromSystem
                    );
            }
            catch (Exception e)
            {
                result.ErrMsg = e.Message;
                Logs.Write(e.ToString());
            }

            if (isSuccess) // Auto insert bingo account successful
            {

                // Get Status Current user. Check data concurrent update
                Hashtable outputs = new Hashtable();
                AuthorizationBO authorizationBO = new AuthorizationBO(CurrentUserSession.NetSessionId);
                authorizationBO.GetCustomerStatus(CurrentUserSession.CustId, 0, ref outputs);

                if (outputs.Count > 0)
                {
                    // Update syncs status
                    result.ErrCode = accountInfo.UpdateBingoStatus(custId, (bool)(outputs["disablebingo"]), true, true, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), ref result.ErrMsg);
                }

                if (result.ErrCode != 0)
                {
                    result.ErrMsg = "Update Bingo status failed!";
                }
            }
            else
            {
                result.ErrMsg = "Auto insert bingo setting failed!";
                // Bingo don't have RemoveAccount API as RNG Casino because it used replication mechanism
                //customer.RemoveAccount(custInfo.UserName, Request.UserHostAddress, CurrentUserSession.CustId, CurrentUserSession.UserName, FromSystem);
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