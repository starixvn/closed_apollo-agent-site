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
    using Apollo.Age.Account.ViewModel;
    using Apollo.Age.Account.ViewModels;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Services.Interface;
    using Apollo.Age.Web;
    using Sunplus.Agent.Model.Entities;

    public class AccountController : BaseController
    {
        private readonly IAgentService _agentService;
        private readonly IMemberService _memberService;
        private readonly AccountService _accountService;
        private int _agentId;
        private int _pageIndex;
        private int _totalPages;
        private string _username;
        private int _pageSize;

        public AccountController()
        {
            _agentService = new AgentService();
            _memberService = new MemberService();
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

            CustomerInfoViewModel viewmodel = null;
            if (!string.IsNullOrWhiteSpace(userName) || custId.HasValue)
            {
                HttpContext.Session["userName"] = userName;
                HttpContext.Session["isMember"] = isMember;

                ismember = isMember == 1;
                custid = custId.HasValue ? custId.Value : -1;
                username = userName;

                if (ismember)
                {
                    var memberInfo = _memberService.GetCustomerInfo(ismember, custid, username, base.AgentId);
                    if (memberInfo != null)
                    {
                        viewmodel = new CustomerInfoViewModel()
                        {
                            CurrentAgentId = base.AgentId,
                            IsMember = ismember,
                            CustId = memberInfo.Id,
                            Username = memberInfo.Username,
                            ParentId = memberInfo.Agent.Id,
                            ParentName = memberInfo.Agent.Username,
                            Group = memberInfo.Group
                        };
                    }
                    else
                    {
                        viewmodel = new CustomerInfoViewModel()
                        {
                            CurrentAgentId = base.AgentId,
                            CustId = -1,
                            IsMember = ismember,
                            Username = username,
                        };
                    }
                }
                else
                {
                    var agentInfo = _agentService.GetCustomerInfo(ismember, custid, username, base.AgentId);
                    if (agentInfo != null)
                    {
                        viewmodel = new CustomerInfoViewModel()
                        {
                            CurrentAgentId = base.AgentId,
                            IsMember = ismember,
                            CustId = agentInfo.Id,
                            Username = agentInfo.Username,
                            ParentId = agentInfo.Parent.Id,
                            ParentName = agentInfo.Parent.Username,
                            HasAgentDownline = agentInfo.HasAgentDownline,
                            HasMemberDownline = agentInfo.HasMemberDownline,
                            Level = agentInfo.Level
                        };
                    }
                    else
                    {
                        viewmodel = new CustomerInfoViewModel()
                        {
                            CurrentAgentId = base.AgentId,
                            CustId = -1,
                            IsMember = ismember,
                            Username = username,
                        };
                    }
                }
            }

            return View(viewmodel);
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
            var customerList = _agentService.GetDirectDownline(id);
            return Json(customerList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSuggestValues(string term, bool isMember)
        {
            var listValue = isMember
                ? _memberService.GetCustomerSearch(true, term, base.AgentId)
                    .Select(x => x.Username).ToList()
                : _agentService.GetCustomerSearch(false, term, base.AgentId)
                    .Select(x => x.Username).ToList();

            return Json(listValue, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AgentList()
        {
            return View();
        }

        public ActionResult CustomerList(int? agentId)
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAgentList(int? agentId, int? pageIndex, int? totalPages, string userName, int? pageSize)
        {
            _agentId = !agentId.HasValue ? UserSession.Current.AgentId : agentId.Value;
            _pageIndex = !pageIndex.HasValue ? 0 : pageIndex.Value;
            _totalPages = !totalPages.HasValue ? 0 : totalPages.Value;
            _username = string.IsNullOrWhiteSpace(userName) ? string.Empty : userName;
            _pageSize = !pageSize.HasValue ? 5 : pageSize.Value;

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