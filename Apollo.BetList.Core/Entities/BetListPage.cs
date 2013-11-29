// ---------------------------------------------------------------------------------
// <copyright file="BetListPage.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Kaden.Trinh" date="2013.06.27">Create</change>
// </history>
// ---------------------------------------------------------------------------------
namespace Apollo.BetList.Core.Entities
{
    public enum BetListPage : int
    {
        /// <summary>
        /// Link from forecast page.
        /// </summary>
        Forecast = 1,

        /// <summary>
        /// Link from report Outstanding page.
        /// </summary>
        Outstanding = 4,

        /// <summary>
        /// Link from report page.
        /// </summary>
        Report = 2,

        /// <summary>
        /// Link from cancel ticket.
        /// </summary>
        Cancel = 3,

        /// <summary>
        /// Link from transactions ticket.
        /// </summary>
        Transaction = 6,

        /// <summary>
        /// Link from account info.
        /// </summary>
        AccountInfo = 7,

        /// <summary>
        /// Link from Wager page.
        /// </summary>
        Wager = 8,

        /// <summary>
        /// Other Pages.
        /// </summary>
        Default = 5
    }
}