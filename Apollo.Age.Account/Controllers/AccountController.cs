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
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Apollo.Age.Account.ViewModel;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Services.Interface;
    using Apollo.Age.Web;
    using System.Collections.Generic;

    public class AccountController : BaseController
    {
        private readonly IAgentService _agentService;
        private readonly IMemberService _memberService;

        public AccountController()
        {
            _agentService = new AgentService();
            _memberService = new MemberService();
        }

        public ActionResult Menu()
        {
            return View(SiteMap.RootNode);
        }

        public ActionResult AccountInfo(string userName, short? isMember, int? custId)
        {
            CustomerInfoViewModel viewmodel = null;
            if (!string.IsNullOrWhiteSpace(userName) || custId.HasValue)
            {
                var ismember = isMember == 1;
                var custid = custId.HasValue ? custId.Value : -1;
                var username = userName;

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

        public JsonResult GetSuggestValues(string query, short isMember)
        {
            if (isMember == 1)
            {
                var memberlist = _memberService.GetCustomerSearch(true, query, base.AgentId);
                return Json(memberlist, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var agentlist = _agentService.GetCustomerSearch(false, query, base.AgentId);
                return Json(agentlist, JsonRequestBehavior.AllowGet);
            }

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