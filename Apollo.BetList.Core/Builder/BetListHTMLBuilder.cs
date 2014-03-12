// ---------------------------------------------------------------------------------
// <copyright file="BetListHTMLBuilder.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Apollo.BetList.Core.Builder.AgentCommBuilder;
    using Apollo.BetList.Core.Builder.AgentShareBuilder;
    using Apollo.BetList.Core.Builder.ChoiceBuilder;
    using Apollo.BetList.Core.Builder.DiscountBuilder;
    using Apollo.BetList.Core.Builder.OddsBuilder;
    using Apollo.BetList.Core.Builder.StakeBuilder;
    using Apollo.BetList.Core.Builder.StatusBuilder;
    using Apollo.BetList.Core.Builder.TimeBuilder;
    using Apollo.BetList.Core.Builder.TransBuilder;
    using Apollo.BetList.Core.Entities;

    /// <summary>
    /// Bet list HTML builder class.
    /// </summary>
    public class BetListHTMLBuilder : IBetListBuilder
    {
        #region Attributes

        private const string ChoiceBuilderNamespace = "Apollo.BetList.Core.Builder.ChoiceBuilder";
        private const string OddsBuilderNamespace = "Apollo.BetList.Core.Builder.OddsBuilder";
        private const string StakeBuilderNamespace = "Apollo.BetList.Core.Builder.StakeBuilder";
        private const string TransBuilderNamespace = "Apollo.BetList.Core.Builder.TransBuilder";
        private const string StatusBuilderNamespace = "Apollo.BetList.Core.Builder.StatusBuilder";
        private const string AgentCommBuilderNamespace = "Apollo.BetList.Core.Builder.AgentCommBuilder";
        private const string AgentShareBuilderNamespace = "Apollo.BetList.Core.Builder.AgentShareBuilder";
        private StringBuilder _content;

        #endregion Attributes

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BetListHTMLBuilder" /> class.
        /// </summary>
        public BetListHTMLBuilder()
        {
            this._content = new StringBuilder();
        }

        #endregion Constructors

        /// <summary>
        /// Gets the bet list.
        /// </summary>
        /// <returns>Content bet list.</returns>
        public string GetBetList()
        {
            return this._content.ToString();
        }

        /// <summary>
        /// Appends the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>Bet List HTML Builder.</returns>
        public BetListHTMLBuilder Append(string content)
        {
            this._content.Append(content);
            return this;
        }

        /// <summary>
        /// Add the time.
        /// </summary>
        /// <param name="ticket">The ticket need to build.</param>
        /// <param name="openTag">The open tag.</param>
        /// <param name="closeTag">The close tag.</param>
        /// <param name="page">The page bet list.</param>
        /// <returns>Bet list HTML builder.</returns>
        public BetListHTMLBuilder AddTime(ITicket ticket, string openTag, string closeTag, BetListPage page)
        {
            string time = BuildTime(ticket, page);
            this._content.Append(string.Join(null, new string[] { openTag, time, closeTag }));

            return this;
        }

        /// <summary>
        /// Build the time.
        /// </summary>
        /// <param name="ticket">The ticket need to build.</param>
        /// <param name="page">The page bet list.</param>
        /// <returns>Time string.</returns>
        public string BuildTime(ITicket ticket, BetListPage page)
        {
            ITime timeBuilder = new Time();
            string time = timeBuilder.Render(ticket, page);

            return time;
        }

        /// <summary>
        /// Add discount.
        /// </summary>
        /// <param name="ticket">The ticket need to build.</param>
        /// <param name="openTag">The open tag.</param>
        /// <param name="closeTag">The close tag.</param>
        /// <param name="page">The page bet list.</param>
        /// <returns>Bet list HTML builder.</returns>
        public BetListHTMLBuilder AddDiscount(ITicket ticket, string openTag, string closeTag, BetListPage page)
        {
            string discount = BuildDiscount(ticket, page);
            this._content.Append(string.Join(null, new string[] { openTag, discount, closeTag }));

            return this;
        }

        /// <summary>
        /// Build discount.
        /// </summary>
        /// <param name="ticket">The ticket need to build.</param>
        /// <param name="page">The page bet list.</param>
        /// <returns>Discount string.</returns>
        public string BuildDiscount(ITicket ticket, BetListPage page)
        {
            IDiscount discountBuilder = new Discount();
            string discount = discountBuilder.Render(ticket, page);

            return discount;
        }

        /// <summary>
        /// Adds the choice.
        /// </summary>
        /// <param name="ticket">The ticket need to build.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="openTag">The open tag.</param>
        /// <param name="closeTag">The close tag.</param>
        /// <param name="isShowScoreMap">Is show score map or not.</param>
        /// <returns>
        /// Bet List HTML Builder.
        /// </returns>
        public BetListHTMLBuilder AddChoice(
            ITicket ticket,
            ITicketHelper ticketHelper,
            List<ITicketData> ticketData,
            string openTag,
            string closeTag,
            bool isShowScoreMap)
        {
            string choice = BuildChoice(ticket, ticketHelper, ticketData, isShowScoreMap);
            this._content.Append(string.Join(null, new string[] { openTag, choice, closeTag }));

            return this;
        }

        /// <summary>
        /// Adds the choice.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="ticketMixParlay">The ticket mix parlay.</param>
        /// <param name="openTag">The open tag.</param>
        /// <param name="closeTag">The close tag.</param>
        /// <param name="isShowScoreMap">If set to <c>true</c> [is show score map].</param>
        /// <returns>Bet List HTML Builder.</returns>
        public BetListHTMLBuilder AddChoice(
           ITicket ticket,
           ITicketHelper ticketHelper,
           List<ITicketData> ticketData,
             List<ITicket> ticketMixParlay,
           string openTag,
           string closeTag,
           bool isShowScoreMap)
        {
            string choice = BuildChoice(ticket, ticketHelper, ticketData, ticketMixParlay, isShowScoreMap);
            this._content.Append(string.Join(null, new string[] { openTag, choice, closeTag }));

            return this;
        }

        /// <summary>
        /// Builds the choice.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="isShowScoreMap">If set to <c>true</c> [is show score map].</param>
        /// <returns>
        /// Choice string.
        /// </returns>
        public string BuildChoice(
            ITicket ticket,
            ITicketHelper ticketHelper,
            List<ITicketData> ticketData,
            bool isShowScoreMap)
        {
            IChoice choiceBuilder = this.CreateChoiceBuilder(ticket.BetTypeId);
            string choice = choiceBuilder.Render(ticket, ticketHelper, ticketData, isShowScoreMap);

            return choice;
        }

        /// <summary>
        /// Builds the choice.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="ticketMixParlay">The ticket mix parlay.</param>
        /// <param name="isShowScoreMap">If set to <c>true</c> [is show score map].</param>
        /// <returns>
        /// Choice string.
        /// </returns>
        public string BuildChoice(
            ITicket ticket,
            ITicketHelper ticketHelper,
            List<ITicketData> ticketData,
            IList<ITicket> ticketMixParlay,
            bool isShowScoreMap)
        {
            IChoice choiceBuilder = this.CreateChoiceBuilder(ticket.BetTypeId);
            string choice = choiceBuilder.Render(ticket, ticketHelper, ticketData, ticketMixParlay, isShowScoreMap);

            return choice;
        }

        /// <summary>
        /// Adds the odd.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="openTag">The open tag.</param>
        /// <param name="closeTag">The close tag.</param>
        /// <param name="funcGetOddsTypeName">Name of the function get odds type.</param>
        /// <returns>Bet List HTML Builder.</returns>
        public BetListHTMLBuilder AddOdds(
            ITicket ticket,
            List<ITicketData> ticketData,
            string openTag,
            string closeTag,
            GetCachePropertyById funcGetOddsTypeName)
        {
            string odds = BuildOdds(ticket, ticketData, funcGetOddsTypeName);
            this._content.Append(string.Join(null, new string[] { openTag, odds, closeTag }));

            return this;
        }

        /// <summary>
        /// Builds the odds.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="funcGetOddsTypeName">Name of the function get odds type.</param>
        /// <returns>Odd string.</returns>
        public string BuildOdds(ITicket ticket, List<ITicketData> ticketData, GetCachePropertyById funcGetOddsTypeName)
        {
            IOdds oddsBuilder = this.CreateOddsBuilder(ticket.BetTypeId);
            string odds = oddsBuilder.Render(ticket, ticketData, funcGetOddsTypeName);

            return odds;
        }

        /// <summary>
        /// Adds the stake.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="openTag">The open tag.</param>
        /// <param name="closeTag">The close tag.</param>
        /// <returns>Bet List HTML Builder.</returns>
        public BetListHTMLBuilder AddStake(ITicket ticket, string openTag, string closeTag)
        {
            string stake = BuildStake(ticket);
            this._content.Append(string.Join(null, new string[] { openTag, stake, closeTag }));

            return this;
        }

        /// <summary>
        /// Builds the stake.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>
        /// Stake string.
        /// </returns>
        public string BuildStake(ITicket ticket)
        {
            IStake stakeBuilder = this.CreateStakeBuilder(ticket.BetTypeId);
            string stake = stakeBuilder.Render(ticket);

            return stake;
        }

        /// <summary>
        /// Adds the trans.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="openTag">The open tag.</param>
        /// <param name="closeTag">The close tag.</param>
        /// <param name="funcGetOddsTypeName">Name of the function get odds type.</param>
        /// <param name="betListPage">The bet list page.</param>
        /// <returns>
        /// Bet List HTML Builder.
        /// </returns>
        public BetListHTMLBuilder AddTrans(
            ITicket ticket,
            ITicketHelper ticketHelper,
            string openTag,
            string closeTag,
            GetCachePropertyById funcGetOddsTypeName,
            BetListPage betListPage)
        {
            string trans = BuildTrans(ticket, ticketHelper, funcGetOddsTypeName, betListPage);
            this._content.Append(string.Join(null, new string[] { openTag, trans, closeTag }));

            return this;
        }

        /// <summary>
        /// Builds the trans.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="funcGetOddsTypeName">Name of the function get odds type.</param>
        /// <param name="betListPage">The bet list page.</param>
        /// <returns>Transaction string.</returns>
        public string BuildTrans(
            ITicket ticket,
            ITicketHelper ticketHelper,
            GetCachePropertyById funcGetOddsTypeName,
            BetListPage betListPage)
        {
            ITrans transBuilder = this.CreateTransBuilder(ticket.BetTypeId);
            string trans = transBuilder.Render(ticket, ticketHelper, funcGetOddsTypeName, betListPage);

            return trans;
        }

        /// <summary>
        /// Adds the status.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data (Status33 Win/Place).</param>
        /// <param name="openTag">The open tag.</param>
        /// <param name="closeTag">The close tag.</param>
        /// <returns>
        /// Bet List HTML Builder.
        /// </returns>
        public BetListHTMLBuilder AddStatus(
            ITicket ticket,
            List<ITicketData> ticketData,
            string openTag,
            string closeTag)
        {
            string status = BuildStatus(ticket, ticketData);
            this._content.Append(string.Join(null, new string[] { openTag, status, closeTag }));

            return this;
        }

        /// <summary>
        /// Builds the status.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data (Status33 Win/Place).</param>
        /// <returns>
        /// Status string.
        /// </returns>
        public string BuildStatus(ITicket ticket, List<ITicketData> ticketData)
        {
            IStatus statusBuilder = this.CreateStatusBuilder(ticket.BetTypeId);
            string status = statusBuilder.Render(ticket, ticketData);

            return status;
        }

        /// <summary>
        /// Add the agent Commission.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="openTag">The open tag.</param>
        /// <param name="closeTag">The close tag.</param>
        /// <returns>Bet List HTML Builder.</returns>
        public BetListHTMLBuilder AddAgentComm(ITicket ticket, string openTag, string closeTag)
        {
            string agentComm = BuildAgentComm(ticket);
            this._content.Append(string.Join(null, new string[] { openTag, agentComm, closeTag }));

            return this;
        }

        /// <summary>
        /// Build agent commission.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>Agent commission string.</returns>
        public string BuildAgentComm(ITicket ticket)
        {
            IAgentComm agentCommBuilder = this.CreateAgentCommBuilder(ticket.BetTypeId);
            string agentComm = agentCommBuilder.Render(ticket);

            return agentComm;
        }

        /// <summary>
        /// Add Agent share.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">List of tickets data.</param>
        /// <param name="openTag">The open tag.</param>
        /// <param name="closeTag">The close tag.</param>
        /// <returns>Bet List HTML Builder.</returns>
        public BetListHTMLBuilder AddAgentShare(ITicket ticket, List<ITicketData> ticketData, string openTag, string closeTag)
        {
            string agentShare = BuildAgentShare(ticket, ticketData);
            this._content.Append(string.Join(null, new string[] { openTag, agentShare, closeTag }));

            return this;
        }

        /// <summary>
        /// Build agent share.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">Ticket data.</param>
        /// <returns>Agent share string.</returns>
        public string BuildAgentShare(ITicket ticket, List<ITicketData> ticketData)
        {
            IAgentShare agentShareBuilder = this.CreateAgentShareBuilder(ticket.BetTypeId);
            string agentShare = agentShareBuilder.Render(ticket, ticketData);

            return agentShare;
        }

        /// <summary>
        /// Creates the choice builder.
        /// </summary>
        /// <param name="bettype">The bet type.</param>
        /// <returns>Choice interface.</returns>
        private IChoice CreateChoiceBuilder(int bettype)
        {
            string[] choiceBuilder = new string[] { ChoiceBuilderNamespace, ".Choice", bettype.ToString() };
            string typeName = string.Join(null, choiceBuilder);

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

        /// <summary>
        /// Creates the odd builder.
        /// </summary>
        /// <param name="bettype">The bet type.</param>
        /// <returns>Odds interface.</returns>
        private IOdds CreateOddsBuilder(int bettype)
        {
            if (bettype <= 1199 && bettype >= 1101)
            {
                // Odds of Live Casino bettypes
                bettype = 1101;
            }

            string[] choiceBuilder = new string[] { OddsBuilderNamespace, ".Odds", bettype.ToString() };
            string typeName = string.Join(null, choiceBuilder);

            Type type = Type.GetType(typeName);

            if (type == null)
            {
                return new Odds1();
            }
            else
            {
                return Activator.CreateInstance(type) as IOdds;
            }
        }

        /// <summary>
        /// Creates the stake builder.
        /// </summary>
        /// <param name="bettype">The bet type.</param>
        /// <returns>Stake interface.</returns>
        private IStake CreateStakeBuilder(int bettype)
        {
            string[] choiceBuilder = new string[] { StakeBuilderNamespace, ".Stake", bettype.ToString() };
            string typeName = string.Join(null, choiceBuilder);

            Type type = Type.GetType(typeName);

            if (type == null)
            {
                return new Stake1();
            }
            else
            {
                return Activator.CreateInstance(type) as IStake;
            }
        }

        /// <summary>
        /// Creates the status builder.
        /// </summary>
        /// <param name="bettype">The bet type.</param>
        /// <returns>Status interface.</returns>
        private IStatus CreateStatusBuilder(int bettype)
        {
            string[] choiceBuilder = new string[] { StatusBuilderNamespace, ".Status", bettype.ToString() };
            string typeName = string.Join(null, choiceBuilder);

            Type type = Type.GetType(typeName);

            if (type == null)
            {
                return new Status1();
            }
            else
            {
                return Activator.CreateInstance(type) as IStatus;
            }
        }

        /// <summary>
        /// Creates the trans builder.
        /// </summary>
        /// <param name="bettype">The bet type.</param>
        /// <returns>
        /// Transaction interface.
        /// </returns>
        private ITrans CreateTransBuilder(int bettype)
        {
            string[] choiceBuilder = new string[] { TransBuilderNamespace, ".Trans", bettype.ToString() };
            string typeName = string.Join(null, choiceBuilder);

            Type type = Type.GetType(typeName);

            if (type == null)
            {
                return new Trans();
            }
            else
            {
                return Activator.CreateInstance(type) as ITrans;
            }
        }

        /// <summary>
        /// Creates the agent PT builder.
        /// </summary>
        /// <param name="bettype">The bet type.</param>
        /// <returns>
        /// Agent share interface.
        /// </returns>
        private IAgentShare CreateAgentShareBuilder(int bettype)
        {
            string[] agentShareBuilder = new string[] { AgentShareBuilderNamespace, ".AgentShare", bettype.ToString() };
            string typeName = string.Join(null, agentShareBuilder);

            Type type = Type.GetType(typeName);

            if (type == null)
            {
                return new AgentShare1();
            }
            else
            {
                return Activator.CreateInstance(type) as IAgentShare;
            }
        }

        /// <summary>
        /// Creates the agent Commission builder.
        /// </summary>
        /// <param name="bettype">The bet type.</param>
        /// <returns>
        /// Agent commission interface.
        /// </returns>
        private IAgentComm CreateAgentCommBuilder(int bettype)
        {
            string[] agentCommBuilder = new string[] { AgentCommBuilderNamespace, ".AgentComm", bettype.ToString() };
            string typeName = string.Join(null, agentCommBuilder);

            Type type = Type.GetType(typeName);

            if (type == null)
            {
                return new AgentComm1();
            }
            else
            {
                return Activator.CreateInstance(type) as IAgentComm;
            }
        }
    }
}