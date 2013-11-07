// --------------------------------------------------------------------------
// <copyright file="PagerOptionsBuilder.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Apollo.Age.Web.Pager
{
    using System;
    using System.Web.Mvc.Ajax;
    using System.Web.Routing;

    /// <summary>
    /// Pager option builder.
    /// </summary>
    public class PagerOptionsBuilder
    {
        private PagerOptions _pagerOptions;

        public PagerOptionsBuilder(PagerOptions pagerOptions)
        {
            this._pagerOptions = pagerOptions;
        }

        /// <summary>
        /// Set the action name for the pager links. Note that we're always using the current controller.
        /// </summary>
        /// <param name="action">The action name.</param>
        /// <returns>Pager option builder.</returns>
        public PagerOptionsBuilder Action(string action)
        {
            if (action != null)
            {
                if (_pagerOptions.RouteValues.ContainsKey("action"))
                {
                    throw new ArgumentException("The valuesDictionary already contains an action.", "action");
                }

                _pagerOptions.RouteValues.Add("action", action);
                _pagerOptions.Action = action;
            }

            return this;
        }

        /// <summary>
        /// Add a custom route value parameter for the pager links.
        /// </summary>
        /// <param name="name">Input the name.</param>
        /// <param name="value">Input the value.</param>
        /// <returns>Page option builder.</returns>
        public PagerOptionsBuilder AddRouteValue(string name, object value)
        {
            _pagerOptions.RouteValues[name] = value;
            return this;
        }

        /// <summary>
        /// Set custom route value parameters for the pager links.
        /// </summary>
        /// <param name="routeValues">Route values.</param>
        /// <returns>Page option builder.</returns>
        public PagerOptionsBuilder RouteValues(object routeValues)
        {
            RouteValues(new RouteValueDictionary(routeValues));
            return this;
        }

        /// <summary>
        /// Set custom route value parameters for the pager links.
        /// </summary>
        /// <param name="routeValues">Route values.</param>
        /// <returns>Page option builder.</returns>
        public PagerOptionsBuilder RouteValues(RouteValueDictionary routeValues)
        {
            if (routeValues == null)
            {
                throw new ArgumentException("routeValues may not be null", "routeValues");
            }

            this._pagerOptions.RouteValues = routeValues;

            if (!string.IsNullOrWhiteSpace(_pagerOptions.Action) && !_pagerOptions.RouteValues.ContainsKey("action"))
            {
                _pagerOptions.RouteValues.Add("action", _pagerOptions.Action);
            }

            return this;
        }

        /// <summary>
        /// Displays the template.
        /// </summary>
        /// <param name="displayTemplate">The display template.</param>
        /// <returns>Pager Options Builder.</returns>
        public PagerOptionsBuilder DisplayTemplate(string displayTemplate)
        {
            this._pagerOptions.DisplayTemplate = displayTemplate;
            return this;
        }

        /// <summary>
        /// Set the maximum number of pages to show. The default is 10.
        /// </summary>
        /// <param name="maxNrOfPages">The maximum nr of pages.</param>
        /// <returns>
        /// Page option builder.
        /// </returns>
        public PagerOptionsBuilder MaxNrOfPages(int maxNrOfPages)
        {
            this._pagerOptions.MaxNrOfPages = maxNrOfPages;
            return this;
        }

        /// <summary>
        /// Always add the page number to the generated link for the first page.
        /// </summary>
        /// <returns>Page option builder.</returns>
        /// <remarks>
        /// By default we don't add the page number for page 1 because it results in canonical links.
        /// Use this option to override this behavior.
        /// </remarks>
        public PagerOptionsBuilder AlwaysAddFirstPageNumber()
        {
            this._pagerOptions.AlwaysAddFirstPageNumber = true;
            return this;
        }

        /// <summary>
        /// Set the page routeValue key for pagination links.
        /// </summary>
        /// <param name="pageRouteValueKey">The page route value key.</param>
        /// <returns>Page option builder.</returns>        
        public PagerOptionsBuilder PageRouteValueKey(string pageRouteValueKey)
        {
            if (pageRouteValueKey == null)
            {
                throw new ArgumentException("pageRouteValueKey may not be null", "pageRouteValueKey");
            }

            this._pagerOptions.PageRouteValueKey = pageRouteValueKey;

            return this;
        }

        /// <summary>
        /// Set the AjaxOptions.
        /// </summary>
        /// <param name="ajaxOptions">AJAX options.</param>
        /// <returns>Page option builder.</returns>
        internal PagerOptionsBuilder AjaxOptions(AjaxOptions ajaxOptions)
        {
            this._pagerOptions.AjaxOptions = ajaxOptions;
            return this;
        }
    }
}