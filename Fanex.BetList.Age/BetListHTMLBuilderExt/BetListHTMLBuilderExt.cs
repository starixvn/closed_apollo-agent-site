// ---------------------------------------------------------------------------------
// <copyright file="BetListHTMLBuilderExt.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.18">Create</change>
//     <change who="Marc.Bui" date="2013.01.28">Update PTs columns</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Age.BetListHTMLBuilderExt
{
    using System;
    using System.Collections.Generic;
    using Fanex.BetList.Age.BetListHTMLBuilderExt.AgentCommBuilder;
    using Fanex.BetList.Age.BetListHTMLBuilderExt.AgentPTBuilder;
    using Fanex.BetList.Age.BetListHTMLBuilderExt.FooterBuilder;
    using Fanex.BetList.Age.BetListHTMLBuilderExt.WinLostBuilder;
    using Fanex.BetList.Age.Definition;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;

    /// <summary>
    /// Extend CoreBetList HTML Builder.
    /// </summary>
    public static class BetListHTMLBuilderExt
    {
        private const string FOOTER_BUILDER_NAMESPACE = "Fanex.BetList.Age.BetListHTMLBuilderExt.FooterBuilder";
        private const string AGENTPT_BUILDER_NAMESPACE = "Fanex.BetList.Age.BetListHTMLBuilderExt.AgentPTBuilder";
        private const string AGENTCOMM_BUILDER_NAMESPACE = "Fanex.BetList.Age.BetListHTMLBuilderExt.AgentCommBuilder";

        /// <summary>
        /// Adds the no.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="index">The index.</param>
        /// <param name="openTag">The open tag.</param>
        /// <param name="closeTag">The close tag.</param>
        /// <returns></returns>
        public static BetListHTMLBuilder AddNo(this BetListHTMLBuilder factory, int index, string openTag, string closeTag)
        {
            factory.Append(string.Join(null, new string[] { openTag, index.ToString(), closeTag }));
            return factory;
        }

        /// <summary>
        /// Adds the win loss.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="ticket">The ticket.</param>
        /// <param name="openTag">The open tag.</param>
        /// <param name="closeTag">The close tag.</param>
        /// <returns></returns>
        public static BetListHTMLBuilder AddWinLoss(this BetListHTMLBuilder factory, ITicket ticket, string openTag, string closeTag)
        {
            IWinLoss winlossBuilder = CreateWinLossBuilder();
            string winloss = winlossBuilder.Render(ticket);
            factory.Append(string.Join(null, new string[] { openTag, winloss, closeTag }));

            return factory;
        }

        /// <summary>
        /// Adds the agent PT.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="ticket">The ticket.</param>
        /// <param name="openTag">The open tag.</param>
        /// <param name="closeTag">The close tag.</param>
        /// <returns></returns>
        public static BetListHTMLBuilder AddAgentPT(this BetListHTMLBuilder factory, ITicket ticket, List<ITicketData> ticketData, string openTag, string closeTag)
        {
            IAgentPT agentPTBuilder = CreateAgentPTBuilder(ticket.BetTypeId);
            string agentPT = agentPTBuilder.Render(ticket, ticketData);
            factory.Append(string.Join(null, new string[] { openTag, agentPT, closeTag }));

            return factory;
        }

        /// <summary>
        /// Add the agent Comm.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="ticket"></param>
        /// <param name="ticketData"></param>
        /// <param name="openTag"></param>
        /// <param name="closeTag"></param>
        /// <returns></returns>
        public static BetListHTMLBuilder AddAgentComm(
            this BetListHTMLBuilder factory, 
            ITicket ticket, 
            List<ITicketData> ticketData, 
            bool hideDetail, 
            string openTag, 
            string closeTag)
        {
            IAgentComm agentCommBuilder = CreateAgentCommBuilder(ticket.BetTypeId);
            string agentCom = agentCommBuilder.Render(ticket, ticketData, hideDetail);
            factory.Append(string.Join(null, new string[] { openTag, agentCom, closeTag }));

            return factory;
        }

        /// <summary>
        /// Adds the member.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="ticket">The ticket.</param>
        /// <param name="openTag">The open tag.</param>
        /// <param name="closeTag">The close tag.</param>
        /// <returns></returns>
        public static BetListHTMLBuilder AddMember(this BetListHTMLBuilder factory, ITicket ticket, string openTag, string closeTag)
        {
            factory.Append(string.Join(null, new string[] { openTag, ticket.UserName, closeTag }));

            return factory;
        }

        /// <summary>
        /// Adds the footer.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="tickets">The tickets.</param>
        /// <param name="openTag">The open tag.</param>
        /// <param name="closeTag">The close tag.</param>
        /// <param name="userType">Type of the user.</param>
        /// <param name="betlistType">Type of the betlist.</param>
        /// <returns></returns>
        public static BetListHTMLBuilder AddFooter(this BetListHTMLBuilder factory, IList<ITicket> tickets, string openTag, string closeTag, BetListType betlistType)
        {
            int footerType;

            if (tickets.Count == 0)
            {
                footerType = 0;
            }
            else if ((betlistType & BetListType.Winlost) == BetListType.Winlost)
            {
                footerType = 1;
            }
            else
            {
                return factory;
            }

            IFooter footerBuilder = CreateFooterBuilder(footerType);
            string footer = footerBuilder.Render(tickets, betlistType);
            factory.Append(string.Join(null, new string[] { openTag, footer, closeTag }));

            return factory;
        }

        /// <summary>
        /// Creates the win loss builder.
        /// </summary>
        /// <returns></returns>
        private static IWinLoss CreateWinLossBuilder()
        {
            return new WinLost1();
        }

        /// <summary>
        /// Creates the agent PT builder.
        /// </summary>
        /// <returns></returns>
        private static IAgentPT CreateAgentPTBuilder(int bettype)
        {
            string typeName = string.Join(
                                           null,
                                           new string[]
                                            {
                                                AGENTPT_BUILDER_NAMESPACE,
                                                ".AgentPT",
                                                bettype.ToString()
                                            });

            Type type = Type.GetType(typeName);

            if (type == null)
            {
                return new AgentPT1();
            }
            else
            {
                return Activator.CreateInstance(type) as IAgentPT;
            }
        }

        /// <summary>
        /// Creates the agent Comm builder.
        /// </summary>
        /// <returns></returns>
        private static IAgentComm CreateAgentCommBuilder(int bettype)
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

        /// <summary>
        /// Creates the footer builder.
        /// </summary>
        /// <param name="footerType">Type of the footer.</param>
        /// <returns></returns>
        private static IFooter CreateFooterBuilder(int footerType)
        {
            string typeName = string.Join(
                                            null,
                                            new string[]
                                            {
                                                FOOTER_BUILDER_NAMESPACE,
                                                ".Footer",
                                                footerType.ToString()
                                            });

            Type type = Type.GetType(typeName);

            if (type == null)
            {
                return new BlankFooter();
            }
            else
            {
                return Activator.CreateInstance(type) as IFooter;
            }
        }
    }
}