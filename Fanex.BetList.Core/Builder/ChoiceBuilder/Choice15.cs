// ---------------------------------------------------------------------------------
// <copyright file="Choice15.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Builder.ChoiceBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.App_GlobalResources;
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// 1st 1X2
    /// </summary>
    public class Choice15 : Choice1
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

            this.Template.betTeam = string.Join(
                                                    null, 
                                                    new string[] 
                                                    { 
                                                        CoreBetList.ht, 
                                                        ".", 
                                                        ticket.BetTeam.ToUpper() 
                                                    }
                                               );

            this.Template.betTeamClassName = BL_FAVORITE;
            this.Template.Handicap.handicap = null;
        }
    }
}