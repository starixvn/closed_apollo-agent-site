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
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Apollo.Age.Account.Core;
    using Apollo.Age.Account.Criterias;
    using Apollo.Age.Account.Models;
    using Apollo.Age.Account.Services;
    using Apollo.Age.Web;

    /// <summary>
    /// Message controller.
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class MessageController : BaseController
    {
        /// <summary>
        /// For temp, set site list is 3000 and 3300.
        /// </summary>
        private const string SiteList = "3000,3300";
        private const int SiteId = 3300;        

        private MessageService _messageService;

        public MessageController()
            : this(null)
        {
        }

        public MessageController(MessageService messageService = null)
        {
            this._messageService = messageService ?? new MessageService();
        }

        /// <summary>
        /// Get announcement.
        /// </summary>
        /// <param name="content">Content search.</param>
        /// <param name="modeDate">Search mode date.</param>
        /// <returns>View announcement.</returns>
        [ValidateInput(false)]
        public ViewResult Announcement(string content = "", int modeDate = 0)
        {
            DateTime? startDate = new DateTime();
            DateTime endDate = new DateTime();

            /* Value of ModeDate
             * 0 : All (From the month before until now)
             * 1 : Today
             * 2 : Yesterday
             * 3 : Before Yesterday
             */
            switch (modeDate)
            {
                case 1:
                    startDate = DateTime.Now;
                    endDate = DateTime.Now;
                    break;
                case 2:
                    startDate = DateTime.Now.AddDays(-1);
                    endDate = DateTime.Now.AddDays(-1);
                    break;
                case 3:
                    startDate = DateTime.Now.AddDays(-2);
                    endDate = DateTime.Now.AddDays(-2);
                    break;
                default:
                    startDate = null;
                    endDate = DateTime.Now;
                    break;
            }

            var model = this._messageService.GetAnnouncement(
                    agentId: this.CurrentSession.AgentId,
                    siteIds: SiteList,
                    lastPostDate: endDate,
                    startTime: startDate,
                    content: content,
                    langue: this.CurrentSession.Language);

            ViewBag.UserLang = this.CurrentSession.Language;
            ViewBag.ModeDate = modeDate;

            return View(model);
        }

        /// <summary>
        /// Portal action.
        /// </summary>
        /// <returns>View portal action.</returns>
        public ViewResult Portal()
        {
            return View();
        }

        /// <summary>
        /// Get new message.
        /// </summary>
        /// <param name="criteria">View model get new message.</param>
        /// <returns>Content new message.</returns>
        public ViewResult NewMessage(GetMessageCriteria criteria)
        {
            int isPublic = 0;
            int special = 0;
            int maxSize = 50;

            criteria.CustId = this.CurrentSession.AgentId;
            criteria.SiteId = this._messageService.GetParentSitesID(SiteId.ToString());
            criteria.Record = maxSize;

            criteria.From = criteria.From != null ? criteria.From : DateTime.Now.AddDays(-3);
            criteria.To = criteria.To != null ? criteria.To : DateTime.Now;
            criteria.PageQuery = string.IsNullOrWhiteSpace(criteria.PageQuery) ? Constants.Normal : criteria.PageQuery;

            if (string.Compare(criteria.PageQuery, Constants.Private) == 0
                && criteria.Extra == 1)
            {
                if (!this.CurrentSession.IsSuper)
                {
                    this._messageService.UpdateReadPrivateMessage(this.CurrentSession.AgentId, this.CurrentSession.Username);
                }
            }

            if (string.Compare(criteria.PageQuery, Constants.Special) == 0
                || string.Compare(criteria.PageQuery, Constants.AllSpecial) == 0)
            {
                isPublic = 2;
                special = 1;
                criteria.From = null;
                criteria.To = null;
            }

            if (string.Compare(criteria.PageQuery, Constants.Normal) == 0)
            {
                isPublic = 1;
                special = 0;
            }

            if (string.Compare(criteria.PageQuery, Constants.Private) == 0)
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

        /// <summary>
        /// Set private message is read.
        /// </summary>
        /// <param name="messageId">Message id.</param>
        /// <returns>Message is updated.</returns>
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

        /// <summary>
        /// Update read private messages.
        /// </summary>
        /// <returns>Message is read.</returns>
        [HttpPost]
        public ActionResult UpdatePriMsg()
        {
            if (Request.IsAjaxRequest())
            {
                return Json(this._messageService.UpdateReadPrivateMessage(this.CurrentSession.AgentId, this.CurrentSession.Username));
            }

            return Json(null);
        }

        /// <summary>
        /// Get message for ajax.
        /// </summary>
        /// <returns>Content of message.</returns>
        [HttpPost]
        public ActionResult GetMessage()
        {
            // Message type from older source code
            int messageType = 3;

            int agentId = this.CurrentSession.AgentId;
            string username = this.CurrentSession.Username;

            object[] publicMessages = new object[3];
            object[] privateMessages = new object[2];

            var pubMessage = this._messageService.GetTopPublicMessage(messageType, SiteId);
            if (!ReferenceEquals(pubMessage, null))
            {
                publicMessages[0] = pubMessage.MessageId;
                publicMessages[1] = pubMessage.GetContent();
                publicMessages[2] = Constants.Normal;
                if (pubMessage.IsSticky)
                {
                    publicMessages[2] = Constants.AllSpecial;
                }
            }

            var priMessage = this._messageService.GetTopPrivateMessage24(agentId);
            if (!ReferenceEquals(priMessage, null))
            {
                privateMessages[0] = priMessage.MessageId;
                privateMessages[1] = priMessage.GetContent();
            }

            var countPriMessage = this._messageService.CountUnreadPrivateMessage(agentId);

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