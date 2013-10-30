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

    public static class HtmlHelperExtensions
    {
        #region DatePickerFor

        //public static MvcHtmlString DatePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
        //    Expression<Func<TModel, TProperty>> expression)
        //{
        //    return htmlHelper.DatePickerFor(expression, format: null);
        //}

        //public static MvcHtmlString DatePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
        //    Expression<Func<TModel, TProperty>> expression, string format)
        //{
        //    return htmlHelper.DatePickerFor(expression, format, minDate: null, maxDate: null);
        //}

        //public static MvcHtmlString DatePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
        //    Expression<Func<TModel, TProperty>> expression, string format,
        //    DateTime? minDate, DateTime? maxDate)
        //{
        //    return htmlHelper.DatePickerFor(expression, format, minDate, maxDate,
        //        unavailableDates: null);
        //}

        //public static MvcHtmlString DatePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
        //    Expression<Func<TModel, TProperty>> expression, string format,
        //    DateTime? minDate, DateTime? maxDate, IEnumerable<DateTime> unavailableDates)
        //{
        //    return htmlHelper.DatePickerFor(expression, format, minDate, maxDate,
        //        unavailableDates, (IDictionary<string, object>)null);
        //}

        //public static MvcHtmlString DatePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
        //    Expression<Func<TModel, TProperty>> expression, string format,
        //    DateTime? minDate, DateTime? maxDate,
        //    IEnumerable<DateTime> unavailableDates, object htmlAttributes)
        //{
        //    return htmlHelper.DatePickerFor(expression, format,
        //        minDate, maxDate, unavailableDates,
        //        HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        //}

        //public static MvcHtmlString DatePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
        //    Expression<Func<TModel, TProperty>> expression, string format,
        //    DateTime? minDate, DateTime? maxDate,
        //    IEnumerable<DateTime> unavailableDates,
        //    IDictionary<string, object> htmlAttributes)
        //{
        //    if (string.IsNullOrWhiteSpace(format))
        //    {
        //        format = "MM/dd/yyyy";
        //    }

        //    //var value = date.HasValue ? date.Value.ToString(formatDate) : string.Empty;

        //    htmlAttributes = htmlAttributes ?? new Dictionary<string, object>();

        //    if (htmlAttributes.ContainsKey("class"))
        //    {
        //        htmlAttributes["class"] += " datepicker";
        //    }
        //    else
        //    {
        //        htmlAttributes.Add("class", "datepicker");
        //    }

        //    #region min/max date
        //    if (minDate != null)
        //    {
        //        htmlAttributes.Add("mindate", minDate);
        //    }

        //    if (maxDate != null)
        //    {
        //        htmlAttributes.Add("maxdate", maxDate);
        //    }
        //    #endregion

        //    #region unavailable dates
        //    if (unavailableDates != null && unavailableDates.Any())
        //    {
        //        htmlAttributes.Add("disabledDates",
        //            string.Join(",", unavailableDates.Select(d => d.ToString("MM/dd/yyyy"))));
        //    }
        //    #endregion

        //    ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
        //    return DatePickerHelper(htmlHelper, metadata,
        //        ExpressionHelper.GetExpressionText(expression),
        //        format, htmlAttributes);
        //}

        //private static MvcHtmlString DatePickerHelper(HtmlHelper htmlHelper, ModelMetadata metadata,
        //    string name, string format, IDictionary<string, object> htmlAttributes)
        //{
        //    string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

        //    if (String.IsNullOrEmpty(fullName))
        //    {
        //        throw new ArgumentException(Resources.Common_NullOrEmpty, "name");
        //    }

        //    TagBuilder tagBuilder = new TagBuilder("input");
        //    tagBuilder.MergeAttributes(htmlAttributes);
        //    tagBuilder.MergeAttribute("type", HtmlHelper.GetInputTypeString(InputType.Text));
        //    tagBuilder.MergeAttribute("name", fullName, true);

        //    DateTime? dateValue = null;
        //    if (metadata.Model is Nullable<DateTime> || metadata.Model is DateTime)
        //    {
        //        dateValue = (DateTime?)metadata.Model;
        //    }
        //    tagBuilder.MergeAttribute("value",
        //        dateValue.HasValue ? dateValue.Value.ToString(format) : string.Empty, true);
        //    tagBuilder.GenerateId(fullName);

        //    ModelState modelState;
        //    if (htmlHelper.ViewData.ModelState.TryGetValue(fullName, out modelState))
        //    {
        //        // If there are any errors for a named field, we add the css attribute.
        //        if (modelState.Errors.Count > 0)
        //        {
        //            tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
        //        }
        //    }

        //    tagBuilder.MergeAttributes(htmlHelper.GetUnobtrusiveValidationAttributes(name, metadata));

        //    return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.SelfClosing));
        //}

        public static DateTime StartOfWeek(this DateTime dt)
        {
            int diff = dt.DayOfWeek - DayOfWeek.Monday;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime EndOfWeek(this DateTime dt)
        {
            return dt.StartOfWeek().AddDays(6);
        }
        #endregion

        #region Date, Year Picker

        public static MvcHtmlString DatePickerFor<TModel, TValue>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            DateTime? minDate = null,
            DateTime? maxDate = null,
            //string formatDate = "M/d/yyyy",
            string formatDate = "yyyy-MM-dd",
            IEnumerable<DateTime> unavailDates = null)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            DateTime? value = DateTime.Now;

            if (metaData.Model is Nullable<DateTime> || metaData.Model is DateTime)
            {
                value = (DateTime?)metaData.Model;
            }
            else
            {
                value = null;
            }

            return html.DatePicker(metaData.PropertyName, value, minDate, maxDate, formatDate, unavailDates);
        }

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
        /// <param name="name">In name.</param>
        /// <param name="fromYear">Default from year 2000.</param>
        /// <param name="currentYear">>Default currentYear year 2000.</param>
        /// <returns></returns>
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

        #endregion

        #region Dropdownlist

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

        #endregion

        #region Resources

        /// <summary>
        /// Returns the value of the specified System.String resource.
        /// </summary>
        /// <param name="key">The name of the resource to get.</param>
        /// <param name="resourceSource">A type from which the resource manager derives all information 
        /// for finding .resources files (default value is Apollo.Age.Resources.Lang).</param>
        /// <returns></returns>
        public static string Resource(this HtmlHelper htmlhelper, string key, Type resourceSource = null)
        {
            return LanguageManager.GetResourceData(key);
        }
        #endregion

        #region Status

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

        #endregion

        #region Format Show Time
        public static HtmlString FormatShowTime(this HtmlHelper htmlhelper, string showTime, bool ignoreTd = false)
        {
            string showTimeFormat = string.IsNullOrEmpty(showTime) ? string.Empty : showTime.Replace("</font>", "<#font>").Replace('/', '-').Replace("<#font>", "</font></br>").TrimEnd('M');

            if (!ignoreTd)
            {
                showTimeFormat = string.Format("<td class=\"c show-time\" >{0}</td>", showTimeFormat);
            }

            return new HtmlString(showTimeFormat);
        }
        #endregion

        #region Distinct by

        public static IEnumerable<TSource> DistinctBy<TSource, TKey> (
            this IEnumerable<TSource> source, 
            Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();

            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        #endregion

        #region Encrypt Username

        public static string EncryptUsername(this string username, string stringEncrypt = "***")
        {
            if (!string.IsNullOrWhiteSpace(username)
                && username.Length > 1)
            {
                return username[0] + stringEncrypt;
            }

            return username;
        }

        #endregion
    }
}