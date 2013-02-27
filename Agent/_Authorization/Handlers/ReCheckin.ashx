<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._Authorization.ReCheckin_Handler" %>
/*
 * Created 20100126@Lee - Recheck-in handler. Keep session alive by interval request check in On_User database server.
 * Revision:
 * 20111010@Qui - Locking pattern on checking downtime 
 * 
 */

namespace Sunplus.Agent.UI._Authorization
{
    using System;
    using System.Web;
    using System.Collections;

    using Fanex.Age.UI;

    using Sunplus.Agent.Model.BL;
    using Sunplus.Agent.Model.BL.Authorization;

    using Sunplus.Agent.Model.Services;    
    
    public class ReCheckin_Handler : Fanex.Age.UI.TplHandler, System.Web.SessionState.IReadOnlySessionState
    {
        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            p.AddHttpParameters(
                "onid",
                Request);
        }

        // Under maintenance checking
        static object _locker = new object();
        static DateTime _lastCheckUM = DateTime.Now;
        static bool _isUM = false;
        
        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            int onId = 0;
            int.TryParse(p["onid"] as string, out onId);

            // Invalid re-check-in
            if (onId == 0 || onId != CurrentUserSession.OnUserId || !Request.IsAuthenticated)
            {
                return new Result(ErrorType.AccessDenied, Resources.Lang.strNotAccessPage);
            }


            // Check under maintenance time
            string startTime = string.Empty, endTime = string.Empty;
            // Double check and lock
            if (DateTime.Now.Subtract(_lastCheckUM) > TimeSpan.FromSeconds(5)) // Check down time each 5 seconds
            {
                lock (_locker)
                {
                    if (DateTime.Now.Subtract(_lastCheckUM) > TimeSpan.FromSeconds(5))
                    {

                        GC.Collect(); 
                        _isUM = MaintenanceService.GetDownTime(out startTime, out endTime);
                        _lastCheckUM = DateTime.Now;
                    }
                }
            }

            if (_isUM)
            {
                return new Result(ErrorType.AccessDenied, string.Format(Resources.Lang.undermaintenance, startTime, endTime));
            }
            
            // Validating success
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            int custId = CurrentUserSession.CustId;
            Hashtable statusCustomer = new Hashtable();
            bool isClosed;
            
            // Hard-code: exclude itibc, itmaster and testcs account
            if (custId == 68287 || custId == 68289 || custId == 1373389 || custId == 1373580 || custId == 1374738 || custId == 4317260 || custId == 2)
            {
                return new Result(ErrorType.Success, string.Empty);
            }

            // Perform re-check-in
            AuthorizationBO bo = new AuthorizationBO(CurrentUserSession.NetSessionId);
            
            string errMsg = null;
            
            bool isSuccess = (custId != 0) && bo.ReCheckin(out errMsg);
            if (isSuccess)
            {
                bo.GetCustomerStatus(custId, CurrentUserSession.AdminId, ref statusCustomer);
                bool.TryParse(statusCustomer["closed"].ToString(), out isClosed);
                isSuccess = !isClosed;
            }

            int errCode = isSuccess ? 0 : (int)(ErrorType.KickedOut);
            
            errMsg = isSuccess ? string.Empty : Resources.Lang.stringSessionLogged;
            
            return new Result(errCode, errMsg);
        }

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            Result r = View as Result;
            data.AsResult(r);
        }
    }
}