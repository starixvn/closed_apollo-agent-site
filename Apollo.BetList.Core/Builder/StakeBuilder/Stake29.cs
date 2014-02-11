// ---------------------------------------------------------------------------------
// <copyright file="Stake29.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Builder.StakeBuilder
{
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Templates;
    using Apollo.BetList.Core.Utils;
    using NPOI.SS.UserModel;

    /// <summary>
    /// The Stake 29.
    /// </summary>
    public class Stake29 : IStake
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Stake29" /> class.
        /// </summary>
        public Stake29()
        {
            this.Template = new Stake_Template();
        }

        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        public Stake_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>Stake Template.</returns>
        public Stake_Template Render(ITicket ticket)
        {
            this.Template.stake = ConvertByBetType.Stake(ticket.Stake);

            return Template;
        }

        /// <summary>
        /// Renders the RTF.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="rtfHelper">The RTF helper.</param>
        /// <returns>
        /// Rich Text String.
        /// </returns>
        public IRichTextString RenderRTF(ITicket ticket, RTFHelper rtfHelper)
        {
            Render(ticket);
            Template.stake = Template.stake.Replace("<br/>&nbsp;", "\n");

            rtfHelper.RTFRenderer.AddText(Template.stake);
            IRichTextString rtfStake = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();
            return rtfStake;
        }
    }
}