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
    using Fanex.Age.UI;
    using Fanex.Age.UI.Components;
    using Sunplus.Agent.Model;
    using Sunplus.Agent.Model.BL.AccountInfo;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;
    using Sunplus.Agent.Utilities;

    public partial class SearchUser_Page : SearchAndCopy_Page
    {
        #region Variable

        private SearchUser_Template tpl = new SearchUser_Template();

        #endregion Variable

        #region Override

        public override void RenderContent(HtmlTextWriter writer)
        {
            RegisterJavascriptObject("userNameDefault", Resources.Lang.enterusername);
            RegisterJavascriptObject("alertUserName", Resources.Lang.alertUserName);
            RegisterJavascriptObject("custid", UserSession.Current.CustId);
            RegisterJavascriptObject("roleId", UserSession.Current.RoleId);
            RegisterJavascriptObject("userName", userName);

            this.tpl.review_function = "SearchAndCopy.GetSetting(" + UserSession.Current.RoleId + ")";
            this.tpl.next_function = "SearchAndCopy.GetInfo(" + UserSession.Current.RoleId + ")";
            RenderHeaderStep(out this.tpl.Header.header, 1);

            if (UserSession.Current.IsLiveCasinoSync)
            {
                if (UserSession.Current.IsSuper)
                {
                    this.tpl.infoClickPreview = Resources.Lang.InfoClickPreview + " " + Resources.Lang.infoSearchAndCopyMaster;
                }
                else if (UserSession.Current.IsMaster)
                {
                    this.tpl.infoClickPreview = Resources.Lang.InfoClickPreview + " " + Resources.Lang.infoSearchAndCopyAgent;
                }
                else
                {
                    this.tpl.infoClickPreview = Resources.Lang.InfoClickPreview + " " + Resources.Lang.infoSearchAndCopyMember;
                }

                this.tpl.cssInfoClickPreview = "infoClickPreview";
            }
            else
            {
                this.tpl.infoClickPreview = Resources.Lang.InfoClickPreview;
                this.tpl.cssInfoClickPreview = "infoClickPreviewNoLC";
            }

            writer.Write(this.tpl);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            return null;
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            userName = GetStringRequest("userName", userName);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        #endregion Override
    }
}