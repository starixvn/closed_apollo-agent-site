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

namespace Fanex.BetList.Core.Builder.OddsBuilder
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Templates;
    using Fanex.BetList.Core.Utils;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Win/Place
    /// </summary>
    public class Odds33 : IOdds
    {
        public Odds_Template Template { get; set; }

        protected int winBetId, placeBetId;

        /// <summary>
        /// Initializes a new instance of the <see cref="Odds33" /> class.
        /// </summary>
        public Odds33()
        {
            this.Template = new Odds_Template();
            winBetId = 31;
            placeBetId = 32;
        }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="FuncGetOddsTypeName">Name of the func get odds type.</param>
        /// <returns>Odds_Template.</returns>
        public Odds_Template Render(ITicket ticket, List<ITicketData> ticketData, GetCachePropertyById FuncGetOddsTypeName)
        {
            if (ticketData != null && ticketData.Count > 0)
            {
                #region declare params

                string odds_win = string.Empty;
                string odds_place = string.Empty;
                string oddsWinMarkup, oddsPlaceMarkup;

                #endregion declare params

                List<ITicketData> foundTicketData = this.GetReferenceData(ticket, ticketData);

                if (foundTicketData != null && foundTicketData.Count > 1)
                {
                    #region Win/Place

                    ITicketData winTicket = foundTicketData.Find(item => item.BetTypeId == winBetId);
                    ITicketData placeTicket = foundTicketData.Find(item => item.BetTypeId == placeBetId);

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

                    oddsWinMarkup = string.Join(null, new string[] { Apollo.Age.Resources.BetList.win, ": ", "<b>", oddsWin, "</b>" });
                    oddsPlaceMarkup = string.Join(null, new string[] { Apollo.Age.Resources.BetList.place, ": ", "<b>", oddsPlace, "</b>" });

                    #endregion Win/Place

                    string status = ticket.Status.ToLower();

                    if (status.Equals(BetStatus.Won) || status.Equals(BetStatus.Lose))
                    {
                        odds_win = string.Join(
                                                    null,
                                                    new string[]
                                                    {
                                                        "<span class='blue' style='font-weight:normal;'>",
                                                        oddsWinMarkup,
                                                        "</span>"
                                                    });

                        odds_place = string.Join(
                                                    null,
                                                    new string[]
                                                    {
                                                        "<span class='' style='font-weight:normal;'>",
                                                        oddsPlaceMarkup,
                                                        "</span>"
                                                    });

                        this.Template.odds = string.Join(
                                                        null,
                                                        new string[]
                                                        {
                                                            odds_win,
                                                            "<br>",
                                                            odds_place
                                                        });
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

            this.Template.oddsType = FuncGetOddsTypeName(ConvertByBetType.OddsType(ticket.BetTypeId, ticket.OddsType));

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

        /// <summary>
        /// Sets the odds.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="odds_win">The odds_win.</param>
        /// <param name="odds_place">The odds_place.</param>
        private void SetOdds(ITicket row, ref string odds_win, ref string odds_place)
        {
            string status = row.Status.ToString().ToLower();
            if (status.Equals(BetStatus.Won) || status.Equals(BetStatus.Lose))
            {
                odds_win = "<span class='blue' style='font-weight:normal;'>" + odds_win + "</span>";
                odds_place = "<span class='' style='font-weight:normal;'>" + odds_place + "</span>";
                this.Template.odds = odds_win + "<br>" + odds_place;
            }
            else
            {
                this.Template.odds = "-";
            }
        }

        /// <summary>
        /// Gets the place.
        /// </summary>
        /// <param name="odds_place">The odds_place.</param>
        /// <param name="status_place">The status_place.</param>
        /// <param name="foundRows">The found rows.</param>
        /// <param name="i">The index.</param>
        private static void GetPlace(ref string odds_place, ref string status_place, DataRow[] foundRows, int i)
        {
            switch ((foundRows[i]["status"]).ToString().ToLower())
            {
                case BetStatus.Won:
                    odds_place += string.Join(null, new string[] {Apollo.Age.Resources.BetList.place,
                                                                          ": ",
                                                                          "<b>",
                                                                          Formatter.FormatNumber20(foundRows[i]["odds"], null),
                                                                          "</b>"});

                    status_place += string.Join(null, new string[] {Apollo.Age.Resources.BetList.place,
                                                                          ": ",
                                                                          "<b>",
                                                                          Apollo.Age.Resources.BetList.won,
                                                                          "</b>"});
                    break;
                default:
                    odds_place += string.Join(null, new string[] {Apollo.Age.Resources.BetList.place,
                                                                          ": ",
                                                                          "<b>",
                                                                          "0.00",
                                                                          "</b>"});

                    status_place += string.Join(null, new string[] {Apollo.Age.Resources.BetList.place,
                                                                          ": ",
                                                                          "<b>",
                                                                          Apollo.Age.Resources.BetList.lose,
                                                                          "</b>"});
                    break;
            }
        }

        /// <summary>
        /// Gets the win.
        /// </summary>
        /// <param name="odds_win">The odds_win.</param>
        /// <param name="status_win">The status_win.</param>
        /// <param name="foundRows">The found rows.</param>
        /// <param name="i">The index.</param>
        private static void GetWin(ref string odds_win, ref string status_win, DataRow[] foundRows, int i)
        {
            switch ((foundRows[i]["status"]).ToString().ToLower())
            {
                case BetStatus.Won:
                    odds_win += string.Join(null, new string[] { Apollo.Age.Resources.BetList.win, ": ", "<b>", Formatter.FormatNumber20(foundRows[i]["odds"], null), "</b>" });

                    status_win += string.Join(null, new string[] { Apollo.Age.Resources.BetList.win, ": ", "<b>", Apollo.Age.Resources.BetList.won, "</b>" });

                    break;
                default:
                    odds_win += string.Join(null, new string[]{Apollo.Age.Resources.BetList.win,
                                                                          ": ",
                                                                          "<b>",
                                                                          "0.00",
                                                                          "</b>"});

                    status_win += string.Join(null, new string[]{Apollo.Age.Resources.BetList.win,
                                                                          ": ",
                                                                          "<b>",
                                                                          Apollo.Age.Resources.BetList.lose,
                                                                          "</b>"});

                    break;
            }
        }

        /// <summary>
        /// Renders the RTF.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="FuncGetOddsTypeName">Name of the func get odds type.</param>
        /// <param name="rtfHelper">The RTF helper.</param>
        /// <returns>
        /// IRichTextString
        /// </returns>
        public IRichTextString RenderRTF(ITicket ticket, List<ITicketData> ticketData, GetCachePropertyById FuncGetOddsTypeName, RTFHelper rtfHelper)
        {
            Render(ticket, ticketData, FuncGetOddsTypeName);
            var rtfOdds = BuildRTFOdds(rtfHelper);

            return rtfOdds;
        }

        /// <summary>
        /// Builds the RTF odds.
        /// </summary>
        /// <param name="rtfHelper">The RTF helper.</param>
        /// <returns>IRichTextString</returns>
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

            var rtfNumberRenderer = new RTFNumber(rtfHelper.RTFRenderer, rtfHelper.PosFont, rtfHelper.NegFont);

            //Add Win
            rtfHelper.RTFRenderer.AddText(string.Join(null, new string[] { Apollo.Age.Resources.BetList.win, ": " }), rtfHelper.PosFont);
            rtfNumberRenderer.Render(odds_WinVal);

            //Add Place
            rtfHelper.RTFRenderer.AddText(string.Join(null, new string[] { "\n", Apollo.Age.Resources.BetList.place, ": " }), rtfHelper.PosFont);
            rtfNumberRenderer.Render(odds_PlaceVal);

            var rtfOdds = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();
            return rtfOdds;
        }

        /// <summary>
        /// Extract string value between tags from a source string
        /// </summary>
        /// <param name="tag">tag name</param>
        /// <param name="source">source string</param>
        /// <returns></returns>
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