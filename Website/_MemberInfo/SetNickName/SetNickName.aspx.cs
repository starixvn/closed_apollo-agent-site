// --------------------------------------------------------------------------
// <copyright file="SetNickName.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2011.08.30">Set Nickname</change>
//   <change who="Kaden.Trinh" date="2012.05.30">Refactor code and check StyleCop</change>
// </history>

namespace Sunplus.Agent.UI._MemberInfo.SetNickName
{
    using System.Web.UI;
    using Sunplus.Agent.Model.Entities;

    public partial class SetNickName_Page : Fanex.Age.UI.TplPage
    {
        public override void RenderContent(HtmlTextWriter writer)
        {
            var tpl = new SetNickName_Template();
            if (UserSession.Current.NickName != null && UserSession.Current.NickName != string.Empty)
            {
                tpl.NickName = UserSession.Current.NickName;
                tpl.NickNameReadOnly = "readonly";
                tpl.msgValidNickName = null;
                if (UserSession.Current.IsNicknameLoginOnly)
                {
                    tpl.IsCheckedNickName = "checked";
                    tpl.IsCheckedUserNameNickName = null;
                }
                else
                {
                    tpl.IsCheckedNickName = null;
                    tpl.IsCheckedUserNameNickName = "checked";
                }
            }
            else
            {
                tpl.NickName = string.Empty;
                tpl.NickNameReadOnly = string.Empty;
                tpl.msgValidNickName = Resources.Lang.msgValidNickName;
                tpl.IsCheckedNickName = null;
                tpl.IsCheckedUserNameNickName = "checked";
            }

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
            return null;
        }
    }
}