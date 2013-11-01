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
            public static int MaxNrOfPages = DefaultDefaults.MAXNROFPAGES;

            public static string DisplayTemplate = DefaultDefaults.DISPLAYTEMPLATE;

            public static bool AlwaysAddFirstPageNumber = DefaultDefaults.ALWAYSADDFIRSTPAGENUMBER;

            public static string DefaultPageRouteValueKey = DefaultDefaults.DEFAULTPAGEROUTEVALUEKEY;

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
