namespace Sunplus.Agent.UI._MemberInfo.SearchAndCopy
{
    using System;
    using System.Collections.Generic;
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

    public partial class _MasterPage : SearchAndCopy_Page
    {
        public override void RenderContent(HtmlTextWriter writer)
        {
            MasterPage_Template tplMain = new MasterPage_Template();

            tplMain.content_url = "../../_MemberInfo/SearchAndCopy/SearchUser.aspx";
            tplMain.header_url = "../../_MemberInfo/SearchAndCopy/Header.aspx";
            writer.Write(tplMain);
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
    }
}