using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fanex.Age.UI;
using CasinoLibrary.SBAgent;
using System.Data;
using Sunplus.Agent.Model.BL.Casino;
using Sunplus.Agent.Utilities;
using Fanex.Age.UI.Components;
using Sunplus.Agent.Model.BL.AccountInfo;
using Sunplus.Agent.Model.Entities;



namespace Sunplus.Agent.UI._AccountInfo
{
    public partial class CustomerEditInfo_Page : Fanex.Age.UI.TplPage
    {
        public Hashtable jsVars = new Hashtable();
        public Hashtable htbQueryString = new Hashtable();
        EditCustomerInfo_Template tpl = new EditCustomerInfo_Template();
        Dictionary<String, DataTable> viewData = new Dictionary<string, DataTable>();
        int custid = 0;
        public override void RenderContent(HtmlTextWriter writer)
        {
            DataTable customerData = (View as Dictionary<String, DataTable>)["CustomerData"];
            DataRowCollection dr = customerData.Rows;

            jsRegister("emailnotvalid", Resources.Lang.emailnotvalid);
            jsRegister("updatesuccessfull", Resources.Lang.updatesuccessfull);
            jsRegister("isSync", Request.QueryString["isSyncCasino"]);

            RegisterClientVariables();

            tpl.fname_val = HttpUtility.HtmlEncode(dr[0]["firstname"].ToString());
            tpl.lname_val = HttpUtility.HtmlEncode(dr[0]["lastname"].ToString());
            tpl.phone_val = HttpUtility.HtmlEncode(dr[0]["phone"].ToString());
            tpl.mobile_val = HttpUtility.HtmlEncode(dr[0]["mobilephone"].ToString());
            tpl.email_val = HttpUtility.HtmlEncode(dr[0]["email"].ToString());
            tpl.fax_val = HttpUtility.HtmlEncode(dr[0]["fax"].ToString());
            tpl.lbleditinfo = Resources.Lang.lblEditInfo + ' ' + dr[0]["username"].ToString();
            tpl.custId = custid.ToString();
            tpl.TargetCustName = dr[0]["username"].ToString();
            string sync = Request.QueryString["isSyncCasino"];
            writer.Write(tpl);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            AccountInfoBO bo = new AccountInfoBO(CurrentUserSession.NetSessionId);
            DataTable dtb = bo.GetCustomerSelUserInfo(custid);
            viewData.Add("CustomerData", dtb);
            return viewData;
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            custid = GetIntRequest("custid", CurrentUserSession.CustId);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (custid == 0)
            {
                return new ErrorPage_Page("Invalid user bet id!");
            }

            return null;
        }

        public void jsRegister(object name, object value)
        {
            jsVars.Add(name, value);
        }

        public void RegisterClientVariables()
        {
            foreach (DictionaryEntry item in jsVars)
            {
                RegisterJavascriptObject(item.Key, item.Value);
            }
        }
    }
}
