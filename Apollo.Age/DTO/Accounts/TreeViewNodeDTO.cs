// --------------------------------------------------------------------------
// <copyright file="TreeViewNodeDTO.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.3.22">Created</change>
// </history>

namespace Apollo.Age.DTO.Accounts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class TreeViewNodeDTO
    {
        private readonly string urlAgentList = "<a href='{0}{1}'>{2}</a>";

        private readonly string urlAcountInfo = "<a href='{0}{1}'>{2}</a>";

        public static int Type = 0;

        // public static int Root = 0;

        public int Id { get; set; }

        public string UserName { get; set; }

        public string Text 
        {
            get 
            {
                if (Type == 0)
                {
                    return string.Format(urlAgentList, UserName, Id, UserName);
                }
                else if (Type == 1)
                {
                    return string.Format(urlAcountInfo, UserName, Id, UserName);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public bool HasChildren { get; set; }
    }
}
