// --------------------------------------------------------------------------
// <copyright file="MessageController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.4">Created</change>
// </history>

namespace Apollo.Age.Account.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Web;
    using System.Web.Mvc;
    using Apollo.Age.Account.Services;
    using System.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MessageController : BaseController
    {
        private MessageService _messageService;

        public MessageController() : this(null) { }

        public MessageController(MessageService messageService = null)
        {
            _messageService = messageService ?? new MessageService();
        }

        public ViewResult Announcement(DateTime? lastPostTime)
        {
            var model = _messageService.GetAnnouncement(3, lastPostTime ?? DateTime.Now);

            var cookie = new HttpCookie("lastPostTime", model.Last().PostTime.ToString());
            HttpContext.Response.Cookies.Set(cookie);

            ViewBag.LastPostTime = lastPostTime ?? model.Last().PostTime;

            return View(model);
        }

        public ViewResult Portal()
        {
            return View();
        }

        #region Ajax request

        [HttpPost]
        public ActionResult SetReadPriMsg(int messageId)
        {
            if (Request.IsAjaxRequest())
            {
                return Json(_messageService.UpdateReadMessage(this.CurrentSession.AgentId, messageId));
            }

            return Json(null);
        }

        //[HttpPost]
        public ActionResult GetMessage()
        {
            int siteId = 3300;
            int messageType = 3;
            //if (Request.IsAjaxRequest()) 
            //{
            var listPubMessage = _messageService.GetTopPublicMessage(messageType, siteId);

            var listPriMessage = _messageService.GetTopPrivateMessage24(this.CurrentSession.UserId);

            var countPriMessage = _messageService.CountUnreadPrivateMessage(this.CurrentSession.UserId, this.CurrentSession.Username);

            //}

            return Json(null);
        }
        #endregion

        //private int RoleToSiteID(int siteId)
        //{
        //    if (this.CurrentSession. == (int)UserTypes.Super)
        //    {
        //        siteId = 3100;
        //    }
        //    else if (CurrentUserSession.RoleId == (int)UserTypes.Master)
        //    {
        //        siteId = 3200;
        //    }
        //    else if (CurrentUserSession.RoleId == (int)UserTypes.Agent)
        //    {
        //        siteId = 3300;
        //    }
        //    return siteId;
        //}
    }
}
