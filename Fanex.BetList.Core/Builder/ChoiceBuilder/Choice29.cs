// ---------------------------------------------------------------------------------
// <copyright file="Choice29.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Builder.ChoiceBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    using Fanex.BetList.Core.Entities;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Combination Mix Parlay.
    /// </summary>
    public class Choice29 : Choice1
    {
        /// <summary>
        /// The _combination offer name.
        /// </summary>
        private static Dictionary<string, string> _combinationOfferName = new Dictionary<string, string>
        {
            {"110", "Trixie"},
            {"1110", "Yankee"},
            {"11110", "Canadian"},
            {"111110", "Heinz"},
            {"1111110", "Super Heinz"},
            {"11111110", "Goliath"}
        };

        /// <summary>
        /// Builds the choice.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        protected override void BuildChoice(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, BetListPage betListPage)
        {
            this.Template.handicap = Apollo.Age.Resources.BetList.vs;

            string betTypeName = ticketHelper.GetBetTypeNameById(ticket.BetTypeId);
            string mixParlay = string.Empty, custid = string.Empty;

            if (HttpContext.Current.Request["custid"] != null)
            {
                custid = HttpContext.Current.Request["custid"];
            }
            else
            {
                custid = ticket.CustId.ToString();
            }

            mixParlay = String.Join(
                                        null,
                                        new string[]
                                        {
                                            "<div class=\"combinationLink\"><a id=\"hidden0\" href=\"javascript:showMP(&quot;",
                                            ticket.TransId.ToString(),
                                            "&quot;);\">",
                                            betTypeName,
                                            "</a>",
                                            " - ",
                                            "<span>",
                                            GetCombinationOfferName(ticket.BetTeam),
                                            "</span>",
                                            "</div>",
                                            "<div id=\"divEvent_",
                                            ticket.TransId.ToString(),
                                            "\" style=\"display: none\" ></div>",
                                            "<div class=\"detail\" onclick=\"showCombMPDetail(&quot;",
                                            ticket.RefNo,
                                            "&quot;",
                                            ",&quot;",
                                            ticket.WinlostDate.ToString(),
                                            "&quot;",
                                            ",&quot;",
                                            ticket.TransId.ToString(),
                                            "&quot;",
                                            ",&quot;",
                                            ticket.BetId.ToString(),
                                            "&quot;",
                                            ",&quot;",
                                            custid,
                                            "&quot;, this);\">",
                                            Apollo.Age.Resources.BetList.details,
                                            "</div>"
                                        });

            this.Template.SetValue(mixParlay);
        }

        /// <summary>
        /// Gets the name of the combination offer.
        /// </summary>
        /// <param name="betteam">The betteam.</param>
        /// <returns>System.String.</returns>
        private static string GetCombinationOfferName(string betteam)
        {
            betteam = betteam.Trim();
            return _combinationOfferName.ContainsKey(betteam) ? _combinationOfferName[betteam] : string.Empty;
        }

        public override List<IRichTextString> RenderRTF(
            ITicket ticket, 
            ITicketHelper ticketHelper, 
            List<ITicketData> ticketData, 
            bool isShowScoreMap, 
            Entities.RTFHelper rtfHelper)
        {
            List<IRichTextString> lstRTFChoices = new List<IRichTextString>(1);
            IRichTextString rtfChoice = new HSSFRichTextString(Apollo.Age.Resources.BetList.lblSystemParlay);
            lstRTFChoices.Add(rtfChoice);
            return lstRTFChoices;
        }
    }
}