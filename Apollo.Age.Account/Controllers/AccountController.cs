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
//   <change who="Edward.Nguyen" date="2013.7.22">Member and Agent list</change>
//   <change who="Terry.Nguyen" date="2013.8.1">Checking Allow FundOut for Super, MainShareHolder and ShareHolder</change>
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
    using Apollo.Age.Account.Entities;
    using Apollo.Age.Account.Models;
    using Apollo.Age.Account.Services;
    using Apollo.Age.Criterias.Accounts;
    using Apollo.Age.Entities;
    using Apollo.Age.Web;
    using Apollo.Age.Web.Filters;
    using Apollo.Age.Web.Pager;
    using Apollo.Age.Account.ViewModels;

    /// <summary>
    /// Account controllers.
    /// </summary>
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

        /// <summary>
        /// Get member down line.
        /// </summary>
        /// <param name="agentId">Input Agent id.</param>
        /// <param name="page">Input Page index.</param>
        /// <returns>Member down line.</returns>
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

        /// <summary>
        /// Get agent info for left menu.
        /// </summary>
        /// <param name="username">Parameter User name.</param>
        /// <param name="agentId">Parameter agent id.</param>
        /// <returns>Agent info.</returns>
        [ValidateInput(false)]
        public ActionResult AgentInfo(string username, int agentId)
        {
            var agentInfo = _accountService.GetAgentInfo(new GetAgentAllInfoCriteria
            {
                ActorAgentId = CurrentSession.AgentId,
                AgentId = agentId,
                Username = username,
                ExcludeActor = false
            });

            var isDirectAgent = _accountService.IsDirectAgent(base.CurrentSession.AgentId, agentInfo != null ? agentInfo.CustId : -1, base.CurrentSession.IsSuper);

            if (agentInfo == null)
            {
                agentInfo = new CustomerInfoModel()
                {
                    ActorAgentId = this.CurrentSession.AgentId,
                    CustId = -1,
                    IsMember = false,
                    Username = username,
                    IsMaster = this.CurrentSession.IsMaster && isDirectAgent
                };
            }
            else
            {
                agentInfo.ActorAgentId = this.CurrentSession.AgentId;
                agentInfo.IsMember = false;
                agentInfo.IsMaster = this.CurrentSession.IsMaster && isDirectAgent;

                if (this.CurrentSession.AgentId != agentInfo.CustId)
                {
                    var agentPathList = _accountService.GetAgentDownlinePath(new AgentDownlinePathCriteria
                    {
                        ParentId = this.CurrentSession.AgentId,
                        AgentId = agentInfo.CustId
                    });

                    agentInfo.AgentPathList = agentPathList.Where(t => t.Id != agentInfo.CustId).OrderByDescending(t => t.Level).ToList();
                }
            }

            agentInfo.IsSuper = base.CurrentSession.IsSuper;
            

            return View("AccountInfo", agentInfo);
        }

        /// <summary>
        /// Get member info for left menu.
        /// </summary>
        /// <param name="username">Parameter User name.</param>
        /// <param name="custId">Parameter member id.</param>
        /// <returns>Member info.</returns>
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

                if (this.CurrentSession.AgentId != memberInfo.CustId)
                {
                    var agentPathList = _accountService.GetAgentDownlinePath(new AgentDownlinePathCriteria
                    {
                        ParentId = this.CurrentSession.AgentId,
                        AgentId = memberInfo.ParentId
                    });

                    memberInfo.AgentPathList = agentPathList.OrderByDescending(t => t.Level).ToList();
                }
            }

            memberInfo.IsSuper = base.CurrentSession.IsSuper;
            memberInfo.ActorAgentUsername = base.CurrentSession.AgentName;

            return View("AccountInfo", memberInfo);
        }

        /// <summary>
        /// Blank page for left menu when click from menu.
        /// </summary>
        /// <returns>Account Info.</returns>
        public ActionResult AccountInfo()
        {
            CustomerInfoModel customerInfo = null;
            return View(customerInfo);
        }

        /// <summary>
        /// Get agent list (tree) of agent which searched.
        /// </summary>
        /// <param name="agentId">Parent agent id.</param>
        /// <returns>Agent tree list.</returns>
        public ViewResult AgentTreeList(int agentId)
        {
            ViewBag.AgentId = agentId;
            return View();
        }

        /// <summary>
        /// Get tree view list for account tab.
        /// </summary>
        /// <returns>Tree view list.</returns>
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

        /// <summary>
        /// Get child node of a agent.
        /// </summary>
        /// <param name="agentId">Parent agent id.</param>
        /// <returns>List agent direct down line.</returns>
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

        /// <summary>
        /// Get agent root node and agent direct down line list of root node (account tab).
        /// </summary>
        /// <returns>Tree view default.</returns>
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
                agentList.ForEach(t => childList.Add(new AgentTreeViewNode
                    {
                        Id = t.Id,
                        Username = t.Username,
                        HasAgentDownline = t.HasAgentDownline
                    }));
            }

            var root = new AgentTreeViewNode
            {
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

        /// <summary>
        /// Get agent down line list by user name.
        /// </summary>
        /// <param name="username">User name is searched.</param>
        /// <param name="excludeactor">Exclude actor or no.</param>
        /// <returns>Agent down line list.</returns>
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

        /// <summary>
        /// Get member down line list by user name.
        /// </summary>
        /// <param name="username">User name is searched.</param>
        /// <returns>Member down line list.</returns>
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

        /// <summary>
        /// Get direct agent list view action.
        /// </summary>
        /// <param name="agentId">Search agent id.</param>
        /// <param name="pageIndex">Current page index.</param>
        /// <param name="username">Search username of agent.</param>
        /// <param name="status">Filter by agent status.</param>
        /// <param name="oldPageIndex">Old page index.</param>
        /// <param name="orderType">Filter order by username.</param>
        /// <returns>Agent list view action.</returns>
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

            var agentInfo = _accountService.GetAgentInfo(new GetAgentAllInfoCriteria
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

        /// <summary>
        /// Get direct agent list view action.
        /// </summary>
        /// <param name="criteria">View model get agent list.</param>
        /// <returns>Agent list view action.</returns>
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

        /// <summary>
        /// Get indirect agent list view action.
        /// </summary>
        /// <param name="parentAgentId">Parent agent id.</param>
        /// <param name="agentId">Search agent id.</param>
        /// <param name="pageIndex">Current page index.</param>
        /// <param name="username">Search agent username.</param>
        /// <param name="status">Filter status of agent.</param>
        /// <param name="oldPageIndex">Old page index.</param>
        /// <param name="orderType">Filter order by username.</param>
        /// <returns>Indirect agent list view action.</returns>
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

            var agentInfo = _accountService.GetAgentInfo(new GetAgentAllInfoCriteria
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
            ViewBag.IsMaster = this.CurrentSession.IsMaster && base.CurrentSession.IsSuper;
            ViewBag.AgentUsername = agentInfo.Username;

            return View(criteria);
        }

        /// <summary>
        /// Get indirect agent list view action.
        /// </summary>
        /// <param name="criteria">View model get indirect agent list.</param>
        /// <returns>Indirect agent list view action.</returns>
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

        /// <summary>
        /// Get direct member list.
        /// </summary>
        /// <param name="agentId">Search agent id.</param>
        /// <param name="pageIndex">Current page index.</param>
        /// <param name="username">Search username of member.</param>
        /// <param name="status">Filter status of agent.</param>
        /// <param name="oldPageIndex">Old page index.</param>
        /// <param name="orderType">Filter order by username.</param>
        /// <returns>Direct member list view action.</returns>
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

            var agentInfo = _accountService.GetAgentInfo(new GetAgentAllInfoCriteria
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

            // Super, MainShareHolder and ShareHolder only are allowed fund out sport credit
            ViewBag.AllowFundOut = this.CurrentSession.AgentLevel <= 2;
            return View(criteria);
        }

        /// <summary>
        /// Get direct member list.
        /// </summary>
        /// <param name="criteria">View model get direct member list.</param>
        /// <returns>Direct member list view action.</returns>
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

        /// <summary>
        /// Get indirect member list view action.
        /// </summary>
        /// <param name="parentAgentId">Parent agent id.</param>
        /// <param name="agentId">Search agent id.</param>
        /// <param name="pageIndex">Current page index.</param>
        /// <param name="username">Search member username.</param>
        /// <param name="status">Filter status of agent.</param>
        /// <param name="oldPageIndex">Old page index.</param>
        /// <param name="orderType">Filter order by username.</param>
        /// <returns>Indirect member list view action.</returns>
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

            var agentInfo = _accountService.GetAgentInfo(new GetAgentAllInfoCriteria
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
            ViewBag.IsSupper = base.CurrentSession.IsSuper;
            ViewBag.AgentList = _accountService.GetAgentDownlinePath(new AgentDownlinePathCriteria
            {
                ParentId = this.CurrentSession.AgentId,
                AgentId = agentId
            });

            // Super, MainShareHolder and ShareHolder only are allowed fund out sport credit
            ViewBag.AllowFundOut = this.CurrentSession.AgentLevel <= 2;
            ViewBag.ActorAgentUsername = this.CurrentSession.AgentName;

            return View(criteria);
        }

        /// <summary>
        /// Get indirect member list view action.
        /// </summary>
        /// <param name="criteria">View model get indirect member list.</param>
        /// <returns>Indirect member list view action.</returns>
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

        /// <summary>
        /// Get agent down line path (from login agent to agent is searched).
        /// </summary>
        /// <param name="agentId">Agent is searched.</param>
        /// <returns>Agent down line path.</returns>
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

        
        public ActionResult FundOut(decimal memberId, decimal sportCredit)
        {
            var model = new FundOutViewModel() {
                MemberId = memberId,
                SportCredit = sportCredit
            };

            return View(model);
        }

        public JsonResult PostFundOut(decimal memberId, decimal sportCredit)
        {


            return Json(
                new
                {
                    ErrCode = 0,
                    ErrMsg = string.Empty
                },
                JsonRequestBehavior.AllowGet
            );
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