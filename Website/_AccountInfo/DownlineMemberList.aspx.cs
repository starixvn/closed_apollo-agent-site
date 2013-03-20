using System.Data;
using System.Web.UI;
using Fanex.Age.UI.Components;
using Fanex.Agent.UI._AccountInfo;
using Sunplus.Agent.Model.BL.AccountInfo;

public partial class _AccountInfo_DownlineMemberList : Fanex.Age.UI.TplPage
{
    private int custId = 0;
    private int pageSize = 50;
    private int pageIndex = 1;
    private int totalPage = 1;
    private DataSet dataset = null;

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
        AccountInfoBO bo = new AccountInfoBO(CurrentUserSession.NetSessionId);
        this.dataset = bo.Age_Customer_GetMemberDownLine(this.custId, pageIndex, pageSize, ref totalPage);

        return this.dataset;
    }

    public override void RenderContent(HtmlTextWriter writer)
    {
        DownlineMemberList_Template tpl = new DownlineMemberList_Template();
        DataTable dt = new DataTable();
        if (dataset != null)
        {
            if (dataset.Tables[0] != null)
            {
                dt = dataset.Tables[0];
            }
        }

        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                DownlineMemberList_Content_Block tplContent = new DownlineMemberList_Content_Block();
                tpl.Content.Clear();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    RenderContent(tplContent, dt.Rows[i]);
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
        }
        else
        {
            tpl.Content.Hide();
            tpl.PagingControlBlock.Hide();
        }

        writer.Write(tpl);
    }

    private void RenderContent(DownlineMemberList_Content_Block tplContent, DataRow row)
    {
        tplContent.CustID = row["CustId"].ToString();
        tplContent.UserName = row["Username"].ToString();
        tplContent.MemberUserName = row["Username"].ToString();
    }
}