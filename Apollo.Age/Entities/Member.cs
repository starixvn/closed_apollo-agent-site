// --------------------------------------------------------------------------
// <copyright file="Member.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.18">Created</change>
// </history>

namespace Apollo.Age.Entities
{
    /// <summary>
    /// This class is model that inherited from customer.
    /// </summary>
    public class Member : Customer
    {
        public string Group { get; set; }
    }
}