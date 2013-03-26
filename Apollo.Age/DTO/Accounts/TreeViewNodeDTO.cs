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

    public class TreeViewNode
    {
        private readonly string urlAcountInfo = "<a href='../_AccountInfo/AccountInfo.aspx?IsMember=0&custid={0}&txtUsername={1}' target='menu'> {2} </a>";

        private readonly string urlAgentList = "<a href='../_MemberInfo/CustomerList/AgentList.aspx?custid={0}&us={1}' target='main'> {2} </a>";

        public static int Type = 0;

        public static int Root = 0;

        public int Id { get; set; }

        public string UserName { get; set; }

        public string Text 
        {
            get 
            {
                if (Type == 0)
                {
                    return string.Format(urlAgentList, Id, UserName, UserName);
                }
                else if (Type == 1)
                {
                    return string.Format(urlAcountInfo, Id, UserName, UserName);
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
