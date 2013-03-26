using System.Collections.Generic;
using System.Web.UI;
using Apollo.Age.Criterias.Accounts;
using Apollo.Age.DTO.Accounts;
using Apollo.Age.Entities;
using Apollo.Age.Services.Impl;
using Apollo.Age.Services.Interface;
using Fanex.Age.UI.Components;
using Fanex.Agent.UI._AccountInfo;

public partial class _AccountInfo_DownlineMemberList : Fanex.Age.UI.TplPage
{
    private int custId = 0;
    private int pageSize = 50;
    private int pageIndex = 1;
    private int totalPage = 1;
    private List<Member> membersList = null;

    public override void ProcessRequest(ref System.Collections.Hashtable p)
    {
        this.custId = string.IsNullOrEmpty(Request.QueryString["custid"]) ? -1 : int.Parse(Request.QueryString["custid"].ToString());
        this.pageIndex = string.IsNullOrEmpty(Request.QueryString["pageIndex"]) ? pageIndex : int.Parse(Request.QueryString["pageIndex"].ToString());
        this.pageSize = string.IsNullOrEmpty(Request.QueryString["pageSize"]) ? pageSize : int.Parse(Request.QueryString["pageSize"].ToString());
    }

    public override object ValidateRequest(System.Collections.Hashtable p)
    {
        return null;
    }

    public override object InvokeData(System.Collections.Hashtable p)
    {
        IMemberService service = new MemberService();

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