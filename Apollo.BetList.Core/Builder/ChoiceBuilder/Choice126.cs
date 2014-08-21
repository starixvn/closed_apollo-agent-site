namespace Apollo.BetList.Core.Builder.ChoiceBuilder
{
    using System.Collections.Generic;
    using Apollo.Age.Resources;
    using Apollo.BetList.Core.Entities;

    /// <summary>
    /// HALF TIME 1x2 Asia handicap shorted as "1st 1x2 HDP".
    /// </summary>
    public class Choice126 : Choice1
    {
        /// <summary>
        /// Builds the choice.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        protected override void BuildChoice(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData)
        {
            base.BuildChoice(ticket, ticketHelper, ticketData);
            this.Template.handicap = BetList.vs;
            if (ticket.BetTeam.ToLower() == "4-over" || ticket.BetTeam.ToLower() == "7-over")
            {
                this.Template.betTeam = ticket.BetTeam.Replace("-", "&");
            }
            else
            {
                this.Template.betTeam = ticket.BetTeam;
            }
        }
    }
}