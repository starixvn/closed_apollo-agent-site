/*
 * Created 20100327@Lee - User permission entity
 * Revision:
 *
 * !!!CAUTION: THIS IS GLOBAL DEFINITIONS, ANY REVISION MUST BE APPROVED BEFORE PROCEEDING!!!
 *
 */

namespace Sunplus.Agent.Model.Entities.Permission
{
    using System;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.Entities.Base;

    public class UserPermission : IEntity
    {
        private static string neutralSite = "789y,"; // Seperate by comma
        private static string creditNeutralSite = "789y,"; // Seperate by comma

        public static bool IsDisabledHR
        {
            get
            {
                return false;
            }
        }

        public static bool IsDisabledFI
        {
            get
            {
                return true;
            }
        }

        #region Racing

        public static bool IsShowRacing
        {
            get
            {
                return IsShowRacing_151_31 |
                    IsShowRacing_151_41 |
                    IsShowRacing_151_51 |
                    IsShowRacing_152_41 |
                    IsShowRacing_152_51 |
                    IsShowRacing_153_51;
            }
        }

        public static bool IsShowRacing_151_31
        {
            get
            {
                if (AppSettings.ISPOSuperId == CurrentUserSession.SRecommendId || AppSettings.SpecialSuperId == CurrentUserSession.SRecommendId)
                {
                    return true;
                }

                return CurrentUserSession.IsAllowHRCurrency && !AppSettings.Disable151_31;
            }
        }

        public static bool IsShowRacing_151_41
        {
            get
            {
                return !AppSettings.Disable151_41;
            }
        }

        public static bool IsShowRacing_151_51
        {
            get
            {
                return !AppSettings.Disable151_51;
            }
        }

        public static bool IsShowRacing_152_41
        {
            get
            {
                return !AppSettings.Disable152_41;
            }
        }

        public static bool IsShowRacing_152_51
        {
            get
            {
                return !AppSettings.Disable152_51;
            }
        }

        public static bool IsShowRacing_153_51
        {
            get
            {
                return !AppSettings.Disable153_51;
            }
        }

        #endregion Racing

        public static bool IsShowNumberGroup
        {
            get
            {
                return AppSettings.EnableNumberGroup;
            }
        }

        public static bool IsShowFinance
        {
            get
            {
                return false;
            }
        }

        public static string NeutralSite
        {
            get { return UserPermission.neutralSite; }
            set { UserPermission.neutralSite = value; }
        }

        /// <summary>
        /// Indicate logged-in customer is neutral or not.
        /// </summary>
        public static bool IsNeutral
        {
            get
            {
                return AppSettings.IsNeutralSite ||
                    (CurrentUserSession.Site != null && neutralSite.IndexOf(CurrentUserSession.Site, 0, StringComparison.CurrentCultureIgnoreCase) != -1);
            }
        }

        public static bool IsShowCasino // Already offer for 789y site
        {
            get
            {
                return CurrentUserSession.IsCasinoSync;
            }
        }

        public static bool IsShowBingo
        {
            get
            {
                return !IsNeutral && AppSettings.IsEnableBingo && CurrentUserSession.IsBingoSync;
            }
        }

        public static bool IsShowBingoReport
        {
            get
            {
                //return (!IsNeutral ||
                //   (CurrentUserSession.Site != null && creditNeutralSite.IndexOf(CurrentUserSession.Site, 0, StringComparison.CurrentCultureIgnoreCase) != -1))
                //   && CurrentUserSession.IsBingoSync;
                return false;
            }
        }

        public static string CreditNeutralSite
        {
            get { return UserPermission.creditNeutralSite; }
            set { UserPermission.creditNeutralSite = value; }
        }

        public static bool IsShowNumberGame
        {
            get
            {
                // Offer for CreditNeutral
                return (!IsNeutral ||
                    (CurrentUserSession.Site != null && creditNeutralSite.IndexOf(CurrentUserSession.Site, 0, StringComparison.CurrentCultureIgnoreCase) != -1))
                    && !AppSettings.DisabledNumberGame;
            }
        }

        public static bool IsShowMixedSport
        {
            get
            {
                return AppSettings.IsShowMixedSport;
            }
        }

        public static bool IsShowP2P
        {
            get
            {
                return /*!IsNeutral && */AppSettings.IsEnableP2P && !CurrentUserSession.IsP2PDisabled;
            }
        }

        public static bool IsShowLiveCasino
        {
            get
            {
                return AppSettings.IsEnableLiveCasino && CurrentUserSession.IsLiveCasinoSync;
            }
        }

        public static bool IsShowVirtualSports
        {
            get
            {
                return AppSettings.IsEnableVirtualSports && !CurrentUserSession.IsHideVirtualSports;
            }
        }

        public static bool IsShowDoubleCommission
        {
            get
            {
                return AppSettings.IsEnableDoubleCommission && CurrentUserSession.IsDoubleCommLosing;
            }
        }

        protected static IUserSession CurrentUserSession
        {
            get
            {
                return Sunplus.Agent.Model.Entities.UserSession.Current;
            }
        }
    }
}