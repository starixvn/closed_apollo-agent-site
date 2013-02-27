namespace Sunplus.Agent.UI.BetList
{
    using System.Collections;
    using System.Data;
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
