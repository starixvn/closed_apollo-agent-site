// --------------------------------------------------------------------------
// <copyright file="_Extension.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lee" date="2013.7.4">Created</change>
// </history>

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
    using System.Collections;
    using System.Web;

    /// <summary>
    /// Extension class.
    /// </summary>
    public static class _Extension
    {
        /// <summary>
        /// Add a requested parameter from http request.
        /// </summary>
        /// <param name="parameters">Input The parameters.</param>
        /// <param name="key">Input The key.</param>
        /// <param name="request">Input The request.</param>
        public static void AddHttpParameters(this Hashtable parameters, string key, HttpRequest request)
        {
            parameters.Add(key, request[key]);
        }

        /// <summary>
        /// Add an array of requested parameters from http request.
        /// </summary>
        /// <param name="parameters">Input The parameters.</param>
        /// <param name="keys">Input The keys.</param>
        /// <param name="request">Input The request.</param>
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
        /// <param name="node">Input The node.</param>
        /// <param name="name">Input The name.</param>
        /// <returns>Node element.</returns>
        public static string GetAttribute(this SiteMapNode node, string name)
        {
            return node[name];
        }
    }
}