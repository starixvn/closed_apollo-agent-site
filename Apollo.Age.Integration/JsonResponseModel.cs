// -----------------------------------------------------------------------
// <copyright file="JsonResultModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Apollo.Age.Integration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    internal class JsonResponseModel
    {
        internal string access_token { get; set; }

        internal string acct_id { get; set; }

        internal string acct_login { get; set; }

        internal string expires_in { get; set; }

        internal string token_type { get; set; }        
    }
}
