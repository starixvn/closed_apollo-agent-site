using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Sunplus.Agent.Model.Entities;

namespace Sunplus.Agent.UI.BetSlip.Factories
{
    public class RenderingFactory
    {
        /// <summary>
        /// Render bet slip 
        /// </summary>
        /// <param name="betSlipData"></param>
        /// <param name="betlistParam"></param>
        /// <returns></returns>
        public static StringBuilder Render(DataTable betSlipData)
        {
            IUserSession user = UserSession.Current;
            var content = new StringBuilder();

            var dataRow = betSlipData.Rows[0];
            var betTypeItem = BetItemFactory.CreateItem(dataRow);
            betTypeItem.AssignData();

            betTypeItem.ToString(ref content);

            return content;
        }
    }
}
