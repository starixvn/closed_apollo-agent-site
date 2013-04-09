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
    using Apollo.Age.Account.Criterias;
    using System.Collections.Generic;
    using Apollo.Age.Account.ViewModels;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MessageController : BaseController
    {
        private MessageService _messageService;

        private const string NORMAL = "normal";
        private const string SPECIAL = "special";
        private const string ALLSPECIAL = "allSpecial";
        private const string PRIVATE = "private";
        private const int SITEID = 3300;

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

        public ViewResult NewMessage(GetMessageCriteria criteria)
        {
            var listMessage = new List<MessageViewModel>();
            int isPublic = 0;
            int isSpecial = 0;
            int maxSize = 50;

            criteria.CustId = this.CurrentSession.UserId;
            criteria.SiteId = _messageService.GetParentSitesID(SITEID.ToString());
            criteria.Record = maxSize;

            criteria.From = criteria.From != null ? criteria.From : DateTime.Now.AddDays(-3);
            criteria.To = criteria.To != null ? criteria.To : DateTime.Now;
            criteria.PageQuery = string.IsNullOrWhiteSpace(criteria.PageQuery) ? NORMAL : criteria.PageQuery;

            if (string.Compare(criteria.PageQuery, PRIVATE) == 0 && criteria.Extra == 1)
            {
                if (!this.CurrentSession.IsSuper)
                {
                    _messageService.UpdateReadPrivateMessage(this.CurrentSession.UserId, this.CurrentSession.Username);
                }
            }

            if (string.Compare(criteria.PageQuery, SPECIAL) == 0
                || string.Compare(criteria.PageQuery, ALLSPECIAL) == 0)
            {
                isPublic = 2;
                isSpecial = 1;
                criteria.From = null;
                criteria.To = null;
            }

            if (string.Compare(criteria.PageQuery, NORMAL) == 0)
            {
                isPublic = 1;
                isSpecial = 0;
            }

            if (string.Compare(criteria.PageQuery, PRIVATE) == 0)
            {
                isPublic = 0;
                isSpecial = 2;
            }

            criteria.IsPublic = isPublic;
            criteria.IsSpecial = isSpecial;

            listMessage = _messageService.GetMessage(criteria).ToList();

            ViewBag.Messages = listMessage;

            return View(criteria);
        }

        #region Ajax request

        [HttpPost]
        public ActionResult SetReadPriMsg(int messageId)
        {
            if (Request.IsAjaxRequest())
            {
                int agentId = this.CurrentSession.AgentId;

                return Json(_messageService.UpdateReadMessage(agentId, messageId));
            }

            return Json(null);
        }

        [HttpPost]
        public ActionResult GetMessage()
        {
            int messageType = 3;

            int userId = this.CurrentSession.UserId;
            string username = this.CurrentSession.Username;

            object[] publicMessages = new object[3];
            object[] privateMessages = new object[2];

            //if (Request.IsAjaxRequest())
            //{
            var pubMessage = _messageService.GetTopPublicMessage(messageType, SITEID);
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
