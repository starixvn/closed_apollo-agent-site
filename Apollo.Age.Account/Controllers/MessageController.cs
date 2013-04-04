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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MessageController : BaseController
    {
        private MessageService _messageService;

        public MessageController(MessageService messageService = null)
        {
            _messageService = messageService;
        }

        public ViewResult Announcement()
        {
            return View();
        }
    }
}
