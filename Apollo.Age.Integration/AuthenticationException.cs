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
    using System.Runtime.Serialization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
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

        /// <summary>
        /// Get object data.
        /// </summary>
        /// <param name="info">Serialization information.</param>
        /// <param name="context">Streaming context.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}