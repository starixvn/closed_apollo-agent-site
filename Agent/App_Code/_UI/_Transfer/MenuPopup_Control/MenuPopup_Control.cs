// --------------------------------------------------------------------------
// <copyright file="InfoMember.aspx.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jeff.Nguyen" date="2012.05.31">Improve performance, check StypeCop</change>
// </history>

namespace Sunplus.Agent.UI._Transfer.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using MenuPopup_Control;
    using TplCore.Element;

    public class MenuPopup : IComponent 
    {
        private MenuPopup_Control_Template tpl = new MenuPopup_Control_Template();

        public static implicit operator string(MenuPopup value)
        {
            return value.ToString();
        }

        public string RenderLink(object[] texts, object[] ids, object[] isDisable, string popupId)
        {
            this.tpl.divPopupId = popupId;
            this.tpl.checkbox = null;
            this.tpl.link.Clear();
            MenuPopup_Control_link_Block link = new MenuPopup_Control_link_Block();
            for (int i = 0; i < ids.Length; i++)
            {
                link.id = ids == null ? null : ids[i] as string;
                link.isDisable = isDisable[i].ToString();
                link.text = texts[i] as string;
                this.tpl.link.Append(link);
            }

            return this.tpl;
        }

        public string RenderCheckbox(object[] texts, object[] ids, object[] isDisableChk, string popupId)
        {
            this.tpl.divPopupId = popupId;
            this.tpl.link = null;
            MenuPopup_Control_checkbox_Block checkbox = new MenuPopup_Control_checkbox_Block();
            this.tpl.checkbox.Clear();
            for (int i = 0; i < ids.Length; i++)
            {
                checkbox.text = texts[i] as string;
                checkbox.chkId = ids[i] as string;
                checkbox.isdisableChk = isDisableChk[i].ToString();
                this.tpl.checkbox.Append(checkbox);
            }

            return this.tpl;
        }

        public void HideLinkedResources()
        {
            this.tpl.ExternalResources = null;
        }

        public override string ToString()
        {
            return this.tpl.ToString();
        }
    }
}