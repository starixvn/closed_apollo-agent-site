// --------------------------------------------------------------------------
// <copyright file="IBasePage.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Age.UI
{
    using System.Collections;

    public interface IBasePage
    {
        /// <summary>
        /// Process the request and return requested parameters.
        /// </summary>
        /// <param name="p">The hashtable.</param>
        void ProcessRequest(ref Hashtable p);

        /// <summary>
        /// Validating the request with a list of request parameters.
        /// </summary>
        /// <param name="p">List of request parameters.</param>
        /// <returns>Null is success, otherwise is error.</returns>
        object ValidateRequest(Hashtable p);

        /// <summary>
        /// Process data working with provided parameters.
        /// </summary>
        /// <param name="p">The Hashtable.</param>
        /// <returns>Return object.</returns>
        object InvokeData(Hashtable p);
    }
}
