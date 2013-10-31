// --------------------------------------------------------------------------
// <copyright file="FunctionPolicy.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.3">Created</change>
// </history>

namespace Apollo.Age.Models
{
    using Apollo.Age.Entities;

    /// <summary>
    /// Function policy.
    /// </summary>
    public class FunctionPolicy
    {
        public Function Function { get; set; }

        public byte Policy { get; set; }
    }
}
