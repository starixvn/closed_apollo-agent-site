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
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class BaseController : Controller
    {
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