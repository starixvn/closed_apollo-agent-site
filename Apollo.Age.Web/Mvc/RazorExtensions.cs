// --------------------------------------------------------------------------
// <copyright file="RazorExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.6.30">Created</change>
// </history>

namespace Apollo.Age.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Apollo.Age.Web.Properties;
    using System.Text;
    using System.Web;
    using System.Web.WebPages;
    using System.Web.Compilation;
    using System.Globalization;
    using System.Resources;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Resources;

    /// <summary>
    /// <example>http://haacked.com/archive/2011/02/27/templated-razor-delegates.aspx</example> 
    /// </summary>
    public static class RazorExtensions
    {
        public static HelperResult List<T>(this IEnumerable<T> items,
          Func<T, HelperResult> template)
        {
            if (items != null && items.Any())
            {
                return new HelperResult(writer =>
                {
                    foreach (var item in items)
                    {
                        template(item).WriteTo(writer);
                    }
                });
            }

            return new HelperResult(writer =>
            {
                return;
            });
        }
    }
}
