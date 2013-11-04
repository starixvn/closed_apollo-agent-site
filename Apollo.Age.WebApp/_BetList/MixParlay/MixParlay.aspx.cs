// ---------------------------------------------------------------------------------
// <copyright file="MixParlay.aspx.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//      <change who="Kaden.Trinh" date="2013.01.12">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Sunplus.Agent.UI._BetList.MixParlay
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Web.UI;
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.BetList.Services;
    using Fanex.Age.UI;
    using Fanex.Age.UI.Components;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;

    /// <summary>
    /// Mix parlay page.
    /// </summary>
    public partial class MixParlay_Page : TplPage
    {        
        long transid;

        public override void RenderContent(HtmlTextWriter writer)
        {
            var tickets = View as IList<ITicket>;

            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            ITicketHelper ticketHelper = new TicketHelper();

            ticketHelper.IsShowOddsWithChoice = true;

            string line = "<div class='line'></div>";

            for (int i = 0; i < tickets.Count; i++)
            {
                if (i == tickets.Count - 1)
                {
                    line = "<div></div>";
                }

                if (tickets[i].StatusId == (int)Apollo.Age.Enumerations.BetStatus.Void)
                {
                    builder.AddChoice(tickets[i], ticketHelper, null, "<div class='strike_through'>", "</div>" + line, false);
                }
                else
                {
                    builder.AddChoice(tickets[i], ticketHelper, null, string.Empty, line, false);
                }
            }

            string row = builder.GetBetList();

            writer.Write(row.ToString());
        }

        public override object InvokeData(Hashtable hash)
        {
            var betListService = new BetListService();
            var criteria = new BetListCriteria
            {
                TransId = transid,
            };
            var mainTickets = betListService.GetMixParlayDetailTicket(criteria);
            return mainTickets;
        }

        public override void ProcessRequest(ref Hashtable hash)
        {
            long.TryParse(Request["transid"].ToString(), out transid);
        }

        public override object ValidateRequest(Hashtable p)
        {
            if (transid < 0)
            {
                return new ErrorPage_Page(Apollo.Age.Resources.Lang.strNotAccessPage);
            }

            return null;
        }
    }
}