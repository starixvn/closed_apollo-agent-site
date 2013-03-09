// ---------------------------------------------------------------------------------
// <copyright file="Trans71.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.26">Create</change>
//     <change who="Marc.Bui" date="2013.01.24">Remove BetSlip, so that Trans9 can reuse Trans1 and therefore
//                                              Trans71 inherits from Trans1 instead of from Trans9</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Builder.TransBuilder
{
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// Casino Games
    /// </summary>
    public class Trans71 : Trans1
    {
        public override Templates.Trans_Template Render(ITicket ticket)
        {
            base.Render(ticket);
            this.Template.TransTime.refNo = ticket.RefNo;

            return Template;
        }
    }
}