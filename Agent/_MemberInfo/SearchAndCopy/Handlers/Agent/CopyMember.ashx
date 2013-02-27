<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers.Agent.CopyMember_Handler" %>

/*
 * Created 20120224@Jeff - Copy user
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers.Agent
{
    using System;
    using System.Web;
    using System.Data;
    using System.Collections;

    using Fanex.Age.UI;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.BL.MemberInfo.SearchAndCopy;
    using Sunplus.Agent.Model.Security;

    public class CopyMember_Handler : TplHandler
    {
        private string userName = string.Empty;
        private string userNameDetail = string.Empty;
        private string password = string.Empty;
        private string firstName = string.Empty;
        private string lastName = string.Empty;
        private string phone = string.Empty;
        private string mobilePhone = string.Empty;
        private string fax = string.Empty;
        private float giveCredit = 0;
        private int custId = 0;
        private int custIdExist = 0;
        private string errMsg = string.Empty;
        private int errCode = 0;
        private string code = string.Empty;
        private Hashtable infoCustomer = new Hashtable();

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            this.userNameDetail = Request.Form["userName"];
            this.password = HttpUtility.HtmlDecode(Request.Form["password"]);
            this.firstName = HttpUtility.HtmlDecode(Request.Form["firstName"]);
            this.lastName = HttpUtility.HtmlDecode(Request.Form["lastName"]);
            this.phone = HttpUtility.HtmlDecode(Request.Form["phone"]);
            this.mobilePhone = HttpUtility.HtmlDecode(Request.Form["mobilePhone"]);
            this.fax = HttpUtility.HtmlDecode(Request.Form["fax"]);
            float.TryParse(Request.Form["giveCredit"], out this.giveCredit);
            int.TryParse(Request.Form["custId"], out this.custIdExist);
            this.code = Request.Form["code"];
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if ((this.custIdExist == 0) || string.IsNullOrEmpty(this.userNameDetail) || string.IsNullOrEmpty(this.password) ||
                !CurrentUserSession.SubPageAccess.UpdateAccount || CurrentUserSession.IsInternal)
            {
                errCode = 101;
                errMsg = Resources.Lang.lblNotAuthorized;
                return new Result(errCode, errMsg);
            }
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            HttpFrequenceRule rule1 = new HttpFrequenceRule("CopyMember.ashx", 20, TimeSpan.FromDays(2));
            if (UserSession.Current.IsInternal || !UserSession.Current.SubPageAccess.CreateAccount)
            {
                errCode = (int)ErrorType.AccessDenied;
                errMsg = Resources.Lang.lblNotAuthorized;

                return new Result(errCode, errMsg);
            }

            if(rule1.IsViolated)
            {
                if (code != Sunplus.Agent.Model.Entities.UserSession.Current.CaptchaCode)
                {
                    errCode = 602;
                    errMsg = Resources.Lang.errInvalidCaptcha;
                    return new Result(errCode, errMsg);
                }
                else
                {
                    rule1.Reset();
                }
            }
            
            this.userName = UserSession.Current.UserName + this.userNameDetail;
            this.infoCustomer.Add("username", this.userName);
            this.infoCustomer.Add("userpwd", DNAide.Utils.Encryption.CfsEnCode(this.password.Trim()));
            this.infoCustomer.Add("firstname", this.firstName);
            this.infoCustomer.Add("lastname", this.lastName);
            this.infoCustomer.Add("phone", this.phone);
            this.infoCustomer.Add("mobilephone", this.mobilePhone);
            this.infoCustomer.Add("fax", this.fax);
            this.infoCustomer.Add("credit", this.giveCredit);
            this.infoCustomer.Add("copyfromcustid", this.custIdExist);

            Hashtable result = new Hashtable();
            SearchAndCopyBO searchAndCopyBo = new SearchAndCopyBO(UserSession.Current.NetSessionId);
            if (Session["HashtableSportbook"] == null)
            {
                DataTable dtb = new DataTable();

                dtb = searchAndCopyBo.GetSportBookSettingMember(this.custIdExist);
                DataRow settingCustomer = dtb.Rows[0];

                var checkConstraintData = new CheckConstraintDataForSportBook();

                checkConstraintData.GenerateInfoAccount(settingCustomer, ref this.infoCustomer);
                checkConstraintData.GenerateBettingLimit(settingCustomer, ref this.infoCustomer);
                checkConstraintData.GeneratePositionTaking(settingCustomer, ref this.infoCustomer);

                result = searchAndCopyBo.InsertMember(this.infoCustomer);
            }
            else
            {
                foreach (DictionaryEntry entry in (Hashtable)Session["HashtableSportbook"])
                {
                    if (!this.infoCustomer.ContainsKey(entry.Key))
                    {
                        this.infoCustomer.Add(entry.Key, entry.Value);
                    }
                }
                result = searchAndCopyBo.InsertMember(this.infoCustomer);
            }

            int.TryParse(result["custid"].ToString(), out custId);
            if (custId > 0)
            {
                errCode = 999;
                errMsg = custId.ToString();
                rule1.Hit();

                return new Result(errCode, errMsg);
            }

            return new Result((int)result["errcode"], result["errmsg"].ToString());
        }

        public override void RenderResult(ref JSONData data)
        {
            Result r = View as Result;
            data.AsResult(r);
        }

    }
}
