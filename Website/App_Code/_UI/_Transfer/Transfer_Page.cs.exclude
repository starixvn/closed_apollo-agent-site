// --------------------------------------------------------------------------
// <copyright file="InfoMember.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2012.05.31">Improve performance, check StypeCop</change>
// </history>

namespace Sunplus.Agent.UI._Transfer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Data;
    using System.Collections;

    using Sunplus.Agent.UI._MemberInfo.CustomerList;
    using Fanex.Age.UI;
    using Fanex.Age.UI.Components;
    using Sunplus.Agent.UI._MemberInfo.CustomerList.Components;

    using Sunplus.Agent.Model.Entities.Permission;
    using Sunplus.Agent.Model;

    public abstract class Transfer_Page : TplPage
    {
        public int status = 1;
        public int pageIndex = 1;
        public int totalPage = 0;
        public int pageSize = 500;
        public int custId = 0;
        public string userName = string.Empty;
        public bool isYesterdayBalance = false;
        public string langKey = string.Empty;
        public string bgColor = string.Empty;
        public string isCheckAllYesBalance = string.Empty;
        public Dictionary<String, DataTable> viewData = new Dictionary<string, DataTable>();
        private List<object> PAGE_SIZES = new List<object>(new object[] { 10, 50, 100, 500});
        public Hashtable htbQueryString = new Hashtable();

        public Transfer_Page()
        {
           
        }

        public string RenderPageSize(string id, int pagesize)
        {
            return Resources.Lang.pagesize + Utilities.HtmlGenerator.ComboBox("sel" + id, (object)(pagesize), PAGE_SIZES, id + ".SetPageSize(this.value)");
        }
    }
}