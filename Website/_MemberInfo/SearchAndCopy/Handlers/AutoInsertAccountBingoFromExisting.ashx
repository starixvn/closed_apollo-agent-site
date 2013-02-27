<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers.AutoInsertAccountBingoFromExisting_Handler" %>

using System;
using System.Web;

using LiveCasinoLibrary;
using Sunplus.Agent.Model.BL.Casino;

using DAL;
using BLL;

using Fanex.Age.UI;
using Sunplus.Agent.Utilities;
using Sunplus.Agent.Model.BL.AccountInfo;

namespace Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers
{
    public class AutoInsertAccountBingoFromExisting_Handler : TplHandler, System.Web.SessionState.IReadOnlySessionState
    {
        string SiteNo = Sunplus.Bingo.Core.BingoSettings.BingoGame.CodeGameNo;
        int custId = 0;
        int custIdOld = 0;
        int FromSystem = 2; // Agent
        string errMsg = string.Empty;
        int errCode = (int)ErrorType.Unknown;

        Model.Entities.CustomerInfo custInfo;
        Model.Entities.CustomerInfo custInfoOld;

        public override void RenderResult(ref JSONData data)
        {
            Result r = View as Result;
            data.AsResult(r);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            if (!CurrentUserSession.IsBingoSync) // Don't call auto insert casino account
            {
                return new Result(101, errMsg);
            }

            AccountInfoBO accountInfo = new AccountInfoBO(CurrentUserSession.NetSessionId);

            custInfoOld = accountInfo.GetCustInfo(custIdOld);

            LiveCasinoLibrary.SBAgent.Customer customer = new LiveCasinoLibrary.SBAgent.Customer();
            bool isSuccess = false;

            if (custInfoOld == null)
            {
                return new Result(102, errMsg);
            }

            // Always auto sync bingo if upline was sync
            try
            {
                if (custInfoOld.BingoSync)
                {
                    custInfo = accountInfo.GetCustInfo(custId);

                    isSuccess = customer.AutoInsertAccountFromExisting(
                        SiteNo,
                        custId,
                        custInfoOld.IBCCustID,
                        custInfo.UserName,
                        DateTime.Now,
                        Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                        CurrentUserSession.CustId,
                        CurrentUserSession.UserName,
                        FromSystem
                        );
                }
                else
                {
                    Server.Transfer("~/_Bingo/Handlers/AutoInsertAccount.ashx?custid=" + custId);
                }
            }
            catch (Exception e)
            {
                Logs.Write(e.ToString());
                errMsg = e.Message;
            }

            if (isSuccess) // Auto insert bingo account successful
            {
                errCode = (int)(ErrorType.Success);
                // Update bingo status
                accountInfo.UpdateBingoStatus(custInfo.IBCCustID, custInfo.BingoDisabled, true, true, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), ref errMsg);
            }
            else
            {
                return new Result(103, "Auto insert bingo setting failed!");
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
            if (custId <= 0 || custIdOld <= 0)
            {
                return new Result(ErrorType.Unknown, string.Empty);
            }

            return null;
        }
    }
}