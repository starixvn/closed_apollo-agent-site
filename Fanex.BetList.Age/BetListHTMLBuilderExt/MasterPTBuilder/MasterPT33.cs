// ---------------------------------------------------------------------------------
// <copyright file="MasterPT33.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2013.01.28">Create</change>
//     <change who="Marc.Bui" date="2013.02.27">Add GetReferenceData for PT 1233</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Age.BetListHTMLBuilderExt.MasterPTBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Age.ExtTemplate;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;

    public class MasterPT33 : MasterPT1
    {
        protected int winBetId, placeBetId;

        /// <summary>
        /// Initializes a new instance of the <see cref="MasterPT33" /> class.
        /// </summary>
        public MasterPT33()
        {
            winBetId = 31;
            placeBetId = 32;
        }

        public override MasterPT_Template Render(ITicket ticket, List<Core.Entities.ITicketData> ticketData)
        {
            base.Render(ticket, ticketData);

            decimal masterPT_Win = 0;
            decimal masterPT_Place = 0;

            List<ITicketData> refTicketData = this.GetReferenceData(ticket, ticketData);

            if (refTicketData.Count > 1)
            {
                masterPT_Win = refTicketData.FindLast(item => item.BetTypeId == winBetId).MasterPT;
                masterPT_Place = refTicketData.FindLast(item => item.BetTypeId == placeBetId).MasterPT;
            }

            this.Template.MasterPT.masterPT_setting = string.Join(null, new string[]{ "<span class='blue' style='font-weight:normal;'>"
                                                                      ,Resources.BetList.win + ": "
                                                                      ,"<b>",Formatter.FormatNumber2(masterPT_Win*100, null),"%</b>"
                                                                      ,"</span><br>"
                                                                      ,"<span class='' style='font-weight:normal;'>"
                                                                      ,Resources.BetList.place + ": "
                                                                      ,"<b>",Formatter.FormatNumber2(masterPT_Place*100, null),"%</b>"
                                                                      ,"</span>"});

            return this.Template;
        }

        /// <summary>
        /// Gets the reference data.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <returns></returns>
        protected virtual List<ITicketData> GetReferenceData(ITicket ticket, List<ITicketData> ticketData)
        {
            return ticketData.FindAll(item => item.RefNo.Equals(ticket.TransId.ToString()));
        }
    }
}
