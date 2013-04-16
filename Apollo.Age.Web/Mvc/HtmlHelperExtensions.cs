// --------------------------------------------------------------------------
// <copyright file="HtmlHelperExtensions.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.4.8">Created</change>
//   <change who="Lewis" date="2013.4.15">Added htmlAttributes param for DatePicker</change>
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

        #endregion

        public static MvcHtmlString DatePickerFor<TModel, TValue>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            DateTime? minDate = null,
            DateTime? maxDate = null,
            string formatDate = "MM/dd/yyyy",
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
           string formatDate = "MM/dd/yyyy",
           IEnumerable<DateTime> unavailDates = null)
        {
            if (string.IsNullOrWhiteSpace(formatDate))
            {
                throw new ArgumentNullException("formatDate cannot be blank");
            }

            string disabledDate = (unavailDates != null && unavailDates.Count() > 0)
                ? string.Join(",", unavailDates.Select(d => d.ToString("MM/dd/yyyy")))
                : string.Empty;

            var htmlAttribute = new
            {
                @class = "datepicker",
                mindate = minDate != null && minDate != DateTime.MinValue ? ((DateTime)minDate).ToString("MM/dd/yyyy") : "null",
                maxdate = maxDate != null && maxDate != DateTime.MinValue ? ((DateTime)maxDate).ToString("MM/dd/yyyy") : "null",
                disabledDate = disabledDate
            };

            var dateValue = date.HasValue ? date.Value.ToString(formatDate) : string.Empty;
            var textbox = htmlHelper.TextBox(name, dateValue, htmlAttribute);

            return MvcHtmlString.Create(textbox.ToString());
        }

        /// <summary>
        /// year picker
        /// </summary>
        /// <param name="htmlHelper">html Helper</param>
        /// <param name="name">in name</param>
        /// <param name="fromYear">default from year 2000</param>
        /// <param name="currentYear">>default currentYear year 2000</param>
        /// <returns></returns>
        public static MvcHtmlString YearPicker(this HtmlHelper htmlHelper,
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
    }
}