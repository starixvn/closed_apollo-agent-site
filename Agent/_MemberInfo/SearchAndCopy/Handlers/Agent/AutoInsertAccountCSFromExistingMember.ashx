<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers.Agent.AutoInsertAccountCSFromExistingMember_Handler" %>

using System;
using System.Web;
using System.Data;
using System.Collections.Generic;

using CasinoLibrary;
using CasinoLibrary.SBAgent;
using Sunplus.Agent.Model.BL.Casino;

using DAL;
using BLL;

using Fanex.Age.UI;
using Sunplus.Agent.Utilities;
using Sunplus.Agent.Model.BL.AccountInfo;

namespace Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers.Agent
{
    public class AutoInsertAccountCSFromExistingMember_Handler : TplHandler, System.Web.SessionState.IReadOnlySessionState
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
            bool isSuccess = false;
            Customer customer = new Customer();

            if (!CurrentUserSession.IsCasinoSync) // Don't call auto insert casino account
            {
                return new Result(101, errMsg);
            }
            try
            {
                custInfo = CasinoManager.GetCustInfoIBC(custId);
                custInfoOld = CasinoManager.GetCustInfoIBC(custIdOld);
                CasinoManager.GetCustSettingCS(ref custInfoOld);

                if (custInfo == null || custInfoOld == null || !custInfoOld.CasinoSync) // Haven't been replicated yet
                {
                    return new Result(102, errMsg);
                }

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
                errMsg = e.Message;
            }

