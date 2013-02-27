using System;
using System.Text;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using JSoft.Utility;
using System.Collections;
using CasinoLibrary.Model.BO;
using System.Collections.Generic;
using System.Net;
using CasinoLibrary;
using CasinoLibrary.SBAgent;

using Sunplus.Agent.Utilities;

[Obsolete]
public partial class Common
{
    public static int CasinoID = 1;


    public static string ExtraOption = "Please Choose One !";
    public static string ShowPerPage = "100,500,1000,1500,2000,2500,3000";
    public static string ShowPerPage1 = "50,100,150,200,250,300";

    public static string GameTableStatusValue = "10,20,30,40,50,90,91,92,93,99";
    public static string GameTableStatusShow = "Idle,Open for Bet,End Bet,Dealing,Settle,Shuffling,Change Shoe,Change Dealer,Frozen,Close";

    public static string OnOffValue = "1,0";
    public static string OnOffShow = "On,Off";

    public static string YesNoValue = "1,0";
    public static string YesNoShow = "Yes,No";

    public static string ServerTypeShow = "T,L";
    public static string ServerTypeValue = "Table,Lobby";

    public static string AdminRoleValue = "6,7";
    public static string AdminRoleShow = "Server Admin,Admin";

    public static string RoleValue = "1,2,3,4";
    public static string RoleShow = "Member,Agent,Master Agent,Super Agent";

    public static string CustStatusValue = "00,10,20";
    public static string CustStatusShow = "Open,Suspend,Closed";

    public static string OperatorValue = ">=,<=";
    public static string OperatorShow = ">=,<=";

    public static decimal DecMax = 99999999999.99999999M;
    public static decimal DecMin = -99999999999.99999999M;

    public static string PasswordCompareValue = "1,2";
    public static string PasswordCompareShow = "Match,Not Match";

    public static string CodeTypeNo = "01,03,04,06,07,12,13,14";
    public static string CodeTypeNoName = "Country,Language,Department,Currency,Color,Subject of Correspondence,Reason of Baddata,Seven Chips";

    public static string OnOffQueryValue = "1,0,";
    public static string OnOffQueryShow = "On,Off,None";

    public static string UseChatQueryValue = "1,2,0,";
    public static string UseChatQueryShow = "Enable Chat,Enable Chat Template,Off,None";

    public static string UseChatValue = "1,2,0";
    public static string UseChatShow = "Enable Chat,Enable Chat Template,Off";

    public static string AdminStatusQueryValue = "00,10,20,";
    public static string AdminStatusQueryShow = "Open,Suspend,Closed,None";

    public static string AdminStatusValue = "00,10,20";
    public static string AdminStatusShow = "Open,Suspend,Closed";

    public static string GenderValue = "1,0";
    public static string GenderShow = "Male,Female";

    public static string CorrespondenceTypeValue = "1,2,3,4,5";
    public static string CorrespondenceTypeShow = "Phone,E-mail,Fax,Chat,Other";

    public static string CorrespondenceContentValue = "1,2,3,4";
    public static string CorrespondenceContentShow = "Question,Suggestion,Suspended Bug,Other";

    public static string CorrespondenceUserMoodValue = "1,2,3,4";
    public static string CorrespondenceUserMoodShow = "Calm,Critical,Upset,Other";

    public static string CorrespondenceStatusValue = "0,1";
    public static string CorrespondenceStatusShow = "Not Done,Done";

    public static string ReportValue = "Daily,Monthly,Username,Game,Currency,Table";

    public static string WinLossReportValue = "Daily,Monthly,SuperAgent,Games,Table,Dealer";

    public static string UserType = "Agent,Member";



    public static string DealerRoleValue = "1,2";
    public static string DealerRoleShow = "Dealer,Pit Boss";

    public static string TransStatusValue = "00,10,11,20,30,31";
    public static string TransStatusShow = "Extner Table,Exit Table,Exit Table,Exit Table,Exit Table,Exit Table";

    public static string UserBetStatusValue = "80,81,90";
    public static string UserBetStatusShow = "Exit Table,Exit Casino,Network Problem";

    public static string BroadcastCriteriaTypeValue = "1,2";
    public static string BroadcastCriteriaTypeShow = "Send to whole casino,Send to usergroup and table";

    public static string BroadcastShowTypeValue = "1,2";
    public static string BroadcastShowTypeShow = "Status Bar,Popup";

    public static string BroadcastTimeTypeValue = "1,N";
    public static string BroadcastTimeTypeShow = "Once(Broadcasting immediately),Repeatedly";

    public static string BadDataTypeValue = "I,U";
    public static string BadDataTypeShow = "IPs,Users";

    public static string BetKindValue = "banker,tie,player";
    public static string BetKindShow = "banker,tie,player";

    public static int ActionID_Update = 2002;
    public static int ActionID_Delete = 2003;
    //AccessLevel
    public static string AccessLevelValue = "1,2,3";
    public static string AccessLevelShow = "Admin System,Agent System,Casino Client";
    //BlockBy
    public static string BlockByValue = "1,2";
    public static string BlockByShow = "Country,IP Address";
    //Period Kind
    public static string PeriodKindValue = "1,2";
    public static string PeriodKindShow = "Forever,Period";

    public static string MonthYearValue = "M,Y";
    public static string MonthYearShow = "Month,Year";

    //flash porker full show width and height
    public static string FlashPokerFull_Width = "176";
    public static string FlashPokerFull_Height = "62";
    /// <summary>
    /// Date Query Category Show
    /// </summary>
    public static string DateQueryShow = "Today,Yesterday,Current Week,Last Week,Current Month,Last Month,Since Last Month,All Until Yesterday,All,Specific Date";
    /// <summary>
    /// Date Query Category Value
    /// </summary>
    public static string DateQueryValue = "Today,Yesterday,Current Week,Last Week,Current Month,Last Month,Since Last Month,All Until Yesterday,All,Specific Date";
    /// <summary>
    /// Date Query Category Default Value
    /// </summary>
    public static string DateQueryDefaultValue = "Today";

    /// <summary>
    /// PT and Commission Setting Shoe
    /// </summary>
    public static string PTSettingShow = "By Game Group,By Game Type";
    /// <summary>
    /// PT and Commission Setting Value
    /// </summary>
    public static string PTSettingValue = "GameKind,GameType";

    private static Hashtable BadDataType;

    private static Hashtable CustStaus;

    private static Hashtable Code;

    private static Hashtable CorrespondenceType;

    private static Hashtable CorrespondenceContent;

    private static Hashtable CorrespondenceStatus;

    private static Hashtable UserMood;

    private static Hashtable GameTableStatus;

    private static Hashtable PasswordCompare;

    private static Hashtable Role;

    private static Hashtable TransStatus;

    private static Hashtable UserBetStatus;

    private static Hashtable BroadcastCriteriaType;

    private static Hashtable BroadcastShowType;

    private static Hashtable BroadcastTimeType;

    public static string LiveGameCodeNo = "01";
    public static string RNGGameCodeNo = "02";
    public static string P2PGameCodeNo = "03";
    public static string GameCategoryCodeTypeNo = "02";

    public static string ConvertCommonDataTypeIDToName(string strShow,string strValue,string strCommonDataTypeID)
    {
        Hashtable htResult = new Hashtable();


        String[] straryStatusID = strValue.Split(',');
        String[] straryStatusName = strShow.Split(',');
        for (int i = 0; i < straryStatusID.Length; i++)
        {
            htResult.Add(straryStatusID[i], straryStatusName[i]);

        }

        string strReturn = "";

        if (htResult.Contains(strCommonDataTypeID))
        {
            strReturn = htResult[strCommonDataTypeID].ToString();
        }

        htResult = null;
        return strReturn;
    }

