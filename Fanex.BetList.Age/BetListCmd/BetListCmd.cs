// ---------------------------------------------------------------------------------
// <copyright file="BetListCmd.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2013.02.19">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Age.BetListCmd
{
    using System.Collections;
    using System.Data;
    using Fanex.BetList.Age.Definition;
    using Sunplus.Agent.Model.Entities;

    public abstract class BetListCmd : IBetListCmd
    {
        public string Title { get; set; }

        public IUserSession CurrentUserSession
        {
            get
            {
                return UserSession.Current;
            }
        }

        public BetListType BetListType { get; set; }

        public abstract void SetParams(Hashtable paramters);

        public abstract object Execute();

        public abstract DataTable OutDataTable();
    }
}
