// ---------------------------------------------------------------------------------
// <copyright file="CombMixParlayBetListCmd.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2013.02.28">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Age.BetListCmd
{
    /// <summary>
    /// Summary description for Combination MixParlayBetList Command
    /// </summary>
    public class CombMixParlayBetListCmd : MixParlayBetListCmd
    {
        public CombMixParlayBetListCmd(string sessionId)
            : base(sessionId)
        {
        }

        public override object Execute()
        {
            Result = BetList.GetCombinationMixParlay(TransId);
            return Result;
        }
    }
}
