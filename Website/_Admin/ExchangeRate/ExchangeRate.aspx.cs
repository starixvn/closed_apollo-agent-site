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
using System.Collections;
using Fanex.Age.UI.Admin;
using Fanex.Age.Excel.Admin.ExchangeRate;

public partial class _Admin_ExchangeRate_ExchangeRate : AdminBasePage
{
    public _Admin_ExchangeRate_ExchangeRate()
    {
        _fileName = "ExchangeRate.aspx";
    }

    public override void RenderContent(HtmlTextWriter writer)
    {
        var tpl = new ExchangeRate_Template
        {
            actionForm = string.Format("action='{0}?{1}'", _fileName, Request.QueryString.ToString())
        };

        if (_exportToExcel)
        {
            var data = GetInvokedData();
            var resMgr = GetLangResourceManager();
            var builder = new ExchangeRateDataBuilder(data, resMgr);
            var excelStream = builder.Build();
            
            if (excelStream != null)
            {
                ExportToExcel(excelStream, "ExchangeRate.xls");
                excelStream.Dispose();
                return;
            }

            //// Need to render report content only
            //tpl.ExcelContents.BoldRow = string.Empty;
            //tpl.BoldExportExcel.Hide();
            //RenderReportContent(tpl.ExcelContents);
            //ExportToExcel(Response, tpl.ExcelContents.ToString(), "WinlossAgent");
        }
        else
        {
            RenderReportContent(tpl.ExcelContents);
        }

        writer.Write(tpl);
    }

    private void RenderReportContent(ExchangeRate_ExcelContents_Block excelBlock)
    {
        var data = GetInvokedData();

        excelBlock.Content.Clear();
        int numRow = data.Rows.Count;

        if (numRow > 0)
        {
            var contentBlock = new ExchangeRate_ExcelContents_Content_Block();

            foreach (DataRow exRate in data.Rows)
            {
                contentBlock.rowClass = (numRow % 2) == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;

                contentBlock.ExchangeId = exRate["ExchangeId"].ToString();
                contentBlock.Currency = exRate["Currency"].ToString();
                contentBlock.MultipliedBy = exRate["MultipliedBy"].ToString();
                contentBlock.BaseRate = exRate["ActualRate"].ToString();
                contentBlock.MinBet = exRate["MinBet"].ToString();
                contentBlock.MinBetLive = exRate["MinBetLive"].ToString();
                contentBlock.Description = exRate["Description"].ToString();

                excelBlock.Content.Append(contentBlock);

                numRow--;
            }
        }
    }

    public override object InvokeData(Hashtable p)
    {
        return AdminBO.GetAllExchangeRates() ?? new DataTable();
    }
}