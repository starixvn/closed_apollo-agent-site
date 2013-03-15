using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fanex.Age.UI;
using Fanex.Age.UI.Admin.ExchangeRate;
using System.Data;

public partial class _Admin_ExchangeRate_ExchangeRate : TplPage
{
    private string _fileName = "ExchangeRate.aspx";

    public override void RenderContent(HtmlTextWriter writer)
    {
        var tpl = new ExchangeRate_Template
        {
            actionForm = string.Format("action='{0}?{1}'", _fileName, Request.QueryString.ToString())
        };

        writer.Write(tpl);
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
        return new DataTable();
    }
}