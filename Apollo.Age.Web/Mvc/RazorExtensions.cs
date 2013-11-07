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
    using System.Web.WebPages;

    /// <summary>
    /// Razor extensions.
    /// </summary>
    public static class RazorExtensions
    {
        /// <summary>
        /// Lists the specified items.
        /// </summary>
        /// <typeparam name="T">The object.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="template">The template.</param>
        /// <returns>Helper result.</returns>
        public static HelperResult List<T>(this IEnumerable<T> items, Func<T, HelperResult> template)
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