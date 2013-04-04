// --------------------------------------------------------------------------
// <copyright file="MessageService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.4">Created</change>
// </history>

namespace Apollo.Age.Account.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MessageService
    {
        private static int TargetId = 3300;

        private static string SiteList = "3000,3300";

        private string GetMessageType(int msgType, bool isSticky)
        {
            var typeName = string.Empty;

            if (msgType == 1 && !isSticky)
            {
                typeName = "private";
            }
            else if ((msgType == 1 || msgType == 3) && isSticky)
            {
                typeName = "special";
            }
            else if (msgType == 3 && !isSticky)
            {
                typeName = "public";
            }
            else
            {
                typeName = "--";
            }

            return typeName;
        }
    }
}
