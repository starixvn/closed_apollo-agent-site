// --------------------------------------------------------------------------
// <copyright file="LanguageManager.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry" date="2013.6.3">Created</change>
//   <change who="Edward.Nguyen" date="2013.7.2">Change to static class</change>
// </history>
namespace Apollo.Age.Web
{
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Resources;
    using System.Threading;
    using Apollo.Age.Resources;

    /// <summary>
    /// Language manager class.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1311:StaticReadonlyFieldsMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
    public static class LanguageManager
    {
        private static readonly CultureInfo usCulture = CultureInfo.CreateSpecificCulture("en-US");
        private static ResourceManager _resourceManagerLang;

        /// <summary>
        /// Initializes static members of the <see cref="LanguageManager"/> class.
        /// </summary>
        static LanguageManager()
        {
            _resourceManagerLang = new ResourceManager(typeof(Lang)) { IgnoreCase = true };
        }

        /// <summary>
        /// Sets the culture.
        /// </summary>
        /// <param name="language">The language.</param>
        public static void SetCulture(string language)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
            Thread.CurrentThread.CurrentCulture.DateTimeFormat = usCulture.DateTimeFormat;
            Thread.CurrentThread.CurrentCulture.NumberFormat = usCulture.NumberFormat;
        }

        /// <summary>
        /// Gets the resource data.
        /// </summary>
        /// <param name="key">The resource key.</param>
        /// <returns>Resource value from a key.</returns>
        public static string GetResourceData(string key)
        {
            if (!ReferenceEquals(_resourceManagerLang, null))
            {
                return _resourceManagerLang.GetString(key);
            }

            return string.Empty;
        }
    }
}