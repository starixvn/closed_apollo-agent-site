// --------------------------------------------------------------------------
// <copyright file="WagerCriteriaBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.6.30">Created</change>
//   <change who="Anders.Le" date="2013.7.1">Add paging params</change>
//   <change who="Edward.Nguyen" date="2013.7.1">Add get page size from app setting</change>
// </history>
namespace Apollo.Age.Wagers.Criterias
{
    using System;
    using System.Configuration;
    using Fanex.NAP.Framework.Data;

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

        [SpParam(Ignored = true)]
        public int PageIndex { get; set; }

        [SpParam(Ignored = true)]
        public int TotalPages { get; set; }

        [SpParam(Ignored = true)]
        public int PageSize
        {
            get
            {
                int pageSize = 0;

                int.TryParse(ConfigurationManager.AppSettings["PageSize"].ToString(), out pageSize);

                return pageSize;
            }
        }
    }
}