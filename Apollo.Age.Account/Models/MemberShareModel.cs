// --------------------------------------------------------------------------
// <copyright file="MemberShareModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.17">Created</change>
// </history>

namespace Apollo.Age.Account.Models
{
    /// <summary>
    /// Member share model.
    /// </summary>
    public class MemberShareModel : AgentShareModel
    {
        public short TakenShareMember { get; set; }
    }
}