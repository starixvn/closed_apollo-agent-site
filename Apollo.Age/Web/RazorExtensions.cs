namespace Apollo.Age.Web
{
    using System.Web.Mvc;

    public static class RazorExtensions
    {
        /// <summary>
        /// Gets global resources
        /// </summary>
        /// <param name="page">Razor WebViewPage</param>
        /// <param name="classKey">A string that represents the ClassKey property of the requested resource object (e.g. Lang)</param>
        /// <param name="key">A string that represents the ResourceKey property of the requested resource object (e.g. lblAgentList)</param>
        /// <returns></returns>
        public static string GlobalResources(this WebViewPage page, string classKey, string key)
        {
            return page.ViewContext.HttpContext.GetGlobalResourceObject(classKey, key) as string;
        }

        public static string GlobalLangResources(this WebViewPage page, string key)
        {
            return GlobalResources(page, "Lang", key);
        }
    }
}