/*
 * Created 20100715@Simon - Render Account Info page
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Sunplus.Agent.UI._AccountInfo
{
    using System;
    using System.Web;
    using System.Data;
    using Sunplus.Agent.Model.BL.AccountInfo; 

    public partial class DownlineListInfo_Page : Fanex.Age.UI.TplPage
    {
        const int PAGESIZE = 500;
        int custid = 0;
        int totalPages = 0;
        int pageIndex = 1;

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            custid = (string.IsNullOrEmpty(Request["custid"])) ? 0 : Convert.ToInt32(Request["custid"]);
            totalPages = (string.IsNullOrEmpty(Request["totalpages"])) ? 0 : Convert.ToInt32(Request["totalpages"]);
            pageIndex = (string.IsNullOrEmpty(Request["pageidx"])) ? 1 : Convert.ToInt32(Request["pageidx"]);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            AccountInfoBO bo = new AccountInfoBO(CurrentUserSession.NetSessionId);
            return bo.GetCustomerList(custid, PAGESIZE, pageIndex, ref totalPages);
        }

        public override void RenderContent(System.Web.UI.HtmlTextWriter writer)
        {
            DownlineListInfo_Template tpl = new DownlineListInfo_Template();
            string maxHeight = "height";
            int versionIE = getVersionIE();
            if (versionIE < 0 || versionIE > 6) maxHeight = "max-height";
            tpl.maxHeight = maxHeight;
            tpl.Paging.PPrev.custid = custid.ToString();
            tpl.Paging.PNext.custid = custid.ToString();
            tpl.Paging.curpage = pageIndex.ToString();
            tpl.Paging.totalpages = totalPages.ToString();
            CreatePageButton(ref tpl, pageIndex, totalPages);

            DataTable tblDownline = View as DataTable;
            DataRowCollection rows = tblDownline.Rows;
            int userCount = rows.Count;
            if (userCount > 0)
            {
                //If exist member has length too long, then change the UI
                bool isExistLongText = IsExistLongText(rows);
                string widthLeft = "50%", textAlign = "left";
                if (isExistLongText)
                {
                    widthLeft = "52%";
                    textAlign = "";
                }

                string rowCollection = string.Empty;
                for (int i = 0; i < userCount; i = i + 2)
                {
                    DownlineListInfo_UserLine_Block tplUserLine = new DownlineListInfo_UserLine_Block();
                    tplUserLine.widthLeft = widthLeft;
                    tplUserLine.textAlign = textAlign;
                    tplUserLine.usercol1 = rows[i]["username"].ToString();
                    if (i + 1 < userCount)
                    {
                        tplUserLine.usercol2 = rows[i + 1]["username"].ToString();
                    }
                    else
                    {
                        tplUserLine.usercol2 = String.Empty;
                    }
                    rowCollection += tplUserLine.ToString();
                }
                tpl.UserLine.SetValue(rowCollection);
                writer.Write(String.Format("<script type='text/javascript'>parent.loadCustDownlineList('{0}');</script>", tpl.ToString()));
            }
            else
            {
                writer.Write(String.Format("<script type='text/javascript'>parent.loadCustDownlineList('{0}');</script>", "<center>" + Resources.Lang.noInfo + "</center>"));
            }
        }

        private static void CreatePageButton(ref DownlineListInfo_Template tpl, int pageIndex, int totalPages)
        {
            if (totalPages > 1)
            {
                int pageNext = pageIndex + 1;
                int pagePrev = pageIndex - 1;

                if (pageNext > totalPages)
                    tpl.Paging.PNext = null;
                else
                    tpl.Paging.PNext.pageidxNext = pageNext.ToString();

                if (pagePrev < 1)
                    tpl.Paging.PPrev = null;
                else
                    tpl.Paging.PPrev.pageidxPrev = pagePrev.ToString();
            }
            else
            {
                tpl.Paging = null;
            }
        }

        public bool IsExistLongText(DataRowCollection Rows)
        {
            string userName = string.Empty;
            foreach (DataRow row in Rows)
            {
                userName = row["username"].ToString();
                if (userName.Length > 12)
                    return true;
            }
            return false;
        }

        public int getVersionIE()
        {
            if (HttpContext.Current.Request.Browser.Browser == "IE")
            {
                int versionIE = 6;
                string version = HttpContext.Current.Request.Browser.Version.Substring(0, 1);
                if (!String.IsNullOrEmpty(version))
                {
                    versionIE = Convert.ToInt32(version);
                }
                return versionIE;
            }
            return -1; //not IE
        }
    }
}
