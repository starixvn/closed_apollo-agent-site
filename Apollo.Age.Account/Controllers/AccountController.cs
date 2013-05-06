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
//   <change who="Anders.Le" date="2013.4.23">Rename namespace of pager</change>
// </history>

namespace Apollo.Age.Account.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Web.SessionState;
    using Apollo.Age.Account.Criterias;
    using Apollo.Age.Account.Models;
    using Apollo.Age.Account.Services;
    using Apollo.Age.Criterias.Accounts;
    using Apollo.Age.Web;
    using Apollo.Age.Web.Pager;
    using Fanex.Common;

    // [SessionState(SessionStateBehavior.ReadOnly)]
    public class AccountController : BaseController
    {
        private const int PAGEINDEX = 0; // Current index page
        private const int PAGESIZE = 50; // Maximum items in a page
        private const int TOTALPAGE = 0; // Maximum page totals

        private readonly IAccountService _accountService;
        private int _agentId;
        private string _username;
        private int _agentLevel;

        private int? CurrentAgentId
        {
            get
            {
                return _agentId;
            }
            set
            {
                _agentId = value.HasValue ? value.Value : CurrentSession.AgentId;
            }
        }

        private string CurrentUsername
        {
            get
            {
                return _username;
            }
            set
            {
                _username = string.IsNullOrWhiteSpace(value) ? CurrentSession.AgentName : value;
            }
        }

        private int? CurrentAgentLevel
        {
            get
            {
                return _agentLevel;
            }
            set
            {
                _agentLevel = value.HasValue ? value.Value : CurrentSession.AgentLevel;
            }
        }

        public AccountController()
        {
            _accountService = new AccountService();
        }

        public ActionResult TopBar()
        {
            return View(base.CurrentSession);
        }

        public ActionResult Menu()
        {
            ViewBag.RootNode = SiteMap.RootNode;

            return View(base.CurrentSession);
        }

        public ActionResult MemberDownline(int agentId, int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            int totalPage = 0;
            var criteria = new MemberDownlineCriteria()
            {
                AgentId = agentId,
                PageIndex = currentPageIndex + 1,
                PageSize = PAGESIZE,
                TotalPages = totalPage
            };

            var memberList = _accountService.GetMemberDownline(ref criteria);
            totalPage = criteria.TotalPages;

            return View(memberList.ToPagedList(currentPageIndex, PAGESIZE, totalPage * PAGESIZE));
        }

        public ActionResult AgentInfo(string username, int agentId)
        {
            HttpContext.Session["username"] = username;
            HttpContext.Session["isMember"] = "false";

            var agentInfo = _accountService.GetAgentInfo(new GetAgentInfoCriteria
            {
                ActorAgentId = CurrentSession.AgentId,
                AgentId = agentId,
                Username = username,
                ExcludeActor = false
            });

            if (agentInfo == null)
            {
                agentInfo = new CustomerInfoModel()
                {
                    ActorAgentId = this.CurrentSession.AgentId,
                    CustId = -1,
                    IsMember = false,
                    Username = username,
                    IsMaster = this.CurrentSession.IsMaster
                };
            }
            else
            {
                agentInfo.ActorAgentId = this.CurrentSession.AgentId;
                agentInfo.IsMember = false;
                agentInfo.IsMaster = this.CurrentSession.IsMaster;
            }

            return View("AccountInfo", agentInfo);
        }

        public ActionResult MemberInfo(string username, int custId)
        {
            HttpContext.Session["username"] = username;
            HttpContext.Session["isMember"] = "true";

            var memberInfo = _accountService.GetMemberInfo(new GetMemberInfoCriteria
            {
                ActorAgentId = CurrentSession.AgentId,
                CustId = custId,
                Username = username
            });

            if (memberInfo == null)
            {
                memberInfo = new CustomerInfoModel()
                {
                    ActorAgentId = this.CurrentSession.AgentId,
                    CustId = -1,
                    IsMember = true,
                    Username = username,
                    IsMaster = this.CurrentSession.IsMaster
                };
            }
            else
            {
                memberInfo.ActorAgentId = this.CurrentSession.AgentId;
                memberInfo.IsMember = true;
                memberInfo.IsMaster = this.CurrentSession.IsMaster;
            }

            return View("AccountInfo", memberInfo);
        }

        public ActionResult AccountInfo()
        {
            var username = string.Empty;
            var isMember = false;

            username = HttpContext.Session["username"] != null
                ? HttpContext.Session["username"].ToString() : string.Empty;
            isMember = HttpContext.Session["isMember"] != null
                ? Converter.ToBool(HttpContext.Session["isMember"])
                : false;

            CustomerInfoModel customerInfo = null;

            if (!string.IsNullOrWhiteSpace(username))
            {
                if (isMember)
                {
                    return MemberInfo(username, custId: -1);
                }
                else
                {
                    return AgentInfo(username, agentId: -1);
                }
            }

            return View(customerInfo);
        }

        public ViewResult AgentTreeList(int agentId)
        {
            ViewBag.AgentId = agentId;
            return View();
        }

        public ViewResult TreeViewList()
        {
            var criteria = new AgentDownlineCriteria()
            {
                AgentId = this.CurrentSession.AgentId
            };
            var agentList = _accountService.GetDirectDownline(criteria);

            var hasAgentDownline = agentList.Count > 0;

            ViewBag.AgentId = this.CurrentSession.AgentId;
            ViewBag.HasAgentDownline = hasAgentDownline;
            return View();
        }

        public ActionResult SignOut()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return Redirect("~/_Authorization/SignIn.aspx");
        }

        public JsonResult GetDataByNode(int agentId)
        {
            var criteria = new AgentDownlineCriteria()
            {
                AgentId = agentId
            };

            var agentList = _accountService.GetDirectDownline(criteria).Select(x => new { x.Id, x.Username, x.HasAgentDownline });
            return Json(agentList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAgentByUsername(string username, bool? excludeactor)
        {
            var criteria = new GetAgentByUsernameCriteria()
            {
                ActorAgentId = this.CurrentSession.AgentId,
                Username = username,
                ExcludeActor = excludeactor.HasValue ? excludeactor.Value : false
            };

            var agentList = _accountService.GetAgentByUsername(criteria).Select(x => new { x.Id, x.Username }).ToList();

            return Json(agentList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMemberByUsername(string username)
        {
            var criteria = new GetMemberByUsernameCriteria()
            {
                ActorAgentId = this.CurrentSession.AgentId,
                Username = username
            };

            var memberList = _accountService.GetMemberByUsername(criteria).Select(x => new { x.Id, x.Username }).ToList();

            return Json(memberList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AgentList(int? agentId)
        {
            CurrentAgentId = agentId;
            ViewBag.AgentId = CurrentAgentId.Value;
            ViewBag.IsMaster = CurrentSession.IsMaster;
            return View();
        }

        public ActionResult MemberList(int? agentId)
        {
            ViewBag.IsMaster = CurrentSession.IsMaster;

            CurrentAgentId = agentId;

            var criteria = new AgentDownlinePathCriteria()
            {
                ParentId = this.CurrentSession.AgentId,
                AgentId = CurrentAgentId.Value
            };

            var listAgent = _accountService.GetAgentDownlinePath(criteria).ToList();
            return View(listAgent);
        }

        public JsonResult GetAgentDownlinePath(int? agentId)
        {
            CurrentAgentId = agentId;
            var criteria = new AgentDownlinePathCriteria()
            {
                ParentId = this.CurrentSession.AgentId,
                AgentId = CurrentAgentId.Value
            };

            var listValue = _accountService.GetAgentDownlinePath(criteria)
                .Select(x => new { x.Id, x.Username, x.Level })
                .ToList();

            return Json(listValue, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAgentList(int? agentId, int? pageIndex, int? totalPages, string userName, int? pageSize)
        {
            CurrentAgentId = agentId;
            var currentPageIndex = !pageIndex.HasValue ? PAGEINDEX : pageIndex.Value;
            var currentUsername = string.IsNullOrWhiteSpace(userName) ? string.Empty : userName;
            var currentPageSize = !pageSize.HasValue ? PAGESIZE : pageSize.Value;

            AgentListCriteria criteria = new AgentListCriteria()
            {
                AgentId = CurrentAgentId.Value,
                PageIndex = currentPageIndex,
                UserName = currentUsername,
                PageSize = currentPageSize
            };

            IEnumerable<AgentListModel> models = _accountService.GetAgentList(ref criteria);

            return Json(new
            {
                TotalItem = criteria.TotalItems,
                CurrentPage = currentPageIndex,
                ItemsPerPage = currentPageSize,
                CurrentAgentId = CurrentAgentId.Value,
                ListData = models
            });
        }

        [HttpPost]
        public JsonResult GetMemberList(int? agentId, int? pageIndex, int? totalPages, string userName, int? pageSize)
        {
            CurrentAgentId = agentId;
            var currentPageIndex = !pageIndex.HasValue ? PAGEINDEX : pageIndex.Value;
            var currentUsername = string.IsNullOrWhiteSpace(userName) ? string.Empty : userName;
            var currentPageSize = !pageSize.HasValue ? PAGESIZE : pageSize.Value;

            CustomerListCriteria criteria = new CustomerListCriteria()
            {
                AgentId = CurrentAgentId.Value,
                PageIndex = currentPageIndex,
                UserName = currentUsername,
                PageSize = currentPageSize
            };

            IEnumerable<MemberListModel> models = _accountService.GetCustomerList(ref criteria);

            return Json(new
            {
                TotalItem = criteria.TotalItems,
                CurrentPage = currentPageIndex,
                ItemsPerPage = currentPageSize,
                CurrentAgentId = CurrentAgentId.Value,
                ListData = models
            });
        }
    }
}