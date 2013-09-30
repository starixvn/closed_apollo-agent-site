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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represent the object returned from authorization model
    /// So pls ignore stylecop rule violations
    /// </summary>
    public class JsonResponseModel
    {
        public string access_token { get; set; }

        public string acct_id { get; set; }

        public string acct_login { get; set; }

        public string expires_in { get; set; }

        public string token_type { get; set; }        
    }
}
