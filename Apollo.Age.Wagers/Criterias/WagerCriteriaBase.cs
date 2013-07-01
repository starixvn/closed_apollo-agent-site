// --------------------------------------------------------------------------
// <copyright file="WagerCriteriaBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.6.30">Created</change>
//   <change who="Anders.Le" date="2013.7.1">Add paging params</change>
// </history>
namespace Apollo.Age.Wagers.Criterias
{
    using Fanex.NAP.Framework.Data;
    using System;

    public class WagerCriteriaBase : CriteriaBase
    {
        private string _groupBetName = string.Empty;

        [SpParam(Ignored = true)]
        public string GroupBetName
        {
            get
            {
                return !string.IsNullOrWhiteSpace(_groupBetName) ? _groupBetName.ToLower() : string.Empty;
            }

            set
            {
                _groupBetName = value;
            }
        }

        public int LoggedId { get; set; }

        public DateTime FromDate { get; set; }

        public string UGroup { get; set; }

        public bool IsDirectMem { get; set; }

        public int LeagueId { get; set; }

        public int PageIndex { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }    
    }
}