    public string ConvertBadDataTypeIDToName(string strBadDataTypeID)
    {
        if (BadDataType == null)
        {
            BadDataType = new Hashtable();

            String[] straryStatusID = BadDataTypeValue.Split(',');
            String[] straryStatusName = BadDataTypeShow.Split(',');
            for (int i = 0; i < straryStatusID.Length; i++)
            {
                BadDataType.Add(straryStatusID[i], straryStatusName[i]);

            }
        }

        string strReturn = "";
        if (BadDataType != null)
        {
            if (BadDataType.Contains(strBadDataTypeID))
            {
                strReturn = BadDataType[strBadDataTypeID].ToString();
            }
        }


        return strReturn;
    }

    public string ConvertBroadcastTimeTypeIDToName(string strBroadcastTimeTypeID)
    {
        if (BroadcastTimeType == null)
        {
            BroadcastTimeType = new Hashtable();

            String[] straryStatusID = BroadcastTimeTypeValue.Split(',');
            String[] straryStatusName = BroadcastTimeTypeShow.Split(',');
            for (int i = 0; i < straryStatusID.Length; i++)
            {
                BroadcastTimeType.Add(straryStatusID[i], straryStatusName[i]);

            }
        }

        string strReturn = "";
        if (BroadcastTimeType != null)
        {
            if (BroadcastTimeType.Contains(strBroadcastTimeTypeID))
            {
                strReturn = BroadcastTimeType[strBroadcastTimeTypeID].ToString();
            }
        }

        return strReturn;
    }

    public string ConvertBroadcastShowTypeIDToName(string strBroadcastShowTypeID)
    {
        if (BroadcastShowType == null)
        {
            BroadcastShowType = new Hashtable();

            String[] straryStatusID = BroadcastShowTypeValue.Split(',');
            String[] straryStatusName = BroadcastShowTypeShow.Split(',');
            for (int i = 0; i < straryStatusID.Length; i++)
            {
                BroadcastShowType.Add(straryStatusID[i], straryStatusName[i]);

            }
        }

        string strReturn = "";
        if (BroadcastShowType != null)
        {
            if (BroadcastShowType.Contains(strBroadcastShowTypeID))
            {
                strReturn = BroadcastShowType[strBroadcastShowTypeID].ToString();
            }
        }


        return strReturn;
    }

    public string ConvertBroadcastCriteriaTypeIDToName(string strBroadcastCriteriaTypeID)
    {
        if (BroadcastCriteriaType == null)
        {
            BroadcastCriteriaType = new Hashtable();

            String[] straryStatusID = BroadcastCriteriaTypeValue.Split(',');
            String[] straryStatusName = BroadcastCriteriaTypeShow.Split(',');
            for (int i = 0; i < straryStatusID.Length; i++)
            {
                BroadcastCriteriaType.Add(straryStatusID[i], straryStatusName[i]);

            }
        }

        string strReturn = "";
        if (BroadcastCriteriaType != null)
        {
            if (BroadcastCriteriaType.Contains(strBroadcastCriteriaTypeID))
            {
                strReturn = BroadcastCriteriaType[strBroadcastCriteriaTypeID].ToString();
            }
        }

        return strReturn;
    }

    public string ConvertCorrespondenceStatusIDToName(string strCorrStatusID)
    {
        if (CorrespondenceStatus == null)
        {
            CorrespondenceStatus = new Hashtable();

            String[] straryStatusID = CorrespondenceStatusValue.Split(',');
            String[] straryStatusName = CorrespondenceStatusShow.Split(',');
            for (int i = 0; i < straryStatusID.Length; i++)
            {
                CorrespondenceStatus.Add(straryStatusID[i], straryStatusName[i]);

            }
        }

        string strReturn = "";
        if (CorrespondenceStatus != null)
        {
            if (CorrespondenceStatus.Contains(strCorrStatusID))
            {
                strReturn = CorrespondenceStatus[strCorrStatusID].ToString();
            }
        }


        return strReturn;
    }

    public string ConvertCorrespondenceTypeIDToName(string strCorrTypeID)
    {
        if (CorrespondenceType == null)
        {
            CorrespondenceType = new Hashtable();

            String[] straryStatusID = CorrespondenceTypeValue.Split(',');
            String[] straryStatusName = CorrespondenceTypeShow.Split(',');
            for (int i = 0; i < straryStatusID.Length; i++)
            {
                CorrespondenceType.Add(straryStatusID[i], straryStatusName[i]);

            }
        }

        string strReturn = "";
        if (CorrespondenceType != null)
        {
            if (CorrespondenceType.Contains(strCorrTypeID))
            {
                strReturn = CorrespondenceType[strCorrTypeID].ToString();
            }
        }


        return strReturn;
    }

    public string ConvertCorrespondenceContentIDToName(string strCorrSubjectID)
    {
        if (CorrespondenceContent == null)
        {
            CorrespondenceContent = new Hashtable();

            String[] straryStatusID = CorrespondenceContentValue.Split(',');
            String[] straryStatusName = CorrespondenceContentShow.Split(',');
            for (int i = 0; i < straryStatusID.Length; i++)
            {
                CorrespondenceContent.Add(straryStatusID[i], straryStatusName[i]);

            }


        }

        string strReturn = "";
        if (CorrespondenceContent != null)
        {
            if (CorrespondenceContent.Contains(strCorrSubjectID))
            {
                strReturn = CorrespondenceContent[strCorrSubjectID].ToString();
            }
        }


        return strReturn;
    }

    public string ConvertCorrespondenceUserMoodIDToName(string strCorrUserMoodID)
    {
        if (UserMood == null)
        {
            UserMood = new Hashtable();

            String[] straryStatusID = CorrespondenceUserMoodValue.Split(',');
            String[] straryStatusName = CorrespondenceUserMoodShow.Split(',');
            for (int i = 0; i < straryStatusID.Length; i++)
            {
                UserMood.Add(straryStatusID[i], straryStatusName[i]);
            }
        }

        string strReturn = "";
        if (UserMood != null)
        {
            if (UserMood.Contains(strCorrUserMoodID))
            {
                strReturn = UserMood[strCorrUserMoodID].ToString();
            }
        }
        return strReturn;
    }

    public string ConvertCustStatusToName(string strCustStatusID)
    {
        if (CustStaus == null)
        {
            CustStaus = new Hashtable();

            String[] straryStatusID = CustStatusValue.Split(',');
            String[] straryStatusName = CustStatusShow.Split(',');
            for (int i = 0; i < straryStatusID.Length; i++)
            {
                CustStaus.Add(straryStatusID[i], straryStatusName[i]);
            }
        }

        string strReturn = "";
        if (CustStaus != null)
        {
            if (CustStaus.Contains(strCustStatusID))
            {
                strReturn = CustStaus[strCustStatusID].ToString();
            }
        }
        CustStaus = null;

        return strReturn;
    }

    public string ConvertGameTableStatusIDToName(string strGameTableStatusID)
    {
        if (GameTableStatus == null)
        {
            GameTableStatus = new Hashtable();
            String[] straryStatusID = GameTableStatusValue.Split(',');
            String[] straryStatusName = GameTableStatusShow.Split(',');
            for (int i = 0; i < straryStatusID.Length; i++)
            {
                GameTableStatus.Add(straryStatusID[i], straryStatusName[i]);
            }
        }

        string strReturn = "";
        if (GameTableStatus != null)
        {
            if (GameTableStatus.Contains(strGameTableStatusID))
            {
                strReturn = GameTableStatus[strGameTableStatusID].ToString();
            }
        }

        return strReturn;
    }

