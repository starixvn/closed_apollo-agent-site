using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sunplus.Shared.ViewLogLib.Entities;
using Sunplus.Agent.Model.Entities;

namespace Sunplus.Shared.ViewLogLib.Services
{
    public interface ILogService<T>
    {
        List<T> GetPaged(IRequestViewLogContext context
            , int pageIndex, int pageSize, out int totalRecords, IList<IExtObject<string, string>> permissions, Dictionary<string, string> p2pStakes);
    }
}
