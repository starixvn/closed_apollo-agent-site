// --------------------------------------------------------------------------
// <copyright file="AgentShareViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.17">Created</change>
// </history>

namespace Apollo.Age.Account.ViewModels
{
    using System.Collections.Generic;
    using Apollo.Age.Account.Models;
    using Apollo.Age.Entities;

    public class AgentShareViewModel
    {
        public bool IsAgent { get; set; }

        public string Username { get; set; }

        public byte SportType { get; set; }

        public int BetType { get; set; }

        public List<Sport> SportList { get; set; }

        public List<BetType> BetTypeList { get; set; }

        public List<AgentShareModel> AgentShareModelList { get; set; }
    }
}
