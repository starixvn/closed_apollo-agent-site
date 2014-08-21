namespace Apollo.BetList.Core.Builder.ChoiceBuilder
{
    using System.Collections.Generic;
    using Apollo.Age.Resources;
    using Apollo.BetList.Core.Entities;

    /// <summary>
    /// HALF TIME 1x2 Asia handicap shorted as "1st 1x2 HDP".
    /// </summary>
    public class Choice127 : Choice1
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
            switch (ticket.BetTeam)
            {
                case "1:1":
                    this.Template.betTeam = BetList.homefg;
                    break;

                case "1:2":
                    this.Template.betTeam = BetList.homelg;
                    break;

                case "2:1":
                    this.Template.betTeam = BetList.awayfg;
                    break;

                case "2:2":
                    this.Template.betTeam = BetList.awaylg;
                    break;

                default:
                    this.Template.betTeam = BetList.nogoal;
                    break;
            }

            this.Template.Score.Visible = false;
        }
    }
}