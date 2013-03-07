/*
 * Created 20121015@Marc - Render Multi-edit Double Commission Confirmation Dialog
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.UI._AccountInfo
{
    using System;
    using System.Web;
    using System.Web.UI;
    using Sunplus.Agent.UI._AccountInfo;
    using Fanex.Agent.UI._AccountInfo;

    /// <summary>
    /// Dialog for confirming on editing double commission of multiple users
    /// </summary>
    public partial class ComfirmMultiEditDoubleComm_Page : Fanex.Age.UI.TplPage
    {
        /// <summary>
        /// Render template
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderContent(HtmlTextWriter writer)
        {
            var template = new ConfirmMultiEditDoubleComm_Template();
            if (CurrentUserSession.IsAgent)
            {
                template.dcommMultiEditPopupAllowOption = Resources.Lang.dcommMultiEditPopupONOption;
                template.dcommMultiEditPopupDisallowOption = Resources.Lang.dcommMultiEditPopupOFFOption;
            }
            else
            {
                template.dcommMultiEditPopupAllowOption = Resources.Lang.dcommMultiEditPopupAllowOption;
                template.dcommMultiEditPopupDisallowOption = Resources.Lang.dcommMultiEditPopupDisallowOption;
            }
            writer.Write(template);
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
            return null;
        }
    }
}