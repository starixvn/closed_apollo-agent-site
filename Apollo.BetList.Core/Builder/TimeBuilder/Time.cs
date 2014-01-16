// ---------------------------------------------------------------------------------
// <copyright file="Time.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Kaden.Trinh" date="2013.06.26">Create</change>
// </history>
// ---------------------------------------------------------------------------------
namespace Apollo.BetList.Core.Builder.TimeBuilder
{
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Templates;
    using Apollo.BetList.Core.Utils;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Time class.
    /// </summary>
    public class Time : ITime
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Time" /> class.
        /// </summary>
        public Time()
        {
            this.Template = new Time_Template();
        }

        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        public Time_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="page">Link from page.</param>
        /// <returns>Time Template.</returns>
        public virtual Time_Template Render(ITicket ticket, BetListPage page)
        {
            switch (page)
            {
                case BetListPage.Wager:
                case BetListPage.Forecast:
                    Template.transDate = Formatter.FormatDateTime22(ticket.TransDate);
                    break;

                case BetListPage.Cancel:
                case BetListPage.Transaction:
                case BetListPage.AccountInfo:
                case BetListPage.Report:
                case BetListPage.Outstanding:
                    Template.transDate = Formatter.FullDateFormat(ticket.TransDate);
                    break;

                default:
                    Template.transDate = Formatter.FullDateFormat(ticket.TransDate);
                    break;
            }

            return Template;
        }

        /// <summary>
        /// Renders the RTF.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="page">The page.</param>
        /// <param name="rtfHelper"></param>
        /// <returns>
        /// Time Template.
        /// </returns>
        public IRichTextString RenderRTF(ITicket ticket, BetListPage page, RTFHelper rtfHelper)
        {
            Render(ticket, page);
            Template.transDate = Template.transDate.Replace(" ", "\n");
            rtfHelper.RTFRenderer.AddText(Template.transDate);
            IRichTextString rtfTime = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();

            return rtfTime;
        }
    }
}