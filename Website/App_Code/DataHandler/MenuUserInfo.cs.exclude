using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using DNAide.Core;
using DNAide.Utils;
using DNAide.Web;
using Fanex.Data.DNAide;
using Sunplus.Agent.Utilities;

/// <summary>
/// Menu User Info DAL
/// </summary>
///
namespace IBCAgent
{
    public class UserInfo
    {
        public static DataRowCollection CustomerSelOnline(int custId)
        {
            DbResult rs = null;

            Hashtable param = new Hashtable();
            param.Add("custid", custId);
            rs = DNAide.Data.DbHelper.Execute("Acc_CustomerSelOnline", param);

            if (rs != null)
                return rs.ResultSet.Tables[0].Rows;
            return null;
        }

        /// <summary>
        /// Just get customer info only
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static DbResult GetCustomerByInfo(int custid)
        {
            Hashtable param = new Hashtable();
            param.Add("custid", custid);
            return DNAide.Data.DbHelper.Execute("Age_CustomerSelUserInfo", param);
        }

        public static int UpdateCustomerByInfo(int custid, string firstName, string lastName, string phone, string mobilePhone,
            string email, string fax,
            int userId, int subAccId, string langChar, ref string errorMsg)
        {
            Hashtable param = new Hashtable();
            param.Add("custid", custid);
            param.Add("firstname", firstName);
            param.Add("lastname", lastName);
            param.Add("phone", phone);
            param.Add("mobilephone", mobilePhone);
            param.Add("email", email);
            param.Add("fax", fax);
            //default
            param.Add("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
            param.Add("userid", userId);
            param.Add("discasino", Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledCasino);
            param.Add("subaccid", subAccId);
            param.Add("language", langChar);
            param.Add("errorcode", 0);
            param.Add("errormsg", "");

            DbResult dbr = DNAide.Data.DbHelper.Execute("Age_CustomerUpdUserInfo", param);
            errorMsg = dbr.Outputs["errormsg"].ToString();

            int errCode = CastAide.AsInt32(dbr.Outputs["errorcode"], -1);

            return errCode;
        }

        public static DbResult SelectCommisionByCustomer(int custid, int userid)
        {
            Hashtable param = new Hashtable();
            param.Add("custid", custid);
            param.Add("userid", userid);
            return DNAide.Data.DbHelper.Execute("Age_CustomerSelUserComm", param);
        }

        public static int UpdateCommissionByCustomer(int custid,
            double playerDiscountVal, double playerDiscountcsVal, double discount1x2, double discountnumber,
            int userId, int subAccId, string langChar, ref string errorMsg)
        {
            Hashtable param = new Hashtable();
            param.Add("custid", custid);
            param.Add("playerdiscount", playerDiscountVal);
            param.Add("playerdiscountcs", playerDiscountcsVal);
            param.Add("playerdiscount1x2", discount1x2);
            param.Add("playerdiscountnumber", discountnumber);
            //default
            param.Add("userid", userId);
            param.Add("subaccid", subAccId);
            param.Add("language", langChar);
            param.Add("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
            param.Add("errcode", 0);
            param.Add("errmsg", "");
            DbResult dbr = DNAide.Data.DbHelper.Execute("Age_CustomerUpdUserComm", param);
            errorMsg = dbr.Outputs["errmsg"].ToString();

            return CastAide.AsInt32(dbr.Outputs["errcode"], -1);
        }

        public static int UpdateCommissionByAgent(int custid, int roleId,
            double groupa, double groupb, double groupc, double groupd, double discount1x2, double discountcs, double discountnumnber,
            int userId, int subAccId, string langChar, ref string errorMsg)
        {
            Hashtable param = new Hashtable();
            param.Add("custid", custid);
            param.Add("groupa", groupa);
            param.Add("groupb", groupb);
            param.Add("groupc", groupc);
            param.Add("groupd", groupd);
            param.Add("discount1x2", discount1x2);
            param.Add("discountcs", discountcs);
            param.Add("discountnumber", discountnumnber);

            //default
            param.Add("userid", userId);
            param.Add("subaccid", subAccId);
            param.Add("language", langChar);
            param.Add("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
            param.Add("errcode", 0);
            param.Add("errmsg", "");

            DbResult dbr = null;
            if (roleId == 2) //agent
            {
                dbr = DNAide.Data.DbHelper.Execute("Age_CustomerUpdAgentComm", param);
            }
            else if (roleId == 3) //master
            {
                dbr = DNAide.Data.DbHelper.Execute("Age_CustomerUpdMasterComm", param);
            }

            errorMsg = dbr.Outputs["errmsg"].ToString();

            return CastAide.AsInt32(dbr.Outputs["errcode"], -1);
        }

        public static DbResult SelectBetSettingByCustomer(int custid, int userid)
        {
            Hashtable param = new Hashtable();
            param.Add("custid", custid);
            param.Add("userid", userid);
            return DNAide.Data.DbHelper.Execute("Age_CustomerSelBetSetting", param);
        }

        public static int UpdateMemberBetSetting(Hashtable param, ref string errorMsg)
        {
            DbResult dbr = DNAide.Data.DbHelper.Execute("Age_CustomerUpdMemBetSetting", param);
            errorMsg = dbr.Outputs["errmsg"].ToString();

            return CastAide.AsInt32(dbr.Outputs["errcode"], -1);
        }
    }
}