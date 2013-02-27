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
    public interface IHandHistoryDetailBusiness
    {
        HoldemHandHistoryDetail GetHoldemHandHistoryDetail(Int64 handNo);
        ThirteenCardsHandHistoryDetail GetThirteenCardsHandHistoryDetail(Int64 handNo);
    }
}
