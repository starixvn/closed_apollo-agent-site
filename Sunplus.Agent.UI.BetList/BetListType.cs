namespace Sunplus.Agent.UI.BetList
{
    public enum BetListType : int
    {
        None = 0x0,

        /// <summary>
        /// Add Winlost column.
        /// </summary>
        Winlost = 0x1,  

        /// <summary>
        /// Add Member information column.
        /// </summary>
        Member = 0x2, 

        /// <summary>
        /// Canceled bet list (will not have total row and postion taking column, including Winlost.
        /// </summary>
        Canceled = 0x4,

        Default = 0x1, // Winlost.

        // Modified by Kaden 20110510
        SmartPhone = 0x8, // Check hide column on SmartPhone

        /// <summary>
        /// Will not have SuperPT, MasterPT, AgentPT
        /// </summary>
        DailyStatement = 0x10
    }
}