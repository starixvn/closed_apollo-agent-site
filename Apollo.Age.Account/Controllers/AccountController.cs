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
    using System.Web.Mvc;
    using System.Web.Security;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Web;
    using System.Web;

    public class AccountController : BaseController
    {
        private readonly AgentService _agentService;

        public AccountController()
        {
            _agentService = new AgentService();
        }

        public ActionResult Menu()
        {
            return View(SiteMap.RootNode);
        }

        public ActionResult AccountInfo()
        {
            return View();
        }

        public ViewResult AgentTreeList()
        {
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