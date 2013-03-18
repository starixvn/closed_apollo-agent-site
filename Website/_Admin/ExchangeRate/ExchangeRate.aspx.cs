/*
 * Created 20130315@Lewis
 * Revision:
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fanex.Age.UI;
using Fanex.Age.UI.Admin.ExchangeRate;
using System.Data;
using Sunplus.Agent.Model.BL.Admin;

public partial class _Admin_ExchangeRate_ExchangeRate : TplPage
{
    private string _fileName = "ExchangeRate.aspx";

    protected const string EVEN_ROW_CLASS = "even";
    protected const string ODD_ROW_CLASS = "odd";

    private AdminBO _adminBO;
    protected AdminBO AdminBO
    {
        get
        {
            if (_adminBO == null)
            {
                _adminBO = new AdminBO(CurrentUserSession.NetSessionId);
            }

            return _adminBO;
        }
    }

    public override void RenderContent(HtmlTextWriter writer)
    {
        var tpl = new ExchangeRate_Template
        {
            actionForm = string.Format("action='{0}?{1}'", _fileName, Request.QueryString.ToString())
        };

        RenderReportContent(tpl.ExcelContents);

        writer.Write(tpl);
    }

    private void RenderReportContent(ExchangeRate_ExcelContents_Block excelBlock)
    {
        var data = View as DataTable; // data got from InvokeData()

        excelBlock.Content.Clear();
        int numRow = data.Rows.Count;

        if (numRow > 0)
        {
            var contentBlock = new ExchangeRate_ExcelContents_Content_Block();

            foreach (DataRow exRate in data.Rows)
            {
                contentBlock.rowClass = (numRow % 2) == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;

                contentBlock.ExchangeId = exRate["ExchangeId"].ToString();
                contentBlock.Currency = exRate["Code"].ToString();
                contentBlock.MultipliedBy = exRate["MultipliedBy"].ToString();
                contentBlock.BaseRate = exRate["ActualRate"].ToString();
                contentBlock.MinBet = exRate["MinBet"].ToString();
                contentBlock.MinBetLive = exRate["MinBetLive"].ToString();
                contentBlock.Description = exRate["Description"].ToString();

                //contentBlock.UserName = row["username"].ToString();
                //contentBlock.DownlineMemberId = row["custid"].ToString();
                //contentBlock.FdateDownline = Formater.ShortDateFormat(_dateFrom);
                //contentBlock.TdateDownline = Formater.ShortDateFormat(_dateTo);
                //contentBlock.ActionPage = "../../_BetList/BetList.aspx";
                //contentBlock.turnover = Formater.DecFormat(row["turnover"], 2);
                //contentBlock.gcomm = Formater.DecFormat(row["grosscomm"], 2);
                //contentBlock.winlossmem = Formater.DecFormat(row["memberwinlost"], 2);
                //contentBlock.commem = Formater.DecFormat(row["playercomm"], 2);
                //contentBlock.totalmem = Formater.DecFormat(row["membertotal"], 2);
                //contentBlock.winlossagent = Formater.DecFormat(row["agwinlost"], 2);
                //contentBlock.comagent = Formater.DecFormat(row["agcomm"], 2);
                //contentBlock.totalagent = Formater.DecFormat(row["agtotal"], 2);
                //contentBlock.company = Formater.DecFormat(row["company"], 2);

                excelBlock.Content.Append(contentBlock);

                numRow--;
            }
        }
    }

    public override void ProcessRequest(ref System.Collections.Hashtable p)
    {

    }

    public override object ValidateRequest(System.Collections.Hashtable p)
    {
        return null;
    }

    public override object InvokeData(System.Collections.Hashtable p)
    {
        return AdminBO.GetAllExchangeRates() ?? new DataTable();
    }
}