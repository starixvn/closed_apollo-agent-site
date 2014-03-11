// --------------------------------------------------------------------------
// <copyright file="StringValueAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.12">Created</change>
// </history>

namespace Apollo.Age.Enumerations
{
    using System;

    /// <summary>
    /// String value attribute.
    /// </summary>
    public class StringValueAttribute : Attribute
    {
        private readonly string _value;

        public StringValueAttribute(string value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value;
        }
    }
}