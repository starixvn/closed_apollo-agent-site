// --------------------------------------------------------------------------
// <copyright file="MessageController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.4">Created</change>
//   <change who="Edward.Nguyen" date="2013.4.9">Add Annoucement</change>
//   <change who="Edward.Nguyen" date="2013.4.9">Add Get Message</change>
// </history>

namespace Apollo.Age.Account.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Apollo.Age.Account.Core;
    using Apollo.Age.Account.Criterias;
    using Apollo.Age.Account.Models;
    using Apollo.Age.Account.Services;
    using Apollo.Age.Web;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class MessageController : BaseController
    {
        private MessageService _messageService;

        /// <summary>
        /// For temp
        /// </summary>
        private const int SITEID = 3300;

        public MessageController()
            : this(null)
        {
        }

        public MessageController(MessageService messageService = null)
        {
            this._messageService = messageService ?? new MessageService();
        }

        public ViewResult Announcement(DateTime? lastPostTime)
        {
            var model = this._messageService.GetAnnouncement(this.CurrentSession.AgentId, lastPostTime ?? DateTime.Now);

            if (!ReferenceEquals(model, null) && model.Count() > 0)
            {
                var cookie = new HttpCookie(Constants.LASTPOSTTIMEKEY, model.Last().PostTime.ToString());
                HttpContext.Response.Cookies.Set(cookie);

                ViewBag.LastPostTime = lastPostTime ?? model.Last().PostTime;
            }

            return View(model);
        }

        public ViewResult Portal()
        {
            return View();
        }

        public ViewResult NewMessage(GetMessageCriteria criteria)
        {
            int isPublic = 0;
            int special = 0;
            int maxSize = 50;

            criteria.CustId = this.CurrentSession.UserId;
            criteria.SiteId = this._messageService.GetParentSitesID(SITEID.ToString());
            criteria.Record = maxSize;

            criteria.From = criteria.From != null ? criteria.From : DateTime.Now.AddDays(-3);
            criteria.To = criteria.To != null ? criteria.To : DateTime.Now;
            criteria.PageQuery = string.IsNullOrWhiteSpace(criteria.PageQuery) ? Constants.NORMAL : criteria.PageQuery;

            if (string.Compare(criteria.PageQuery, Constants.PRIVATE) == 0
                && criteria.Extra == 1)
            {
                if (!this.CurrentSession.IsSuper)
                {
                    this._messageService.UpdateReadPrivateMessage(this.CurrentSession.UserId, this.CurrentSession.Username);
                }
            }

            if (string.Compare(criteria.PageQuery, Constants.SPECIAL) == 0
                || string.Compare(criteria.PageQuery, Constants.ALLSPECIAL) == 0)
            {
                isPublic = 2;
                special = 1;
                criteria.From = null;
                criteria.To = null;
            }

            if (string.Compare(criteria.PageQuery, Constants.NORMAL) == 0)
            {
                isPublic = 1;
                special = 0;
            }

            if (string.Compare(criteria.PageQuery, Constants.PRIVATE) == 0)
            {
                isPublic = 0;
                special = 2;
            }

            criteria.IsPublic = isPublic;
            criteria.Special = special;
            criteria.IsAgent = true;

            ViewBag.Messages = (_messageService.GetMessage(criteria) ?? new List<MessageModel>()).ToList();

            return View(criteria);
        }

        #region Ajax request

        [HttpPost]
        public ActionResult SetReadPriMsg(int messageId)
        {
            if (Request.IsAjaxRequest())
            {
                int agentId = this.CurrentSession.AgentId;

                return Json(this._messageService.UpdateReadMessage(agentId, messageId));
            }

            return Json(null);
        }

        [HttpPost]
        public ActionResult UpdatePriMsg()
        {
            if (Request.IsAjaxRequest())
            {
                return Json(this._messageService.UpdateReadPrivateMessage(this.CurrentSession.UserId, this.CurrentSession.Username));
            }

            return Json(null);
        }

        [HttpPost]
        public ActionResult GetMessage()
        {
            // Message type from older source code
            int messageType = 3;

            int userId = this.CurrentSession.UserId;
            string username = this.CurrentSession.Username;

            object[] publicMessages = new object[3];
            object[] privateMessages = new object[2];

            var pubMessage = this._messageService.GetTopPublicMessage(messageType, SITEID);
            if (!ReferenceEquals(pubMessage, null))
            {
                publicMessages[0] = pubMessage.MessageId;
                publicMessages[1] = pubMessage.GetContent();
                publicMessages[2] = Constants.NORMAL;
                if (pubMessage.IsSticky)
                {
                    publicMessages[2] = Constants.ALLSPECIAL;
                }
            }

            var priMessage = this._messageService.GetTopPrivateMessage24(userId);
            if (!ReferenceEquals(priMessage, null))
            {
                privateMessages[0] = priMessage.MessageId;
                privateMessages[1] = priMessage.GetContent();
            }

            var countPriMessage = this._messageService.CountUnreadPrivateMessage(userId, username);

            return Json(new
            {
                public_message = publicMessages,
                private_message = privateMessages,
                unread_private_messages = countPriMessage
            });
        }

        #endregion Ajax request
    }
}