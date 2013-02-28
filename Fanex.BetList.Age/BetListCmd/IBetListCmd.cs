// ---------------------------------------------------------------------------------
// <copyright file="IBetListCmd.cs" company="Nexcel Solutions Vietnam">
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

    public interface IBetListCmd
    {
        string Title { get; set; }

        IUserSession CurrentUserSession { get; }

        BetListType BetListType { get; set; }

        void SetParams(Hashtable paramters);

        object Execute();

        DataTable OutDataTable();
    }
}

