<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._AccountInfo.QueryUserName_Handler" %>

namespace Sunplus.Agent.UI._AccountInfo
{
    using System;
    using System.Web;
    using System.Data;
    using Fanex.Age.UI;
    using Apollo.Age.Services.Interface;
    using Apollo.Age.Services.Impl;
    using Sunplus.Agent.Model.BL.AccountInfo;
    using Apollo.Age.Criterias.Accounts;
    using System.Collections.Generic;    
    using Apollo.Age.Entities;
    using System.Web.SessionState;
    using System.Collections;
    
    public class QueryUserName_Handler : TplHandler, IReadOnlySessionState
    {
        private string query = string.Empty;
        private bool isMember = false;

        public QueryUserName_Handler()
        {
            IsValidateRequest = false;
        }

        public override void RenderResult(ref JSONData data)
        {
            List<Customer> lstSuggest = View as List<Customer>;

            int count = lstSuggest.Count;

            object[] suggessions = new object[count];

            for (int i = 0; i < count; i++)
            {
                suggessions[i] = new object[] { HttpUtility.HtmlEncode(lstSuggest[i].Username) };
            }

            data.Add("query", query);
            data.Add("suggestions", suggessions);
        }


        public override void ProcessRequest(ref Hashtable p)
        {
            this.query = Request.QueryString["query"];
            this.isMember = Request.QueryString["isMember"] == "1";
        }

        public override object ValidateRequest(Hashtable p)
        {
            if (string.IsNullOrEmpty(query))
            {
                return new Result(ErrorType.AccessDenied, string.Empty);
            }

            return null;
        }

        public override object InvokeData(Hashtable p)
        {
            ICustomerService service = new CustomerService();
            CustomerSearchCriteria criteria = new CustomerSearchCriteria()
            {
                AgentId = CurrentUserSession.AgentId,
                IsMember = this.isMember,
                UserName = this.query
            };

            var lstSuggest = service.GetCustomerSearch(criteria);
            return lstSuggest;
        }
    }
}