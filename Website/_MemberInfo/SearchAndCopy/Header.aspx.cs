/*
 * Created 20120117@Jeff - Search And Copy
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.UI._MemberInfo.SearchAndCopy
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Sunplus.Agent.Model.Entities;

    public partial class _Header_Page : SearchAndCopy_Page
    {
        #region Variable

        #endregion Variable

        #region Override

        public override void RenderContent(HtmlTextWriter writer)
        {
            Header_Template tpl = new Header_Template();
            RegisterJavascriptObject("roleId", UserSession.Current.RoleId);

            switch (UserSession.Current.RoleId)
            {
                case 2:
                    tpl.title = Resources.Lang.lblAddNewMember;
                    break;
                case 3:
                    tpl.title = Resources.Lang.lblAddNewAgent;
                    break;
                case 4:
                    tpl.title = Resources.Lang.lblAddNewMaster;
                    break;
            }

            tpl.roleId = UserSession.Current.RoleId.ToString();
            writer.Write(tpl);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            return null;
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        #endregion Override

        #region Utility

        #endregion Utility
    }
}