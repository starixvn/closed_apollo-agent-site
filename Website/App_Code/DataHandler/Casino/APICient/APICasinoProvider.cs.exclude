/// <summary>
/// Summary description for APICasinoProvider
/// </summary>
namespace DAL.APIClient
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Web;
    using CasinoLibrary.SBAgent;
    using DNAide.Core;
    using Fanex.Data.DNAide;

    public class APICasinoProvider : CasinoProvider
    {
        public APICasinoProvider()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        //Call from Casino API to get the game kind list and assign to List<GameKindDetail>
        public override List<GameKindDetail> GetGameKinds()
        {
            List<GameKindDetail> lsGameKind = new List<GameKindDetail>();
            BasicInfo _csBasicInfo = new BasicInfo();
            DataTable dtGameKind = _csBasicInfo.GetGameKind(null);
            foreach (DataRow row in dtGameKind.Rows)
            {
                GameKindDetail gameKind = new GameKindDetail(CastAide.AsInt32(row["CodeID"]), CastAide.AsString(row["CodeNo"]), CastAide.AsString(row[DAL.APIClient.APICasinoProvider.Lang("CodeName")]), CastAide.AsString(row["CodeSName"]));
                lsGameKind.Add(gameKind);
            }

            return lsGameKind;
        }

        public override bool GetCasinoStatus(int custid)
        {
            Hashtable param = new Hashtable();
            param.Add("custid", custid);

            DbResult drCustomer = DNAide.Data.DbHelper.Execute("Age_GetCSStatus", param, 300);
            return CastAide.AsBoolean(drCustomer.Outputs["status"]);
        }

        public override bool UpdateCSStatus(int custid, bool bSyncCS, bool bDisabledCS)
        {
            Hashtable param = new Hashtable();
            param.Add("custid", custid);
            param.Add("disabledcs", bDisabledCS);
            param.Add("synccs", bSyncCS);
            param.Add("enabledownline", false);

            param.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);
            param.Add("subaccid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId);
            param.Add("loginip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
            param.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);

            DbResult drReturn = DNAide.Data.DbHelper.Execute("Acc_UpdCSStatus", param, 300);

            bool status = CastAide.AsBoolean(drReturn.Outputs["status"]);

            if (!status) return false;

            CasinoLibrary.SBAgent.Customer cust = new CasinoLibrary.SBAgent.Customer();
            return cust.UpdateCustEnableCasino(custid, !bDisabledCS, true, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), Sunplus.Agent.Model.Entities.UserSession.Current.CustId, Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId == 0 ? Sunplus.Agent.Model.Entities.UserSession.Current.Username : Sunplus.Agent.Model.Entities.UserSession.Current.SubUserName, 2);
        }

        public override bool UpdateCSStatus(int custid, bool bSyncCS, bool bDisabledCS, bool isUpdDownline)
        {
            string errMsg = "";
            return UpdateCSStatus(custid, bSyncCS, bDisabledCS, isUpdDownline, out errMsg);
        }

        public override bool UpdateCSStatus(int custid, bool bSyncCS, bool bDisabledCS, bool isUpdDownline, out string errMsg)
        {
            bool status = DoUpdateCSStatusWithoutSync(custid, bSyncCS, bDisabledCS, isUpdDownline, out errMsg);
            if (!status) return false;

            CasinoLibrary.SBAgent.Customer cust = new CasinoLibrary.SBAgent.Customer();
            return cust.UpdateCustEnableCasino(custid, !bDisabledCS, isUpdDownline, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), Sunplus.Agent.Model.Entities.UserSession.Current.CustId, Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId == 0 ? Sunplus.Agent.Model.Entities.UserSession.Current.Username : Sunplus.Agent.Model.Entities.UserSession.Current.SubUserName, 2);
        }

        public override bool UpdateCSStatusWithoutSync(int custid, bool bSyncCS, bool bDisabledCS, bool isUpdDownline)
        {
            string errMsg = "";
            return DoUpdateCSStatusWithoutSync(custid, bSyncCS, bDisabledCS, isUpdDownline, out errMsg);
        }

        private static bool DoUpdateCSStatusWithoutSync(int custid, bool bSyncCS, bool bDisabledCS, bool isUpdDownline, out string errMsg)
        {
            Hashtable param = new Hashtable();
            param.Add("custid", custid);
            param.Add("disabledcs", bDisabledCS);
            param.Add("synccs", bSyncCS);
            param.Add("enabledownline", isUpdDownline);

            param.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);
            param.Add("subaccid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId);
            param.Add("loginip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
            param.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);

            DbResult drReturn = DNAide.Data.DbHelper.Execute("Acc_UpdCSStatus", param, 300);

            errMsg = drReturn.Outputs["errmsg"].ToString();
            return CastAide.AsBoolean(drReturn.Outputs["status"]);
        }

        /// <summary>
        /// Initialize basics property of an object from posted values of a form. Be used in new customer function.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override CSCustomerDetail GetCustInfoIBC(string custid, Hashtable r)
        {
            CSCustomerDetail _custSetting = new CSCustomerDetail();

            _custSetting.UserName = CastAide.AsString(r["username"]);
            _custSetting.UserPwd = CastAide.AsString(r["userpwd"]);
            _custSetting.FirstName = CastAide.AsString(r["firstname"]);
            _custSetting.LastName = CastAide.AsString(r["lastname"]);
            _custSetting.Phone = CastAide.AsString(r["phone"]);
            _custSetting.MobilePhone = CastAide.AsString(r["mobilephone"]);
            _custSetting.Fax = CastAide.AsString(r["fax"]);
            _custSetting.Email = CastAide.AsString(r["email"]);
            _custSetting.RoleId = Sunplus.Agent.Model.Entities.UserSession.Current.RoleId - 1;
            _custSetting.Currency = Sunplus.Agent.Model.Entities.UserSession.Current.CurrencyId;
            _custSetting.CurrencyName = Sunplus.Agent.Model.Entities.UserSession.Current.Currency;
            _custSetting.Closed = CastAide.AsBoolean(r["closed"]);
            _custSetting.Suspend = CastAide.AsBoolean(r["suspend"]);

            _custSetting.SIBCCustID = Sunplus.Agent.Model.Entities.UserSession.Current.RoleId == 4 ? Sunplus.Agent.Model.Entities.UserSession.Current.CustId : 0;
            _custSetting.MIBCCustID = Sunplus.Agent.Model.Entities.UserSession.Current.RoleId == 3 ? Sunplus.Agent.Model.Entities.UserSession.Current.CustId : 0;
            _custSetting.AIBCCustID = Sunplus.Agent.Model.Entities.UserSession.Current.RoleId == 2 ? Sunplus.Agent.Model.Entities.UserSession.Current.CustId : 0;

            _custSetting.CasinoSync = false;
            _custSetting.CasinoDisabled = false;
            _custSetting.IBCCustID = CastAide.AsInt32(custid);

            return _custSetting;
        }

        public override List<GameKindDetail> GetGameKinds(string casinoNo)
        {
            List<GameKindDetail> lsGameKind = new List<GameKindDetail>();
            BasicInfo _csBasicInfo = new BasicInfo();
            DataTable dtGameKind = _csBasicInfo.GetGameKind(casinoNo);
            foreach (DataRow row in dtGameKind.Rows)
            {
                GameKindDetail gameKind = new GameKindDetail(CastAide.AsInt32(row["CodeID"]), CastAide.AsString(row["CodeNo"]), CastAide.AsString(row[DAL.APIClient.APICasinoProvider.Lang("CodeName")]), CastAide.AsString(row["CodeSName"]));
                lsGameKind.Add(gameKind);
            }

            return lsGameKind;
        }

        public override List<GameKindDetail> GetGameKinds(int custId)
        {
            List<GameKindDetail> lsGameKind = new List<GameKindDetail>();
            CasinoLibrary.SBAgent.Customer _cust = new CasinoLibrary.SBAgent.Customer();
            DataTable dtGameKind = _cust.GetCustGameKind(custId);
            foreach (DataRow row in dtGameKind.Rows)
            {
                GameKindDetail gameKind = new GameKindDetail();
                gameKind.CodeID = CastAide.AsInt32(row["GameKind"]);
                gameKind.CodeName = CastAide.AsString(row[Lang("GameKindName")]);
                gameKind.AgentRollingRate = CastAide.AsDouble(row["AgentRollingRate"]);
                gameKind.APositionTaking = CastAide.AsDouble(row["APositionTaking"]);
                gameKind.AutoPositionTaking = CastAide.AsDouble(row["AutoPositionTaking"]);
                gameKind.IsAutoPositionTaking = CastAide.AsBoolean(row["IsAutoPositionTaking"]);
                gameKind.MAgentRollingRate = CastAide.AsDouble(row["MAgentRollingRate"]);
                gameKind.MaxPositionTaking = CastAide.AsDouble(row["MaxPositionTaking"]);
                gameKind.MemberRollingRate = CastAide.AsDouble(row["MemberRollingRate"]);
                // gameKind.MinMPositionTaking = CastAide.AsDouble(row["MinMPositionTaking"]);
                gameKind.MinPositionTaking = CastAide.AsDouble(row["MinPositionTaking"]);
                gameKind.MPositionTaking = CastAide.AsDouble(row["MPositionTaking"]);
                gameKind.SMAgentRollingRate = CastAide.AsDouble(row["SMAgentRollingRate"]);
                gameKind.SPositionTaking = CastAide.AsDouble(row["SPositionTaking"]);
                gameKind.SuperSPositionTaking = CastAide.AsDouble(row["SuperSPositionTaking"]);

                gameKind.AgentRollingRateUpline = CastAide.AsDouble(row["UplineAgentRollingRate"]);
                gameKind.APositionTakingUpline = CastAide.AsDouble(row["UplineAPositionTaking"]);
                gameKind.AutoPositionTakingUpline = CastAide.AsDouble(row["UplineAutoPositionTaking"]);
                gameKind.IsAutoPositionTakingUpline = CastAide.AsBoolean(row["UplineIsAutoPositionTaking"]);
                gameKind.MAgentRollingRateUpline = CastAide.AsDouble(row["UplineMAgentRollingRate"]);
                gameKind.MaxPositionTakingUpline = CastAide.AsDouble(row["UplineMaxPositionTaking"]);
                gameKind.MemberRollingRateUpline = CastAide.AsDouble(row["UplineMemberRollingRate"]);
                // gameKind.MinMPositionTakingUpline = CastAide.AsDouble(row["MinMPositionTaking"]);
                gameKind.MinPositionTakingUpline = CastAide.AsDouble(row["UplineMinPositionTaking"]);
                gameKind.MPositionTakingUpline = CastAide.AsDouble(row["UplineMPositionTaking"]);
                gameKind.SMAgentRollingRateUpline = CastAide.AsDouble(row["UplineSMAgentRollingRate"]);
                gameKind.SPositionTakingUpline = CastAide.AsDouble(row["UplineSPositionTaking"]);

                lsGameKind.Add(gameKind);
            }

            return lsGameKind;
        }

        /// <summary>
        /// Get customer PT when super edit member
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public override List<GameKindDetail> GetGameKinds_EditMember(int memberId)
        {
            List<GameKindDetail> lsGameKind = new List<GameKindDetail>();
            CasinoLibrary.SBAgent.Customer _cust = new CasinoLibrary.SBAgent.Customer();
            DataTable dtGameKind = _cust.GetCustGameKind_EditMember(memberId);
            foreach (DataRow row in dtGameKind.Rows)
            {
                GameKindDetail gameKind = new GameKindDetail();
                gameKind.CodeID = CastAide.AsInt32(row["GameKind"]);
                gameKind.CodeName = CastAide.AsString(row[Lang("GameKindName")]);
                gameKind.APositionTaking = CastAide.AsDouble(row["APositionTaking"]);
                gameKind.AutoPositionTaking = CastAide.AsDouble(row["AutoPositionTaking"]);
                gameKind.IsAutoPositionTaking = CastAide.AsBoolean(row["IsAutoPositionTaking"]);
                gameKind.MPositionTaking = CastAide.AsDouble(row["MPositionTaking"]);
                gameKind.SPositionTaking = CastAide.AsDouble(row["SPositionTaking"]);
                gameKind.SuperMaxPositionTaking = CastAide.AsDouble(row["SuperMaxPositionTaking"]);
                gameKind.SuperMinPositionTaking = CastAide.AsDouble(row["SuperMinPositionTaking"]);

                lsGameKind.Add(gameKind);
            }

            return lsGameKind;
        }

        //Get customer PT by game type when super edit member
        public override List<GameTypeDetail> GetGameTypes_EditMember(int memberId, int gameKindID)
        {
            List<GameTypeDetail> lsGameType = new List<GameTypeDetail>();
            CasinoLibrary.SBAgent.Customer _cust = new CasinoLibrary.SBAgent.Customer();
            DataTable dtGameType = _cust.GetCustGameType_EditMember(memberId, gameKindID);
            foreach (DataRow row in dtGameType.Rows)
            {
                GameTypeDetail gameType = new GameTypeDetail();

                gameType.GameTypeID = CastAide.AsInt32(row["GameTypeID"]);
                gameType.GameTypeName = CastAide.AsString(row[Lang("GameTypeName")]);
                gameType.APositionTaking = CastAide.AsDouble(row["APositionTaking"]);
                gameType.AutoPositionTaking = CastAide.AsDouble(row["AutoPositionTaking"]);
                gameType.IsAutoPositionTaking = CastAide.AsBoolean(row["IsAutoPositionTaking"]);
                gameType.MPositionTaking = CastAide.AsDouble(row["MPositionTaking"]);
                gameType.SPositionTaking = CastAide.AsDouble(row["SPositionTaking"]);
                gameType.SuperMaxPositionTaking = CastAide.AsDouble(row["SuperMaxPositionTaking"]);
                gameType.SuperMinPositionTaking = CastAide.AsDouble(row["SuperMinPositionTaking"]);

                lsGameType.Add(gameType);
            }
            return lsGameType;
        }

        /// <summary>
        /// Get game type list of a certain game kind
        /// </summary>
        /// <param name="gameKindID">ID of game kind</param>
        /// <returns>List GameTypeDetail</returns>
        public override List<GameTypeDetail> GetGameType(int gameKindID)
        {
            List<GameTypeDetail> lsGameType = new List<GameTypeDetail>();
            BasicInfo _csBasicInfo = new BasicInfo();
            DataTable dtGameType = _csBasicInfo.GetGameType(gameKindID);
            foreach (DataRow row in dtGameType.Rows)
            {
                GameTypeDetail gameType = new GameTypeDetail();
                gameType.CasinoID = CastAide.AsInt32(row["CasinoID"]);
                gameType.GameTypeID = CastAide.AsInt32(row["GameTypeID"]);
                gameType.GameTypeName = CastAide.AsString(row[Lang("GameTypeName")]);
                gameType.GameTypeNo = CastAide.AsString(row["GameTypeNo"]);
                gameType.GameKind = CastAide.AsInt32(row["GameKind"]);
                gameType.GameGroupID = CastAide.AsInt32(row["GameGroupID"]);
                gameType.FlashFileName = CastAide.AsString(row["FlashFileName"]);
                gameType.AlertMaxIdle = CastAide.AsInt32(row["AlertMaxIdle"]);
                gameType.IsTrialTime = CastAide.AsBoolean(row["IsTrialTime"]);
                gameType.MaxIdle = CastAide.AsInt32(row["MaxIdle"]);
                lsGameType.Add(gameType);
            }
            return lsGameType;
        }

        public override List<GameTypeDetail> GetGameType(int custId, int gameKindID)
        {
            List<GameTypeDetail> lsGameType = new List<GameTypeDetail>();
            CasinoLibrary.SBAgent.Customer _cust = new CasinoLibrary.SBAgent.Customer();
            DataTable dtGameType = _cust.GetCustGameType(custId, gameKindID);
            foreach (DataRow row in dtGameType.Rows)
            {
                GameTypeDetail gameType = new GameTypeDetail();

                gameType.GameTypeID = CastAide.AsInt32(row["GameTypeID"]);
                gameType.GameTypeName = CastAide.AsString(row[Lang("GameTypeName")]);
                gameType.AgentRollingRate = CastAide.AsDouble(row["AgentRollingRate"]);
                gameType.APositionTaking = CastAide.AsDouble(row["APositionTaking"]);
                gameType.AutoPositionTaking = CastAide.AsDouble(row["AutoPositionTaking"]);
                gameType.IsAutoPositionTaking = CastAide.AsBoolean(row["IsAutoPositionTaking"]);
                gameType.MAgentRollingRate = CastAide.AsDouble(row["MAgentRollingRate"]);
                gameType.MaxPositionTaking = CastAide.AsDouble(row["MaxPositionTaking"]);
                gameType.MemberRollingRate = CastAide.AsDouble(row["MemberRollingRate"]);
                //gameType.MinMPositionTaking = CastAide.AsDouble(row["MinMPositionTaking"]);
                gameType.MinPositionTaking = CastAide.AsDouble(row["MinPositionTaking"]);
                gameType.MPositionTaking = CastAide.AsDouble(row["MPositionTaking"]);
                gameType.SMAgentRollingRate = CastAide.AsDouble(row["SMAgentRollingRate"]);
                gameType.SPositionTaking = CastAide.AsDouble(row["SPositionTaking"]);

                gameType.AgentRollingRateUpline = CastAide.AsDouble(row["UplineAgentRollingRate"]);
                gameType.APositionTakingUpline = CastAide.AsDouble(row["UplineAPositionTaking"]);
                gameType.AutoPositionTakingUpline = CastAide.AsDouble(row["UplineAutoPositionTaking"]);
                gameType.IsAutoPositionTakingUpline = CastAide.AsBoolean(row["UplineIsAutoPositionTaking"]);
                gameType.MAgentRollingRateUpline = CastAide.AsDouble(row["UplineMAgentRollingRate"]);
                gameType.MaxPositionTakingUpline = CastAide.AsDouble(row["UplineMaxPositionTaking"]);
                gameType.MemberRollingRateUpline = CastAide.AsDouble(row["UplineMemberRollingRate"]);
                //gameType.MinMPositionTaking = CastAide.AsDouble(row["MinMPositionTaking"]);
                gameType.MinPositionTakingUpline = CastAide.AsDouble(row["UplineMinPositionTaking"]);
                gameType.MPositionTakingUpline = CastAide.AsDouble(row["UplineMPositionTaking"]);
                gameType.SMAgentRollingRateUpline = CastAide.AsDouble(row["UplineSMAgentRollingRate"]);
                gameType.SPositionTakingUpline = CastAide.AsDouble(row["UplineSPositionTaking"]);

                lsGameType.Add(gameType);
            }
            return lsGameType;
        }

        /// <summary>
        /// Get Game Kind of Master Edit Member
        /// </summary>
        /// <param name="memberid"></param>
        /// <returns></returns>

        public override List<GameKindDetail> GetGameKinds_MasterEditMember(int memberid)
        {
            List<GameKindDetail> lsGameKind = new List<GameKindDetail>();
            CasinoLibrary.SBAgent.Customer _cust = new CasinoLibrary.SBAgent.Customer();
            DataTable dtGameKind = _cust.GetCustGameKind(memberid);
            foreach (DataRow row in dtGameKind.Rows)
            {
                GameKindDetail gameKind = new GameKindDetail();
                gameKind.CodeID = CastAide.AsInt32(row["GameKind"]);
                gameKind.CodeName = CastAide.AsString(row[Lang("GameKindName")]);
                gameKind.APositionTaking = CastAide.AsDouble(row["APositionTaking"]);
                gameKind.AutoPositionTaking = CastAide.AsDouble(row["AutoPositionTaking"]);
                gameKind.IsAutoPositionTaking = CastAide.AsBoolean(row["IsAutoPositionTaking"]);
                gameKind.MPositionTaking = CastAide.AsDouble(row["MPositionTaking"]);
                gameKind.MasterMaxPositionTaking = CastAide.AsDouble(row["MasterMaxPositionTalking"]);
                gameKind.MasterMinPositionTaking = CastAide.AsDouble(row["MasterMinPositionTaking"]);
                gameKind.SPositionTaking = CastAide.AsDouble(row["SPositionTaking"]);
                gameKind.SuperSPositionTaking = CastAide.AsDouble(row["SuperSPositionTaking"]);

                gameKind.IsAutoPositionTaking = CastAide.AsBoolean(row["IsAutoPositionTaking"]);
                gameKind.IsAutoPositionTakingUpline = CastAide.AsBoolean(row["SuperIsAutoPositionTaking"]);

                lsGameKind.Add(gameKind);
            }

            return lsGameKind;
        }

        /// <summary>
        /// Get Game Types of Master Edit Member
        /// </summary>
        /// <param name="memberid"></param>
        /// <param name="gameKindID"></param>
        /// <returns></returns>
        public override List<GameTypeDetail> GetGameTypes_MasterEditMember(int memberid, int gameKindID)
        {
            List<GameTypeDetail> lsGameType = new List<GameTypeDetail>();
            CasinoLibrary.SBAgent.Customer _cust = new CasinoLibrary.SBAgent.Customer();
            DataTable dtGameType = _cust.GetCustGameType(memberid, gameKindID);
            foreach (DataRow row in dtGameType.Rows)
            {
                GameTypeDetail gameType = new GameTypeDetail();

                gameType.GameTypeID = CastAide.AsInt32(row["GameTypeID"]);
                gameType.GameTypeName = CastAide.AsString(row[Lang("GameTypeName")]);
                gameType.AgentRollingRate = CastAide.AsDouble(row["AgentRollingRate"]);
                gameType.APositionTaking = CastAide.AsDouble(row["APositionTaking"]);
                gameType.AutoPositionTaking = CastAide.AsDouble(row["AutoPositionTaking"]);
                gameType.IsAutoPositionTaking = CastAide.AsBoolean(row["IsAutoPositionTaking"]);
                gameType.MPositionTaking = CastAide.AsDouble(row["MPositionTaking"]);
                gameType.MasterMaxPositionTaking = CastAide.AsDouble(row["MasterMaxPositionTalking"]);
                gameType.MasterMinPositionTaking = CastAide.AsDouble(row["MasterMinPositionTaking"]);
                gameType.SPositionTaking = CastAide.AsDouble(row["SPositionTaking"]);
                gameType.SuperSPositionTaking = CastAide.AsDouble(row["SuperSPositionTaking"]);
                gameType.IsAutoPositionTaking = CastAide.AsBoolean(row["IsAutoPositionTaking"]);
                gameType.IsAutoPositionTakingUpline = CastAide.AsBoolean(row["SuperIsAutoPositionTaking"]);
                lsGameType.Add(gameType);
            }
            return lsGameType;
        }

        public override List<StakeLevelDetail> GetCustStakeLevels(int custId, int gameTypeID)
        {
            List<StakeLevelDetail> lsStakeLevel = new List<StakeLevelDetail>();
            CasinoLibrary.SBAgent.Customer _cust = new CasinoLibrary.SBAgent.Customer();
            DataTable dtStakeList = _cust.GetCustStakeLevel(custId, gameTypeID);
            foreach (DataRow row in dtStakeList.Rows)
            {
                StakeLevelDetail stake = new StakeLevelDetail();
                stake.StakeLevelID = CastAide.AsInt32(row["StakeLevelID"]);
                stake.StakeLevelName = CastAide.AsString(row["StakeLevelName"]);
                stake.IsChecked = CastAide.AsBoolean(row["IsChecked"]);
                lsStakeLevel.Add(stake);
            }
            return lsStakeLevel;
        }

        public override List<StakeLevelDetail> GetGameTypeStakeLevels(int gameTypeID, int currencyId)
        {
            //int custId = 1267168;
            List<StakeLevelDetail> lsStakeLevel = new List<StakeLevelDetail>();
            BasicInfo bi = new BasicInfo();
            DataTable dtStakeList = bi.GetGameTypeStakeLevel(gameTypeID, currencyId);
            foreach (DataRow row in dtStakeList.Rows)
            {
                StakeLevelDetail stake = new StakeLevelDetail();
                stake.StakeLevelID = CastAide.AsInt32(row["StakeLevelID"]);
                stake.StakeLevelName = CastAide.AsString(row["StakeLevelName"]);
                stake.IsChecked = true;
                lsStakeLevel.Add(stake);
            }
            return lsStakeLevel;
        }

        public override List<StakeLevelDetail> GetGameTypeStakeLevels(int gameTypeID, string currencyName)
        {
            throw new NotImplementedException();
        }

        public override CSCustomerDetail GetCustSettingCS(int custId)
        {
            CasinoLibrary.SBAgent.Customer _cust = new CasinoLibrary.SBAgent.Customer();
            DataTable dtCustomer = _cust.GetCustSetting(custId);
            DataRow row = null;
            CSCustomerDetail _custSetting = new CSCustomerDetail();
            if (dtCustomer.Rows.Count > 0)
            {
                row = dtCustomer.Rows[0];
                _custSetting.IsEnablePT = CastAide.AsBoolean(row["IsEnablePT"]);
                _custSetting.TotalWinLoss = CastAide.AsDouble(row["ActualMaxTransfer"]);
                _custSetting.MaxWin = CastAide.AsDouble(row["MaxWin"]);
                _custSetting.MaxLose = CastAide.AsDouble(row["MaxLose"]);
                _custSetting.MaxTransferCredit = CastAide.AsDouble(row["MaxTransferCredit"]);
                _custSetting.IsMaxWin = CastAide.AsBoolean(row["IsMaxWin"]);
                _custSetting.IsMaxLose = CastAide.AsBoolean(row["IsMaxLose"]);
                _custSetting.PTByGameType = CastAide.AsBoolean(row["PTByGameType"]);
                _custSetting.RoleId = CastAide.AsInt32(row["RoleId"]);
                _custSetting.UpperCustID = CastAide.AsInt32(row["UpperCustId"]);
                _custSetting.IsAutoReset = CastAide.AsBoolean(row["IsAutoReset"]);

                // Get max transfer information
                if (_custSetting.UpperCustID != 0)
                {
                    DataTable dt = _cust.GetDefaultMaxTransfer(_custSetting.UpperCustID, 0, false);
                    _custSetting.MaxTransferCreditLimit = CastAide.AsDouble((dt.Rows[0]["MaxLimit"]));
                    _custSetting.MinTransferCreditLimit = CastAide.AsDouble((dt.Rows[0]["MinLimit"]));

                    _custSetting.DefaultTransferCredit = CastAide.AsDouble((dt.Rows[0]["DefMaxTransferCredit"]));
                }

                _custSetting.NoCSData = false;
            }
            return _custSetting;
        }

        //get customer setting from Casino and add to AGE customer info
        public override void GetCustSettingCS(ref CSCustomerDetail _custIBC)
        {
            CasinoLibrary.SBAgent.Customer _cust = new CasinoLibrary.SBAgent.Customer();
            DataTable dtCustomer = _cust.GetCustSetting(_custIBC.IBCCustID);
            DataRow row = null;
            if (dtCustomer != null && dtCustomer.Rows.Count > 0)
            {
                row = dtCustomer.Rows[0];
                _custIBC.IsEnablePT = CastAide.AsBoolean(row["IsEnablePT"]);
                _custIBC.TotalWinLoss = CastAide.AsDouble(row["ActualMaxTransfer"]);
                _custIBC.MaxWin = CastAide.AsDouble(row["MaxWin"]);
                _custIBC.MaxLose = CastAide.AsDouble(row["MaxLose"]);

                _custIBC.MaxTransferCredit = CastAide.AsDouble(row["MaxTransferCredit"]);

                _custIBC.IsMaxWin = CastAide.AsBoolean(row["IsMaxWin"]);
                _custIBC.IsMaxLose = CastAide.AsBoolean(row["IsMaxLose"]);
                _custIBC.PTByGameType = CastAide.AsBoolean(row["PTByGameType"]);
                _custIBC.RoleId = CastAide.AsInt32(row["RoleId"]);
                _custIBC.UpperCustID = CastAide.AsInt32(row["UpperCustId"]);
                _custIBC.IsAutoReset = CastAide.AsBoolean(row["IsAutoReset"]);

                // Get max transfer information
                if (_custIBC.UpperCustID != 0)
                {
                    DataTable dt = _cust.GetDefaultMaxTransfer(_custIBC.UpperCustID, 0, false);
                    _custIBC.MaxTransferCreditLimit = CastAide.AsDouble((dt.Rows[0]["MaxLimit"]));
                    _custIBC.MinTransferCreditLimit = CastAide.AsDouble((dt.Rows[0]["MinLimit"]));

                    _custIBC.DefaultTransferCredit = CastAide.AsDouble((dt.Rows[0]["DefMaxTransferCredit"]));
                }

                _custIBC.NoCSData = false;
            }
        }

        /// <summary>
        /// Return a string which matching with current language
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static string Lang(string original)
        {
            switch (Sunplus.Agent.Model.Entities.UserSession.Current.Language)
            {
                case "en-US": return original + "_En";
                case "ja-JP": return original + "_Jp";
                case "zh-TW": return original + "_Ct"; // Traditional
                case "zh-CN": return original + "_Cs"; // Simplified
                case "th-TH": return original + "_Th";
                case "ko-KR": return original + "_Ko";
            }

            return original;
        }

        public override CSCustomerDetail GetCustInfoIBC(int _IBCCustId)
        {
            throw new NotImplementedException();
        }
    }
}