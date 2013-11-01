// --------------------------------------------------------------------------
// <copyright file="JsonResponseModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.5.2">Created</change>
// </history>

namespace Apollo.Age.Integration.Models
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represent the object returned from authorization model
    /// So please ignore stylecop rule violations.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
    public class JsonResponseModel
    {
        public string access_token { get; set; }

        public string acct_id { get; set; }

        public string acct_login { get; set; }

        public string expires_in { get; set; }

        public string token_type { get; set; }
    }
}