namespace Sunplus.Agent.UI.BetList
{
    using System.Collections;
    using System.Data;
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
