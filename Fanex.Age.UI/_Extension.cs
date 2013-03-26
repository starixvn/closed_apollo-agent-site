using System.Collections;
using System.Web;

/*
 * Created 20091211@Lee - Extension for UI
 * Revision: 
 * ?@? - ... 
 * 
 * !!!CAUTION: ANY REVISION MUST BE APPROVED BEFORE PROCEEDING!!!
 * 
 */

namespace Fanex.Age.UI
{
    public static class _Extension
    {
        /// <summary>
        /// Add a requested parameter from http request.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="key"></param>
        public static void AddHttpParameters(this Hashtable parameters, string key, HttpRequest request)
        {
            parameters.Add(key, request[key]);
        }

        /// <summary>
        /// Add an array of requested parameters from http request.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="keys"></param>
        public static void AddHttpParameters(this Hashtable parameters, string[] keys, HttpRequest request)
        {
            foreach (string key in keys)
            {
                parameters.Add(key, request[key]);
            }
        }

        /// <summary>
        /// Get an attribute of a site map node.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetAttribute(this SiteMapNode node, string name)
        {
            return node[name];
        }
    }
}