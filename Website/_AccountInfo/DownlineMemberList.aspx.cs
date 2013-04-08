// --------------------------------------------------------------------------
// <copyright file="DownlineMemberList.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.3.26">Created</change>
// </history>

using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using Apollo.Age.Criterias.Accounts;
using Apollo.Age.Entities;
using Apollo.Age.Services.Impl;
using Apollo.Age.Services.Interface;
using Fanex.Age.UI;
using Fanex.Age.UI.Components;
using Fanex.Agent.UI._AccountInfo;
using Fanex.Common;
using Apollo.Age.Account.Services;

public partial class _AccountInfo_DownlineMemberList : TplPage
{
    private int custId = 0;
    private int pageSize = 50;
    private int pageIndex = 1;
    private int totalPage = 1;
    private List<Member> membersList = null;

    public int CustId
    {
        get
        {
            return Converter.ToInt(Request.QueryString["custid"]);
        }
    }

    public int PageIndex
    {
        get
        {
            return Converter.ToInt(Request.QueryString["pageIndex"]);
        }
    }

    public int PageSize
    {
        get
        {
            return Converter.ToInt(Request.QueryString["pageSize"]);
        }
    }

    public override void ProcessRequest(ref Hashtable p)
    {
        this.custId = CustId;
        this.pageIndex = PageIndex == 0 ? this.pageIndex : PageIndex;
        this.pageSize = PageSize == 0 ? this.pageSize : PageSize;
    }

    public override object ValidateRequest(Hashtable p)
    {
        return null;
    }

    public override object InvokeData(Hashtable p)
    {
        IAccountService service = new AccountService();

        MemberDownlineCriteria criteria = new MemberDownlineCriteria()
        {
            AgentId = this.custId,
            PageIndex = this.pageIndex,
            PageSize = this.pageSize,
            TotalPages = this.totalPage
        };

        membersList = service.GetMemberDownline(ref criteria);

        this.totalPage = criteria.TotalPages;
        return membersList;
    }

    public override void RenderContent(HtmlTextWriter writer)
    {
        DownlineMemberList_Template tpl = new DownlineMemberList_Template();

        if (membersList != null)
        {
            DownlineMemberList_Content_Block tplContent = new DownlineMemberList_Content_Block();
            tpl.Content.Clear();

            foreach (var downLine in membersList)
            {
                RenderContent(tplContent, downLine);
                tpl.Content.Append(tplContent);
            }

            tpl.NoInfoBlock.Hide();

            tpl.PagingControlBlock.pagingControl = new Paging_Control(null, "PagingControl", pageSize, Request.Url.AbsoluteUri, totalPage * pageSize, false);
        }
        else
        {
            tpl.Content.Hide();
            tpl.PagingControlBlock.Hide();
        }

        writer.Write(tpl);
    }

    private void RenderContent(DownlineMemberList_Content_Block tplContent, Member downLine)
    {
        tplContent.CustID = downLine.Id.ToString();
        tplContent.UserName = downLine.Username;
        tplContent.MemberUserName = downLine.Username;
    }
}
