using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using DNAide.Web;
using DNAide.Core;
using DNAide.Utils;
using CasinoLibrary.SBAgent;
using System.Text;

public partial class Popup_ViewCasinoDetail : TplPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.CssGroup = "popup";
        int userBetID = CastAide.AsInt32(Request["userbetid"], 0);
        string main = TemplateHandler.LoadTemplate("Popup/CasinoWinLossDetail.html");
        string noInfo = TemplateHandler.LoadTemplate("NoInfo.html");
        string row = TemplateHandler.CopyBlock(main, "row");
        string row2 = TemplateHandler.CopyBlock(main, "row2");
        StringBuilder _row2 = new StringBuilder();
        StringBuilder _row = new StringBuilder();

        // load language
        TemplateHandler.Assign(ref main, "lblHeader", Resources.Lang.handhistorydetail);
        TemplateHandler.Assign(ref main, "Result", Resources.Lang.csresult);
        TemplateHandler.Assign(ref main, "Player", Resources.Lang.player);
        TemplateHandler.Assign(ref main, "type", Resources.Lang.type);
        TemplateHandler.Assign(ref main, "bet", Resources.Lang.lblBet);
        TemplateHandler.Assign(ref main, "win", Resources.Lang.win);
        TemplateHandler.Assign(ref main, "total", Resources.Lang.total);
        TemplateHandler.Assign(ref noInfo, "noinfo", Resources.Lang.noInfo);
        TemplateHandler.Assign(ref noInfo, "num-cols", "3");

        Report report = new Report();
        string typeGame = CastAide.AsString(Request.QueryString["typegame"], "casinogame");
        DataTable[] tableArray = report.WinLossDetail(userBetID);
        decimal dcBet = 0, dcWin = 0;
        if (tableArray.Length < 2)
        {
            TemplateHandler.ReplaceBlock(ref main, "content", noInfo);
        }
        else
        {
            //first table
            if (tableArray[0].Rows.Count > 0)
            {
                TemplateHandler.Assign(ref main, "tablename", CastAide.AsString(tableArray[0].Rows[0][DAL.APIClient.APICasinoProvider.Lang("TableName")]));
                TemplateHandler.Assign(ref main, "datetime", CastAide.AsString(tableArray[0].Rows[0]["BetTime"]));
                TemplateHandler.Assign(ref main, "UserName", CastAide.AsString(tableArray[0].Rows[0]["UserName"]));
                foreach (DataRow r in tableArray[0].Rows)
                {
                    string tpl = row;
                    TemplateHandler.Assign(ref tpl, "action", CastAide.AsString(r[DAL.APIClient.APICasinoProvider.Lang("ResultName")]));
                    TemplateHandler.Assign(ref tpl, "vl", CastAide.AsString(r["Result"]));
                    _row.Append(tpl);
                }
                TemplateHandler.ReplaceBlock(ref main, "row", _row.ToString());
            }
            else
            {
                TemplateHandler.Assign(ref main, "tablename", "");
                TemplateHandler.Assign(ref main, "datetime", "");
                TemplateHandler.Assign(ref main, "UserName", "-");
                TemplateHandler.ReplaceBlock(ref main, "row", noInfo);
            }

            //second table
            if (tableArray[1].Rows.Count > 0)
            {
                foreach (DataRow r in tableArray[1].Rows)
                {
                    string rowTpl = row2;
                    TemplateHandler.Assign(ref rowTpl, "bettype", CastAide.AsString(r[DAL.APIClient.APICasinoProvider.Lang("BetTypeName")]));
                    TemplateHandler.Assign(ref rowTpl, "betvl", NumberFormat.DecFormat(r["Bet"], 2));
                    TemplateHandler.Assign(ref rowTpl, "winvl", NumberFormat.DecFormat(r["WinLoss"], 2));
                    dcBet += CastAide.AsDecimal(r["Bet"]);
                    dcWin += CastAide.AsDecimal(r["WinLoss"]);
                    _row2.Append(rowTpl);
                }
                TemplateHandler.ReplaceBlock(ref main, "row2", _row2.ToString());
                TemplateHandler.Assign(ref main, "totalbet", Common.NumberReport(dcBet));
                TemplateHandler.Assign(ref main, "totalwin", Common.NumberReport(dcWin));
            }
            else
            {
                TemplateHandler.RemoveBlock(ref main, "rowTotal");
                TemplateHandler.ReplaceBlock(ref main, "row2", noInfo);
            }
        }

        this.TplSetContent(main);
    }
}
