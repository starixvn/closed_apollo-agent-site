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
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Apollo.Age.Account.Services;
    using Apollo.Age.Web;
    using Sunplus.Agent.Model.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MessageController : BaseController
    {
        private MessageService _messageService;

        private readonly const string NORMAL = "normal";
        private readonly const string SPECIAL = "special";
        private readonly const string ALLSPECIAL = "allSpecial";
        private readonly const string PRIVATE = "private";

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

        public ViewResult NewMessage(string pageQuery = NORMAL, int extra = 0, DateTime? from = null, DateTime? to = null)
        {
            from = from != null ? from : DateTime.Now.AddDays(-3);
            to = to != null ? to : DateTime.Now;

            if (string.Compare(pageQuery, PRIVATE) == 0 && extra == 1)
            {
                if (UserSession.Current.AdminId < 0)
                {
                    _messageService.UpdateReadPrivateMessage(UserSession.Current.UserId, UserSession.Current.Username);
                }
            }

            return View();
        }

        #region Ajax request

        [HttpPost]
        public ActionResult SetReadPriMsg(int messageId)
        {
            if (Request.IsAjaxRequest())
            {
                int agentId = UserSession.Current.AgentId;

                return Json(_messageService.UpdateReadMessage(agentId, messageId));
            }

            return Json(null);
        }

        [HttpPost]
        public ActionResult GetMessage()
        {
            int siteId = 3300;
            int messageType = 3;

            int userId = UserSession.Current.UserId;
            string username = UserSession.Current.Username;

            object[] publicMessages = new object[3];
            object[] privateMessages = new object[2];

            //if (Request.IsAjaxRequest())
            //{
            var pubMessage = _messageService.GetTopPublicMessage(messageType, siteId);
            if (!ReferenceEquals(pubMessage, null))
            {
                publicMessages[0] = pubMessage.MessageId;
                publicMessages[1] = pubMessage.GetContent();
                publicMessages[2] = "normal";
                if (pubMessage.IsSticky)
                {
                    publicMessages[2] = "allSpecial";
                }
            }

            var priMessage = _messageService.GetTopPrivateMessage24(userId);
            if (!ReferenceEquals(priMessage, null))
            {
                privateMessages[0] = priMessage.MessageId;
                privateMessages[1] = priMessage.GetContent();
            }

            var countPriMessage = _messageService.CountUnreadPrivateMessage(userId, username);

            //}

            return Json(new
            {
                public_message = publicMessages,
                private_message = privateMessages,
                unread_private_messages = countPriMessage
            });
        }
        #endregion
    }
}
