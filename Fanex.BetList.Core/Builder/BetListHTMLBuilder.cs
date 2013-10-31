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
    using Fanex.BetList.Core.Builder.AgentCommBuilder;
    using Fanex.BetList.Core.Builder.AgentShareBuilder;
    using Fanex.BetList.Core.Builder.ChoiceBuilder;
    using Fanex.BetList.Core.Builder.DiscountBuilder;
    using Fanex.BetList.Core.Builder.OddsBuilder;
    using Fanex.BetList.Core.Builder.StakeBuilder;
    using Fanex.BetList.Core.Builder.StatusBuilder;
    using Fanex.BetList.Core.Builder.TimeBuilder;
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
        private const string AGENTCOMM_BUILDER_NAMESPACE = "Fanex.BetList.Core.Builder.AgentCommBuilder";
        private const string AGENTSHARE_BUILDER_NAMESPACE = "Fanex.BetList.Core.Builder.AgentShareBuilder";

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
        /// Add the time.
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="openTag"></param>
        /// <param name="closeTag"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public BetListHTMLBuilder AddTime(ITicket ticket, string openTag, string closeTag, BetListPage page)
        {
            string time = BuildTime(ticket, page);
            this._content.Append(string.Join(null, new string[] { openTag, time, closeTag }));
            return this;
        }

        /// <summary>
        /// Build the time.
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public string BuildTime(ITicket ticket, BetListPage page)
        {
            ITime timeBuilder = new Time();
            string time = timeBuilder.Render(ticket, page);
            return time;
        }

        /// <summary>
        /// Add discount.
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="openTag"></param>
        /// <param name="closeTag"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public BetListHTMLBuilder AddDiscount(ITicket ticket, string openTag, string closeTag, BetListPage page)
        {
            string discount = BuildDiscount(ticket, page);
            this._content.Append(string.Join(null, new string[] { openTag, discount, closeTag }));
            return this;
        }

        /// <summary>
        /// Build discount.
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public string BuildDiscount(ITicket ticket, BetListPage page)
        {
            IDiscount discountBuilder = new Discount();
            string discount = discountBuilder.Render(ticket, page);

            return discount;
        }

        /// <summary>
        /// Adds the choice.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="openTag">The open tag.</param>
        /// <param name="closeTag">The close tag.</param>
        /// <param name="isShowScoreMap">If set to <c>true</c> [is show score map].</param>
        /// <returns>BetListHTMLBuilder.</returns>
        public BetListHTMLBuilder AddChoice(
            ITicket ticket, 
            ITicketHelper ticketHelper, 
            List<ITicketData> ticketData,
            string openTag, 
            string closeTag, 
            bool isShowScoreMap)
        {
            string choice = BuildChoice(ticket, ticketHelper, ticketData, isShowScoreMap, BetListPage.Default);
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
        /// <returns>string.</returns>
        public string BuildChoice(
            ITicket ticket, 
            ITicketHelper ticketHelper, 
            List<ITicketData> ticketData, 
            bool isShowScoreMap,
            BetListPage betlistPage)
        {
            IChoice choiceBuilder = this.CreateChoiceBuilder(ticket.BetTypeId);
            string choice = choiceBuilder.Render(ticket, ticketHelper, ticketData, isShowScoreMap, betlistPage);

            return choice;
        }

        /// <summary>
        /// Adds the odd.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="openTag">The open tag.</param>
        /// <param name="closeTag">The close tag.</param>
        /// <param name="FuncGetOddsTypeName">Name of the function get odds type.</param>
        /// <returns>BetListHTMLBuilder.</returns>
        public BetListHTMLBuilder AddOdds(
            ITicket ticket, 
            List<ITicketData> ticketData, 
            string openTag, 
            string closeTag, 
            GetCachePropertyById FuncGetOddsTypeName)
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
        /// <param name="FuncGetOddsTypeName">Name of the function get odds type.</param>
        /// <returns>String.</returns>
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
        public BetListHTMLBuilder AddTrans(
            ITicket ticket, 
            ITicketHelper ticketHelper, 
            string openTag, 
            string closeTag, 
            GetCachePropertyById FuncGetOddsTypeName, 
            BetListPage betListPage)
        {
            string trans = BuildTrans(ticket, ticketHelper, FuncGetOddsTypeName, betListPage);
            this._content.Append(string.Join(null, new string[] { openTag, trans, closeTag }));

            return this;
        }

        /// <summary>
        /// Builds the trans.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns></returns>
        public string BuildTrans(
            ITicket ticket, 
            ITicketHelper ticketHelper, 
            GetCachePropertyById FuncGetOddsTypeName, 
            BetListPage betListPage)
        {
            ITrans transBuilder = this.CreateTransBuilder(ticket.BetTypeId);
            string trans = transBuilder.Render(ticket, ticketHelper, FuncGetOddsTypeName, betListPage);

            return trans;
        }

        /// <summary>
        /// Adds the status.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data (Status33 Win/Place).</param>
        /// <param name="openTag">The open tag.</param>
        /// <param name="closeTag">The close tag.</param>
        /// <param name="isShowVNIP">If set to <c>true</c> [is show VNIP].</param>
        /// <returns></returns>
        public BetListHTMLBuilder AddStatus(
            ITicket ticket, 
            List<ITicketData> ticketData, 
            string openTag, 
            string closeTag, 
            bool isShowVNIP)
        {
            string status = BuildStatus(ticket, ticketData);
            this._content.Append(string.Join(null, new string[] { openTag, status, closeTag }));

            return this;
        }

        /// <summary>
        /// Builds the status.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data (Status33 Win/Place).</param>
        /// <param name="isShowVNIP">If set to <c>true</c> [is show VNIP].</param>
        /// <returns></returns>
        public string BuildStatus(ITicket ticket, List<ITicketData> ticketData)
        {
            IStatus statusBuilder = this.CreateStatusBuilder(ticket.BetTypeId);
            string status = statusBuilder.Render(ticket, ticketData);

            return status;
        }

        /// <summary>
        /// Add the agent Comm.
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="openTag"></param>
        /// <param name="closeTag"></param>
        /// <returns></returns>
        public BetListHTMLBuilder AddAgentComm(ITicket ticket, string openTag, string closeTag)
        {
            string agentComm = BuildAgentComm(ticket);
            this._content.Append(string.Join(null, new string[] { openTag, agentComm, closeTag }));

            return this;
        }

        /// <summary>
        /// Build agent comm.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns></returns>
        public string BuildAgentComm(ITicket ticket)
        {
            IAgentComm agentCommBuilder = this.CreateAgentCommBuilder(ticket.BetTypeId);
            string agentComm = agentCommBuilder.Render(ticket);

            return agentComm;
        }

        /// <summary>
        /// Add Agent share.
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="ticketData">Ticket data.</param>
        /// <param name="openTag"></param>
        /// <param name="closeTag"></param>
        /// <returns></returns>
        public BetListHTMLBuilder AddAgentShare(ITicket ticket, List<ITicketData> ticketData, string openTag, string closeTag)
        {
            string agentShare = BuildAgentShare(ticket, ticketData);
            this._content.Append(string.Join(null, new string[] { openTag, agentShare, closeTag }));

            return this;
        }

        /// <summary>
        /// Buil agent share.
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="ticketData">Ticket data.</param>
        /// <returns></returns>
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
                                            });

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
        /// <returns>IOdds.</returns>
        private IOdds CreateOddsBuilder(int bettype)
        {
            if (bettype <= 1199 && bettype >= 1101)
            {
                // Odds of Live Casino bettypes
                bettype = 1101;
            }

            string typeName = string.Join(
                                            null,
                                            new string[]
                                            {
                                                ODDS_BUILDER_NAMESPACE,
                                                ".Odds",
                                                bettype.ToString()
                                            });

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
                                            });

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
                                            });

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
                                            });

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
        /// <returns></returns>
        private IAgentShare CreateAgentShareBuilder(int bettype)
        {
            string typeName = string.Join(
                                           null,
                                           new string[]
                                            {
                                                AGENTSHARE_BUILDER_NAMESPACE,
                                                ".AgentShare",
                                                bettype.ToString()
                                            });

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
        /// Creates the agent Comm builder.
        /// </summary>
        /// <returns></returns>
        private IAgentComm CreateAgentCommBuilder(int bettype)
        {
            string typeName = string.Join(
                                           null,
                                           new string[]
                                            {
                                                AGENTCOMM_BUILDER_NAMESPACE,
                                                ".AgentComm",
                                                bettype.ToString()
                                            });

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