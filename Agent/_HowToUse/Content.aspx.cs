using System.Web.UI;
using System;

namespace Sunplus.Agent.UI._HowToUse
{
    public partial class Content_Page : Fanex.Age.UI.TplPage
    {
        // Folder Name
        private const string fdCasinoGuide = "CasinoGuide";
        private const string fdAccInfo = "AccInfo";
        private const string fdAutoPT = "AutoPT";
        private const string fdViewLog = "ViewLog";
        private const string fdNumberGame = "NumberGame";
        private const string fdAgentWapUI = "WapUI";

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {

        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            return null;
        }

        public override void RenderContent(HtmlTextWriter writer)
        {
            Content_Template tpl = new Content_Template();
            string folderName = Request.QueryString["folder"].ToString();

            // Encrypt paras to Cache for Flash
            string parameter = string.Empty, path = string.Empty;
            parameter = folderName + "&" + CurrentUserSession.RoleId + "&" + CurrentUserSession.Language;
            parameter = Sunplus.Agent.Utilities.Encryption.CfsEnCrypt(parameter);
            tpl.source = Sunplus.Agent.Model.BL.Definitions.AppSettings.BaseUrl + "_HowToUse/Handlers/Flash.ashx?params=" + parameter;                        

            tpl.width = Request.QueryString["fw"];
            tpl.height = Request.QueryString["fh"];

            if (folderName == fdCasinoGuide) tpl.title = Resources.HowToUse.HowToEnableCasino;
            else if (folderName == fdAccInfo) tpl.title = Resources.HowToUse.AccInfoFunction;
            else if (folderName == fdViewLog) tpl.title = Resources.HowToUse.ViewLogFunction;
            else if (folderName == fdNumberGame) tpl.title = Resources.HowToUse.NumberGame;
            else if (folderName == fdAutoPT)
            {
                if (CurrentUserSession.IsSuper) tpl.title = Resources.HowToUse.MasterAutoPT;
                else tpl.title = Resources.HowToUse.AutoPT;
            }
            else if (folderName == fdAgentWapUI) tpl.title = Resources.HowToUse.AgentWapUserGuide;

            writer.Write(tpl);
        }
    }
}
