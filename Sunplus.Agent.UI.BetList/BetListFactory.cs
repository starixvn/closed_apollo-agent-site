namespace Sunplus.Agent.UI.BetList
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Summary description for Factory
    /// </summary>
    public class BetListFactory
    {
        public static BetListCmd CreateBetList(string betListType, string sessionId, ICollection<KeyValuePair<string, bool>> permission)
        {
            string typeName = string.Join(string.Empty, new string[] { "Sunplus.Agent.UI.BetList.", betListType, "Cmd" });

            Type type = Type.GetType(typeName);
            if (type == null)
            {
                return CreateBetList(sessionId, permission);
            }

            if (type.Name.Equals("WLByDateCmd"))
            {
                return Activator.CreateInstance(type, sessionId, permission) as BetListCmd;
            }
            else
            {
                return Activator.CreateInstance(type, sessionId) as BetListCmd;
            }
        }

        public static BetListCmd CreateBetList(string sessionId, ICollection<KeyValuePair<string, bool>> permission)
        {
            return CreateBetList(Fanex.Age.UI.Common.BetListWinLossByDate, sessionId, permission);
        }        
    }
}