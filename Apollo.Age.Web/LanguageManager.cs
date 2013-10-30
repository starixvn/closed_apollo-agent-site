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
    using System.Globalization;
    using System.Resources;
    using System.Threading;
    using Apollo.Age.Resources;

    public static class LanguageManager
    {
        private static readonly CultureInfo usCulture = CultureInfo.CreateSpecificCulture("en-US");
        private static ResourceManager _resourceManagerLang;

        static LanguageManager()
        {
            _resourceManagerLang = new ResourceManager(typeof(Lang)) { IgnoreCase = true };
        }

        public static void SetCulture(string language)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);

            Thread.CurrentThread.CurrentCulture.DateTimeFormat = usCulture.DateTimeFormat;
            Thread.CurrentThread.CurrentCulture.NumberFormat = usCulture.NumberFormat;
        }

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