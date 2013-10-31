// ---------------------------------------------------------------------------------
// <copyright file="Stake29.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Builder.StakeBuilder
{
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Templates;
    using Fanex.BetList.Core.Utils;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Stake 29.
    /// </summary>
    public class Stake29 : IStake
    {
        public Stake_Template Template { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stake29" /> class.
        /// </summary>
        public Stake29()
        {
            this.Template = new Stake_Template();
        }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>Stake_Template.</returns>
        public Stake_Template Render(ITicket ticket)
        {
            this.Template.stake = (Cast.AsDecimal(ticket.Stake) == 0)
                          ? ConvertByBetType.Stake(ticket.ActualStake)
                          : string.Join(
                                            null,
                                            new string[] 
                                            { 
                                                ConvertByBetType.Stake(ticket.ActualStake), 
                                                "<br/>", 
                                                "&nbsp;(", 
                                                ConvertByBetType.Stake(ticket.Stake), 
                                                ")" 
                                            });

            return Template;
        }

        /// <summary>
        /// Renders the RTF.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>
        /// IRichTextString
        /// </returns>
        public IRichTextString RenderRTF(ITicket ticket, RTFHelper rtfHelper)
        {
            Render(ticket);
            Template.stake = Template.stake.Replace("<br/>&nbsp;", "\n");

            rtfHelper.RTFRenderer.AddText(Template.stake, rtfHelper.PosFont);
            IRichTextString rtfStake = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();
            return rtfStake;
        }
    }
}