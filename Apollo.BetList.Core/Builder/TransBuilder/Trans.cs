// --------------------------------------------------------------------------
// <copyright file="Trans.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.11.5">Created</change>
// </history>

namespace Apollo.BetList.Core.Builder.TransBuilder
{
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Templates;
    using Apollo.BetList.Core.Utils;
    using NPOI.SS.UserModel;
    using NPOI.HSSF.Util;

    /// <summary>
    /// Transaction class.
    /// </summary>
    public class Trans : ITrans
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Trans" /> class.
        /// </summary>
        public Trans()
        {
            this.Template = new Trans_Template();
        }

        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        public Trans_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="funcGetOddsTypeName">Name of the function get odds type.</param>
        /// <param name="betListPage">The bet list page.</param>
        /// <returns>Transaction Template.</returns>
        public virtual Trans_Template Render(ITicket ticket, ITicketHelper ticketHelper, GetCachePropertyById funcGetOddsTypeName, BetListPage betListPage)
        {
            this.Template.oddsName = string.Empty;
            this.Template.transId = ticket.TransId.ToString();
            this.Template.bettypeName = ticketHelper.GetBetTypeNameById(ticket.BetTypeId);
            this.Template.betfrom = Common.BetFromFormat(ticket.BetFrom);

            switch (betListPage)
            {
                case BetListPage.Wager:
                case BetListPage.Forecast:
                case BetListPage.AccountInfo:
                case BetListPage.Cancel:
                case BetListPage.Transaction:
                case BetListPage.Report:
                case BetListPage.Outstanding:
                    this.Template.sportName = string.Format("<br/>{0}&nbsp;", ticketHelper.GetSportNameById(ticket.SportTypeId));
                    break;

                default:
                    this.Template.sportName = string.Empty;
                    break;
            }

            return Template;
        }

        /// <summary>
        /// Renders the RTF.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="page">The bet list page.</param>
        /// <param name="rtfHelper">The RTF helper.</param>
        /// <param name="funcGetOddsTypeName">Name of the function get odds type.</param>
        /// <returns>
        /// Rich Text String.
        /// </returns>
        public IRichTextString RenderRTF(ITicket ticket, ITicketHelper ticketHelper, BetListPage page, RTFHelper rtfHelper, GetCachePropertyById funcGetOddsTypeName)
        {
            Render(ticket, ticketHelper, funcGetOddsTypeName, page);

            rtfHelper.RTFRenderer.AddText(string.Join(string.Empty, new string[] { Template.transId, " \n " }));

            string sportName = Template.sportName.Replace("<br/>", string.Empty).Replace("&nbsp;", string.Empty);
            rtfHelper.RTFRenderer.AddText(string.Join(string.Empty, new string[] { sportName, " ", Template.bettypeName }));

            IFont redFont = rtfHelper.Font(HSSFColor.Red.Index, FontBoldWeight.Normal, false);
            string betFrom = Common.BetFromFormat(ticket.BetFrom);
            rtfHelper.RTFRenderer.AddText(string.Join(string.Empty, new string[] { "\n", betFrom }), redFont);

            IRichTextString rtfTrans = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();

            return rtfTrans;
        }
    }
}