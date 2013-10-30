// ---------------------------------------------------------------------------------
// <copyright file="ITime.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Kaden.Trinh" date="2013.06.26">Create</change>
// </history>
// ---------------------------------------------------------------------------------
namespace Fanex.BetList.Core.Builder.TimeBuilder
{
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Templates;
    using NPOI.SS.UserModel;

    public interface ITime
    {
        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        Time_Template Template { get; set; }

        /// <summary>
        /// Render time.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="page">Link from page.</param>
        /// <returns></returns>
        Time_Template Render(ITicket ticket, BetListPage page);
    }
}