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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AuthenticationException : Exception
    {
        public int UserId { get; set; }
        public string Lang { get; set; }
    }
}