            if (isSuccess) // Auto insert casino account successful
            {
                errCode = (int)(ErrorType.Success);

                UpdateStatus(custInfoOld.CasinoDisabled);
                UpdatePositionTaking(custInfoOld);

            }
            else
            {
                // Update syncs status
                AccountInfoBO accountInfo = new AccountInfoBO(CurrentUserSession.NetSessionId);
                accountInfo.UpdateCasinoStatus(custInfo.IBCCustID, CurrentUserSession.IsCasinoDisabled, false, false, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
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

        private void UpdateStatus(bool IsCasinoDisabled)
        {
            AccountInfoBO accountInfo = new AccountInfoBO(CurrentUserSession.NetSessionId);
            accountInfo.UpdateCasinoStatus(custInfo.IBCCustID, IsCasinoDisabled, true, true, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
        }

        private void UpdatePositionTaking(CSCustomerDetail custInfo)
        {
            DataTable dtGameKinds = GameKindsDataTable();
            DataTable dtGameTypesStake = GameTypesStakesDataTable();
            DataTable dtGameKindsReturn = null;
            DataTable dtGameTypesReturn = null;
            bool status = false;

            List<GameKindDetail> lsGameKind = (custInfo.CasinoSync) ? CasinoManager.GetGameKinds(custInfo.IBCCustID) : CasinoManager.GetGameKinds(Sunplus.Agent.Model.Entities.UserSession.Current.CustId);
            foreach (GameKindDetail gameKindDetail in lsGameKind)
            {
                dtGameKinds.Rows.Add(new object[] {
                    custId,
                    gameKindDetail.CodeID,
                    null,
                    null,
                    null,
                    (gameKindDetail.MaxPositionTakingUpline < gameKindDetail.APositionTaking)? gameKindDetail.MaxPositionTakingUpline : gameKindDetail.APositionTaking,
                    gameKindDetail.IsAutoPositionTaking,
                    !gameKindDetail.IsAutoPositionTaking? null : (object)gameKindDetail.AutoPositionTaking,
                    null,
                    0,
                    null,
                    null,
                    null,
                    gameKindDetail.MemberRollingRate,
                    custInfo.RoleId,
                    Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                    Sunplus.Agent.Model.Entities.UserSession.Current.CustId, Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId == 0 ? Sunplus.Agent.Model.Entities.UserSession.Current.UserName : Sunplus.Agent.Model.Entities.UserSession.Current.SubUserName, 2
                });
                List<GameTypeDetail> lsGameType = (custInfo.CasinoSync) ? CasinoManager.GetGameType(custInfo.IBCCustID, gameKindDetail.CodeID) : CasinoManager.GetGameType(gameKindDetail.CodeID);
                if (lsGameType != null && lsGameType.Count > 0)
                {
                    foreach (var item in lsGameType)
                    {
                        string selectedStakeLevels = string.Empty;
                        List<StakeLevelDetail> lsStakeLevel = new List<StakeLevelDetail>();
                        lsStakeLevel = (custInfo.CasinoSync) ? CasinoManager.GetCustStakeLevels(custInfo.IBCCustID, item.GameTypeID) : CasinoManager.GetCustStakeLevels(Sunplus.Agent.Model.Entities.UserSession.Current.CustId, item.GameTypeID);
                        foreach (var betItem in lsStakeLevel)
                        {
                            if (betItem.IsChecked)
                            {
                                selectedStakeLevels += betItem.StakeLevelID + ",";
                            }
                        }
                        if (selectedStakeLevels.EndsWith(","))
                        {
                            selectedStakeLevels = selectedStakeLevels.Remove(selectedStakeLevels.LastIndexOf(","), 1);
                        }
                        dtGameTypesStake.Rows.Add(new object[] {
                        custId,
                        item.GameTypeID,
                        selectedStakeLevels,
                        custInfo.RoleId,
                        Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                        Sunplus.Agent.Model.Entities.UserSession.Current.CustId, Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId == 0 ? Sunplus.Agent.Model.Entities.UserSession.Current.UserName : Sunplus.Agent.Model.Entities.UserSession.Current.SubUserName, 2
                    });

                    }
                }
            }

            CasinoLibrary.SBAgent.Customer csCustomer = new CasinoLibrary.SBAgent.Customer();
            status = csCustomer.UpdateMaxTransferMisc(
                     custId,
                     !custInfo.CasinoDisabled,
                     true,
                     true,
                     (decimal)custInfo.MaxTransferCredit, //max transfer credit
                     custInfo.IsAutoReset,
                     custInfo.PTByGameType,
                     Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                     Sunplus.Agent.Model.Entities.UserSession.Current.CustId,
                     Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId == 0 ? Sunplus.Agent.Model.Entities.UserSession.Current.UserName : Sunplus.Agent.Model.Entities.UserSession.Current.SubUserName,
                     2);
            dtGameKindsReturn = csCustomer.UpdateCustGameKind(dtGameKinds);
            dtGameTypesReturn = csCustomer.UpdateCustGameTypeStakeLevel(dtGameTypesStake);
        }

        private DataTable GameKindsDataTable()
        {
            DataTable dtGameKinds = new DataTable();
            dtGameKinds.Columns.AddRange(new DataColumn[] {
            new DataColumn("IBCCustID"),
            new DataColumn("GameKind"),
            new DataColumn("HPositionTaking"),
            new DataColumn("SPositionTaking"),
            new DataColumn("MPositionTaking"),
            new DataColumn("APositionTaking"),
            new DataColumn("IsAutoPositionTaking"),
            new DataColumn("AutoPositionTaking"),
            new DataColumn("MinPositionTaking"),
            new DataColumn("MaxPositionTaking"),
            new DataColumn("SMAgentRollingRate"),
            new DataColumn("MAgentRollingRate"),
            new DataColumn("AgentRollingRate"),
            new DataColumn("MemberRollingRate"),
            new DataColumn("RoleId"),
            new DataColumn("FromIP"),
            new DataColumn("ModifyBy"), new DataColumn("ModifyUserName"), new DataColumn("FromSystem")
        });

            return dtGameKinds;
        }

        private DataTable GameTypesStakesDataTable()
        {
            DataTable dtGameTypesStake = new DataTable();
            dtGameTypesStake.Columns.AddRange(new DataColumn[] {
            new DataColumn("IBCCustID"),
            new DataColumn("GameTypeID"),
            new DataColumn("StakeLevelList"),
            new DataColumn("RoleId"),
            new DataColumn("FromIP"),
            new DataColumn("ModifyBy"), new DataColumn("ModifyUserName"), new DataColumn("FromSystem")
        });

            return dtGameTypesStake;
        }
    }
}