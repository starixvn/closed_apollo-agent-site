using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Fanex.Data.DNAide;
using DNAide.Web;
using DNAide.Utils;
using System.Text;
using DNAide.Core;
using LiveCasinoLibrary;
using LiveCasinoLibrary.SBAgent;

public class Bingo
{
    public static string EditBingoSetting(string custid, string roleid)
    {
        StringBuilder strHTML = new StringBuilder();
        string content = string.Empty;
        string template = TemplateHandler.LoadTemplate("MemberInfo/BingoPT.html");

        string strP2PGroup = TemplateHandler.CopyBlock(template, "p2p_group_setting");
        string strP2PGame = TemplateHandler.CopyBlock(template, "p2p_private_setting");

        return null;
    }
}
