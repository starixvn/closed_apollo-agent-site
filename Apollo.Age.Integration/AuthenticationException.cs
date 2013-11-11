// --------------------------------------------------------------------------
// <copyright file="AuthenticationException.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.4.28">Created</change>
// </history>

namespace Apollo.Age.Integration
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AuthenticationException : Exception
    {
        public int UserId { get; set; }

        public string Lang { get; set; }

        public override string Message
        {
            get
            {
                return base.Message + "UserId=" + UserId.ToString() + ";Lang=" + Lang;
            }
        }

        /// <summary>
        /// Override TOSTRING.
        /// </summary>
        /// <returns>Return a value with user id and language.</returns>        
        public override string ToString()
        {
            return base.ToString() + "UserId=" + UserId.ToString() + ";Lang=" + Lang;
        }
    }
}