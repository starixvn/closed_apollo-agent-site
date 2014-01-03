// --------------------------------------------------------------------------
// <copyright file="AuthModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.26">Created</change>
//   <change who="Anders.Le" date="2013.5.7">Add ErrorMessage</change>
// </history>

namespace Apollo.Age.Integration.Models
{
    using System;

    /// <summary>
    /// It contains properties parsed from JSON response returned from authentication process.
    /// </summary>
    public class AuthModel
    {
        internal AuthModel()
        {
        }

        internal AuthModel(JsonResponseModel jsonResponseModel)
        {
            AccessToken = jsonResponseModel.access_token;
            UserId = Convert.ToInt32(jsonResponseModel.acct_id);
            Username = jsonResponseModel.acct_login;
            Expired = Convert.ToInt32(jsonResponseModel.expires_in);
            TokenType = jsonResponseModel.token_type;
            ErrorCode = string.Empty;
        }

        public string AccessToken { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public int Expired { get; set; }

        public string TokenType { get; set; }

        public string ErrorCode { get; set; }

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Username)
                    && !string.IsNullOrWhiteSpace(AccessToken)
                    && UserId > 0
                    && string.IsNullOrWhiteSpace(ErrorCode);
            }
        }
    }
}
