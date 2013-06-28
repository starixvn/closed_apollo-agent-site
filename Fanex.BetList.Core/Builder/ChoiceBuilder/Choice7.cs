// ---------------------------------------------------------------------------------
// <copyright file="Choice7.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
// </history>
// ---------------------------------------------------------------------------------

using System.Collections.Generic;
using Fanex.BetList.Core.Entities;

namespace Fanex.BetList.Core.Builder.ChoiceBuilder
{
    /// <summary>
    /// 1st Handicap
    /// </summary>
    public class Choice7 : Choice1
    {
        /// <summary>
        /// Builds the choice.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        protected override void BuildChoice(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData)
        {
            this.Template.handicap = Apollo.Age.Resources.BetList.vs;
        }
    }
}