    public string ConvertCodeIDToName(string strCodeID, int intIsDisabled)
    {
        if (Code == null)
        {
            CodeBO objCodeBO = new CodeBO();
            objCodeBO.CasinoID = Common.CasinoID;
            objCodeBO.IsDisabled = intIsDisabled;
            DataSet ds = objCodeBO.GetDataSet(objCodeBO);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Code = new Hashtable();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Code.Add(dr["CodeID"].ToString(), dr["CodeName"].ToString());
                }
            }
            objCodeBO = null;
            ds = null;
        }

        string strReturn = "";
        if (Code != null)
        {
            if (Code.Contains(strCodeID))
            {
                strReturn = Code[strCodeID].ToString();
            }
        }

        return strReturn;
    }

    /*public string FormatAmt(Decimal decAmt)
    {
        string strResult = "";
        strResult = StringUtil.VBFormatNumber(decAmt.ToString(), 2, true, false, true);
        //strResult = string.Format("{0:0.00}", decAmt);
        return strResult;

    }*/

    public string ConvertPasswordCompareIDToName(string strPasswordCompareID)
    {
        if (PasswordCompare == null)
        {
            PasswordCompare = new Hashtable();
            String[] straryStatusID = PasswordCompareValue.Split(',');
            String[] straryStatusName = PasswordCompareShow.Split(',');
            for (int i = 0; i < straryStatusID.Length; i++)
            {
                PasswordCompare.Add(straryStatusID[i], straryStatusName[i]);
            }
        }

        string strReturn = "";
        if (PasswordCompare != null)
        {
            if (PasswordCompare.Contains(strPasswordCompareID))
            {
                strReturn = PasswordCompare[strPasswordCompareID].ToString();
            }
        }

        return strReturn;
    }

    public string ConvertTransStatusIDToName(string strTransStatusID)
    {
        if (TransStatus == null)
        {
            TransStatus = new Hashtable();
            String[] straryStatusID = TransStatusValue.Split(',');
            String[] straryStatusName = TransStatusShow.Split(',');
            for (int i = 0; i < straryStatusID.Length; i++)
            {
                TransStatus.Add(straryStatusID[i], straryStatusName[i]);
            }
        }

        string strReturn = "";
        if (TransStatus != null)
        {
            if (TransStatus.Contains(strTransStatusID))
            {
                strReturn = TransStatus[strTransStatusID].ToString();
            }
        }

        return strReturn;
    }

    public string ConvertRoleIDToName(string strRoleID)
    {
        if (Role == null)
        {
            Role = new Hashtable();

            String[] straryStatusID = RoleValue.Split(',');
            String[] straryStatusName = RoleShow.Split(',');
            for (int i = 0; i < straryStatusID.Length; i++)
            {
                Role.Add(straryStatusID[i], straryStatusName[i]);

            }
        }

        string strReturn = "";
        if (Role != null)
        {
            if (Role.Contains(strRoleID))
            {
                strReturn = Role[strRoleID].ToString();
            }
        }


        return strReturn;
    }

    public string ConvertUserBetStatusIDToName(string strStatus)
    {
        if (UserBetStatus == null)
        {
            UserBetStatus = new Hashtable();
            String[] straryStatusID = UserBetStatusValue.Split(',');
            String[] straryStatusName = UserBetStatusShow.Split(',');
            for (int i = 0; i < straryStatusID.Length; i++)
            {
                UserBetStatus.Add(straryStatusID[i], straryStatusName[i]);
            }
        }

        string strReturn = "";
        if (UserBetStatus != null)
        {
            if (UserBetStatus.Contains(strStatus))
            {
                strReturn = UserBetStatus[strStatus].ToString();
            }
        }

        return strReturn;
    }
    
    //取得ds的欄位中比對到的值
    public static string GetFieldValueFromDSByID(DataSet ds,string strSourceField,string strSourceValue,string strResultField)
    {
        string strResult = "";
        try
        {
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (dr[strSourceField].ToString() == strSourceValue)
                        {
                            strResult = dr[strResultField].ToString();
                            break;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Sunplus.Agent.Utilities.Logs.Write(ex.Message + ex.StackTrace);
            throw ex;
        } 
        return strResult;
    }

    /// <summary>
    /// 以取得CustID取得此User的User Type
    /// </summary>
    /// <param name="strCustID">Cust ID</param>
    /// <returns>User Type</returns>
    public static string GetUserTypeByID(string strCustID)
    {
        string strResult = "";
        string strRoleID = "";
        CustomerBO objCustomerBO = new CustomerBO();
        DataSet ds = null;
        try
        {
            objCustomerBO.CustID = StringUtil.CInt(strCustID);
            ds = objCustomerBO.GetDataSet(objCustomerBO);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        strRoleID = dr["RoleId"].ToString();
                    }
                }

                //假如RoleId = 1則為meber，不為1則為agent
                if (strRoleID == "1")
                {
                    strResult = Common.UserType.Split(',')[1];
                }
                else
                {
                    strResult = Common.UserType.Split(',')[0];
                }
            }
        }
        catch (Exception ex)
        {
            Sunplus.Agent.Utilities.Logs.Write(ex.Message + ex.StackTrace);
            throw ex;
        }
        finally
        {
            objCustomerBO = null;
            ds = null;
        }
        return strResult;


    }

    /// <summary>
    /// 以Action Name取得Action ID
    /// </summary>
    /// <param name="strName">Action Name</param>
    /// <returns>Action ID</returns>
    public static string GetActionIDByName(string strName)
    {
        string strResult = "";

        ActionBO objActionBO = new ActionBO();
        DataSet ds = null;
        try
        {
            objActionBO.ActionName = strName;
            ds = objActionBO.GetDataSet(objActionBO);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        strResult = dr["ActionID"].ToString();
                    }
                }


            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objActionBO = null;
            ds = null;
        }
        return strResult;


    }

    /// <summary>
    /// Get Admin Name by Admin ID
    /// </summary>
    /// <param name="strAdminID">Admin ID</param>
    /// <returns>admin name</returns>
    public static string GetAdminNameByID(string strAdminID)
    {
        string strAdminName = "";
        AdminBO objAdminBO = new AdminBO();
        objAdminBO.AdminID = StringUtil.CInt(strAdminID);
        DataSet ds = objAdminBO.GetDataSet(objAdminBO);
        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    strAdminName = StringUtil.CStr(dr["AdminName"]);
                }
            }
        }
        objAdminBO = null;
        ds = null;
        return strAdminName;
    }

    /// <summary>
    /// Get Flash Game Path and file name by game type no
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public static Hashtable GetFlashGameByGameTypeNo()
    {
        Hashtable FlashGame = new Hashtable();
        FlashGame.Add("L0001", "../Flash/PokerComponent.swf");
        FlashGame.Add("L0002", "../Flash/PokerComponent.swf");
        FlashGame.Add("R0001", "../Flash/PokerComponent.swf");
        FlashGame.Add("L0003", "../Flash/RouletteComponent.swf");
        FlashGame.Add("R0008", "../Flash/RouletteComponent.swf");
        FlashGame.Add("R0009", "../Flash/RouletteComponent.swf");
        FlashGame.Add("R0010", "../Flash/RouletteComponent.swf");
        FlashGame.Add("R0006", "../Flash/PokerComponent.swf");
        FlashGame.Add("R0011", "../Flash/SicBoComponent.swf");
        FlashGame.Add("R0003", "../Flash/PokerComponent.swf");
        FlashGame.Add("R0012", "../Flash/PokerComponent.swf");
        
        return FlashGame;
    }

    /// <summary>
    /// Get GameName By Game Type No
    /// </summary>
    /// <param name="strGameTypeNo">Game Type No</param>
    /// <returns>Game Name</returns>
    public static string GetGameNameByGameTypeNo(string strGameTypeNo)
    {
        string strResult = "";

        if (strGameTypeNo == "L0001" || strGameTypeNo == "L0002" || strGameTypeNo == "R0001")
        {
            strResult = "Baccarat";
        }
        return strResult;
    }

    /// <summary>
    ///  Get JacksorBeter Result 
    /// </summary>
    /// <param name="strUserBetID">UserBetID</param>
    /// <returns>result string</returns>
    public static DataTable GetJBGameResultByGameID(string strGameID)
    {
       // return Report.GetJBGameResultByGameID(strGameID);


        DataTable dt;
        Dictionary<string, string> objDictionary = null;
        GameDetailBO objSelJBResultBO = null;

        try
        {
            objSelJBResultBO = new GameDetailBO();
            objSelJBResultBO.ConnKey = "CS_BackupDB";
            objDictionary = new Dictionary<string, string>();
            objDictionary.Add("GameID", strGameID.Trim());
            objDictionary.Add("ErrCode", "");
            objDictionary.Add("ErrMsg", "");
            dt = objSelJBResultBO.SelJackBetter(objDictionary);

            if (objDictionary["ErrCode"] != "")
            {
                throw new Exception(objDictionary["ErrMsg"]);
            }
        }
        catch (Exception ex)
        {
            Sunplus.Agent.Utilities.Logs.Write(ex.Message + ex.StackTrace);
            throw ex;
        }
        finally
        {
            objDictionary = null;
            objSelJBResultBO = null;
        }

        return dt;
    }


     /// <summary>
    ///  Get BlackJacky  Total BetType :  "BetMoney" and  "Win"  Field 
    /// </summary>
    /// <param name="strUserBetID">UserBetID</param>
    /// <returns>result string</returns>
    public static DataTable GetBJGameResultByUserBetID(string strUserBetID)
    {
        //return Report.GetBJGameResultByUserBetID(strUserBetID);
        
        DataTable dt;
        Dictionary<string, string> objDictionary = null;
        UserBetTransBO objSelBJTotalBetBO = null;

        try
        {
            objSelBJTotalBetBO = new UserBetTransBO();
            objSelBJTotalBetBO.ConnKey = "CS_BackupDB";
            objDictionary = new Dictionary<string, string>();
            objDictionary.Add("UserBetID", strUserBetID.Trim());
            objDictionary.Add("ErrCode", "");
            objDictionary.Add("ErrMsg", "");
            dt = objSelBJTotalBetBO.TotalBetTypeBetSelBJ(objDictionary);

            if (objDictionary["ErrCode"] != "")
            {
                throw new Exception(objDictionary["ErrMsg"]);
            }
        }
        catch (Exception ex)
        {
            Sunplus.Agent.Utilities.Logs.Write(ex.Message + ex.StackTrace);
            throw ex;
        }
        finally
        {
            objDictionary = null;
            objSelBJTotalBetBO = null;
        }

        return dt;
         
    }


    /// <summary>
    /// Get BlackJack  Game player point  by Game ID
    /// 因為 BlackJack player 的 Participator 欄位若是　player 時value 值為不固定 
    /// </summary>
    /// <param name="strGameID">Game ID</param>
    /// <param name="strSequence">Field Sequence</param>
    /// <returns>result string</returns>
    public static string GetBJGameResultByGameID(string strGameID, string strSequence)
    {
        //return Report.GetBJGameResultByGameID(strGameID, strSequence);
        
        string strPoint = "";
        DataTable dt;
        Dictionary<string, string> objDictionary = null;
        GameDetailBO objSelBJPointBO = null;

        try
        {
            objSelBJPointBO = new GameDetailBO();
            objSelBJPointBO.ConnKey = "CS_BackupDB";
            objDictionary = new Dictionary<string, string>();
            objDictionary.Add("GameID", strGameID.Trim());
            objDictionary.Add("ErrCode", "");
            objDictionary.Add("ErrMsg", "");
            dt=objSelBJPointBO.SelBlackJackPoint(objDictionary);

            if (objDictionary["ErrCode"] != "")
            {
                throw new Exception(objDictionary["ErrMsg"]);
            }
            else
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (strSequence.Trim() == "Result10")
                    {
                         strPoint = dt.Rows[0]["Result10"].ToString()+",";
                    }
                    else
                    {
                        for (int i = 1; i <= 10; i++)
                        {
                            if (dt.Rows[0]["Result" + i.ToString()].ToString() == "") break;

                            strPoint += dt.Rows[0]["Result" + i.ToString()].ToString() + ",";
                        }
                    }

                    strPoint = strPoint.Substring(0, strPoint.Length - 1);
                }

               
            }
        }
        catch (Exception ex)
        {
            Sunplus.Agent.Utilities.Logs.Write(ex.Message + ex.StackTrace);
            throw ex;
        }
        finally
        {
            objDictionary = null;
            objSelBJPointBO = null;
            dt = null;

        }

        return strPoint.Trim();
         
    }

    /// <summary>
    /// Get Game Result by Game ID
    /// </summary>
    /// <param name="strGameID">Game ID</param>
    /// <param name="strParticipator">Participator</param>
    /// <param name="strSequence">Field Sequence</param>
    /// <returns>result string</returns>
    public static string GetGameResultByGameID(string strGameID,string strParticipator,string strSequence)
    {
        StringBuilder sbResult = new StringBuilder();
        DataSet ds = null;
        GameDetailBO objGameDetailBO = new GameDetailBO();
        objGameDetailBO.ConnKey = "CS_BackupDB";
        objGameDetailBO.GameID = StringUtil.CInt(strGameID);
        if (strParticipator != "") objGameDetailBO.Participator = strParticipator;
        ds = objGameDetailBO.GetDataSet(objGameDetailBO);
        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (strSequence == "")
                    {
                        for (int i = 1; i <= 10; i++)
                        {
                            if (dr["Result" + i.ToString()].ToString() == "") break;
                            sbResult.Append(dr["Result" + i.ToString()].ToString() + ",");
                        }
                    }
                    else
                    {
                        if (strSequence == "Result8")
                        {
                            //若為Result8，則依Result8中所指示的欄位解回
                            strSequence = dr[strSequence].ToString();
                            for (int j = 0; j < strSequence.Length; j++)
                            {
                                sbResult.Append(dr["Result" + strSequence.Substring(j, 1)].ToString() + ",");
                            }
                        }
                        else if (strSequence == "Result10")
                        {
                            //若為Result10，則是秀　BlackJack   的總點數
                             sbResult.Append(dr["Result10"].ToString()+",");
                        }
                        else
                        {
                            //若不為空白也不為Result8則為指定欄位
                            for (int j = 0; j < strSequence.Length; j++)
                            {
                                sbResult.Append(dr["Result" + strSequence.Substring(j, 1)].ToString() + ",");
                            }
                        }
                        

                    }
                }
                if (sbResult.Length > 0) sbResult.Remove(sbResult.Length - 1, 1);
            }
        }
        objGameDetailBO = null;
        ds = null;
        return sbResult.ToString();
    }
    
    /// <summary>
    /// Get Game Result HTML by Game ID
    /// </summary>
    /// <param name="strGameID">Game ID</param>
    /// <param name="strGameTypeNo">Game Type No</param>
    /// <param name="strAction">Action</param>
    /// <returns>result string</returns>
    public static Hashtable GetGameResultHtmlByGameID(string strGameID, string strGameTypeNo, string strAction)
    {
        Hashtable htResult = new Hashtable();
        Hashtable htFlash = Common.GetFlashGameByGameTypeNo();
        StringBuilder sbHtml = new StringBuilder();
        StringBuilder sbClientCmd = new StringBuilder();
        //show flash
        if (StringUtil.CStr(htFlash[strGameTypeNo]).IndexOf("Poker") > 0)
        {
            //poker類
            if (strGameTypeNo == "R0006")
            {
                //hold em
                if (strAction == "Call")
                {
                    //dealer

                    sbHtml.Append("Dealer: <img id=\"GenImg\" width=\"115\" height=\"25\" src=\"..\\Form\\GenHistoryImg.aspx?CameType=01&Content=" + Common.GetGameResultByGameID(strGameID, "B", "Result8") + "&ShowType=H&Overlap=Y&TopMsgPos=&TopMsg=&RaisePos=\"  align=\"absmiddle\" border=\"0\" class=\"code\" />");


                    sbHtml.Append(" Player: <img id=\"GenImg\" width=\"115\" height=\"25\" src=\"..\\Form\\GenHistoryImg.aspx?CameType=01&Content=" + Common.GetGameResultByGameID(strGameID, "P", "Result8") + "&ShowType=H&Overlap=Y&TopMsgPos=&TopMsg=&RaisePos=\"  align=\"absmiddle\" border=\"0\" class=\"code\" />");

                    /*
                    sbHtml.Append("Dealer:<div  id=\"dvflash_B_" + strGameID + "\">Flash Player 9 required</div>");
                    sbClientCmd.Append("ShowPokerResult('" + htFlash[strGameTypeNo] + "','dvflash_B_" + strGameID + "','" + Common.GetGameResultByGameID(strGameID, "B", "Result8") + "','H','Y');");
                    //player
                    sbHtml.Append("Player:<div  id=\"dvflash_P_" + strGameID + "\">Flash Player 9 required</div>");
                    sbClientCmd.Append("ShowPokerResult('" + htFlash[strGameTypeNo] + "','dvflash_P_" + strGameID + "','" + Common.GetGameResultByGameID(strGameID, "P", "Result8") + "','H','Y');");
                     */

                }
                else if (strAction == "Fold")
                {
                    //只抓閒家的result1,result2
                    sbHtml.Append("Player: <img id=\"GenImg\" width=\"75\" height=\"25\" src=\"..\\Form\\GenHistoryImg.aspx?CameType=01&Content=" + Common.GetGameResultByGameID(strGameID, "P", "12") + "&ShowType=H&Overlap=Y&TopMsgPos=&TopMsg=&RaisePos=\"  align=\"absmiddle\" border=\"0\" class=\"code\" />");

                    /*
                    sbHtml.Append("Player:<div  id=\"dvflash_P_" + strGameID + "\">Flash Player 9 required</div>");
                    sbClientCmd.Append("ShowPokerResult('" + htFlash[strGameTypeNo] + "','dvflash_P_" + strGameID + "','" + Common.GetGameResultByGameID(strGameID, "P", "12") + "','H','Y');");
                     */

                }
            }
            else if (strGameTypeNo == "R0003") //BlackJack
            {
                string strcontent = "";
                string[] Ary_Card = null;
                string strwidth = "";
                string strhight = "";
                strcontent = Common.GetGameResultByGameID(strGameID, "B", "");


                Ary_Card = strcontent.Split(",".ToCharArray());


                if (Ary_Card.Length <= 3)
                {
                    strwidth = "75";
                    strhight = "25";
                }
                else
                {
                    strwidth = "115";
                    strhight = "25";
                }

                sbHtml.Append("Dealer(" + Common.GetGameResultByGameID(strGameID, "B", "Result10") + "): <img id=\"GenImg\" width=\"" + strwidth + "\" height=\"" + strhight + "\" src=\"..\\Form\\GenHistoryImg.aspx?CameType=01&Content=" + Common.GetGameResultByGameID(strGameID, "B", "") + "&ShowType=H&Overlap=Y&TopMsgPos=&TopMsg=&RaisePos=\"  align=\"absmiddle\" border=\"0\" class=\"code\" />");

                strcontent = "";
                Ary_Card = null;
                strcontent = Common.GetBJGameResultByGameID(strGameID, "");
                Ary_Card = strcontent.Split(",".ToCharArray());


                if (Ary_Card.Length <= 3)
                {
                    strwidth = "75";
                    strhight = "25";
                }
                else
                {
                    strwidth = "115";
                    strhight = "25";
                }


                sbHtml.Append(" Player(" + Common.GetBJGameResultByGameID(strGameID, "Result10") + "): <img id=\"GenImg\" width=\"" + strwidth + "\" height=\"" + strhight + "\" src=\"..\\Form\\GenHistoryImg.aspx?CameType=01&Content=" + Common.GetBJGameResultByGameID(strGameID, "") + "&ShowType=H&Overlap=Y&TopMsgPos=&TopMsg=&RaisePos=\"  align=\"absmiddle\" border=\"0\" class=\"code\" />");

               /* 
                //banker
                sbHtml.Append("Dealer(" + Common.GetGameResultByGameID(strGameID, "B", "Result10") + "):<div  id=\"dvflash_B_" + strGameID + "\">Flash Player 9 required</div>");
                sbClientCmd.Append("ShowPokerResult('" + htFlash[strGameTypeNo] + "','dvflash_B_" + strGameID + "','" + Common.GetGameResultByGameID(strGameID, "B", "") + "','H','Y');");
                //player
                sbHtml.Append("Player(" +Common.GetBJGameResultByGameID(strGameID,"Result10") + "):<div  id=\"dvflash_P_" + strGameID + "\">Flash Player 9 required</div>");
                sbClientCmd.Append("ShowPokerResult('" + htFlash[strGameTypeNo] + "','dvflash_P_" + strGameID + "','" + Common.GetBJGameResultByGameID(strGameID, "") + "','H','Y');");
              */   
            }
            else if (strGameTypeNo == "R0012") //Jacks or Better
            {

                DataTable dt_Result = Common.GetJBGameResultByGameID(strGameID.Trim());
                string str_Resutl = "";
                string str_TopMsgPos = "";
                //string str_TopMsg = "";
                string str_RaisePos = "";
         

                if (dt_Result != null && dt_Result.Rows.Count > 0)
                {
                    DataRow[] dr_SelResult = dt_Result.Select("HandNo='2'"); //

                    if (dr_SelResult.Length > 0)
                    {
                        foreach (DataRow dr_row in dr_SelResult)
                        {
                            str_Resutl = "";
                            for (int i = 1; i <= 5; i++)
                            {
                                if (dr_row["Result" + i.ToString()].ToString() == "") break;
                                str_Resutl += (dr_row["Result" + i.ToString()].ToString() + ",");
                            }

                            if (dr_row["Result6"].ToString().Trim() != "")
                            {
                                str_RaisePos = dr_row["Result6"].ToString().Trim();

                                char[] char_RaisePos = str_RaisePos.ToCharArray();
                                str_RaisePos = "";
                                foreach (char str_pos in char_RaisePos)
                                {
                                    str_RaisePos += str_pos.ToString() + ",";
                                }

                                str_RaisePos = str_RaisePos.Substring(0, str_RaisePos.Length - 1);
                            }
                        }
                        str_Resutl = str_Resutl.Substring(0, str_Resutl.Length - 1);

                        sbHtml.Append("<img id=\"GenImg\" width=\"175\" height=\"35\" src=\"..\\Form\\GenHistoryImg.aspx?CameType=01&Content=" + str_Resutl + "&ShowType=H&Overlap=N&TopMsgPos=&TopMsg=&RaisePos=" + str_RaisePos + "\"  align=\"absmiddle\" border=\"0\" class=\"code\" />");
                        /*
                        sbHtml.Append("detail:<div  id=\"dvflash_B_" + strGameID + "\">Flash Player 9 required</div>");
                        sbClientCmd.Append("ShowPokerResult('" + htFlash[strGameTypeNo] + "','dvflash_B_" + strGameID + "','" + str_Resutl + "','H','N','','','','','" + str_RaisePos + "');");
                         */
                    }

                    dr_SelResult = null;
                    str_Resutl = "";
                    str_RaisePos = "";

                     dr_SelResult = dt_Result.Select("Participator='2'"); //

                     if (dr_SelResult.Length > 0)
                     {
                        foreach (DataRow dr_row in dr_SelResult)
                        {
                             str_Resutl = "";
                            str_TopMsgPos="";
                             for (int i = 1; i <= 5; i++)
                             {
                                 if (dr_row["Result" + i.ToString()].ToString() == "") break;
                                 str_Resutl += (dr_row["Result" + i.ToString()].ToString() + ",");
                             }

                             if (dr_row["Result6"].ToString().Trim() != "")
                             {
                                 str_TopMsgPos = dr_row["Result6"].ToString().Trim();

                                 char[] char_TopMsgPos = str_TopMsgPos.ToCharArray();
                                 str_TopMsgPos = "";
                                 foreach (char str_pos in char_TopMsgPos)
                                 {
                                     str_TopMsgPos += str_pos.ToString() + ",";
                                 }

                                 str_TopMsgPos = str_TopMsgPos.Substring(0, str_TopMsgPos.Length - 1);
                                 //str_TopMsg = "Held";
                             }
                        }
                         str_Resutl = str_Resutl.Substring(0, str_Resutl.Length - 1);

                         sbHtml.Append("detail: <img id=\"GenImg\" width=\"175\" height=\"35\" src=\"..\\Form\\GenHistoryImg.aspx?CameType=01&Content=" + str_Resutl + "&ShowType=H&Overlap=N&TopMsgPos=" + str_TopMsgPos + "&TopMsg=&RaisePos=\"  align=\"absmiddle\" border=\"0\" class=\"code\" />");
                         /*
                         sbHtml.Append("detail:<div  id=\"dvflash_B_" + strGameID + "\">Flash Player 9 required</div>");
                         sbClientCmd.Append("ShowPokerResult('" + htFlash[strGameTypeNo] + "','dvflash_B_" + strGameID + "','" + str_Resutl + "','H','N','','','" + str_TopMsgPos + "','" + str_TopMsg + "','');");
                         */
                     }
                     
                     dr_SelResult = null;
                     str_Resutl = "";
                     str_TopMsgPos = "";
                     //str_TopMsg = "";
                }
            }
            else
            {
                sbHtml.Append("Banker: <img id=\"GenImg\" width=\"75\" height=\"25\" src=\"..\\Form\\GenHistoryImg.aspx?CameType=01&Content=" + Common.GetGameResultByGameID(strGameID, "B", "") + "&ShowType=H&Overlap=Y&TopMsgPos=&TopMsg=&RaisePos=\"  align=\"absmiddle\" border=\"0\" class=\"code\" />");

                sbHtml.Append(" Player: <img id=\"GenImg\" width=\"75\" height=\"25\" src=\"..\\Form\\GenHistoryImg.aspx?CameType=01&Content=" + Common.GetGameResultByGameID(strGameID, "P", "") + "&ShowType=H&Overlap=Y&TopMsgPos=&TopMsg=&RaisePos=\"  align=\"absmiddle\" border=\"0\" class=\"code\" />");
                /*
                //banker5
                sbHtml.Append("Banker:<div  id=\"dvflash_B_" + strGameID + "\">Flash Player 9 required</div>");
                sbClientCmd.Append("ShowPokerResult('" + htFlash[strGameTypeNo] + "','dvflash_B_" + strGameID + "','" + Common.GetGameResultByGameID(strGameID, "B", "") + "','H','Y');");
                //player
                sbHtml.Append("Player:<div  id=\"dvflash_P_" + strGameID + "\">Flash Player 9 required</div>");
                sbClientCmd.Append("ShowPokerResult('" + htFlash[strGameTypeNo] + "','dvflash_P_" + strGameID + "','" + Common.GetGameResultByGameID(strGameID, "P", "") + "','H','Y');");
             */
            }
        }
        else if (StringUtil.CStr(htFlash[strGameTypeNo]).IndexOf("Roulette") > 0)
        {
            //Roulette類

            sbHtml.Append("Roulette: <img id=\"GenImg\" width=\"109\" height=\"49\" src=\"..\\Form\\GenHistoryImg.aspx?CameType=02&Content=" + Common.GetGameResultByGameID(strGameID, "", "") + "\"  align=\"absmiddle\" border=\"0\" class=\"code\" />");
            /*
            sbHtml.Append("Roulette:<div  id=\"dvflash_" + strGameID + "\" >Flash Player 9 required</div>");

            sbClientCmd.Append("ShowRouletteResult('" + htFlash[strGameTypeNo] + "','dvflash_" + strGameID + "','" + Common.GetGameResultByGameID(strGameID, "", "") + "');");
            */

        }
        else if (StringUtil.CStr(htFlash[strGameTypeNo]).IndexOf("SicBo") > 0)
        {
            //SicBo類

            sbHtml.Append("SicBo: <img id=\"GenImg\" width=\"115\" height=\"35\" src=\"..\\Form\\GenHistoryImg.aspx?CameType=03&Content=" + Common.GetGameResultByGameID(strGameID, "", "123") + "\"  align=\"absmiddle\" border=\"0\" class=\"code\" />");
            /*
            sbHtml.Append("SicBo:<div  id=\"dvflash_" + strGameID + "\">Flash Player 9 required</div>");

            sbClientCmd.Append("ShowSicBoResult('" + htFlash[strGameTypeNo] + "','dvflash_" + strGameID + "','" + Common.GetGameResultByGameID(strGameID, "", "123") + "');");
             */

        }

        //加資料加入hashtable
        htResult.Add("HTML", sbHtml.ToString());
        htResult.Add("ClientCmd", sbClientCmd.ToString());

        return htResult;
    }

    /// <summary>
    /// Convert number to English 
    /// </summary>
    /// <param name="strNum">Number</param>
    /// <returns>result string</returns>
    public static string ConvertNum2EngStr(string strNum)
    {
        string strResult = "";
        if (strNum == "1")
        {
            strResult = "One";
        }
        else if (strNum == "2")
        {
            strResult = "Two";
        }
        else if (strNum == "3")
        {
            strResult = "Three";
        }
        else if (strNum == "4")
        {
            strResult = "Four";
        }
        else if (strNum == "5")
        {
            strResult = "Five";
        }
        else if (strNum == "6")
        {
            strResult = "Six";
        }
        return strResult;
    }
   
    /// <summary>
    /// Get Upline List
    /// </summary>
    /// <param name="strCustID">Cust Id</param>
    /// <returns>Hashtable</returns>
    public static Hashtable GetUplineList(string strCustID)
    {
        Hashtable htResult = new Hashtable();
        htResult.Add("IDList", "");
        htResult.Add("NameList", "");
        htResult.Add("RoleList", "");
        DataSet ds = null;
        string strIDList = "";
        string strNameList = "";
        string strRoleList = "";
        string strRoleID = "";
        string strUplineID = "";
        int intCount = 0;
        CustomerBO objCustomerBO = null;

        if (strCustID != "")
        {
            //先取得自己
            objCustomerBO = new CustomerBO();
            objCustomerBO.ConnKey = "CS_BackupDB";
            objCustomerBO.CustID = StringUtil.CInt(strCustID);
            ds = objCustomerBO.GetDataSet(objCustomerBO);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow drself in ds.Tables[0].Rows)
                    {
                        strIDList = StringUtil.CStr(drself["CustID"]) + ",";
                        strNameList = StringUtil.CStr(drself["UserName"]) + ",";
                        strRoleList = StringUtil.CStr(drself["RoleId"]) + ",";
                        strRoleID = StringUtil.CStr(drself["RoleId"]);
                        if (strRoleID == "4")
                        {
                            //本身最上層
                            strUplineID = StringUtil.CStr(drself["CustID"]);
                        }
                        else if (strRoleID == "3")
                        {
                            //master agent，上線為super agent
                            strUplineID = StringUtil.CStr(drself["SAgentID"]);
                        }
                        else if (strRoleID == "2")
                        {
                            //agent，上線為master agent
                            strUplineID = StringUtil.CStr(drself["MAgentID"]);
                        }
                        else if (strRoleID == "1")
                        {
                            //member，上線為 agent
                            strUplineID = StringUtil.CStr(drself["AgentID"]);
                        }
                    }
                }

            }

            if (strRoleID != "" && strRoleID != "4" && strUplineID !="")
            {
                while (true)
                {
                    if (strUplineID == "") break;
                    objCustomerBO = new CustomerBO();
                    objCustomerBO.CustID = StringUtil.CInt(strUplineID);
                    ds = objCustomerBO.GetDataSet(objCustomerBO);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            foreach (DataRow drUp in ds.Tables[0].Rows)
                            {
                                strIDList = strIDList +  StringUtil.CStr(drUp["CustID"]) + ",";
                                strNameList = strNameList +  StringUtil.CStr(drUp["UserName"]) + ",";
                                strRoleList = strRoleList + StringUtil.CStr(drUp["RoleId"]) + ",";
                                strRoleID = StringUtil.CStr(drUp["RoleId"]);
                                if (strRoleID == "4")
                                {
                                    //本身最上層
                                    strUplineID = StringUtil.CStr(drUp["CustID"]);
                                }
                                else if (strRoleID == "3")
                                {
                                    //master agent，上線為super agent
                                    strUplineID = StringUtil.CStr(drUp["SAgentID"]);
                                }
                                else if (strRoleID == "2")
                                {
                                    //agent，上線為master agent
                                    strUplineID = StringUtil.CStr(drUp["MAgentID"]);
                                }
                                else if (strRoleID == "1")
                                {
                                    //member，上線為 agent
                                    strUplineID = StringUtil.CStr(drUp["AgentID"]);
                                }
                            }
                        }
                    }

                    intCount++;
                    if (strRoleID == "4") break;
                    //預防設定錯誤變無窮廻圈
                    if (intCount > 10) break;
                }


            }

            //一律加上House
            strIDList = strIDList + "0,";
            strNameList = strNameList + "House,";
            strRoleList = strRoleList + "5,";


            //過濾逗號
            if (strIDList != "") strIDList = strIDList.Substring(0, strIDList.Length - 1);
            if (strNameList != "") strNameList = strNameList.Substring(0, strNameList.Length - 1);
            if (strRoleList != "") strRoleList = strRoleList.Substring(0, strRoleList.Length - 1);
            //放進hashtable
            htResult["IDList"] = strIDList;
            htResult["NameList"] = strNameList;
            htResult["RoleList"] = strRoleList;
        }
        

        return htResult;
    }

    /// <summary>
    /// Update Downline Game Type Data
    /// </summary>
    /// <param name="RequestID">Request By which user</param>
    /// <param name="GameTypeID">Game Type ID</param>
    /// <param name="GameEnablePlay">Game Enable Play</param>
    public static void UpdateDownlineData(string strCustID,string RequestID, string GameTypeID, string GameEnablePlay)
    {

        string RoleID = "";
        CustomerBO objCustomerBO = new CustomerBO();
        CustGameTypeBO objCustGameTypeBO = null;
        Dictionary<string, string> objDictionary = null;
        //先取得自己的Role
        objCustomerBO.ConnKey = "CS_BackupDB";
        objCustomerBO.CustID = StringUtil.CInt(strCustID);
        DataSet ds = objCustomerBO.GetDataSet(objCustomerBO);
        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    RoleID = dr["RoleId"].ToString();
                }
            }
        }
        //找出所有的下屬
        if (RoleID != "")
        {
            objCustomerBO = new CustomerBO();
            objCustomerBO.ConnKey = "CS_BackupDB";
            if (RoleID == "4") objCustomerBO.SAgentID = StringUtil.CInt(strCustID);
            if (RoleID == "3") objCustomerBO.MAgentID = StringUtil.CInt(strCustID);
            if (RoleID == "2") objCustomerBO.AgentID = StringUtil.CInt(strCustID);
            if (RoleID == "1") objCustomerBO.CustID = StringUtil.CInt(strCustID);
            DataSet dsMem = objCustomerBO.GetDataSet(objCustomerBO);
            if (dsMem != null)
            {
                if (dsMem.Tables.Count > 0)
                {
                    foreach (DataRow drMem in dsMem.Tables[0].Rows)
                    {
                        objCustGameTypeBO = new CustGameTypeBO();
                        objCustGameTypeBO.ConnKey =  "CS_BackupDB";
                        objDictionary = new Dictionary<string, string>();
                        objDictionary.Add("GameTypeID", GameTypeID);
                        objDictionary.Add("CustID", drMem["CustID"].ToString());
                        objDictionary.Add("GameEnablePlay", GameEnablePlay);
                        objDictionary.Add("GameRequestBy", RequestID);

                        objDictionary.Add("ErrCode", "");
                        objDictionary.Add("ErrMsg", "");

                        objCustGameTypeBO.UpdGameEnablePlay(objDictionary);
                    }
                }
            }

            objCustGameTypeBO = new CustGameTypeBO();
            objCustGameTypeBO.ConnKey = "CS_BackupDB";
            objDictionary = new Dictionary<string, string>();
            objDictionary.Add("GameTypeID", GameTypeID);
            objDictionary.Add("CustID", strCustID);
            objDictionary.Add("GameEnablePlay", GameEnablePlay);
            objDictionary.Add("GameRequestBy", RequestID);
            objDictionary.Add("ErrCode", "");
            objDictionary.Add("ErrMsg", "");
            objCustGameTypeBO.UpdGameEnablePlay(objDictionary);

            dsMem = null;
        }

        objCustomerBO = null;
        objCustGameTypeBO = null;
        objDictionary = null;
        ds = null;
        
        objCustGameTypeBO = null;
    }

    public static bool GetUplineIsEnablePT(string strCustID)
    {
        bool blnResult = false;
        DataSet ds = null;

        string strRoleID = "";
        string strUplineID = "";

        CustomerBO objCustomerBO = null;
        CustSettingBO objCustSettingBO = null;

        if (strCustID != "")
        {
            //先取得自己
            objCustomerBO = new CustomerBO();
            objCustomerBO.ConnKey = "CS_BackupDB";
            objCustomerBO.CustID = StringUtil.CInt(strCustID);
            ds = objCustomerBO.GetDataSet(objCustomerBO);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow drself in ds.Tables[0].Rows)
                    {
                        strRoleID = StringUtil.CStr(drself["RoleId"]);
                        if (strRoleID == "4")
                        {
                            //本身最上層
                            strUplineID = StringUtil.CStr(drself["CustID"]);
                        }
                        else if (strRoleID == "3")
                        {
                            //master agent，上線為super agent
                            strUplineID = StringUtil.CStr(drself["SAgentID"]);
                        }
                        else if (strRoleID == "2")
                        {
                            //agent，上線為master agent
                            strUplineID = StringUtil.CStr(drself["MAgentID"]);
                        }
                        else if (strRoleID == "1")
                        {
                            //member，上線為 agent
                            strUplineID = StringUtil.CStr(drself["AgentID"]);
                        }
                    }

                }

            }

            //if (strRoleID != "" && strRoleID != "4" && strUplineID != "")
            if (strRoleID != ""  && strUplineID != "")
            {
                objCustSettingBO = new CustSettingBO();
                objCustSettingBO.ConnKey = "CS_BackupDB";
                objCustSettingBO.CustSettingID = StringUtil.CInt(strUplineID);
                ds = objCustSettingBO.GetDataSet(objCustSettingBO);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow drUp in ds.Tables[0].Rows)
                        {
                            if (drUp["IsEnablePT"].ToString() == "True")
                            {
                                blnResult = true;
                            }

                        }
                    }

                }
                objCustSettingBO = null;
            }
            //假如是Super預設為True
            if (strRoleID == "4") blnResult = true;

        }
        objCustomerBO = null;
        objCustSettingBO = null;
        ds = null;
        return blnResult;
    }

    public static bool GetUplineIsGameRequestLock(string strCustID,string strGameTypeID)
    {
        bool blnResult = false;
        DataSet ds = null;

        string strRoleID = "";
        string strUplineID = "";

        CustomerBO objCustomerBO = null;
        CustGameTypeBO objCustGameTypeBO = null;

        if (strCustID != "")
        {
            //先取得自己
            objCustomerBO = new CustomerBO();
            objCustomerBO.ConnKey = "CS_BackupDB";
            objCustomerBO.CustID = StringUtil.CInt(strCustID);
            ds = objCustomerBO.GetDataSet(objCustomerBO);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow drself in ds.Tables[0].Rows)
                    {
                        strRoleID = StringUtil.CStr(drself["RoleId"]);
                        if (strRoleID == "4")
                        {
                            //本身最上層
                            strUplineID = StringUtil.CStr(drself["CustID"]);
                        }
                        else if (strRoleID == "3")
                        {
                            //master agent，上線為super agent
                            strUplineID = StringUtil.CStr(drself["SAgentID"]);
                        }
                        else if (strRoleID == "2")
                        {
                            //agent，上線為master agent
                            strUplineID = StringUtil.CStr(drself["MAgentID"]);
                        }
                        else if (strRoleID == "1")
                        {
                            //member，上線為 agent
                            strUplineID = StringUtil.CStr(drself["AgentID"]);
                        }
                    }

                }

            }

            if (strRoleID != "" && strUplineID != "")
            {

                objCustGameTypeBO = new CustGameTypeBO();
                objCustGameTypeBO.ConnKey = "CS_BackupDB";
                objCustGameTypeBO.CustID = StringUtil.CInt(strUplineID);
                objCustGameTypeBO.GameTypeID = StringUtil.CInt(strGameTypeID);
                ds = objCustGameTypeBO.GetDataSet(objCustGameTypeBO);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow drUp in ds.Tables[0].Rows)
                        {
                            if (drUp["IsGameRequestLock"].ToString() == "True")
                            {
                                blnResult = true;
                            }

                        }
                    }
                }
                objCustGameTypeBO = null;
            }
        }
        objCustomerBO = null;
        objCustGameTypeBO = null;
        return blnResult;
    }


    public static string GetUplineGameRequestLockID(string strCustID, string strGameTypeID)
    {
        string strResult = "";
        DataSet ds = null;

        string strRoleID = "";
        string strUplineID = "";

        CustomerBO objCustomerBO = null;
        CustGameTypeBO objCustGameTypeBO = null;

        if (strCustID != "")
        {
            //先取得自己
            objCustomerBO = new CustomerBO();
            objCustomerBO.ConnKey = "CS_BackupDB";
            objCustomerBO.CustID = StringUtil.CInt(strCustID);
            ds = objCustomerBO.GetDataSet(objCustomerBO);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow drself in ds.Tables[0].Rows)
                    {
                        strRoleID = StringUtil.CStr(drself["RoleId"]);
                        if (strRoleID == "4")
                        {
                            //本身最上層
                            strUplineID = StringUtil.CStr(drself["CustID"]);
                        }
                        else if (strRoleID == "3")
                        {
                            //master agent，上線為super agent
                            strUplineID = StringUtil.CStr(drself["SAgentID"]);
                        }
                        else if (strRoleID == "2")
                        {
                            //agent，上線為master agent
                            strUplineID = StringUtil.CStr(drself["MAgentID"]);
                        }
                        else if (strRoleID == "1")
                        {
                            //member，上線為 agent
                            strUplineID = StringUtil.CStr(drself["AgentID"]);
                        }
                    }

                }

            }

            if (strRoleID != "" && strUplineID != "")
            {

                objCustGameTypeBO = new CustGameTypeBO();
                objCustGameTypeBO.ConnKey = "CS_BackupDB";
                objCustGameTypeBO.CustID = StringUtil.CInt(strUplineID);
                objCustGameTypeBO.GameTypeID = StringUtil.CInt(strGameTypeID);
                ds = objCustGameTypeBO.GetDataSet(objCustGameTypeBO);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow drUp in ds.Tables[0].Rows)
                        {
                            if (drUp["IsGameRequestLock"].ToString() == "True")
                            {
                                strResult = drUp["GameRequestBy"].ToString();
                            }

                        }
                    }
                }
                objCustGameTypeBO = null;
            }
        }
        objCustomerBO = null;
        objCustGameTypeBO = null;
        return strResult;
    }

    /// <summary>
    /// Get Upline ID
    /// </summary>
    /// <param name="strCustID">Cust Id</param>
    /// <returns>Upline ID</returns>
    public static string GetUplineID(string strCustID)
    {

        DataSet ds = null;

        string strRoleID = "";
        string strUplineID = "";
        CustomerBO objCustomerBO = null;

        if (strCustID != "")
        {
            //先取得自己
            objCustomerBO = new CustomerBO();
            objCustomerBO.ConnKey = "CS_BackupDB";
            objCustomerBO.CustID = StringUtil.CInt(strCustID);
            ds = objCustomerBO.GetDataSet(objCustomerBO);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow drself in ds.Tables[0].Rows)
                    {

                        strRoleID = StringUtil.CStr(drself["RoleId"]);
                        if (strRoleID == "4")
                        {
                            //本身最上層
                            strUplineID = StringUtil.CStr(drself["CustID"]);
                        }
                        else if (strRoleID == "3")
                        {
                            //master agent，上線為super agent
                            strUplineID = StringUtil.CStr(drself["SAgentID"]);
                        }
                        else if (strRoleID == "2")
                        {
                            //agent，上線為master agent
                            strUplineID = StringUtil.CStr(drself["MAgentID"]);
                        }
                        else if (strRoleID == "1")
                        {
                            //member，上線為 agent
                            strUplineID = StringUtil.CStr(drself["AgentID"]);
                        }
                    }
                }

            }


       }
       objCustomerBO = null;

       return strUplineID;
    }

    /// <summary>
    /// Get Upline CustGameKind PTRange
    /// </summary>
    /// <param name="CustID">self id</param>
    /// <param name="GameCodeNo"></param>
    /// <param name="GameKind"></param>
    /// <returns></returns>
    public static DataRow GetUplineCustGameKindPTRange(string CustID,string GameCodeNo,string GameKind)
    {
        DataRow drResult = null;
        CustGameKindBO objCustGameKindBO = null;
        Dictionary<string, string> objDictionary = null;
        DataTable dt = null;
        string strUplineID = "";
        try
        {
            //Get Upline ID
            strUplineID = Common.GetUplineID(CustID);
            if (strUplineID != "")
            {
                objCustGameKindBO = new CustGameKindBO();
                objCustGameKindBO.ConnKey = "CS_BackupDB";
                objDictionary = new Dictionary<string, string>();
                objDictionary.Add("CustID", strUplineID);
                objDictionary.Add("CodeNo", GameCodeNo);
                objDictionary.Add("GameKind", GameKind);
                objDictionary.Add("ErrCode", "");
                objDictionary.Add("ErrMsg", "");
                dt = objCustGameKindBO.GetPositionTaking(objDictionary);
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        drResult = dr;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            Sunplus.Agent.Utilities.Logs.Write(ex.Message + ex.StackTrace);
            throw ex;
        }
        finally
        {
            objCustGameKindBO = null;
            objDictionary = null;            
        }

        return drResult;
    }

    public static DataRow GetUplineCustGameTypePTRange(string CustID, string GameCodeNo, string GameTypeID)
    {
        DataRow drResult = null;
        CustGameTypeBO objCustGameTypeBO = null;
        Dictionary<string, string> objDictionary = null;
        DataTable dt = null;
        string strUplineID = "";
        try
        {
            //Get Upline ID
            strUplineID = Common.GetUplineID(CustID);
            if (strUplineID != "")
            {
                objCustGameTypeBO = new CustGameTypeBO();
                objCustGameTypeBO.ConnKey = "CS_BackupDB";
                objDictionary = new Dictionary<string, string>();
                objDictionary.Add("CustID", strUplineID);
                objDictionary.Add("CodeNo", GameCodeNo);
                objDictionary.Add("GameTypeID", GameTypeID);
                objDictionary.Add("ErrCode", "");
                objDictionary.Add("ErrMsg", "");
                dt = objCustGameTypeBO.GetPositionTaking(objDictionary);
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        drResult = dr;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            Sunplus.Agent.Utilities.Logs.Write(ex.Message + ex.StackTrace);
            throw ex;
        }
        finally
        {
            objCustGameTypeBO = null;
            objDictionary = null;
        }

        return drResult;
    }

    /*public static string GetIBCCustIDByCustID(string strCustID)
    {
        string strIBCCustID = "";
        CustomerBO objCustomerBO = new CustomerBO();
        objCustomerBO.CustID = StringUtil.CInt(strCustID);
        DataSet ds = objCustomerBO.GetDataSet(objCustomerBO);
        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    strIBCCustID = StringUtil.CStr(dr["IBCCustID"]);
                }
            }
        }
        return strIBCCustID;
    }*/

    public static string GetLocalIP()
    {
        string strIP = "";
        IPAddress[] ipAddress = NetworkUtil.GetMachineIP();

        for (int i = 0; i < ipAddress.Length; i++)
        {
            strIP = StringUtil.CStr(ipAddress[i]);
            //如果是合法的IP才加入
            if (NetworkUtil.IsValidIPv4(strIP))
            {
                strIP = ipAddress[i].ToString();
                break;
            }
        }
        return strIP;
    }

}
