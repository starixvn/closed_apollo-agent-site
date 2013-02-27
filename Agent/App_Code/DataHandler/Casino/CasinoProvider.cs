using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;

/// <summary>
/// Summary description for CasinoProvider
/// </summary>
namespace DAL
{

    public abstract class CasinoProvider
    {
        private static CasinoProvider _Instance = null;

        public static CasinoProvider Instance
        {
            get
            {
                  return (CasinoProvider)Activator.CreateInstance(Type.GetType(Globals.IBCSetting.Casino.ProviderType));
            }
            set
            {
                _Instance = value;
            }
        }


        /// <summary>
        /// Update Casino status to SP database
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="bSyncCS"></param>
        /// <param name="bDisabledCS"></param>
        /// <returns></returns>
        public abstract bool UpdateCSStatus(int custid, bool bSyncCS, bool bDisabledCS);
        public abstract bool UpdateCSStatus(int custid, bool bSyncCS, bool bDisabledCS, bool isUpdDownline);
        public abstract bool UpdateCSStatus(int custid, bool bSyncCS, bool bDisabledCS, bool isUpdDownline, out string errMsg);
        public abstract bool UpdateCSStatusWithoutSync(int custid, bool bSyncCS, bool bDisabledCS, bool isUpdDownline);
        public abstract CSCustomerDetail GetCustInfoIBC(string custid, Hashtable r);
        public abstract bool GetCasinoStatus(int custid);

        public abstract List<GameKindDetail> GetGameKinds();
       
        /// <summary>
        /// Get game kind list
        /// <param name="casinoNo"/>
        /// casinoNo= "01", "02"
        /// </summary>
        public abstract List<GameKindDetail> GetGameKinds(string casinoNo);


        public abstract List<GameKindDetail> GetGameKinds(int custId);
        public abstract List<GameKindDetail> GetGameKinds_EditMember(int memberId);
        

        /// <summary>
        /// Get game type list of a certain game kind
        /// </summary>
        /// <param name="gameKindID">ID of game kind</param>
        /// <returns>List GameTypeDetail</returns>
        public abstract List<GameTypeDetail> GetGameType(int gameKindID);

        public abstract List<GameTypeDetail> GetGameType(int custId, int gameKindID);
        public abstract List<GameTypeDetail> GetGameTypes_EditMember(int memberId, int gameKindID);

        /// <summary>
        /// Get game type stake levels
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="gameTypeID"></param>
        /// <returns></returns>
        public abstract List<StakeLevelDetail> GetCustStakeLevels(int custId, int gameTypeID);
       

        /// <summary>
        /// Get game type stake levels list
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="gameTypeID"></param>
        /// <returns></returns>
        public abstract List<StakeLevelDetail> GetGameTypeStakeLevels(int gameTypeID, int currencyId);
        public abstract List<StakeLevelDetail> GetGameTypeStakeLevels(int gameTypeID, string currencyName);

        public abstract CSCustomerDetail GetCustSettingCS(int _IBCCustId);

        public abstract void GetCustSettingCS(ref CSCustomerDetail _custIBC);

        public abstract CSCustomerDetail GetCustInfoIBC(int _IBCCustId);

        public virtual List<GameKindDetail> GetGameKinds_MasterEditMember(int memberid)
        {
            throw new NotImplementedException();
        }

        public virtual List<GameTypeDetail> GetGameTypes_MasterEditMember(int memberid, int gameKindId)
        {
            throw new NotImplementedException();
        }
        

    }
}

