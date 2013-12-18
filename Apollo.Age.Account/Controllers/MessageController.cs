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
    using Apollo.Age.Configurations;
    using Apollo.Age.Web;

    /// <summary>
    /// Message controller.
    /// </summary>
    [SessionState(SessionStateBehavior.ReadOnly)]
    [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
    public class MessageController : BaseController
    {
        /// <summary>
        /// For temp, set site list is 3000000 and 3300.
        /// </summary>
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
        /// <param name="isLevelAnnouncement">If set to <c>true</c> [is level announcement].</param>
        /// <returns>
        /// View announcement.
        /// </returns>
        [ValidateInput(false)]
        public ViewResult Announcement(string content = "", int modeDate = 0, bool isLevelAnnouncement = false)
        {
            DateTime? startDate = new DateTime();
            DateTime endDate = new DateTime();
            short? agentLevel = null;
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

            agentLevel = isLevelAnnouncement ? Convert.ToInt16(CurrentSession.AgentLevel) : agentLevel;

            var model = this._messageService.GetAnnouncement(
                this.CurrentSession.AgentId,
                AppSettings.SiteId.ToString(),
                startDate,
                endDate,
                agentLevel,
                10,
                false,
                content.Trim(),
                CurrentSession.Language);

            ViewBag.UserLang = this.CurrentSession.Language;
            ViewBag.ModeDate = modeDate;
            ViewBag.IsLevelAnnouncement = isLevelAnnouncement;
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
            int publicMessage = 0;
            int special = 0;
            int maxSize = 50;

            criteria.CustId = this.CurrentSession.AgentId;
            criteria.SiteId = this._messageService.GetParentSitesID(AppSettings.SiteId.ToString());
            criteria.Record = maxSize;

            criteria.FromDate = criteria.FromDate != null ? criteria.FromDate : DateTime.Now.AddDays(-3);
            criteria.ToDate = criteria.ToDate != null ? criteria.ToDate : DateTime.Now;
            criteria.PageQuery = string.IsNullOrWhiteSpace(criteria.PageQuery) ? Constants.Normal : criteria.PageQuery;

            if (string.Compare(criteria.PageQuery, Constants.Special) == 0
                || string.Compare(criteria.PageQuery, Constants.AllSpecial) == 0)
            {
                publicMessage = 2;
                special = 1;
            }

            if (string.Compare(criteria.PageQuery, Constants.Normal) == 0)
            {
                publicMessage = 1;
                special = 0;
            }

            if (string.Compare(criteria.PageQuery, Constants.Private) == 0)
            {
                publicMessage = 0;
                special = 2;
            }

            criteria.Special = special;
            criteria.IsAgent = true;

            IEnumerable<MessageModel> listMessage = null;

            switch (publicMessage)
            {
                case 0:
                    listMessage = GetListMessage(criteria.FromDate.Value, criteria.ToDate.Value, criteria.Record, false, 1);
                    break;

                case 1:
                    listMessage = GetListMessage(criteria.FromDate.Value, criteria.ToDate.Value, criteria.Record, false, 3);
                    break;

                default:
                    listMessage = GetListMessage(criteria.FromDate.Value, criteria.ToDate.Value, criteria.Record, true, 3);
                    break;
            }

            ViewBag.Messages = (listMessage ?? new List<MessageModel>()).ToList();
            ViewBag.UserLang = this.CurrentSession.Language;
            return View(criteria);
        }

        #region Ajax request

        /// <summary>
        /// Get message for ajax.
        /// </summary>
        /// <returns>Content of message.</returns>
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult GetMessage()
        {
            // Message type from older source code
            int messageType = 3;

            int agentId = this.CurrentSession.AgentId;
            string username = this.CurrentSession.Username;

            object[] publicMessages = new object[3];
            object[] privateMessages = new object[2];

            var pubMessage = this._messageService.GetTopPublicMessage(messageType, AppSettings.SiteId);
            if (!ReferenceEquals(pubMessage, null))
            {
                publicMessages[0] = pubMessage.MessageId;
                publicMessages[1] = pubMessage.GetContent(CurrentSession.Language);
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
                privateMessages[1] = priMessage.GetContent(CurrentSession.Language);
            }

            var countPriMessage = this._messageService.CountUnreadPrivateMessage(agentId);

            return Json(new
            {
                public_message = publicMessages,
                private_message = privateMessages,
                unread_private_messages = countPriMessage
            });
        }

        /// <summary>
        /// Gets the level message.
        /// </summary>
        /// <returns>JSON Result.</returns>
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GetLevelMessage()
        {
            var messages = this._messageService.GetTopPublicMessageLevel(AppSettings.SiteId, this.CurrentSession.AgentLevel);
            if (messages != null)
            {
                return Json(
                    new
                    {
                        public_message_content = messages.GetContent(CurrentSession.Language),
                        public_message_id = messages.MessageId
                    },
                JsonRequestBehavior.AllowGet);
            }

            return Json(
                new
                {
                    public_message_content = string.Empty,
                    public_message_id = 0
                },
            JsonRequestBehavior.AllowGet);
        }

        #endregion Ajax request

        /// <summary>
        /// Gets the list message.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="record">The record.</param>
        /// <param name="isSpecial">If set to <c>true</c> [is special].</param>
        /// <param name="messageType">Type of the message.</param>
        /// <returns>List of message.</returns>
        private IEnumerable<MessageModel> GetListMessage(DateTime startDate, DateTime endDate, int record, bool isSpecial = false, byte messageType = 0)
        {
            return _messageService.GetMessage(new GetListMessageCriteria
            {
                CustomerId = this.CurrentSession.AgentId,
                SiteIds = AppSettings.SiteId.ToString(),
                StartDate = startDate,
                EndDate = endDate,
                IsSpecial = isSpecial,
                IsTest = false,
                MsgType = messageType,
                Record = record
            });
        }
    }
}