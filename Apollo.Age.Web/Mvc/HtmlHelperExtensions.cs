// --------------------------------------------------------------------------
// <copyright file="HtmlHelperExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.4.8">Created</change>
//   <change who="Lewis" date="2013.4.15">Added htmlAttributes param for DatePicker</change>
//   <change who="Lewis" date="2013.5.17">Added Resource</change>
//   <change who="Edward.Nguyen" date="2013.6.24">Add get Start of Week</change>
// </history>

namespace Apollo.Age.Web.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Resources;

    /// <summary>
    /// HTML helper extensions class.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        #region DatePickerFor

        /// <summary>
        /// Starts the of week.
        /// </summary>
        /// <param name="dt">The a date.</param>
        /// <returns>Return a start day of a week.</returns>
        public static DateTime StartOfWeek(this DateTime dt)
        {
            int diff = dt.DayOfWeek - DayOfWeek.Monday;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }

        /// <summary>
        /// Ends the of week.
        /// </summary>
        /// <param name="dt">The a date.</param>
        /// <returns>Return a end day of a week.</returns>
        public static DateTime EndOfWeek(this DateTime dt)
        {
            return dt.StartOfWeek().AddDays(6);
        }

        #endregion DatePickerFor

        #region Date, Year Picker

        /// <summary>
        /// Dates the picker for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="minDate">The minimum date.</param>
        /// <param name="maxDate">The maximum date.</param>
        /// <param name="formatDate">The format date.</param>
        /// <param name="unavailDates">The unavailable dates.</param>
        /// <returns>Return MVC HTML string.</returns>
        public static MvcHtmlString DatePickerFor<TModel, TValue>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            DateTime? minDate = null,
            DateTime? maxDate = null,
            string formatDate = "yyyy-MM-dd",
            IEnumerable<DateTime> unavailDates = null)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            DateTime? value = DateTime.Now;

            if (metaData.Model is Nullable || metaData.Model is DateTime)
            {
                value = (DateTime?)metaData.Model;
            }
            else
            {
                value = null;
            }

            return html.DatePicker(metaData.PropertyName, value, minDate, maxDate, formatDate, unavailDates);
        }

        /// <summary>
        /// Dates the picker.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">Input the name.</param>
        /// <param name="date">Input the date.</param>
        /// <param name="minDate">The minimum date.</param>
        /// <param name="maxDate">The maximum date.</param>
        /// <param name="formatDate">The format date.</param>
        /// <param name="unavailDates">The unavailable dates.</param>
        /// <returns>
        /// MVC HTML String.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">FormatDate cannot be blank.</exception>
        public static MvcHtmlString DatePicker(
           this HtmlHelper htmlHelper,
           string name,
           DateTime? date,
           DateTime? minDate = null,
           DateTime? maxDate = null,
           string formatDate = "yyyy-MM-dd",
           IEnumerable<DateTime> unavailDates = null)
        {
            if (string.IsNullOrWhiteSpace(formatDate))
            {
                throw new ArgumentNullException("formatDate cannot be blank");
            }

            string disabledDate = (unavailDates != null && unavailDates.Count() > 0)
                ? string.Join(",", unavailDates.Select(d => d.ToString("yyyy-MM-dd")))
                : string.Empty;

            var htmlAttribute = new
            {
                @class = "datepicker",
                mindate = minDate != null && minDate != DateTime.MinValue ? ((DateTime)minDate).ToString("yyyy-MM-dd") : "null",
                maxdate = maxDate != null && maxDate != DateTime.MinValue ? ((DateTime)maxDate).ToString("yyyy-MM-dd") : "null",
                disabledDate = disabledDate
            };

            var dateValue = date.HasValue ? date.Value.ToString(formatDate) : string.Empty;
            var textbox = htmlHelper.TextBox(name, dateValue, htmlAttribute);

            return MvcHtmlString.Create(textbox.ToString());
        }

        /// <summary>
        /// Year picker.
        /// </summary>
        /// <param name="htmlHelper">HTML Helper.</param>
        /// <param name="name">Input the name.</param>
        /// <param name="fromYear">Default from year 2000.</param>
        /// <param name="currentYear">Default currentYear year 2000.</param>
        /// <param name="hasShowNextYear">If set to <c>true</c> [has show next year].</param>
        /// <returns>
        /// MVC HTML String.
        /// </returns>
        public static MvcHtmlString YearPicker(
            this HtmlHelper htmlHelper,
            string name,
            int fromYear = 2000,
            int currentYear = 2000,
            bool hasShowNextYear = true)
        {
            var list = new List<SelectListItem>();
            var now = DateTime.Now;
            int toYear = now.Year;

            // wanna show next year when the current month is 12
            if (hasShowNextYear)
            {
                if (now.AddMonths(1).Year > toYear)
                {
                    toYear = DateTime.Now.AddYears(1).Year;
                }
            }

            for (var i = toYear; i >= fromYear; i--)
            {
                list.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString(),
                    Selected = i == currentYear
                });
            }

            var dropDownList = htmlHelper.DropDownList(name, list, new { @class = "yearpicker" });
            return MvcHtmlString.Create(dropDownList.ToString());
        }

        #endregion Date, Year Picker

        #region Dropdownlist

        /// <summary>
        /// Drops down list for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="selectList">The select list.</param>
        /// <param name="firstItem">The first item.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>MVC HTML String.</returns>
        public static MvcHtmlString DropDownListFor<TModel, TProperty>(
           this HtmlHelper<TModel> htmlHelper,
           Expression<Func<TModel, TProperty>> expression,
           IEnumerable<SelectListItem> selectList,
           SelectListItem firstItem,
           object htmlAttributes)
        {
            if (firstItem != null)
            {
                if (firstItem.Value != null)
                {
                    List<SelectListItem> list = new List<SelectListItem>();
                    list.Add(firstItem);

                    if (selectList != null)
                    {
                        foreach (SelectListItem item in selectList)
                        {
                            list.Add(item);
                        }
                    }

                    selectList = list;
                }
            }

            return SelectExtensions.DropDownListFor(htmlHelper, expression, selectList, htmlAttributes);
        }

        #endregion Dropdownlist

        #region Resources

        /// <summary>
        /// Returns the value of the specified System.String resource.
        /// </summary>
        /// <param name="htmlhelper">The html helper.</param>
        /// <param name="key">The name of the resource to get.</param>
        /// <param name="resourceSource">A type from which the resource manager derives all information
        /// for finding .resources files (default value is Apollo.Age.Resources.Lang).</param>
        /// <returns>
        /// The resource value.
        /// </returns>
        public static string Resource(this HtmlHelper htmlhelper, string key)
        {
            return LanguageManager.GetResourceData(key);
        }

        #endregion Resources

        #region Status

        /// <summary>
        /// To the string status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="uplineStatus">The UPLINE status.</param>
        /// <returns>MVC HTML String.</returns>
        public static MvcHtmlString ToStringStatus(this byte status, byte uplineStatus)
        {
            string format = "<span class=\"{0}\">{1}</span>";
            MvcHtmlString result = null;

            if (uplineStatus == (int)CustomerStatus.Stopped)
            {
                result = new MvcHtmlString(string.Format(format, "close", Lang.lblCLock));
                return result;
            }

            switch (status)
            {
                case (int)CustomerStatus.Active:
                    result = new MvcHtmlString(string.Format(format, "active", Lang.lblActive));
                    break;

                case (int)CustomerStatus.Stopped:
                    result = new MvcHtmlString(string.Format(format, "close", Lang.stopped));
                    break;

                default:
                    result = new MvcHtmlString(string.Format(format, "suspended", Lang.locked));
                    break;
            }

            return result;
        }

        #endregion Status

        #region Format Show Time

        /// <summary>
        /// Formats the show time.
        /// </summary>
        /// <param name="htmlhelper">The HTML helper.</param>
        /// <param name="showTime">The show time.</param>
        /// <param name="ignoreTd">If set to <c>true</c> [ignore td].</param>
        /// <returns>MVC HTML String.</returns>
        public static HtmlString FormatShowTime(this HtmlHelper htmlhelper, string showTime, bool ignoreTd = false)
        {
            string showTimeFormat = 
                string.IsNullOrEmpty(showTime) 
                ? string.Empty 
                : showTime.Replace("</font>", "<#font>").Replace('/', '-').Replace("<#font>", "</font></br>").TrimEnd('M');

            if (!ignoreTd)
            {
                showTimeFormat = string.Format("<td class=\"c show-time\" >{0}</td>", showTimeFormat);
            }

            return new HtmlString(showTimeFormat);
        }

        #endregion Format Show Time

        #region Distinct by

        /// <summary>
        /// Distinct the by.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns>List of object.</returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();

            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        #endregion Distinct by

        #region Encrypt Username

        /// <summary>
        /// Encrypts the username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="stringEncrypt">The string encrypt.</param>
        /// <returns>The username that is encrypted.</returns>
        public static string EncryptUsername(this string username, string stringEncrypt = "***")
        {
            if (!string.IsNullOrWhiteSpace(username) && username.Length > 1)
            {
                return username[0] + stringEncrypt;
            }

            return username;
        }

        #endregion Encrypt Username
    }
}