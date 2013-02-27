<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers.AutoInsertAccountCSFromExisting_Handler" %>

using System;
using System.Web;

using CasinoLibrary;
using Sunplus.Agent.Model.BL.Casino;

using DAL;
using BLL;

using Fanex.Age.UI;
using Sunplus.Agent.Utilities;
using Sunplus.Agent.Model.BL.AccountInfo;

namespace Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers
{
    public class AutoInsertAccountCSFromExisting_Handler : TplHandler, System.Web.SessionState.IReadOnlySessionState
    {
        string CasinoNo = Globals.CASINO_GROUP1;
        int custId;
        int custIdOld;
        int FromSystem = 2; // Agent
        string errMsg = string.Empty;
        int errCode = (int)ErrorType.Unknown;

        CSCustomerDetail custInfo;
        CSCustomerDetail custInfoOld;

        public override void RenderResult(ref JSONData data)
        {
            Result r = View as Result;
            data.AsResult(r);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            if (!CurrentUserSession.IsCasinoSync) // Don't call auto insert casino account
            {
                return new Result(101, errMsg);
            }

            custInfo = CasinoManager.GetCustInfoIBC(custId);
            custInfoOld = CasinoManager.GetCustInfoIBC(custIdOld);

            if (custInfo == null || custInfoOld == null || !custInfoOld.CasinoSync) // Haven't been replicated yet
            {
                return new Result(102, errMsg);
            }

            bool isSuccess = false;

            CasinoLibrary.SBAgent.Customer customer = new CasinoLibrary.SBAgent.Customer();

            try
            {
                isSuccess = customer.AutoInsertAccountFromExisting(
                    CasinoNo,
                    custInfo.IBCCustID,
                    custInfoOld.IBCCustID,
                    custInfo.UserName,
                    custInfo.UserName2,
                    custInfo.FirstName,
                    custInfo.LastName,
                    custInfo.UserPwd,
                    custInfo.Phone,
                    custInfo.MobilePhone,
                    custInfo.Fax,
                    DateTime.Now,
                    Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                    CurrentUserSession.CustId,
                    CurrentUserSession.UserName,
                    FromSystem
                    );
            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            if (isSuccess) // Auto insert casino account successful
            {
                errCode = (int)(ErrorType.Success);
                // Update syncs status
                AccountInfoBO accountInfo = new AccountInfoBO(CurrentUserSession.NetSessionId);
                accountInfo.UpdateCasinoStatus(custInfo.IBCCustID, custInfo.CasinoDisabled, true, true, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
            }
            else
            {
                customer.RemoveAccount(custInfo.UserName, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), CurrentUserSession.CustId, CurrentUserSession.UserName, FromSystem);

                return new Result(103, errMsg);
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