using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TplCore.Element;

/// <summary>
/// Summary description for MenuPopup_Control
/// </summary>
/// 
namespace Sunplus.Agent.UI._MemberInfo.CustomerList.Components
{
    using MenuPopup_Control;
    using Sunplus.Agent.Model.Entities;
    public class MenuPopup:IComponent 
    {
        MenuPopup_Control_Template tpl = new MenuPopup_Control_Template();
               

        public string RenderLink(object[] texts, object[] ids, object[] isDisable, string popupId)
        {
            tpl.divPopupId = popupId;
            tpl.checkbox = null;
            tpl.link.Clear();
            MenuPopup_Control_link_Block link = new MenuPopup_Control_link_Block();
            for (int i = 0; i < ids.Length; i++)
            {
                link.id = ids == null ? null : ids[i] as string;
                link.isDisable = isDisable[i].ToString();
                link.text = texts[i] as string;
                tpl.link.Append(link);
            }
            return tpl;
        }

        public string RenderCheckbox(object[] texts, object[] ids, object[] isDisableChk, string popupId)
        {
            tpl.divPopupId = popupId;
            tpl.link = null;
            MenuPopup_Control_checkbox_Block checkbox = new MenuPopup_Control_checkbox_Block();
            tpl.checkbox.Clear();
            for (int i = 0; i < ids.Length; i++)
            {
                checkbox.text = texts[i] as string;
                checkbox.chkId = ids[i] as string;
                checkbox.isdisableChk = isDisableChk[i].ToString();
                checkbox.disabled = UserSession.Current.IsSubAcc && !UserSession.Current.SubPageAccess.UpdateAccount ? "disabled" : "";
                tpl.checkbox.Append(checkbox);
            }
            return tpl;
        }

        public static implicit operator string(MenuPopup value)
        {
            return value.ToString();
        }
        public void HideLinkedResources()
        {
            tpl.ExternalResources = null;
        }
        public override string ToString()
        {
            return tpl.ToString();
        }

    }
}
