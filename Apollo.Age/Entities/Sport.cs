// --------------------------------------------------------------------------
// <copyright file="Sport.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.10">Created</change>
// </history>

namespace Apollo.Age.Entities
{
    /// <summary>
    /// This class to override equals.
    /// </summary>
    public class Sport
    {
        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Override equals.
        /// </summary>
        /// <param name="obj">Input a object.</param>
        /// <returns>Return true or false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Sport)
            {
                return ((Sport)obj).Id == this.Id;
            }

            return false;
        }

        /// <summary>
        /// Override get hash code.
        /// </summary>
        /// <returns>Return a integer value.</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}