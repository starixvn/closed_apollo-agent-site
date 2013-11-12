// --------------------------------------------------------------------------
// <copyright file="PagerOptions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Apollo.Age.Web.Pager
{
    using System.Web.Mvc.Ajax;
    using System.Web.Routing;

    /// <summary>
    /// Pager options.
    /// </summary>
    public class PagerOptions
    {
        public PagerOptions()
        {
            RouteValues = new RouteValueDictionary();
            DisplayTemplate = Defaults.DisplayTemplate;
            MaxNrOfPages = Defaults.MaxNrOfPages;
            AlwaysAddFirstPageNumber = Defaults.AlwaysAddFirstPageNumber;
            PageRouteValueKey = Defaults.DefaultPageRouteValueKey;
        }

        public RouteValueDictionary RouteValues { get; internal set; }

        public string DisplayTemplate { get; internal set; }

        public int MaxNrOfPages { get; internal set; }

        public AjaxOptions AjaxOptions { get; internal set; }

        public bool AlwaysAddFirstPageNumber { get; internal set; }

        public string Action { get; internal set; }

        public string PageRouteValueKey { get; set; }

        /// <summary>
        /// Default defaults.
        /// </summary>
        public static class DefaultDefaults
        {
            public const int MAXNROFPAGES = 3;
            public const string DISPLAYTEMPLATE = null;
            public const bool ALWAYSADDFIRSTPAGENUMBER = false;
            public const string DEFAULTPAGEROUTEVALUEKEY = "page";
        }

        /// <summary>
        /// The static Defaults class allows you to set Pager defaults for the entire application. 
        /// Set values at application startup.
        /// </summary>
        public static class Defaults
        {
            private static int _maxNrOfPages = DefaultDefaults.MAXNROFPAGES;
            private static string _displayTemplate = DefaultDefaults.DISPLAYTEMPLATE;
            private static bool _alwaysAddFirstPageNumber = DefaultDefaults.ALWAYSADDFIRSTPAGENUMBER;
            private static string _defaultPageRouteValueKey = DefaultDefaults.DEFAULTPAGEROUTEVALUEKEY;

            /// <summary>
            /// Gets or sets the maximum nr of pages.
            /// </summary>
            /// <value>
            /// The maximum nr of pages.
            /// </value>
            public static int MaxNrOfPages
            {              
                get
                {
                    return _maxNrOfPages;
                }

                set
                {
                    _maxNrOfPages = value;
                }
            }

            /// <summary>
            /// Gets or sets the display template.
            /// </summary>
            /// <value>
            /// The display template.
            /// </value>
            public static string DisplayTemplate 
            {
                get
                {
                    return _displayTemplate;
                }

                set
                {
                    _displayTemplate = value;
                }
            }

            /// <summary>
            /// Gets or sets a value indicating whether [always add first page number].
            /// </summary>
            /// <value>
            /// <c>true</c> if [always add first page number]; otherwise, <c>false</c>.
            /// </value>
            public static bool AlwaysAddFirstPageNumber 
            {
                get 
                {
                    return _alwaysAddFirstPageNumber;
                }

                set
                {
                    _alwaysAddFirstPageNumber = value;
                }
            }

            /// <summary>
            /// Gets or sets the default page route value key.
            /// </summary>
            /// <value>
            /// The default page route value key.
            /// </value>
            public static string DefaultPageRouteValueKey 
            {
                get
                {
                    return _defaultPageRouteValueKey;
                }

                set
                {
                    _defaultPageRouteValueKey = value;
                }
            }

            /// <summary>
            /// Resets this instance.
            /// </summary>
            public static void Reset()
            {
                MaxNrOfPages = DefaultDefaults.MAXNROFPAGES;
                DisplayTemplate = DefaultDefaults.DISPLAYTEMPLATE;
                AlwaysAddFirstPageNumber = DefaultDefaults.ALWAYSADDFIRSTPAGENUMBER;
                DefaultPageRouteValueKey = DefaultDefaults.DEFAULTPAGEROUTEVALUEKEY;
            }
        }
    }
}
