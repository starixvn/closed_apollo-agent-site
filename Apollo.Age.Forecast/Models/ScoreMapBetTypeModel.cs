// --------------------------------------------------------------------------
// <copyright file="ScoreMapBetTypeModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.9">Created</change>
// </history>

namespace Apollo.Age.Forecast.Models
{
    using System;
    using Apollo.Age.Services;

    /// <summary>
    /// Score map bet type model.
    /// </summary>
    [Serializable]
    public class ScoreMapBetTypeModel
    {
        public int TypeId { get; set; }

        public int IsLive { get; set; }

        public int IsDead { get; set; }

        public int Id1st { get; set; }

        public string BetTypeName
        {
            get
            {
                return CacheRef.GetBettype(this.TypeId);
            }
        }
    }
}