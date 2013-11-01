// --------------------------------------------------------------------------
// <copyright file="TplBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Age.UI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Web;
    using System.Web.UI;
    using Apollo.Age.Web;

    /// <summary>
    /// Base IBC page.
    /// </summary>
    public abstract class TplBase : Page, IBasePage
    {
        // Handle external resources of controls
        protected List<string> RegisteredControls = new List<string>(10);

        protected object _View;

        protected Hashtable _parameters;

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

        /// <summary>
        /// Parameters was parsed in the http request.
        /// </summary>
        public Hashtable Parameters
        {
            get
            {
                return _parameters;
            }
        }

        /// <summary>
        /// Return false that mean the control already registered.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool RegisterControl(string name)
        {
            if (RegisteredControls.Contains(name))
            {
                return false;
            }
            else
            {
                RegisteredControls.Add(name);
            }

            return true;
        }

        #region IBasePage Members

        /// <summary>
        /// Process HttpRequest and return a list of using parameters.
        /// </summary>
        /// <param name="p">List of parameters are using in this page.</param>
        public abstract void ProcessRequest(ref Hashtable p);

        /// <summary>
        /// Validating the request with a list of request parameters.
        /// </summary>
        /// <param name="p">List of request parameters.</param>
        /// <returns>Null is success otherwise is error message.</returns>
        public abstract object ValidateRequest(Hashtable p);

        /// <summary>
        /// Loading data in _BL with provided parameters. This function was called  in http get only.
        /// Inherit from TplPage2 will be supported ProcessRequest method.
        /// </summary>
        /// <param name="p">List of request parameters.</param>
        /// <returns>Return data in array of objects.</returns>
        public abstract object InvokeData(Hashtable p);

        #endregion IBasePage Members

        protected override void InitializeCulture()
        {
            SetLanguage(SessionManager.CurrentSession.Language);
            base.InitializeCulture();
        }

        protected void SetLanguage(string language)
        {
            CultureInfo def = CultureInfo.CreateSpecificCulture("en-US");

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);

            Thread.CurrentThread.CurrentCulture.DateTimeFormat = def.DateTimeFormat;
            Thread.CurrentThread.CurrentCulture.NumberFormat = def.NumberFormat;
        }

        #region RequestUtility

        /// <summary>
        /// Get int request value.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns></returns>
        protected int GetIntRequest(string name, int defaultValue)
        {
            if (string.IsNullOrEmpty(HttpContext.Current.Request[name]))
            {
                return defaultValue;
            }
            else
            {
                int value = 0;

                if (!int.TryParse(HttpContext.Current.Request[name], out value))
                {
                    return defaultValue;
                }

                return value;
            }
        }

        /// <summary>
        /// Get DateTime request.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns></returns>
        protected DateTime GetDateTimeRequest(string name, DateTime defaultValue)
        {
            if (string.IsNullOrEmpty(HttpContext.Current.Request[name]))
            {
                return defaultValue;
            }
            else
            {
                DateTime value = DateTime.Now;
                if (!DateTime.TryParse(HttpContext.Current.Request[name], out value))
                {
                    return defaultValue;
                }

                return value;
            }
        }

        /// <summary>
        /// Get boolean request.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns></returns>
        protected bool GetBooleanRequest(string name, bool defaultValue)
        {
            if (string.IsNullOrEmpty(HttpContext.Current.Request[name]))
            {
                return defaultValue;
            }
            else
            {
                string value = HttpContext.Current.Request[name];

                if (value == "1" || value == "on" || value == "true")
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Get string request.
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns></returns>
        protected string GetStringRequest(string name, string defaultValue)
        {
            if (string.IsNullOrEmpty(HttpContext.Current.Request[name]))
            {
                return defaultValue;
            }
            else
            {
                return HttpContext.Current.Request[name];
            }
        }

        /// <summary>
        /// Get long request.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns></returns>
        protected long GetLongRequest(string name, long defaultValue)
        {
            if (string.IsNullOrEmpty(HttpContext.Current.Request[name]))
            {
                return defaultValue;
            }
            else
            {
                long value = 0;
                if (!long.TryParse(HttpContext.Current.Request[name], out value))
                {
                    return defaultValue;
                }

                return value;
            }
        }

        /// <summary>
        /// Get decimal request.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns></returns>
        protected decimal GetDecimalRequest(string name, decimal defaultValue)
        {
            if (string.IsNullOrEmpty(HttpContext.Current.Request[name]))
            {
                return defaultValue;
            }
            else
            {
                decimal value = 0;

                if (!decimal.TryParse(HttpContext.Current.Request[name], out value))
                {
                    return defaultValue;
                }

                return value;
            }
        }

        /// <summary>
        /// Get double request.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns></returns>
        protected double GetDoubleRequest(string name, double defaultValue)
        {
            if (string.IsNullOrEmpty(HttpContext.Current.Request[name]))
            {
                return defaultValue;
            }
            else
            {
                double value = 0;

                if (!double.TryParse(HttpContext.Current.Request[name], out value))
                {
                    return defaultValue;
                }

                return value;
            }
        }

        #endregion RequestUtility
    }
}