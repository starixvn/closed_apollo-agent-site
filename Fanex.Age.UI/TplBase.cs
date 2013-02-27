using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Collections;

using Sunplus.Agent.Model.Entities;
using Sunplus.Agent.Model.BL;

/*
 * Created 20091211@Lee - Base aspx page
 * Revision: 
 * 20091211@Lee - Initializing
 * 
 * !!!CAUTION: ANY REVISION MUST BE APPROVED BEFORE PROCEEDING!!!
 * 
 */

namespace Fanex.Age.UI
{
    /// <summary>
    /// Base IBC page.
    /// </summary>
    public abstract class TplBase : System.Web.UI.Page, IBasePage
    {
        protected override void InitializeCulture()
        {
            SetLanguage(CurrentUserSession.Language);
            base.InitializeCulture();
        }

        protected void SetLanguage(string language)
        {
            System.Globalization.CultureInfo def = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");

            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(language);
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(language);

            System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat = def.DateTimeFormat;
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat = def.NumberFormat;
        }

        // Handle external resources of controls
        protected List<string> RegisteredControls = new List<string>(10);
        /// <summary>
        /// Return false that mean the control aldready registered.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool RegisterControl(string name)
        {
            if (RegisteredControls.Contains(name)) return false;
            else RegisteredControls.Add(name);
            return true;
        }
        
        private IUserSession _CurrentUserSession;
        /// <summary>
        /// Current user session.
        /// </summary>
        public IUserSession CurrentUserSession
        {
            get
            {
                if (null == HttpContext.Current.Session) return null;

                if (null == _CurrentUserSession)
                {
                    _CurrentUserSession = UserSessionService.GetUserSessionInstance(HttpContext.Current.Session.SessionID);
                }

                return _CurrentUserSession;
            }
        }

        protected object _View;
        /// <summary>
        /// Data was loaded in InvokeData.
        /// </summary>
        public object View
        {
            get 
            {
                return _View;
            }
        }

        protected Hashtable _Parameters;
        /// <summary>
        /// Parameters was parsed in the http request.
        /// </summary>
        public Hashtable Parameters
        {
            get 
            {
                return _Parameters;
            }
        }

        #region IBasePage Members

        /// <summary>
        /// Process HttpRequest and return a list of using params.
        /// </summary>
        /// <param name="p">List of params are using in this page</param>
        public abstract void ProcessRequest(ref Hashtable p);

        /// <summary>
        /// Validating the request with a list of request params.
        /// </summary>
        /// <param name="p">List of request params</param>
        /// <returns>Null is success otherwise is error message</returns>
        public abstract object ValidateRequest(Hashtable p);

        /// <summary>
        /// Loading data in _BL with provided params. This function was called  in http get only. 
        /// Inherit from TplPage2 will be supported ProcessRequest method.
        /// </summary>
        /// <param name="p">List of request params</param>
        /// <returns>Return data in array of objects</returns>
        public abstract object InvokeData(Hashtable p);

        #endregion

        #region RequestUtility

        /// <summary>
        /// Get int request value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected int GetIntRequest(string name, int defaultValue)
        {
            if (string.IsNullOrEmpty(HttpContext.Current.Request[name])) return defaultValue;
            else
            {
                int value = 0;
                if (!int.TryParse(HttpContext.Current.Request[name], out value)) return defaultValue;
                return value;
            }
        }
        /// <summary>
        /// Get DateTime request
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected DateTime GetDateTimeRequest(string name, DateTime defaultValue)
        {
            if (string.IsNullOrEmpty(HttpContext.Current.Request[name])) return defaultValue;
            else
            {
                DateTime value = DateTime.Now;
                if (!DateTime.TryParse(HttpContext.Current.Request[name], out value)) return defaultValue;
                return value;
            }
        }
        /// <summary>
        /// Get boolean request
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected bool GetBooleanRequest(string name, bool defaultValue)
        {
            if (string.IsNullOrEmpty(HttpContext.Current.Request[name])) return defaultValue;
            else
            {
                string value=HttpContext.Current.Request[name];
                if(value=="1"||value=="on"||value=="true") return true;

                return false;
            }
        }
        /// <summary>
        /// Get string request
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected string GetStringRequest(string name, string defaultValue)
        {
            if (string.IsNullOrEmpty(HttpContext.Current.Request[name])) return defaultValue;
            else return HttpContext.Current.Request[name];
        }
        /// <summary>
        /// Get long request
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected long GetLongRequest(string name, long defaultValue)
        {
            if (string.IsNullOrEmpty(HttpContext.Current.Request[name])) return defaultValue;
            else
            {
                long value = 0;
                if (!long.TryParse(HttpContext.Current.Request[name], out value)) return defaultValue;
                return value;
            }
        }
        /// <summary>
        /// Get decimal request
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected decimal GetDecimalRequest(string name, decimal defaultValue)
        {
            if (string.IsNullOrEmpty(HttpContext.Current.Request[name])) return defaultValue;
            else
            {
                decimal value = 0;
                if (!decimal.TryParse(HttpContext.Current.Request[name], out value)) return defaultValue;
                return value;
            }
        }
        /// <summary>
        /// Get double request
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected double GetDoubleRequest(string name, double defaultValue)
        {
            if (string.IsNullOrEmpty(HttpContext.Current.Request[name])) return defaultValue;
            else
            {
                double value = 0;
                if (!double.TryParse(HttpContext.Current.Request[name], out value)) return defaultValue;
                return value;
            }
        }
        #endregion
    }
}