// --------------------------------------------------------------------------
// <copyright file="AccountController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.29">Created</change>
//   <change who="Lewis.Le" date="2013.4.4">Added action: Menu</change>
// </history>

namespace Apollo.Age.Account.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Apollo.Age.Account.Criterias;
    using Apollo.Age.Account.Services;
    using Apollo.Age.Account.ViewModels;
    using Apollo.Age.Web;
    using Sunplus.Agent.Model.Entities;

    public class AccountController : BaseController
    {
        private readonly AccountService _accountService;
        private int _agentId;
        private int _pageIndex;
        private int _totalPages;
        private string _username;
        private int _pageSize;

        public AccountController()
        {
            _accountService = new AccountService();
        }

        public ActionResult Menu()
        {
            return View(SiteMap.RootNode);
        }

        public ActionResult AccountInfo(string userName, short? isMember, int? custId)
        {
            bool ismember = false;
            int custid = -1;
            string username = string.Empty;

            if (string.IsNullOrWhiteSpace(userName) && !custId.HasValue)
            {
                userName = HttpContext.Session["userName"] != null
                    ? HttpContext.Session["userName"].ToString() : string.Empty;
                ismember = HttpContext.Session["isMember"] != null
                    ? (HttpContext.Session["isMember"].ToString() == "1") : false;
            }

            CustomerInfoViewModel customerInfo = null;
            if (!string.IsNullOrWhiteSpace(userName) || custId.HasValue)
            {
                HttpContext.Session["userName"] = userName;
                HttpContext.Session["isMember"] = isMember;

                ismember = isMember == 1;
                custid = custId.HasValue ? custId.Value : -1;
                username = userName;

                customerInfo = _accountService.GetCustomerInfo(ismember, custid, username, base.AgentId);
                if (customerInfo == null)
                {
                    customerInfo = new CustomerInfoViewModel()
                    {
                        CurrentAgentId = base.AgentId,
                        CustId = -1,
                        IsMember = ismember,
                        Username = username,
                    };
                }
                else
                {
                    customerInfo.CurrentAgentId = base.AgentId;
                }
            }

            return View(customerInfo);
        }

        public ViewResult AgentTreeList(int id)
        {
            ViewBag.AgentId = id;
            return View();
        }

        public ViewResult TreeViewList()
        {
            ViewBag.AgentId = base.AgentId;
            return View();
        }

        public ActionResult SignOut()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return Redirect("~/_Authorization/SignIn.aspx");
        }

        public JsonResult TreeNodeData(int id)
        {
            var customerList = _accountService.GetDirectDownline(id);
            return Json(customerList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSuggestValues(string term, bool isMember)
        {
            var listValue = _accountService.GetCustomerSearch(isMember, term, base.AgentId)
                    .Select(x => x.Username).ToList();

            return Json(listValue, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAgentDownlinePath(int agentId)
        {
            var listValue = _accountService.GetAgentDownlinePath(base.AgentId, agentId).ToList();

            return Json(listValue, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AgentList(int? agentId, int? pageIndex, int? totalPages, string userName, int? pageSize)
        {
            _agentId = agentId.HasValue ? agentId.Value : UserSession.Current.AgentId;
            _pageIndex = pageIndex.HasValue ? pageIndex.Value : 1;
            _totalPages = totalPages.HasValue ? totalPages.Value : 0;
            _username = string.IsNullOrWhiteSpace(userName) ? string.Empty : userName;
            _pageSize = pageSize.HasValue ? pageSize.Value : 50;

            AgentListCriteria criteria = new AgentListCriteria()
            {
                AgentId = _agentId,
                PageIndex = _pageIndex,
                TotalPages = _totalPages,
                UserName = _username,
                PageSize = _pageSize
            };

            IEnumerable<AgentListViewModel> models = _accountService.GetAgentList(ref criteria);

            return Json(new
            {
                TotalPage = criteria.TotalPages,
                ListData = models
            });
        }

        [HttpPost]
        public ActionResult CustomerList(int agentId, int pageIndex, int totalPages, string userName, int pageSize)
        {
            CustomerListCriteria criteria = new CustomerListCriteria()
            {
                AgentId = agentId,
                PageIndex = pageIndex,
                TotalPages = totalPages,
                UserName = userName,
                PageSize = pageSize
            };

            IEnumerable<CustomerListViewModel> models = _accountService.GetCustomerList(ref criteria);

            return Json(new
            {
                TotalPage = criteria.TotalPages,
                ListData = models
            });
        }

        //public JsonResult ReCheckIn()
        //{
        //    IUserSession userSession = UserSession.Current;
        //    Result result = new Result(0, string.Empty);
        //    if (PermissionService.IsKickOut(userSession.AgentId, userSession.UserId) && !AppSettings.IsLocal)
        //    {
        //        result.ErrCode = (int)ErrorType.KickedOut;
        //    }
        //    return Json(result);

        //}
    }
}