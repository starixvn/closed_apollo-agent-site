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
        #region Date Picker

        public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name)
        {
            return DatePicker(htmlHelper, name, value: null);
        }

        public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name, object value)
        {
            return DatePicker(htmlHelper, name, value, format: null);
        }

        public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name, object value,
            string format)
        {
            return DatePicker(htmlHelper, name, value, format, htmlAttributes: (object)null);
        }

        public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name, object value,
            object htmlAttributes)
        {
            return DatePicker(htmlHelper, name, value, format: null, htmlAttributes: htmlAttributes);
        }

        public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name, object value,
            string format, object htmlAttributes)
        {
            return DatePicker(htmlHelper, name, value, format, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name, object value,
            IDictionary<string, object> htmlAttributes)
        {
            return DatePicker(htmlHelper, name, value, format: null, htmlAttributes: htmlAttributes);
        }

        public static MvcHtmlString DatePicker(this HtmlHelper htmlHelper, string name, object value,
            string format, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.TextBox(name, value, format, htmlAttributes);

            //return InputHelper(htmlHelper,
            //                   InputType.Text,
            //                   metadata: null,
            //                   name: name,
            //                   value: value,
            //                   useViewData: (value == null),
            //                   isChecked: false,
            //                   setId: true,
            //                   isExplicitValue: true,
            //                   format: format,
            //                   htmlAttributes: htmlAttributes);
        }

        public static MvcHtmlString DatePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            return htmlHelper.DatePickerFor(expression, format: null);
        }

        public static MvcHtmlString DatePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, string format)
        {
            return htmlHelper.DatePickerFor(expression, format, (IDictionary<string, object>)null);
        }

        public static MvcHtmlString DatePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return htmlHelper.DatePickerFor(expression, format: null, htmlAttributes: htmlAttributes);
        }

        public static MvcHtmlString DatePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, string format, object htmlAttributes)
        {
            return htmlHelper.DatePickerFor(expression, format: format,
                htmlAttributes: HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
        
        public static MvcHtmlString DatePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.DatePickerFor(expression, format: null, htmlAttributes: htmlAttributes);
        }
        
        public static MvcHtmlString DatePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, string format, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.TextBoxFor(expression, format, htmlAttributes);

            //return TextBoxHelper(htmlHelper,
            //                     metadata,
            //                     metadata.Model,
            //                     ExpressionHelper.GetExpressionText(expression),
            //                     format,
            //                     htmlAttributes);
        }

        //private static MvcHtmlString TextBoxHelper(this HtmlHelper htmlHelper, ModelMetadata metadata, object model,
        //    string expression, string format, IDictionary<string, object> htmlAttributes)
        //{
        //    return InputHelper(htmlHelper,
        //                       InputType.Text,
        //                       metadata,
        //                       expression,
        //                       model,
        //                       useViewData: false,
        //                       isChecked: false,
        //                       setId: true,
        //                       isExplicitValue: true,
        //                       format: format,
        //                       htmlAttributes: htmlAttributes);
        //}

        //private static MvcHtmlString InputHelper(HtmlHelper htmlHelper, InputType inputType, ModelMetadata metadata, string name, object value, bool useViewData, bool isChecked, bool setId, bool isExplicitValue, string format, IDictionary<string, object> htmlAttributes)
        //{
        //    string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

        //    if (String.IsNullOrEmpty(fullName))
        //    {
        //        throw new ArgumentException(Resources.Common_NullOrEmpty, "name");
        //    }

        //    TagBuilder tagBuilder = new TagBuilder("input");
        //    tagBuilder.MergeAttributes(htmlAttributes);
        //    tagBuilder.MergeAttribute("type", HtmlHelper.GetInputTypeString(inputType));
        //    tagBuilder.MergeAttribute("name", fullName, true);

        //    string valueParameter = htmlHelper.FormatValue(value, format);
        //    bool usedModelState = false;

        //    switch (inputType)
        //    {
        //        case InputType.CheckBox:
        //            bool? modelStateWasChecked = htmlHelper.GetModelStateValue(fullName, typeof(bool)) as bool?;
        //            if (modelStateWasChecked.HasValue)
        //            {
        //                isChecked = modelStateWasChecked.Value;
        //                usedModelState = true;
        //            }
        //            goto case InputType.Radio;
        //        case InputType.Radio:
        //            if (!usedModelState)
        //            {
        //                string modelStateValue = htmlHelper.GetModelStateValue(fullName, typeof(string)) as string;
        //                if (modelStateValue != null)
        //                {
        //                    isChecked = String.Equals(modelStateValue, valueParameter, StringComparison.Ordinal);
        //                    usedModelState = true;
        //                }
        //            }
        //            if (!usedModelState && useViewData)
        //            {
        //                isChecked = htmlHelper.EvalBoolean(fullName);
        //            }
        //            if (isChecked)
        //            {
        //                tagBuilder.MergeAttribute("checked", "checked");
        //            }
        //            tagBuilder.MergeAttribute("value", valueParameter, isExplicitValue);
        //            break;
        //        case InputType.Password:
        //            if (value != null)
        //            {
        //                tagBuilder.MergeAttribute("value", valueParameter, isExplicitValue);
        //            }
        //            break;
        //        default:
        //            string attemptedValue = (string)htmlHelper.GetModelStateValue(fullName, typeof(string));
        //            tagBuilder.MergeAttribute("value", attemptedValue ?? ((useViewData) ? htmlHelper.EvalString(fullName, format) : valueParameter), isExplicitValue);
        //            break;
        //    }

        //    if (setId)
        //    {
        //        tagBuilder.GenerateId(fullName);
        //    }

        //    // If there are any errors for a named field, we add the css attribute.
        //    ModelState modelState;
        //    if (htmlHelper.ViewData.ModelState.TryGetValue(fullName, out modelState))
        //    {
        //        if (modelState.Errors.Count > 0)
        //        {
        //            tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
        //        }
        //    }

        //    tagBuilder.MergeAttributes(htmlHelper.GetUnobtrusiveValidationAttributes(name, metadata));

        //    if (inputType == InputType.CheckBox)
        //    {
        //        // Render an additional <input type="hidden".../> for checkboxes. This
        //        // addresses scenarios where unchecked checkboxes are not sent in the request.
        //        // Sending a hidden input makes it possible to know that the checkbox was present
        //        // on the page when the request was submitted.
        //        StringBuilder inputItemBuilder = new StringBuilder();
        //        inputItemBuilder.Append(tagBuilder.ToString(TagRenderMode.SelfClosing));

        //        TagBuilder hiddenInput = new TagBuilder("input");
        //        hiddenInput.MergeAttribute("type", HtmlHelper.GetInputTypeString(InputType.Hidden));
        //        hiddenInput.MergeAttribute("name", fullName);
        //        hiddenInput.MergeAttribute("value", "false");
        //        inputItemBuilder.Append(hiddenInput.ToString(TagRenderMode.SelfClosing));
        //        return MvcHtmlString.Create(inputItemBuilder.ToString());
        //    }

        //    return tagBuilder.ToMvcHtmlString(TagRenderMode.SelfClosing);
        //}

        //public static MvcHtmlString DatePicker(
        //   this HtmlHelper htmlHelper,
        //   string name,
        //   DateTime? date,
        //   DateTime? minDate = null,
        //   DateTime? maxDate = null,
        //   string formatDate = "MM/dd/yyyy",
        //   IEnumerable<DateTime> unavailDates = null)
        //{
        //    if (string.IsNullOrWhiteSpace(formatDate))
        //    {
        //        throw new ArgumentNullException("formatDate cannot be blank");
        //    }

        //    string disabledDate = (unavailDates != null && unavailDates.Count() > 0)
        //        ? string.Join(",", unavailDates.Select(d => d.ToString("MM/dd/yyyy")))
        //        : string.Empty;

        //    var htmlAttribute = new
        //    {
        //        @class = "datepicker",
        //        mindate = minDate != null && minDate != DateTime.MinValue ? ((DateTime)minDate).ToString("MM/dd/yyyy") : "null",
        //        maxdate = maxDate != null && maxDate != DateTime.MinValue ? ((DateTime)maxDate).ToString("MM/dd/yyyy") : "null",
        //        disabledDate = disabledDate
        //    };

        //    var dateValue = date.HasValue ? date.Value.ToString(formatDate) : string.Empty;
        //    var textbox = htmlHelper.TextBox(name, dateValue, htmlAttribute);

        //    return MvcHtmlString.Create(textbox.ToString());
        //}

        //public static MvcHtmlString DatePickerFor<TModel, TValue>(
        //    this HtmlHelper<TModel> html,
        //    Expression<Func<TModel, TValue>> expression,
        //    DateTime? minDate = null,
        //    DateTime? maxDate = null,
        //    string formatDate = "MM/dd/yyyy",
        //    IEnumerable<DateTime> unavailDates = null)
        //{
        //    var metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
        //    DateTime? value = DateTime.Now;

        //    if (metaData.Model is Nullable<DateTime> || metaData.Model is DateTime)
        //    {
        //        value = (DateTime?)metaData.Model;
        //    }
        //    else
        //    {
        //        value = null;
        //    }

        //    return html.DatePicker(metaData.PropertyName, value, minDate, maxDate, formatDate, unavailDates);
        //}

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
    }
}