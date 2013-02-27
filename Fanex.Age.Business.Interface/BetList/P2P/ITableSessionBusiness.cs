using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Age.DTO.BetList.P2P;

/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Fanex.Age.Business.Interface.BetList.P2P
{
    public interface ITableSessionBusiness
    {
        TableSession GetTableSession(Int64 refNo, string userCurrency);
    }
}
