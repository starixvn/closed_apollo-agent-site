// --------------------------------------------------------------------------
// <copyright file="ControllerBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.3.27">Created</change>
//   <change who="Anders" date="2013.4.6">Add CurrentSession Property</change>
// </history>

namespace Apollo.Age.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Fanex.Common;

    public class BaseController : Controller
    {        
        protected int AgentId
        {
            get
            {
                return Converter.ToInt(HttpContext.Session[HttpContext.Session.SessionID + "_Apollo-AgentId"]);
            }
        }

        /// <summary>
        ///  
        /// </summary>
        protected ICurrentSession CurrentSession
        {
            get
            {
                ICurrentSession currentSession = (ICurrentSession)Session["Apollo.Age.Web.CurrentSession"];

                return currentSession;
            }
            set
            {
                if (value is ICurrentSession)
                {
                    Session["Apollo.Age.Web.CurrentSession"] = value;
                }
            }            
        }
    }
}
