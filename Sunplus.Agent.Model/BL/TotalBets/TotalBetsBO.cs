/*
 * Created 20100507@Jacob - Business class of total bet function
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Sunplus.Agent.Model.BL.TotalBets
{
    using System;
    using System.Data;
    using Sunplus.Agent.Model.DL.TotalBets;

    public class TotalBetsBO : Sunplus.Agent.Model.BL.Base.BOBase
    {
        #region Variable
        private static readonly TotalBetsDAO totalBetsDAO = new TotalBetsDAO();
        #endregion

        public TotalBetsBO(string netSessionId)
            : base(netSessionId)
        { 
        }

        #region TotalGoal
        /// <summary>
        /// Get data for total goal page
        /// </summary>
        /// <returns></returns>
        public DataTable GetTotalGoal()
        {
            DataTable totalGoal = null;

            // parameter
            int customerID = CurrentSession.CustId;
            string language = CurrentSession.LangKey;

            // execute
            if (CurrentSession.IsAgent)
            {
                totalGoal = totalBetsDAO.Age_TotalBets_Sel6_Ag(customerID, language);
            }
            else if (CurrentSession.IsMaster)
            {
                totalGoal = totalBetsDAO.Age_TotalBets_Sel6_MAg(customerID, language);
            }
            else if (CurrentSession.IsSuper)
            {
                totalGoal = totalBetsDAO.Age_TotalBets_Sel6_SAg(customerID, language);
            }
            else
            {
                totalGoal = new DataTable();
            }

            return totalGoal;
        }
        #endregion

        #region HDPOU
        /// <summary>
        /// Get data for HDPOU page
        /// </summary>
        /// <returns></returns>
        public DataTable GetHDPOU()
        {
            DataTable hpdOU = null;

            // parameter
            int customerID = CurrentSession.CustId;
            string language = CurrentSession.LangKey;

            // execute
            if (CurrentSession.IsAgent)
            {
                hpdOU = totalBetsDAO.Age_TotalBets_Sel1378_Ag(customerID, language);
            }
            else if (CurrentSession.IsMaster)
            {
                hpdOU = totalBetsDAO.Age_TotalBets_Sel1378_MAg(customerID, language);
            }
            else if (CurrentSession.IsSuper)
            {
                hpdOU = totalBetsDAO.Age_TotalBets_Sel1378_SAg(customerID, language);
            }
            else
            {
                hpdOU = new DataTable();
            }

            return hpdOU;
        }
        #endregion

        #region MoneyLine

        /// <summary>
        /// Get data for money line page
        /// </summary>
        /// <returns></returns>
        public DataTable GetMoneyLine()
        {
            DataTable moneyline = null;

            // parameter
            int customerID = CurrentSession.CustId;
            string language = CurrentSession.LangKey;

            // execute
            if (CurrentSession.IsAgent)
            {
                moneyline = totalBetsDAO.Age_TotalBets_Sel2021_Ag(customerID, language);
            }
            else if (CurrentSession.IsMaster)
            {
                moneyline = totalBetsDAO.Age_TotalBets_Sel2021_MAg(customerID, language);
            }
            else if (CurrentSession.IsSuper)
            {
                moneyline = totalBetsDAO.Age_TotalBets_Sel2021_SAg(customerID, language);
            }
            else
            {
                moneyline = new DataTable();
            }

            return moneyline;
        }
        #endregion

        #region OddEvent_OneXTwo

        /// <summary>
        /// Get data for money line page
        /// </summary>
        /// <returns></returns>
        public DataTable GetOddEventOneXTwo()
        {
            DataTable oddEventOneXTwo = null;

            // parameter
            int customerID = CurrentSession.CustId;
            string language = CurrentSession.LangKey;

            // execute
            if (CurrentSession.IsAgent)
            {
                oddEventOneXTwo = totalBetsDAO.Age_TotalBets_Sel25_Ag(customerID, language);
            }
            else if (CurrentSession.IsMaster)
            {
                oddEventOneXTwo = totalBetsDAO.Age_TotalBets_Sel25_MAg(customerID, language);
            }
            else if (CurrentSession.IsSuper)
            {
                oddEventOneXTwo = totalBetsDAO.Age_TotalBets_Sel25_SAg(customerID, language);
            }
            else
            {
                oddEventOneXTwo = new DataTable();
            }

            return oddEventOneXTwo;
        }
        #endregion

        #region OutRight
        /// <summary>
        /// Get data for outright page
        /// </summary>
        /// <returns></returns>
        public DataTable GetOutRight()
        {
            DataTable outright = null;

            // parameter
            int customerID = CurrentSession.CustId;
            string language = CurrentSession.LangKey;

            // execute
            if (CurrentSession.IsAgent)
            {
                outright = totalBetsDAO.Age_TotalBets_Sel10_Ag(customerID, language);
            }
            else if (CurrentSession.IsMaster)
            {
                outright = totalBetsDAO.Age_TotalBets_Sel10_MAg(customerID, language);
            }
            else if (CurrentSession.IsSuper)
            {
                outright = totalBetsDAO.Age_TotalBets_Sel10_SAg(customerID, language);
            }
            else
            {
                outright = new DataTable();
            }

            return outright;
        }
        #endregion

        #region HTFT
        /// <summary>
        /// Get data for HTFT page
        /// </summary>
        /// <returns></returns>
        public DataTable GetHTFT()
        {
            DataTable asHTFT = null;

            // parameter
            int customerID = CurrentSession.CustId;
            string language = CurrentSession.LangKey;

            // execute
            if (CurrentSession.IsAgent)
            {
                asHTFT = totalBetsDAO.Age_TotalBets_Sel16_Ag(customerID, language);
            }
            else if (CurrentSession.IsMaster)
            {
                asHTFT = totalBetsDAO.Age_TotalBets_Sel16_MAg(customerID, language);
            }
            else if (CurrentSession.IsSuper)
            {
                asHTFT = totalBetsDAO.Age_TotalBets_Sel16_SAg(customerID, language);
            }
            else
            {
                asHTFT = new DataTable();
            }

            return asHTFT;
        }
        #endregion

        #region FGLG
        /// <summary>
        /// Get data for FGLG page
        /// </summary>
        /// <returns></returns>
        public DataTable GetFGLG()
        {
            DataTable asFGLG = null;

            // parameter
            int customerID = CurrentSession.CustId;
            string language = CurrentSession.LangKey;

            // execute
            if (CurrentSession.IsAgent)
            {
                asFGLG = totalBetsDAO.Age_TotalBets_Sel14_Ag(customerID, language);
            }
            else if (CurrentSession.IsMaster)
            {
                asFGLG = totalBetsDAO.Age_TotalBets_Sel14_MAg(customerID, language);
            }
            else if (CurrentSession.IsSuper)
            {
                asFGLG = totalBetsDAO.Age_TotalBets_Sel14_SAg(customerID, language);
            }
            else
            {
                asFGLG = new DataTable();
            }

            return asFGLG;
        }
        #endregion

        #region Home/Draw/AwayNoBet
        /// <summary>
        /// Get data for Home/Draw/AwayNoBet page
        /// </summary>
        /// <returns></returns>
        public DataTable GetHomeDrawAwayNoBet()
        {
            DataTable homeDrawAwayNoBet = null;

            // parameter
            int customerID = CurrentSession.CustId;
            string language = CurrentSession.LangKey;

            // execute
            if (CurrentSession.IsAgent)
            {
                homeDrawAwayNoBet = totalBetsDAO.Age_TotalBets_Sel121_Ag(customerID, language);
            }
            else if (CurrentSession.IsMaster)
            {
                homeDrawAwayNoBet = totalBetsDAO.Age_TotalBets_Sel121_MAg(customerID, language);
            }
            else if (CurrentSession.IsSuper)
            {
                homeDrawAwayNoBet = totalBetsDAO.Age_TotalBets_Sel121_SAg(customerID, language);
            }
            else
            {
                homeDrawAwayNoBet = new DataTable();
            }

            return homeDrawAwayNoBet;
        }
        #endregion

        #region NextGoal
        /// <summary>
        /// Get data for NextGoal page
        /// </summary>
        /// <returns></returns>
        public DataTable GetNextGoal()
        {
            DataTable nextGoal = null;

            // parameter
            int customerID = CurrentSession.CustId;
            string language = CurrentSession.LangKey;

            // execute
            if (CurrentSession.IsAgent)
            {
                nextGoal = totalBetsDAO.Age_TotalBets_Sel22_Ag(customerID, language);
            }
            else if (CurrentSession.IsMaster)
            {
                nextGoal = totalBetsDAO.Age_TotalBets_Sel22_MAg(customerID, language);
            }
            else if (CurrentSession.IsSuper)
            {
                nextGoal = totalBetsDAO.Age_TotalBets_Sel22_SAg(customerID, language);
            }
            else
            {
                nextGoal = new DataTable();
            }

            return nextGoal;
        }
        #endregion

        #region HorseRacing
        /// <summary>
        /// Get data for NextGoal page
        /// </summary>
        /// <returns></returns>
        public DataTable GetHorseRacing()
        {
            DataTable horseRacing = null;

            // parameter
            int customerID = CurrentSession.CustId;
            string language = CurrentSession.LangKey;

            // execute
            if (CurrentSession.IsAgent)
            {
                horseRacing = totalBetsDAO.Age_TotalBets_SelHR_Ag(customerID, language);
            }
            else if (CurrentSession.IsMaster)
            {
                horseRacing = totalBetsDAO.Age_TotalBets_SelHR_MAg(customerID, language);
            }
            else if (CurrentSession.IsSuper)
            {
                horseRacing = totalBetsDAO.Age_TotalBets_SelHR_SAg(customerID, language);
            }
            else
            {
                horseRacing = new DataTable();
            }

            return horseRacing;
        }
        #endregion

        #region Financial
        /// <summary>
        /// Get data for Financial page
        /// </summary>
        /// <returns></returns>
        public DataTable GetFinancial()
        {
            DataTable financial = null;

            // parameter
            int customerID = CurrentSession.CustId;
            string language = CurrentSession.LangKey;

            // execute
            if (CurrentSession.IsAgent)
            {
                financial = totalBetsDAO.Age_TotalBets_Sel201_Ag(customerID, language);
            }
            else if (CurrentSession.IsMaster)
            {
                financial = totalBetsDAO.Age_TotalBets_Sel201_MAg(customerID, language);
            }
            else if (CurrentSession.IsSuper)
            {
                financial = totalBetsDAO.Age_TotalBets_Sel201_SAg(customerID, language);
            }
            else
            {
                financial = new DataTable();
            }

            return financial;
        }
        #endregion

        #region NumberGame
        /// <summary>
        /// Get data for number game page
        /// </summary>
        /// <returns></returns>
        public DataTable GetNumberGame()
        {
            DataTable numberGame = null;

            // parameter
            int customerID = CurrentSession.CustId;
            string language = CurrentSession.LangKey;

            // execute
            if (CurrentSession.IsAgent)
            {
                numberGame = totalBetsDAO.Age_TotalBets_Sel161_Ag(customerID, language);
            }
            else if (CurrentSession.IsMaster)
            {
                numberGame = totalBetsDAO.Age_TotalBets_Sel161_MAg(customerID, language);
            }
            else if (CurrentSession.IsSuper)
            {
                numberGame = totalBetsDAO.Age_TotalBets_Sel161_SAg(customerID, language);
            }
            else
            {
                numberGame = new DataTable();
            }

            return numberGame;
        }
        #endregion

        #region LiveCasino
        /// <summary>
        /// Get data for LiveCasino page
        /// </summary>
        /// <returns></returns>
        public DataTable GetLiveCasino()
        {
            DataTable liveCasino = null;

            // parameter
            int customerID = CurrentSession.CustId;
            string language = CurrentSession.LangKey;

            // execute
            if (CurrentSession.IsAgent)
            {
                liveCasino = totalBetsDAO.Age_TotalBets_Sel162_Ag(customerID, language);
            }
            else if (CurrentSession.IsMaster)
            {
                liveCasino = totalBetsDAO.Age_TotalBets_Sel162_MAg(customerID, language);
            }
            else if (CurrentSession.IsSuper)
            {
                liveCasino = totalBetsDAO.Age_TotalBets_Sel162_SAg(customerID, language);
            }
            else
            {
                liveCasino = new DataTable();
            }

            return liveCasino;
        }
        #endregion

        #region VirtualSports
        /// <summary>
        /// Get data for VirtualSports page
        /// </summary>
        /// <returns></returns>
        public DataTable GetVirtualSports()
        {
            DataTable virtualSports = null;

            // parameter
            int customerID = CurrentSession.CustId;
            string language = CurrentSession.LangKey;

            // execute
            if (CurrentSession.IsAgent)
            {
                virtualSports = totalBetsDAO.Age_TotalBets_Sel180_Ag(customerID, language);
            }
            else if (CurrentSession.IsMaster)
            {
                virtualSports = totalBetsDAO.Age_TotalBets_Sel180_MAg(customerID, language);
            }
            else if (CurrentSession.IsSuper)
            {
                virtualSports = totalBetsDAO.Age_TotalBets_Sel180_SAg(customerID, language);
            }
            else
            {
                virtualSports = new DataTable();
            }

            return virtualSports;
        }
        #endregion

        #region Other bettypes
        /// <summary>
        /// Get data for Other bettypes page
        /// </summary>
        /// <returns></returns>
        public DataTable GetOtherBettypes()
        {
            DataTable otherBettypes = null;

            // parameter
            int customerID = CurrentSession.CustId;
            string language = CurrentSession.LangKey;

            // execute
            if (CurrentSession.IsAgent)
            {
                otherBettypes = totalBetsDAO.Age_TotalBets_SelOthers_Ag(customerID, language);
            }
            else if (CurrentSession.IsMaster)
            {
                otherBettypes = totalBetsDAO.Age_TotalBets_SelOthers_MAg(customerID, language);
            }
            else if (CurrentSession.IsSuper)
            {
                otherBettypes = totalBetsDAO.Age_TotalBets_SelOthers_SAg(customerID, language);
            }
            else
            {
                otherBettypes = new DataTable();
            }

            return otherBettypes;
        }

        #endregion
    }
}
