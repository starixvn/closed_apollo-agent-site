// ---------------------------------------------------------------------------------
// <copyright file="BetListHTMLBuilder.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Fanex.BetList.Core.Builder.ChoiceBuilder;
    using Fanex.BetList.Core.Builder.OddsBuilder;
    using Fanex.BetList.Core.Builder.StakeBuilder;
    using Fanex.BetList.Core.Builder.StatusBuilder;
    using Fanex.BetList.Core.Builder.TransBuilder;
    using Fanex.BetList.Core.Entities;

    public class BetListHTMLBuilder : IBetListBuilder
    {
        #region Attributes

        private StringBuilder _content;
        private const string CHOICE_BUILDER_NAMESPACE = "Fanex.BetList.Core.Builder.ChoiceBuilder";
        private const string ODDS_BUILDER_NAMESPACE = "Fanex.BetList.Core.Builder.OddsBuilder";
        private const string STAKE_BUILDER_NAMESPACE = "Fanex.BetList.Core.Builder.StakeBuilder";
        private const string TRANS_BUILDER_NAMESPACE = "Fanex.BetList.Core.Builder.TransBuilder";
        private const string STATUS_BUILDER_NAMESPACE = "Fanex.BetList.Core.Builder.StatusBuilder";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BetListHTMLBuilder" /> class.
        /// </summary>
        public BetListHTMLBuilder()
        {
            this._content = new StringBuilder();
        }

        #endregion

        /// <summary>
        /// Gets the bet list.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetBetList()
        {
            return this._content.ToString();
        }

        /// <summary>
        /// Appends the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        public BetListHTMLBuilder Append(string content)
        {
            this._content.Append(content);
            return this;
        }

        /// <summary>
        /// Adds the choice.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="openTag">The open tag.</param>
        /// <param name="closeTag">The close tag.</param>
        /// <param name="isShowScoreMap">if set to <c>true</c> [is show score map].</param>
        /// <returns>BetListHTMLBuilder.</returns>
        public BetListHTMLBuilder AddChoice(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, string openTag, string closeTag, bool isShowScoreMap)
        {
            string choice = BuildChoice(ticket, ticketHelper, ticketData, isShowScoreMap);
            this._content.Append(string.Join(null, new string[] { openTag, choice, closeTag }));
            return this;
        }

        /// <summary>
        /// Builds the choice.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="isShowScoreMap">if set to <c>true</c> [is show score map].</param>
        /// <returns>string</returns>
        public string BuildChoice(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, bool isShowScoreMap)
        {
            IChoice choiceBuilder = this.CreateChoiceBuilder(ticket.BetTypeId);
            string choice = choiceBuilder.Render(ticket, ticketHelper, ticketData, isShowScoreMap);

            return choice;
        }

        /// <summary>
        /// Adds the odd.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="openTag">The open tag.</param>
        /// <param name="closeTag">The close tag.</param>
        /// <param name="FuncGetOddsTypeName">Name of the func get odds type.</param>
        /// <returns>BetListHTMLBuilder.</returns>
        public BetListHTMLBuilder AddOdds(ITicket ticket, List<ITicketData> ticketData, string openTag, string closeTag, GetCachePropertyById FuncGetOddsTypeName)
        {
            string odds = BuildOdds(ticket, ticketData, FuncGetOddsTypeName);
            this._content.Append(string.Join(null, new string[] { openTag, odds, closeTag }));
            return this;
        }

        /// <summary>
        /// Builds the odds.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="FuncGetOddsTypeName">Name of the func get odds type.</param>
        /// <returns>string</returns>
        public string BuildOdds(ITicket ticket, List<ITicketData> ticketData, GetCachePropertyById FuncGetOddsTypeName)
        {
            IOdds oddsBuilder = this.CreateOddsBuilder(ticket.BetTypeId);
            string odds = oddsBuilder.Render(ticket, ticketData, FuncGetOddsTypeName);

            return odds;
        }

        /// <summary>
        /// Adds the stake.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="openTag">The open tag.</param>
        /// <param name="closeTag">The close tag.</param>
        /// <returns>BetListHTMLBuilder.</returns>
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
        /// <returns>string</returns>
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
        /// <param name="openTag">The open tag.</param>
        /// <param name="closeTag">The close tag.</param>
        /// <returns></returns>
        public BetListHTMLBuilder AddTrans(ITicket ticket, string openTag, string closeTag)
        {
            string trans = BuildTrans(ticket);
            this._content.Append(string.Join(null, new string[] { openTag, trans, closeTag }));
            return this;
        }

        /// <summary>
        /// Builds the trans.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns></returns>
        public string BuildTrans(ITicket ticket)
        {
            ITrans transBuilder = this.CreateTransBuilder(ticket.BetTypeId);
            string trans = transBuilder.Render(ticket);
            return trans;
        }

        /// <summary>
        /// Adds the status.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data (Status33 Win/Place)</param>
        /// <param name="openTag">The open tag.</param>
        /// <param name="closeTag">The close tag.</param>
        /// <param name="isShowVNIP">if set to <c>true</c> [is show VNIP].</param>
        /// <returns></returns>
        public BetListHTMLBuilder AddStatus(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, string openTag, string closeTag, bool isShowVNIP)
        {
            string status = BuildStatus(ticket, ticketHelper, ticketData, isShowVNIP);
            this._content.Append(string.Join(null, new string[] { openTag, status, closeTag }));
            return this;
        }

        /// <summary>
        /// Builds the status.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data (Status33 Win/Place)</param>
        /// <param name="isShowVNIP">if set to <c>true</c> [is show VNIP].</param>
        /// <returns></returns>
        public string BuildStatus(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, bool isShowVNIP)
        {
            IStatus statusBuilder = this.CreateStatusBuilder(ticket.BetTypeId);
            string status = statusBuilder.Render(ticket, ticketHelper, ticketData, isShowVNIP);

            return status;
        }

        /// <summary>
        /// Creates the choice builder.
        /// </summary>
        /// <param name="bettype">The bettype.</param>
        /// <returns>IChoice.</returns>
        private IChoice CreateChoiceBuilder(int bettype)
        {
            string typeName = string.Join(
                                            null,
                                            new string[] 
                                            { 
                                                CHOICE_BUILDER_NAMESPACE, 
                                                ".Choice", 
                                                bettype.ToString() 
                                            }
                                         );

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
        /// <param name="bettype">The bettype.</param>
        /// <returns>IOdds.</returns>
        private IOdds CreateOddsBuilder(int bettype)
        {
            if (bettype <= 1199 && bettype >= 1101)
            {
                //Odds of Live Casino bettypes
                bettype = 1101;
            }
            string typeName = string.Join(
                                            null,
                                            new string[] 
                                            { 
                                                ODDS_BUILDER_NAMESPACE, 
                                                ".Odds", 
                                                bettype.ToString() 
                                            }
                                         );

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
        /// <param name="bettype">The bettype.</param>
        /// <returns>IStake.</returns>
        private IStake CreateStakeBuilder(int bettype)
        {
            string typeName = string.Join(
                                            null,
                                            new string[] 
                                            { 
                                                STAKE_BUILDER_NAMESPACE, 
                                                ".Stake", 
                                                bettype.ToString() 
                                            }
                                         );

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
        /// <returns></returns>
        private IStatus CreateStatusBuilder(int bettype)
        {
            string typeName = string.Join(
                                            null,
                                            new string[] 
                                            { 
                                                STATUS_BUILDER_NAMESPACE, 
                                                ".Status", 
                                                bettype.ToString() 
                                            }
                                         );

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
        /// <returns></returns>
        private ITrans CreateTransBuilder(int bettype)
        {
            string typeName = string.Join(
                                            null,
                                            new string[] 
                                            { 
                                                TRANS_BUILDER_NAMESPACE, 
                                                ".Trans", 
                                                bettype.ToString() 
                                            }
                                         );

            Type type = Type.GetType(typeName);

            if (type == null)
            {
                return new Trans1();
            }
            else
            {
                return Activator.CreateInstance(type) as ITrans;
            }
        }
    }
}