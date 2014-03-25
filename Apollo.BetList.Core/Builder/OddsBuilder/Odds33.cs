// ---------------------------------------------------------------------------------
// <copyright file="Odds33.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
//     <change who="Marc.Bui" date="2013.01.25">Update export excel</change>
//     <change who="Marc.Bui" date="2013.01.25">Change Odds display</change>
//     <change who="Marc.Bui" date="2013.02.27">Add GetReferenceData for Odds 1233</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Builder.OddsBuilder
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Apollo.Age.Resources;
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Templates;
    using Apollo.BetList.Core.Utils;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Win Place.
    /// </summary>
    public class Odds33 : IOdds
    {
        private int _winBetId;
        private int _placeBetId;

        /// <summary>
        /// Initializes a new instance of the <see cref="Odds33" /> class.
        /// </summary>
        public Odds33()
        {
            this.Template = new Odds_Template();
            WinBetId = 31;
            PlaceBetId = 32;
        }

        public int WinBetId
        {
            get
            {
                return _winBetId;
            }

            set
            {
                _winBetId = value;
            }
        }

        public int PlaceBetId
        {
            get
            {
                return _placeBetId;
            }

            set
            {
                _placeBetId = value;
            }
        }

        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        public Odds_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="funcGetOddsTypeName">Name of the function get odds type.</param>
        /// <returns>Odds Template.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        public Odds_Template Render(
            ITicket ticket,
            List<ITicketData> ticketData,
            GetCachePropertyById funcGetOddsTypeName)
        {
            if (ticketData != null && ticketData.Count > 0)
            {
                string odds_win = string.Empty;
                string odds_place = string.Empty;
                string oddsWinMarkup, oddsPlaceMarkup;

                List<ITicketData> foundTicketData = this.GetReferenceData(ticket, ticketData);

                if (foundTicketData != null && foundTicketData.Count > 1)
                {
                    ITicketData winTicket = foundTicketData.Find(item => item.BetTypeId == WinBetId);
                    ITicketData placeTicket = foundTicketData.Find(item => item.BetTypeId == PlaceBetId);

                    string oddsWin, oddsPlace;

                    string winStatus = winTicket.Status.ToLower();
                    string placeStatus = placeTicket.Status.ToLower();

                    if (!winStatus.Equals(BetStatus.Won) && !winStatus.Equals(BetStatus.Lose))
                    {
                        oddsWin = "-";
                    }
                    else
                    {
                        oddsWin = Formatter.FormatNumber3(winTicket.Odds);
                    }

                    if (!placeStatus.Equals(BetStatus.Won) && !placeStatus.Equals(BetStatus.Lose))
                    {
                        oddsPlace = "-";
                    }
                    else
                    {
                        oddsPlace = Formatter.FormatNumber3(placeTicket.Odds);
                    }

                    oddsWinMarkup = string.Join(null, new string[] { BetList.win, ": ", "<b>", oddsWin, "</b>" });
                    oddsPlaceMarkup = string.Join(null, new string[] { BetList.place, ": ", "<b>", oddsPlace, "</b>" });

                    string status = ticket.Status.ToLower();

                    if (status.Equals(BetStatus.Won) || status.Equals(BetStatus.Lose))
                    {
                        odds_win = string.Join(null, "<span class='blue' style='font-weight:normal;'>", oddsWinMarkup, "</span>");
                        odds_place = string.Join(null, "<span class='' style='font-weight:normal;'>", oddsPlaceMarkup, "</span>");

                        this.Template.odds = string.Join(null, odds_win, "<br>", odds_place);
                    }
                    else
                    {
                        this.Template.odds = "-";
                    }
                }
                else
                {
                    this.Template.odds = "-";
                }
            }
            else
            {
                this.Template.odds = "-";
            }

            this.Template.oddsType = funcGetOddsTypeName(ConvertByBetType.OddsType(ticket.BetTypeId, ticket.OddsType));

            return this.Template;
        }

        /// <summary>
        /// Renders the RTF.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="funcGetOddsTypeName">Name of the function get odds type.</param>
        /// <param name="rtfHelper">The RTF helper.</param>
        /// <returns>
        /// Rich Text String interface.
        /// </returns>
        public IRichTextString RenderRTF(ITicket ticket, List<ITicketData> ticketData, GetCachePropertyById funcGetOddsTypeName, RTFHelper rtfHelper)
        {
            Render(ticket, ticketData, funcGetOddsTypeName);
            var rtfOdds = BuildRTFOdds(rtfHelper);

            return rtfOdds;
        }

        /// <summary>
        /// Gets the reference data.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <returns>List of tickets.</returns>
        protected virtual List<ITicketData> GetReferenceData(ITicket ticket, List<ITicketData> ticketData)
        {
            return ticketData.FindAll(item => item.RefNo.Equals(ticket.TransId.ToString()));
        }

        /// <summary>
        /// Builds the RTF odds.
        /// </summary>
        /// <param name="rtfHelper">The RTF helper.</param>
        /// <returns>Rich text interface.</returns>
        protected virtual IRichTextString BuildRTFOdds(RTFHelper rtfHelper)
        {
            if (this.Template.odds == "-")
            {
                return new HSSFRichTextString("-");
            }

            int breakPos = this.Template.odds.IndexOf("<br>");

            string odds_WinString = this.Template.odds.Substring(0, breakPos);
            string odds_PlaceString = this.Template.odds.Replace(odds_WinString, string.Empty);

            string odds_WinVal = ExtractStringBetweenTag("b", odds_WinString);
            string odds_PlaceVal = ExtractStringBetweenTag("b", odds_PlaceString);

            var rtfNumberRenderer = new RTFNumber(
                rtfHelper.RTFRenderer, 
                rtfHelper.Font(HSSFColor.Black.Index, FontBoldWeight.Normal), 
                rtfHelper.Font(HSSFColor.Red.Index, FontBoldWeight.Normal));

            // Add Win
            rtfHelper.RTFRenderer.AddText(string.Join(null, new string[] { BetList.win, ": " }), rtfHelper.Font(HSSFColor.Black.Index, FontBoldWeight.Normal));
            rtfNumberRenderer.Render(odds_WinVal);

            // Add Place
            rtfHelper.RTFRenderer.AddText(string.Join(null, new string[] { "\n", BetList.place, ": " }), rtfHelper.Font(HSSFColor.Black.Index, FontBoldWeight.Normal));
            rtfNumberRenderer.Render(odds_PlaceVal);

            var rtfOdds = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();

            return rtfOdds;
        }

        /// <summary>
        /// Extract string value between tags from a source string.
        /// </summary>
        /// <param name="tag">Input tag name.</param>
        /// <param name="source">Source string.</param>
        /// <returns>Extract string value between tags.</returns>
        private string ExtractStringBetweenTag(string tag, string source)
        {
            string startTag = string.Join(null, new string[] { "<", tag, ">" });
            string endTag = string.Join(null, new string[] { "</", tag, ">" });

            int startIndex = source.IndexOf(startTag) + startTag.Length;
            int endIndex = source.IndexOf(endTag, startIndex);

            return source.Substring(startIndex, endIndex - startIndex);
        }
    }
}