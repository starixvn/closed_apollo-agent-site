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
    using System.Web;
    using Apollo.Age.Web;
    using Fanex.BetList.Age.Definition;

    public abstract class BetListCmd : IBetListCmd
    {
        public string Title { get; set; }

        public ICurrentSession CurrentUserSession
        {
            get
            {
                ICurrentSession currentSession = (ICurrentSession)HttpContext.Current.Session["Apollo.Age.Web.CurrentSession"];

                return currentSession;
            }
        }

        public BetListType BetListType { get; set; }

        public abstract void SetParams(Hashtable paramters);

        public abstract object Execute();

        public abstract DataTable OutDataTable();
    }
}