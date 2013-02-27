<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers.Agent.AutoInsertAccountBingoFromExistingMember_Handler" %>
/*
 * Created 20120725@Jeff - auto insert account Bingo from exist
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers.Agent
{
    using System;
    using System.Web;
    using System.Linq;

    using LiveCasinoLibrary;
    using Fanex.Age.UI;
    using Sunplus.Agent.Utilities;
    using Sunplus.Agent.Model.BL.AccountInfo;
    using Sunplus.Agent.Model.BL.Authorization;
    using Sunplus.Agent.Model.Entities;
    using System.Collections.Generic;
    using Sunplus.Bingo.Entities;
    using Sunplus.Bingo.Core;
    using Sunplus.Bingo.Services;
    using System.Collections;
    using Microsoft.Practices.Unity;

    public class AutoInsertAccountBingoFromExistingMember_Handler : TplHandler, System.Web.SessionState.IReadOnlySessionState
    {
        string SiteNo = BingoSettings.BingoGame.CodeGameNo;
        int custId;
        int custIdOld;
        int FromSystem = 2; // Agent
        string errMsg = string.Empty;
        int errCode = (int)ErrorType.Unknown;

        Model.Entities.CustomerInfo custInfo;

        public override void RenderResult(ref JSONData data)
        {
            Fanex.Age.UI.Result r = View as Fanex.Age.UI.Result;
            data.AsResult(r);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            Fanex.Age.UI.Result result = new Fanex.Age.UI.Result(ErrorType.Unknown, string.Empty);

            if (!CurrentUserSession.IsBingoSync) // Don't call auto insert bingo account
            {
                return new Fanex.Age.UI.Result(101, errMsg);
            }

            AccountInfoBO accountInfo = new AccountInfoBO(CurrentUserSession.NetSessionId);

            custInfo = accountInfo.GetCustInfo(custId);

            // Get status old user
            Hashtable statuses = new Hashtable();
            AuthorizationBO authorizationBO = new AuthorizationBO(CurrentUserSession.NetSessionId);
            authorizationBO.GetCustomerStatus(custIdOld, 0, ref statuses);

            if (custInfo == null || !(bool)(statuses["syncbingo"])) // Haven't been replicated yet
            {
                return new Fanex.Age.UI.Result(102, errMsg);
            }

            LiveCasinoLibrary.SBAgent.Customer customer = new LiveCasinoLibrary.SBAgent.Customer();

            ICustomerService customerService = Sunplus.Bingo.Core.BingoContainer.Current.Resolve<ICustomerService>();
            IResult outputs = new Sunplus.Bingo.Entities.Result();
            Customer customerSettingOld = customerService.GetCustomerSetting(custIdOld, UserSession.Current.RoleId, ref outputs) as Customer;

            customerSettingOld.ID = custId;

            customerSettingOld.SyncBingo = false;
            customerSettingOld.IsEnabledBingo = (bool)(statuses["disablebingo"]);
            customerSettingOld.UserName = custInfo.UserName;

            AnalyzeGameKindSetting(customerSettingOld);
            outputs = customerService.UpdateCustomerSetting(customerSettingOld, CurrentUserSession.RoleId, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), CurrentUserSession.UserId, CurrentUserSession.UserName, 2) as Sunplus.Bingo.Entities.Result;

            errCode = outputs.ErrorCode;
            errMsg = outputs.ErrorMessage;

            if (errCode == 0)
            {
                if (statuses.Count > 0)
                {
                    // Update bingo status
                    result.ErrCode = accountInfo.UpdateBingoStatus(custId, (bool)(statuses["disablebingo"]), true, true, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), ref result.ErrMsg);
                }
            }

            return new Fanex.Age.UI.Result(errCode, errMsg);
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
                return new Fanex.Age.UI.Result(ErrorType.Unknown, string.Empty);
            }

            return null;
        }

        private void AnalyzeGameKindSetting(Customer customerOld)
        {
            int gameKindCount = 0;
            // Process each game kind
            foreach (GameKind gameKind in customerOld.GameKindList)
            {
                customerOld.GameKindList[gameKindCount].PositionTaking = gameKind.PositionTaking < gameKind.PositionTakingMax ? gameKind.PositionTaking : gameKind.PositionTakingMax;
                gameKindCount++;
            }
        }

    }
}