<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers.Agent.AutoInsertAccountVirtualSportsFromExistingMember_Handler" %>
/*
 * Created 20120824@Jeff - auto insert account Virtual Sports from exist
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers.Agent
{
    using System;
    using System.Web;
    using System.Linq;

    using Fanex.Age.UI;
    using Sunplus.Agent.Utilities;
    using Sunplus.Agent.Model.Entities;
    using Fanex.PTEngine.BusinessObjects;
    using Fanex.PTVirtualSports.Services;
    using System.Collections.Generic;
    using System.Web.SessionState;
    using System.Collections;
    using Microsoft.Practices.Unity;
    using Sunplus.Agent.Model.Entities.Permission;

    public class AutoInsertAccountVirtualSportsFromExistingMember_Handler : TplHandler, IReadOnlySessionState
    {
        int custId;
        int custIdOld;
        string errMsg = string.Empty;
        int errCode = (int)ErrorType.Success;

        public override void RenderResult(ref JSONData data)
        {
            Fanex.Age.UI.Result r = View as Fanex.Age.UI.Result;
            data.AsResult(r);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            if (!UserPermission.IsShowVirtualSports)
            {
                return new Fanex.Age.UI.Result(ErrorType.Unknown, string.Empty);
            }

            try
            {
                IRequestContext oldCustomerContext = new RequestContext
                {
                    CustId = CurrentUserSession.CustId,
                    TargetCustId = custIdOld,
                    TargetRoleId = 1,
                    FormId = FormType.EditPT,
                    SubAccId = CurrentUserSession.SubAccId,
                    AdminId = CurrentUserSession.AdminId,
                    Language = CurrentUserSession.LangKey,
                    LangKey = CurrentUserSession.LangKey,
                    IP = Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                    RoleId = 2
                };

                IRequestContext currentContext = new RequestContext
                {
                    CustId = CurrentUserSession.CustId,
                    TargetCustId = custId,
                    TargetRoleId = 1,
                    FormId = FormType.EditPT,
                    SubAccId = CurrentUserSession.SubAccId,
                    AdminId = CurrentUserSession.UserId,
                    Language = CurrentUserSession.LangKey,
                    LangKey = CurrentUserSession.LangKey,
                    IP = Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                    RoleId = 2
                };

                CustomerService customerService = new CustomerService();
                var oldCustomer = customerService.GetCustomer(oldCustomerContext);
                CheckConstraintData(ref oldCustomer);

                var results = customerService.SaveCustomer(null, oldCustomer, currentContext, null);
            }
            catch (Exception ex)
            {
                Logs.Write(ex.Message);
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

        private void CheckConstraintData(ref ICustomer oldCustomerSettings)
        {
            //check status
            oldCustomerSettings.StatusSetting.DisableVirtualSports = oldCustomerSettings.StatusSetting.DisableUplineVirtualSports;

            //check betting limit
            var index = 0;
            if (oldCustomerSettings.BettingLimit != null)
            {
                foreach (BettingLimitItem bettingLimitItem in oldCustomerSettings.BettingLimit.BettingLimitItems)
                {
                    oldCustomerSettings.BettingLimit.BettingLimitItems[index].MinBet = bettingLimitItem.MinBet > bettingLimitItem.MinBetMax ?
                                                                                        bettingLimitItem.MinBet : bettingLimitItem.MinBetMax;
                    oldCustomerSettings.BettingLimit.BettingLimitItems[index].MaxBet = bettingLimitItem.MaxBet < bettingLimitItem.MaxBetMin ?
                                                                                        bettingLimitItem.MaxBet : bettingLimitItem.MaxBetMin;
                    oldCustomerSettings.BettingLimit.BettingLimitItems[index].MaxPerMatch = bettingLimitItem.MaxPerMatch < bettingLimitItem.MaxPerMatchMax ?
                                                                                            bettingLimitItem.MaxPerMatch : bettingLimitItem.MaxPerMatchMax;
                    index++;
                }
            }

            //check PT
            var sportTypeIndex = 0;
            foreach (SportType sport in oldCustomerSettings.PtSetting.SportTypes)
            {
                var bettypeGroupSetIndex = 0;
                foreach (BetTypeGroupSet bettypeGroupSet in sport.BetTypeGroupSets)
                {
                    var bettypeGroupIndex = 0;
                    foreach (BetTypeGroup bettypeGroup in bettypeGroupSet.BetTypeGroups)
                    {
                        var bettypeIndex = 0;
                        foreach (BetType bettype in bettypeGroup.BetTypes)
                        {
                            oldCustomerSettings.PtSetting.SportTypes[sportTypeIndex].BetTypeGroupSets[bettypeGroupSetIndex].BetTypeGroups[bettypeGroupIndex].BetTypes[bettypeIndex].Pt.A1 =
                                                                                                                        bettype.Pt.A1 < bettype.Pt.A2 ? bettype.Pt.A1 : bettype.Pt.A2;
                            bettypeIndex++;
                        }

                        bettypeGroupIndex++;
                    }

                    bettypeGroupSetIndex++;
                }

                sportTypeIndex++;
            }
        }
    }
}