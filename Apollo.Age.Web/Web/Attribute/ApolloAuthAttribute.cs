// --------------------------------------------------------------------------
// <copyright file="ApolloAuthAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.3">Created</change>
// </history>

namespace Apollo.Age.Web.Web.Attribute
{
    using System.Web;
    using System.Web.Mvc;
    using Apollo.Age.Core;
    using Apollo.Age.Resources;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Services.Interface;
    using Apollo.Age.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ApolloAuthAttribute : AuthorizeAttribute
    {
        #region Member

        private IUserService _agentUserService = new UserService();

        #endregion Member

        #region Properties

        private string _functionKey;

        private Policy _policy;

        public string FunctionKey
        {
            get
            {
                return _functionKey == null ? string.Empty : _functionKey;
            }

            set
            {
                _functionKey = value;
            }
        }

        public Policy Policy
        {
            get
            {
                return _policy == Policy.None ? Policy.Query : _policy;
            }

            set
            {
                _policy = value;
            }
        }

        #endregion Properties

        /// <summary>
        /// Called when a process requests authorization.
        /// </summary>
        /// <param name="filterContext">The filter context, which encapsulates information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute" />.</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (ReferenceEquals(SessionManager.CurrentSession, null))
            {
                filterContext.HttpContext.Response.Redirect(ApoURLs.URLs.Unauthorized, false);
                return;
            }

            var userId = SessionManager.CurrentSession.UserId;

            if (!CheckFunctionAuthorization(userId))
            {
                CloseConnection(filterContext);
                return;
            }

            base.OnAuthorization(filterContext);
        }

        /// <summary>
        /// Checks the function authorization.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Permission of user on function.</returns>
        public bool CheckFunctionAuthorization(int userId)
        {
            if (userId <= 0)
            {
                return false;
            }

            string[] arrayFunctionKey = FunctionKey.Split(';');
            var policy = 0;
            var temp = 0;
            foreach (var itemKey in arrayFunctionKey)
            {
                if (!string.IsNullOrWhiteSpace(itemKey))
                {
                    temp = _agentUserService.GetUserPolicy(userId, itemKey);
                    policy = policy > temp ? policy : temp;
                }
            }

            return policy >= (byte)Policy;
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        private void CloseConnection(AuthorizationContext filterContext)
        {
            HttpResponseBase response = filterContext.HttpContext.Response;

            if (response == null)
            {
                response.Redirect(ApoURLs.URLs.Unauthorized, false);
                return;
            }

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new HttpStatusCodeResult(403, "<center><b>" + Lang.UnauthorizedMsg + "</b></center>");

                response.Buffer = true;
                response.StatusCode = 403; // Unauthorized status code
                response.Write("<center><b>" + Lang.UnauthorizedMsg + "</b></center>");
                response.Flush();
                response.Close();
                response.End();
            }
            else
            {
                response.Redirect("~/Error/AccessDenied", false);
            }
        }
    }
}