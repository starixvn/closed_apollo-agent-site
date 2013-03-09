// ---------------------------------------------------------------------------------
// <copyright file="BetListFactory.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2013.02.28">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Age.BetListCmd
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BetListFactory
    {
        public static BetListCmd CreateBetList(string betListType, string sessionId, ICollection<KeyValuePair<string, bool>> permission)
        {
            string typeName = string.Join(string.Empty, new string[] { "Fanex.BetList.Age.BetListCmd.", betListType, "Cmd" });

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
