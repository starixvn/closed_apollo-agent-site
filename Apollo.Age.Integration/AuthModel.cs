// --------------------------------------------------------------------------
// <copyright file="AuthModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.26">Created</change>
// </history>


namespace Apollo.Age.Integration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// It contains properies parsed from json respone of authentication process
    /// </summary>
    public class AuthModel
    {
        internal AuthModel(JsonResponseModel jsonResponseModel)
        {
            AccessToken = jsonResponseModel.access_token;
            UserId = Convert.ToInt32(jsonResponseModel.acct_id);
            Username = jsonResponseModel.acct_login;
            Expired = Convert.ToInt32(jsonResponseModel.expires_in);
            TokenType = jsonResponseModel.token_type;
        }

        public string AccessToken { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public int Expired { get; set; }

        public string TokenType { get; set; }

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Username)
            && !string.IsNullOrWhiteSpace(AccessToken)
            && UserId > 0;
            }
        }

    }
}
