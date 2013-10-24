// --------------------------------------------------------------------------
// <copyright file="FunctionGroup.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.2">Created</change>
// </history>

namespace Apollo.Age.Entities
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FunctionGroup
    {
        public short Id { get; set; }

        public string Name { get; set; }

        public string GroupKey { get; set; }

        public short DisplayOrder { get; set; }

        public bool IsActive { get; set; }
    }
}
