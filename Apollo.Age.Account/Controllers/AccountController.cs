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
    using Apollo.Age.Entities;
    using Apollo.Age.Web;
    using Apollo.Age.Web.Filters;
    using Apollo.Age.Web.Pager;
    using Apollo.Age.Account.Entities;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class AccountController : BaseController
    {
        #region Const for paging
        private const int _PageIndex = 0; // Current index page
        private const int _PageSize = 50; // Maximum items in a page
        #endregion

        private readonly IAccountService _accountService;
        private int _agentId;
        private string _username;
        private int _agentLevel;

        public AccountController()
        {
            _accountService = new AccountService();
        }

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

        public ActionResult TopBar()
        {
            return View(CurrentSession);
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        public ActionResult Menu()
        {
            ViewBag.RootNode = SiteMap.RootNode;

            return View(this.CurrentSession);
        }

        public ActionResult MiniMenu()
        {
            ViewBag.RootNode = SiteMap.RootNode;

            return View(this.CurrentSession);
        }

        public ActionResult MemberDownline(int agentId, int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            int totalPage = 0;
            var criteria = new MemberDownlineCriteria()
            {
                AgentId = agentId,
                PageIndex = currentPageIndex + 1,
                PageSize = _PageSize,
                TotalPages = totalPage
            };

            var memberList = _accountService.GetMemberDownline(ref criteria);
            totalPage = criteria.TotalPages;

            return View(memberList.ToPagedList(currentPageIndex, _PageSize, totalPage * _PageSize));
        }

        [ValidateInput(false)]
        public ActionResult AgentInfo(string username, int agentId)
        {
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

        [ValidateInput(false)]
        public ActionResult MemberInfo(string username, int custId)
        {
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
            CustomerInfoModel customerInfo = null;
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

            var hasAgentDownline = agentList == null ? false : (agentList.Count > 0);

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

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GetDataByNode(int agentId)
        {
            var criteria = new AgentDownlineCriteria()
            {
                AgentId = agentId
            };

            List<AgentTreeViewNode> childList = new List<AgentTreeViewNode>();
            var agentList = _accountService.GetDirectDownline(criteria).Select(x => new { x.Id, x.Username, x.HasAgentDownline });
            
            return Json(agentList, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GetRootNode()
        {
            var criteria = new AgentDownlineCriteria()
            {
                AgentId = this.CurrentSession.AgentId
            };

            var agentList = _accountService.GetDirectDownline(criteria);
            List<AgentTreeViewNode> childList = new List<AgentTreeViewNode>();
            bool hasAgentDownline = false;

            if (agentList != null && agentList.Any())
            {
                hasAgentDownline = true;
                agentList.ForEach(t=> childList.Add(
                    new AgentTreeViewNode
                    {
                        Id = t.Id, 
                        Username = t.Username, 
                        HasAgentDownline = t.HasAgentDownline
                    }
                ));
            }

            var root = new AgentTreeViewNode { 
                Id = this.CurrentSession.AgentId, 
                Username = this.CurrentSession.AgentName, 
                HasAgentDownline = hasAgentDownline, 
                Children = childList,
                Expanded = true
            };

            var rootList = new List<AgentTreeViewNode>();
            rootList.Add(root);

            return Json(rootList, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
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

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
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

        #region Agent List

        [HttpGet]
        public ActionResult AgentList(int agentId = -1, int pageIndex = 0, string username = "", int status = 1, int oldPageIndex = 0, int orderType = 1)
        {
            AgentListCriteria criteria = new AgentListCriteria()
            {
                AgentId = agentId == -1 ? this.CurrentSession.AgentId : agentId,
                Status = status,
                PageIndex = pageIndex,
                UserName = username,
                OldPageIndex = oldPageIndex,
                OrderType = orderType
            };

            var agentInfo = _accountService.GetAgentInfo(new GetAgentInfoCriteria
            {
                ActorAgentId = CurrentSession.AgentId,
                AgentId = agentId == -1 ? this.CurrentSession.AgentId : agentId,
                ExcludeActor = false
            });

            IEnumerable<AgentListModel> models = _accountService.GetAgentList(ref criteria);

            /* Order type
            * ASC : 1
            * DES : 2
            */
            if (criteria.OrderType == 1)
            {
                models = models.OrderBy(m => m.Username).ToList();
            }
            else
            {
                models = models.OrderByDescending(m => m.Username).ToList();
            }

            ViewBag.HasMemberDownline = agentInfo != null ? agentInfo.HasMemberDownline : false;

            ViewBag.AgentList = models;
            ViewBag.ListPage = GetListPage(criteria.NumPages, criteria.PageIndex);
            ViewBag.IsMaster = this.CurrentSession.IsMaster;
            return View(criteria);
        }

        [HttpPost]
        public ActionResult AgentList(AgentListCriteria criteria)
        {
            if (criteria.PageIndex == criteria.OldPageIndex)
            {
                criteria.PageIndex = 0;
            }
            else
            {
                criteria.OldPageIndex = criteria.PageIndex;
            }

            return RedirectToAction(
                "AgentList",
                new
                {
                    agentId = criteria.AgentId,
                    pageIndex = criteria.PageIndex,
                    username = criteria.UserName,
                    status = criteria.Status,
                    oldPageIndex = criteria.OldPageIndex,
                    orderType = criteria.OrderType
                });
        }

        [HttpGet]
        public ActionResult IndirectAgentList(
            int parentAgentId,
            int agentId,
            int pageIndex = 0,
            string username = "",
            int status = 1,
            int oldPageIndex = 0,
            int orderType = 1)
        {
            AgentListCriteria criteria = new AgentListCriteria()
            {
                AgentId = agentId,
                PageIndex = pageIndex,
                UserName = username,
                Status = status,
                OldPageIndex = oldPageIndex,
                OrderType = orderType
            };

            var agentInfo = _accountService.GetAgentInfo(new GetAgentInfoCriteria
            {
                ActorAgentId = CurrentSession.AgentId,
                AgentId = agentId == -1 ? this.CurrentSession.AgentId : agentId,
                ExcludeActor = false
            });

            IEnumerable<AgentListModel> models = _accountService.GetAgentList(ref criteria);

            /* Order type
             * ASC : 1
             * DES : 2
             */
            if (criteria.OrderType == 1)
            {
                models = models.OrderBy(m => m.Username).ToList();
            }
            else
            {
                models = models.OrderByDescending(m => m.Username).ToList();
            }

            List<Agent> directAgent = _accountService.GetDirectDownline(new AgentDownlineCriteria
            {
                AgentId = parentAgentId
            });

            ViewBag.HasMemberDownline = agentInfo != null ? agentInfo.HasMemberDownline : false;
            ViewBag.ParentId = parentAgentId;

            ViewBag.AgentList = models;
            ViewBag.ListPage = GetListPage(criteria.NumPages, criteria.PageIndex);
            ViewBag.DirectAgentList = directAgent;
            ViewBag.IsMaster = this.CurrentSession.IsMaster;
            ViewBag.AgentUsername = agentInfo.Username;

            return View(criteria);
        }

        [HttpPost]
        public ActionResult IndirectAgentList(AgentListCriteria criteria)
        {
            if (criteria.PageIndex == criteria.OldPageIndex)
            {
                criteria.PageIndex = 0;
            }
            else
            {
                criteria.OldPageIndex = criteria.PageIndex;
            }

            return RedirectToAction(
                "IndirectAgentList",
                new
            {
                parentAgentId = criteria.ParentAgentId,
                agentId = criteria.UplineAgentId,
                pageIndex = criteria.PageIndex,
                username = criteria.UserName,
                status = criteria.Status,
                oldPageIndex = criteria.OldPageIndex,
                orderType = criteria.OrderType
            });
        }

        #endregion

        #region Member List

        [HttpGet]
        public ActionResult MemberList(
            int agentId = -1,
            int pageIndex = 0,
            string username = "",
            int status = 1,
            int oldPageIndex = 0,
            int orderType = 1)
        {
            CustomerListCriteria criteria = new CustomerListCriteria()
            {
                AgentId = agentId == -1 ? this.CurrentSession.AgentId : agentId,
                PageIndex = pageIndex,
                UserName = username,
                Status = status,
                OldPageIndex = oldPageIndex,
                OrderType = orderType
            };

            var agentInfo = _accountService.GetAgentInfo(new GetAgentInfoCriteria
            {
                ActorAgentId = CurrentSession.AgentId,
                AgentId = agentId == -1 ? this.CurrentSession.AgentId : agentId,
                ExcludeActor = false
            });

            IEnumerable<MemberListModel> models = _accountService.GetCustomerList(ref criteria);

            /* Order type
            * ASC : 1
            * DES : 2
            */
            if (criteria.OrderType == 1)
            {
                models = models.OrderBy(m => m.Username).ToList();
            }
            else
            {
                models = models.OrderByDescending(m => m.Username).ToList();
            }

            ViewBag.HasAgentDownline = agentInfo != null ? agentInfo.HasAgentDownline : false;
            ViewBag.MemberList = models;
            ViewBag.ListPage = GetListPage(criteria.NumPages, criteria.PageIndex);
            ViewBag.IsMaster = this.CurrentSession.IsMaster;

            return View(criteria);
        }

        [HttpPost]
        public ActionResult MemberList(CustomerListCriteria criteria)
        {
            if (criteria.PageIndex == criteria.OldPageIndex)
            {
                criteria.PageIndex = 0;
            }
            else
            {
                criteria.OldPageIndex = criteria.PageIndex;
            }

            return RedirectToAction(
                "MemberList",
                new
            {
                agentId = criteria.AgentId,
                pageIndex = criteria.PageIndex,
                username = criteria.UserName,
                status = criteria.Status,
                oldPageIndex = criteria.OldPageIndex,
                orderType = criteria.OrderType
            });
        }

        [HttpGet]
        public ActionResult IndirectMemberList(
            int parentAgentId,
            int agentId,
            int pageIndex = 0,
            string username = "",
            int status = 1,
            int oldPageIndex = 0,
            int orderType = 1)
        {
            CustomerListCriteria criteria = new CustomerListCriteria()
            {
                AgentId = agentId,
                PageIndex = pageIndex,
                UserName = username,
                Status = status,
                OldPageIndex = oldPageIndex,
                OrderType = orderType
            };

            var agentInfo = _accountService.GetAgentInfo(new GetAgentInfoCriteria
            {
                ActorAgentId = CurrentSession.AgentId,
                AgentId = agentId,
                ExcludeActor = false
            });

            IEnumerable<MemberListModel> models = _accountService.GetCustomerList(ref criteria);

            /* Order type
           * ASC : 1
           * DES : 2
           */
            if (criteria.OrderType == 1)
            {
                models = models.OrderBy(m => m.Username).ToList();
            }
            else
            {
                models = models.OrderByDescending(m => m.Username).ToList();
            }

            List<Agent> directAgent = _accountService.GetDirectDownline(new AgentDownlineCriteria
            {
                AgentId = parentAgentId
            });

            var agent = directAgent.Where(m => m.Id == criteria.AgentId).FirstOrDefault();

            ViewBag.HasAgentDownline = agentInfo != null ? agentInfo.HasAgentDownline : false;
            ViewBag.ParentId = parentAgentId;
            ViewBag.MemberList = models;
            ViewBag.ListPage = GetListPage(criteria.NumPages, criteria.PageIndex);
            ViewBag.DirectAgentList = directAgent;
            ViewBag.AgentUsername = agent != null ? agent.Username : string.Empty;
            ViewBag.IsMaster = this.CurrentSession.IsMaster;
            ViewBag.AgentList = _accountService.GetAgentDownlinePath(new AgentDownlinePathCriteria
            {
                ParentId = this.CurrentSession.AgentId,
                AgentId = agentId
            });

            return View(criteria);
        }

        [HttpPost]
        public ActionResult IndirectMemberList(CustomerListCriteria criteria)
        {
            if (criteria.PageIndex == criteria.OldPageIndex)
            {
                criteria.PageIndex = 0;
            }
            else
            {
                criteria.OldPageIndex = criteria.PageIndex;
            }

            return RedirectToAction(
                "IndirectMemberList",
                new
            {
                parentAgentId = criteria.ParentAgentId,
                agentId = criteria.UplineAgentId,
                pageIndex = criteria.PageIndex,
                username = criteria.UserName,
                status = criteria.Status,
                oldPageIndex = criteria.OldPageIndex,
                orderType = criteria.OrderType
            });
        }

        #endregion

        [InternalChecking]
        public ActionResult LogLogin()
        {
            var agentId = CurrentSession.AgentId;
            var agentUserId = CurrentSession.AdminId;

            GetLoginLogCriteria criteria = new GetLoginLogCriteria()
            {
                AgentId = agentId,
                AgentUserId = agentUserId
            };

            IEnumerable<AgentLogLoginModel> models = _accountService.GetAgentLogLogin(criteria);

            return View(models);
        }

        public JsonResult GetAgentDownlinePath(int? agentId)
        {
            var criteria = new AgentDownlinePathCriteria()
            {
                ParentId = this.CurrentSession.AgentId,
                AgentId = agentId.HasValue ? agentId.Value : this.CurrentSession.AgentId
            };

            var listValue = _accountService.GetAgentDownlinePath(criteria)
                 .Select(x => new { x.Id, x.Username, x.Level })
                 .ToList();

            return Json(listValue, JsonRequestBehavior.AllowGet);
        }

        #region Ultil

        private List<SelectListItem> GetListPage(int numPages, int currentPage)
        {
            var listPage = new List<SelectListItem>();
            for (int i = 0; i < numPages; i++)
            {
                listPage.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text = (i + 1).ToString(),
                    Selected = i == currentPage
                });
            }

            return listPage;
        }

        #endregion
    }
}