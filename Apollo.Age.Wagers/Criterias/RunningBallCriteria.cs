// --------------------------------------------------------------------------
// <copyright file="RunningBallCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.7.3">Created</change>
// </history>
namespace Apollo.Age.Wagers.Criterias
{
    public class RunningBallCriteria : WagerCriteriaBase
    {
        public string Sporttypes { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Wagers_RunningBall";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}