// ---------------------------------------------------------------------------------
// <copyright file="Choice29.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Builder.ChoiceBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using Apollo.Age.Resources;
    using Apollo.BetList.Core.Entities;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Combination Mix Parlay.
    /// </summary>
    public class Choice29 : Choice1
    {
        private const string ChoiceBuilderNameSpace = "Apollo.BetList.Core.Builder.ChoiceBuilder";

        /// <summary>
        /// The combination offer name.
        /// </summary>
        private static Dictionary<string, string> _combinationOfferName = new Dictionary<string, string>
        {
            { "110", "Trixie" },
            { "1110", "Yankee" },
            { "11110", "Canadian" },
            { "111110", "Heinz" },
            { "1111110", "Super Heinz" },
            { "11111110", "Goliath" }
        };

        /// <summary>
        /// Renders the RTF.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="ticketMixParlay">The ticket mix parlay.</param>
        /// <param name="isShowScoreMap">If set to <c>true</c> [is show score map].</param>
        /// <param name="rtfHelper">The RTF helper.</param>
        /// <returns>
        /// Choice Template.
        /// </returns>
        public override List<IRichTextString> RenderRTF(
            ITicket ticket,
            ITicketHelper ticketHelper,
            List<ITicketData> ticketData,
            IList<ITicket> ticketMixParlay,
            bool isShowScoreMap,
            RTFHelper rtfHelper)
        {
            string sportName = string.Join(null, new string[] { BetList.lblSystemParlay, " - ", GetCombinationOfferName(ticket.BetTeam), "\n\n" });

            rtfHelper.RTFRenderer.AddText(sportName, rtfHelper.Font(HSSFColor.Blue.Index, FontBoldWeight.Normal));

            if (ticketMixParlay != null)
            {
                IList<ITicket> parlayData = ticketMixParlay.Where(x => x.RefNo == ticket.RefNo).ToList();

                foreach (ITicket tkt in parlayData)
                {
                    IChoice choiceBuilder = this.CreateChoiceBuilder(tkt.BetTypeId);
                    this.Template = choiceBuilder.Render(tkt, ticketHelper, null, isShowScoreMap);

                    if (tkt.StatusId.ToString() == BetStatusId.Void)
                    {
                        rtfHelper = this.CreateRTFHelper(tkt, rtfHelper, ticketHelper, true, true);
                    }
                    else
                    {
                        rtfHelper = this.CreateRTFHelper(tkt, rtfHelper, ticketHelper, true);
                    }

                    if (tkt.TransId != parlayData.Last().TransId)
                    {
                        rtfHelper.RTFRenderer.AddText("\n--------------------------------------------\n");
                    }
                }
            }

            var choice = rtfHelper.RTFRenderer.Render();

            rtfHelper.RTFRenderer.Clear();

            return new List<IRichTextString>() { choice };
        }

        /// <summary>
        /// Builds the choice.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="ticketMixParlay">The ticket mix parlay.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Reviewed. Suppression is OK here.")]
        protected override void BuildChoice(
            ITicket ticket,
            ITicketHelper ticketHelper,
            List<ITicketData> ticketData,
            IList<ITicket> ticketMixParlay)
        {
            this.Template.handicap = BetList.vs;

            string betTypeName = ticketHelper.GetBetTypeNameById(ticket.BetTypeId);
            string mixParlay = string.Empty;
            string custid = HttpContext.Current.Request["custid"] ?? ticket.CustId.ToString();

            string systemParlay = string.Join(
                null,
                "<div class='combinationLink'>",
                "<a name='mixParlayDetail' data-transId='" + ticket.TransId.ToString() + "' href='javascript:void(0);'>",
                betTypeName,
                "</a>",
                " - ",
                "<span>",
                GetCombinationOfferName(ticket.BetTeam),
                "</span><br/>",
                "<div id='" + ticket.TransId.ToString() + "'>",
                RenderMixParlayDetail(ticket, ticketMixParlay, ticketHelper),
                "</div>",
                "<div class='detail' data-refno='" + ticket.RefNo + "' data-winlostDate='" + ticket.WinlostDate.ToString() + "' data-transid='" + ticket.TransId.ToString() + "' data-betid='" + ticket.BetId.ToString() + "' data-custid='" + custid + "'>",
                BetList.details,
                "</div>",
                "</div>");

            this.Template.SetValue(systemParlay);
        }

        /// <summary>
        /// Gets the name of the combination offer.
        /// </summary>
        /// <param name="betteam">The bet team.</param>
        /// <returns>Combination offer name.</returns>
        private static string GetCombinationOfferName(string betteam)
        {
            betteam = betteam.Trim();
            return _combinationOfferName.ContainsKey(betteam) ? _combinationOfferName[betteam] : string.Empty;
        }

        /// <summary>
        /// Renders the mix parlay detail.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketMixParlay">The ticket mix parlay.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <returns>Return system string.</returns>
        private string RenderMixParlayDetail(ITicket ticket, IList<ITicket> ticketMixParlay, ITicketHelper ticketHelper)
        {
            var line = "<div class='line'></div>";
            var result = string.Empty;
            var tickets = ticketMixParlay.Where(x => x.RefNo == ticket.RefNo);

            foreach (var item in tickets)
            {
                var builder = new BetListHTMLBuilder();

                if (tickets.LastOrDefault().TransId == item.TransId)
                {
                    line = string.Empty;
                }

                if (item.StatusId.ToString() == BetStatusId.Void)
                {
                    builder.AddChoice(item, ticketHelper, null, "<div class='strike_through'>", "</div>" + line, false);
                }
                else
                {
                    builder.AddChoice(item, ticketHelper, null, string.Empty, line, false);
                }

                result += builder.GetBetList().Replace(
                    "<div class=\"league\">",
                    string.Format("<div class=\"league\"> <span class=\"red-color\"> {0} </span> ", ticketHelper.GetSportNameById(item.SportTypeId)));
            }

            return result;
        }

        /// <summary>
        /// Creates the choice builder.
        /// </summary>
        /// <param name="bettype">The bet type.</param>
        /// <returns>Return choice.</returns>
        private IChoice CreateChoiceBuilder(int bettype)
        {
            string[] buildChoice = new string[] { ChoiceBuilderNameSpace, ".Choice", bettype.ToString() };
            string typeName = string.Join(null, buildChoice);

            Type type = Type.GetType(typeName);

            if (type == null)
            {
                return new Choice1();
            }
            else
            {
                return Activator.CreateInstance(type) as IChoice;
            }
        }
    }
}