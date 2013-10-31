// ---------------------------------------------------------------------------------
// <copyright file="Choice1000.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Templates;

    /// <summary>
    /// P2P Games.
    /// </summary>
    public class Choice1000 : Choice1
    {
        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket need to build.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The list of tickets data.</param>
        /// <param name="isShowScoreMap">Is show score map or not.</param>
        /// <param name="betListPage">The bet list page.</param>
        /// <returns>
        /// Choice Template.
        /// </returns>
        public override Choice_Template Render(
            ITicket ticket,
            ITicketHelper ticketHelper,
            List<ITicketData> ticketData,
            bool isShowScoreMap,
            BetListPage betListPage)
        {
            string content = string.Join(
                                            null,
                                            new string[]
                                            {
                                                "<div><span class='",
                                                BL_FAVORITE,
                                                "'>",
                                                Apollo.Age.Resources.BetList.p2pGame,
                                                "</span></div>"
                                            });

            this.Template.SetValue(content);

            return this.Template;
        }

        /// <summary>
        /// Builds the RTF choice.
        /// </summary>
        /// <param name="ticket">The ticket need to build.</param>
        /// <param name="rtfHelper">The RTF helper.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <returns>
        /// Rich text interface.
        /// </returns>
        protected override NPOI.SS.UserModel.IRichTextString BuildRTFChoice(ITicket ticket, RTFHelper rtfHelper, ITicketHelper ticketHelper)
        {
            rtfHelper.RTFRenderer.AddText(Apollo.Age.Resources.BetList.p2pGame, rtfHelper.PosFont);

            var choiceRtf = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();
            return choiceRtf;
        }

        /// <summary>
        /// Builds the choice.
        /// </summary>
        /// <param name="ticket">The ticket need to build.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="betListPage">The bet list page.</param>
        protected override void BuildChoice(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, BetListPage betListPage)
        {
            base.BuildChoice(ticket, ticketHelper, ticketData, betListPage);
            this.Template.handicap = Apollo.Age.Resources.BetList.vs;
        }
    }
}