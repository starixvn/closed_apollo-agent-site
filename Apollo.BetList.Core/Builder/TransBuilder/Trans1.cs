// ---------------------------------------------------------------------------------
// <copyright file="Trans1.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.24">Create</change>
//     <change who="Marc.Bui" date="2013.01.24">Remove BetSlip from Trans.Time</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Builder.TransBuilder
{
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Templates;
    using Apollo.BetList.Core.Utils;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Transaction 1.
    /// </summary>
    public class Trans1 : ITrans
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Trans1" /> class.
        /// </summary>
        public Trans1()
        {
            this.Template = new Trans_Template();
        }

        public Trans_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="funcGetOddsTypeName">Name of the function get odds type.</param>
        /// <param name="betListPage">The bet list page.</param>
        /// <returns>Transaction template.</returns>
        public virtual Trans_Template Render(ITicket ticket, ITicketHelper ticketHelper, GetCachePropertyById funcGetOddsTypeName, BetListPage betListPage)
        {
            object oddsType = new object();

            if (string.IsNullOrEmpty(ticket.OddsType))
            {
                oddsType = 0;
            }
            else
            {
                oddsType = ticket.OddsType;
            }

            this.Template.oddsName = funcGetOddsTypeName(ConvertByBetType.OddsType(ticket.BetTypeId, oddsType));
            this.Template.transId = ticket.TransId.ToString();
            this.Template.bettypeName = ticketHelper.GetBetTypeNameById(ticket.BetTypeId);

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


        public IRichTextString RenderRTF(ITicket ticket, ITicketHelper ticketHelper, BetListPage page, RTFHelper rtfHelper, GetCachePropertyById funcGetOddsTypeName)
        {
            Render(ticket, ticketHelper, funcGetOddsTypeName, page);

            rtfHelper.RTFRenderer.AddText(string.Join("", new string[] { Template.transId, " \n " }));

            rtfHelper.RTFRenderer.AddText(string.Join("", new string[] { Template.sportName.Replace("<br/>", "").Replace("&nbsp;", ""), " ", Template.bettypeName }));

            IRichTextString rtfTrans = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();

            return rtfTrans;
        }
    }
}