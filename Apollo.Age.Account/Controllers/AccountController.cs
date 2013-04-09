// --------------------------------------------------------------------------
// <copyright file="AccountController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.29">Created</change>
//   <change who="Lewis.Le" date="2013.4.4">Added action: Menu</change>
//   <change who="Quinn.Vo" date="2013.4.9">Added action: AccountInfo, AgentTreeList, TreeViewList, GetDataByNote, GetSuggestValues, GetAgentDownlinePath</change>
//   <change who="Quinn.Vo" date="2013.4.9">Added action: MemberDownline </change>
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
    using Apollo.MvcPaging;
    using Apollo.Age.Entities;

    public class AccountController : BaseController
    {
        private const int PAGEINDEX = 0; // Current index page
        private const int PAGESIZE = 50; // Maximum items in a page
        private const int TOTALPAGE = 0; // Maximum page total

        private IList<Member> allProducts = new List<Member>();

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
            ViewBag.RootNode = SiteMap.RootNode;
            return View(base.CurrentSession);
        }

        public ActionResult MemberDownline(int custId, int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            int totalPage = 0;
            var memberList = _accountService.GetMemberDownline(custId, currentPageIndex + 1, PAGESIZE, ref totalPage);

            return View(memberList.ToPagedList(currentPageIndex, PAGESIZE, totalPage * PAGESIZE));
        }

        public ActionResult AccountInfo(string userName, short? isMember, int? custId)
        {
            bool ismember = false;
            int custid = -1;

            if (string.IsNullOrWhiteSpace(userName) && !custId.HasValue)
            {
                userName = HttpContext.Session["userName"] != null
                    ? HttpContext.Session["userName"].ToString() : string.Empty;
                isMember = HttpContext.Session["isMember"] != null
                    ? Fanex.Common.Converter.To<short>(HttpContext.Session["isMember"].ToString())
                    : Fanex.Common.Converter.To<short>("0");
            }

            CustomerInfoViewModel customerInfo = null;
            if (!string.IsNullOrWhiteSpace(userName) || custId.HasValue)
            {
                HttpContext.Session["userName"] = userName;
                HttpContext.Session["isMember"] = isMember;

                ismember = isMember == 1;
                custid = custId.HasValue ? custId.Value : -1;

                customerInfo = _accountService.GetCustomerInfo(ismember, custid, userName, this.CurrentSession.AgentId);
                if (customerInfo == null)
                {
                    customerInfo = new CustomerInfoViewModel()
                    {
                        CurrentAgentId = this.CurrentSession.AgentId,
                        CustId = -1,
                        IsMember = ismember,
                        Username = userName,
                    };
                }
                else
                {
                    customerInfo.CurrentAgentId = this.CurrentSession.AgentId;
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
            ViewBag.AgentId = this.CurrentSession.AgentId;
            return View();
        }

        public ActionResult SignOut()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return Redirect("~/_Authorization/SignIn.aspx");
        }

        public JsonResult GetDataByNote(int id)
        {
            var customerList = _accountService.GetDirectDownline(id);
            return Json(customerList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSuggestValues(string term, bool isMember)
        {
            var listValue = _accountService.GetCustomerSearch(isMember, term, this.CurrentSession.AgentId)
                    .Select(x => x.Username).ToList();

            return Json(listValue, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AgentList()
        {
            return View();
        }

        public ActionResult CustomerList(int? agentId)
        {
            ViewBag.CurrentAgentId = !agentId.HasValue ? UserSession.Current.AgentId : agentId.Value;
            return View();
        }

        public JsonResult GetAgentDownlinePath(int agentId)
        {
            var listValue = _accountService.GetAgentDownlinePath(this.CurrentSession.AgentId, agentId).ToList();

            return Json(listValue, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAgentList(int? agentId, int? pageIndex, int? totalPages, string userName, int? pageSize)
        {
            _agentId = !agentId.HasValue ? UserSession.Current.AgentId : agentId.Value;
            _pageIndex = !pageIndex.HasValue ? PAGEINDEX : pageIndex.Value;
            _totalPages = !totalPages.HasValue ? TOTALPAGE : totalPages.Value;
            _username = string.IsNullOrWhiteSpace(userName) ? string.Empty : userName;
            _pageSize = !pageSize.HasValue ? PAGESIZE : pageSize.Value;

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
                TotalItem = criteria.TotalItems,
                CurrentPage = _pageIndex,
                ItemsPerPage = _pageSize,
                CurrentAgentId = _agentId,
                ListData = models
            });
        }

        [HttpPost]
        public JsonResult GetCustomerList(int? agentId, int? pageIndex, int? totalPages, string userName, int? pageSize)
        {
            _agentId = !agentId.HasValue ? UserSession.Current.AgentId : agentId.Value;
            _pageIndex = !pageIndex.HasValue ? 0 : pageIndex.Value;
            _totalPages = !totalPages.HasValue ? 0 : totalPages.Value;
            _username = string.IsNullOrWhiteSpace(userName) ? string.Empty : userName;
            _pageSize = !pageSize.HasValue ? 50 : pageSize.Value;

            CustomerListCriteria criteria = new CustomerListCriteria()
            {
                AgentId = _agentId,
                PageIndex = _pageIndex,
                TotalPages = _totalPages,
                UserName = _username,
                PageSize = _pageSize
            };

            IEnumerable<CustomerListViewModel> models = _accountService.GetCustomerList(ref criteria);

            return Json(new
            {
                TotalPage = criteria.TotalPages,
                TotalItem = criteria.TotalItems,
                CurrentPage = _pageIndex,
                ItemsPerPage = _pageSize,
                CurrentAgentId = _agentId,
                ListData = models
            });
        }
    }
}