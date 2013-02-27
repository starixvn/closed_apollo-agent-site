<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._Authorization.ProcessAdminLogin_Handler" %>

using System;
using System.Web;
using System.Web.Security;

using Fanex.Age.UI;
using Sunplus.Agent.Model.Entities;
using Sunplus.Agent.Model.BL.Authorization;
using Sunplus.Agent.Model.BL;

using RSACryptoPad;
using System.Collections;

using CasinoLibrary;
using CasinoSetting;
using System.Configuration;
using Sunplus.Agent.Configurations;
using Sunplus.Shared.ViewLogLib;
using Sunplus.Shared.ViewLogLib.Services;
using Sunplus.Shared.ViewLogLib.Entities;

namespace Sunplus.Agent.UI._Authorization
{
    public class ProcessAdminLogin_Handler : Fanex.Age.UI.TplHandler 
    {       
        const int dwKeySize = 1024;
        private void RedirectToLoginPage()
        {
            Session.Abandon();
            Response.Redirect(FormsAuthentication.LoginUrl);
        }

        private void RedirectToDefaultPage()
        {
            Request.Cookies.Remove(CookieCurrentURLKey);
            FormsAuthentication.RedirectFromLoginPage(CurrentUserSession.UserName, false);
        }

        public string CookieCurrentURLKey
        {
            get
            {
                string name = string.Join("_", new string[] { 
               UserSession.Current.CustId.ToString(), 
               UserSession.Current.SubAccId.ToString(),
               "URL"
            });

                return Sunplus.Agent.Utilities.Encryption.PearsonEncode(name);
            }
        }          
        
        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            string keyPath = Server.MapPath("../../IbcKeys/ibckey.snk");
            
            // Get encrypted parameters: userName, adminId
            string parameter = Request["params"] + string.Empty;
            parameter = parameter.Replace(' ', '+');
            EncryptionThread encryption = new EncryptionThread();

            try
            {
                parameter = encryption.DecryptString(parameter, dwKeySize, keyPath);
            }
            catch
            {
                parameter = null;
            }

            if (null == parameter)
            {
                RedirectToLoginPage();
                return;
            }

            string[] ps = parameter.Split('&');
            p.Add("parameter", ps);
        }

        string userName = null;
        int adminId;
        DateTime loginTime;
        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            string[] ps = p["parameter"] as string[];

            userName = ps[0];
            adminId = 0;
            long timeTicks;

            if (ps.Length < 2 || string.IsNullOrEmpty(userName) || !int.TryParse(ps[1], out adminId) || !long.TryParse(ps[2], out timeTicks))
            {
                RedirectToLoginPage();
                return new Result(ErrorType.AccessDenied, Resources.Lang.strNotAccessPage);
            }

            loginTime = new DateTime(timeTicks);

            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            AuthorizationBO bo = new AuthorizationBO(CurrentUserSession.NetSessionId);

            int errCode = 0;
            string errMsg = null;

            IUserSession session = CurrentUserSession;
            //Get Browser
            Hashtable requestInfo = Sunplus.Agent.Model.Services.DeviceService.GetRequestInfo(Request);
            
            bool isSuccess = bo.InternalSignIn(userName, adminId, loginTime, requestInfo, out errCode, out errMsg, ref session);

            if (!isSuccess)
            {                
                RedirectToLoginPage();
            }
            else
            {
                if (!Sunplus.Agent.Model.BL.Definitions.AppSettings.LocalTest)
                {
                    string[] domains = Sunplus.Agent.Model.BL.Definitions.AppSettings.CookielessDomainCollection;
                    TplCore.TplSettings.SetCookielessDomains(domains);
                }
                
                RedirectToDefaultPage();
            }
            
            return null;
        }

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            
        }
    }
}