// --------------------------------------------------------------------------
// <copyright file="ApolloAuthAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.3">Created</change>
// </history>

namespace Apollo.Age.PermissionControl.Web.Attribute
{
    using System.Web;
    using System.Web.Mvc;
    using Apollo.Age.PermissionControl.Core;
    using Apollo.Age.PermissionControl.Services;
    using Apollo.Age.Resources;
    using Apollo.Age.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ApolloAuthAttribute : AuthorizeAttribute
    {
        #region Member

        private static readonly ICurrentSession CurrentSession = SessionManager.CurrentSession;

        private static readonly IAgentUserService _agentUserService = new AgentUserService();

        #endregion Member

        #region Properties

        private string _functionKey;

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

        private Policy _policy;

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

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            try
            {
                if (filterContext.HttpContext == null)
                {
                    CloseConnection(filterContext);
                    return;
                }

                if (CheckFunctionAuthorization(CurrentSession.UserId))
                {

                }
                else
                {
                    CloseConnection(filterContext);
                }
            }
            catch
            {
                throw;
            }
        }

        public bool CheckFunctionAuthorization(int userId)
        {
            if (userId <= 0)
            {
                return false;
            }

            var policy = _agentUserService.GetUserPolicy(CurrentSession.UserId, FunctionKey);

            return policy >= (byte)Policy;
        }

        private void CloseConnection(AuthorizationContext filterContext)
        {
            try
            {
                if (filterContext == null
                    || filterContext.HttpContext == null
                    || filterContext.HttpContext.Request == null)
                {
                    SignOut();
                    return;
                }

                HttpResponseBase response = filterContext.HttpContext.Response;

                if (response == null)
                {
                    SignOut();
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
                else if (!filterContext.HttpContext.Request.IsAuthenticated)
                {
                    SignOut();
                }
                else
                {
                    response.Redirect("~/Error/Unauthorized");
                }
            }
            catch
            {
                throw;
            }
        }

        private void SignOut()
        {

        }
    }
}
